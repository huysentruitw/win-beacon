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

namespace WinBeacon.Stack.Hci
{
    /// <summary>
    /// The Advertising_Type is used to determine the packet type that is used for advertising when advertising is enabled.
    /// </summary>
    public enum AdvertisingType : byte
    {
        /// <summary>
        /// Connectable undirected advertising (ADV_IND) (default)
        /// </summary>
        ADV_IND = 0x00,
        /// <summary>
        /// Connectable high duty cycle directed advertising (ADV_DIRECT_IND, high duty cycle)
        /// </summary>
        ADV_DIRECT_IND_high = 0x01,
        /// <summary>
        /// Scannable undirected advertising (ADV_SCAN_IND)
        /// </summary>
        ADV_SCAN_IND = 0x02,
        /// <summary>
        /// Non connectable undirected advertising (ADV_NONCONN_IND)
        /// </summary>
        ADV_NONCONN_IND = 0x03,
        /// <summary>
        /// Connectable low duty cycle directed advertising (ADV_DIRECT_IND, low duty cycle)
        /// </summary>
        ADV_DIRECT_IND_low = 0x04
    }

    /// <summary>
    /// Own_Address_Type parameter indicates the type of address being used in the advertising packets.
    /// </summary>
    public enum OwnAddressType : byte
    {
        /// <summary>
        /// Public Device Address (default)
        /// </summary>
        Public = 0x00,
        /// <summary>
        /// Random Device Address
        /// </summary>
        Random = 0x01,
        /// <summary>
        /// Controller generates Resolvable Private Address based on the local IRK from resolving list. If resolving list contains no matching entry, use public address.
        /// </summary>
        ListThanPublic = 0x02,
        /// <summary>
        /// Controller generates Resolvable Private Address based on the local IRK from resolving list. If resolving list contains no matching entry, use random address from LE_Set_Random_Address.
        /// </summary>
        ListThanRandom = 0x03
    }

    public enum PeerAddressType : byte
    {
        /// <summary>
        /// Public Device Address (default) or Public Identity Address
        /// </summary>
        Public = 0x00,
        /// <summary>
        /// Random Device Address or Random (static) Identity Address
        /// </summary>
        Random = 0x01
    }

    /// <summary>
    /// The Advertising_Channel_Map is a bit field that indicates the advertising channels that shall be used when transmitting advertising packets.At least one channel bit shall be set in the Advertising_Channel_Map parameter.
    /// </summary>
    public enum AdvertisingChannelMap : byte
    {
        /// <summary>
        /// Channel 37 shall be used
        /// </summary>
        Channel37 = 0x01,
        /// <summary>
        /// Channel 38 shall be used
        /// </summary>
        Channel38 = 0x02,
        /// <summary>
        /// Channel 39 shall be used
        /// </summary>
        Channel39 = 0x04,
        /// <summary>
        /// Default (all channels enabled)
        /// </summary>
        ChannelAll = 0x07
    }

    /// <summary>
    /// The Advertising_Filter_Policy parameter shall be ignored when directed advertising is enabled.
    /// </summary>
    public enum AdvertisingFilterPolicy : byte
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
}
