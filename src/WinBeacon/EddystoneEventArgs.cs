/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

using System;

namespace WinBeacon
{
    /// <summary>
    /// Event arguments for Eddystone events.
    /// </summary>
    public class EddystoneEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the detected beacon.
        /// </summary>
        public Eddystone Eddystone { get; private set; }

        internal EddystoneEventArgs(Eddystone eddystone)
        {
            Eddystone = eddystone;
        }
    }
}
