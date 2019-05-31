/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
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
