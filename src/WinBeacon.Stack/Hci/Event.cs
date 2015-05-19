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

using System.Collections.Generic;
using WinBeacon.Stack.Hci.Events;

namespace WinBeacon.Stack.Hci
{
    /// <summary>
    /// Base class for Bluetooth events.
    /// </summary>
    public class Event
    {
        /// <summary>
        /// The event code.
        /// </summary>
        public EventCode Code { get; protected set; }

        /// <summary>
        /// Creates an event object.
        /// </summary>
        protected Event()
        {
        }

        /// <summary>
        /// Creates an event object with event code.
        /// </summary>
        protected Event(EventCode code)
        {
            Code = code;
        }

        internal static Event Parse(byte[] data)
        {
            return Parse(new Queue<byte>(data));
        }

        internal static Event Parse(Queue<byte> data)
        {
            if (data.Count < 2)
                return null;
            EventCode code = (EventCode)data.Dequeue();
            int payloadSize = data.Dequeue();
            switch (code)
            {
                case EventCode.CommandComplete:
                    return CommandCompleteEvent.Parse(code, data);
                case EventCode.LeMeta:
                    return LeMetaEvent.Parse(code, data);
                default:
                    return new Event(code);
            }
        }
    }
}
