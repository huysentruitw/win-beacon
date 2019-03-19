/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

using WinBeacon.Stack.Hci.Opcodes;
using WinBeacon.Stack.Hci.Parameters;

namespace WinBeacon.Stack.Hci.Commands
{
    internal class SetEventMaskCommand : Command
    {
        public SetEventMaskCommand(byte[] mask)
            : base(OpcodeGroup.ControllerBaseband, (int)ControllerBasebandOpcode.SetEventMask)
        {
            Parameters.Add(new ByteArrayCommandParameter(mask));
        }
    }
}
