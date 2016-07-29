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
using System.Diagnostics;
using WinBeacon.Stack.Hci.Opcodes;
using WinBeacon.Stack.Hci.Parameters;
using System.Linq;
using System.Text.RegularExpressions;

namespace WinBeacon.Stack.Hci.Commands
{
    internal class LeSetAdvertisingParameters : Command
    {
        /// <summary>
        /// Set the advertising parameters
        /// The Advertising_Interval_Min and Advertising_Interval_Max shall not be set to less than 0x00A0 (100 ms) if the Advertising_Type is set to 0x02 (ADV_SCAN_IND) or 0x03 (ADV_NONCONN_IND).
        /// The Host shall not issue this command when advertising is enabled in the Controller; if it is the Command Disallowed error code shall be used.
        /// </summary>
        /// <param name="advertisingIntervalMinInMs">Minimum advertising interval for undirected and low duty cycle directed advertising. Range: 20-10240  Default: 1280 = 1.28 seconds</param>
        /// <param name="advertisingIntervalMaxInMs">Maximum advertising interval for undirected and low duty cycle directed advertising. Range: 20-10240  Default: 1280 = 1.28 seconds</param>
        /// <param name="advertisingType">The Advertising_Type is used to determine the packet type that is used for advertising when advertising is enabled.</param>
        /// <param name="ownAdressType">Own_Address_Type parameter indicates the type of address being used in the advertising packets.</param>
        /// <param name="peerAdressType">The Peer_Address_Type parameter contains the Peer’s Identity Type</param>
        /// <param name="peerAddress">The Peer_Address parameter contains the peer’s Identity Address</param>
        /// <param name="advertisingChannelMap">The Advertising_Channel_Map is a bit field that indicates the advertising channels that shall be used when transmitting advertising packets.At least one channel bit shall be set in the Advertising_Channel_Map parameter.</param>
        /// <param name="advertisingFilterPolicy">The Advertising_Filter_Policy parameter shall be ignored when directed advertising is enabled.</param>
        public LeSetAdvertisingParameters(ushort advertisingIntervalMinInMs, ushort advertisingIntervalMaxInMs, AdvertisingType advertisingType, OwnAddressType ownAdressType, PeerAddressType peerAdressType, string peerAddress, AdvertisingChannelMap advertisingChannelMap, AdvertisingFilterPolicy advertisingFilterPolicy)
            : base(OpcodeGroup.LeController, (int)LeControllerOpcode.SetAdvertisingParameters)
        {
            ushort advertisingIntervalMin = (ushort)Math.Ceiling(advertisingIntervalMinInMs / 0.625);
            ushort advertisingIntervalMax = (ushort)Math.Ceiling(advertisingIntervalMaxInMs / 0.625);
            Debug.Assert(advertisingIntervalMin >= 0x0020);
            Debug.Assert(advertisingIntervalMin <= 0x4000);
            Debug.Assert(advertisingIntervalMax >= 0x0020);
            Debug.Assert(advertisingIntervalMax <= 0x4000);
            Parameters.Add(new UshortCommandParameter(advertisingIntervalMin));
            Parameters.Add(new UshortCommandParameter(advertisingIntervalMax));
            Parameters.Add(new UshortCommandParameter((byte)advertisingType));
            Parameters.Add(new UshortCommandParameter((byte)ownAdressType));
            Parameters.Add(new UshortCommandParameter((byte)peerAdressType));
            Parameters.Add(new ByteArrayCommandParameter((from Match m in Regex.Matches(peerAddress, @"[0-9a-f]{2}", RegexOptions.IgnoreCase) select Convert.ToByte(m.Value, 16)).ToArray()));
            Parameters.Add(new ByteCommandParameter((byte)advertisingChannelMap));
            Parameters.Add(new ByteCommandParameter((byte)advertisingFilterPolicy));
        }
    }
    
}
