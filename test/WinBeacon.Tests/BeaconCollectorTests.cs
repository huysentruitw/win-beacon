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

using Moq;
using NUnit.Framework;
using System;
using System.Threading;

namespace WinBeacon.Tests
{
    [TestFixture]
    public class BeaconCollectorTests
    {
        private Mock<IBeaconHub> beaconHubMock;

        [Test]
        public void BeaconInRangeEvent_OneBeaconInRange_ShouldTriggerEvent()
        {
            var beacon = new Beacon("UUID", 1, 2, -50);
            BeaconEventArgs eventArgs = null;
            using (var collector = new BeaconCollector(beaconHubMock.Object))
            {
                collector.BeaconInRange += (sender, e) => eventArgs = e;
                beaconHubMock.Raise(x => x.BeaconDetected += null, new BeaconEventArgs(beacon));
                Assert.AreEqual(beacon, eventArgs.Beacon);
            }
        }

        [Test]
        public void BeaconInRangeEvent_TwoBeaconsInRange_ShouldTriggerEventTwice()
        {
            int index = 0;
            var beacons = new[]
            {
                new Beacon("UUID", 1, 1, -50),
                new Beacon("UUID", 2, 2, -50)
            };
            using (var collector = new BeaconCollector(beaconHubMock.Object))
            {
                collector.BeaconInRange += (sender, e) =>
                    {
                        Assert.AreEqual(beacons[index++], e.Beacon);
                    };
                beaconHubMock.Raise(x => x.BeaconDetected += null, new BeaconEventArgs(beacons[0]));
                beaconHubMock.Raise(x => x.BeaconDetected += null, new BeaconEventArgs(beacons[1]));
                Assert.AreEqual(2, index);
            }
        }

        [Test]
        public void BeaconInRangeEvent_OneBeaconAdvertisesThreeTimes_ShouldTriggerEventOnlyOnce()
        {
            int count = 0;
            var beacon = new Beacon("UUID", 1, 2, -50);
            using (var collector = new BeaconCollector(beaconHubMock.Object))
            {
                collector.BeaconInRange += (sender, e) => count++;
                beaconHubMock.Raise(x => x.BeaconDetected += null, new BeaconEventArgs(beacon));
                beaconHubMock.Raise(x => x.BeaconDetected += null, new BeaconEventArgs(beacon));
                beaconHubMock.Raise(x => x.BeaconDetected += null, new BeaconEventArgs(beacon));
                Assert.AreEqual(1, count);
            }
        }

        [Test]
        public void BeaconInRangeEvent_OneBeaconOutOfRangeBackInRange_ShouldTriggerEventTwice()
        {
            int count = 0;
            var beacon = new Beacon("UUID", 1, 2, -50);
            using (var collector = new BeaconCollector(beaconHubMock.Object, TimeSpan.FromSeconds(1)))
            {
                collector.BeaconInRange += (sender, e) =>
                {
                    Assert.AreEqual(beacon, e.Beacon);
                    count++;
                };
                beaconHubMock.Raise(x => x.BeaconDetected += null, new BeaconEventArgs(beacon));
                Thread.Sleep(1100);
                beaconHubMock.Raise(x => x.BeaconDetected += null, new BeaconEventArgs(beacon));
                Assert.AreEqual(2, count);
            }
        }

        [Test]
        public void BeaconOutOfRangeEvent_OneBeaconOutOfRange_ShouldTriggerEvent()
        {
            var beacon = new Beacon("UUID", 1, 2, -50);
            BeaconEventArgs eventArgs = null;
            using (var collector = new BeaconCollector(beaconHubMock.Object, TimeSpan.FromSeconds(1)))
            {
                collector.BeaconOutOfRange += (sender, e) => eventArgs = e;
                beaconHubMock.Raise(x => x.BeaconDetected += null, new BeaconEventArgs(beacon));
                Thread.Sleep(1100);
                Assert.AreEqual(beacon, eventArgs.Beacon);
            }
        }

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            beaconHubMock = new Mock<IBeaconHub>();
        }
    }
}
