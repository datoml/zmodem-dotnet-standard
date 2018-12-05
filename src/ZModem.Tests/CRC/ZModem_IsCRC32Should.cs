using System;
using System.Text;
using Xunit;
using ZModem.CRC;

namespace ZModem.Tests
{
    public class ZModem_IsCRC32Should
    {
        private readonly CRC32 CRCCalculator;

        public ZModem_IsCRC32Should()
        {
            CRCCalculator = new CRC32();
        }

        [Fact]
        public void Return0x00()
        {
            var data = new byte[0];
            var crcResult = CRCCalculator.ComputeHash(data);
            var result = BitConverter.ToUInt32(crcResult);
            Assert.True(result == 0x00);
        }

        [Theory]
        [InlineData("123456789", 0xcbf43926)]
        public void ReturnExpectedChecksum(string message, uint expectedChecksum)
        {
            var data = Encoding.ASCII.GetBytes(message);
            var crcValue = CRCCalculator.ComputeHash(data);
            var result = BitConverter.ToUInt32(crcValue);
            Assert.True(expectedChecksum.Equals(result));
        }
    }
}
