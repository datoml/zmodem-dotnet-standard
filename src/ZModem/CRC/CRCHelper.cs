using System;

namespace ZModem.CRC
{
    public static class CRCHelper
    {
        public static int Compute16BitHeader(int type, int p0, int p1, int p2, int p3, CRC16 crcCalculator)
        {
            var crc = Compute16BitHeaderAsArray(type, p0, p1, p2, p3, crcCalculator);

            return BitConverter.ToUInt16(crc, 0);
        }

        public static uint Compute32BitHeader(int type, int p0, int p1, int p2, int p3, CRC32 crcCalculator)
        {
            var crc = Compute32BitHeaderAsArray(type, p0, p1, p2, p3, crcCalculator);

            return BitConverter.ToUInt32(crc, 0);
        }

        public static byte[] Compute16BitHeaderAsArray(int type, int p0, int p1, int p2, int p3, CRC16 crcCalculator)
        {
            var b = new byte[5];
            b[0] = Convert.ToByte(type);
            b[1] = Convert.ToByte(p0);
            b[2] = Convert.ToByte(p1);
            b[3] = Convert.ToByte(p2);
            b[4] = Convert.ToByte(p3);

            var crc = crcCalculator.ComputeHash(b);

            var encodedCRC = ZDLEEncoder.EscapeControlCharacters(crc);

            return encodedCRC;
        }

        public static byte[] Compute32BitHeaderAsArray(int type, int p0, int p1, int p2, int p3, CRC32 crcCalculator)
        {
            var b = new byte[5];
            b[0] = Convert.ToByte(type);
            b[1] = Convert.ToByte(p0);
            b[2] = Convert.ToByte(p1);
            b[3] = Convert.ToByte(p2);
            b[4] = Convert.ToByte(p3);

            var crc = crcCalculator.ComputeHash(b);

            var encodedCRC = ZDLEEncoder.EscapeControlCharacters(crc);

            return encodedCRC;
        }
    }
}
