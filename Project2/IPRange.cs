using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    public class IPRange
    {
        public IPAddress start;
        public IPAddress end;
        public int prefix;
        public uint totalIpSpace;
        public IPRange(string addr, int pPrefix)
        {
            prefix = pPrefix;
            start = IPAddress.Parse(addr);
            totalIpSpace = Convert.ToUInt32(Math.Pow(2, 32 - prefix)) - 1;
            byte[] byteAddress = start.GetAddressBytes().Reverse().ToArray();
            uint ipAsUint = BitConverter.ToUInt32(byteAddress, 0);
            var nextAddress = BitConverter.GetBytes(ipAsUint + totalIpSpace).Reverse().ToArray();
            end = new IPAddress(nextAddress);
        }

    }
}
