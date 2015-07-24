/*
 * Copyright 2015 Huysentruit Wouter
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
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
        private UsbDevice usbDevice;

        public int Vid { get; private set; }
        public int Pid { get; private set; }

        public LibUsbDevice(int vid, int pid)
        {
            Vid = vid;
            Pid = pid;
        }

        public void Open()
        {
            if (usbDevice != null)
                return;
            usbDevice = UsbDevice.OpenUsbDevice(new UsbDeviceFinder(Vid, Pid));
            if (usbDevice == null)
                throw new WinBeaconException("USB device not found, check VID & PID");
        }

        public void Close()
        {
            if (usbDevice == null)
                return;
            usbDevice.Close();
            usbDevice = null;
        }

        public IEnumerable<UsbConfigInfo> Configs
        {
            get { return usbDevice.Configs; }
        }

        public UsbEndpointReader OpenEndpointReader(ReadEndpointID readEndpointID)
        {
            return usbDevice.OpenEndpointReader(readEndpointID);
        }

        public UsbEndpointWriter OpenEndpointWriter(WriteEndpointID writeEndpointID)
        {
            return usbDevice.OpenEndpointWriter(writeEndpointID);
        }

        public bool ControlTransfer(ref UsbSetupPacket setupPacket, object buffer, int bufferLength, out int lengthTransferred)
        {
            return usbDevice.ControlTransfer(ref setupPacket, buffer, bufferLength, out lengthTransferred);
        }
    }
}
