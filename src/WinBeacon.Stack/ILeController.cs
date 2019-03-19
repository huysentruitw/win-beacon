/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

using System;

namespace WinBeacon.Stack
{
    /// <summary>
    /// Interface for Bluetooth Low Energy controller classes.
    /// </summary>
    public interface ILeController : IDisposable
    {
        /// <summary>
        /// Open underlying transport connection.
        /// </summary>
        void Open();

        /// <summary>
        /// Close underlying transport connection.
        /// </summary>
        void Close();

        /// <summary>
        /// Enable Low Energy device scanning.
        /// </summary>
        void EnableScanning();

        /// <summary>
        /// Disable Low Energy device scanning.
        /// </summary>
        void DisableScanning();

        /// <summary>
        /// Enable Low Energy advertising.
        /// </summary>
        /// <param name="advertisementData">The advertisement data.</param>
        /// <param name="advertisingIntervalInMs">Interval should be between 20 and 10240 ms. Defaults to 1280 ms.</param>
        void EnableAdvertising(byte[] advertisementData, int advertisingIntervalInMs = 1280);
        
        /// <summary>
        /// Disable Low Energy advertising.
        /// </summary>
        void DisableAdvertising();
        
        /// <summary>
        /// Fired for each received Low Energy meta event.
        /// </summary>
        event EventHandler<LeMetaEventReceivedEventArgs> LeMetaEventReceived;
    }
}
