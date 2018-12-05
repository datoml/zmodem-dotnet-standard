using System;
using System.Linq;
using ZModem.Constants;

namespace ZModem
{
    public class ResponseHeader
    {
        public ResponseHeader(byte[] buffer)
        {
            try
            {
                // Expected buffer length is 21.
                // In some cases the buffer length is longer than expected.
                // So we slice the buffer to get the response we want.
                if (buffer.Length > 21)
                {
                    var offset = buffer.Length - 21;
                    buffer = buffer
                        .Skip(offset)
                        .Take(21)
                        .ToArray();
                }


                if (buffer.Length < 20 || buffer.Length > 21)
                {
                    Console.WriteLine(@"Wrong header ¯\_(ツ)_/¯");
                    return;
                }

                // Cast to chars
                var chars = buffer.Select(x => (char)x).ToArray();

                // Set binary header
                BinaryHeader = chars.Take(3).ToArray();
                BinaryHeaderHuman = string.Join("", BinaryHeader);
                FullHeaderHuman = string.Join("", chars);

                // Set frame indicator
                FrameIndicator = (ControlBytes)chars[3];

                // Set header
                var headerInt = Utils.GetIntFromHex(chars[4], chars[5]);
                Enum.TryParse(headerInt.ToString(), out HeaderType header);
                ZHeader = header;

                // Pars arguments
                ZP0 = Utils.GetIntFromHex(chars[6], chars[7]);
                ZP1 = Utils.GetIntFromHex(chars[8], chars[9]);
                ZP2 = Utils.GetIntFromHex(chars[10], chars[11]);
                ZP3 = Utils.GetIntFromHex(chars[12], chars[13]);

                if (ZHeader == HeaderType.ZRPOS || ZHeader == HeaderType.ZACK)
                {
                    RequestedOffset = Utils.GetIntFromZModemOffset(ZP0, ZP1, ZP2, ZP3);
                }
            }
            catch (Exception)
            {

            }
        }

        public char[] BinaryHeader { get; private set; }

        public string BinaryHeaderHuman { get; private set; }

        public string FullHeaderHuman { get; set; }

        public ControlBytes FrameIndicator { get; private set; }

        public HeaderType ZHeader { get; private set; } = HeaderType.None;

        public int ZP0 { get; private set; }

        public int ZP1 { get; private set; }

        public int ZP2 { get; private set; }

        public int ZP3 { get; private set; }

        public int RequestedOffset { get; set; }
    }
}
