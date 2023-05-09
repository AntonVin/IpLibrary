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
        [InlineData("0.0.0.0",0u)]
        [InlineData("192.168.0.5", 3232235525u)]
        [InlineData("0.0.0.0",0u)]
        public void StringToUnit32_ShouldWork(string ip,uint expected)
        {
            uint actual = IpConveter.StringToUint32(ip);
            Assert.Equal(expected,actual);
        }

        [Theory]
        [InlineData("192.168.0.0.0")]
        [InlineData("192.168.0")]
        [InlineData("aaa.aa.a.a")]
        [InlineData(".168.0.0")]
        [InlineData("256.0.0.0")]
        public void StringToUint32_ShoulFail(string ip)
        {
            Assert.Throws<ArgumentException>(()=>IpConveter.StringToUint32(ip));
            Func<string,uint> testCode = IpConveter.StringToUint32;

            Assert.Throws<ArgumentException>(() => testCode(ip));
        }

    }
}
