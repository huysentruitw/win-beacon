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
    /// Interface for wrapping LibUsbDotNet.UsbDevice.
    /// </summary>
    internal interface ILibUsbDevice
    {
        int Vid { get; }
        int Pid { get; }
        void Open();
        void Close();
        IEnumerable<UsbConfigInfo> Configs { get; }
        UsbEndpointReader OpenEndpointReader(ReadEndpointID readEndpointID);
        UsbEndpointWriter OpenEndpointWriter(WriteEndpointID writeEndpointID);
        bool ControlTransfer(ref UsbSetupPacket setupPacket, object buffer, int bufferLength, out int lengthTransferred);
    }
}
