/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

using System;
using System.Collections.Generic;
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
        private readonly ITransport _transport;
        private readonly ManualResetEvent _cancelThread = new ManualResetEvent(false);
        private readonly Queue<Command> _commandQueue = new Queue<Command>();
        private readonly AutoResetEvent _executeNextCommand = new AutoResetEvent(false);
        private Thread _commandExecutionThread;

        internal LeController(ITransport transport)
        {
            _transport = transport;
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
            if (_commandExecutionThread != null)
                return;
            _transport.Open();
            _transport.DataReceived += transport_DataReceived;
            _cancelThread.Reset();
            _commandExecutionThread = new Thread(CommandExecutionThreadProc);
            _commandExecutionThread.Start();
            SendCommand(new ResetCommand());
            SendCommand(new SetEventMaskCommand(new byte[] { 0xFF, 0xFF, 0xFB, 0xFF, 0x07, 0xF8, 0xBF, 0x3D }));
            SendCommand(new LeSetEventMaskCommand(new byte[] { 0x1F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));
            SendCommand(new LeWriteHostSupportedCommand(true, false));
            SendCommand(new LeSetScanEnableCommand(false, false));
            SendCommand(new LeSetScanParametersCommand(true, 10000, 10000, false, false));
            SendCommand(new ReadBdAddrCommand()
            {
                CommandCompleteCallback = (cmd, deviceAddress) =>
                {
                    DeviceAddressReceived?.Invoke(this, new DeviceAddressReceivedEventArgs(deviceAddress));
                }
            });
        }

        /// <summary>
        /// Close underlying transport connection.
        /// </summary>
        public void Close()
        {
            if (_commandExecutionThread == null)
                return;
            _cancelThread.Set();
            _commandExecutionThread.Join();
            _commandExecutionThread = null;
            lock (_commandQueue)
                _commandQueue.Clear();
            _transport.DataReceived -= transport_DataReceived;
            _transport.Close();
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
        /// <param name="advertisingIntervalInMs">Interval should be between 20 and 10240 ms. Defaults to 1280 ms.</param>
        public void EnableAdvertising(byte[] advertisementData, int advertisingIntervalInMs = 1280)
        {
            SendCommand(new LeSetAdvertisingDataCommand(advertisementData));
            SendCommand(new LeSetAdvertisingParametersCommand(
                    advertisingIntervalInMs,
                    advertisingIntervalInMs,
                    AdvertisingType.ConnectableUndirected,
                    OwnAddressType.Public,
                    PeerAddressType.Public,
                    new byte[6],
                    AdvertisingChannelMap.UseAllChannels,
                    AdvertisingFilterPolicy.ConnectAllScanAll
                ));
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
                lock (_commandQueue)
                {
                    if (_commandQueue.Count > 0)
                    {
                        command = _commandQueue.Peek();
                        if (command.Opcode == commandCompleteEvent.CommandOpcode)
                            _commandQueue.Dequeue();
                        else
                            command = null;
                    }
                }
                if (command != null)
                    command.OnCommandComplete(commandCompleteEvent);
                _executeNextCommand.Set();
            }
            else if (evt is LeMetaEvent)
            {
                LeMetaEventReceived?.Invoke(this, new LeMetaEventReceivedEventArgs(evt as LeMetaEvent));
            }
        }

        private void CommandExecutionThreadProc()
        {
            var waitHandles = new WaitHandle[] { _cancelThread, _executeNextCommand };
            while (WaitHandle.WaitAny(waitHandles) == 1)
            {
                Command command = null;
                lock (_commandQueue)
                    if (_commandQueue.Count > 0)
                        command = _commandQueue.Peek();
                if (command != null)
                    _transport.Send(command.ToByteArray(), DataType.Command);
            }
        }

        private void SendCommand(Command command)
        {
            lock (_commandQueue)
            {
                _commandQueue.Enqueue(command);
                if (_commandQueue.Count == 1)
                    _executeNextCommand.Set();
            }
        }
    }
}
