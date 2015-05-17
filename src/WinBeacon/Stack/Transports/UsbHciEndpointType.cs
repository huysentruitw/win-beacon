
namespace WinBeacon.Stack.Transport
{
    /// <summary>
    /// Bluetooth specific USB endpoint types.
    /// </summary>
    internal enum UsbBluetoothEndpointType
    {
        /// <summary>
        /// Command endpoint.
        /// </summary>
        Commands,
        /// <summary>
        /// Event endpoint.
        /// </summary>
        Events,
        /// <summary>
        /// ACL data in endpoint.
        /// </summary>
        AclDataIn,
        /// <summary>
        /// ACL data out endpoint.
        /// </summary>
        AclDataOut
    }
}
