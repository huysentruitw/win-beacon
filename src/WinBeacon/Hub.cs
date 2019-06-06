/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

using System;
using WinBeacon.Stack;
using WinBeacon.Stack.Controllers;
using WinBeacon.Stack.Hci.Events;

namespace WinBeacon
{
    /// <summary>
    /// Hub for detecting beacons and advertise as a beacon.
    /// </summary>
    public class Hub : IDisposable
    {
        private ILeController _controller;

        /// <summary>
        /// Creates a hub instance that uses LibUsb or WinUSB as transport.
        /// </summary>
        /// <param name="usbVid">The VID of the BT4.0 dongle.</param>
        /// <param name="usbPid">The PID of the BT4.0 dongle.</param>
        public Hub(int usbVid, int usbPid)
        {
            _controller = new LibUsbLeController(usbVid, usbPid);
            _controller.LeMetaEventReceived += controller_LeMetaEventReceived;
            _controller.Open();
            _controller.EnableScanning();
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~Hub()
        {
            Dispose();
        }

        /// <summary>
        /// Release used resources.
        /// </summary>
        public void Dispose()
        {
            if (_controller != null)
            {
                _controller.LeMetaEventReceived -= controller_LeMetaEventReceived;
                _controller.Dispose();
            }

            _controller = null;
        }

        /// <summary>
        /// Enable advertising as an Apple iBeacon.
        /// </summary>
        /// <param name="beacon">The Apple iBeacon to emulate.</param>
        /// <param name="advertisingInterval">The advertising interval. Interval should be between 20 ms and 10.24 seconds. Defaults to 1.28 seconds.</param>
        public void EnableAdvertising(Beacon beacon, TimeSpan? advertisingInterval = null)
        {
            if (_controller == null)
                throw new ObjectDisposedException("controller");
            if (beacon == null)
                throw new ArgumentNullException(nameof(beacon));

            if (advertisingInterval.HasValue)
            {
                if (advertisingInterval.Value.TotalMilliseconds < 20 || advertisingInterval.Value.TotalMilliseconds > 10240)
                    throw new ArgumentOutOfRangeException("advertisingInterval", "Interval should be between 20 ms and 10.24 seconds");
                _controller.EnableAdvertising(beacon.ToAdvertisingData(), (ushort)advertisingInterval.Value.TotalMilliseconds);
            }
            else
            {
                _controller.EnableAdvertising(beacon.ToAdvertisingData());
            }
        }

        /// <summary>
        /// Enable advertising as an Eddystone.
        /// </summary>
        /// <param name="eddystone">The Eddystone to emulate.</param>
        /// <param name="advertisingInterval">The advertising interval. Interval should be between 20 ms and 10.24 seconds. Defaults to 1.28 seconds.</param>
        public void EnableAdvertising(Eddystone eddystone, TimeSpan? advertisingInterval = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Disable advertising as a beacon.
        /// </summary>
        public void DisableAdvertising()
        {
            if (_controller == null)
                throw new ObjectDisposedException(nameof(_controller));
            _controller.DisableAdvertising();
        }

        /// <summary>
        /// Event fired when an Apple iBeacon is detected. This happens when the dongle receives an Apple iBeacon compatible advertising packet.
        /// </summary>
        public event EventHandler<BeaconEventArgs> BeaconDetected;

        /// <summary>
        /// Event fired when an Eddystone is detected. This happens when the dongle receives an Eddystone compatible advertising packet.
        /// </summary>
        public event EventHandler<EddystoneEventArgs> EddystoneDetected;

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
                {
                    BeaconDetected?.Invoke(this, new BeaconEventArgs(beacon));
                    continue;
                }

                var eddystone = Eddystone.Parse(advertisingEvent);
                if (eddystone != null)
                {
                    EddystoneDetected?.Invoke(this, new EddystoneEventArgs(eddystone));
                    continue;
                }
            }
        }
    }
}
