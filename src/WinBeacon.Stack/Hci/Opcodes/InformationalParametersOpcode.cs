/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

namespace WinBeacon.Stack.Hci.Opcodes
{
    internal enum InformationalParametersOpcode : ushort
    {
        ReadLocalVersion = 0x0001,
        ReadBdAddr = 0x0009
    }
}
