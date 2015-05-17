using System;

namespace WinBeacon
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
        /// <param name="args">Replaces one or more format items in the specified message.</param>
        public WinBeaconException(string message, params object[] args)
            : base(string.Format(message, args))
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
