/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

using WinBeacon.Stack.Hci.Opcodes;

namespace WinBeacon.Stack.Hci.Commands
{
    internal class ReadLocalVersionCommand : Command
    {
        public ReadLocalVersionCommand()
            : base(OpcodeGroup.InformationalParameters, (int)InformationalParametersOpcode.ReadLocalVersion)
        {
        }
    }
}
