/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinBeacon.Stack.Hci.Events
{
    /// <summary>
    /// Low energy advertising event.
    /// </summary>
    public class LeAdvertisingEvent
    {
        /// <summary>
        /// The event type.
        /// </summary>
        public LeAdvertisingEventType EventType { get; private set; }
        /// <summary>
        /// The address type.
        /// </summary>
        public byte AddressType { get; private set; }
        /// <summary>
        /// The address.
        /// </summary>
        public byte[] Address { get; private set; }
        /// <summary>
        /// The payload.
        /// </summary>
        public byte[] Payload { get; private set; }
        /// <summary>
        /// The RSSI.
        /// </summary>
        public sbyte Rssi { get; private set; }

        /// <summary>
        /// Creates a string representation of this object.
        /// </summary>
        /// <returns>String representation.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("EventType: {0}", EventType);
            sb.AppendFormat("AddressType: {0}", AddressType);
            sb.AppendFormat("Address: {0}", BitConverter.ToString(Address));
            sb.AppendFormat("Payload = {0}", BitConverter.ToString(Payload).Replace("-", ""));
            sb.AppendFormat("RSSI = {0}", Rssi);
            return sb.ToString();
        }

        internal static LeAdvertisingEvent Parse(Queue<byte> data)
        {
            return new LeAdvertisingEvent
                {
                    EventType = (LeAdvertisingEventType)data.Dequeue(),
                    AddressType = data.Dequeue(),
                    Address = data.Dequeue(6).Reverse().ToArray(),
                    Payload = data.Dequeue(data.Dequeue()),
                    Rssi = (sbyte)data.Dequeue()
                };
        }
    }
}
