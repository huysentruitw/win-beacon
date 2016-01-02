/*
 * Copyright 2015-2016 Huysentruit Wouter
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
using System.Linq;
using System.Text.RegularExpressions;

namespace WinBeacon
{
    /// <summary>
    /// Beacon extension methods.
    /// </summary>
    public static class BeaconExtensions
    {
        /// <summary>
        /// Gets the calculated range in meters.
        /// </summary>
        /// <param name="beacon">The beacon instance.</param>
        /// <returns>The calculated range in meters.</returns>
        public static double GetRange(this Beacon beacon)
        {
            double dbRatio = beacon.CalibratedTxPower - beacon.Rssi;
            double linearRatio = Math.Pow(10, dbRatio / 10);
            return Math.Sqrt(linearRatio);
        }

        /// <summary>
        /// Gets the calculated range in meters using a curve that approximates the iOS ranging.
        /// </summary>
        /// <param name="beacon">The beacon instance.</param>
        /// <returns>The calculated range in meters.</returns>
        public static double GetApproximateIosRange(this Beacon beacon)
        {
            if (beacon.Rssi == 0)
                return -1;
            var ratio = (double)beacon.Rssi / (double)beacon.CalibratedTxPower;
            if (ratio < 1.0)
                return Math.Pow(ratio, 10);
            return 0.89979 * Math.Pow(ratio, 7.7095) + 0.111;
        }

        internal static string ToLittleEndianFormattedUuidString(this byte[] uuid)
        {
            var digits = new Queue<char>(BitConverter.ToString(uuid).Replace("-", "").ToLower());
            return new string((from c in "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" select c == '-' ? '-' : digits.Dequeue()).ToArray());
        }

        internal static byte[] FromLittleEndianFormattedUuidString(this string uuid)
        {
            if (!Regex.IsMatch(uuid, @"^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$", RegexOptions.IgnoreCase))
                throw new FormatException("Incorrect uuid format");
            return (from Match m in Regex.Matches(uuid, @"[0-9a-f]{2}", RegexOptions.IgnoreCase) select Convert.ToByte(m.Value, 16)).ToArray();
        }
    }
}
