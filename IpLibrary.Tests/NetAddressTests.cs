using IpLibrary;
using Xunit;

namespace IpLibrary.Tests
{
    public class NetAddressTests
    {

        //[Fact]
        //public void ExtractIp_TypicalAddress()
        //{
        //    //Arrange
        //    var expected = 3232235521;
        //    //Act
        //    var actual = new NetAddress("192.168.0.1/24").ExtractIp("192.168.0.0/24");
        //    //Assert
        //    Assert.Equal(expected, actual);
        //}
        //[Fact]
        //public void ExtractPrefix()
        //{
        //    var expected = 24;

        //    var act = new NetAddress("192.168.0.1/24").ExtractPrefix("192.168.0.0/24");

        //    Assert.Equal(expected, act);
        //}

        [Fact]
        public void ToSubNets_TwoSubnets()
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
            var actual = new NetAddress("192.168.0.0/24").ToSubNets(4);
            //Assert
            Assert.Equal(expected,actual);
        }
    }
}