/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

using WinBeacon.Stack.Transports.LibUsb;

namespace WinBeacon.Stack.Controllers
{
    /// <summary>
    /// LibUsb Bluetooth Low Energy controller.
    /// </summary>
    public class LibUsbLeController : LeController
    {
        /// <summary>
        /// Create a new LibUsbLeController instance.
        /// </summary>
        /// <param name="vid">The Bluetooth dongle USB vendor identifier.</param>
        /// <param name="pid">The Bluetooth dongle USB product identifier.</param>
        public LibUsbLeController(int vid, int pid)
            : base(new LibUsbTransport(new LibUsbDevice(vid, pid)))
        {
        }
    }
}
