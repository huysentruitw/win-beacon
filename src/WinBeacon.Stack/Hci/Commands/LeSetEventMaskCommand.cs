/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

using WinBeacon.Stack.Hci.Opcodes;
using WinBeacon.Stack.Hci.Parameters;

namespace WinBeacon.Stack.Hci.Commands
{
    internal class LeSetEventMaskCommand : Command
    {
        public LeSetEventMaskCommand(byte[] mask)
            : base(OpcodeGroup.LeController, (int)LeControllerOpcode.SetEventMask)
        {
            Parameters.Add(new ByteArrayCommandParameter(mask));
        }
    }
}
