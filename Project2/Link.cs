using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    public enum LINK_TYPE
    {
        P2C_TYPE,
        P2P_TYPE
    }

    public class Link
    {
        public string origin;
        public string destination;
        public LINK_TYPE type;

        public Link(string[] tmp, bool isOrigin = true)
        {
            if (isOrigin)
            {
                origin = tmp[0];
                destination = tmp[1];
            }
            else
            {
                destination = tmp[0];
                origin = tmp[1];
            }

            type = (tmp[2] == "-1") ? LINK_TYPE.P2C_TYPE : LINK_TYPE.P2P_TYPE;
        }
    }
}
