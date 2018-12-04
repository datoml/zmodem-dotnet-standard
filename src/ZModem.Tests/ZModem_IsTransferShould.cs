using System.IO.Ports;
using Xunit;

namespace ZModem.Tests
{
    public class ZModem_IsTransferShould
    {
        [Fact]
        public void ShouldReturnTransferInstance()
        {
            var port = new SerialPort();
            var result = new Transfer(port);

            Assert.NotNull(result);
        }
    }
}
