using System.Collections.Generic;
using NUnit.Framework;
using WinBeacon.Stack.Hci.Events;

namespace WinBeacon.Tests
{
    [TestFixture]
    public class EddystoneUrlTests
    {
        [Test]
        public void Eddystone_Parse_EddystoneUrlAdvertisingEvent_ShouldNotBeNull()
        {
            // Act
            var eddystone = Eddystone.Parse(EddystoneUrlAdvertisingEvent());

            // Assert
            Assert.That(eddystone, Is.Not.Null);
        }

        [Test]
        public void Eddystone_Parse_EddystoneUrlAdvertisingEvent_ShouldNotBeOfTypeEddystoneUrl()
        {
            // Act
            var eddystone = Eddystone.Parse(EddystoneUrlAdvertisingEvent());

            // Assert
            Assert.That(eddystone, Is.InstanceOf<EddystoneUrl>());
        }

        [Test]
        public void Eddystone_Parse_EddystoneUrlAdvertisingEvent_ShouldReturnAddress()
        {
            // Act
            var eddystone = Eddystone.Parse(EddystoneUrlAdvertisingEvent()) as EddystoneUrl;

            // Assert
            Assert.That(eddystone.Address, Is.EqualTo(new byte[] { 0xA6, 0xA5, 0xA4, 0xA3, 0xA2, 0xA1 }));
        }

        [Test]
        public void Eddystone_Parse_EddystoneUrlAdvertisingEvent_ShouldReturnRssi()
        {
            // Act
            var eddystone = Eddystone.Parse(EddystoneUrlAdvertisingEvent()) as EddystoneUrl;

            // Assert
            Assert.That(eddystone.Rssi, Is.EqualTo(-52));
        }

        [Test]
        public void Eddystone_Parse_EddystoneUrlAdvertisingEvent_ShouldReturnDecodedUrl()
        {
            var theories = new (byte SchemePrefix, byte ExpansionText, string ExpectedUrl)[]
            {
                (SchemePrefix: 0x00, ExpansionText: 0x00, ExpectedUrl: "http://www.google.com/"),
                (SchemePrefix: 0x01, ExpansionText: 0x00, ExpectedUrl: "https://www.google.com/"),
                (SchemePrefix: 0x02, ExpansionText: 0x00, ExpectedUrl: "http://google.com/"),
                (SchemePrefix: 0x03, ExpansionText: 0x00, ExpectedUrl: "https://google.com/"),

                (SchemePrefix: 0x00, ExpansionText: 0x01, ExpectedUrl: "http://www.google.org/"),
                (SchemePrefix: 0x00, ExpansionText: 0x02, ExpectedUrl: "http://www.google.edu/"),
                (SchemePrefix: 0x00, ExpansionText: 0x03, ExpectedUrl: "http://www.google.net/"),
                (SchemePrefix: 0x00, ExpansionText: 0x04, ExpectedUrl: "http://www.google.info/"),
                (SchemePrefix: 0x00, ExpansionText: 0x05, ExpectedUrl: "http://www.google.biz/"),
                (SchemePrefix: 0x00, ExpansionText: 0x06, ExpectedUrl: "http://www.google.gov/"),
                (SchemePrefix: 0x00, ExpansionText: 0x07, ExpectedUrl: "http://www.google.com"),
                (SchemePrefix: 0x00, ExpansionText: 0x08, ExpectedUrl: "http://www.google.org"),
                (SchemePrefix: 0x00, ExpansionText: 0x09, ExpectedUrl: "http://www.google.edu"),
                (SchemePrefix: 0x00, ExpansionText: 0x0A, ExpectedUrl: "http://www.google.net"),
                (SchemePrefix: 0x00, ExpansionText: 0x0B, ExpectedUrl: "http://www.google.info"),
                (SchemePrefix: 0x00, ExpansionText: 0x0C, ExpectedUrl: "http://www.google.biz"),
                (SchemePrefix: 0x00, ExpansionText: 0x0D, ExpectedUrl: "http://www.google.gov"),
            };

            foreach (var theory in theories)
            {
                // Act
                var eddystone = Eddystone.Parse(EddystoneUrlAdvertisingEvent(theory.SchemePrefix, theory.ExpansionText)) as EddystoneUrl;

                // Assert
                Assert.That(eddystone.Url, Is.EqualTo(theory.ExpectedUrl));
            }
        }

        [Test]
        public void Eddystone_Parse_EddystoneUrlAdvertisingEvent_ShouldReturnCalibratedTxPower()
        {
            // Act
            var eddystone = Eddystone.Parse(EddystoneUrlAdvertisingEvent()) as EddystoneUrl;

            // Assert
            Assert.That(eddystone.CalibratedTxPower, Is.EqualTo(-18));
        }

        private static LeAdvertisingEvent EddystoneUrlAdvertisingEvent(byte schemePrefix = 0, byte expansionText = 0)
            => LeAdvertisingEvent.Parse(new Queue<byte>(new byte[]
            {
                0x02, // Low Energy Advertising event type: Scan IND
                0x00, // Address type
                0xA1, 0xA2, 0xA3, 0xA4, 0xA5, 0xA6, // Bluetooth MAC address
                0x15, // Payload length
                0x02, 0x01, 0x06, // Flags data
                0x03, 0x03, 0xAA, 0xFE, // Complete list of 16-bit Service UUID (including 16-bit Eddystone UUID)
                0x0D,
                0x16, // Service Data data type value
                0xAA, 0xFE, // 16-bit Eddystone UUID
                0x10, // URL frame type
                0xEE, // Calibrated TX power
                schemePrefix,
                0x67, 0x6F, 0x6F, 0x67, 0x6C, 0x65, // google
                expansionText,
                0xCC, // RSSI
            }));
    }
}
