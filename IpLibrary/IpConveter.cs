using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IpLibrary
{
    static class IpConveter
    {
        static public string Uint32ToString(uint ip) =>
            string.Join('.', 
                new IPAddress(ip).ToString().Split('.').Reverse());

        static public uint StringToUint32(string ip) =>
            (uint)ip.Split('.').
                Select(int.Parse).
                Aggregate((x, y) => x << 8 | y);
            //(uint)IPAddress.NetworkToHostOrder(
            // (int)IPAddress.Parse(ip).Address);
    }
}
