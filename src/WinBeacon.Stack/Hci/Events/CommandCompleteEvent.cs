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
