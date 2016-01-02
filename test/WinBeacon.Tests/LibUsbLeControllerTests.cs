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
