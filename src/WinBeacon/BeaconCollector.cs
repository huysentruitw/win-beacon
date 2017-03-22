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
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace WinBeacon
{
    /// <summary>
    /// Collector that keeps a list of beacons currently seen in range.
    /// </summary>
    public class BeaconCollector : IDisposable
    {
        private static readonly TimeSpan CleanupInterval = TimeSpan.FromSeconds(1);
        private IBeaconHub hub;
        private TimeSpan expiresAfter;
        private List<CollectorItem> items = new List<CollectorItem>();
        private Timer cleanupTimer;

        private class CollectorItem : IEquatable<CollectorItem>
        {
            public DateTime ExpiresAt { get; set; }
            public Beacon Beacon { get; set; }

            /// <summary>
            /// Only compares the Beacons UUID, Major and Minor values.
            /// </summary>
            /// <param name="other">The other item.</param>
            /// <returns>True if both Beacon UUID, Major and Minor values are the same.</returns>
            public bool Equals(CollectorItem other)
            {
                return Beacon.CompareTo(other.Beacon) == 0;
            }
        }

        /// <summary>
        /// Creates a new collector instance.
        /// </summary>
        /// <param name="hub">The hub.</param>
        /// <param name="expiresAfter">The time after which a beacon expires and treated as out-of-range. Defaults to 10 seconds.</param>
        public BeaconCollector(IBeaconHub hub, TimeSpan? expiresAfter = null)
        {
            this.hub = hub;
            this.expiresAfter = expiresAfter ?? TimeSpan.FromSeconds(10);
            cleanupTimer = new Timer(CleanupTimerCallback, null, CleanupInterval, CleanupInterval);
            hub.BeaconDetected += BeaconDetected;
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~BeaconCollector()
        {
            Dispose();
        }

        /// <summary>
        /// Release used resources.
        /// </summary>
        public void Dispose()
        {
            hub.BeaconDetected -= BeaconDetected;
            cleanupTimer.Change(Timeout.Infinite, Timeout.Infinite);
            cleanupTimer.Dispose();
        }

        /// <summary>
        /// Event fired when a new beacon is detected in range.
        /// </summary>
        public event EventHandler<BeaconEventArgs> BeaconInRange;
        private void OnBeaconInRange(Beacon beacon)
        {
            if (BeaconInRange != null)
                BeaconInRange(this, new BeaconEventArgs(beacon));
        }

        /// <summary>
        /// Event fired when a beacon is out-of-range, which means that no signal from the beacon was received within the expiresAfter timeout.
        /// </summary>
        public event EventHandler<BeaconEventArgs> BeaconOutOfRange;
        private void OnBeaconOutOfRange(Beacon beacon)
        {
            if (BeaconOutOfRange != null)
                BeaconOutOfRange(this, new BeaconEventArgs(beacon));
        }

        private void BeaconDetected(object sender, BeaconEventArgs e)
        {
            bool removed;
            var item = new CollectorItem
            {
                Beacon = e.Beacon,
                ExpiresAt = DateTime.Now + expiresAfter
            };
            lock (((ICollection)items).SyncRoot)
            {
                removed = items.Remove(item);
                items.Add(item);
            }
            if (!removed)
                OnBeaconInRange(e.Beacon);
        }

        private void CleanupTimerCallback(object state)
        {
            var now = DateTime.Now;
            var outOfRangeBeacons = new List<Beacon>();
            lock (((ICollection)items).SyncRoot)
            {
                while (items.Count > 0)
                {
                    var item = items[0];
                    if (item.ExpiresAt > now)
                        break;
                    items.RemoveAt(0);
                    outOfRangeBeacons.Add(item.Beacon);
                }
            }
            foreach (var beacon in outOfRangeBeacons)
                OnBeaconOutOfRange(beacon);
        }
    }
}
