/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

using WinBeacon.Stack.Hci.Opcodes;

namespace WinBeacon.Stack.Hci.Commands
{
    internal class ResetCommand : Command
    {
        public ResetCommand()
            : base(OpcodeGroup.ControllerBaseband, (int)ControllerBasebandOpcode.Reset)
        {
        }
    }
}
