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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Moq;
using NUnit.Framework;
using WinBeacon.Stack;
using WinBeacon.Stack.Controllers;
using WinBeacon.Stack.Hci;
using WinBeacon.Stack.Hci.Commands;

namespace WinBeacon.Tests
{
    [TestFixture]
    public class LeControllerTests
    {
        [Test]
        public void LeController_OpenUnderlyingTransport()
        {
            var transportMock = new Mock<ITransport>();
            var controller = new LeController(transportMock.Object);
            controller.Open();
            transportMock.Verify(x => x.Open(), Times.Once());
            transportMock.Verify(x => x.Close(), Times.Never());
        }

        [Test]
        public void LeController_CloseUnderlyingTransport()
        {
            var transportMock = new Mock<ITransport>();
            var controller = new LeController(transportMock.Object);
            controller.Open();
            controller.Close();
            transportMock.Verify(x => x.Open(), Times.Once());
            transportMock.Verify(x => x.Close(), Times.Once());
        }

        [Test]
        public void LeController_Dispose()
        {
            var transportMock = new Mock<ITransport>();
            using (var controller = new LeController(transportMock.Object))
                controller.Open();
            transportMock.Verify(x => x.Open(), Times.Once());
            transportMock.Verify(x => x.Close(), Times.Once());
        }

        [Test]
        public void LeController_InitCommands()
        {
            var transportMock = new Mock<FakeCommandTransport> { CallBase = true };
            using (var controller = new LeController(transportMock.Object))
            {
                controller.Open();
                Thread.Sleep(10);
            }
            transportMock.Verify(
                transport => transport.Send(
                    It.Is<byte[]>(x => x.SequenceEqual(new ResetCommand().ToByteArray())),
                    It.Is<DataType>(x => x == DataType.Command)),
                Times.Exactly(1));
            transportMock.Verify(
                transport => transport.Send(It.IsAny<byte[]>(), It.Is<DataType>(x => x == DataType.Command)),
                Times.Exactly(7));
        }

        [Test]
        public void LeController_DeviceAddressReceived()
        {
            var transportMock = new Mock<FakeCommandTransport> { CallBase = true };
            transportMock
                .Setup(x => x.GetCommandCompletePayload(It.IsAny<byte[]>(), It.IsAny<DataType>()))
                .Returns(new Func<byte[], DataType, byte[]>((data, dataType) =>
                {
                    var readBdAddrData = new ReadBdAddrCommand().ToByteArray();
                    if (!Enumerable.SequenceEqual(data, readBdAddrData))
                        return null;
                    return new byte[] { 0x60, 0x50, 0x40, 0x30, 0x20, 0x10 };
                }));

            using (var controller = new LeController(transportMock.Object))
            {
                DeviceAddress deviceAddress = null;
                var done = new ManualResetEvent(false);
                controller.DeviceAddressReceived += (sender, e) =>
                {
                    deviceAddress = e.DeviceAddress;
                    done.Set();
                };

                controller.Open();
                Assert.IsTrue(done.WaitOne(1000, false));
                Assert.AreEqual(new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50, 0x60 }, deviceAddress.Address);
            }
        }

        #region Helpers

        internal class FakeCommandTransport : ITransport
        {
            public virtual void Send(byte[] data, DataType dataType)
            {
                var payload = GetCommandCompletePayload(data, dataType);
                ushort opcode = (ushort)(data[0] + (data[1] << 8));
                var commandCompleteEvent = new List<byte>
                    {
                        (byte)EventCode.CommandComplete,
                        (byte)(payload != null ? payload.Length : 0),
                        0x00, // NumberOfCommandsAllowedToSend
                        data[0], // CommandOpcode lsb
                        data[1], // CommandOpcode msb
                        0x00 // CommandParameterDataLength
                    };
                if (payload != null)
                    commandCompleteEvent.AddRange(payload);
                if (DataReceived != null)
                    DataReceived(this, new DataReceivedEventArgs(commandCompleteEvent.ToArray(), DataType.Command));
            }
            public event EventHandler<DataReceivedEventArgs> DataReceived;
            public virtual void Open() { }
            public virtual void Close() { }
            public virtual void Dispose() { }
            public virtual byte[] GetCommandCompletePayload(byte[] data, DataType dataType) { return null; }
        }

        #endregion
    }
}
