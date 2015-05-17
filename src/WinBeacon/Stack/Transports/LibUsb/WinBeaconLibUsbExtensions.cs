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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LibUsbDotNet;
using LibUsbDotNet.Main;
using WinBeacon.Stack.Transport;

namespace WinBeacon.Stack.Transports.LibUsb
{
    internal static class WinBeaconLibUsbExtensions
    {
        public static IEnumerable<UsbBluetoothEndpointInfo> EnumerateBluetoothEndpointInfo(this UsbDevice usbDevice)
        {
            var config0 = usbDevice.Configs.FirstOrDefault();
            if (config0 == null)
                throw new WinBeaconException("USB device has no configurations");
 
            var interface0Info = config0.InterfaceInfoList.FirstOrDefault();
            if (interface0Info == null)
                throw new WinBeaconException("USB configuration does not contain an interface");

            foreach (var endpointInfo in interface0Info.EndpointInfoList)
            {
                switch (endpointInfo.Descriptor.Attributes & 0x03)
                {
                    case 0x02:
                        yield return new UsbBluetoothEndpointInfo(
                            type: (endpointInfo.Descriptor.EndpointID & 0x80) == 0x80 ? UsbBluetoothEndpointType.AclDataIn : UsbBluetoothEndpointType.AclDataOut,
                            id: endpointInfo.Descriptor.EndpointID
                        );
                        break;
                    case 0x03:
                        yield return new UsbBluetoothEndpointInfo(
                            type: UsbBluetoothEndpointType.Events,
                            id: endpointInfo.Descriptor.EndpointID
                        );
                        break;
                }
            }
        }

        public static void SubscribeForDataReceived(this UsbEndpointBase endpoint, Action<byte[]> handler)
        {
            Debug.Assert(endpoint is UsbEndpointReader);
            (endpoint as UsbEndpointReader).SubscribeForDataReceived(handler);
        }

        public static void SubscribeForDataReceived(this UsbEndpointReader endpointReader, Action<byte[]> handler)
        {
            endpointReader.DataReceived += (sender, e) =>
                {
                    var data = new byte[e.Count];
                    if (e.Count > 0)
                        Buffer.BlockCopy(e.Buffer, 0, data, 0, e.Count);
                    handler(data);
                };
            endpointReader.DataReceivedEnabled = true;
        }
    }
}
