using IpLibrary;
using Xunit;

namespace IpLibrary.Tests
{
    public class NetAddressTests
    {
        [Fact]
        public void ToSubNets_FourSubnets()
        {
            //Arrange
            var expected = new List<string>()
            {
                "192.168.0.0/26",
                "192.168.0.64/26",
                "192.168.0.128/26",
                "192.168.0.192/26"
            };
            //Act
            var actual = new NetAddress("192.168.0.0/24").ToSubnets(4);
            //Assert
            Assert.Equal(expected,actual);
        }

        [Fact]
        public void IsAffilation_True()
        {
            var adr1 = new NetAddress("192.168.0.0/24");
            var adr2 = new NetAddress("192.168.0.0/24");

            bool isAff = NetAddress.IsAffiliation(adr1, adr2);

            Assert.True(isAff);

        }

        [Fact]
        public void IsAffilation_False()
        {
            var adr1 = new NetAddress("192.168.0.0/18");
            var adr2 = new NetAddress("192.168.128.0/17");

            bool isAff = NetAddress.IsAffiliation(adr1, adr2);

            Assert.False(isAff);
        }

        [Theory]
        [InlineData("192.256.0.5/32")]
        [InlineData("aa.aa.aa.ssds5/32")]
        [InlineData("192.256.0.5/20")]
        [InlineData("192.168.0.5/b")]
        public void Check_ShouldFail(string address)
        {
            Assert.Throws<ArgumentException>(()=>new NetAddress(address));
        }

        [Theory]
        [InlineData("192.255.0.5/32", 3237937157, 32)]
        [InlineData("255.128.0.0/15", 4286578688, 15)]
        [InlineData("0.0.0.0/0", 0u, 0)]
        public void Check_ShouldWork(string address,uint ipNum,int prefix)
        {
            var net = new NetAddress(address);

            Assert.Equal(ipNum, net.Ip);
            Assert.Equal(prefix, net.Prefix);
        }
    }
}