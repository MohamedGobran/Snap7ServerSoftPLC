namespace SnapServerSoftPLC
{
    partial class NetworkConfigDialog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblPort = new System.Windows.Forms.Label();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.lblBindAddress = new System.Windows.Forms.Label();
            this.txtBindAddress = new System.Windows.Forms.ComboBox();
            this.lblRack = new System.Windows.Forms.Label();
            this.numRack = new System.Windows.Forms.NumericUpDown();
            this.lblSlot = new System.Windows.Forms.Label();
            this.numSlot = new System.Windows.Forms.NumericUpDown();
            this.chkAutoStart = new System.Windows.Forms.CheckBox();
            this.lblServerName = new System.Windows.Forms.Label();
            this.txtServerName = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnTestBinding = new System.Windows.Forms.Button();
            this.grpNetwork = new System.Windows.Forms.GroupBox();
            this.grpS7Config = new System.Windows.Forms.GroupBox();
            this.grpGeneral = new System.Windows.Forms.GroupBox();
            this.lblTSAPInfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSlot)).BeginInit();
            this.grpNetwork.SuspendLayout();
            this.grpS7Config.SuspendLayout();
            this.grpGeneral.SuspendLayout();
            this.SuspendLayout();
            
            // lblPort
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(6, 25);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(29, 13);
            this.lblPort.TabIndex = 0;
            this.lblPort.Text = "Port:";
            
            // numPort
            this.numPort.Location = new System.Drawing.Point(100, 23);
            this.numPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            this.numPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numPort.Name = "numPort";
            this.numPort.Size = new System.Drawing.Size(80, 20);
            this.numPort.TabIndex = 1;
            this.numPort.Value = new decimal(new int[] { 102, 0, 0, 0 });
            
            // lblBindAddress
            this.lblBindAddress.AutoSize = true;
            this.lblBindAddress.Location = new System.Drawing.Point(6, 52);
            this.lblBindAddress.Name = "lblBindAddress";
            this.lblBindAddress.Size = new System.Drawing.Size(67, 13);
            this.lblBindAddress.TabIndex = 2;
            this.lblBindAddress.Text = "Bind Address:";
            
            // txtBindAddress
            this.txtBindAddress.FormattingEnabled = true;
            this.txtBindAddress.Location = new System.Drawing.Point(100, 49);
            this.txtBindAddress.Name = "txtBindAddress";
            this.txtBindAddress.Size = new System.Drawing.Size(180, 21);
            this.txtBindAddress.TabIndex = 3;
            this.txtBindAddress.Text = "0.0.0.0";
            
            // btnTestBinding
            this.btnTestBinding.Location = new System.Drawing.Point(290, 47);
            this.btnTestBinding.Name = "btnTestBinding";
            this.btnTestBinding.Size = new System.Drawing.Size(75, 23);
            this.btnTestBinding.TabIndex = 4;
            this.btnTestBinding.Text = "Test";
            this.btnTestBinding.UseVisualStyleBackColor = true;
            this.btnTestBinding.Click += new System.EventHandler(this.btnTestBinding_Click);
            
            // lblRack
            this.lblRack.AutoSize = true;
            this.lblRack.Location = new System.Drawing.Point(6, 25);
            this.lblRack.Name = "lblRack";
            this.lblRack.Size = new System.Drawing.Size(33, 13);
            this.lblRack.TabIndex = 5;
            this.lblRack.Text = "Rack:";
            
            // numRack
            this.numRack.Location = new System.Drawing.Point(70, 23);
            this.numRack.Maximum = new decimal(new int[] { 7, 0, 0, 0 });
            this.numRack.Name = "numRack";
            this.numRack.Size = new System.Drawing.Size(60, 20);
            this.numRack.TabIndex = 6;
            
            // lblSlot
            this.lblSlot.AutoSize = true;
            this.lblSlot.Location = new System.Drawing.Point(150, 25);
            this.lblSlot.Name = "lblSlot";
            this.lblSlot.Size = new System.Drawing.Size(28, 13);
            this.lblSlot.TabIndex = 7;
            this.lblSlot.Text = "Slot:";
            
            // numSlot
            this.numSlot.Location = new System.Drawing.Point(190, 23);
            this.numSlot.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
            this.numSlot.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numSlot.Name = "numSlot";
            this.numSlot.Size = new System.Drawing.Size(60, 20);
            this.numSlot.TabIndex = 8;
            this.numSlot.Value = new decimal(new int[] { 1, 0, 0, 0 });
            
            // lblServerName
            this.lblServerName.AutoSize = true;
            this.lblServerName.Location = new System.Drawing.Point(6, 25);
            this.lblServerName.Name = "lblServerName";
            this.lblServerName.Size = new System.Drawing.Size(69, 13);
            this.lblServerName.TabIndex = 9;
            this.lblServerName.Text = "Server Name:";
            
            // txtServerName
            this.txtServerName.Location = new System.Drawing.Point(90, 22);
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.Size = new System.Drawing.Size(200, 20);
            this.txtServerName.TabIndex = 10;
            this.txtServerName.Text = "Snap7 Soft PLC";
            
            // chkAutoStart
            this.chkAutoStart.AutoSize = true;
            this.chkAutoStart.Location = new System.Drawing.Point(90, 52);
            this.chkAutoStart.Name = "chkAutoStart";
            this.chkAutoStart.Size = new System.Drawing.Size(145, 17);
            this.chkAutoStart.TabIndex = 11;
            this.chkAutoStart.Text = "Start automatically on launch";
            this.chkAutoStart.UseVisualStyleBackColor = true;
            
            // grpNetwork
            this.grpNetwork.Controls.Add(this.lblPort);
            this.grpNetwork.Controls.Add(this.numPort);
            this.grpNetwork.Controls.Add(this.lblBindAddress);
            this.grpNetwork.Controls.Add(this.txtBindAddress);
            this.grpNetwork.Controls.Add(this.btnTestBinding);
            this.grpNetwork.Location = new System.Drawing.Point(12, 12);
            this.grpNetwork.Name = "grpNetwork";
            this.grpNetwork.Size = new System.Drawing.Size(380, 85);
            this.grpNetwork.TabIndex = 12;
            this.grpNetwork.TabStop = false;
            this.grpNetwork.Text = "Network Configuration";
            
            // grpS7Config
            this.grpS7Config.Controls.Add(this.lblRack);
            this.grpS7Config.Controls.Add(this.numRack);
            this.grpS7Config.Controls.Add(this.lblSlot);
            this.grpS7Config.Controls.Add(this.numSlot);
            this.grpS7Config.Controls.Add(this.lblTSAPInfo);
            this.grpS7Config.Location = new System.Drawing.Point(12, 110);
            this.grpS7Config.Name = "grpS7Config";
            this.grpS7Config.Size = new System.Drawing.Size(380, 85);
            this.grpS7Config.TabIndex = 13;
            this.grpS7Config.TabStop = false;
            this.grpS7Config.Text = "S7 Configuration (Client Connection Parameters)";
            
            // lblTSAPInfo
            this.lblTSAPInfo.AutoSize = true;
            this.lblTSAPInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Italic);
            this.lblTSAPInfo.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblTSAPInfo.Location = new System.Drawing.Point(6, 52);
            this.lblTSAPInfo.Name = "lblTSAPInfo";
            this.lblTSAPInfo.Size = new System.Drawing.Size(350, 26);
            this.lblTSAPInfo.TabIndex = 9;
            this.lblTSAPInfo.Text = "Note: These values are for client configuration reference.\nServer accepts all valid S7 client connections.";
            
            // grpGeneral
            this.grpGeneral.Controls.Add(this.lblServerName);
            this.grpGeneral.Controls.Add(this.txtServerName);
            this.grpGeneral.Controls.Add(this.chkAutoStart);
            this.grpGeneral.Location = new System.Drawing.Point(12, 205);
            this.grpGeneral.Name = "grpGeneral";
            this.grpGeneral.Size = new System.Drawing.Size(380, 85);
            this.grpGeneral.TabIndex = 14;
            this.grpGeneral.TabStop = false;
            this.grpGeneral.Text = "General Settings";
            
            // btnOK
            this.btnOK.Location = new System.Drawing.Point(236, 305);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            
            // btnCancel
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(317, 305);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            
            // NetworkConfigDialog
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(404, 340);
            this.Controls.Add(this.grpNetwork);
            this.Controls.Add(this.grpS7Config);
            this.Controls.Add(this.grpGeneral);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NetworkConfigDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Network Configuration";
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSlot)).EndInit();
            this.grpNetwork.ResumeLayout(false);
            this.grpNetwork.PerformLayout();
            this.grpS7Config.ResumeLayout(false);
            this.grpS7Config.PerformLayout();
            this.grpGeneral.ResumeLayout(false);
            this.grpGeneral.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.Label lblBindAddress;
        private System.Windows.Forms.ComboBox txtBindAddress;
        private System.Windows.Forms.Label lblRack;
        private System.Windows.Forms.NumericUpDown numRack;
        private System.Windows.Forms.Label lblSlot;
        private System.Windows.Forms.NumericUpDown numSlot;
        private System.Windows.Forms.CheckBox chkAutoStart;
        private System.Windows.Forms.Label lblServerName;
        private System.Windows.Forms.TextBox txtServerName;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnTestBinding;
        private System.Windows.Forms.GroupBox grpNetwork;
        private System.Windows.Forms.GroupBox grpS7Config;
        private System.Windows.Forms.GroupBox grpGeneral;
        private System.Windows.Forms.Label lblTSAPInfo;
    }
}