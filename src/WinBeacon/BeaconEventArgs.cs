/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

using System;

namespace WinBeacon
{
    /// <summary>
    /// Event arguments for Beacon events.
    /// </summary>
    public class BeaconEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the detected beacon.
        /// </summary>
        public Beacon Beacon { get; private set; }

        internal BeaconEventArgs(Beacon beacon)
        {
            Beacon = beacon;
        }
    }
}
