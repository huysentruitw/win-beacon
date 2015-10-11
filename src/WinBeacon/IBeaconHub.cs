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
    /// Interface for hubs that detect beacons and can advertise as a beacon.
    /// </summary>
    public interface IBeaconHub : IDisposable
    {
        /// <summary>
        /// Enable advertising as a beacon.
        /// </summary>
        /// <param name="beacon">The beacon to emulate.</param>
        void EnableAdvertising(Beacon beacon);

        /// <summary>
        /// Disable advertising as a beacon.
        /// </summary>
        void DisableAdvertising();

        /// <summary>
        /// Event fired when a beacon is detected. This happens when the dongle receives the beacon's advertising packet.
        /// </summary>
        event EventHandler<BeaconEventArgs> BeaconDetected;
    }
}
