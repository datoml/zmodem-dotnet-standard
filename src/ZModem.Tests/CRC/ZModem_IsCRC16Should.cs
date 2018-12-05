using System;
using System.Text;
using Xunit;
using ZModem.CRC;

namespace ZModem.Tests
{
    public class ZModem_IsCRC16Should
    {
        private readonly CRC16 CRCCalculator;

        public ZModem_IsCRC16Should()
        {
            CRCCalculator = new CRC16();
        }

        [Fact]
        public void Return0x00()
        {
            var data = new byte[0];
            var crcValue = CRCCalculator.ComputeHash(data);
            var result = BitConverter.ToUInt16(crcValue);
            Assert.True(result == 0x00);
        }

        [Theory]
        [InlineData("123456789", 0x31c3)]
        public void ReturnExpectedChecksum(string message, ushort expectedChecksum)
        {
            var data = Encoding.ASCII.GetBytes(message);
            var crcValue = CRCCalculator.ComputeHash(data);
            var result = BitConverter.ToUInt16(crcValue);
            Assert.True(expectedChecksum.Equals(result));
        }
    }
}
