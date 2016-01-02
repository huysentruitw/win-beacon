/*
 * Copyright 2015-2016 Huysentruit Wouter
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
