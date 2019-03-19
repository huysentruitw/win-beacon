/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

using System;
using System.Collections.Generic;

namespace WinBeacon.Stack.Hci.Events
{
    /// <summary>
    /// Event that occurs when a command has been executed.
    /// </summary>
    internal class CommandCompleteEvent : Event
    {
        public int NumberOfCommandsAllowedToSend { get; private set; }
        public ushort CommandOpcode { get; private set; }
        public byte CommandParameterDataLength { get; private set; }
        public byte[] ResultData { get; private set; }

        public static CommandCompleteEvent Parse(EventCode code, Queue<byte> data)
        {
            if (data.Count < 4)
                return null;
            return new CommandCompleteEvent
                {
                    Code = code,
                    NumberOfCommandsAllowedToSend = data.Dequeue(),
                    CommandOpcode = (ushort)(data.Dequeue() + (data.Dequeue() << 8)),
                    CommandParameterDataLength = data.Dequeue(),
                    ResultData = data.DequeueAll()
                };
        }
    }
}
