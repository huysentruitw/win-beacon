/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

using WinBeacon.Stack.Hci.Opcodes;
using WinBeacon.Stack.Hci.Parameters;

namespace WinBeacon.Stack.Hci.Commands
{
    internal class LeSetAdvertisingEnableCommand : Command
    {
        public LeSetAdvertisingEnableCommand(bool enable)
            : base(OpcodeGroup.LeController, (int)LeControllerOpcode.SetAdvertisingEnable)
        {
            Parameters.Add(new BoolCommandParameter(enable));
        }
    }
}
