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
using System.Linq;
using System.Text.RegularExpressions;
using WinBeacon.Stack.Hci.Events;

namespace WinBeacon
{
    internal static class BeaconHelpers
    {
        public static double GetRange(sbyte rssi, sbyte calibratedTxPower)
        {
            double dbRatio = calibratedTxPower - rssi;
            double linearRatio = Math.Pow(10, dbRatio / 10);
            return Math.Sqrt(linearRatio);
        }

        public static double GetApproximateIosRange(sbyte rssi, sbyte calibratedTxPower)
        {
            if (rssi == 0)
                return -1;
            var ratio = (double)rssi / (double)calibratedTxPower;
            if (ratio < 1.0)
                return Math.Pow(ratio, 10);
            return 0.89979 * Math.Pow(ratio, 7.7095) + 0.111;
        }

        public static Beacon Parse(LeAdvertisingEvent e)
        {
            if (e.EventType == LeAdvertisingEventType.ScanRsp)
                return null;

            if (e.Payload.Length < 9)
                return null;

            int offset = 0;
            byte ad1Length = e.Payload[offset++];
            byte ad1Type = e.Payload[offset++];
            byte flags = e.Payload[offset++];
            byte ad2Length = e.Payload[offset++];
            byte ad2Type = e.Payload[offset++];
            ushort companyId = (ushort)(e.Payload[offset++] + (e.Payload[offset++] << 8));
            byte b0advInd = e.Payload[offset++];
            byte b1advInd = e.Payload[offset++];

            if (ad1Length != 2 || ad1Type != 0x01 || ad2Length < 26 || ad2Type != 0xFF)
                return null;

            byte[] uuid = new byte[16];
            Array.Copy(e.Payload, offset, uuid, 0, uuid.Length);
            offset += uuid.Length;
            ushort major = (ushort)((e.Payload[offset++] << 8) + e.Payload[offset++]);
            ushort minor = (ushort)((e.Payload[offset++] << 8) + e.Payload[offset++]);
            sbyte txPower = (sbyte)e.Payload[offset++];

            return new Beacon()
            {
                Uuid = ToLittleEndianFormattedUuidString(uuid),
                Address = e.Address,
                Major = major,
                Minor = minor,
                Rssi = e.Rssi,
                CalibratedTxPower = txPower,
                IsAppleIBeacon = companyId == 0x004C
            };
        }

        public static byte[] CreateAdvertisingData(string uuid, ushort major, ushort minor, sbyte calibratedTxPower)
        {
            var result = new List<byte>();
            result.AddRange(new byte[] { 0x02, 0x01, 0x04, 0x1B, 0xFF, 0x4C, 0x00, 0x02, 0x15 }); // 9 header bytes
            result.AddRange(FromLittleEndianFormattedUuidString(uuid));
            result.Add((byte)(major >> 8));
            result.Add((byte)(major & 0xFF));
            result.Add((byte)(minor >> 8));
            result.Add((byte)(minor & 0xFF));
            result.Add((byte)calibratedTxPower);
            return result.ToArray();
        }

        private static string ToLittleEndianFormattedUuidString(byte[] uuid)
        {
            var digits = new Queue<char>(BitConverter.ToString(uuid).Replace("-", "").ToLower());
            return new string((from c in "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" select c == '-' ? '-' : digits.Dequeue()).ToArray());
        }

        private static byte[] FromLittleEndianFormattedUuidString(string uuid)
        {
            if (!Regex.IsMatch(uuid, @"^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$", RegexOptions.IgnoreCase))
                throw new FormatException("Incorrect uuid format");
            return (from Match m in Regex.Matches(uuid, @"[0-9a-f]{2}", RegexOptions.IgnoreCase) select Convert.ToByte(m.Value, 16)).ToArray();
        }
    }
}
