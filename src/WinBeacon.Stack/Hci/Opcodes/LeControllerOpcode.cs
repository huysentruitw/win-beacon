/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

namespace WinBeacon.Stack.Hci.Opcodes
{
    internal enum LeControllerOpcode : ushort
    {
        SetEventMask = 0x0001,
        SetAdvertisingParameters = 0x0006, // http://stackoverflow.com/questions/21124993/is-there-a-way-to-increase-ble-advertisement-frequency-in-bluez
        SetAdvertisingData = 0x0008,
        SetAdvertisingEnable = 0x000A,
        SetScanParameters = 0x000B,
        SetScanEnable = 0x000C,
    }
}
