﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using ZModem.Constants;
using ZModem.CRC;

namespace ZModem
{
    [ExcludeFromCodeCoverage]
    public class Transfer
    {
        private static readonly object PadLock = new object();

        private const int TaskTimeout = 4;
        private const int ChunkSize = 2048;
        private const int ReadRetryAttempts = 3;
        private const int ReadRetryAttemptTimeout = 100;

        private readonly SerialPort SerialPort;
        private readonly bool WithDebug;

        /// <summary>
        /// Create transfer instance
        /// </summary>
        /// <param name="serialPort"></param>
        public Transfer(SerialPort serialPort, bool withDebug = false)
        {
            SerialPort = serialPort;
            WithDebug = withDebug;
        }

        /// <summary>
        /// Upload a file
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool Upload(string filename)
        {
            var fileInfo = new FileInfo(filename);

            var data = File.ReadAllBytes(fileInfo.FullName);

            return Upload(fileInfo.Name, data, fileInfo.LastWriteTimeUtc);
        }

        /// <summary>
        /// Upload from memory
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Upload(byte[] data)
        {
            var tempFile = Path.GetTempFileName();
            var fileInfo = new FileInfo(tempFile);

            var result = Upload(fileInfo.Name, data, DateTimeOffset.UtcNow);

            if (File.Exists(fileInfo.FullName))
            {
                File.Delete(fileInfo.FullName);
            }

            return result;
        }

        /// <summary>
        /// Upload to a remote device
        /// </summary>
        /// <param name="serialPort">Serial port</param>
        /// <param name="fileaName">Full file name</param>
        /// <returns></returns>
        private bool Upload(string filename, byte[] data, DateTimeOffset lastWriteTimeUtc)
        {
            var hasUploadedSuccessfully = default(bool);

            var sw = new Stopwatch();
            sw.Start();

            var crc16 = new CRC16();
            var crc32 = new CRC32();

            // Close serial port if already open.
            CloseSerialPortIfOpen();

            // Initiate session
            var hasExecutedCommandSuccessfully = SendZRQINITFrame(crc16);

            if (hasExecutedCommandSuccessfully)
            {
                // Send ZFILE header with filename.
                hasUploadedSuccessfully = hasExecutedCommandSuccessfully = SendZFILEHeaderCommand(filename, data.Length, lastWriteTimeUtc, crc32); 
            }

            if (hasExecutedCommandSuccessfully)
            {
                // Send binary data wrapped into ZDATA header.
                SendZDATAPackets(data, 0, ChunkSize, crc32); 
            }

            if (hasExecutedCommandSuccessfully)
            {
                // Send EOF command
                hasUploadedSuccessfully = hasExecutedCommandSuccessfully = SendEOFCommand(data.Length, crc16); 
            }

            // Send ZFIN to finish the session
            hasExecutedCommandSuccessfully = SendFinishSession(crc16);

            // Send over and out
            SendCommand("OO");

            sw.Stop();
            Console.WriteLine($"Took: {sw.ElapsedMilliseconds}ms");

            return hasUploadedSuccessfully;
        }

        /// <summary>
        /// Sent by the sending program, to trigger the receiving program to send its ZRINIT header.
        /// This avoids the aggravating startup delay associated with XMODEM and Kermit transfers.
        /// The sending program may repeat the receive invitation (including ZRQINIT) if a response is not obtained at first.
        /// 
        /// ZF0 contains ZCOMMAND if the program is attempting to send a command, 0 otherwise. 
        /// </summary>
        private bool SendZRQINITFrame(CRC16 crcCalculator)
        {
            var result = default(bool);

            var zrqinitFrame = Utils.BuildCommonHexHeader(HeaderType.ZRQINIT, 0, 0, 0, 0, crcCalculator);

            // Send command
            var response = SendCommand(zrqinitFrame, true, HeaderType.ZRINIT);

            // We expect ZRINIT as response
            if (response?.ZHeader == HeaderType.ZRINIT)
            {
                result = true;
            }
            else
            {
                // TODO
                /*
                 * If the receiving program receives a ZRQINIT header, it resends the
                 * ZRINIT header.  If the sending program receives the ZCHALLENGE header,
                 * it places the data in ZP0...ZP3 in an answering ZACK header.
                 * If the receiving program receives a ZRINIT header, it is an echo
                 * indicating that the sending program is not operational.
                 * Eventually the sending program correctly receives the ZRINIT header.
                 * The sender may then send an optional ZSINIT frame to define the
                 * receiving program's Attn sequence, or to specify complete control
                 * character escaping. if the receiver specifies the same or higher
                 * level of escaping the ZSINIT frame need not be sent unless an Attn
                 * sequence is needed.
                 * If the ZSINIT header specifies ESCCTL or ESC8, a HEX header is used,
                 * and the receiver activates the specified ESC modes before reading the
                 * following data subpacket.
                 * The receiver sends a ZACK header in response, containing 0
                 */
            }

            return result;
        }

        /// <summary>
        /// The sender then sends a ZFILE header with ZMODEM Conversion, Management, and Transport options[3]
        /// followed by a ZCRCW data subpacket containing the file name, file length,
        /// modification date, and other information identical to that used by YMODEM Batch. 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="crcCalculator"></param>
        private bool SendZFILEHeaderCommand(string filename, int length, DateTimeOffset lastWriteTimeUtc, CRC32 crcCalculator)
        {
            var result = default(bool);

            var isExtended = true;

            var zFileHeader = Utils.Build32BitBinHeader(
                HeaderType.ZFILE,
                ZFILEConversionOption.ZCBIN,
                ZFILEManagementOption.ZMNEWL,
                ZFILETransportOption.None,
                ZFILEExtendedOptions.None,
                crcCalculator
            );

            var zFileHeaderQueue = new Queue<byte>();
            foreach (var c in zFileHeader)
            {
                zFileHeaderQueue.Enqueue((byte)c);
            }

            // Send ZFILE header first - No response
            SendCommand(zFileHeaderQueue.ToArray());

            var dataQueue = new Queue<byte>();
            foreach (char c in filename)
            {
                dataQueue.Enqueue((byte)c);
            }

            if (isExtended)
            {
                dataQueue.Enqueue(0);

                // File length as decimal string
                var fileLength = length.ToString();

                foreach (var c in fileLength)
                {
                    dataQueue.Enqueue((byte)c);
                }

                // Space
                dataQueue.Enqueue(0x20);

                var utcTime = lastWriteTimeUtc.ToUnixTimeSeconds();
                var octalString = Convert.ToString(utcTime, 8);

                // Modification date
                foreach (var c in octalString)
                {
                    dataQueue.Enqueue((byte)c);
                }
            }
            else
            {
                // The file information is terminated by a null.
                // If only the pathname is sent, the pathname is terminated with two nulls.
                dataQueue.Enqueue(0);
                dataQueue.Enqueue(0);
            }

            byte[] data = dataQueue.Concat(new byte[] { (byte)ZDLESequence.ZCRCW }).ToArray();
            dataQueue.Enqueue((byte)ControlBytes.ZDLE);
            dataQueue.Enqueue((byte)ZDLESequence.ZCRCW);

            var crc = crcCalculator.ComputeHash(data);

            var encodedCRC = dataQueue
                .Concat(ZDLEEncoder.EscapeControlCharacters(crc))
                .ToArray();

            var response = SendCommand(encodedCRC, true, HeaderType.ZRPOS);

            // We expect ZRPOS
            if (response?.ZHeader == HeaderType.ZRPOS)
            {
                result = true;
            }

            /*
             * The receiver may respond with a ZSKIP header, which makes the sender
             * proceed to the next file (if any) in the batch.
             * 
             * A ZRPOS header from the receiver initiates transmission of the file
             * data starting at the offset in the file specified in the ZRPOS header.
             * Normally the receiver specifies the data transfer to begin begin at
             * offset 0 in the file.
             */

            if (response?.ZHeader == HeaderType.ZSKIP
                || response?.ZHeader == HeaderType.ZRPOS
                || response?.ZHeader == HeaderType.ZCRC)
            {
                /*
                 * The receiver has a file with the same name and length, may
                 * respond with a ZCRC header with a byte count, which
                 * requires the sender to perform a 32 bit CRC on the
                 * specified number of bytes in the file and transmit the
                 * complement of the CRC in an answering ZCRC header.the crc is
                 * initialised to 0xfffffff; a byte count of 0 implies the entire
                 * file the receiver uses this information to determine whether to
                 * accept the file or skip it.  This sequence may be triggered
                 * by the ZMCRC Management Option. 
                 */
            }

            return result;
        }

        private void SendZDATAPackets(byte[] src, int? offset, int chunkSize, CRC32 crcCalculator)
        {
            // Generate intitial ZDATA header frame
            var zdataHeaderFrame = GenerateZDATAHeaderFrame(offset, crcCalculator);

            // Send initial ZDATA header frame
            SendCommand(zdataHeaderFrame);

            // Response object
            ResponseHeader zdataResponse = null;

            // Set file position to start with
            var initOffset = offset.HasValue ? offset.Value : 0;

            // Start generating data packets.
            for (int i = initOffset; i < src.Length; i += chunkSize)
            {
                if (zdataResponse?.ZHeader == HeaderType.ZACK)
                {

                }

                if (zdataResponse?.ZHeader == HeaderType.ZRINIT)
                {
                    /*
                     * If the receiver cannot overlap serial and disk I/O, it uses the
                     * ZRINIT frame to specify a buffer length which the sender will not
                     * overflow.  The sending program sends a ZCRCW data subpacket and waits
                     * for a ZACK header before sending the next segment of the file. 
                     */
                    var bufferLength = Utils.GetIntFromZModemOffset(
                        zdataResponse.ZP0,
                        zdataResponse.ZP1,
                        zdataResponse.ZP2,
                        zdataResponse.ZP3);
                }

                if (zdataResponse?.ZHeader == HeaderType.ZRPOS)
                {
                    Console.WriteLine($"ZModem stumbled - (╯°□°)╯͡  ┻━┻");
                    Console.WriteLine("We have to revive it - Ȫ_Ȫ");
                }

                var dataSubpacketFrame = GenerateDataSubpacketFrame(crcCalculator, src, i, chunkSize);

                zdataResponse = SendCommand(dataSubpacketFrame, true);

                var progression = (i / (double)src.Length);
                Utils.WriteProgression(progression);
            }
        }

        private static byte[] GenerateDataSubpacketFrame(
            CRC32 crcCalculator, byte[] src, int offset, int chunkSize,
            ZDLESequence? forceZDLESequence = null)
        {
            // Slice data
            var dataSlice = src
                .Skip(offset)
                .Take(chunkSize)
                .ToArray();

            var encodedDataSlice = ZDLEEncoder.EscapeControlCharacters(dataSlice);

            // If a ZDLESequence is forced we use that one.
            // Otherweise we take ZCRCQ and if the frame is the end frame, ZCRCE.
            var requiredSequence = forceZDLESequence.HasValue
                ? forceZDLESequence.Value
                : (offset + dataSlice.Length) < src.Length
                    ? ZDLESequence.ZCRCG
                    : ZDLESequence.ZCRCE;


            // Create data queue for the sliced data.
            var queue = new Queue<byte>(encodedDataSlice);

            // Create data for checksum
            var beforeCTC = dataSlice.Concat(new byte[] { (byte)requiredSequence }).ToArray();

            // Compute hash
            var crc = crcCalculator.ComputeHash(beforeCTC);

            // Add control characters
            queue.Enqueue((byte)ControlBytes.ZDLE);
            queue.Enqueue((byte)requiredSequence);

            var dataSubpacketFrame = queue?.ToArray();

            var encodedCRC = dataSubpacketFrame
                .Concat(ZDLEEncoder.EscapeControlCharacters(crc))
                .ToArray();

            return encodedCRC;
        }

        private byte[] GenerateZDATAHeaderFrame(int? offset, CRC32 crcCalculator)
        {
            // Get offset
            Utils.GenerateZModemFileOffset(
                offset,
                out int p0,
                out int p1,
                out int p2,
                out int p3);

            // Create ZDATA header
            var zdataHeaderQueue = new Queue<byte>();
            var zdataHeaderCommand = Utils.Build32BitDataHeader(HeaderType.ZDATA, p0, p1, p2, p3, crcCalculator);
            foreach (var c in zdataHeaderCommand)
            {
                zdataHeaderQueue.Enqueue((byte)c);
            }

            return zdataHeaderQueue.ToArray();
        }

        private bool SendEOFCommand(int dataLength, CRC16 crcCalculator)
        {
            var result = default(bool);

            Utils.GenerateZModemFileOffset(dataLength, out int p0, out int p1, out int p2, out int p3);

            var zeofCommand = Utils.BuildCommonHexHeader(HeaderType.ZEOF, p0, p1, p2, p3, crcCalculator);

            // Send command
            var response = SendCommand(zeofCommand, true, HeaderType.ZRINIT);

            if (response?.ZHeader == HeaderType.ZRINIT)
            {
                result = true;
            }

            return result;
        }

        private bool SendFinishSession(CRC16 crcCalculator)
        {
            var result = default(bool);

            var zfinCommand = Utils.BuildCommonHexHeader(HeaderType.ZFIN, 0, 0, 0, 0, crcCalculator);

            // Send command
            var response = SendCommand(zfinCommand, true, HeaderType.ZFIN);

            if (response?.ZHeader == HeaderType.ZFIN)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Open serial port if closed and discard out buffer.
        /// </summary>
        private void PrepareSerialPort()
        {
            if (!SerialPort.IsOpen)
            {
                SerialPort.Open();
            }

            // Clear buffers
            SerialPort.DiscardInBuffer();
            SerialPort.DiscardOutBuffer();
        }

        /// <summary>
        /// Close serial port if already open.
        /// </summary>
        private void CloseSerialPortIfOpen()
        {
            if (SerialPort.IsOpen)
            {
                SerialPort.Close();
            }
        }

        /// <summary>
        /// Send HEX string command
        /// </summary>
        /// <param name="command">HEX string</param>
        /// <returns></returns>
        ResponseHeader SendCommand(string command, bool withResponse = false, HeaderType expectedResponse = HeaderType.None)
        {
            var rawCommand = Encoding.ASCII.GetBytes(command);

            return SendCommand(rawCommand, withResponse, expectedResponse);
        }

        /// <summary>
        /// Send byte command
        /// </summary>
        /// <param name="Command">Byte array</param>
        /// <returns></returns>
        ResponseHeader SendCommand(byte[] Command, bool withResponse = false, HeaderType expectedResponse = HeaderType.None)
        {
            ResponseHeader response = default(ResponseHeader);

            for (int i = 0; i < ReadRetryAttempts; i++)
            {
                // Lock and send command
                lock (PadLock)
                {
                    PrepareSerialPort();
                    SerialPort.Write(Command, 0, Command.Length);
                }

                // Delay a bit
                Thread.Sleep(TaskTimeout);

                if (!withResponse)
                {
                    break;
                }

                if (SerialPort.BytesToRead > 0)
                {
                    var buffer = new byte[SerialPort.BytesToRead];

                    lock (PadLock)
                    {
                        SerialPort.Read(buffer, 0, SerialPort.BytesToRead);
                    }

                    response = new ResponseHeader(buffer);

                    if (response?.ZHeader == expectedResponse)
                    {
                        break;
                    }
                    else
                    {
                        // Sleep n ms and try again
                        Thread.Sleep(ReadRetryAttemptTimeout);
                    }
                }
                else if (expectedResponse == HeaderType.None)
                {
                    break;
                }
                else
                {
                    // Sleep n ms and try again
                    Thread.Sleep(ReadRetryAttemptTimeout);
                }
            }

            return response;
        }
    }
}
