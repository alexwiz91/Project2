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
        public string id;
        public Dictionary<string, Link> p2cLinks;
        public Dictionary<string, Link> p2pLinks;
        //public List<Link> links;
        public List<IPRange> ranges;
        public IPAddress ip_addr;

        public AS(string pId)
        {
            id = pId;
            degree = 0;
            //links = new List<Link>();
            p2cLinks = new Dictionary<string, Link>();
            p2pLinks = new Dictionary<string, Link>();
            ranges = new List<IPRange>();
        }

        public void AddLink(Link l, bool isProvider = true)
        {
            if (l.type == LINK_TYPE.P2C_TYPE && isProvider)
            {
                p2cLinks.Add(l.destination, l);
            }
            if (l.type == LINK_TYPE.P2P_TYPE)
            {
                if (!p2pLinks.ContainsKey(l.destination))
                    p2pLinks.Add(l.destination, l);
            }
            degree++;
        }

        public void AddRange(string r, int prefix)
        {
            ranges.Add(new IPRange(r, prefix));
        }

        public bool IsConnected(string id)
        {
            return p2cLinks.ContainsKey(id) || p2pLinks.ContainsKey(id);
        }
    }
}
