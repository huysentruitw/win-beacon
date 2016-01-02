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

namespace WinBeacon.Stack.Hci
{
    /// <summary>
    /// Low Energy event codes
    /// </summary>
    public enum EventCode : byte
    {
        /// <summary>
        /// Inquiry complete.
        /// </summary>
        InquiryComplete = 0x01,
        /// <summary>
        /// Inquiry result.
        /// </summary>
        InquiryResult = 0x02,
        /// <summary>
        /// Connection complete.
        /// </summary>
        ConnectionComplete = 0x03,
        /// <summary>
        /// Connection request.
        /// </summary>
        ConnectionRequest = 0x04,
        /// <summary>
        /// Disconnection complete.
        /// </summary>
        DisconnectionComplete = 0x05,
        /// <summary>
        /// Authentication complete.
        /// </summary>
        AuthenticationComplete = 0x06,
        /// <summary>
        /// Remote name request complete.
        /// </summary>
        RemoteNameRequestComplete = 0x07,
        /// <summary>
        /// Encryption change.
        /// </summary>
        EncryptionChange = 0x08,
        /// <summary>
        /// Change connection link key complete.
        /// </summary>
        ChangeConnectionLinkKeyComplete = 0x09,
        /// <summary>
        /// Master link key complete.
        /// </summary>
        MasterLinkKeyComplete = 0x0A,
        /// <summary>
        /// Read remote supported features complete.
        /// </summary>
        ReadRemoteSupportedFeaturesComplete = 0x0B,
        /// <summary>
        /// Read remote version information complete.
        /// </summary>
        ReadRemoteVersionInformationComplete = 0x0C,
        /// <summary>
        /// Quality of service setup complete.
        /// </summary>
        QosSetupComplete = 0x0D,
        /// <summary>
        /// Command complete.
        /// </summary>
        CommandComplete = 0x0E,
        /// <summary>
        /// Command status.
        /// </summary>
        CommandStatus = 0x0F,
        /// <summary>
        /// Hardware error.
        /// </summary>
        HardwareError = 0x10,
        /// <summary>
        /// Flush occurred.
        /// </summary>
        FlushOccurred = 0x11,
        /// <summary>
        /// Role changed.
        /// </summary>
        RoleChanged = 0x12,
        /// <summary>
        /// Number of completed packets.
        /// </summary>
        NumberOfCompletedPackets = 0x13,
        /// <summary>
        /// Mode change.
        /// </summary>
        ModeChange = 0x14,
        /// <summary>
        /// Return link keys.
        /// </summary>
        ReturnLinkKeys = 0x15,
        /// <summary>
        /// Pin code request.
        /// </summary>
        PinCodeRequest = 0x16,
        /// <summary>
        /// Link key request.
        /// </summary>
        LinkKeyRequest = 0x17,
        /// <summary>
        /// Link key notification.
        /// </summary>
        LinkKeyNotification = 0x18,
        /// <summary>
        /// Loopback command.
        /// </summary>
        LoopbackCommand = 0x19,
        /// <summary>
        /// Data buffer overflow.
        /// </summary>
        DataBufferOverflow = 0x1A,
        /// <summary>
        /// Max slots change.
        /// </summary>
        MaxSlotsChange = 0x1B,
        /// <summary>
        /// Read clock offset complete.
        /// </summary>
        ReadClockOffsetComplete = 0x1C,
        /// <summary>
        /// Connection packet type changed.
        /// </summary>
        ConnectionPacketTypeChanged = 0x1D,
        /// <summary>
        /// Quality of service violation.
        /// </summary>
        QosViolation = 0x1E,
        /// <summary>
        /// Page scan mode change.
        /// </summary>
        PageScanModeChange = 0x1F,
        /// <summary>
        /// Page scan repetition mode change.
        /// </summary>
        PageScanRepetitionModeChange = 0x20,
        /// <summary>
        /// HCI flow specification complete.
        /// </summary>
        HciFlowSpecificationComplete = 0x21,
        /// <summary>
        /// Inquiry result with RSSI.
        /// </summary>
        InquiryResultWithRssi = 0x22,
        /// <summary>
        /// Read remote extended features complete.
        /// </summary>
        ReadRemoteExtendedFeaturesComplete = 0x23,
        /// <summary>
        /// Synchronous connection complete.
        /// </summary>
        SynchronousConnectionComplete = 0x2C,
        /// <summary>
        /// Synchronous connection changed.
        /// </summary>
        SynchronousConnectionChanged = 0x2D,
        /// <summary>
        /// Low energy meta event.
        /// </summary>
        LeMeta = 0x3E,
        /// <summary>
        /// Reserved for Bluetooth logo testing.
        /// </summary>
        ReservedBluetoothLogoTesting = 0xFE,
        /// <summary>
        /// Vendor specific events.
        /// </summary>
        VendorSpecificEvents = 0xFF
    }
}
