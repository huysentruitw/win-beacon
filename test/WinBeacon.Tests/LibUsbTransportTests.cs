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

using System.Collections.Generic;
using System.Linq;
using LibUsbDotNet;
using LibUsbDotNet.Info;
using LibUsbDotNet.Main;
using Moq;
using NUnit.Framework;
using WinBeacon.Stack;
using WinBeacon.Stack.Hci;
using WinBeacon.Stack.Transports.LibUsb;

namespace WinBeacon.Tests
{
    [TestFixture]
    public class LibUsbTransportTests
    {
        [Test]
        public void LibUsbTransport_Constructor()
        {
            using (var transport = new LibUsbTransport(new LibUsbDevice(0x1234, 0x5678)))
            {
                Assert.AreEqual(0x1234, transport.Vid);
                Assert.AreEqual(0x5678, transport.Pid);
            }
        }

        [Test]
        public void LibUsbTransport_OpenClose()
        {
            var usbDeviceMock = new Mock<TestLibUsbDevice>();
            var usbTransportMock = new Mock<LibUsbTransport>(usbDeviceMock.Object);
            usbDeviceMock.Verify(x => x.Open(), Times.Never);
            usbTransportMock.Verify(x => x.OpenEndpoints(), Times.Never);
            
            usbTransportMock.Object.Open();
            usbDeviceMock.Verify(x => x.Open(), Times.Once);
            usbTransportMock.Verify(x => x.OpenEndpoints(), Times.Once);
            usbDeviceMock.Verify(x => x.Close(), Times.Never);
            usbTransportMock.Verify(x => x.CloseEndpoints(), Times.Never);
            
            usbTransportMock.Object.Dispose();
            usbDeviceMock.Verify(x => x.Close(), Times.Once);
            usbTransportMock.Verify(x => x.CloseEndpoints(), Times.Once);
        }

        [Test]
        public void LibUsbTransport_Send()
        {
            var aclData = new byte[] { 0x12, 0x34, 0x56 };
            var commandData = new byte[] { 0x78, 0x9A, 0xBC };
            var usbDeviceMock = new Mock<TestLibUsbDevice>();
            var usbTransportMock = new Mock<LibUsbTransport>(usbDeviceMock.Object);

            usbTransportMock.Object.Send(aclData, DataType.Acl);
            usbTransportMock.Verify(x => x.SendCommand(It.IsAny<byte[]>()), Times.Never);
            usbTransportMock.Verify(x => x.SendAcl(It.Is<byte[]>(y => y.SequenceEqual(aclData))), Times.Once);

            usbTransportMock.Object.Send(commandData, DataType.Command);
            usbTransportMock.Verify(x => x.SendCommand(It.Is<byte[]>(y => y.SequenceEqual(commandData))), Times.Once);
            usbTransportMock.Verify(x => x.SendAcl(It.Is<byte[]>(y => y.SequenceEqual(aclData))), Times.Once);
        }

        [Test]
        [ExpectedException(typeof(WinBeaconException), ExpectedMessage = "USB device has no configurations")]
        public void LibUsbTransport_NoConfigurations()
        {
            var usbDeviceMock = new Mock<TestLibUsbDevice>();
            using (var transport = new LibUsbTransport(usbDeviceMock.Object))
                transport.Open();
        }

        #region Helpers

        internal abstract class TestLibUsbDevice : ILibUsbDevice
        {
            public abstract int Vid { get; }
            public abstract int Pid { get; }
            public abstract void Open();
            public abstract void Close();

            public abstract UsbEndpointReader OpenEndpointReader(ReadEndpointID readEndpointID);
            public abstract UsbEndpointWriter OpenEndpointWriter(WriteEndpointID writeEndpointID);
            public abstract bool ControlTransfer(ref UsbSetupPacket setupPacket, object buffer, int bufferLength, out int lengthTransferred);

            public IEnumerable<UsbConfigInfo> Configs
            {
                get { return Enumerable.Empty<UsbConfigInfo>(); }
            }
        }

        #endregion
    }
}
