using Microsoft.VisualStudio.CodeCoverage;
using System.Runtime.Loader;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IpLibrary
{
    public class NetAddress
    {
        public int Prefix { get; }
        public uint Ip { get; }
        public NetAddress(string addressIp)
        {
            Check(addressIp);
            this.Ip = ExtractIp(addressIp);
            this.Prefix = ExtractPrefix(addressIp);
        }

        public override string ToString()
        {
            string ip = IpConveter.Uint32ToString(this.Ip);
            return $"{ip}/{this.Prefix}";
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
                throw (new ArgumentException($"Недопустимое количество подсетей.Максимальное число возможных подсетей: {maxCountSubnets}"));
            if (Math.Log2(count) % 1 != 0)
                throw (new ArgumentException($"Недопустимое количество подсетей. Количество подсетей должно быть равно степени двойки(2,4,8,16 и т.п.)"));

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
        
        /// <summary>
        /// Проверка принадлежности сетей. Т.е. является ли сеть adr1 подсетью adr2, и наоборот.
        /// </summary>
        /// <param name="adr1"></param>
        /// <param name="adr2"></param>
        /// <returns></returns>
        public static bool IsAffiliation(NetAddress adr1,NetAddress adr2)
        {

            int minPrefix = Math.Min(adr1.Prefix, adr2.Prefix);
            uint totalMask = minPrefix==0?
                0 : 0b11111111_11111111_11111111_11111111u << (32 - minPrefix);
            return (adr1.Ip & totalMask) == (adr2.Ip & totalMask);
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

        private void Check(string addressIp)
        {
            var reg = new Regex(@"^(?<ip>\d+\.\d+\.\d+\.\d+)\/(?<prefix>\d+)$");
            if (!reg.IsMatch(addressIp))
                throw new ArgumentException("Некорретный формат");

            string ip = reg.Match(addressIp).Groups["ip"].Value;
            int prefix = int.Parse(reg.Match(addressIp).Groups["prefix"].Value);
            if (!ip.Split('.').Select(int.Parse).All(x=> x>=0 && x<=255))
                throw new ArgumentException("Числа в октетах выходят за даипозон 0..255");
            if(prefix<0 || prefix >32 )
                throw new ArgumentException("Префикс выходит за диапозон 0..32");
        }


    }
}