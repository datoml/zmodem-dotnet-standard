<br />
<div style="text-align:center">
  <img alt="ZModemNet" src="./docs/assets/logo_cropped.png">
</div>

<br />

<div align="center">
  <strong>
  Native ZModem implementation for dotnet projects
  </strong>
  <br />
</div>

<br />

This repo contains a ZModem specification implementation into a dotnet standard 2.0 library.

For now, only a upload to a remote device is implemented.

### Example

```c#
// Create serial port
var serialPort = new SerialPort("COMx", 115200, Parity.None, 8, StopBits.One);

// Create transfer instance
var zTransfer = new Transfer(serialPort);

// Upload file to device
zTransfer.Upload("..path_to_your_file..");
```
