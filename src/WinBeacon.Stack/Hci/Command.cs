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

using System;
using System.Collections.Generic;
using WinBeacon.Stack.Hci.Events;

namespace WinBeacon.Stack.Hci
{
    internal class Command
    {
        public ushort Opcode { get; private set; }
        public List<ICommandParameter> Parameters { get; private set; }

        internal Command(OpcodeGroup opcodeGroup, int opcodeCommand)
        {
            Opcode = (ushort)(opcodeCommand | ((byte)opcodeGroup << 10));
            Parameters = new List<ICommandParameter>();
        }

        public byte[] ToByteArray()
        {
            var result = new List<byte>();
            result.Add(Opcode.LoByte());
            result.Add(Opcode.HiByte());
            result.Add(0);  // Length placeholder
            Parameters.ForEach(param => result.AddRange(param.ToByteArray()));
            result[2] = (byte)(result.Count - 3);
            return result.ToArray();
        }

        internal Action<Command, CommandCompleteEvent> CommandCompleteCallback = null;

        internal virtual void OnCommandComplete(CommandCompleteEvent e)
        {
            if (CommandCompleteCallback != null)
                CommandCompleteCallback(this, e);
        }
    }

    internal abstract class Command<TResult> : Command
    {
        internal Command(OpcodeGroup opcodeGroup, int opcodeCommand)
            : base(opcodeGroup, opcodeCommand)
        {
        }

        internal abstract TResult ParseCommandResult(CommandCompleteEvent e);

        internal new Action<Command, TResult> CommandCompleteCallback = null;

        internal override void OnCommandComplete(CommandCompleteEvent e)
        {
            if (CommandCompleteCallback != null)
                CommandCompleteCallback(this, ParseCommandResult(e));
        }
    }
}
