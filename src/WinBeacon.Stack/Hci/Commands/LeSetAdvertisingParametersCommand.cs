/*
 * Copyright 2015-2016 Huysentruit Wouter, Stefan Magerstedt
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
using WinBeacon.Stack.Hci.Opcodes;
using WinBeacon.Stack.Hci.Parameters;

namespace WinBeacon.Stack.Hci.Commands
{
    internal class LeSetAdvertisingParametersCommand : Command
    {
        /// <summary>
        /// Set the advertising parameters
        /// The Advertising_Interval_Min and Advertising_Interval_Max shall not be set to less than 0x00A0 (100 ms) if the Advertising_Type is set to 0x02 (ADV_SCAN_IND) or 0x03 (ADV_NONCONN_IND).
        /// The Host shall not issue this command when advertising is enabled in the Controller; if it is the Command Disallowed error code shall be used.
        /// </summary>
        /// <param name="minimumAdvertisingIntervalInMs">Minimum advertising interval for undirected and low duty cycle directed advertising. Ranges from 20 ms to 10.24 sec. Defaults to 1.28 sec.</param>
        /// <param name="maximumAdvertisingIntervalInMs">Maximum advertising interval for undirected and low duty cycle directed advertising. Ranges from 20 ms to 10.24 sec. Defaults to 1.28 sec.</param>
        /// <param name="advertisingType">Used to determine the packet type that is used for advertising when advertising is enabled.</param>
        /// <param name="ownAdressType">This parameter indicates the type of address being used in the advertising packets.</param>
        /// <param name="peerAdressType">This parameter contains the Peer’s Identity Type.</param>
        /// <param name="peerAddress">This parameter contains the peer’s Identity Address.</param>
        /// <param name="advertisingChannelMap">This is a bit field that indicates the advertising channels that shall be used when transmitting advertising packets.At least one channel bit shall be set in the Advertising_Channel_Map parameter.</param>
        /// <param name="advertisingFilterPolicy">This parameter shall be ignored when directed advertising is enabled.</param>
        public LeSetAdvertisingParametersCommand(
            int minimumAdvertisingIntervalInMs,
            int maximumAdvertisingIntervalInMs,
            AdvertisingType advertisingType,
            OwnAddressType ownAdressType,
            PeerAddressType peerAdressType,
            byte[] peerAddress,
            AdvertisingChannelMap advertisingChannelMap,
            AdvertisingFilterPolicy advertisingFilterPolicy)
            : base(OpcodeGroup.LeController, (int)LeControllerOpcode.SetAdvertisingParameters)
        {
            if (peerAddress == null) throw new ArgumentNullException("peerAddress");
            if (peerAddress.Length != 6) throw new ArgumentOutOfRangeException("peerAddress", "The peer address should consist of exactly 6 bytes.");

            ushort minimumAdvertisingIntervalCode = (ushort)Math.Ceiling(minimumAdvertisingIntervalInMs / 0.625);
            ushort maximumAdvertisingIntervalCode = (ushort)Math.Ceiling(maximumAdvertisingIntervalInMs / 0.625);
            if (advertisingType != AdvertisingType.ConnectableHighDutyCycleDirected)
            {
                if (minimumAdvertisingIntervalCode > 0x4000) throw new ArgumentOutOfRangeException("minimumAdvertisingIntervalCode", "Interval too long, should be less than 10240 ms");
                if (maximumAdvertisingIntervalCode > 0x4000) throw new ArgumentOutOfRangeException("maximumAdvertisingIntervalCode", "Interval too long, should be less than 10240 ms");
                if (advertisingType == AdvertisingType.ScannableUndirectedAdvertising || advertisingType == AdvertisingType.NonConnectableUndirected)
                {
                    if (minimumAdvertisingIntervalCode < 0x00A0) throw new ArgumentOutOfRangeException("minimumAdvertisingIntervalCode", "Interval too short, should not be less than 100 ms");
                    if (maximumAdvertisingIntervalCode < 0x00A0) throw new ArgumentOutOfRangeException("maximumAdvertisingIntervalCode", "Interval too short, should not be less than 100 ms");
                }
                else
                {
                    if (minimumAdvertisingIntervalCode < 0x0020) throw new ArgumentOutOfRangeException("minimumAdvertisingIntervalCode", "Interval too short, should be more than 20 ms");
                    if (maximumAdvertisingIntervalCode < 0x0020) throw new ArgumentOutOfRangeException("maximumAdvertisingIntervalCode", "Interval too short, should be more than 20 ms");
                }
            }

            Parameters.Add(new UshortCommandParameter(minimumAdvertisingIntervalCode));
            Parameters.Add(new UshortCommandParameter(maximumAdvertisingIntervalCode));
            Parameters.Add(new UshortCommandParameter((byte)advertisingType));
            Parameters.Add(new UshortCommandParameter((byte)ownAdressType));
            Parameters.Add(new UshortCommandParameter((byte)peerAdressType));
            Parameters.Add(new ByteArrayCommandParameter(peerAddress));
            Parameters.Add(new ByteCommandParameter((byte)advertisingChannelMap));
            Parameters.Add(new ByteCommandParameter((byte)advertisingFilterPolicy));
        }
    }

    #region Related enums

    /// <summary>
    /// The AdvertisingType is used to determine the packet type during advertising.
    /// </summary>
    internal enum AdvertisingType : byte
    {
        /// <summary>
        /// Connectable undirected advertising (ADV_IND) (default).
        /// </summary>
        ConnectableUndirected = 0x00,
        /// <summary>
        /// Connectable high duty cycle directed advertising (ADV_DIRECT_IND, high duty cycle).
        /// </summary>
        ConnectableHighDutyCycleDirected = 0x01,
        /// <summary>
        /// Scannable undirected advertising (ADV_SCAN_IND).
        /// </summary>
        ScannableUndirectedAdvertising = 0x02,
        /// <summary>
        /// Non connectable undirected advertising (ADV_NONCONN_IND).
        /// </summary>
        NonConnectableUndirected = 0x03,
        /// <summary>
        /// Connectable low duty cycle directed advertising (ADV_DIRECT_IND, low duty cycle).
        /// </summary>
        ConnectableLowDutyCycleDirected = 0x04
    }

    /// <summary>
    /// OwnAddressType parameter indicates the type of address being used in the advertising packets.
    /// </summary>
    internal enum OwnAddressType : byte
    {
        /// <summary>
        /// Public Device Address (default).
        /// </summary>
        Public = 0x00,
        /// <summary>
        /// Random Device Address.
        /// </summary>
        Random = 0x01,
        /// <summary>
        /// Controller generates Resolvable Private Address based on the local IRK from resolving list. If resolving list contains no matching entry, use public address.
        /// </summary>
        ResolveFromListOrPublic = 0x02,
        /// <summary>
        /// Controller generates Resolvable Private Address based on the local IRK from resolving list. If resolving list contains no matching entry, use random address from LE_Set_Random_Address.
        /// </summary>
        ResolveFromListOrRandom = 0x03
    }

    internal enum PeerAddressType : byte
    {
        /// <summary>
        /// Public Device Address (default) or Public Identity Address.
        /// </summary>
        Public = 0x00,
        /// <summary>
        /// Random Device Address or Random (static) Identity Address.
        /// </summary>
        Random = 0x01
    }

    /// <summary>
    /// The AdvertisingChannelMap is a flags field that indicates the advertising channels that shall be used when transmitting advertising packets.
    /// </summary>
    [Flags]
    internal enum AdvertisingChannelMap : byte
    {
        /// <summary>
        /// Channel 37 shall be used.
        /// </summary>
        UseChannel37 = 0x01,
        /// <summary>
        /// Channel 38 shall be used.
        /// </summary>
        UseChannel38 = 0x02,
        /// <summary>
        /// Channel 39 shall be used.
        /// </summary>
        UseChannel39 = 0x04,
        /// <summary>
        /// Default (all channels enabled).
        /// </summary>
        UseAllChannels = UseChannel37 | UseChannel38 | UseChannel39
    }

    internal enum AdvertisingFilterPolicy : byte
    {
        /// <summary>
        /// Process scan and connection requests from all devices (i.e., the White List is not in use) (default).
        /// </summary>
        ConnectAllScanAll = 0x00,
        /// <summary>
        /// Process connection requests from all devices and only scan requests from devices that are in the White List.
        /// </summary>
        ConnectAllScanWhiteList = 0x01,
        /// <summary>
        /// Process scan requests from all devices and only connection requests from devices that are in the White List..
        /// </summary>
        ConnectWhiteListScanAll = 0x02,
        /// <summary>
        /// Process scan and connection requests only from devices in the White List.
        /// </summary>
        ConnectWhiteListScanWhiteList = 0x03
    }

    #endregion
}
