# WinBeacon

[![Build status](https://ci.appveyor.com/api/projects/status/qjr2y51nq77lihny/branch/master?svg=true)](https://ci.appveyor.com/project/huysentruitw/win-beacon/branch/master)

## Overview

WinBeacon is a managed (C#) library with a minimal Bluetooth LE Stack that is able to detect and act as an iBeacon¹. This stack doesn't support BLE devices, only the detection and transmission of BLE advertisement packets used by beacons.

## Supported operating systems

* Windows XP (not tested, but it should work)
* Windows 7
* Windows 8
* Windows 10*

\* For Windows 10, you should be able to use [BluetoothLEAdvertisementWatcher](https://msdn.microsoft.com/en-us/library/windows.devices.bluetooth.advertisement.bluetoothleadvertisementwatcher.aspx) instead of this library.

## Supported BT4.0 LE dongles

| Manufacturer | Product | Chipset | VID / PID | Compatible |
| ------------ |:------- |:------- |:--------- |:---------- |
| Belkin | Mini Bluetooth 4.0 Adapter Class 2.10M | BCM20702A0 | VID_050D PID_065A | Yes |
| Pluggable | USB Bluetooth 4.0 Low Energy Micro Adapter | BCM20702A0 | VID_0A5C PID_21E8 | Yes |

Small list for the moment as I could only test with the dongle I had. If anyone can test with other BT4.0 dongle types, please let me know how it works out or send us a pull request.

## Installation 

This library needs raw USB access to a BT4.0 dongle. Therefore you should replace the original driver of the dongle with a WinUSB driver.
This also means that the default Bluetooth stack is no longer used and Windows will no longer detect the dongle as a Bluetooth dongle until you re-install the original drivers.

To replace or create a WinUSB driver for the BT4.0 dongle, we advise you to use the [Zadig tool](http://zadig.akeo.ie/).

## Get it on NuGet

    Install-Package WinBeacon

## Usage

### Detecting beacons

```C#
using (var hub = new BeaconHub(0x050D, 0x065A))
{
    hub.BeaconDetected += (sender, e) =>
		{
			Console.WriteLine("Detected beacon: {0}", e.Beacon);
		};
    Console.ReadKey();
}
```

### Advertise as a beacon

```C#
using (var hub = new BeaconHub(0x050D, 0x065A))
{
    hub.EnableAdvertising(new Beacon("B9407F30-F5F8-466E-AFF9-25556B57FE6D", 1000, 2000, -52));
    Console.ReadKey();
}
```

¹ iBeacon is a trademark of Apple inc.
