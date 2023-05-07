using Microsoft.VisualStudio.CodeCoverage;
using System.Runtime.Loader;
using System.Text.RegularExpressions;

namespace IpLibrary
{
    public class NetAddress
    {
        public int Prefix { get; }
        public uint Ip { get; }
        public NetAddress(string addressIp)
        {
            CheckAddressIp(addressIp);
            this.Ip = ExtractIp(addressIp);
            this.Prefix = ExtractPrefix(addressIp);
        }

        public override string ToString()
        {
            return $"{IpConveter.Uint32ToString(this.Ip)}/{this.Prefix}";
        }

        /// <summary>
        /// Делит сеть на подсети.
        /// </summary>
        /// <param name="count">Количество подсетей,которое должно быть равно степени двойки</param>
        /// <returns></returns>
        public List<string> ToSubnets(int count)
        { 
            int maxCountSubnets = 1<<(32 - this.Prefix);
            if (count > maxCountSubnets)
                throw (new Exception($"Недопустимое количество подсетей.Максимальное число возможных подсетей: {maxCountSubnets}"));
            if (Math.Log2(count) % 1 != 0)
                throw (new Exception($"Недопустимое количество подсетей. Количество подсетей должно быть равно степени двойки(2,4,8,16 и т.п.)"));

            var subnets = new List<string>();
            uint subIp = this.Ip;
            int subPrefix = (int)Math.Log2(count) + this.Prefix;
            for(int i = 0; i < count;i++)
            {
                string subnet = $"{IpConveter.Uint32ToString(subIp)}/{subPrefix}";
                subnets.Add(subnet);
                uint increment = 1u << (32 - subPrefix);
                subIp += increment;
            }
            return subnets;
        }
         
        static public bool IsAffiliation(NetAddress adr1,NetAddress adr2)
        {
            uint totalMask = 1u<<Math.Min(adr1.Prefix, adr2.Prefix);
            return (adr1.Ip| totalMask) == (adr2.Ip| totalMask);
        }

        private uint ExtractIp(string addressIp)
        {
            int indPrefix = addressIp.IndexOf('/');
            string ip = addressIp.Substring(0, indPrefix);
            return IpConveter.StringToUint32(ip);

        }

        private int ExtractPrefix(string addressIp)
        {
            int indPrefix = addressIp.IndexOf('/');
            //string prefix = addressIp[(indPrefix + 1)..^0];
            string prefix = addressIp.Substring(indPrefix + 1, addressIp.Length - indPrefix-1) ;
            return int.Parse(prefix);
        }

        private void CheckAddressIp(string addressIp)
        {
            var reg = new Regex(@"^(?<ip>\d+\.\d+\.\d+\.\d+)\/(?<prefix>\d+)$");
            if (!reg.IsMatch(addressIp))
                throw new Exception("Неправильно введённый формат адреса сети");

            string ip = reg.Match(addressIp).Groups["ip"].Value;
            int prefix = int.Parse(reg.Match(addressIp).Groups["prefix"].Value);
            if (!ip.Split('.').Select(int.Parse).All(x=> x>=0 || x<=255))
                throw new Exception("Числа в октетах выходят за даипозон 0..255");
            if(prefix<0 || prefix >32 )
                throw new Exception("Префикс выходит за диапозон 0..32");
        }


    }
}