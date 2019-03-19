/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

namespace WinBeacon.Stack.Hci.Opcodes
{
    internal enum ControllerBasebandOpcode : ushort
    {
        SetEventMask = 0x0001,
        Reset = 0x0003,
        LeReadHostSupported = 0x006C,
        LeWriteHostSupported = 0x006D
    }
}
