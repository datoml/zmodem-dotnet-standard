using Xunit;
using ZModem.Constants;

namespace ZModem.Tests
{
    public class ZModem_IsResponseHeaderShould
    {
        [Theory]
        [InlineData(new byte[] { 0, 1, 2, 3, 4 })]
        public void ReturnDefault(byte[] data)
        {
            var responseHeader = new ResponseHeader(data);
            Assert.NotNull(responseHeader);
            Assert.Equal(0, responseHeader.ZP0);
            Assert.Equal(0, responseHeader.ZP1);
            Assert.Equal(0, responseHeader.ZP2);
            Assert.Equal(0, responseHeader.ZP3);
            Assert.Equal(0, responseHeader.RequestedOffset);
            Assert.Null(responseHeader.BinaryHeader);
            Assert.Null(responseHeader.BinaryHeaderHuman);
            Assert.Null(responseHeader.FullHeaderHuman);
            Assert.Equal(0, (byte)responseHeader.FrameIndicator);
            Assert.Equal(HeaderType.None, responseHeader.ZHeader);
        }
    }
}
