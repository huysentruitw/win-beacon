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
using WinBeacon.Stack;
using WinBeacon.Stack.Hci.Events;

namespace WinBeacon
{
    /// <summary>
    /// Class that represents a Beacon.
    /// </summary>
    public class Beacon : IComparable
    {
        private const int AppleCompanyId = 0x4C00;
        /// <summary>
        /// UUID of the beacon.
        /// </summary>
        public string Uuid { get; private set; }
        /// <summary>
        /// Bluetooth MAC-address of the beacon.
        /// </summary>
        public byte[] Address { get; private set; }
        /// <summary>
        /// Major number of the beacon.
        /// </summary>
        public int Major { get; private set; }
        /// <summary>
        /// Minor number of the beacon.
        /// </summary>
        public int Minor { get; private set; }
        /// <summary>
        /// RSSI power of the beacon in dB.
        /// </summary>
        public int Rssi { get; internal set; }
        /// <summary>
        /// Calibrated TX power of the beacon in dB.
        /// </summary>
        public int CalibratedTxPower { get; private set; }
        /// <summary>
        /// CompanyId of the beacon (0x4C00 for Apple iBeacon).
        /// </summary>
        public int CompanyId { get; private set; }
        /// <summary>
        /// True if the beacon is an (emulation of) Apple iBeacon.
        /// </summary>
        public bool IsAppleIBeacon
        {
            get { return CompanyId == AppleCompanyId; }
        }

        private Beacon()
        {
        }

        /// <summary>
        /// Creates a beacon.
        /// </summary>
        /// <param name="uuid">UUID of the beacon.</param>
        /// <param name="major">Major number of the beacon.</param>
        /// <param name="minor">Minor number of the beacon.</param>
        /// <param name="companyId">CompanyId, defaults to comapny id of Apple (0x4C00).</param>
        /// <param name="calibratedTxPower">Calibrated TX power of the beacon in dB.</param>
        public Beacon(string uuid, int major, int minor, int calibratedTxPower, int companyId = AppleCompanyId)
        {
            Uuid = uuid;
            Major = major;
            Minor = minor;
            CalibratedTxPower = calibratedTxPower;
            CompanyId = companyId;
        }

        /// <summary>
        /// Compare this beacon to an other instance.
        /// </summary>
        /// <param name="obj">The other instance.</param>
        /// <returns>A value that indicates the lexical relationship between the two comparands.</returns>
        public int CompareTo(object obj)
        {
            var other = obj as Beacon;
            if (other == null)
                throw new ArgumentException("Must be of type Beacon", "obj");
            return Uuid.NullableCompareTo(other.Uuid)
                ?? Major.NullableCompareTo(other.Major)
                ?? Minor.CompareTo(other.Minor);
        }

        /// <summary>
        /// Returns a string representation of the beacon data.
        /// </summary>
        /// <returns>String representation of the beacon data.</returns>
        public override string ToString()
        {
            return string.Format("UUID: {0}, Address: {1}, Major: {2}, Minor: {3}, RSSI: {4}, TxPower: {5}dB, CompanyId: 0x{6:X}, Distance: {7:0.00}m",
                Uuid, BitConverter.ToString(Address), Major, Minor, Rssi, CalibratedTxPower, CompanyId, this.GetRange());
        }

        /// <summary>
        /// Parse low energy advertising event to a beacon instance.
        /// </summary>
        /// <param name="e">The event.</param>
        /// <returns>The beacon or null in case of failure.</returns>
        internal static Beacon Parse(LeAdvertisingEvent e)
        {
            if (e.EventType == LeAdvertisingEventType.ScanRsp)
                return null;
            if (e.Payload.Length < 9)
                return null;
            var payload = new Queue<byte>(e.Payload);
            var ad1Length = payload.Dequeue();
            var ad1Type = payload.Dequeue();
            var flags = payload.Dequeue();
            var ad2Length = payload.Dequeue();
            var ad2Type = payload.Dequeue();
            var companyId = (ushort)((payload.Dequeue() << 8) + payload.Dequeue());
            var b0advInd = payload.Dequeue();
            var b1advInd = payload.Dequeue();
            if (ad1Length != 2 || ad1Type != 0x01 || ad2Length < 26 || ad2Type != 0xFF)
                return null;
            var uuid = payload.Dequeue(16);
            var major = (ushort)((payload.Dequeue() << 8) + payload.Dequeue());
            var minor = (ushort)((payload.Dequeue() << 8) + payload.Dequeue());
            var txPower = (sbyte)payload.Dequeue();
            return new Beacon
            {
                Uuid = uuid.ToLittleEndianFormattedUuidString(),
                Address = e.Address,
                Major = major,
                Minor = minor,
                Rssi = e.Rssi,
                CalibratedTxPower = txPower,
                CompanyId = companyId
            };
        }

        internal byte[] ToAdvertisingData()
        {
            var result = new List<byte>();
            result.AddRange(new byte[]
            {
                0x02,   // ad1Length
                0x01,   // ad1Type
                0x1A,   // flags
                0x1A,   // ad2Length
                0xFF,   // ad2Type
                (byte)(CompanyId >> 8),
                (byte)(CompanyId & 0xFF),
                0x02,   // b0advInd
                0x15    // b1advInd
            });
            result.AddRange(Uuid.FromLittleEndianFormattedUuidString());
            result.AddRange(new byte[]
            {
                (byte)(Major >> 8),
                (byte)(Major & 0xFF),
                (byte)(Minor >> 8),
                (byte)(Minor & 0xFF),
                (byte)CalibratedTxPower
            });
            return result.ToArray();
        }
    }
}
