using System;
using System.IO.Ports;
using ZModem;

namespace ZModemConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var portNames = SerialPort.GetPortNames();

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Available ports:");

            for (int i = 0; i < portNames.Length; i++)
            {
                var portName = $"{i}: {portNames[i]}";
                Console.WriteLine(portName);
            }

            Console.Write("Selected port ID: ");
            var selectedID = Console.ReadLine();

            if (int.TryParse(selectedID, out int portID))
            {
                try
                {
                    var portName = portNames[portID];
                    var boudRate = 9600;
                    var parity = Parity.None;
                    var dataBits = 8;
                    var stopBits = StopBits.One;

                    var createInfo = $"Using {portName} with BoudRate: {boudRate}, Parity: {parity}, Databits: {dataBits}, StopBits: {stopBits}";
                    Console.WriteLine(createInfo);

                    var serialPort = new SerialPort(portName, boudRate, parity, dataBits, stopBits);

                    var zTransfer = new Transfer(serialPort, true);
                    zTransfer.Upload("sample.txt");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else
            {
                Console.WriteLine($"Port ID {selectedID} not found. Exiting...");
            }

            Console.ReadKey();
        }
    }
}
