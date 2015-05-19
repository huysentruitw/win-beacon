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

namespace WinBeacon.Stack.Hci
{
    /// <summary>
    /// Event arguments for the DataReceived event.
    /// </summary>
    internal class DataReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// The received data.
        /// </summary>
        public byte[] Data { get; private set; }
        /// <summary>
        /// The type of the received data.
        /// </summary>
        public DataType DataType { get; private set; }

        /// <summary>
        /// Constructs a new DataReceivedEventArgs instance.
        /// </summary>
        /// <param name="data">The received data.</param>
        /// <param name="dataType">The type of the received data.</param>
        public DataReceivedEventArgs(byte[] data, DataType dataType)
        {
            Data = data;
            DataType = dataType;
        }
    }
}
