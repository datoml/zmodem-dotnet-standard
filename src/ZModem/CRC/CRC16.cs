using System;
using System.Linq;
using System.Security.Cryptography;

namespace ZModem.CRC
{
    public class CRC16 : HashAlgorithm
    {
        private static readonly ushort Polynominal = 0x1021;
        private static readonly ushort Initializer = 0x00000000;

        private readonly ushort[] LookupTable = new ushort[256];

        private ushort value;

        public CRC16()
        {
            // Generate lookup table
            LookupTable = CRCUtils.GenerateLookupTable(Polynominal);

            // Initialize all required values
            Initialize();
        }

        public override void Initialize()
        {
            // Initial value
            value = Initializer;
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            // Get buffer slice
            var buffer = array
                .Skip(ibStart)
                .Take(cbSize);

            // Compute hash
            unchecked
            {
                ushort val = value;
                foreach (byte data in buffer)
                {
                    byte index = (byte)((val >> 8) ^ data);
                    val = (ushort)(val << 8);
                    val ^= LookupTable[index];
                }

                value = val;
            }
        }

        protected override byte[] HashFinal()
        {
            // Return final result
            var result = (ushort)(value ^ 0);
            return BitConverter.GetBytes(result);
        }        
    }
}
