using System;

namespace WinBeacon.Stack.Hci
{
    /// <summary>
    /// Event arguments for the DataReceived event.
    /// </summary>
    internal class DataReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// The received data.
        /// </summary>
        public byte[] Data { get; private set; }
        /// <summary>
        /// The type of the received data.
        /// </summary>
        public DataType DataType { get; private set; }

        /// <summary>
        /// Constructs a new DataReceivedEventArgs instance.
        /// </summary>
        /// <param name="data">The received data.</param>
        /// <param name="dataType">The type of the received data.</param>
        public DataReceivedEventArgs(byte[] data, DataType dataType)
        {
            Data = data;
            DataType = dataType;
        }
    }
}
