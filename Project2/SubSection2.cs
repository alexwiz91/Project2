﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Project2
{
    public class SubSection2 :  Dictionary<string, AS>
    {
        private StreamWriter export;

        public SubSection2(string ASfilename, string IPFilename) : base()
        {
            ConfigureASSes(ASfilename);
            ConfigureIPs(IPFilename);
           
        }

        public void ConfigureASSes(string filename)
        {
			string[] parsed_line = new string[4];

			foreach (string line in File.ReadAllLines(filename))
			{
				if (line[0] == '#')
					continue;
				parsed_line = line.Split('|');

				if (!this.ContainsKey(parsed_line[0]))
				{
					Add(parsed_line[0], new AS(parsed_line[0]));
				}
                if (!this.ContainsKey(parsed_line[1]))
                {
                   // Console.WriteLine("{0}", parsed_line[1]);
                    Add(parsed_line[1], new AS(parsed_line[1]));
                }
				this[parsed_line[0]].AddLink(new Link(parsed_line));
                this[parsed_line[1]].AddLink(new Link(parsed_line, false), false);

			}
        }

        public void ConfigureIPs(string filename)
        {
            string[] parsed_line = new string[3];
            foreach(string line in File.ReadAllLines(filename))
            {
                string tmp = line;
                if (tmp.Contains(','))
                    tmp = line.Replace(',', '_');   
                
                parsed_line = tmp.Split('\t');   

                foreach (string str in parsed_line[2].Split('_'))
                {
                    if (!ContainsKey(str))
                    {
                        Add(str, new AS(str));
                    }
                    this[str].AddRange(parsed_line[0], Convert.ToInt32(parsed_line[1]));
                }
            }
        }

        public void PrintAll()
        {
            foreach(AS a in this.Values)
            {
                Console.WriteLine("AS id: {0}, AS degree: {1}", a.id, a.degree);
                //foreach (Link link in a.links)
                //{   
                //    Console.WriteLine("\t\tOrigin: {0}, Destination: {1}, Type: {2}", link.origin, link.destination, link.type.ToString());
                //}
                foreach (IPRange r in a.ranges)
                {
                    Console.WriteLine("\t\tStart Address: {0}, EndAddress: {1}", r.start, r.end);
                }
            }
        }
        public void ExportGraph4Data()
        {
            int totalEnterpriseAS = 0;
            int totalContentAS = 0;
            int totalTransitAS = 0;
            Console.WriteLine("Graph 4 data:");

            export = new StreamWriter("graph4.csv");

            foreach(AS a in Values)
            {
                if (a.degree <= 2 && a.p2pLinks.Count == 0 && a.p2cLinks.Count == 0)
                {
                    //Console.WriteLine("Enterprise AS found: {0}", a._id);
                    totalEnterpriseAS++;
                }
                else if(a.p2cLinks.Count == 0 && a.p2pLinks.Count >= 1)
                {
                    //Console.WriteLine("Content AS found: {0}", a._id);
                    totalContentAS++;
                }
                else if(a.p2cLinks.Count >= 1)
                {
					//Console.WriteLine("Transit AS found: {0}", a._id);
                    totalTransitAS++;
                }
            }

            Console.WriteLine("Total Enterprise: {0}", totalEnterpriseAS);
            Console.WriteLine("Total Content: {0}", totalContentAS);
            Console.WriteLine("Total Transit: {0}", totalTransitAS);

            export.WriteLine("Enterprise," + totalEnterpriseAS);
            export.WriteLine("Content," + totalContentAS);
            export.WriteLine("Transit," + totalTransitAS);

            export.Close();
        }

        public void ExportGraph3Data()
        {
            SortedDictionary<IPAddress, int> ipBins = new SortedDictionary<IPAddress, int>();
            StreamWriter export = new StreamWriter("graph3.csv");
        
            foreach (AS autoSys in Values)
            {
                foreach (IPRange ipr in autoSys.ranges)
                {
                    //Alex: I don't understand if this is supposed to be "total ip space" as in total number of possible ips
                    //or is it the "total ip space" range, like 192.168.1.0-192.168.1.255. The only graph I can create that
                    //makes sense is using the first number in each ip as the bin.

                    // Just pooping three different data points. We can put together some sort of histogram from them.
                    export.WriteLine(ipr.start + "," + ipr.start.ToString().Split('.')[0] + "," + ipr.GetHashCode());
                }
            }

            export.Close();
        }

        public void ExportGraph2Data()
        {
            int totalCount = 0;
            int firstBin = 0;
            int secondBin = 0;
            int thirdBin = 0;
            int fourthBin = 0;
            int fifthBin = 0;
            int sixthBin = 0;

            export = new StreamWriter("graph2.csv");

            foreach(AS a in Values)
            {
                if (a.degree == 1)
                    firstBin++;
                else if (a.degree > 1 && a.degree <= 5)
                    secondBin++;
                else if (a.degree > 5 && a.degree <= 100)
                    thirdBin++;
                else if (a.degree > 100 && a.degree <= 200)
                    fourthBin++;
                else if (a.degree > 200 && a.degree <= 1000)
                    fifthBin++;
                else
                {
                    //Console.WriteLine(a._id);
                    sixthBin++;
                }
                totalCount++;
            }
        

            Console.WriteLine("Printing Graph2 Data:");
            Console.WriteLine("FirstBin(1): {0}", firstBin);
            Console.WriteLine("SecondBin(2-5): {0}", secondBin);
            Console.WriteLine("ThirdBin(6-100): {0}", thirdBin);
            Console.WriteLine("FourthBin(101-200): {0}", fourthBin);
            Console.WriteLine("FifthBin(201-1000): {0}", fifthBin);
            Console.WriteLine("SixthBin(>1000): {0}", sixthBin);

            export.WriteLine("1," + firstBin);
            export.WriteLine("2-5," + secondBin);
            export.WriteLine("6-100," + thirdBin);
            export.WriteLine("101-200," + fourthBin);
            export.WriteLine("201-1000," + fifthBin);
            export.WriteLine(">1000," + sixthBin);

            export.Close();
        }

        public void ExportTable1Data()
        {
            List<AS> sortedList = Values.ToList().OrderBy(o=>o.degree).Reverse().ToList();
            List<AS> s = new List<AS>();

            SortedDictionary< int, List<AS> > cliques = new SortedDictionary< int, List<AS> >();

            bool addToSet = false;

            int i = 0;
            int j = 0;

            //foreach(AS autoSys in sortedList)
            for (i = 0; i < sortedList.Count; i++)
            {
                for (j = i; j < sortedList.Count; j++)
                {
                    addToSet = true;

                    foreach (AS fromSet in s)
                    {
                        if (!sortedList[j].IsConnected(fromSet.id))
                        {
                            addToSet = false;
                            break;
                        }
                    }

                    if (addToSet)
                    {
                        s.Add(sortedList[j]);
                    }
                    else
                    {
                        break;
                    }
                }

                if (!cliques.ContainsKey(s.Count))
                {
                    cliques.Add(s.Count, s);
                }

                s = new List<AS>();
            }

            export = new StreamWriter("table1.csv");

            export.WriteLine("T1 Size:," + cliques.Last().Key);
            export.WriteLine();

            int count = 0;
            foreach (AS autoSys in cliques.Last().Value)
            {
                //if (count >= 10)
                //    break;

                export.WriteLine(autoSys.id + "," + autoSys.degree);
                count++;
            }

            export.Close();
        }

        public Tuple<int, HashSet<AS>> TraverseCustomers_recursive(AS start)
        {
            int count = 0;
            HashSet<AS> h = new HashSet<AS>();
            h.Add(start);
            count += TraverseCustomers(start, ref h);
            return new Tuple<int, HashSet<AS>>(count, h);

          
        }
        public int TraverseCustomers(AS start, ref HashSet<AS> assesAlreadyAdded)
        {
            int count = 0;
            // Base case, if we don't have links to traverse then we wanna return out of this
            if (start.p2cLinks.Count == 0)
            {
                count = 0;
                Console.WriteLine("{0} has no more p2c links", start.id);
            }
            else
            {
                // Chec each link associated with the passed AS
                foreach (Link l in start.p2cLinks.Values)
                {
                    if (this.ContainsKey(l.destination))
                    {
                        // The assesAlreadyAdded structure avoids iterating through the same AS over and over
                        // Add() returns true when the addition to the set was successfull and false otherwise
                        if (assesAlreadyAdded.Add(this[l.destination]))
                        {
                            // Recursively iterate through ASes with id of link.destination
                            count += 1 + TraverseCustomers(this[l.destination], ref assesAlreadyAdded);
                        }
                    }
                }
            }
            return count;

        }

        public int p2cCount(AS start)
		{
            int count = start.p2cLinks.Count();
			// Base case, if we don't have links to traverse then we wanna return out of this
			
			// Chec each link associated with the passed AS
			foreach (Link l in start.p2cLinks.Values)
			{
				if (this.ContainsKey(l.destination))
				{
                    count += p2cCount(this[l.destination]);
				}
			}
		
			return count;

		}

        public void ExportTable2Data()
        {
            List<AS> sortedList = Values.ToList().OrderBy(o => o.degree).Reverse().ToList();
			List<AS> s = new List<AS>();

            SortedDictionary<int, HashSet<AS>> customerCones = new SortedDictionary<int, HashSet<AS>>();
            SortedDictionary<int, AS> customerCones2 = new SortedDictionary<int, AS>();
            List<int> removalList = new List<int>();
            foreach (AS a in sortedList)
            {
                Tuple<int, HashSet<AS>> customerCountAndAS = TraverseCustomers_recursive(a);
                //foreach(KeyValuePair<int, HashSet<AS>> kv in customerCones)
                //{
                //    if(kv.Value.Contains(customerCountAndAS.Item2.First()) && customerCountAndAS.Item1 > kv.Key)
                //    {
                //        removalList.Add(kv.Key);
                //    }
                //}
                if (!customerCones.ContainsKey(customerCountAndAS.Item1))
                {
                    customerCones.Add(customerCountAndAS.Item1, customerCountAndAS.Item2);
                }
            }
            foreach (AS ab in sortedList)
            {
                int count = p2cCount(ab);
                if (!customerCones2.ContainsKey(count))
                    customerCones2.Add(p2cCount(ab), ab);
            }


            foreach (int removalIndex in removalList)
                customerCones.Remove(removalIndex);

			export = new StreamWriter("table2.csv");
			export.WriteLine("customer2 p2c link count: {0}", customerCones2.Last().Key);
            Console.WriteLine("Customer Cone Size: {0}", customerCones.Keys.Reverse().FirstOrDefault());
            export.WriteLine("Customer Cone Size: {0}", customerCones.Keys.Reverse().FirstOrDefault());
			foreach (AS a in customerCones[customerCones.Keys.Reverse().FirstOrDefault()])
                export.WriteLine(",,{0}", a.id);
				//        for (int i = 0; i < 15; i++)
				//        {
				//// Nifty way to get the 15 highest customer cone networks
				//Console.WriteLine("Customer Cone Size: {0}", customerCones.Keys.Reverse().Skip(i).FirstOrDefault());
				//            export.WriteLine("Customer Cone Size: {0}\tCustomer Cone: ", customerCones.Keys.Reverse().Skip(i).FirstOrDefault());
				//            foreach(AS a in customerCones[customerCones.Keys.Reverse().Skip(i).FirstOrDefault()])
				//                export.WriteLine("\t\t\t\t\t\t{0}", a.id);
				//export.WriteLine();
				//}


				export.Close();
            
        }
    }
}
