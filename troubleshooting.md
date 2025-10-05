# 🛠 Troubleshooting Guide

A list of common problems and their solutions.

## 1: Error while installing or uninstalling the driver

Driver installation or reinstallation fails with errors such as:

> Unable to load one or more of the requested types.

Chis usually happens when required dependencies are missing. The driver requires ASCOM Platform 7.0 – Update 2 and Microsoft .NET Framework 4.8.

 - Verify that ASCOM Platform 7.0 – Update 2 is installed.
 - Ensure .NET Framework 4.8 is installed.
 - Install or update any missing dependencies, then retry the driver installation.

## 2: Controller Is Not Detected

The issue is likely caused by a poor connection of the USB cable. Ensure the cable is properly aligned and fully inserted into both the adapter and the host system.

Make sure you're using a USB **data** cable with 4 wires to communicate with the adapter. A charging cable has only two power wires connected and is not suitable.

The adapter with a USB port supports USB CDC (Communications Device Class) and does not require any special drivers on most modern operating systems, including Linux, macOS, and Windows. It should appear automatically as a serial port (e.g., COM3 on Windows).

The device identifies itself as _Lens Controller cef135_ when connected.

## 3. Lens Does Not Focus at All

If the lens does not focus and the image remains static when using the focuser module, it is likely because the lens has not been calibrated and does not know its full focus range.

Open the application, click Connect, then click Calibrate.

A valid, calibrated lens will record a positive value (e.g., 1203) in the _Maximum position_. If you see 0 or 65535 there, the lens has not been calibrated. 

You only need to do this once per lens. 
