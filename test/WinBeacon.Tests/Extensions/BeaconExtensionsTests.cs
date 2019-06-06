/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

using NUnit.Framework;

namespace WinBeacon.Tests.Extensions
{
    [TestFixture]
    public class BeaconExtensionsTests
    {
        [Test]
        public void BeaconExtension_GetRange()
        {
            var beacon = new Beacon("", 0, 0, -39) { Rssi = -52 };
            Assert.AreEqual(4.4668359215096309d, beacon.GetRange());
        }

        [Test]
        public void BeaconExtension_GetApproximateIosRange()
        {
            var beacon = new Beacon("", 0, 0, -39) { Rssi = -52 };
            Assert.AreEqual(8.3781601753285457d, beacon.GetApproximateIosRange());
            beacon.Rssi = 0;
            Assert.AreEqual(-1d, beacon.GetApproximateIosRange());
        }
    }
}