/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

using WinBeacon.Stack.Hci.Opcodes;
using WinBeacon.Stack.Hci.Parameters;

namespace WinBeacon.Stack.Hci.Commands
{
    internal class LeWriteHostSupportedCommand : Command
    {
        public LeWriteHostSupportedCommand(bool supportedHost, bool simultaneousHost)
            : base(OpcodeGroup.ControllerBaseband, (int)ControllerBasebandOpcode.LeWriteHostSupported)
        {
            Parameters.Add(new BoolCommandParameter(supportedHost));
            Parameters.Add(new BoolCommandParameter(simultaneousHost));
        }
    }
}
