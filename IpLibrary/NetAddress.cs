using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.Loader;
using System.Text.RegularExpressions;
using Xunit.Sdk;

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

        public List<string> ToSubNets(int count)
        { 
            int maxCountSubNets = 1<<(32 - this.Prefix);
            if (count > maxCountSubNets)
                throw (new Exception($"Недопустимое количество подсетей.Максимальное число возможных подсетей: {maxCountSubNets}"));
            if (Math.Log2(count) % 1 != 0)
                throw (new Exception($"Недопустимое количество подсетей. Количество подсетей должно быть равно степени двойки(2,4,8,16 и т.п.)"));

            var subNets = new List<string>();
            uint subIp = this.Ip;
            int subPrefix = (int)Math.Log2(count) + this.Prefix;
            for(int i = 0; i < count;i++)
            {
                string subNet = $"{IpConveter.Uint32ToString(subIp)}/{subPrefix}";
                subNets.Add(subNet);
                uint increment = 1u << (32 - subPrefix);
                subIp += increment;
            }
            return subNets;
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
            var reg = new Regex(@"^\d+\.\d+\.\d+\.\d+\/\d+$");
            if (!reg.IsMatch(addressIp))
                throw new Exception("Неправильно введённый формат адреса сети");

            int indPrefix = addressIp.IndexOf('/');
            string ip = addressIp.Substring(0, indPrefix);
            int prefix = int.Parse(addressIp[(indPrefix + 1)..^0]);
            if (!ip.Split('.').Select(int.Parse).All(x=> x>=0 || x<=255))
                throw new Exception("Числа в октетах выходят за даипозон 0..255");
            if(prefix<0 || prefix >32 )
                throw new Exception("Префикс выходит за диапозон 0..32");
        }

        public override string ToString()
        {
            return $"{IpConveter.Uint32ToString(this.Ip)}/{ this.Prefix}";
        }
    }
}