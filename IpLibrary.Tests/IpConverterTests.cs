using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IpLibrary.Tests
{
    public class IpConverterTests
    {
        [Theory]
        [InlineData("192.168.0.5", 3232235525u)]
        [InlineData("0.0.0.0",0u)]
        public void StringToUnit32_ShouldWork(string ip,uint expected)
        {
            uint actual = IpConveter.StringToUint32(ip);
            Assert.Equal(expected,actual);
        }        
        
        [Theory]
        [InlineData("256.168.0.5")]
        [InlineData("0.0.0.0f")]
        public void StringToUnit32_ShouldFail(string ip)
        {
            Assert.Throws<ArgumentException>(()=>IpConveter.StringToUint32(ip));
        }
    }
}
