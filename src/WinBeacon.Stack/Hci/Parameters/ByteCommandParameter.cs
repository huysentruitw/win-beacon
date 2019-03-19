/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

namespace WinBeacon.Stack.Hci.Parameters
{
    internal class ByteCommandParameter : ICommandParameter
    {
        public byte Value { get; private set; }

        public ByteCommandParameter(byte value)
        {
            Value = value;
        }

        public byte[] ToByteArray()
        {
            return new byte[] { Value };
        }
    }
}
