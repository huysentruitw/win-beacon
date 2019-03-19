/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

namespace WinBeacon.Stack.Hci.Events
{
    /// <summary>
    /// Low Energy Advertising event types
    /// </summary>
    public enum LeAdvertisingEventType : byte
    {
        /// <summary>
        /// Connectable undirected advertising
        /// </summary>
        Ind = 0x00,
        /// <summary>
        /// Connectable directed advertising event.
        /// </summary>
        DirectInd = 0x01,
        /// <summary>
        /// Scannable undirected advertising event. 
        /// </summary>
        ScanInd = 0x02,
        /// <summary>
        /// Non-connectable and non-scannable advertisement packets.
        /// </summary>
        NonConnInd = 0x03,
        /// <summary>
        /// Scan response.
        /// </summary>
        ScanRsp = 0x04
    }
}
