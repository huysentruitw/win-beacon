/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

using WinBeacon.Stack.Hci.Opcodes;

namespace WinBeacon.Stack.Hci.Commands
{
    internal class LeReadHostSupportedCommand : Command
    {
        public LeReadHostSupportedCommand()
            : base(OpcodeGroup.ControllerBaseband, (int)ControllerBasebandOpcode.LeReadHostSupported)
        {
        }
    }
}
