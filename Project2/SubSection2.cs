using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Project2
{
    public enum LINK_TYPE
    {
        P2C_TYPE,
        P2P_TYPE
    }
    public class IPRange
    {
        public IPAddress start;
        public IPAddress end;
        public IPAddress subnet;
        public IPRange(string addr, int prefix)
        {
            start = IPAddress.Parse(addr);
            uint possible_ips = Convert.ToUInt32(Math.Pow(2, 32 - prefix)) - 1;
            byte[] byteAddress = start.GetAddressBytes().Reverse().ToArray();
            uint ipAsUint = BitConverter.ToUInt32(byteAddress, 0);
            var nextAddress = BitConverter.GetBytes(ipAsUint + possible_ips).Reverse().ToArray();
            end = new IPAddress(nextAddress);
        }

    }
    public class AS
    {
        public int degree;
        public string _id;
        public List<Link> links;
        public List<IPRange> ranges;
        public IPAddress ip_addr;
        public AS(string id)
        {
            _id = id;
            degree = 0;
            links = new List<Link>();
            ranges = new List<IPRange>();
        }


        public void AddLink(Link l, bool isProvider = true)
        {
            if (l.type == LINK_TYPE.P2C_TYPE && isProvider)
            {
                links.Add(l);
            }
            degree++;
        }

        public void AddRange(string r, int prefix)
        {
            ranges.Add(new IPRange(r, prefix));
        }


    }

    public class Link
    {
        public string origin;
        public string destination;
        public LINK_TYPE type;

        public Link(string[] tmp)
        {
            origin = tmp[0];
            destination = tmp[1];
            type = (tmp[2] == "-1") ? LINK_TYPE.P2C_TYPE : LINK_TYPE.P2P_TYPE;
        }
    }

    public class SubSection2 :  Dictionary<string, AS>
    {
        public SubSection2(string ASfilename, string IPFilename) : base()
        {
            configureASSes(ASfilename);
            configureIPs(IPFilename);
           
        }
        public void configureASSes(string filename)
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
                    Add(parsed_line[1], new AS(parsed_line[1]));
                }
				this[parsed_line[0]].AddLink(new Link(parsed_line));
                this[parsed_line[1]].AddLink(new Link(parsed_line), false);

			}
        }
        public void configureIPs(string filename)
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

        public void printAll()
        {
            foreach(AS a in this.Values)
            {
                Console.WriteLine("AS id: {0}, AS degree: {1}", a._id, a.degree);
                foreach(Link link in a.links)
                    Console.WriteLine("\t\tOrigin: {0}, Destination: {1}, Type: {2}", link.origin, link.destination, link.type.ToString());
                foreach (IPRange r in a.ranges)
                {
                    Console.WriteLine("\t\tStart Address: {0}, EndAddress: {1}", r.start, r.end);
                }
            }


        }

        public void PrintBins()
        {
            int FirstBin = 0;
            int SecondBin = 0;
            int ThirdBin = 0;
            int FourthBin = 0;
            int FifthBin = 0;
            int SixthBin = 0;
            foreach(AS a in Values)
            {
                if (a.degree == 1)
                    FirstBin++;
                else if (a.degree > 1 && a.degree <= 5)
                    SecondBin++;
                else if (a.degree > 5 && a.degree <= 100)
                    ThirdBin++;
                else if (a.degree > 100 && a.degree <= 200)
                    FourthBin++;
                else if (a.degree > 200 && a.degree <= 1000)
                    FifthBin++;
                else
                {
                    //Console.WriteLine(a._id);
                    SixthBin++;
                }
            }

            Console.WriteLine("FirstBin(1): {0}", FirstBin);
            Console.WriteLine("SecondBin(2-5): {0}", SecondBin);
            Console.WriteLine("ThirdBin(6-100): {0}", ThirdBin);
            Console.WriteLine("FourthBin(101-200): {0}", FourthBin);
            Console.WriteLine("FifthBin(201-1000): {0}", FifthBin);
            Console.WriteLine("SixthBin(>1000): {0}", SixthBin);
        }
    }
}
