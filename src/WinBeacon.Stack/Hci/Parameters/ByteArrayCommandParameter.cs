/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

namespace WinBeacon.Stack.Hci.Parameters
{
    internal class ByteArrayCommandParameter : ICommandParameter
    {
        public byte[] Value { get; private set; }

        public ByteArrayCommandParameter(byte[] value)
        {
            Value = value;
        }

        public byte[] ToByteArray()
        {
            return Value;
        }
    }
}
