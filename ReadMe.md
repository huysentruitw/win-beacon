# WinBeacon

## Overview

WinBeacon is a managed (C#) library with a minimal Bluetooth LE Stack that is able to detect and act as an iBeacon¹. This stack doesn't support BLE devices, only the detection and transmission of BLE advertisement packets used by beacons.

## Supported operating systems

* Windows XP (not tested, but it should work)
* Windows 7
* Windows 8

Windows 10 should have native beacon support which makes this library obsolete.

## Installation 

This library needs raw USB access to a BT4.0 dongle. Therefore you should replace the original driver of the dongle with a WinUSB driver.
This also means that the default Bluetooth stack is no longer used and Windows will no longer detect the dongle as a Bluetooth dongle until you re-install the original drivers.

To replace or create a WinUSB driver for the BT4.0 dongle, we advise you to use the [Zadig tool](http://zadig.akeo.ie/).

¹ iBeacon is a trademark of Apple inc.
