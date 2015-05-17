using System;
using WinBeacon.Stack.Hci;

namespace WinBeacon.Stack
{
    /// <summary>
    /// Interface for classes that enable a transport link with the Bluetooth dongle.
    /// </summary>
    internal interface ITransport : IDisposable
    {
        /// <summary>
        /// Open the transport link.
        /// </summary>
        /// <returns>True on success, false on failure.</returns>
        void Open();

        /// <summary>
        /// Close the transport link.
        /// </summary>
        void Close();

        /// <summary>
        /// Send data to the dongle.
        /// </summary>
        void Send(byte[] data, DataType dataType);
        
        /// <summary>
        /// Event triggered on data reception from the dongle.
        /// </summary>
        event EventHandler<DataReceivedEventArgs> DataReceived;
    }
}
