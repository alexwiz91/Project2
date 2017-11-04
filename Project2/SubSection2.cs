using System;
using System.Collections.Generic;
using System.IO;
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
            double possible_ips = Math.Pow(2, 32 - prefix);



        }

        public IPAddress constructRange()
        {
            return null;
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
            links = new List<Link>();
        }


        public void AddLink(Link l)
        {
            if (l.type == LINK_TYPE.P2C_TYPE)
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
				this[parsed_line[0]].AddLink(new Link(parsed_line));

			}
        }
        public void configureIPs(string filename)
        {
            string[] parsed_line = new string[3];
            foreach(string line in File.ReadAllLines(filename))
            {
                parsed_line = line.Split('\t');
                this[parsed_line[2]].AddRange(parsed_line[0], Convert.ToInt32(parsed_line[1]));
            }
        }

        public void printAll()
        {
            foreach(AS a in this.Values)
            {
                Console.WriteLine("AS id: {0}, AS degree: {1}", a._id, a.degree);
                foreach(Link link in a.links)
                    Console.WriteLine("\t\tOrigin: {0}, Destination: {1}, Type: {2}", link.origin, link.destination, link.type.ToString());
            }


        }
    }
}
