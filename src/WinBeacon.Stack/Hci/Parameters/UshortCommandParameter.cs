/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

namespace WinBeacon.Stack.Hci.Parameters
{
    internal class UshortCommandParameter : ICommandParameter
    {
        public ushort Value { get; private set; }

        public UshortCommandParameter(ushort value)
        {
            Value = value;
        }

        public byte[] ToByteArray()
        {
            return new byte[] { (byte)(Value & 0xFF), (byte)(Value >> 8) };
        }
    }
}
