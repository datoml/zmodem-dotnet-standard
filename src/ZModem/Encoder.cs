using System.Collections.Generic;
using System.Linq;
using ZModem.Constants;

namespace ZModem
{
    public static class ZDLEEncoder
    {
        /// <summary>
        /// Zmodem software escapes ZDLE (0x18), 0x10, 0x90, 0x11, 0x91, 0x13, and 0x93. 
        /// </summary>
        private static readonly byte[] EscapeCharacters = new byte[] {
            (byte)ControlBytes.ZDLE,
            (byte)ControlBytes.XON,
            (byte)ControlBytes.XOFF,
            (byte)ControlBytes.XON | 0x80,
            (byte)ControlBytes.XOFF | 0x80,
            (byte)ControlBytes.DLE,
            (byte)ControlBytes.CTRL0x90,
            };

        /// <summary>
        /// If preceded by 0x40 or 0xc0 (@), 0x0d and 0x8d are also escaped to
        /// protect the Telenet command escape CR-@-CR. The receiver ignores
        /// 0x11, 0x91, 0x13, and 0x93 characters in the data stream. 
        /// </summary>
        private static readonly byte[] SpeicalEscapeCharacterTrigger = new byte[] {
            (byte)ControlBytes.ATSymbol,
            (byte)ControlBytes.À
        };
        private static readonly byte[] SpecialEscapeCharacters = new byte[] {
            (byte)ControlBytes.CR,
            (byte)ControlBytes.RI
        };


        public static byte[] EscapeControlCharacters(byte[] src)
        {
            var result = new List<byte>();

            for (int i = 0; i < src.Length; i++)
            {
                var b = src[i];

                if (b == (byte)ControlBytes.ZDLE)
                {
                    var escape = new byte[] { (byte)ControlBytes.ZDLE, (byte)ControlBytes.ZDLEE };
                    result.AddRange(escape);
                }
                else if (EscapeCharacters.Contains(b))
                {
                    var c = (char)(b | 0x40);

                    var escape = new byte[] { (byte)ControlBytes.ZDLE, (byte)c };
                    result.AddRange(escape);
                }
                else if (SpecialEscapeCharacters.Contains(b))
                {
                    var c = (char)(b | 0x40);
                    var escape = new byte[] { (byte)ControlBytes.ZDLE, (byte)c };
                    result.AddRange(escape);
                }
                else
                {
                    result.Add(b);
                }
            }

            return result.ToArray();
        }
    }
}
