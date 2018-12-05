namespace ZModem.CRC
{
    public static class CRCUtils
    {
        public static ushort[] GenerateLookupTable(ushort polynominal)
        {
            unchecked
            {
                var result = new ushort[256];
                byte t = 0;

                do
                {
                    ushort val = 0;

                    for (byte mask = 0x80; mask != 0; mask >>= 1)
                    {
                        if ((t & mask) != 0)
                        {
                            val ^= 0x8000;
                        }

                        if ((val & 0x8000) != 0)
                        {
                            val <<= 1;
                            val ^= polynominal;
                        }
                        else
                        {
                            val <<= 1;
                        }
                    }

                    result[t] = val;

                    ++t;
                }
                while (t != 0);

                return result;
            }
        }

        public static uint[] GenerateLookupTable(uint polynominal)
        {
            unchecked
            {
                uint[] result = new uint[256];
                byte t = 0;

                do
                {
                    uint val = 0;

                    for (byte mask = 0x80; mask != 0; mask >>= 1)
                    {
                        if ((t & mask) != 0)
                        {
                            val ^= 0x80000000;
                        }

                        if ((val & 0x80000000) != 0)
                        {
                            val <<= 1;
                            val ^= polynominal;
                        }
                        else
                        {
                            val <<= 1;
                        }
                    }

                    result[Reverse(t)] = Reverse(val);

                    ++t;
                }
                while (t != 0);

                return result;
            }
        }

        public static uint Reverse(uint data)
        {
            unchecked
            {
                uint ret = data;
                ret = (ret & 0x55555555) << 1 | (ret >> 1) & 0x55555555;
                ret = (ret & 0x33333333) << 2 | (ret >> 2) & 0x33333333;
                ret = (ret & 0x0F0F0F0F) << 4 | (ret >> 4) & 0x0F0F0F0F;
                ret = (ret << 24) | ((ret & 0xFF00) << 8) | ((ret >> 8) & 0xFF00) | (ret >> 24);
                return ret;
            }
        }

        public static ushort Reverse(ushort data)
        {
            unchecked
            {
                ushort ret = data;
                ret = (ushort)((ret & 0x5555) << 1 | (ret >> 1) & 0x5555);
                ret = (ushort)((ret & 0x3333) << 2 | (ret >> 2) & 0x3333);
                ret = (ushort)((ret & 0x0F0F) << 4 | (ret >> 4) & 0x0F0F);
                ret = (ushort)((ret & 0x00FF) << 8 | (ret >> 8) & 0x00FF);
                return ret;
            }
        }

        public static byte Reverse(byte data)
        {
            unchecked
            {
                uint u = (uint)data * 0x00020202;
                uint m = 0x01044010;
                uint s = u & m;
                uint t = (u << 2) & (m << 1);
                return (byte)((0x01001001 * (s + t)) >> 24);
            }
        }
    }
}
