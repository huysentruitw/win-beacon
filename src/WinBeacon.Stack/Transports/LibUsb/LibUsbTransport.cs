/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using LibUsbDotNet;
using LibUsbDotNet.Main;
using WinBeacon.Stack.Hci;
using WinBeacon.Stack.Transport;

namespace WinBeacon.Stack.Transports.LibUsb
{
    internal class LibUsbTransport : ITransport
    {
        private static readonly TimeSpan writeTimeout = TimeSpan.FromSeconds(5);

        private readonly ILibUsbDevice _usbDevice;
        private readonly Dictionary<UsbBluetoothEndpointType, UsbEndpointBase> _endpoints = new Dictionary<UsbBluetoothEndpointType, UsbEndpointBase>();

        internal LibUsbTransport(ILibUsbDevice usbDevice)
        {
            _usbDevice = usbDevice ?? throw new ArgumentNullException(nameof(usbDevice));
        }

        ~LibUsbTransport()
        {
            Dispose();
        }

        #region ITransport implementation

        public void Dispose()
        {
            Close();
        }

        public void Open()
        {
            _usbDevice.Open();
            OpenEndpoints();
        }

        public void Close()
        {
            CloseEndpoints();
            _usbDevice.Close();
        }

        public void Send(byte[] data, DataType dataType)
        {
            if (_usbDevice == null)
                throw new WinBeaconException("Transport not open");
            if (data == null)
                throw new ArgumentNullException("data");
            switch (dataType)
            {
                case DataType.Command:
                    SendCommand(data);
                    break;
                case DataType.Acl:
                    SendAcl(data);
                    break;
                default:
                    throw new ArgumentException("Invalid dataType");
            }
        }

        public event EventHandler<DataReceivedEventArgs> DataReceived;
        private void OnDataReceived(byte[] data, DataType dataType)
        {
            if (DataReceived != null)
                DataReceived(this, new DataReceivedEventArgs(data, dataType));
        }

        #endregion

        public int Vid => _usbDevice.Vid;

        public int Pid => _usbDevice.Pid;

        private Dictionary<UsbBluetoothEndpointType, byte> GetBluetoothEndpointIds()
        {
            var endpointIds = new Dictionary<UsbBluetoothEndpointType, byte>
                {
                    { UsbBluetoothEndpointType.Commands, 0x00 },
                    { UsbBluetoothEndpointType.Events, 0x81 },
                    { UsbBluetoothEndpointType.AclDataOut, 0x02 },
                    { UsbBluetoothEndpointType.AclDataIn, 0x82 }
                };
            foreach (var info in _usbDevice.EnumerateBluetoothEndpointInfo())
                endpointIds[info.Type] = info.Id;
            return endpointIds;
        }

        internal virtual void OpenEndpoints()
        {
            var endpointIds = GetBluetoothEndpointIds();
            _endpoints[UsbBluetoothEndpointType.Commands] = _usbDevice.OpenEndpointWriter((WriteEndpointID)endpointIds[UsbBluetoothEndpointType.Commands]);
            _endpoints[UsbBluetoothEndpointType.Events] = _usbDevice.OpenEndpointReader((ReadEndpointID)endpointIds[UsbBluetoothEndpointType.Events]);
            _endpoints[UsbBluetoothEndpointType.AclDataOut] = _usbDevice.OpenEndpointWriter((WriteEndpointID)endpointIds[UsbBluetoothEndpointType.AclDataOut]);
            _endpoints[UsbBluetoothEndpointType.AclDataIn] = _usbDevice.OpenEndpointReader((ReadEndpointID)endpointIds[UsbBluetoothEndpointType.AclDataIn]);
            var failedEndpointTypes = from x in _endpoints where x.Value == null select x.Key;
            if (failedEndpointTypes.Count() > 0)
                throw new WinBeaconException($"Failed to open endpoint(s): {string.Join(" ", failedEndpointTypes)}");
            _endpoints[UsbBluetoothEndpointType.Events].SubscribeForDataReceived(data => OnDataReceived(data, DataType.Command));
            _endpoints[UsbBluetoothEndpointType.AclDataIn].SubscribeForDataReceived(data => OnDataReceived(data, DataType.Acl));
        }

        internal virtual void CloseEndpoints()
        {
            var openEndpoints = from x in _endpoints where x.Value != null select x.Value;
            foreach (var endpoint in openEndpoints)
            {
                if (endpoint is UsbEndpointReader)
                    (endpoint as UsbEndpointReader).DataReceivedEnabled = false;
                endpoint.Dispose();
            }

            _endpoints.Clear();
        }

        internal virtual void SendCommand(byte[] data)
        {
            var setupPacket = new UsbSetupPacket((byte)UsbRequestType.TypeClass | (byte)UsbRequestRecipient.RecipInterface, 0, 0, 0, (short)data.Length);
            if (!_usbDevice.ControlTransfer(ref setupPacket, data, data.Length, out int lengthTransferred))
                throw new WinBeaconException("USB ControlTransfer failed");
            if (lengthTransferred != data.Length)
                throw new WinBeaconException($"USB ControlTransfer didn't send all bytes. Sent {lengthTransferred} out of {data.Length} bytes");
        }

        internal virtual void SendAcl(byte[] data)
        {
            var endpoint = _endpoints[UsbBluetoothEndpointType.AclDataOut] as UsbEndpointWriter;
            var errorCode = endpoint.Write(data, (int)writeTimeout.TotalMilliseconds, out int transferLength);
            if (errorCode != ErrorCode.Ok)
                throw new WinBeaconException($"USB write operation failed with error code: {errorCode}");
            if (transferLength != data.Length)
                throw new WinBeaconException($"USB write operation didn't send all bytes. Sent {transferLength} out of {data.Length} bytes");
        }
    }
}
