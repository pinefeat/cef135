var X = new ActiveXObject("ASCOM.PinefeatCEF.Focuser");
WScript.Echo("This is " + X.Name);
X.SetupDialog();
