/*
 * Copyright 2015 Huysentruit Wouter
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
using WinBeacon.Stack;
using WinBeacon.Stack.Controllers;
using WinBeacon.Stack.Hci;
using WinBeacon.Stack.Hci.Events;

namespace WinBeacon
{
    /// <summary>
    /// Hub for detecting beacons and advertise as a beacon.
    /// </summary>
    public class BeaconHub : IDisposable
    {
        private ILeController controller;

        /// <summary>
        /// Creates a hub instance that uses LibUsb or WinUSB as transport.
        /// </summary>
        /// <param name="usbVid">The VID of the BT4.0 dongle.</param>
        /// <param name="usbPid">The PID of the BT4.0 dongle.</param>
        public BeaconHub(int usbVid, int usbPid)
        {
            controller = new LibUsbLeController(usbVid, usbPid);
            controller.LeMetaEventReceived += controller_LeMetaEventReceived;
            controller.Open();
            controller.EnableScanning();
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~BeaconHub()
        {
            Dispose();
        }

        /// <summary>
        /// Release used resources.
        /// </summary>
        public void Dispose()
        {
            if (controller != null)
            {
                controller.LeMetaEventReceived -= controller_LeMetaEventReceived;
                controller.Dispose();
            }
            controller = null;
        }

        /// <summary>
        /// Enable advertising as a beacon.
        /// </summary>
        /// <param name="beacon">The beacon to emulate.</param>
        public void EnableAdvertising(Beacon beacon)
        {
            if (controller == null)
                throw new ObjectDisposedException("controller");
            if (beacon == null)
                throw new ArgumentNullException("beacon");
            controller.EnableAdvertising(beacon.ToAdvertisingData());
        }

        /// <summary>
        /// Disable advertising as a beacon.
        /// </summary>
        public void DisableAdvertising()
        {
            if (controller == null)
                throw new ObjectDisposedException("controller");
            controller.DisableAdvertising();
        }

        /// <summary>
        /// Event fired when a beacon is detected. This happens when the dongle receives the beacon's advertising packet.
        /// </summary>
        public EventHandler<BeaconDetectedEventArgs> BeaconDetected;
        private void OnBeaconDetected(Beacon beacon)
        {
            if (BeaconDetected != null)
                BeaconDetected(this, new BeaconDetectedEventArgs(beacon));
        }

        private void controller_LeMetaEventReceived(object sender, LeMetaEventReceivedEventArgs e)
        {
            if (e.LeMetaEvent == null)
                return;
            if (e.LeMetaEvent.SubEvent != LeMetaEvent.LeMetaSubEvent.AdvertisingReport)
                return;
            if (e.LeMetaEvent.AdvertisingEvents == null)
                return;

            foreach (var advertisingEvent in e.LeMetaEvent.AdvertisingEvents)
            {
                var beacon = Beacon.Parse(advertisingEvent);
                if (beacon != null)
                    OnBeaconDetected(beacon);
            }
        }
    }
}
