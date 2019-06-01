/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

using System;

namespace WinBeacon.Stack
{
    /// <summary>
    /// WinBeacon specific exception.
    /// </summary>
    public class WinBeaconException : Exception
    {
        /// <summary>
        /// Creates a new exception containing a formatted message.
        /// </summary>
        /// <param name="message">The message.</param>
        public WinBeaconException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new exception containing a message and inner-exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner-exception.</param>
        public WinBeaconException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
