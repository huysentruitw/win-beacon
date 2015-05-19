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

namespace WinBeacon.Stack
{
    /// <summary>
    /// ushort extension methods.
    /// </summary>
    public static class UshortExtensions
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
