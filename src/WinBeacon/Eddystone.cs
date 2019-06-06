/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

using System;
using System.Collections.Generic;
using WinBeacon.Stack;
using WinBeacon.Stack.Hci.Events;

namespace WinBeacon
{
    /// <summary>
    /// Abstract base class that represents an Eddystone Beacon.
    /// </summary>
    public abstract class Eddystone
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        protected Eddystone()
        {
        }

        /// <summary>
        /// Parses an incoming <see cref="LeAdvertisingEvent"/> for Eddystone frames.
        /// </summary>
        /// <param name="e">The incoming event frame.</param>
        /// <returns>An <see cref="Eddystone"/> instance or null in case the event could not be parsed as an Eddystone event.</returns>
        /// <remarks>Implemented according to https://github.com/google/eddystone/blob/master/protocol-specification.md </remarks>
        internal static Eddystone Parse(LeAdvertisingEvent e)
        {
            if (e.EventType == LeAdvertisingEventType.ScanRsp)
                return null;

            var payload = new Queue<byte>(e.Payload);

            // Skip flags
            var flagsLength = payload.Dequeue();
            payload.Dequeue(flagsLength);

            // Extract and verify Eddystone UUID
            // The Complete List of 16-bit Service UUIDs as defined in The Bluetooth Core Specification Supplement (CSS) v5, Part A, § 1.1.
            // The Complete List of 16-bit Service UUIDs must contain the Eddystone Service UUID of 0xFEAA. This is included to allow background scanning on iOS devices.
            var eddystoneUuidLength = payload.Dequeue();
            if (eddystoneUuidLength != 3)
                return null;
            payload.Dequeue();
            var eddystoneUuid = (ushort)((payload.Dequeue() << 8) + payload.Dequeue());
            if (eddystoneUuid != 0xAAFE)
                return null;

            // Extract service data
            // The Service Data data type, Ibid., § 1.11.
            // The Service Data - 16 bit UUID data type must be the Eddystone Service UUID of 0xFEAA
            var serviceDataLength = payload.Dequeue();
            var serviceDataTypeValue = payload.Dequeue();
            eddystoneUuid = (ushort)((payload.Dequeue() << 8) + payload.Dequeue());
            if (eddystoneUuid != 0xAAFE)
                return null;
            var eddystoneFrameType = (EddystoneFrameType)payload.Dequeue();
            switch (eddystoneFrameType)
            {
                case EddystoneFrameType.Uid:
                    return EddystoneUid.ParseFrame(payload, e);
                case EddystoneFrameType.Url:
                    return EddystoneUrl.ParseFrame(payload, e);
                case EddystoneFrameType.Tlm:
                    return EddystoneTlm.ParseFrame(payload, e);
                case EddystoneFrameType.Eid:
                    return EddystoneEid.ParseFrame(payload, e);
            }

            return null;
        }
    }

    internal enum EddystoneFrameType : byte
    {
        Uid = 0x00,
        Url = 0x10,
        Tlm = 0x20,
        Eid = 0x30,
    }

    /// <summary>
    /// Class that represents an Eddystone Beacon sending Eddystone-UID frames.
    /// </summary>
    public sealed class EddystoneUid : Eddystone
    {
        /// <summary>
        /// Bluetooth MAC-address of the beacon.
        /// </summary>
        public byte[] Address { get; private set; }

        /// <summary>
        /// The 10-byte namespace.
        /// </summary>
        public byte[] Namespace { get; private set; }

        /// <summary>
        /// The 6-byte instance.
        /// </summary>
        public byte[] Instance { get; private set; }

        /// <summary>
        /// Calibrated TX power of the beacon in dB at 0m.
        /// </summary>
        public int CalibratedTxPower { get; private set; }

        /// <summary>
        /// RSSI power of the beacon in dB.
        /// </summary>
        public int Rssi { get; internal set; }

        internal static EddystoneUid ParseFrame(Queue<byte> payload, LeAdvertisingEvent e)
        {
            if (payload.Count != 19)
                return null;

            return new EddystoneUid
            {
                Address = e.Address,
                CalibratedTxPower = (sbyte)payload.Dequeue(),
                Namespace = payload.Dequeue(10),
                Instance = payload.Dequeue(6),
                Rssi = e.Rssi,
            };
        }

        /// <summary>
        /// Returns a string representation of the Eddystone UID data.
        /// </summary>
        /// <returns>String representation of the Eddystone UID data.</returns>
        public override string ToString()
            => $"Namespace: {BitConverter.ToString(Namespace)}, Instance: {BitConverter.ToString(Instance)}, TxPower: {CalibratedTxPower}dB";

    }

    /// <summary>
    /// Class that represents an Eddystone Beacon sending Eddystone-URL frames.
    /// </summary>
    public sealed class EddystoneUrl : Eddystone
    {
        internal static EddystoneUrl ParseFrame(Queue<byte> payload, LeAdvertisingEvent e)
        {
            return null;
        }
    }

    /// <summary>
    /// Class that represents an Eddystone Beacon sending Eddystone-TLM frames.
    /// </summary>
    public sealed class EddystoneTlm : Eddystone
    {
        internal static EddystoneTlm ParseFrame(Queue<byte> payload, LeAdvertisingEvent e)
        {
            return null;
        }
    }

    /// <summary>
    /// Class that represents an Eddystone Beacon sending Eddystone-EID frames.
    /// </summary>
    public sealed class EddystoneEid : Eddystone
    {
        internal static EddystoneEid ParseFrame(Queue<byte> payload, LeAdvertisingEvent e)
        {
            return null;
        }
    }
}
