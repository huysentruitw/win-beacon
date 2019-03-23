/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

using System.Threading;
using NUnit.Framework;
using WinBeacon.Stack.Controllers;

namespace WinBeacon.Tests
{
    [TestFixture]
    public class LibUsbLeControllerTests
    {
        private const int vid = 0x050D;
        private const int pid = 0x065A;

        /// <summary>
        /// This is a live test that needs some pre-requisites:
        ///  * A BLE compatible dongle with WinUSB driver
        ///  * The correct vid and pid combination in the consts above
        ///  * A beacon that broadcasts at minimum 2Hz rate (iPad users can use the BLEBeacon app to advertise as a beacon) 
        /// </summary>
        [Test]
        [Ignore]
        public void LibUsbLeController_WaitForLeMetaEvent()
        {
            var leMetaEventReceived = false;
            using (var controller = new LibUsbLeController(vid, pid))
            {
                controller.Open();
                controller.EnableScanning();
                controller.LeMetaEventReceived += (sender, e) =>
                    {
                        if (e.LeMetaEvent.Code == Stack.Hci.EventCode.LeMeta)
                            leMetaEventReceived = true;
                    };
                Thread.Sleep(1 * 1000);
            }
            Assert.IsTrue(leMetaEventReceived, "No meta event received");
        }
    }
}
