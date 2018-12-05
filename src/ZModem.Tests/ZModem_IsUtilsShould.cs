using Xunit;
using ZModem.Constants;
using ZModem.CRC;

namespace ZModem.Tests
{
    public class ZModem_IsUtilsShould
    {
        private static CRC16 CRC16Calculator = new CRC16();
        private static CRC32 CRC32Calculator = new CRC32();

        [Theory]
        [InlineData(HeaderType.ZRQINIT, 0, 0, 0, 0, "**\u0018B00000000000000\r\n")]
        [InlineData(HeaderType.ZRINIT, 0, 0, 0, 0, "**\u0018B0100000000aa51\r\n")]
        [InlineData(HeaderType.ZSINIT, 0, 0, 0, 0, "**\u0018B02000000004483\r\n")]
        [InlineData(HeaderType.ZACK, 0, 0, 0, 0, "**\u0018B0300000000eed2\r\n")]
        [InlineData(HeaderType.ZFILE, 0, 0, 0, 0, "**\u0018B04000000008906\r\n")]
        [InlineData(HeaderType.ZSKIP, 0, 0, 0, 0, "**\u0018B05000000002357\r\n")]
        [InlineData(HeaderType.ZNAK, 0, 0, 0, 0, "**\u0018B0600000000cd85\r\n")]
        [InlineData(HeaderType.ZABORT, 0, 0, 0, 0, "**\u0018B070000000067d4\r\n")]
        [InlineData(HeaderType.ZFIN, 0, 0, 0, 0, "**\u0018B0800000000022d\r\n")]
        [InlineData(HeaderType.ZRPOS, 0, 0, 0, 0, "**\u0018B0900000000a87c\r\n")]
        [InlineData(HeaderType.ZDATA, 0, 0, 0, 0, "**\u0018B0a0000000046ae\r\n")]
        [InlineData(HeaderType.ZEOF, 0, 0, 0, 0, "**\u0018B0b00000000ecff\r\n")]
        [InlineData(HeaderType.ZFERR, 0, 0, 0, 0, "**\u0018B0c000000008b2b\r\n")]
        [InlineData(HeaderType.ZCRC, 0, 0, 0, 0, "**\u0018B0d00000000217a\r\n")]
        [InlineData(HeaderType.ZCHALLENGE, 0, 0, 0, 0, "**\u0018B0e00000000cfa8\r\n")]
        [InlineData(HeaderType.ZCOMPL, 0, 0, 0, 0, "**\u0018B0f0000000065f9\r\n")]
        [InlineData(HeaderType.ZCAN, 0, 0, 0, 0, "**\u0018B1000000000045a\r\n")]
        [InlineData(HeaderType.ZFREECNT, 0, 0, 0, 0, "**\u0018B1100000000ae0b\r\n")]
        [InlineData(HeaderType.ZCOMMAND, 0, 0, 0, 0, "**\u0018B120000000040d9\r\n")]
        [InlineData(HeaderType.ZESTERR, 0, 0, 0, 0, "**\u0018B1300000000ea88\r\n")]
        public void ReturnHexHeader(HeaderType type, int p0, int p1, int p2, int p3, string expected)
        {
            var actual = Utils.BuildCommonHexHeader(type, p0, p1, p2, p3, CRC16Calculator);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(HeaderType.ZFREECNT, 0, 0, 0, 0, "*\u0018A\u0011\0\0\0\0®\v")]
        [InlineData(HeaderType.ZRQINIT, 0, 0, 0, 0, "*\u0018A\0\0\0\0\0\0\0")]
        [InlineData(HeaderType.ZRINIT, 0, 0, 0, 0, "*\u0018A\u0001\0\0\0\0ªQ")]
        [InlineData(HeaderType.ZSINIT, 0, 0, 0, 0, "*\u0018A\u0002\0\0\0\0D\u0083")]
        [InlineData(HeaderType.ZACK, 0, 0, 0, 0, "*\u0018A\u0003\0\0\0\0îÒ")]
        [InlineData(HeaderType.ZFILE, 0, 0, 0, 0, "*\u0018A\u0004\0\0\0\0\u0089\u0006")]
        [InlineData(HeaderType.ZSKIP, 0, 0, 0, 0, "*\u0018A\u0005\0\0\0\0#W")]
        [InlineData(HeaderType.ZNAK, 0, 0, 0, 0, "*\u0018A\u0006\0\0\0\0Í\u0085")]
        [InlineData(HeaderType.ZABORT, 0, 0, 0, 0, "*\u0018A\a\0\0\0\0gÔ")]
        [InlineData(HeaderType.ZFIN, 0, 0, 0, 0, "*\u0018A\b\0\0\0\0\u0002-")]
        [InlineData(HeaderType.ZRPOS, 0, 0, 0, 0, "*\u0018A\t\0\0\0\0¨|")]
        [InlineData(HeaderType.ZDATA, 0, 0, 0, 0, "*\u0018A\n\0\0\0\0F®")]
        [InlineData(HeaderType.ZEOF, 0, 0, 0, 0, "*\u0018A\v\0\0\0\0ìÿ")]
        [InlineData(HeaderType.ZFERR, 0, 0, 0, 0, "*\u0018A\f\0\0\0\0\u008b+")]
        [InlineData(HeaderType.ZCRC, 0, 0, 0, 0, "*\u0018A\r\0\0\0\0!z")]
        [InlineData(HeaderType.ZCHALLENGE, 0, 0, 0, 0, "*\u0018A\u000e\0\0\0\0Ï¨")]
        [InlineData(HeaderType.ZCOMPL, 0, 0, 0, 0, "*\u0018A\u000f\0\0\0\0eù")]
        [InlineData(HeaderType.ZCAN, 0, 0, 0, 0, "*\u0018A\u0010\0\0\0\0\u0004Z")]
        [InlineData(HeaderType.ZCOMMAND, 0, 0, 0, 0, "*\u0018A\u0012\0\0\0\0@Ù")]
        [InlineData(HeaderType.ZESTERR, 0, 0, 0, 0, "*\u0018A\u0013\0\0\0\0ê\u0088")]
        public void Return16BitBinHeader(HeaderType type, int p0, int p1, int p2, int p3, string expected)
        {
            var actual = Utils.Build16BitBinHeader(type, p0, p1, p2, p3, CRC16Calculator);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(HeaderType.ZFREECNT, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018A\u0011\0\0\u0001\u0001\u0018\u001b")]
        [InlineData(HeaderType.ZRQINIT, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018A\0\0\0\u0001\u0001P\u0018")]
        [InlineData(HeaderType.ZRINIT, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018A\u0001\0\0\u0001\u0001\u0089A")]
        [InlineData(HeaderType.ZSINIT, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018A\u0002\0\0\u0001\u0001Ó\u0018")]
        [InlineData(HeaderType.ZACK, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018A\u0003\0\0\u0001\u0001ÍÂ")]
        [InlineData(HeaderType.ZFILE, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018A\u0004\0\0\u0001\u0001ª\u0016")]
        [InlineData(HeaderType.ZSKIP, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018A\u0005\0\0\u0001\u0001\0G")]
        [InlineData(HeaderType.ZNAK, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018A\u0006\0\0\u0001\u0001î\u0095")]
        [InlineData(HeaderType.ZABORT, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018A\a\0\0\u0001\u0001DÄ")]
        [InlineData(HeaderType.ZFIN, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018A\b\0\0\u0001\u0001!=")]
        [InlineData(HeaderType.ZRPOS, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018A\t\0\0\u0001\u0001\u008bl")]
        [InlineData(HeaderType.ZDATA, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018A\n\0\0\u0001\u0001e¾")]
        [InlineData(HeaderType.ZEOF, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018A\v\0\0\u0001\u0001Ïï")]
        [InlineData(HeaderType.ZFERR, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018A\f\0\0\u0001\u0001¨;")]
        [InlineData(HeaderType.ZCRC, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018A\r\0\0\u0001\u0001\u0002j")]
        [InlineData(HeaderType.ZCHALLENGE, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018A\u000e\0\0\u0001\u0001ì¸")]
        [InlineData(HeaderType.ZCOMPL, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018A\u000f\0\0\u0001\u0001Fé")]
        [InlineData(HeaderType.ZCAN, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018A\u0010\0\0\u0001\u0001'J")]
        [InlineData(HeaderType.ZCOMMAND, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018A\u0012\0\0\u0001\u0001cÉ")]
        [InlineData(HeaderType.ZESTERR, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018A\u0013\0\0\u0001\u0001É\u0098")]
        public void Return16BitBinFileHeader(HeaderType type, ZFILEConversionOption f0, ZFILEManagementOption f1, ZFILETransportOption f2, ZFILEExtendedOptions f3, string expected)
        {
            var actual = Utils.Build16BitBinHeader(type, f0, f1, f2, f3, CRC16Calculator);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(HeaderType.ZFREECNT, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018C\u0011\0\0\u0001\u0001øH¾õ")]
        [InlineData(HeaderType.ZRQINIT, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018C\0\0\0\u0001\u0001Êö>¨")]
        [InlineData(HeaderType.ZRINIT, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018C\u0001\0\0\u0001\u0001zß^\u0095")]
        [InlineData(HeaderType.ZSINIT, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018C\u0002\0\0\u0001\u0001ª¥þÒ")]
        [InlineData(HeaderType.ZACK, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018C\u0003\0\0\u0001\u0001\u001a\u008c\u009eï")]
        [InlineData(HeaderType.ZFILE, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018C\u0004\0\0\u0001\u0001\nP¾]")]
        [InlineData(HeaderType.ZSKIP, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018C\u0005\0\0\u0001\u0001ºyÞ`")]
        [InlineData(HeaderType.ZNAK, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018C\u0006\0\0\u0001\u0001j\u0003~'")]
        [InlineData(HeaderType.ZABORT, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018C\a\0\0\u0001\u0001Ú*\u001e\u001a")]
        [InlineData(HeaderType.ZFIN, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018C\b\0\0\u0001\u0001\v½N\u0098")]
        [InlineData(HeaderType.ZRPOS, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018C\t\0\0\u0001\u0001»\u0094.¥")]
        [InlineData(HeaderType.ZDATA, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018C\n\0\0\u0001\u0001kî\u008eâ")]
        [InlineData(HeaderType.ZEOF, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018C\v\0\0\u0001\u0001ÛÇîß")]
        [InlineData(HeaderType.ZFERR, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018C\f\0\0\u0001\u0001Ë\u001bÎm")]
        [InlineData(HeaderType.ZCRC, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018C\r\0\0\u0001\u0001{2®P")]
        [InlineData(HeaderType.ZCHALLENGE, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018C\u000e\0\0\u0001\u0001«H\u000e\u0017")]
        [InlineData(HeaderType.ZCOMPL, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018C\u000f\0\0\u0001\u0001\u001ban*")]
        [InlineData(HeaderType.ZCAN, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018C\u0010\0\0\u0001\u0001HaÞÈ")]
        [InlineData(HeaderType.ZCOMMAND, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018C\u0012\0\0\u0001\u0001(2\u001e²")]
        [InlineData(HeaderType.ZESTERR, ZFILEConversionOption.ZCBIN, ZFILEManagementOption.ZMNEWL, ZFILETransportOption.None, ZFILEExtendedOptions.None, "*\u0018C\u0013\0\0\u0001\u0001\u0098\u001b~\u008f")]
        public void Return32BitBinFileHeader(HeaderType type, ZFILEConversionOption f0, ZFILEManagementOption f1, ZFILETransportOption f2, ZFILEExtendedOptions f3, string expected)
        {
            var actual = Utils.Build32BitBinHeader(type, f0, f1, f2, f3, CRC32Calculator);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(HeaderType.ZFREECNT, 0, 0, 0, 0, "*\u0018A\u0018J\0\0\0\0®\v")]
        [InlineData(HeaderType.ZRQINIT, 0, 0, 0, 0, "*\u0018A\u0018J\0\0\0\0\0\0")]
        [InlineData(HeaderType.ZRINIT, 0, 0, 0, 0, "*\u0018A\u0018J\0\0\0\0ªQ")]
        [InlineData(HeaderType.ZSINIT, 0, 0, 0, 0, "*\u0018A\u0018J\0\0\0\0D\u0083")]
        [InlineData(HeaderType.ZACK, 0, 0, 0, 0, "*\u0018A\u0018J\0\0\0\0îÒ")]
        [InlineData(HeaderType.ZFILE, 0, 0, 0, 0, "*\u0018A\u0018J\0\0\0\0\u0089\u0006")]
        [InlineData(HeaderType.ZSKIP, 0, 0, 0, 0, "*\u0018A\u0018J\0\0\0\0#W")]
        [InlineData(HeaderType.ZNAK, 0, 0, 0, 0, "*\u0018A\u0018J\0\0\0\0Í\u0085")]
        [InlineData(HeaderType.ZABORT, 0, 0, 0, 0, "*\u0018A\u0018J\0\0\0\0gÔ")]
        [InlineData(HeaderType.ZFIN, 0, 0, 0, 0, "*\u0018A\u0018J\0\0\0\0\u0002-")]
        [InlineData(HeaderType.ZRPOS, 0, 0, 0, 0, "*\u0018A\u0018J\0\0\0\0¨|")]
        [InlineData(HeaderType.ZDATA, 0, 0, 0, 0, "*\u0018A\u0018J\0\0\0\0F®")]
        [InlineData(HeaderType.ZEOF, 0, 0, 0, 0, "*\u0018A\u0018J\0\0\0\0ìÿ")]
        [InlineData(HeaderType.ZFERR, 0, 0, 0, 0, "*\u0018A\u0018J\0\0\0\0\u008b+")]
        [InlineData(HeaderType.ZCRC, 0, 0, 0, 0, "*\u0018A\u0018J\0\0\0\0!z")]
        [InlineData(HeaderType.ZCHALLENGE, 0, 0, 0, 0, "*\u0018A\u0018J\0\0\0\0Ï¨")]
        [InlineData(HeaderType.ZCOMPL, 0, 0, 0, 0, "*\u0018A\u0018J\0\0\0\0eù")]
        [InlineData(HeaderType.ZCAN, 0, 0, 0, 0, "*\u0018A\u0018J\0\0\0\0\u0004Z")]
        [InlineData(HeaderType.ZCOMMAND, 0, 0, 0, 0, "*\u0018A\u0018J\0\0\0\0@Ù")]
        [InlineData(HeaderType.ZESTERR, 0, 0, 0, 0, "*\u0018A\u0018J\0\0\0\0ê\u0088")]
        public void Return16BitBinDataHeader(HeaderType type, int p0, int p1, int p2, int p3, string expected)
        {
            var actual = Utils.Build16BitDataHeader(type, p0, p1, p2, p3, CRC16Calculator);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(HeaderType.ZFREECNT, 0, 0, 0, 0, "*\u0018C\u0018J\0\0\0\0/I¢\u009b")]
        [InlineData(HeaderType.ZRQINIT, 0, 0, 0, 0, "*\u0018C\u0018J\0\0\0\0\u001d÷\"Æ")]
        [InlineData(HeaderType.ZRINIT, 0, 0, 0, 0, "*\u0018C\u0018J\0\0\0\0­ÞBû")]
        [InlineData(HeaderType.ZSINIT, 0, 0, 0, 0, "*\u0018C\u0018J\0\0\0\0}¤â¼")]
        [InlineData(HeaderType.ZACK, 0, 0, 0, 0, "*\u0018C\u0018J\0\0\0\0Í\u0018Í\u0082\u0081")]
        [InlineData(HeaderType.ZFILE, 0, 0, 0, 0, "*\u0018C\u0018J\0\0\0\0ÝQ¢3")]
        [InlineData(HeaderType.ZSKIP, 0, 0, 0, 0, "*\u0018C\u0018J\0\0\0\0mxÂ\u000e")]
        [InlineData(HeaderType.ZNAK, 0, 0, 0, 0, "*\u0018C\u0018J\0\0\0\0½\u0002bI")]
        [InlineData(HeaderType.ZABORT, 0, 0, 0, 0, "*\u0018C\u0018J\0\0\0\0\u0018M+\u0002t")]
        [InlineData(HeaderType.ZFIN, 0, 0, 0, 0, "*\u0018C\u0018J\0\0\0\0Ü¼Rö")]
        [InlineData(HeaderType.ZRPOS, 0, 0, 0, 0, "*\u0018C\u0018J\0\0\0\0l\u00952Ë")]
        [InlineData(HeaderType.ZDATA, 0, 0, 0, 0, "*\u0018C\u0018J\0\0\0\0¼ï\u0092\u008c")]
        [InlineData(HeaderType.ZEOF, 0, 0, 0, 0, "*\u0018C\u0018J\0\0\0\0\fÆò±")]
        [InlineData(HeaderType.ZFERR, 0, 0, 0, 0, "*\u0018C\u0018J\0\0\0\0\u001c\u001aÒ\u0003")]
        [InlineData(HeaderType.ZCRC, 0, 0, 0, 0, "*\u0018C\u0018J\0\0\0\0¬3²>")]
        [InlineData(HeaderType.ZCHALLENGE, 0, 0, 0, 0, "*\u0018C\u0018J\0\0\0\0|I\u0012y")]
        [InlineData(HeaderType.ZCOMPL, 0, 0, 0, 0, "*\u0018C\u0018J\0\0\0\0Ì`rD")]
        [InlineData(HeaderType.ZCAN, 0, 0, 0, 0, "*\u0018C\u0018J\0\0\0\0\u009f`Â¦")]
        [InlineData(HeaderType.ZCOMMAND, 0, 0, 0, 0, "*\u0018C\u0018J\0\0\0\0ÿ3\u0002Ü")]
        [InlineData(HeaderType.ZESTERR, 0, 0, 0, 0, "*\u0018C\u0018J\0\0\0\0O\u001abá")]
        public void Return32BitBinDataHeader(HeaderType type, int p0, int p1, int p2, int p3, string expected)
        {
            var actual = Utils.Build32BitDataHeader(type, p0, p1, p2, p3, CRC32Calculator);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData('0', '1', 1)]
        [InlineData('0', 'D', 13)]
        [InlineData('F', 'F', 255)]
        public void ReturnIntFromHex(char one, char two, int expected)
        {
            var actual = Utils.GetIntFromHex(one, two);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0)]
        [InlineData(255, 255, 0, 0, 0)]
        [InlineData(1024, 0, 4, 0, 0)]
        [InlineData(8192, 0, 32, 0, 0)]
        [InlineData(16384, 0, 64, 0, 0)]
        [InlineData(8388608, 0, 0, 128, 0)]        
        [InlineData(268435456, 0, 0, 0, 16)]
        public void ReturnZModemFileOffset(int offset, int expP0, int expP1, int expP2, int expP3)
        {
            Utils.GenerateZModemFileOffset(offset, out int p0, out int p1, out int p2, out int p3);

            Assert.Equal(p0, expP0);
            Assert.Equal(p1, expP1);
            Assert.Equal(p2, expP2);
            Assert.Equal(p3, expP3);
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0)]
        [InlineData(255, 0, 0, 0, 255)]
        [InlineData(0, 4, 0, 0, 1024)]
        [InlineData(0, 32, 0, 0, 8192)]
        [InlineData(0, 64, 0, 0, 16384)]
        [InlineData(0, 0, 128, 0, 8388608)]
        [InlineData(0, 0, 0, 16, 268435456)]
        public void ReturnIntFromZModemOffset(int p0, int p1, int p2, int p3, int expected)
        {
            var actual = Utils.GetIntFromZModemOffset(p0, p1, p2, p3);
            Assert.Equal(expected, actual);
        }
    }
}
