/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
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
        /// <param name="advertisingInterval">The advertising interval. Interval should be between 20 ms and 10.24 seconds. Defaults to 1.28 seconds.</param>
        void EnableAdvertising(Beacon beacon, TimeSpan? advertisingInterval = null);

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
