using System;
using Xunit;
using ZModem.Constants;

namespace ZModem.Tests
{
    public class ZModem_IsEncoderShould
    {
        [Theory]
        [InlineData((byte)ControlBytes.ZDLE, "18-58")]
        [InlineData((byte)ControlBytes.XON, "18-51")]
        [InlineData((byte)ControlBytes.XOFF, "18-53")]
        [InlineData((byte)ControlBytes.XON | 0x80, "18-D1")]
        [InlineData((byte)ControlBytes.XOFF | 0x80, "18-D3")]
        [InlineData((byte)ControlBytes.DLE, "18-50")]
        [InlineData((byte)ControlBytes.CTRL0x90, "18-D0")]
        [InlineData((byte)ControlBytes.CR, "18-4D")]
        [InlineData((byte)ControlBytes.RI, "18-CD")]
        public void ReturnEscapedValue(byte input, string expected)
        {
            var encodedInput = ZDLEEncoder.EscapeControlCharacters(new byte[] { input });
            var result = BitConverter.ToString(encodedInput);
            Assert.Equal(expected, result);
        }
    }
}
