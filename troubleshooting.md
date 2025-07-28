# 🛠 Troubleshooting Guide

A list of common problems and their solutions.

## 1: Controller Is Not Detected

The issue is likely caused by a poor connection of the USB cable. Ensure the cable is properly aligned and fully inserted into both the adapter and the host system.

The adapter with a USB port supports USB CDC (Communications Device Class) and does not require any special drivers on most modern operating systems, including Linux, macOS, and Windows. It should appear automatically as a serial port (e.g., COM3 on Windows).

The device identifies itself as _Lens Controller cef135_ when connected.

## 2. Lens Does Not Focus at All

If the lens does not focus and the image remains static when using the focuser module, it is likely because the lens has not been calibrated and does not know its full focus range.

Open the application, click Connect, then click Calibrate.

A valid, calibrated lens will record a positive value (e.g., 1203) in the _Maximum position_. If you see 0 or 65535 there, the lens has not been calibrated. 

You only need to do this once per lens. 
