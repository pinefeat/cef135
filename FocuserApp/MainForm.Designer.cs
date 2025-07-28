using ASCOM.DriverAccess;

namespace ASCOM.PinefeatCEF.App
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.buttonProperties = new System.Windows.Forms.Button();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.labelDriverId = new System.Windows.Forms.Label();
            this.labelFocusPosition = new System.Windows.Forms.Label();
            this.captionFocusPosition = new System.Windows.Forms.Label();
            this.captionMaximumFocus = new System.Windows.Forms.Label();
            this.labelMaximumFocus = new System.Windows.Forms.Label();
            this.buttonCalibrate = new System.Windows.Forms.Button();
            this.groupBoxFocus = new System.Windows.Forms.GroupBox();
            this.groupBoxAperture = new System.Windows.Forms.GroupBox();
            this.captionApertureMinimum = new System.Windows.Forms.Label();
            this.labelApertureMinimum = new System.Windows.Forms.Label();
            this.labelApertureMaximum = new System.Windows.Forms.Label();
            this.captionApertureMaximum = new System.Windows.Forms.Label();
            this.numericUpDownAperture = new System.Windows.Forms.NumericUpDown();
            this.buttonSetAperture = new System.Windows.Forms.Button();
            this.captionApertureValue = new System.Windows.Forms.Label();
            this.mainFormBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBoxFocus.SuspendLayout();
            this.groupBoxAperture.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAperture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainFormBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonProperties
            // 
            this.buttonProperties.Location = new System.Drawing.Point(637, 25);
            this.buttonProperties.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.buttonProperties.Name = "buttonProperties";
            this.buttonProperties.Size = new System.Drawing.Size(144, 44);
            this.buttonProperties.TabIndex = 0;
            this.buttonProperties.Text = "Properties";
            this.buttonProperties.UseVisualStyleBackColor = true;
            this.buttonProperties.Click += new System.EventHandler(this.buttonProperties_Click);
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(637, 80);
            this.buttonConnect.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(144, 44);
            this.buttonConnect.TabIndex = 1;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // labelDriverId
            // 
            this.labelDriverId.Location = new System.Drawing.Point(24, 30);
            this.labelDriverId.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelDriverId.Name = "labelDriverId";
            this.labelDriverId.Size = new System.Drawing.Size(581, 39);
            this.labelDriverId.TabIndex = 2;
            this.labelDriverId.Text = "Pinefeat EF / EF-S Lens Controller";
            this.labelDriverId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFocusPosition
            // 
            this.labelFocusPosition.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelFocusPosition.Location = new System.Drawing.Point(44, 70);
            this.labelFocusPosition.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelFocusPosition.Name = "labelFocusPosition";
            this.labelFocusPosition.Size = new System.Drawing.Size(320, 39);
            this.labelFocusPosition.TabIndex = 0;
            this.labelFocusPosition.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // captionFocusPosition
            // 
            this.captionFocusPosition.AutoSize = true;
            this.captionFocusPosition.Location = new System.Drawing.Point(39, 45);
            this.captionFocusPosition.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.captionFocusPosition.Name = "captionFocusPosition";
            this.captionFocusPosition.Size = new System.Drawing.Size(157, 25);
            this.captionFocusPosition.TabIndex = 4;
            this.captionFocusPosition.Text = "Current Position:";
            // 
            // captionMaximumFocus
            // 
            this.captionMaximumFocus.AutoSize = true;
            this.captionMaximumFocus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.captionMaximumFocus.Location = new System.Drawing.Point(392, 45);
            this.captionMaximumFocus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.captionMaximumFocus.Name = "captionMaximumFocus";
            this.captionMaximumFocus.Size = new System.Drawing.Size(177, 25);
            this.captionMaximumFocus.TabIndex = 6;
            this.captionMaximumFocus.Text = "Maximum Position:";
            // 
            // labelMaximumFocus
            // 
            this.labelMaximumFocus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelMaximumFocus.Location = new System.Drawing.Point(397, 70);
            this.labelMaximumFocus.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelMaximumFocus.Name = "labelMaximumFocus";
            this.labelMaximumFocus.Size = new System.Drawing.Size(319, 39);
            this.labelMaximumFocus.TabIndex = 1;
            this.labelMaximumFocus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonCalibrate
            // 
            this.buttonCalibrate.Location = new System.Drawing.Point(44, 142);
            this.buttonCalibrate.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.buttonCalibrate.Name = "buttonCalibrate";
            this.buttonCalibrate.Size = new System.Drawing.Size(144, 44);
            this.buttonCalibrate.TabIndex = 2;
            this.buttonCalibrate.Text = "Calibrate";
            this.buttonCalibrate.UseVisualStyleBackColor = true;
            this.buttonCalibrate.Click += new System.EventHandler(this.buttonCalibrate_Click);
            // 
            // groupBoxFocus
            // 
            this.groupBoxFocus.Controls.Add(this.buttonCalibrate);
            this.groupBoxFocus.Controls.Add(this.captionMaximumFocus);
            this.groupBoxFocus.Controls.Add(this.labelMaximumFocus);
            this.groupBoxFocus.Controls.Add(this.labelFocusPosition);
            this.groupBoxFocus.Controls.Add(this.captionFocusPosition);
            this.groupBoxFocus.Location = new System.Drawing.Point(24, 145);
            this.groupBoxFocus.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxFocus.Name = "groupBoxFocus";
            this.groupBoxFocus.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxFocus.Size = new System.Drawing.Size(757, 224);
            this.groupBoxFocus.TabIndex = 2;
            this.groupBoxFocus.TabStop = false;
            this.groupBoxFocus.Text = "Focus";
            // 
            // groupBoxAperture
            // 
            this.groupBoxAperture.Controls.Add(this.captionApertureMinimum);
            this.groupBoxAperture.Controls.Add(this.labelApertureMinimum);
            this.groupBoxAperture.Controls.Add(this.labelApertureMaximum);
            this.groupBoxAperture.Controls.Add(this.captionApertureMaximum);
            this.groupBoxAperture.Controls.Add(this.numericUpDownAperture);
            this.groupBoxAperture.Controls.Add(this.buttonSetAperture);
            this.groupBoxAperture.Controls.Add(this.captionApertureValue);
            this.groupBoxAperture.Location = new System.Drawing.Point(24, 395);
            this.groupBoxAperture.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxAperture.Name = "groupBoxAperture";
            this.groupBoxAperture.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxAperture.Size = new System.Drawing.Size(757, 243);
            this.groupBoxAperture.TabIndex = 3;
            this.groupBoxAperture.TabStop = false;
            this.groupBoxAperture.Text = "Aperture, f-stop";
            // 
            // captionApertureMinimum
            // 
            this.captionApertureMinimum.AutoSize = true;
            this.captionApertureMinimum.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.captionApertureMinimum.Location = new System.Drawing.Point(392, 46);
            this.captionApertureMinimum.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.captionApertureMinimum.Name = "captionApertureMinimum";
            this.captionApertureMinimum.Size = new System.Drawing.Size(97, 25);
            this.captionApertureMinimum.TabIndex = 8;
            this.captionApertureMinimum.Text = "Minimum:";
            // 
            // labelApertureMinimum
            // 
            this.labelApertureMinimum.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelApertureMinimum.Location = new System.Drawing.Point(397, 71);
            this.labelApertureMinimum.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelApertureMinimum.Name = "labelApertureMinimum";
            this.labelApertureMinimum.Size = new System.Drawing.Size(319, 39);
            this.labelApertureMinimum.TabIndex = 1;
            this.labelApertureMinimum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelApertureMaximum
            // 
            this.labelApertureMaximum.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelApertureMaximum.Location = new System.Drawing.Point(44, 71);
            this.labelApertureMaximum.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelApertureMaximum.Name = "labelApertureMaximum";
            this.labelApertureMaximum.Size = new System.Drawing.Size(320, 39);
            this.labelApertureMaximum.TabIndex = 0;
            this.labelApertureMaximum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // captionApertureMaximum
            // 
            this.captionApertureMaximum.AutoSize = true;
            this.captionApertureMaximum.Location = new System.Drawing.Point(39, 46);
            this.captionApertureMaximum.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.captionApertureMaximum.Name = "captionApertureMaximum";
            this.captionApertureMaximum.Size = new System.Drawing.Size(97, 25);
            this.captionApertureMaximum.TabIndex = 6;
            this.captionApertureMaximum.Text = "Maximum";
            // 
            // numericUpDownAperture
            // 
            this.numericUpDownAperture.DecimalPlaces = 2;
            this.numericUpDownAperture.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownAperture.Location = new System.Drawing.Point(44, 166);
            this.numericUpDownAperture.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            131072});
            this.numericUpDownAperture.Name = "numericUpDownAperture";
            this.numericUpDownAperture.Size = new System.Drawing.Size(320, 30);
            this.numericUpDownAperture.TabIndex = 2;
            this.numericUpDownAperture.Value = new decimal(new int[] {
            56,
            0,
            0,
            65536});
            this.numericUpDownAperture.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDownAperture_KeyDown);
            // 
            // buttonSetAperture
            // 
            this.buttonSetAperture.Location = new System.Drawing.Point(397, 158);
            this.buttonSetAperture.Name = "buttonSetAperture";
            this.buttonSetAperture.Size = new System.Drawing.Size(144, 44);
            this.buttonSetAperture.TabIndex = 3;
            this.buttonSetAperture.Text = "Set";
            this.buttonSetAperture.UseVisualStyleBackColor = true;
            this.buttonSetAperture.Click += new System.EventHandler(this.buttonSetAperture_Click);
            // 
            // captionApertureValue
            // 
            this.captionApertureValue.AutoSize = true;
            this.captionApertureValue.Location = new System.Drawing.Point(39, 137);
            this.captionApertureValue.Name = "captionApertureValue";
            this.captionApertureValue.Size = new System.Drawing.Size(63, 25);
            this.captionApertureValue.TabIndex = 1;
            this.captionApertureValue.Text = "Value";
            // 
            // mainFormBindingSource
            // 
            this.mainFormBindingSource.DataSource = typeof(ASCOM.PinefeatCEF.App.MainForm);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 662);
            this.Controls.Add(this.groupBoxAperture);
            this.Controls.Add(this.groupBoxFocus);
            this.Controls.Add(this.labelDriverId);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.buttonProperties);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "ASCOM.PinefeatCEF.App";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.groupBoxFocus.ResumeLayout(false);
            this.groupBoxFocus.PerformLayout();
            this.groupBoxAperture.ResumeLayout(false);
            this.groupBoxAperture.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAperture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainFormBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonProperties;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Label labelDriverId;
        private System.Windows.Forms.BindingSource mainFormBindingSource;
        private System.Windows.Forms.Label captionFocusPosition;
        private System.Windows.Forms.Label captionMaximumFocus;
        private System.Windows.Forms.Label labelMaximumFocus;
        private System.Windows.Forms.Button buttonCalibrate;
        private System.Windows.Forms.Label labelFocusPosition;
        private System.Windows.Forms.GroupBox groupBoxFocus;
        private System.Windows.Forms.GroupBox groupBoxAperture;
        private System.Windows.Forms.Button buttonSetAperture;
        private System.Windows.Forms.Label captionApertureValue;
        private System.Windows.Forms.NumericUpDown numericUpDownAperture;
        private System.Windows.Forms.Label captionApertureMinimum;
        private System.Windows.Forms.Label labelApertureMinimum;
        private System.Windows.Forms.Label labelApertureMaximum;
        private System.Windows.Forms.Label captionApertureMaximum;
    }
}

