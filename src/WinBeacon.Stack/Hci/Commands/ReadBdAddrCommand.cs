/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

using System.Linq;
using WinBeacon.Stack.Hci.Opcodes;

namespace WinBeacon.Stack.Hci.Commands
{
    internal class ReadBdAddrCommand : Command<DeviceAddress>
    {
        public ReadBdAddrCommand()
            : base(OpcodeGroup.InformationalParameters, (int)InformationalParametersOpcode.ReadBdAddr)
        {
        }

        internal override DeviceAddress ParseCommandResult(Events.CommandCompleteEvent e)
        {
            if (e.ResultData == null || e.ResultData.Length != 6)
                return DeviceAddress.Empty;
            return new DeviceAddress(e.ResultData.Reverse().ToArray());
        }
    }
}
