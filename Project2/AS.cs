using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    public class AS
    {
        public int degree;
        public string id;
        public int customerConeSize;
        public int customerConePrefixNum;
        public float percentIpSpace;
        public Dictionary<string, Link> p2cLinks;
        public Dictionary<string, Link> p2pLinks;
        public List<AS> customers;
        public Dictionary<string, AS> providers;
        //public List<AS> p2p;
        //public List<Link> links;
        public List<IPRange> ranges;
        public IPAddress ip_addr;
        public uint totalIpReach;

        public AS(string pId)
        {
            id = pId;
            degree = 0;
            customerConeSize = 0;
            customerConePrefixNum = 0;
            percentIpSpace = 0.0f;
            //links = new List<Link>();
            p2cLinks = new Dictionary<string, Link>();
            p2pLinks = new Dictionary<string, Link>();
            customers = new List<AS>();
            providers = new Dictionary<string, AS>();
            //p2p = new List<AS>();
            ranges = new List<IPRange>();
            totalIpReach = 0;
        }

        public void AddLink(Link l, bool isProvider = true, AS linked = null)
        {
            if (l.type == LINK_TYPE.P2C_TYPE)
            {
                if (isProvider)
                {
                    p2cLinks.Add(l.destination, l);

                    if (linked != null)
                    {
                        customers.Add(linked);
                    }
                }
                else
                {
                    if (linked != null)
                    {
                        providers.Add(linked.id, linked);
                    }
                }
            }

            if (l.type == LINK_TYPE.P2P_TYPE)
            {
                if (!p2pLinks.ContainsKey(l.destination))
                    p2pLinks.Add(l.destination, l);
            }
            degree++;
        }

        public int DetermineCone()
        {
            int count = 0;
            //int prefixNum = 0;

            foreach (AS autoSys in customers)
            {
                if (!EvalSet.Instance.Contains(autoSys.id))
                {
                    EvalSet.Instance.Add(autoSys.id);
                    count += 1 + autoSys.DetermineCone();
                    EvalSet.Instance.totalIpSpace += autoSys.numIpAddresses();
                    EvalSet.Instance.totalPrefix += autoSys.ranges.Count;
                    //customerConePrefixNum += autoSys.ranges.Count;
                    //EvalSet.Instance.total++;
                }
            }

            totalIpReach = EvalSet.Instance.totalIpSpace;
            customerConeSize = count;
            customerConePrefixNum = EvalSet.Instance.totalPrefix;
            return count;
        }

        public void printRecursiveCones(StreamWriter export, string tabs)
        {
            //tabs += ",";
            export.WriteLine(tabs + id);
            foreach(AS cust in customers)
            {
                if (!EvalSet.Instance.Contains(cust.id))
                {
                    EvalSet.Instance.Add(cust.id);
                    cust.printRecursiveCones(export, tabs);
                }
            }

            export.Flush();
        }

        public void AddRange(string r, int prefix)
        {
            ranges.Add(new IPRange(r, prefix));
        }

        public uint numIpAddresses()
        {
            uint count = 0;

            foreach (IPRange ipr in ranges)
            {
                count += ipr.totalIpSpace;
            }

            //totalIpReach = count;
            return count;
        }

        public bool IsConnected(string id)
        {
            return p2cLinks.ContainsKey(id) || p2pLinks.ContainsKey(id) || providers.ContainsKey(id);
        }
    }
}
