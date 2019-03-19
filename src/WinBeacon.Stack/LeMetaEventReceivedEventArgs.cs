/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

using System;
using WinBeacon.Stack.Hci.Events;

namespace WinBeacon.Stack
{
    /// <summary>
    /// Event arguments for the LeMetaEventReceived event.
    /// </summary>
    public class LeMetaEventReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Received LeMetaEvent.
        /// </summary>
        public LeMetaEvent LeMetaEvent { get; private set; }

        internal LeMetaEventReceivedEventArgs(LeMetaEvent leMetaEvent)
        {
            LeMetaEvent = leMetaEvent;
        }
    }
}
