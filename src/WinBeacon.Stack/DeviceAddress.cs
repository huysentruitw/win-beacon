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
