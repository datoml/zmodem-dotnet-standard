using Xunit;
using ZModem.Constants;
using ZModem.CRC;

namespace ZModem.Tests
{
    public class ZModem_IsUtilsShould
    {
        private readonly CRC16 CRC16Calculator;

        public ZModem_IsUtilsShould()
        {
            CRC16Calculator = new CRC16();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ShouldReturnNull(string message)
        {
            var result = Utils.RemoveControlCharacters(message);
            Assert.Null(result);
        }

        [Fact]
        public void ShouldReturnHexCommand()
        {
            var result = Utils.BuildCommonHexHeader(HeaderType.ZRQINIT, 0, 0, 0, 0, CRC16Calculator);
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void ShouldReturnBinCommand()
        {
            var result = Utils.Build16BitBinHeader(HeaderType.ZFILE, 0, 0, 0, 0, CRC16Calculator);
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void ShouldReturnDataCommand()
        {
            var result = Utils.Build16BitDataHeader(HeaderType.ZDATA, 0, 0, 0, 0, CRC16Calculator);
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Theory]
        [InlineData(0, 1024, ZDLESequence.ZCRCW)]
        [InlineData(900, 1024, ZDLESequence.ZCRCW)]
        [InlineData(1023, 1024, ZDLESequence.ZCRCW)]
        [InlineData(1024, 1024, ZDLESequence.ZCRCG)]
        public void ShouldReturnControlSequence (int dataLength, int chunkSize, ZDLESequence expected)
        {
            var result = Utils.GetControlSequenceFor(dataLength, chunkSize);
            Assert.True(result == expected);
        }
    }
}
