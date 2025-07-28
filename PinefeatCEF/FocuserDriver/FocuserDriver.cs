//
// ASCOM Focuser driver for Pinefeat EF / EF-S Lens Controller
//
// Implements:	ASCOM Focuser interface version: 4
// Author:		support@pinefeat.co.uk
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using ASCOM.DeviceInterface;
using ASCOM.LocalServer;
using ASCOM.Utilities;

namespace ASCOM.PinefeatCEF.Focuser
{
    //
    // This code is mostly a presentation layer for the functionality in the FocuserHardware class.
    // Most customisation is in the FocuserHardware class, which is shared by all instances of the driver, and which must handle all aspects of communicating with the device.
    //
    // Driver's DeviceID is ASCOM.PinefeatCEF.Focuser
    //
    // The COM Guid attribute sets the CLSID for ASCOM.PinefeatCEF.Focuser
    // The COM ClassInterface/None attribute prevents an empty interface called _PinefeatCEF from being created and used as the [default] interface
    //

    /// <summary>
    /// ASCOM Focuser driver for Pinefeat EF / EF-S Lens Controller.
    /// </summary>
    [ComVisible(true)]
    [Guid("9138d9a6-e91c-468c-ab8e-eeefdf3320c9")]
    [ProgId("ASCOM.PinefeatCEF.Focuser")]
    [ServedClassName("Pinefeat EF / EF-S Lens Controller")] // Driver description that appears in the Chooser, customise as required
    [ClassInterface(ClassInterfaceType.None)]
    public class Focuser : ReferenceCountedObjectBase, IFocuserV4, IDisposable
    {
        internal static string DriverProgId; // ASCOM DeviceID (COM ProgID) for this driver, the value is retrieved from the ServedClassName attribute in the class initialiser.
        internal static string DriverDescription; // The value is retrieved from the ServedClassName attribute in the class initialiser.

        // connectedState and connectingState holds the states from this driver instance's perspective, as opposed to the local server's perspective, which may be different because of other client connections.
        internal bool connectedState; // The connected state from this driver's perspective)
        internal bool connectingState; // The connecting state from this driver's perspective)
        internal Exception connectionException = null; // Record any exception thrown if the driver encounters an error when connecting to the hardware using Connect() or Disconnect

        internal TraceLogger tl; // Trace logger object to hold diagnostic information just for this instance of the driver, as opposed to the local server's log, which includes activity from all driver instances.
        private bool disposedValue;

        private Guid uniqueId; // A unique ID for this instance of the driver

        #region Initialisation and Dispose

        /// <summary>
        /// Initializes a new instance of the <see cref="PinefeatCEF"/> class. Must be public to successfully register for COM.
        /// </summary>
        public Focuser()
        {
            try
            {
                // Pull the ProgID from the ProgID class attribute.
                Attribute attr = Attribute.GetCustomAttribute(this.GetType(), typeof(ProgIdAttribute));
                DriverProgId = ((ProgIdAttribute)attr).Value ?? "PROGID NOT SET!";  // Get the driver ProgIDfrom the ProgID attribute.

                // Pull the display name from the ServedClassName class attribute.
                attr = Attribute.GetCustomAttribute(this.GetType(), typeof(ServedClassNameAttribute));
                DriverDescription = ((ServedClassNameAttribute)attr).DisplayName ?? "DISPLAY NAME NOT SET!";  // Get the driver description that displays in the ASCOM Chooser from the ServedClassName attribute.

                // LOGGING CONFIGURATION
                // By default all driver logging will appear in Hardware log file
                // If you would like each instance of the driver to have its own log file as well, uncomment the lines below

                tl = new TraceLogger("", "PinefeatCEF.Driver"); // Remove the leading ASCOM. from the ProgId because this will be added back by TraceLogger.
                SetTraceState();

                // Initialise the hardware if required
                FocuserHardware.InitialiseHardware();

                LogMessage("Focuser", "Starting driver initialisation");
                LogMessage("Focuser", $"ProgID: {DriverProgId}, Description: {DriverDescription}");

                connectedState = false; // Initialise connected to false

                // Create a unique ID to identify this driver instance
                uniqueId = Guid.NewGuid();

                LogMessage("Focuser", "Completed initialisation");
            }
            catch (Exception ex)
            {
                LogMessage("Focuser", $"Initialisation exception: {ex}");
                MessageBox.Show($"{ex.Message}", "Exception creating ASCOM.PinefeatCEF.Focuser", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Class destructor called automatically by the .NET runtime when the object is finalised in order to release resources that are NOT managed by the .NET runtime.
        /// </summary>
        /// <remarks>See the Dispose(bool disposing) remarks for further information.</remarks>
        ~Focuser()
        {
            // Please do not change this code.
            // The Dispose(false) method is called here just to release unmanaged resources. Managed resources will be dealt with automatically by the .NET runtime.

            Dispose(false);
        }

        /// <summary>
        /// Deterministically dispose of any managed and unmanaged resources used in this instance of the driver.
        /// </summary>
        /// <remarks>
        /// Do not dispose of items in this method, put clean-up code in the 'Dispose(bool disposing)' method instead.
        /// </remarks>
        public void Dispose()
        {
            // Please do not change the code in this method.

            // Release resources now.
            Dispose(disposing: true);

            // Do not add GC.SuppressFinalize(this); here because it breaks the ReferenceCountedObjectBase COM connection counting mechanic
        }

        /// <summary>
        /// Dispose of large or scarce resources created or used within this driver file
        /// </summary>
        /// <remarks>
        /// The purpose of this method is to enable you to release finite system resources back to the operating system as soon as possible, so that other applications work as effectively as possible.
        ///
        /// NOTES
        /// 1) Do not call the FocuserHardware.Dispose() method from this method. Any resources used in the static FocuserHardware class itself, 
        ///    which is shared between all instances of the driver, should be released in the FocuserHardware.Dispose() method as usual. 
        ///    The FocuserHardware.Dispose() method will be called automatically by the local server just before it shuts down.
        /// 2) You do not need to release every .NET resource you use in your driver because the .NET runtime is very effective at reclaiming these resources. 
        /// 3) Strong candidates for release here are:
        ///     a) Objects that have a large memory footprint (> 1Mb) such as images
        ///     b) Objects that consume finite OS resources such as file handles, synchronisation object handles, memory allocations requested directly from the operating system (NativeMemory methods) etc.
        /// 4) Please ensure that you do not return exceptions from this method
        /// 5) Be aware that Dispose() can be called more than once:
        ///     a) By the client application
        ///     b) Automatically, by the .NET runtime during finalisation
        /// 6) Because of 5) above, you should make sure that your code is tolerant of multiple calls.    
        /// </remarks>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    try
                    {
                        // Dispose of managed objects here

                        // Clean up the trace logger object
                        if (!(tl is null))
                        {
                            tl.Enabled = false;
                            tl.Dispose();
                            tl = null;
                        }
                    }
                    catch (Exception)
                    {
                        // Any exception is not re-thrown because Microsoft's best practice says not to return exceptions from the Dispose method. 
                    }
                }

                try
                {
                    // Dispose of unmanaged objects, if any, here (OS handles etc.)
                }
                catch (Exception)
                {
                    // Any exception is not re-thrown because Microsoft's best practice says not to return exceptions from the Dispose method. 
                }

                // Flag that Dispose() has already run and disposed of all resources
                disposedValue = true;
            }
        }

        #endregion

        // PUBLIC COM INTERFACE IFocuserV4 IMPLEMENTATION

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialogue form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public void SetupDialog()
        {
            try
            {
                if (connectedState) // Don't show if already connected
                {
                    MessageBox.Show("Already connected, just press OK");
                }
                else // Show dialogue
                {
                    LogMessage("SetupDialog", $"Calling SetupDialog.");
                    FocuserHardware.SetupDialog();
                    LogMessage("SetupDialog", $"Completed.");
                }
            }
            catch (Exception ex)
            {
                LogMessage("SetupDialog", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }

        /// <summary>Returns the list of custom action names supported by this driver.</summary>
        /// <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
        public ArrayList SupportedActions
        {
            get
            {
                try
                {
                    CheckConnected($"SupportedActions");
                    ArrayList actions = FocuserHardware.SupportedActions;
                    LogMessage("SupportedActions", $"Returning {actions.Count} actions.");
                    return actions;
                }
                catch (Exception ex)
                {
                    LogMessage("SupportedActions", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>Invokes the specified device-specific custom action.</summary>
        /// <param name="ActionName">A well known name agreed by interested parties that represents the action to be carried out.</param>
        /// <param name="ActionParameters">List of required parameters or an <see cref="String.Empty">Empty String</see> if none are required.</param>
        /// <returns>A string response. The meaning of returned strings is set by the driver author.
        /// <para>Suppose filter wheels start to appear with automatic wheel changers; new actions could be <c>QueryWheels</c> and <c>SelectWheel</c>. The former returning a formatted list
        /// of wheel names and the second taking a wheel name and making the change, returning appropriate values to indicate success or failure.</para>
        /// </returns>
        public string Action(string actionName, string actionParameters)
        {
            try
            {
                CheckConnected($"Action {actionName} - {actionParameters}");
                LogMessage("", $"Calling Action: {actionName} with parameters: {actionParameters}");
                string actionResponse = FocuserHardware.Action(actionName, actionParameters);
                LogMessage("Action", $"Completed.");
                return actionResponse;
            }
            catch (Exception ex)
            {
                LogMessage("Action", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and does not wait for a response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        public void CommandBlind(string command, bool raw)
        {
            try
            {
                CheckConnected($"CommandBlind: {command}, Raw: {raw}");
                LogMessage("CommandBlind", $"Calling method - Command: {command}, Raw: {raw}");
                FocuserHardware.CommandBlind(command, raw);
                LogMessage("CommandBlind", $"Completed.");
            }
            catch (Exception ex)
            {
                LogMessage("CommandBlind", $"Command: {command}, Raw: {raw} threw an exception: \r\n{ex}");
                throw;
            }
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a boolean response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>
        /// Returns the interpreted boolean response received from the device.
        /// </returns>
        public bool CommandBool(string command, bool raw)
        {
            try
            {
                CheckConnected($"CommandBool: {command}, Raw: {raw}");
                LogMessage("CommandBlind", $"Calling method - Command: {command}, Raw: {raw}");
                bool commandBoolResponse = FocuserHardware.CommandBool(command, raw);
                LogMessage("CommandBlind", $"Returning: {commandBoolResponse}.");
                return commandBoolResponse;
            }
            catch (Exception ex)
            {
                LogMessage("CommandBool", $"Command: {command}, Raw: {raw} threw an exception: \r\n{ex}");
                throw;
            }
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a string response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>
        /// Returns the string response received from the device.
        /// </returns>
        public string CommandString(string command, bool raw)
        {
            try
            {
                CheckConnected($"CommandString: {command}, Raw: {raw}");
                LogMessage("CommandString", $"Calling method - Command: {command}, Raw: {raw}");
                string commandStringResponse = FocuserHardware.CommandString(command, raw);
                LogMessage("CommandString", $"Returning: {commandStringResponse}.");
                return commandStringResponse;
            }
            catch (Exception ex)
            {
                LogMessage("CommandString", $"Command: {command}, Raw: {raw} threw an exception: \r\n{ex}");
                throw;
            }
        }

        /// <summary>
        /// Connect to the device asynchronously using Connecting as the completion variable
        /// </summary>
        public void Connect()
        {
            try
            {
                if (connectedState)
                {
                    LogMessage("Connect", "Device already connected, ignoring method");
                    return;
                }

                // Initialise connection variables
                connectionException = null; // Clear any previous exception
                connectingState = true;

                // Start a task to connect to the hardware and then set the connected state to true
                _ = Task.Run(() =>
                {
                    try
                    {
                        LogMessage("Connect Task", "Starting connection");
                        FocuserHardware.SetConnected(uniqueId, true);
                        connectedState = true;
                        LogMessage("Connect Task", "Connection completed");
                    }
                    catch (Exception ex)
                    {
                        // Something went wrong so save the returned exception to return through Connecting and log the event.
                        connectionException = ex;
                        LogMessage("Connect Task", $"The connect task threw an exception: {ex.Message}\r\n{ex}");
                    }
                    finally
                    {
                        connectingState = false;
                    }
                });
            }
            catch (Exception ex)
            {
                LogMessage("Connect", $"Threw an exception: \r\n{ex}");
                throw;
            }
            LogMessage("Connect", $"Connect completed OK");
        }

        /// <summary>
        /// Set True to connect to the device hardware. Set False to disconnect from the device hardware.
        /// You can also read the property to check whether it is connected. This reports the current hardware state.
        /// </summary>
        /// <value><c>true</c> if connected to the hardware; otherwise, <c>false</c>.</value>
        public bool Connected
        {
            get
            {
                try
                {
                    // Returns the driver's connection state rather than the local server's connected state, which could be different because there may be other client connections still active.
                    LogMessage("Connected Get", connectedState.ToString());
                    return connectedState;
                }
                catch (Exception ex)
                {
                    LogMessage("Connected Get", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
            set
            {
                try
                {
                    if (value == connectedState)
                    {
                        LogMessage("Connected Set", "Device already connected, ignoring Connected Set = true");
                        return;
                    }

                    if (value)
                    {
                        LogMessage("Connected Set", "Connecting to device...");
                        FocuserHardware.SetConnected(uniqueId, true);
                        LogMessage("Connected Set", "Connected OK");
                        connectedState = true;
                    }
                    else
                    {
                        connectedState = false;
                        LogMessage("Connected Set", "Disconnecting from device...");
                        FocuserHardware.SetConnected(uniqueId, false);
                        LogMessage("Connected Set", "Disconnected OK");
                    }
                }
                catch (Exception ex)
                {
                    LogMessage("Connected Set", $"Threw an exception: {ex.Message}\r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Completion variable for the asynchronous Connect() and Disconnect()  methods
        /// </summary>
        public bool Connecting
        {
            get
            {
                // Return any exception returned by the Connect() or Disconnect() methods
                if (!(connectionException is null))
                    throw connectionException;

                // Otherwise return the current connecting state
                return connectingState;
            }
        }

        /// <summary>
        /// Disconnect from the device asynchronously using Connecting as the completion variable
        /// </summary>
        public void Disconnect()
        {
            try
            {
                if (!connectedState)
                {
                    LogMessage("Disconnect", "Device already disconnected, ignoring method");
                    return;
                }

                // Initialise connection variables
                connectionException = null; // Clear any previous exception
                connectingState = true;

                // Start a task to connect to the hardware and then set the connected state to true
                _ = Task.Run(() =>
                {
                    try
                    {
                        LogMessage("Disconnect Task", "Calling Connected");
                        FocuserHardware.SetConnected(uniqueId, false);
                        connectedState = false;
                        LogMessage("Disconnect Task", "Disconnection completed");
                    }
                    catch (Exception ex)
                    {
                        // Something went wrong so save the returned exception to return through Connecting and log the event.
                        connectionException = ex;
                        LogMessage("Disconnect Task", $"The disconnect task threw an exception: {ex.Message}\r\n{ex}");
                    }
                    finally
                    {
                        connectingState = false;
                    }
                });
            }
            catch (Exception ex)
            {
                LogMessage("Disconnect", $"Threw an exception: {ex.Message}\r\n{ex}");
                throw;
            }

            LogMessage("Disconnect", $"Disconnect completed OK");
        }

        /// <summary>
        /// Returns a description of the device, such as manufacturer and model number. Any ASCII characters may be used.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get
            {
                try
                {
                    CheckConnected($"Description");
                    string description = FocuserHardware.Description;
                    LogMessage("Description", description);
                    return description;
                }
                catch (Exception ex)
                {
                    LogMessage("Description", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Descriptive and version information about this ASCOM driver.
        /// </summary>
        public string DriverInfo
        {
            get
            {
                try
                {
                    // This should work regardless of whether or not the driver is Connected, hence no CheckConnected method.
                    string driverInfo = FocuserHardware.DriverInfo;
                    LogMessage("DriverInfo", driverInfo);
                    return driverInfo;
                }
                catch (Exception ex)
                {
                    LogMessage("DriverInfo", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// A string containing only the major and minor version of the driver formatted as 'm.n'.
        /// </summary>
        public string DriverVersion
        {
            get
            {
                try
                {
                    // This should work regardless of whether or not the driver is Connected, hence no CheckConnected method.
                    string driverVersion = FocuserHardware.DriverVersion;
                    LogMessage("DriverVersion", driverVersion);
                    return driverVersion;
                }
                catch (Exception ex)
                {
                    LogMessage("DriverVersion", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// The interface version number that this device supports.
        /// </summary>
        public short InterfaceVersion
        {
            get
            {
                try
                {
                    // This should work regardless of whether or not the driver is Connected, hence no CheckConnected method.
                    short interfaceVersion = FocuserHardware.InterfaceVersion;
                    LogMessage("InterfaceVersion", interfaceVersion.ToString());
                    return interfaceVersion;
                }
                catch (Exception ex)
                {
                    LogMessage("InterfaceVersion", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// The short name of the driver, for display purposes
        /// </summary>
        public string Name
        {
            get
            {
                try
                {
                    // This should work regardless of whether or not the driver is Connected, hence no CheckConnected method.
                    string name = FocuserHardware.Name;
                    LogMessage("Name Get", name);
                    return name;
                }
                catch (Exception ex)
                {
                    LogMessage("Name", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        #endregion

        #region IFocuser Implementation

        /// <summary>
        /// True if the focuser is capable of absolute position; that is, being commanded to a specific step location.
        /// </summary>
        public bool Absolute
        {
            get
            {
                try
                {
                    CheckConnected("Absolute");
                    bool absolute = FocuserHardware.Absolute;
                    LogMessage("Absolute", absolute.ToString());
                    return absolute;
                }
                catch (Exception ex)
                {
                    LogMessage("Absolute", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Return the device's state in one call
        /// </summary>
        public IStateValueCollection DeviceState
        {
            get
            {
                try
                {
                    CheckConnected("DeviceState");

                    // Create an array list to hold the IStateValue entries
                    List<IStateValue> deviceState = new List<IStateValue>();

                    // Add one entry for each operational state, if possible
                    try { deviceState.Add(new StateValue(nameof(IFocuserV4.IsMoving), IsMoving)); } catch { }
                    try { deviceState.Add(new StateValue(nameof(IFocuserV4.Position), Position)); } catch { }
                    try { deviceState.Add(new StateValue(nameof(IFocuserV4.Temperature), Temperature)); } catch { }
                    try { deviceState.Add(new StateValue(DateTime.Now)); } catch { }

                    // Return the overall device state
                    return new StateValueCollection(deviceState);
                }
                catch (Exception ex)
                {
                    LogMessage("DeviceState", $"Threw an exception: {ex.Message}\r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Immediately stop any focuser motion due to a previous <see cref="Move" /> method call.
        /// </summary>
        public void Halt()
        {
            try
            {
                CheckConnected("Halt");
                LogMessage("Halt", $"Calling method.");
                FocuserHardware.Halt();
                LogMessage("Halt", $"Completed.");
            }
            catch (Exception ex)
            {
                LogMessage("Halt", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }

        /// <summary>
        /// True if the focuser is currently moving to a new position. False if the focuser is stationary.
        /// </summary>
        public bool IsMoving
        {
            get
            {
                try
                {
                    CheckConnected("IsMoving");
                    bool isMoving = FocuserHardware.IsMoving;
                    LogMessage("IsMoving", isMoving.ToString());
                    return isMoving;
                }
                catch (Exception ex)
                {
                    LogMessage("IsMoving", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// State of the connection to the focuser.
        /// </summary>
        public bool Link
        {
            get
            {
                try
                {
                    CheckConnected("Link Get");
                    bool link = Connected;
                    LogMessage("Link Get", link.ToString());
                    return link;
                }
                catch (Exception ex)
                {
                    LogMessage("Link Get", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
            set
            {
                try
                {
                    CheckConnected("Link Set");
                    LogMessage("Link Set", value.ToString());
                    Connected = value;
                }
                catch (Exception ex)
                {
                    LogMessage("Link Set", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Maximum increment size allowed by the focuser;
        /// i.e. the maximum number of steps allowed in one move operation.
        /// </summary>
        public int MaxIncrement
        {
            get
            {
                try
                {
                    CheckConnected("MaxIncrement");
                    int maxIncrement = FocuserHardware.MaxIncrement;
                    LogMessage("MaxIncrement", maxIncrement.ToString());
                    return maxIncrement;
                }
                catch (Exception ex)
                {
                    LogMessage("MaxIncrement", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Maximum step position permitted.
        /// </summary>
        public int MaxStep
        {
            get
            {
                try
                {
                    CheckConnected("MaxStep");
                    int maxStep = FocuserHardware.MaxStep;
                    LogMessage("MaxStep", maxStep.ToString());
                    return maxStep;
                }
                catch (Exception ex)
                {
                    LogMessage("MaxStep", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Moves the focuser by the specified amount or to the specified position depending on the value of the <see cref="Absolute" /> property.
        /// </summary>
        /// <param name="position">Step distance or absolute position, depending on the value of the <see cref="Absolute" /> property.</param>
        public void Move(int position)
        {
            try
            {
                CheckConnected("Move");
                LogMessage("Move", $"Calling method.");
                FocuserHardware.Move(position);
                LogMessage("Move", $"Completed.");
            }
            catch (Exception ex)
            {
                LogMessage("Move", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }

        /// <summary>
        /// Current focuser position, in steps.
        /// </summary>
        public int Position
        {
            get
            {
                try
                {
                    CheckConnected("Position");
                    int position = FocuserHardware.Position;
                    LogMessage("Position", position.ToString());
                    return position;
                }
                catch (Exception ex)
                {
                    LogMessage("Position", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }


        /// <summary>
        /// Step size (microns) for the focuser.
        /// </summary>
        public double StepSize
        {
            get
            {
                try
                {
                    CheckConnected("StepSize");
                    double stepSize = FocuserHardware.StepSize;
                    LogMessage("StepSize", stepSize.ToString());
                    return stepSize;
                }
                catch (Exception ex)
                {
                    LogMessage("StepSize", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// The state of temperature compensation mode (if available), else always False.
        /// </summary>
        public bool TempComp
        {
            get
            {
                try
                {
                    CheckConnected("TempComp Get");
                    bool tempComp = FocuserHardware.TempComp;
                    LogMessage("TempComp Get", tempComp.ToString());
                    return tempComp;
                }
                catch (Exception ex)
                {
                    LogMessage("TempComp Get", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
            set
            {
                try
                {
                    CheckConnected("TempComp Set");
                    LogMessage("TempComp Set", value.ToString());
                    FocuserHardware.TempComp = value;
                }
                catch (Exception ex)
                {
                    LogMessage("TempComp Set", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// True if focuser has temperature compensation available.
        /// </summary>
        public bool TempCompAvailable
        {
            get
            {
                try
                {
                    CheckConnected("TempCompAvailable");
                    bool tempCompAvailable = FocuserHardware.TempCompAvailable;
                    LogMessage("TempCompAvailable", tempCompAvailable.ToString());
                    return tempCompAvailable;
                }
                catch (Exception ex)
                {
                    LogMessage("TempCompAvailable", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Current ambient temperature in degrees Celsius as measured by the focuser.
        /// </summary>
        public double Temperature
        {
            get
            {
                try
                {
                    CheckConnected("Temperature");
                    double temperature = FocuserHardware.Temperature;
                    LogMessage("Temperature", temperature.ToString());
                    return temperature;
                }
                catch (Exception ex)
                {
                    LogMessage("Temperature", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        #endregion

        #region Private properties and methods
        // Useful properties and methods that can be used as required to help with driver development

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private void CheckConnected(string message)
        {
            if (!connectedState)
            {
                throw new NotConnectedException($"{DriverDescription} ({DriverProgId}) is not connected: {message}");
            }
        }

        /// <summary>
        /// Log helper function that writes to the driver or local server loggers as required
        /// </summary>
        /// <param name="identifier">Identifier such as method name</param>
        /// <param name="message">Message to be logged.</param>
        private void LogMessage(string identifier, string message)
        {
            // This code is currently set to write messages to an individual driver log AND to the shared hardware log.

            // Write to the individual log for this specific instance (if enabled by the driver having a TraceLogger instance)
            if (tl != null)
            {
                tl.LogMessageCrLf(identifier, message); // Write to the individual driver log
            }

            // Write to the common hardware log shared by all running instances of the driver.
            FocuserHardware.LogMessage(identifier, message); // Write to the local server logger
        }

        /// <summary>
        /// Read the trace state from the driver's Profile and enable / disable the trace log accordingly.
        /// </summary>
        private void SetTraceState()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Focuser";
                tl.Enabled = Convert.ToBoolean(driverProfile.GetValue(DriverProgId, FocuserHardware.traceStateProfileName, string.Empty, FocuserHardware.traceStateDefault));
            }
        }

        #endregion
    }
}
