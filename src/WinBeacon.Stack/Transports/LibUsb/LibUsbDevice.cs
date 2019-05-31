/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

using System.Collections.Generic;
using LibUsbDotNet;
using LibUsbDotNet.Info;
using LibUsbDotNet.Main;

namespace WinBeacon.Stack.Transports.LibUsb
{
    /// <summary>
    /// LibUsbDotNet.UsbDevice wrapper that implements ILibUsbDevice.
    /// </summary>
    internal class LibUsbDevice : ILibUsbDevice
    {
        private UsbDevice _usbDevice;

        public int Vid { get; private set; }
        public int Pid { get; private set; }

        public LibUsbDevice(int vid, int pid)
        {
            Vid = vid;
            Pid = pid;
        }

        public void Open()
        {
            if (_usbDevice != null)
                return;
            _usbDevice = UsbDevice.OpenUsbDevice(new UsbDeviceFinder(Vid, Pid))
                ?? throw new WinBeaconException("USB device not found, check VID & PID");
        }

        public void Close()
        {
            if (_usbDevice == null)
                return;
            _usbDevice.Close();
            _usbDevice = null;
        }

        public IEnumerable<UsbConfigInfo> Configs => _usbDevice.Configs;

        public UsbEndpointReader OpenEndpointReader(ReadEndpointID readEndpointID)
        {
            return _usbDevice.OpenEndpointReader(readEndpointID);
        }

        public UsbEndpointWriter OpenEndpointWriter(WriteEndpointID writeEndpointID)
        {
            return _usbDevice.OpenEndpointWriter(writeEndpointID);
        }

        public bool ControlTransfer(ref UsbSetupPacket setupPacket, object buffer, int bufferLength, out int lengthTransferred)
        {
            return _usbDevice.ControlTransfer(ref setupPacket, buffer, bufferLength, out lengthTransferred);
        }
    }
}
