/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

namespace WinBeacon.Stack.Hci
{
    internal enum OpcodeGroup : byte
    {
        LinkControl = 0x01,
        LinkPolicy = 0x02,
        ControllerBaseband = 0x03,
        InformationalParameters = 0x04,
        StatusParameters = 0x05,
        LeController = 0x08,
        Vendor = 0x3F
    }
}
