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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WinBeacon.Stack.Hci;
using WinBeacon.Stack.Hci.Commands;
using WinBeacon.Stack.Hci.Events;

namespace WinBeacon.Stack.Controllers
{
    /// <summary>
    /// Base class for Bluetooth Low Energy controllers.
    /// </summary>
    public class LeController : ILeController
    {
        private ITransport transport;
        private Thread commandExecutionThread;
        private ManualResetEvent cancelThread = new ManualResetEvent(false);
        private Queue<Command> commandQueue = new Queue<Command>();
        private AutoResetEvent executeNextCommand = new AutoResetEvent(false);

        internal LeController(ITransport transport)
        {
            this.transport = transport;
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~LeController()
        {
            Dispose();
        }

        /// <summary>
        /// Release all used resources.
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        /// <summary>
        /// Open underlying transport connection.
        /// </summary>
        public void Open()
        {
            if (commandExecutionThread != null)
                return;
            transport.Open();
            transport.DataReceived += transport_DataReceived;
            cancelThread.Reset();
            commandExecutionThread = new Thread(CommandExecutionThreadProc);
            commandExecutionThread.Start();
            SendCommand(new ResetCommand());
            SendCommand(new SetEventMaskCommand(new byte[] { 0xFF, 0xFF, 0xFB, 0xFF, 0x07, 0xF8, 0xBF, 0x3D }));
            SendCommand(new LeSetEventMaskCommand(new byte[] { 0x1F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));
            SendCommand(new LeWriteHostSupportedCommand(true, false));
            SendCommand(new LeSetScanParametersCommand(true, 10000, 10000, false, false));
            SendCommand(new ReadBdAddrCommand()
            {
                CommandCompleteCallback = (cmd, evt) =>
                {
                    if (evt.ResultData.Length != 6)
                        return;
                    if (DeviceAddressReceived != null)
                        DeviceAddressReceived(this, new DeviceAddressReceivedEventArgs(evt.ResultData.Reverse().ToArray()));
                }
            });
        }

        /// <summary>
        /// Close underlying transport connection.
        /// </summary>
        public void Close()
        {
            if (commandExecutionThread == null)
                return;
            cancelThread.Set();
            commandExecutionThread.Join();
            commandExecutionThread = null;
            lock (commandQueue)
                commandQueue.Clear();
            transport.DataReceived -= transport_DataReceived;
            transport.Close();
        }

        /// <summary>
        /// Enable Low Energy device scanning.
        /// </summary>
        public void EnableScanning()
        {
            SendCommand(new LeSetScanEnableCommand(true, false));
        }

        /// <summary>
        /// Disable Low Energy device scanning.
        /// </summary>
        public void DisableScanning()
        {
            SendCommand(new LeSetScanEnableCommand(false, false));
        }

        /// <summary>
        /// Enable Low Energy advertising.
        /// </summary>
        /// <param name="advertisementData">The advertisement data.</param>
        public void EnableAdvertising(byte[] advertisementData)
        {
            SendCommand(new LeSetAdvertisingDataCommand(advertisementData));
            SendCommand(new LeSetAdvertisingEnableCommand(true));
        }

        /// <summary>
        /// Disable Low Energy advertising.
        /// </summary>
        public void DisableAdvertising()
        {
            SendCommand(new LeSetAdvertisingEnableCommand(false));
        }

        /// <summary>
        /// Fired for each received Low Energy meta event.
        /// </summary>
        public event EventHandler<LeMetaEventReceivedEventArgs> LeMetaEventReceived;

        /// <summary>
        /// Fired when the device address of this Bluetooth device was received.
        /// </summary>
        public event EventHandler<DeviceAddressReceivedEventArgs> DeviceAddressReceived;

        private void transport_DataReceived(object sender, Hci.DataReceivedEventArgs e)
        {
            if (e.DataType != DataType.Command)
                return;
            var evt = Event.Parse(e.Data);
            if (evt is CommandCompleteEvent)
            {
                var commandCompleteEvent = (evt as CommandCompleteEvent);
                Command command = null;
                lock (commandQueue)
                {
                    if (commandQueue.Count > 0)
                    {
                        command = commandQueue.Peek();
                        if (command.Opcode == commandCompleteEvent.CommandOpcode)
                            commandQueue.Dequeue();
                        else
                            command = null;
                    }
                }
                if (command != null)
                    command.OnCommandComplete(commandCompleteEvent);
                executeNextCommand.Set();
            }
            else if (evt is LeMetaEvent)
            {
                if (LeMetaEventReceived != null)
                    LeMetaEventReceived(this, new LeMetaEventReceivedEventArgs(evt as LeMetaEvent));
            }
        }

        private void CommandExecutionThreadProc()
        {
            var waitHandles = new WaitHandle[] { cancelThread, executeNextCommand };
            while (WaitHandle.WaitAny(waitHandles) == 1)
            {
                Command command = null;
                lock (commandQueue)
                    if (commandQueue.Count > 0)
                        command = commandQueue.Peek();
                if (command != null)
                    transport.Send(command.ToByteArray(), DataType.Command);
            }
        }

        private void SendCommand(Command command)
        {
            lock (commandQueue)
            {
                commandQueue.Enqueue(command);
                if (commandQueue.Count == 1)
                    executeNextCommand.Set();
            }
        }
    }
}
