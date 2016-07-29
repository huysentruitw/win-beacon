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
using WinBeacon.Stack.Hci;

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
        /// Store the Advertising Parameters. They will be used when EnableAdvertising() is being called.
        /// </summary>
        /// <param name="advertisingIntervalMinInMs">Minimum advertising interval for undirected and low duty cycle directed advertising. Range: 20-10240  Default: 1280 = 1.28 seconds</param>
        /// <param name="advertisingIntervalMaxInMs">Maximum advertising interval for undirected and low duty cycle directed advertising. Range: 20-10240  Default: 1280 = 1.28 seconds</param>
        /// <param name="advertisingType">The Advertising_Type is used to determine the packet type that is used for advertising when advertising is enabled.</param>
        /// <param name="ownAdressType">Own_Address_Type parameter indicates the type of address being used in the advertising packets.</param>
        /// <param name="peerAdressType">The Peer_Address_Type parameter contains the Peer’s Identity Type</param>
        /// <param name="peerAddress">The Peer_Address parameter contains the peer’s Identity Address</param>
        /// <param name="advertisingChannelMap">The Advertising_Channel_Map is a bit field that indicates the advertising channels that shall be used when transmitting advertising packets.At least one channel bit shall be set in the Advertising_Channel_Map parameter.</param>
        /// <param name="advertisingFilterPolicy">The Advertising_Filter_Policy parameter shall be ignored when directed advertising is enabled.</param>
        void StoreAdvertisingParameters(ushort advertisingIntervalMinInMs, ushort advertisingIntervalMaxInMs, AdvertisingType advertisingType, OwnAddressType ownAdressType, PeerAddressType peerAdressType, string peerAddress, AdvertisingChannelMap advertisingChannelMap, AdvertisingFilterPolicy advertisingFilterPolicy);

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
