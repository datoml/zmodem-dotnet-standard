using System;
using System.Linq;
using System.Security.Cryptography;

namespace ZModem.CRC
{
    /// <summary>
    /// CRC 32 HashAlogrithm for ZModem
    /// </summary>
    public class CRC32 : HashAlgorithm
    {
        private static readonly uint Polynominal = 0x04C11DB7;
        private static readonly uint Initializer = 0xFFFFFFFF;
        private static readonly uint FinalXorValue = 0xFFFFFFFF;

        private readonly uint[] LookupTable = new uint[256];

        private uint value;

        public CRC32()
        {
            // Generate lookup table
            LookupTable = Utils.GenerateLookupTable(Polynominal);

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
                uint val = value;
                foreach (byte data in buffer)
                {
                    byte index = (byte)(val ^ data); ;
                    val = val >> 8;
                    val ^= LookupTable[index];
                }

                value = val;
            }
        }

        protected override byte[] HashFinal()
        {
            // Return final result
            var result = value ^ FinalXorValue;
            return BitConverter.GetBytes(result);
        }
    }
}
