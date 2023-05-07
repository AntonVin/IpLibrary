using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IpLibrary
{
    public static class IpConveter
    {
        static public string Uint32ToString(uint ip) =>
            string.Join('.', 
                new IPAddress(ip).ToString().Split('.').Reverse());

        static public uint StringToUint32(string ip)
        {
            var reg = new Regex(@"^\d+\.\d+\.\d+\.\d+$");
            if (!reg.IsMatch(ip))
                throw new ArgumentException("Некорректный формат");

            if (!ip.Split('.').Select(int.Parse).All(x => x >= 0 && x <= 255))
                throw new ArgumentException("Числа в октетах выходят за даипозон 0..255");

            return ip.Split('.').
                Select(uint.Parse).
                Aggregate((x, y) => x << 8 | y);
        }
    }
}
