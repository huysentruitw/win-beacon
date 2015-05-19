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

namespace WinBeacon.Stack.Transport
{
    /// <summary>
    /// USB endpoint info specific for Bluetooth usage.
    /// </summary>
    internal class UsbBluetoothEndpointInfo
    {
        /// <summary>
        /// The Bluetooth endpoint type.
        /// </summary>
        public UsbBluetoothEndpointType Type { get; private set; }
        /// <summary>
        /// The USB endpoint identifier.
        /// </summary>
        public byte Id { get; private set; }

        /// <summary>
        /// Creates a new UsbBluetoothEndpoint instance.
        /// </summary>
        /// <param name="type">The Bluetooth endpoint type.</param>
        /// <param name="id">The USB endpoint identifier.</param>
        public UsbBluetoothEndpointInfo(UsbBluetoothEndpointType type, byte id)
        {
            Type = type;
            Id = id;
        }
    }
}
