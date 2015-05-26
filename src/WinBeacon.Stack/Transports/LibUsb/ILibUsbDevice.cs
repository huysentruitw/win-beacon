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

using System;
using System.Collections.ObjectModel;
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
        ReadOnlyCollection<UsbConfigInfo> Configs { get; }
        UsbEndpointReader OpenEndpointReader(ReadEndpointID readEndpointID);
        UsbEndpointWriter OpenEndpointWriter(WriteEndpointID writeEndpointID);
        bool ControlTransfer(ref UsbSetupPacket setupPacket, object buffer, int bufferLength, out int lengthTransferred);
    }
}
