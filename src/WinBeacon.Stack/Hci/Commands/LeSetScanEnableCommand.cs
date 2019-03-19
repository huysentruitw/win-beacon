/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

using WinBeacon.Stack.Hci.Opcodes;
using WinBeacon.Stack.Hci.Parameters;

namespace WinBeacon.Stack.Hci.Commands
{
    internal class LeSetScanEnableCommand : Command
    {
        public LeSetScanEnableCommand(bool enable, bool filterDuplicates)
            : base(OpcodeGroup.LeController, (int)LeControllerOpcode.SetScanEnable)
        {
            Parameters.Add(new BoolCommandParameter(enable));
            Parameters.Add(new BoolCommandParameter(filterDuplicates));
        }
    }
}
