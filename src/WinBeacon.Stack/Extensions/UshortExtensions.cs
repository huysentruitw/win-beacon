/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

namespace WinBeacon.Stack
{
    /// <summary>
    /// ushort extension methods.
    /// </summary>
    internal static class UshortExtensions
    {
        /// <summary>
        /// Get the least significant byte.
        /// </summary>
        /// <param name="value">The ushort.</param>
        /// <returns>The least significant byte.</returns>
        public static byte LoByte(this ushort value)
        {
            return (byte)(value & 0xFF);
        }

        /// <summary>
        /// Get the most significant byte.
        /// </summary>
        /// <param name="value">The ushort.</param>
        /// <returns>The most significant byte.</returns>
        public static byte HiByte(this ushort value)
        {
            return (byte)(value >> 8);
        }
    }
}
