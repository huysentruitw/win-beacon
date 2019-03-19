/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

using System;
using System.Diagnostics;
using WinBeacon.Stack.Hci.Opcodes;
using WinBeacon.Stack.Hci.Parameters;

namespace WinBeacon.Stack.Hci.Commands
{
    internal class LeSetScanParametersCommand : Command
    {
        public LeSetScanParametersCommand(bool activeScanning, ushort intervalInMs, ushort windowInMs, bool useRandomAddress, bool onlyAcceptWhitelistedAdvertisers)
            : base(OpcodeGroup.LeController, (int)LeControllerOpcode.SetScanParameters)
        {
            ushort intervalCode = (ushort)Math.Ceiling(intervalInMs / 0.625);
            ushort windowCode = (ushort)Math.Ceiling(windowInMs / 0.625);
            Debug.Assert(intervalCode >= 0x0004);
            Debug.Assert(intervalCode <= 0x4000);
            Debug.Assert(windowCode >= 0x0004);
            Debug.Assert(windowCode <= 0x4000);
            Parameters.Add(new BoolCommandParameter(activeScanning));
            Parameters.Add(new UshortCommandParameter(intervalCode));
            Parameters.Add(new UshortCommandParameter(windowCode));
            Parameters.Add(new BoolCommandParameter(useRandomAddress));
            Parameters.Add(new BoolCommandParameter(onlyAcceptWhitelistedAdvertisers));
        }
    }
}
