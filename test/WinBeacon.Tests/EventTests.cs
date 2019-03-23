/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

using NUnit.Framework;
using WinBeacon.Stack.Hci;
using WinBeacon.Stack.Hci.Events;

namespace WinBeacon.Tests
{
    [TestFixture]
    public class EventTests
    {
        [Test]
        public void Event_NullEvent()
        {
            var e = Event.Parse(new byte[] { 0x12 });
            Assert.IsNull(e);
        }

        [Test]
        public void Event_SimpleEvent()
        {
            var e = Event.Parse(new byte[] { 0x12, 0x00 });
            Assert.AreEqual(EventCode.RoleChanged, e.Code);
        }

        [Test]
        public void Event_CommandCompleteEventParse()
        {
            var e = (Event.Parse(new byte[] { 0x0E, 0x10, 0x20, 0x30, 0x40, 0x50, 0x60, 0x70 }) as CommandCompleteEvent);
            Assert.AreEqual(EventCode.CommandComplete, e.Code);
            Assert.AreEqual(0x4030, e.CommandOpcode);
            Assert.AreEqual(0x50, e.CommandParameterDataLength);
            Assert.AreEqual(0x20, e.NumberOfCommandsAllowedToSend);
            Assert.AreEqual(new byte[] { 0x60, 0x70 }, e.ResultData);
        }

        [Test]
        public void Event_LeAdvertisingEventParse()
        {
            var e = (Event.Parse(new byte[] { 0x3E, 0x10, 0x02, 0x01, 0x02, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0x03, 0xBB, 0xCC, 0xDD, 0xEE }) as LeMetaEvent);
            Assert.AreEqual(EventCode.LeMeta, e.Code);
            Assert.AreEqual(LeMetaEvent.LeMetaSubEvent.AdvertisingReport, e.SubEvent);
            Assert.AreEqual(1, e.AdvertisingEvents.Length);
            var ae = e.AdvertisingEvents[0];
            Assert.AreEqual(new byte[] { 0x99, 0x88, 0x77, 0x66, 0x55, 0x44 }, ae.Address);
            Assert.AreEqual(0x33, ae.AddressType);
            Assert.AreEqual(LeAdvertisingEventType.ScanInd, ae.EventType);
            Assert.AreEqual(new byte[] { 0xBB, 0xCC, 0xDD }, ae.Payload);
            Assert.AreEqual(-18, ae.Rssi);
        }
    }
}
