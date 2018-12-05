using Xunit;
using ZModem.Constants;
using ZModem.CRC;

namespace ZModem.Tests
{
    public class ZModem_IsCRCHelperShould
    {
        private readonly CRC16 CRC16Calculator;
        private readonly CRC32 CRC32Calculator;

        public ZModem_IsCRCHelperShould()
        {
            CRC16Calculator = new CRC16();
            CRC32Calculator = new CRC32();
        }

        [Theory]
        [InlineData(new byte[] { 6, 137 })]
        public void Return16BitHeaderChecksumAsByte(byte[] expected)
        {
            var result = CRCHelper.Compute16BitHeaderAsArray((int)HeaderType.ZFILE, 0, 0, 0, 0, CRC16Calculator);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.True(result[i] == expected[i]);
            }
        }

        [Theory]
        [InlineData(35078)]
        public void Return16BitHeaderChecksumAsInt(int expected)
        {
            var crc = CRCHelper.Compute16BitHeader((int)HeaderType.ZFILE, 0, 0, 0, 0, CRC16Calculator);
            Assert.True(expected.Equals(crc));
        }

        [Theory]
        [InlineData(new byte[] { 221, 81, 162, 51 })]
        public void Return32BitHeaderChecksumAsByte(byte[] expected)
        {
            var result = CRCHelper.Compute32BitHeaderAsArray((int)HeaderType.ZFILE, 0, 0, 0, 0, CRC32Calculator);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.True(result[i] == expected[i]);
            }
        }

        [Theory]
        [InlineData(866275805)]
        public void Return32BitHeaderChecksumAsInt(uint expected)
        {
            var crc = CRCHelper.Compute32BitHeader((int)HeaderType.ZFILE, 0, 0, 0, 0, CRC32Calculator);
            Assert.True(expected.Equals(crc));
        }
    }
}
