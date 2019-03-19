/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

using System;

namespace WinBeacon.Stack
{
    /// <summary>
    /// Event arguments for the DeviceAddressReceived event.
    /// </summary>
    public class DeviceAddressReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Received device address.
        /// </summary>
        public DeviceAddress DeviceAddress { get; private set; }

        internal DeviceAddressReceivedEventArgs(DeviceAddress deviceAddress)
        {
            DeviceAddress = deviceAddress;
        }
    }
}
