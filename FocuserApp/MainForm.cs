using System;
using System.Windows.Forms;

namespace ASCOM.PinefeatCEF.App
{
    public partial class MainForm : Form
    {
        private const string DRIVER_ID = "ASCOM.PinefeatCEF.Focuser";

        private DriverAccess.Focuser driver = null;
        private Timer pollTimer;
        private int updateCounter = 1;

        public MainForm()
        {
            InitializeComponent();
            InitializeTimer();
            SetUIState();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsConnected)
                driver.Connected = false;

            pollTimer.Stop();

            if (driver == null)
                return;

            driver.Dispose();
        }

        private void InitializeTimer()
        {
            pollTimer = new Timer();
            pollTimer.Interval = 300;
            pollTimer.Tick += PollTimer_Tick;
        }

        private void InitializeDriver()
        {
            if (driver != null)
                return;

            try
            {
                driver = new DriverAccess.Focuser(DRIVER_ID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "An error occurred while loading the focuser driver.\n\n"
                    + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonProperties_Click(object sender, EventArgs e)
        {
            InitializeDriver();
            if (driver != null)
            {
                driver.SetupDialog();
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsConnected)
                {
                    pollTimer.Stop();
                    driver.Connected = false;
                }
                else
                {
                    driver = new ASCOM.DriverAccess.Focuser(DRIVER_ID)
                    {
                        Connected = true
                    };
                    pollTimer.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "An error occurred while connecting to the focuser driver.\n\n"
                    + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            SetUIState();
        }

        private void SetUIState()
        {
            buttonProperties.Enabled = !IsConnected;
            buttonConnect.Text = IsConnected ? "Disconnect" : "Connect";

            groupBoxFocus.Visible = IsConnected;
            groupBoxAperture.Visible = groupBoxFocus.Visible;
        }

        private bool IsConnected
        {
            get
            {
                return (this.driver != null) && (driver.Connected == true);
            }
        }

        private void PollTimer_Tick(object sender, EventArgs e)
        {
            if (!IsConnected)
                return;

            switch (updateCounter++)
            {
                case 1:
                    updateFocusPosition();
                    break;
                case 2:
                    updateFocusMaximum();
                    break;
                case 3:
                    updateApertureRange();
                    break;
            }

            if (updateCounter > 3)
                updateCounter = 1;
        }

        private void updateFocusPosition()
        {
            try
            {
                labelFocusPosition.Text = driver.Position.ToString();
            }
            catch (Exception)
            {
                labelFocusPosition.Text = "n/a";
            }
        }

        private void updateFocusMaximum()
        {
            try
            {
                labelMaximumFocus.Text = driver.MaxStep.ToString();
            }
            catch (Exception)
            {
                labelMaximumFocus.Text = "n/a";
            }
        }

        private void updateApertureRange()
        {
            try
            {
                string apertureRange = driver.Action("GetApertureRange", String.Empty);
                var parts = apertureRange.Split('-');
                if (parts.Length == 2)
                {
                    labelApertureMaximum.Text = parts[0];
                    labelApertureMinimum.Text = parts[1];
                }
            }
            catch (Exception)
            {
                labelApertureMaximum.Text = "n/a";
                labelApertureMinimum.Text = "n/a";
            }
        }

        private void buttonCalibrate_Click(object sender, EventArgs e)
        {
            try
            {
                driver.Action("Calibrate", String.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "An error occurred during calibration.\n\n"
                    + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSetAperture_Click(object sender, EventArgs e)
        {
            try
            {
                driver.Action("SetAperture", numericUpDownAperture.Value.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "An error occurred during aperture setting.\n\n"
                    + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void numericUpDownAperture_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSetAperture.PerformClick();
            }
        }
    }
}
