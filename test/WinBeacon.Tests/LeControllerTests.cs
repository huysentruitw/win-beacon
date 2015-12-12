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
                Times.Exactly(6));
        }

        #region Helpers

        internal class FakeCommandTransport : ITransport
        {
            public virtual void Send(byte[] data, DataType dataType)
            {
                ushort opcode = (ushort)(data[0] + (data[1] << 8));
                var commandCompleteEvent = new byte[]
                    {
                        (byte)EventCode.CommandComplete,
                        0x00, // payloadSize
                        0x00, // NumberOfCommandsAllowedToSend
                        data[0], // CommandOpcode lsb
                        data[1], // CommandOpcode msb
                        0x00 // CommandParameterDataLength
                    };
                if (DataReceived != null)
                    DataReceived(this, new DataReceivedEventArgs(commandCompleteEvent, DataType.Command));
            }
            public event EventHandler<DataReceivedEventArgs> DataReceived;
            public virtual void Open() { }
            public virtual void Close() { }
            public virtual void Dispose() { }
        }

        #endregion
    }
}
