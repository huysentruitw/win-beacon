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

using System.Collections.Generic;
using NUnit.Framework;
using WinBeacon.Stack.Hci.Events;

namespace WinBeacon.Tests
{
    [TestFixture]
    public class BeaconTests
    {
        private Beacon beacon;

        [TestFixtureSetUp]
        public void Setup()
        {
            beacon = new Beacon("4fe5d5f6-abce-ddfe-1587-123d1a4b567f", 1234, 5678, -48, 0xAABB) { Rssi = -52 };
        }

        [Test]
        public void Beacon_Constructor()
        {
            Assert.AreEqual("4fe5d5f6-abce-ddfe-1587-123d1a4b567f", beacon.Uuid);
            Assert.AreEqual(1234, beacon.Major);
            Assert.AreEqual(5678, beacon.Minor);
            Assert.AreEqual(-48, beacon.CalibratedTxPower);
            Assert.AreEqual(0xAABB, beacon.CompanyId);
        }

        [Test]
        public void Beacon_ToAdvertisingData()
        {
            var data = new byte[] {
                0x02, 0x01, 0x1A, 0x1A, 0xFF, 0xAA, 0xBB, 0x02, 0x15, 0x4F, 0xE5, 0xD5, 0xF6, 0xAB, 0xCE,
                0xDD, 0xFE, 0x15, 0x87, 0x12, 0x3D, 0x1A, 0x4B, 0x56, 0x7F, 0x04, 0xD2, 0x16, 0x2E, 0xD0
            };
            Assert.AreEqual(data, beacon.ToAdvertisingData());
        }

        [Test]
        public void Beacon_Parse()
        {
            var data = new byte[] {
                0x00, 0x00, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12, 0x1E, 0x02, 0x01, 0x1A, 0x1A, 0xFF, 0xAA,
                0xBB, 0x02, 0x15, 0x4F, 0xE5, 0xD5, 0xF6, 0xAB, 0xCE, 0xDD, 0xFE, 0x15, 0x87, 0x12, 0x3D,
                0x1A, 0x4B, 0x56, 0x7F, 0x04, 0xD2, 0x16, 0x2E, 0xD0, 0xCC
            };
            var queue = new Queue<byte>(data);
            var beacon = Beacon.Parse(LeAdvertisingEvent.Parse(queue));
            Assert.AreEqual(new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC }, beacon.Address);
            Assert.AreEqual("4fe5d5f6-abce-ddfe-1587-123d1a4b567f", beacon.Uuid);
            Assert.AreEqual(1234, beacon.Major);
            Assert.AreEqual(5678, beacon.Minor);
            Assert.AreEqual(-48, beacon.CalibratedTxPower);
            Assert.AreEqual(0xAABB, beacon.CompanyId);
            Assert.AreEqual(-52, beacon.Rssi);
            Assert.IsFalse(beacon.IsAppleIBeacon);
        }

        [Test]
        public void Beacon_FullStackParse()
        {
            var data = new byte[] {
                0x00, 0x00, 0x28, 0xEA, 0x6B, 0xB8, 0x5F, 0xD0, 0x1B, 0x1A, 0xFF, 0x4C, 0x00, 0x02, 0x15,
                0xA4, 0x95, 0x00, 0x00, 0xC5, 0xB1, 0x4B, 0x44, 0xB5, 0x12, 0x13, 0x70, 0xF0, 0x2D, 0x74,
                0xDE, 0x00, 0x02, 0x6D, 0x65, 0xC5, 0xBC
            };
            var queue = new Queue<byte>(data);
            var beacon = Beacon.Parse(LeAdvertisingEvent.Parse(queue));
            Assert.NotNull(beacon);
            Assert.AreEqual(0x4C00, beacon.CompanyId);
            Assert.AreEqual(-59, beacon.CalibratedTxPower);
        }
    }
}
