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

namespace WinBeacon
{
    /// <summary>
    /// Class that represents a Beacon.
    /// </summary>
    public class Beacon : IComparable
    {
        /// <summary>
        /// When true, RangeInMeters will be calculated using an approximation of the iOS range calculation
        /// instead of a simple linear ratio calculation. Default is true.
        /// </summary>
        public static bool UseApproximateIosRangeCalculation = true;

        /// <summary>
        /// UUID of the beacon.
        /// </summary>
        public string Uuid { get; internal set; }
        /// <summary>
        /// Bluetooth MAC-address of the beacon.
        /// </summary>
        public byte[] Address { get; internal set; }
        /// <summary>
        /// Major number of the beacon.
        /// </summary>
        public ushort Major { get; internal set; }
        /// <summary>
        /// Minor number of the beacon.
        /// </summary>
        public ushort Minor { get; internal set; }
        /// <summary>
        /// RSSI power of the beacon in dB.
        /// </summary>
        public sbyte Rssi { get; internal set; }
        /// <summary>
        /// Calibrated TX power of the beacon in dB.
        /// </summary>
        public sbyte CalibratedTxPower { get; internal set; }
        /// <summary>
        /// True if the beacon is an (emulation of) Apple iBeacon.
        /// </summary>
        public bool IsAppleIBeacon { get; internal set; }

        /// <summary>
        /// The estimated range in meters.
        /// </summary>
        public double RangeInMeters
        {
            get
            {
                return UseApproximateIosRangeCalculation
                    ? BeaconHelpers.GetApproximateIosRange(Rssi, CalibratedTxPower)
                    : BeaconHelpers.GetRange(Rssi, CalibratedTxPower);
            }
        }

        /// <summary>
        /// Returns a string representation of the beacon data.
        /// </summary>
        /// <returns>String representation of the beacon data.</returns>
        public override string ToString()
        {
            return string.Format("UUID: {0}, Address: {1}, Major: {2}, Minor: {3}, Range: {4:0.0}m, RSSI: {5}, TxPower: {6}dB, IsAppleIBeacon: {7}",
                Uuid, BitConverter.ToString(Address), Major, Minor, RangeInMeters, Rssi, CalibratedTxPower, IsAppleIBeacon);
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
    }
}
