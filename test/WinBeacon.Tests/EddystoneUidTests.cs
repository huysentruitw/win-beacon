using System.Collections.Generic;
using NUnit.Framework;
using WinBeacon.Stack.Hci.Events;

namespace WinBeacon.Tests
{
    [TestFixture]
    public class EddystoneUidTests
    {
        [Test]
        public void Eddystone_Parse_EddystoneUidAdvertisingEvent_ShouldNotBeNull()
        {
            // Act
            var eddystone = Eddystone.Parse(EddystoneUidAdvertisingEvent);

            // Assert
            Assert.That(eddystone, Is.Not.Null);
        }

        [Test]
        public void Eddystone_Parse_EddystoneUidAdvertisingEvent_ShouldNotBeOfTypeEddystoneUid()
        {
            // Act
            var eddystone = Eddystone.Parse(EddystoneUidAdvertisingEvent);

            // Assert
            Assert.That(eddystone, Is.InstanceOf<EddystoneUid>());
        }

        [Test]
        public void Eddystone_Parse_EddystoneUidAdvertisingEvent_ShouldReturnAddress()
        {
            // Act
            var eddystone = Eddystone.Parse(EddystoneUidAdvertisingEvent) as EddystoneUid;

            // Assert
            Assert.That(eddystone.Address, Is.EqualTo(new byte[] { 0xA6, 0xA5, 0xA4, 0xA3, 0xA2, 0xA1 }));
        }

        [Test]
        public void Eddystone_Parse_EddystoneUidAdvertisingEvent_ShouldReturnRssi()
        {
            // Act
            var eddystone = Eddystone.Parse(EddystoneUidAdvertisingEvent) as EddystoneUid;

            // Assert
            Assert.That(eddystone.Rssi, Is.EqualTo(-52));
        }

        [Test]
        public void Eddystone_Parse_EddystoneUidAdvertisingEvent_ShouldReturnNamespace()
        {
            // Act
            var eddystone = Eddystone.Parse(EddystoneUidAdvertisingEvent) as EddystoneUid;

            // Assert
            Assert.That(eddystone.Namespace, Is.EqualTo(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A }));
        }

        [Test]
        public void Eddystone_Parse_EddystoneUidAdvertisingEvent_ShouldReturnInstance()
        {
            // Act
            var eddystone = Eddystone.Parse(EddystoneUidAdvertisingEvent) as EddystoneUid;

            // Assert
            Assert.That(eddystone.Instance, Is.EqualTo(new byte[] { 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 }));
        }

        [Test]
        public void Eddystone_Parse_EddystoneUidAdvertisingEvent_ShouldReturnCalibratedTxPower()
        {
            // Act
            var eddystone = Eddystone.Parse(EddystoneUidAdvertisingEvent) as EddystoneUid;

            // Assert
            Assert.That(eddystone.CalibratedTxPower, Is.EqualTo(-18));
        }

        private static readonly LeAdvertisingEvent EddystoneUidAdvertisingEvent = LeAdvertisingEvent.Parse(new Queue<byte>(new byte[]
        {
            0x02, // Low Energy Advertising event type: Scan IND
            0x00, // Address type
            0xA1, 0xA2, 0xA3, 0xA4, 0xA5, 0xA6, // Bluetooth MAC address
            0x1F, // Payload length
            0x02, 0x01, 0x06, // Flags data
            0x03, 0x03, 0xAA, 0xFE, // Complete list of 16-bit Service UUID (including 16-bit Eddystone UUID)
            0x17,
            0x16, // Service Data data type value
            0xAA, 0xFE, // 16-bit Eddystone UUID
            0x00, // UID frame type
            0xEE, // Calibrated TX power
            0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, // Namespace
            0x11, 0x12, 0x13, 0x14, 0x15, 0x16, // Instance
            0x00, 0x00, // Reserved for future use
            0xCC, // RSSI
        }));
    }
}
