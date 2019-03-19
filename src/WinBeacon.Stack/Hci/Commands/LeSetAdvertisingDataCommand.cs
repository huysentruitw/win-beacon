/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

using System;
using WinBeacon.Stack.Hci.Opcodes;
using WinBeacon.Stack.Hci.Parameters;

namespace WinBeacon.Stack.Hci.Commands
{
    internal class LeSetAdvertisingDataCommand : Command
    {
        public LeSetAdvertisingDataCommand(byte[] data)
            : base(OpcodeGroup.LeController, (int)LeControllerOpcode.SetAdvertisingData)
        {
            if (data.Length > 31)
                throw new ArgumentOutOfRangeException();
            Parameters.Add(new ByteCommandParameter((byte)data.Length));
            Parameters.Add(new ByteArrayCommandParameter(data));
        }
    }
}
