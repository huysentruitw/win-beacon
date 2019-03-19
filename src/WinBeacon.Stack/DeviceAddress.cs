/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

using System;

namespace WinBeacon.Stack
{
    /// <summary>
    /// Class that holds a 48-bit device address.
    /// </summary>
    public class DeviceAddress
    {
        /// <summary>
        /// The device address.
        /// </summary>
        public byte[] Address { get; private set; }

        /// <summary>
        /// Empty device address.
        /// </summary>
        public static DeviceAddress Empty
        {
            get
            {
                return new DeviceAddress(new byte[6]);
            }
        }

        /// <summary>
        /// Creates a new <see cref="DeviceAddress"/> instance.
        /// </summary>
        /// <param name="address"></param>
        public DeviceAddress(byte[] address)
        {
            if (address == null)
                throw new ArgumentNullException("address");
            if (address.Length != 6)
                throw new ArgumentOutOfRangeException("address", "Invalid length, should contain 6 bytes");
            Address = address;
        }
    }
}
