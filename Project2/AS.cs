using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    public class AS
    {
        public int degree;
        public int num_peers;
        public string id;
        public Dictionary<string, Link> links;
        //public List<Link> links;
        public List<IPRange> ranges;
        public IPAddress ip_addr;

        public AS(string pId)
        {
            id = pId;
            degree = 0;
            num_peers = 0;
            //links = new List<Link>();
            links = new Dictionary<string, Link>();
            ranges = new List<IPRange>();
        }


        public void AddLink(Link l, bool isProvider = true)
        {
            if (l.type == LINK_TYPE.P2C_TYPE && isProvider)
            {
                links.Add(l.destination, l);
            }
            if (l.type == LINK_TYPE.P2P_TYPE)
            {
                num_peers++;
            }
            degree++;
        }

        public void AddRange(string r, int prefix)
        {
            ranges.Add(new IPRange(r, prefix));
        }

        public bool IsConnected(string id)
        {
            //foreach (Link l in links)
            //{
            //    if (l.destination == id)
            //    {
            //        return true;
            //    }
            //}

            return links.ContainsKey(id);
        }
    }
}
