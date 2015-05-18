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
using System.Text;

namespace WinBeacon.Stack.Hci.Events
{
    /// <summary>
    /// Low energy meta event.
    /// </summary>
    public class LeMetaEvent : Event
    {
        /// <summary>
        /// Low energy meta sub event.
        /// </summary>
        public enum LeMetaSubEvent : byte
        {
            /// <summary>
            /// Connection complete event.
            /// </summary>
            ConnectionComplete = 0x01,
            /// <summary>
            /// Advertising report event.
            /// </summary>
            AdvertisingReport = 0x02
        }

        /// <summary>
        /// The sub event of this meta event.
        /// </summary>
        public LeMetaSubEvent SubEvent { get; private set; }
        /// <summary>
        /// Advertising events included in this meta event.
        /// </summary>
        public LeAdvertisingEvent[] AdvertisingEvents { get; private set; }

        /// <summary>
        /// Creates a string representation of this object.
        /// </summary>
        /// <returns>String representation.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("SubEvent = {0}", SubEvent);
            if (AdvertisingEvents != null)
                foreach (var e in AdvertisingEvents)
                    sb.AppendFormat(", {0}", e);
            return sb.ToString();
        }

        internal static LeMetaEvent Parse(EventCode code, Queue<byte> data)
        {
            var subEvent = (LeMetaSubEvent)data.Dequeue();
            LeAdvertisingEvent[] events = null;
            switch (subEvent)
            {
                case LeMetaSubEvent.ConnectionComplete:
                    // Not implemented
                    break;
                case LeMetaSubEvent.AdvertisingReport:
                    events = ParseAdvertisingReport(data);
                    break;
            }
            return new LeMetaEvent
                {
                    Code = code,
                    SubEvent = subEvent,
                    AdvertisingEvents = events
                };
        }

        private static LeAdvertisingEvent[] ParseAdvertisingReport(Queue<byte> data)
        {
            int numberOfEvents = data.Dequeue();
            var events = new LeAdvertisingEvent[numberOfEvents];
            for (int i = 0; i < numberOfEvents; i++)
                events[i] = LeAdvertisingEvent.Parse(data);
            return events;
        }
    }
}
