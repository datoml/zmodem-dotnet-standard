/*
 *   Z M O D E M . H     Manifest constants for ZMODEM
 *    application to application file transfer protocol
 *    05-23-87  Chuck Forsberg Omen Technology Inc
 */

namespace ZModem.Constants
{
    public class Constants
    {
        /// <summary>
        /// Max length of attention string
        /// </summary>
        public static int ZSINITAttentionString = 32;
    }

    /// <summary>
    /// Header types
    /// </summary>
    public enum HeaderType
    {
        /// <summary>
        /// Request receive init
        /// </summary>
        ZRQINIT = 0,
        /// <summary>
        /// Receive init
        /// </summary>
        ZRINIT = 1,
        /// <summary>
        /// Send init sequence (optional)
        /// </summary>
        ZSINIT = 2,
        /// <summary>
        /// ACK to above
        /// </summary>
        ZACK = 3,
        /// <summary>
        /// File name from sender
        /// </summary>
        ZFILE = 4,
        /// <summary>
        /// To sender: skip this file
        /// </summary>
        ZSKIP = 5,
        /// <summary>
        /// Last packet was garbled
        /// </summary>
        ZNAK = 6,
        /// <summary>
        /// Abort batch transfers
        /// </summary>
        ZABORT = 7,
        /// <summary>
        /// Finish session
        /// </summary>
        ZFIN = 8,
        /// <summary>
        /// Resume data trans at this position
        /// </summary>
        ZRPOS = 9,
        /// <summary>
        /// Data packet(s) follow
        /// </summary>
        ZDATA = 10,
        /// <summary>
        /// End of file
        /// </summary>
        ZEOF = 11,
        /// <summary>
        /// Fatal Read or Write error Detected
        /// </summary>
        ZFERR = 12,
        /// <summary>
        /// Request for file CRC and response
        /// </summary>
        ZCRC = 13,
        /// <summary>
        /// Receiver's Challenge
        /// </summary>
        ZCHALLENGE = 14,
        /// <summary>
        /// Request is complete
        /// </summary>
        ZCOMPL = 15,
        /// <summary>
        /// Other end canned session with CAN*5
        /// </summary>
        ZCAN = 16,
        /// <summary>
        /// Request for free bytes on filesystem
        /// </summary>
        ZFREECNT = 17,
        /// <summary>
        /// Command from sending program
        /// </summary>
        ZCOMMAND = 18,
        /// <summary>
        /// Output to standard error, data follows
        /// </summary>
        ZESTERR = 19
    };

    /// <summary>
    /// Control bytes
    /// </summary>
    public enum ControlBytes
    {
        /// <summary>
        /// 052 Padding character begins frames
        /// </summary>
        ZPAD = '*',
        /// <summary>
        /// Ctrl-cc Zmodem escape - `ala BISYNC DLE
        /// </summary>
        ZDLE = 0x18,
        /// <summary>
        /// Escaped ZDLE as transmitted
        /// </summary>
        ZDLEE = 0x58,
        /// <summary>
        /// Binary frame indicator
        /// </summary>
        ZBIN = 'A',
        /// <summary>
        /// HEX frame indicator
        /// </summary>
        ZHEX = 'B',
        /// <summary>
        /// Binary frame with 32 bit FCS
        /// </summary>
        ZBINC = 'C',
        XON = 0x011,
        XOFF = 0x013,
        /// <summary>
        /// CR character
        /// </summary>
        CR = 0x0d,
        /// <summary>
        /// LF character
        /// </summary>
        LF = 0x0a,
    }

    /// <summary>
    /// Parameters for ZSINIT frame
    /// </summary>
    public enum ZSINIT
    {
        /// <summary>
        /// Transmitter expects ctl chars to be escaped
        /// </summary>
        TESCCTL = 64,
        /// <summary>
        /// Transmitter expects 8th bit to be escaped
        /// </summary>
        TESC8 = 128
    }

    /// <summary>
    /// Conversion options for ZFILE header
    /// </summary>
    public enum ZFILEConversionOption
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Binary transfer - inhibit conversion
        /// </summary>
        ZCBIN = 1,
        /// <summary>
        /// Convert NL to local end of line convention
        /// </summary>
        ZCNL = 2,
        /// <summary>
        /// Resume interrupted file transfer
        /// </summary>
        ZCRESUM = 3
    }

    /// <summary>
    /// Management options for ZFILE header
    /// </summary>
    public enum ZFILEManagementOption
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Skip file if not present at rx
        /// </summary>
        ZMSKNOLOC = 128,
        /// <summary>
        /// Mask for the choices below
        /// </summary>
        ZMMASK = 31,
        /// <summary>
        /// Transfer if source newer or longer
        /// </summary>
        ZMNEWL = 1,
        /// <summary>
        /// Transfer if different file CRC or length
        /// </summary>
        ZMCRC = 2,
        /// <summary>
        /// Append contents to existing file (if any)
        /// </summary>
        ZMAPND = 3,
        /// <summary>
        /// Replace existing file
        /// </summary>
        ZMCLOB = 4,
        /// <summary>
        /// Transfer if source newer
        /// </summary>
        ZMNEW = 5,
        /// <summary>
        /// Transfer if dates or lengths different
        /// </summary>
        ZMDIFF = 6,
        /// <summary>
        /// Protect destination file
        /// </summary>
        ZMPROT = 7
    }

    /// <summary>
    /// Transport options for ZFile header
    /// </summary>
    public enum ZFILETransportOption
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Lempel-Ziv compression
        /// </summary>
        ZTLZW = 1,
        /// <summary>
        /// Encryption
        /// </summary>
        ZTCRYPT = 2,
        /// <summary>
        /// Run Length encoding
        /// </summary>
        ZTRLE = 3
    }

    /// <summary>
    /// Extended options for ZFILE header
    /// </summary>
    public enum ZFILEExtendedOptions
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Encoding for sparse file operations
        /// </summary>
        ZXSPARS = 64
    }

    /// <summary>
    /// ZDLE Sequences
    /// </summary>
    public enum ZDLESequence
    {
        /// <summary>
        /// CRC next, frame ends, header packet follows
        /// </summary>
        ZCRCE = 'h',
        /// <summary>
        /// CRC next, frame continues nonstop
        /// </summary>
        ZCRCG = 'i',
        /// <summary>
        /// CRC next, frame continues, ZACK expected
        /// </summary>
        ZCRCQ = 'j',
        /// <summary>
        /// CRC next, ZACK expected, end of frame
        /// </summary>
        ZCRCW = 'k',
        /// <summary>
        /// Translate to rubout 0177
        /// </summary>
        ZRUB0 = 'l',
        /// <summary>
        /// Translate to rubout 0377
        /// </summary>
        ZRUB1 = 'm',
    }

    /// <summary>
    /// Parameters for ZCOMMAND frame ZF0 (otherwise 0)
    /// </summary>
    public enum ZSCommandHeader
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Acknowledge, then do command
        /// </summary>
        ZCACK1 = 1
    }
}
