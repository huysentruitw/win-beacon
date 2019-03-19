/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

namespace WinBeacon.Stack.Hci
{
    internal interface ICommandParameter
    {
        byte[] ToByteArray();
    }
}
