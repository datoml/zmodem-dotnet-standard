using System.Text;
using Xunit;
using ZModem.Constants;
using ZModem.CRC;

namespace ZModem.Tests
{
    public class ZModem_IsCRCShould
    {
        private readonly CRC16 CRC16Calculator;

        public ZModem_IsCRCShould()
        {
            CRC16Calculator = new CRC16();
        }

        [Theory]
        [InlineData(35078)]
        public void ReturnHeaderChecksum(int expected)
        {
            var crc = Helper.Compute16BitHeader((int)HeaderType.ZFILE, 0, 0, 0, 0, CRC16Calculator);
            Assert.True(expected.Equals(crc));
        }
    }
}
