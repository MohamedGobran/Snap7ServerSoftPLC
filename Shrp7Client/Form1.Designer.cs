namespace Shrp7Client
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            splitContainer1 = new SplitContainer();
            grpDataBlock = new GroupBox();
            txtDataDump = new TextBox();
            btnReadDataBlock = new Button();
            numLength = new NumericUpDown();
            lblLength = new Label();
            numStartByte = new NumericUpDown();
            lblStartByte = new Label();
            numDBNumber = new NumericUpDown();
            lblDBNumber = new Label();
            grpConnection = new GroupBox();
            lblConnectionStatus = new Label();
            btnDisconnect = new Button();
            btnConnect = new Button();
            numSlot = new NumericUpDown();
            lblSlot = new Label();
            numRack = new NumericUpDown();
            lblRack = new Label();
            txtIP = new TextBox();
            lblIP = new Label();
            grpLog = new GroupBox();
            txtLog = new TextBox();
            grpVariable = new GroupBox();
            btnWriteVariable = new Button();
            btnReadVariable = new Button();
            txtVariableValue = new TextBox();
            lblValue = new Label();
            cmbVarDataType = new ComboBox();
            lblDataType = new Label();
            numVarBitOffset = new NumericUpDown();
            lblBitOffset = new Label();
            numVarOffset = new NumericUpDown();
            lblOffset = new Label();
            numVarDBNumber = new NumericUpDown();
            lblVarDB = new Label();
            lblVarAddress = new Label();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            grpDataBlock.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numLength).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numStartByte).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numDBNumber).BeginInit();
            grpConnection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numSlot).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numRack).BeginInit();
            grpLog.SuspendLayout();
            grpVariable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numVarBitOffset).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numVarOffset).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numVarDBNumber).BeginInit();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(grpDataBlock);
            splitContainer1.Panel1.Controls.Add(grpConnection);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(grpLog);
            splitContainer1.Panel2.Controls.Add(grpVariable);
            splitContainer1.Panel2.Paint += splitContainer1_Panel2_Paint;
            splitContainer1.Size = new Size(1050, 536);
            splitContainer1.SplitterDistance = 513;
            splitContainer1.TabIndex = 4;
            // 
            // grpDataBlock
            // 
            grpDataBlock.Controls.Add(txtDataDump);
            grpDataBlock.Controls.Add(btnReadDataBlock);
            grpDataBlock.Controls.Add(numLength);
            grpDataBlock.Controls.Add(lblLength);
            grpDataBlock.Controls.Add(numStartByte);
            grpDataBlock.Controls.Add(lblStartByte);
            grpDataBlock.Controls.Add(numDBNumber);
            grpDataBlock.Controls.Add(lblDBNumber);
            grpDataBlock.Dock = DockStyle.Fill;
            grpDataBlock.Location = new Point(0, 120);
            grpDataBlock.Name = "grpDataBlock";
            grpDataBlock.Size = new Size(513, 416);
            grpDataBlock.TabIndex = 3;
            grpDataBlock.TabStop = false;
            grpDataBlock.Text = "Data Block Dump";
            // 
            // txtDataDump
            // 
            txtDataDump.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtDataDump.Font = new Font("Consolas", 9F);
            txtDataDump.Location = new Point(3, 99);
            txtDataDump.Multiline = true;
            txtDataDump.Name = "txtDataDump";
            txtDataDump.ReadOnly = true;
            txtDataDump.ScrollBars = ScrollBars.Both;
            txtDataDump.Size = new Size(507, 314);
            txtDataDump.TabIndex = 7;
            txtDataDump.WordWrap = false;
            // 
            // btnReadDataBlock
            // 
            btnReadDataBlock.Enabled = false;
            btnReadDataBlock.Location = new Point(410, 50);
            btnReadDataBlock.Name = "btnReadDataBlock";
            btnReadDataBlock.Size = new Size(75, 23);
            btnReadDataBlock.TabIndex = 6;
            btnReadDataBlock.Text = "Read";
            btnReadDataBlock.UseVisualStyleBackColor = true;
            // 
            // numLength
            // 
            numLength.Location = new Point(320, 50);
            numLength.Maximum = new decimal(new int[] { 65536, 0, 0, 0 });
            numLength.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numLength.Name = "numLength";
            numLength.Size = new Size(75, 23);
            numLength.TabIndex = 5;
            numLength.Value = new decimal(new int[] { 64, 0, 0, 0 });
            // 
            // lblLength
            // 
            lblLength.AutoSize = true;
            lblLength.Location = new Point(270, 52);
            lblLength.Name = "lblLength";
            lblLength.Size = new Size(47, 15);
            lblLength.TabIndex = 4;
            lblLength.Text = "Length:";
            // 
            // numStartByte
            // 
            numStartByte.Location = new Point(180, 50);
            numStartByte.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            numStartByte.Name = "numStartByte";
            numStartByte.Size = new Size(75, 23);
            numStartByte.TabIndex = 3;
            // 
            // lblStartByte
            // 
            lblStartByte.AutoSize = true;
            lblStartByte.Location = new Point(120, 52);
            lblStartByte.Name = "lblStartByte";
            lblStartByte.Size = new Size(60, 15);
            lblStartByte.TabIndex = 2;
            lblStartByte.Text = "Start Byte:";
            // 
            // numDBNumber
            // 
            numDBNumber.Location = new Point(50, 50);
            numDBNumber.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            numDBNumber.Name = "numDBNumber";
            numDBNumber.Size = new Size(60, 23);
            numDBNumber.TabIndex = 1;
            numDBNumber.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblDBNumber
            // 
            lblDBNumber.AutoSize = true;
            lblDBNumber.Location = new Point(15, 52);
            lblDBNumber.Name = "lblDBNumber";
            lblDBNumber.Size = new Size(25, 15);
            lblDBNumber.TabIndex = 0;
            lblDBNumber.Text = "DB:";
            // 
            // grpConnection
            // 
            grpConnection.Controls.Add(lblConnectionStatus);
            grpConnection.Controls.Add(btnDisconnect);
            grpConnection.Controls.Add(btnConnect);
            grpConnection.Controls.Add(numSlot);
            grpConnection.Controls.Add(lblSlot);
            grpConnection.Controls.Add(numRack);
            grpConnection.Controls.Add(lblRack);
            grpConnection.Controls.Add(txtIP);
            grpConnection.Controls.Add(lblIP);
            grpConnection.Dock = DockStyle.Top;
            grpConnection.Location = new Point(0, 0);
            grpConnection.Name = "grpConnection";
            grpConnection.Size = new Size(513, 120);
            grpConnection.TabIndex = 2;
            grpConnection.TabStop = false;
            grpConnection.Text = "Connection";
            // 
            // lblConnectionStatus
            // 
            lblConnectionStatus.AutoSize = true;
            lblConnectionStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblConnectionStatus.ForeColor = Color.Red;
            lblConnectionStatus.Location = new Point(15, 95);
            lblConnectionStatus.Name = "lblConnectionStatus";
            lblConnectionStatus.Size = new Size(97, 15);
            lblConnectionStatus.TabIndex = 8;
            lblConnectionStatus.Text = "✗ Disconnected";
            // 
            // btnDisconnect
            // 
            btnDisconnect.Enabled = false;
            btnDisconnect.Location = new Point(410, 90);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(75, 23);
            btnDisconnect.TabIndex = 7;
            btnDisconnect.Text = "Disconnect";
            btnDisconnect.UseVisualStyleBackColor = true;
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(329, 90);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(75, 23);
            btnConnect.TabIndex = 6;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = true;
            // 
            // numSlot
            // 
            numSlot.Location = new Point(410, 50);
            numSlot.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
            numSlot.Name = "numSlot";
            numSlot.Size = new Size(75, 23);
            numSlot.TabIndex = 5;
            numSlot.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblSlot
            // 
            lblSlot.AutoSize = true;
            lblSlot.Location = new Point(375, 52);
            lblSlot.Name = "lblSlot";
            lblSlot.Size = new Size(30, 15);
            lblSlot.TabIndex = 4;
            lblSlot.Text = "Slot:";
            // 
            // numRack
            // 
            numRack.Location = new Point(280, 50);
            numRack.Maximum = new decimal(new int[] { 7, 0, 0, 0 });
            numRack.Name = "numRack";
            numRack.Size = new Size(75, 23);
            numRack.TabIndex = 3;
            // 
            // lblRack
            // 
            lblRack.AutoSize = true;
            lblRack.Location = new Point(240, 52);
            lblRack.Name = "lblRack";
            lblRack.Size = new Size(35, 15);
            lblRack.TabIndex = 2;
            lblRack.Text = "Rack:";
            // 
            // txtIP
            // 
            txtIP.Location = new Point(15, 50);
            txtIP.Name = "txtIP";
            txtIP.Size = new Size(200, 23);
            txtIP.TabIndex = 1;
            txtIP.Text = "127.0.0.1";
            // 
            // lblIP
            // 
            lblIP.AutoSize = true;
            lblIP.Location = new Point(15, 30);
            lblIP.Name = "lblIP";
            lblIP.Size = new Size(65, 15);
            lblIP.TabIndex = 0;
            lblIP.Text = "IP Address:";
            // 
            // grpLog
            // 
            grpLog.Controls.Add(txtLog);
            grpLog.Dock = DockStyle.Fill;
            grpLog.Location = new Point(0, 200);
            grpLog.Name = "grpLog";
            grpLog.Size = new Size(533, 336);
            grpLog.TabIndex = 5;
            grpLog.TabStop = false;
            grpLog.Text = "Log";
            // 
            // txtLog
            // 
            txtLog.Dock = DockStyle.Fill;
            txtLog.Font = new Font("Consolas", 9F);
            txtLog.Location = new Point(3, 19);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Both;
            txtLog.Size = new Size(527, 314);
            txtLog.TabIndex = 0;
            txtLog.WordWrap = false;
            // 
            // grpVariable
            // 
            grpVariable.Controls.Add(btnWriteVariable);
            grpVariable.Controls.Add(btnReadVariable);
            grpVariable.Controls.Add(txtVariableValue);
            grpVariable.Controls.Add(lblValue);
            grpVariable.Controls.Add(cmbVarDataType);
            grpVariable.Controls.Add(lblDataType);
            grpVariable.Controls.Add(numVarBitOffset);
            grpVariable.Controls.Add(lblBitOffset);
            grpVariable.Controls.Add(numVarOffset);
            grpVariable.Controls.Add(lblOffset);
            grpVariable.Controls.Add(numVarDBNumber);
            grpVariable.Controls.Add(lblVarDB);
            grpVariable.Controls.Add(lblVarAddress);
            grpVariable.Dock = DockStyle.Top;
            grpVariable.Location = new Point(0, 0);
            grpVariable.Name = "grpVariable";
            grpVariable.Size = new Size(533, 200);
            grpVariable.TabIndex = 4;
            grpVariable.TabStop = false;
            grpVariable.Text = "Variable Operations";
            // 
            // btnWriteVariable
            // 
            btnWriteVariable.Enabled = false;
            btnWriteVariable.Location = new Point(420, 160);
            btnWriteVariable.Name = "btnWriteVariable";
            btnWriteVariable.Size = new Size(65, 23);
            btnWriteVariable.TabIndex = 12;
            btnWriteVariable.Text = "Write";
            btnWriteVariable.UseVisualStyleBackColor = true;
            // 
            // btnReadVariable
            // 
            btnReadVariable.Enabled = false;
            btnReadVariable.Location = new Point(350, 160);
            btnReadVariable.Name = "btnReadVariable";
            btnReadVariable.Size = new Size(65, 23);
            btnReadVariable.TabIndex = 11;
            btnReadVariable.Text = "Read";
            btnReadVariable.UseVisualStyleBackColor = true;
            // 
            // txtVariableValue
            // 
            txtVariableValue.Location = new Point(70, 160);
            txtVariableValue.Name = "txtVariableValue";
            txtVariableValue.Size = new Size(270, 23);
            txtVariableValue.TabIndex = 10;
            // 
            // lblValue
            // 
            lblValue.AutoSize = true;
            lblValue.Location = new Point(15, 163);
            lblValue.Name = "lblValue";
            lblValue.Size = new Size(38, 15);
            lblValue.TabIndex = 9;
            lblValue.Text = "Value:";
            // 
            // cmbVarDataType
            // 
            cmbVarDataType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbVarDataType.FormattingEnabled = true;
            cmbVarDataType.Items.AddRange(new object[] { "BOOL", "BYTE", "WORD", "DWORD", "INT", "DINT", "REAL", "STRING" });
            cmbVarDataType.Location = new Point(80, 120);
            cmbVarDataType.Name = "cmbVarDataType";
            cmbVarDataType.Size = new Size(100, 23);
            cmbVarDataType.TabIndex = 8;
            // 
            // lblDataType
            // 
            lblDataType.AutoSize = true;
            lblDataType.Location = new Point(15, 123);
            lblDataType.Name = "lblDataType";
            lblDataType.Size = new Size(61, 15);
            lblDataType.TabIndex = 7;
            lblDataType.Text = "Data Type:";
            // 
            // numVarBitOffset
            // 
            numVarBitOffset.Location = new Point(300, 80);
            numVarBitOffset.Maximum = new decimal(new int[] { 7, 0, 0, 0 });
            numVarBitOffset.Name = "numVarBitOffset";
            numVarBitOffset.Size = new Size(50, 23);
            numVarBitOffset.TabIndex = 6;
            // 
            // lblBitOffset
            // 
            lblBitOffset.AutoSize = true;
            lblBitOffset.Location = new Point(250, 82);
            lblBitOffset.Name = "lblBitOffset";
            lblBitOffset.Size = new Size(52, 15);
            lblBitOffset.TabIndex = 5;
            lblBitOffset.Text = "Bit (0-7):";
            // 
            // numVarOffset
            // 
            numVarOffset.Location = new Point(160, 80);
            numVarOffset.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            numVarOffset.Name = "numVarOffset";
            numVarOffset.Size = new Size(75, 23);
            numVarOffset.TabIndex = 4;
            // 
            // lblOffset
            // 
            lblOffset.AutoSize = true;
            lblOffset.Location = new Point(115, 82);
            lblOffset.Name = "lblOffset";
            lblOffset.Size = new Size(42, 15);
            lblOffset.TabIndex = 3;
            lblOffset.Text = "Offset:";
            // 
            // numVarDBNumber
            // 
            numVarDBNumber.Location = new Point(50, 80);
            numVarDBNumber.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            numVarDBNumber.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numVarDBNumber.Name = "numVarDBNumber";
            numVarDBNumber.Size = new Size(60, 23);
            numVarDBNumber.TabIndex = 2;
            numVarDBNumber.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblVarDB
            // 
            lblVarDB.AutoSize = true;
            lblVarDB.Location = new Point(15, 82);
            lblVarDB.Name = "lblVarDB";
            lblVarDB.Size = new Size(25, 15);
            lblVarDB.TabIndex = 1;
            lblVarDB.Text = "DB:";
            // 
            // lblVarAddress
            // 
            lblVarAddress.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblVarAddress.Location = new Point(15, 30);
            lblVarAddress.Name = "lblVarAddress";
            lblVarAddress.Size = new Size(470, 40);
            lblVarAddress.TabIndex = 0;
            lblVarAddress.Text = "Address (DB1.0.0):";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1050, 536);
            Controls.Add(splitContainer1);
            Name = "Form1";
            Text = "Sharp7 PLC Client - Test Tool";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            grpDataBlock.ResumeLayout(false);
            grpDataBlock.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numLength).EndInit();
            ((System.ComponentModel.ISupportInitialize)numStartByte).EndInit();
            ((System.ComponentModel.ISupportInitialize)numDBNumber).EndInit();
            grpConnection.ResumeLayout(false);
            grpConnection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numSlot).EndInit();
            ((System.ComponentModel.ISupportInitialize)numRack).EndInit();
            grpLog.ResumeLayout(false);
            grpLog.PerformLayout();
            grpVariable.ResumeLayout(false);
            grpVariable.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numVarBitOffset).EndInit();
            ((System.ComponentModel.ISupportInitialize)numVarOffset).EndInit();
            ((System.ComponentModel.ISupportInitialize)numVarDBNumber).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private SplitContainer splitContainer1;
        private GroupBox grpDataBlock;
        private TextBox txtDataDump;
        private Button btnReadDataBlock;
        private NumericUpDown numLength;
        private Label lblLength;
        private NumericUpDown numStartByte;
        private Label lblStartByte;
        private NumericUpDown numDBNumber;
        private Label lblDBNumber;
        private GroupBox grpConnection;
        private Label lblConnectionStatus;
        private Button btnDisconnect;
        private Button btnConnect;
        private NumericUpDown numSlot;
        private Label lblSlot;
        private NumericUpDown numRack;
        private Label lblRack;
        private TextBox txtIP;
        private Label lblIP;
        private GroupBox grpLog;
        private TextBox txtLog;
        private GroupBox grpVariable;
        private Button btnWriteVariable;
        private Button btnReadVariable;
        private TextBox txtVariableValue;
        private Label lblValue;
        private ComboBox cmbVarDataType;
        private Label lblDataType;
        private NumericUpDown numVarBitOffset;
        private Label lblBitOffset;
        private NumericUpDown numVarOffset;
        private Label lblOffset;
        private NumericUpDown numVarDBNumber;
        private Label lblVarDB;
        private Label lblVarAddress;
    }
}