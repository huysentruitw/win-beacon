/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

namespace WinBeacon.Stack.Hci.Parameters
{
    internal class BoolCommandParameter : ICommandParameter
    {
        public bool Value { get; private set; }

        public BoolCommandParameter(bool value)
        {
            Value = value;
        }

        public byte[] ToByteArray()
        {
            return new byte[] { (byte)(Value ? 1 : 0) };
        }
    }
}
