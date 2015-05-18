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
            : base(new LibUsbTransport(vid, pid))
        {
        }
    }
}
