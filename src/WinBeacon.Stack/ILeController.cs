/*
 * Copyright 2015-2016 Huysentruit Wouter
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
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
        void EnableAdvertising(byte[] advertisementData);
        
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
