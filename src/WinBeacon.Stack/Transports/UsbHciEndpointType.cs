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

namespace WinBeacon.Stack.Transport
{
    /// <summary>
    /// Bluetooth specific USB endpoint types.
    /// </summary>
    internal enum UsbBluetoothEndpointType
    {
        /// <summary>
        /// Command endpoint.
        /// </summary>
        Commands,
        /// <summary>
        /// Event endpoint.
        /// </summary>
        Events,
        /// <summary>
        /// ACL data in endpoint.
        /// </summary>
        AclDataIn,
        /// <summary>
        /// ACL data out endpoint.
        /// </summary>
        AclDataOut
    }
}
