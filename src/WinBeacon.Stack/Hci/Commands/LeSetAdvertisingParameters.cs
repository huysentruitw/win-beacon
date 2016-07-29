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
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

/*
0x00 Connectable undirected advertising (ADV_IND) (default)
0x01 Connectable high duty cycle directed advertising (ADV_DIRECT_IND, high duty cycle)
0x02 Scannable undirected advertising(ADV_SCAN_IND)
0x03 Non connectable undirected advertising(ADV_NONCONN_IND)
*/

namespace WinBeacon.Stack.Hci.Commands
{
    internal class LeSetAdvertisingParameters : Command
    {
        /// <summary>
        /// Set the advertising parameters
        /// </summary>
        /// <param name="advertisingIntervalMinInMs">Minimum advertising interval for undirected and low duty cycle directed advertising. Range: 20-10240  Default: 1280 = 1.28 seconds</param>
        /// <param name="advertisingIntervalMaxInMs">Maximum advertising interval for undirected and low duty cycle directed advertising. Range: 20-10240  Default: 1280 = 1.28 seconds</param>
        /// <param name="advertisingType">The Advertising_Type is used to determine the packet type that is used for advertising when advertising is enabled.</param>
        /// <param name="ownAdressType"></param>
        /// <param name="peerAdressType"></param>
        /// <param name="peerAddress"></param>
        /// <param name="advertisingChannelMap"></param>
        /// <param name="advertisingFilterPolicy"></param>
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
            Parameters.Add(new ByteArrayCommandParameter((from Match m in Regex.Matches(peerAddress, @"[0-9a-f]{2}", RegexOptions.IgnoreCase) select Convert.ToByte(m.Value, 16)).ToArray()));
            Parameters.Add(new ByteCommandParameter((byte)advertisingChannelMap));
            Parameters.Add(new ByteCommandParameter((byte)advertisingFilterPolicy));
        }
    }
}
