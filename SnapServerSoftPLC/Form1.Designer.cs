namespace SnapServerSoftPLC
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
            components = new System.ComponentModel.Container();
            btnStartPLC = new Button();
            btnStopPLC = new Button();
            btnNetworkConfig = new Button();
            lblStatus = new Label();
            btnEditDataBlock = new Button();
            btnCopyDataBlock = new Button();
            btnEditVariable = new Button();
            btnDeleteVariable = new Button();
            grpStatus = new GroupBox();
            lblClients = new Label();
            lblServerStatus = new Label();
            grpDataBlocks = new GroupBox();
            btnRemoveDataBlock = new Button();
            btnAddDataBlock = new Button();
            lstDataBlocks = new ListBox();
            grpVariables = new GroupBox();
            btnUpdateValue = new Button();
            btnGroupAddVariable = new Button();
            btnAddVariable = new Button();
            lstVariables = new ListView();
            colVarName = new ColumnHeader();
            colVarType = new ColumnHeader();
            colVarOffset = new ColumnHeader();
            colVarValue = new ColumnHeader();
            colVarComment = new ColumnHeader();
            txtLog = new TextBox();
            grpLog = new GroupBox();
            statusStrip = new StatusStrip();
            statusLabel = new ToolStripStatusLabel();
            timerStatus = new System.Windows.Forms.Timer(components);
            grpStatus.SuspendLayout();
            grpDataBlocks.SuspendLayout();
            grpVariables.SuspendLayout();
            grpLog.SuspendLayout();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // btnStartPLC
            // 
            btnStartPLC.BackColor = Color.LightGreen;
            btnStartPLC.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            btnStartPLC.Location = new Point(14, 14);
            btnStartPLC.Margin = new Padding(4, 3, 4, 3);
            btnStartPLC.Name = "btnStartPLC";
            btnStartPLC.Size = new Size(117, 40);
            btnStartPLC.TabIndex = 0;
            btnStartPLC.Text = "Start PLC";
            btnStartPLC.UseVisualStyleBackColor = false;
            btnStartPLC.Click += btnStartPLC_Click;
            // 
            // btnStopPLC
            // 
            btnStopPLC.BackColor = Color.LightCoral;
            btnStopPLC.Enabled = false;
            btnStopPLC.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            btnStopPLC.Location = new Point(138, 14);
            btnStopPLC.Margin = new Padding(4, 3, 4, 3);
            btnStopPLC.Name = "btnStopPLC";
            btnStopPLC.Size = new Size(117, 40);
            btnStopPLC.TabIndex = 1;
            btnStopPLC.Text = "Stop PLC";
            btnStopPLC.UseVisualStyleBackColor = false;
            btnStopPLC.Click += btnStopPLC_Click;
            // 
            // btnNetworkConfig
            // 
            btnNetworkConfig.Location = new Point(618, 14);
            btnNetworkConfig.Margin = new Padding(4, 3, 4, 3);
            btnNetworkConfig.Name = "btnNetworkConfig";
            btnNetworkConfig.Size = new Size(117, 40);
            btnNetworkConfig.TabIndex = 17;
            btnNetworkConfig.Text = "Network Config";
            btnNetworkConfig.UseVisualStyleBackColor = true;
            btnNetworkConfig.Click += btnNetworkConfig_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            lblStatus.ForeColor = Color.Red;
            lblStatus.Location = new Point(7, 18);
            lblStatus.Margin = new Padding(4, 0, 4, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(60, 15);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "Stopped";
            // 
            // btnEditDataBlock
            // 
            btnEditDataBlock.Location = new Point(196, 183);
            btnEditDataBlock.Margin = new Padding(4, 3, 4, 3);
            btnEditDataBlock.Name = "btnEditDataBlock";
            btnEditDataBlock.Size = new Size(88, 27);
            btnEditDataBlock.TabIndex = 3;
            btnEditDataBlock.Text = "Edit DB";
            btnEditDataBlock.UseVisualStyleBackColor = true;
            btnEditDataBlock.Click += btnEditDataBlock_Click;
            // 
            // btnCopyDataBlock
            // 
            btnCopyDataBlock.Location = new Point(7, 217);
            btnCopyDataBlock.Margin = new Padding(4, 3, 4, 3);
            btnCopyDataBlock.Name = "btnCopyDataBlock";
            btnCopyDataBlock.Size = new Size(88, 27);
            btnCopyDataBlock.TabIndex = 4;
            btnCopyDataBlock.Text = "Copy DB";
            btnCopyDataBlock.UseVisualStyleBackColor = true;
            btnCopyDataBlock.Click += btnCopyDataBlock_Click;
            // 
            // btnEditVariable
            // 
            btnEditVariable.Location = new Point(333, 183);
            btnEditVariable.Margin = new Padding(4, 3, 4, 3);
            btnEditVariable.Name = "btnEditVariable";
            btnEditVariable.Size = new Size(88, 27);
            btnEditVariable.TabIndex = 3;
            btnEditVariable.Text = "Edit Variable";
            btnEditVariable.UseVisualStyleBackColor = true;
            btnEditVariable.Click += btnEditVariable_Click;
            // 
            // btnDeleteVariable
            // 
            btnDeleteVariable.Location = new Point(438, 183);
            btnDeleteVariable.Margin = new Padding(4, 3, 4, 3);
            btnDeleteVariable.Name = "btnDeleteVariable";
            btnDeleteVariable.Size = new Size(88, 27);
            btnDeleteVariable.TabIndex = 4;
            btnDeleteVariable.Text = "Delete Variable";
            btnDeleteVariable.UseVisualStyleBackColor = true;
            btnDeleteVariable.Click += btnDeleteVariable_Click;
            // 
            // grpStatus
            // 
            grpStatus.Controls.Add(lblClients);
            grpStatus.Controls.Add(lblServerStatus);
            grpStatus.Controls.Add(lblStatus);
            grpStatus.Location = new Point(261, 14);
            grpStatus.Margin = new Padding(4, 3, 4, 3);
            grpStatus.Name = "grpStatus";
            grpStatus.Padding = new Padding(4, 3, 4, 3);
            grpStatus.Size = new Size(350, 92);
            grpStatus.TabIndex = 2;
            grpStatus.TabStop = false;
            grpStatus.Text = "Server Status";
            // 
            // lblClients
            // 
            lblClients.AutoSize = true;
            lblClients.Location = new Point(7, 60);
            lblClients.Margin = new Padding(4, 0, 4, 0);
            lblClients.Name = "lblClients";
            lblClients.Size = new Size(55, 15);
            lblClients.TabIndex = 2;
            lblClients.Text = "Clients: 0";
            // 
            // lblServerStatus
            // 
            lblServerStatus.AutoSize = true;
            lblServerStatus.Location = new Point(7, 40);
            lblServerStatus.Margin = new Padding(4, 0, 4, 0);
            lblServerStatus.Name = "lblServerStatus";
            lblServerStatus.Size = new Size(64, 15);
            lblServerStatus.TabIndex = 1;
            lblServerStatus.Text = "Server: Idle";
            // 
            // grpDataBlocks
            // 
            grpDataBlocks.Controls.Add(btnCopyDataBlock);
            grpDataBlocks.Controls.Add(btnEditDataBlock);
            grpDataBlocks.Controls.Add(btnRemoveDataBlock);
            grpDataBlocks.Controls.Add(btnAddDataBlock);
            grpDataBlocks.Controls.Add(lstDataBlocks);
            grpDataBlocks.Location = new Point(14, 113);
            grpDataBlocks.Margin = new Padding(4, 3, 4, 3);
            grpDataBlocks.Name = "grpDataBlocks";
            grpDataBlocks.Padding = new Padding(4, 3, 4, 3);
            grpDataBlocks.Size = new Size(292, 254);
            grpDataBlocks.TabIndex = 3;
            grpDataBlocks.TabStop = false;
            grpDataBlocks.Text = "Data Blocks";
            // 
            // btnRemoveDataBlock
            // 
            btnRemoveDataBlock.Location = new Point(102, 183);
            btnRemoveDataBlock.Margin = new Padding(4, 3, 4, 3);
            btnRemoveDataBlock.Name = "btnRemoveDataBlock";
            btnRemoveDataBlock.Size = new Size(88, 27);
            btnRemoveDataBlock.TabIndex = 2;
            btnRemoveDataBlock.Text = "Remove DB";
            btnRemoveDataBlock.UseVisualStyleBackColor = true;
            btnRemoveDataBlock.Click += btnRemoveDataBlock_Click;
            // 
            // btnAddDataBlock
            // 
            btnAddDataBlock.Location = new Point(7, 183);
            btnAddDataBlock.Margin = new Padding(4, 3, 4, 3);
            btnAddDataBlock.Name = "btnAddDataBlock";
            btnAddDataBlock.Size = new Size(88, 27);
            btnAddDataBlock.TabIndex = 1;
            btnAddDataBlock.Text = "Add DB";
            btnAddDataBlock.UseVisualStyleBackColor = true;
            btnAddDataBlock.Click += btnAddDataBlock_Click;
            // 
            // lstDataBlocks
            // 
            lstDataBlocks.FormattingEnabled = true;
            lstDataBlocks.Location = new Point(7, 22);
            lstDataBlocks.Margin = new Padding(4, 3, 4, 3);
            lstDataBlocks.Name = "lstDataBlocks";
            lstDataBlocks.Size = new Size(277, 154);
            lstDataBlocks.TabIndex = 0;
            lstDataBlocks.SelectedIndexChanged += lstDataBlocks_SelectedIndexChanged;
            // 
            // grpVariables
            // 
            grpVariables.Controls.Add(btnDeleteVariable);
            grpVariables.Controls.Add(btnEditVariable);
            grpVariables.Controls.Add(btnUpdateValue);
            grpVariables.Controls.Add(btnGroupAddVariable);
            grpVariables.Controls.Add(btnAddVariable);
            grpVariables.Controls.Add(lstVariables);
            grpVariables.Location = new Point(313, 113);
            grpVariables.Margin = new Padding(4, 3, 4, 3);
            grpVariables.Name = "grpVariables";
            grpVariables.Padding = new Padding(4, 3, 4, 3);
            grpVariables.Size = new Size(607, 254);
            grpVariables.TabIndex = 4;
            grpVariables.TabStop = false;
            grpVariables.Text = "Variables";
            // 
            // btnUpdateValue
            // 
            btnUpdateValue.Location = new Point(217, 183);
            btnUpdateValue.Margin = new Padding(4, 3, 4, 3);
            btnUpdateValue.Name = "btnUpdateValue";
            btnUpdateValue.Size = new Size(99, 27);
            btnUpdateValue.TabIndex = 2;
            btnUpdateValue.Text = "Update Value";
            btnUpdateValue.UseVisualStyleBackColor = true;
            btnUpdateValue.Click += btnUpdateValue_Click;
            // 
            // btnGroupAddVariable
            // 
            btnGroupAddVariable.Location = new Point(112, 183);
            btnGroupAddVariable.Margin = new Padding(4, 3, 4, 3);
            btnGroupAddVariable.Name = "btnGroupAddVariable";
            btnGroupAddVariable.Size = new Size(88, 27);
            btnGroupAddVariable.TabIndex = 2;
            btnGroupAddVariable.Text = "Add Group";
            btnGroupAddVariable.UseVisualStyleBackColor = true;
            btnGroupAddVariable.Click += btnGroupAddVariable_Click;
            // 
            // btnAddVariable
            // 
            btnAddVariable.Location = new Point(7, 183);
            btnAddVariable.Margin = new Padding(4, 3, 4, 3);
            btnAddVariable.Name = "btnAddVariable";
            btnAddVariable.Size = new Size(88, 27);
            btnAddVariable.TabIndex = 1;
            btnAddVariable.Text = "Add Variable";
            btnAddVariable.UseVisualStyleBackColor = true;
            btnAddVariable.Click += btnAddVariable_Click;
            // 
            // lstVariables
            // 
            lstVariables.Columns.AddRange(new ColumnHeader[] { colVarName, colVarType, colVarOffset, colVarValue, colVarComment });
            lstVariables.FullRowSelect = true;
            lstVariables.GridLines = true;
            lstVariables.Location = new Point(7, 22);
            lstVariables.Margin = new Padding(4, 3, 4, 3);
            lstVariables.Name = "lstVariables";
            lstVariables.Size = new Size(592, 154);
            lstVariables.TabIndex = 0;
            lstVariables.UseCompatibleStateImageBehavior = false;
            lstVariables.View = View.Details;
            // 
            // colVarName
            // 
            colVarName.Text = "Name";
            colVarName.Width = 100;
            // 
            // colVarType
            // 
            colVarType.Text = "Type";
            // 
            // colVarOffset
            // 
            colVarOffset.Text = "Offset";
            colVarOffset.Width = 50;
            // 
            // colVarValue
            // 
            colVarValue.Text = "Value";
            colVarValue.Width = 80;
            // 
            // colVarComment
            // 
            colVarComment.Text = "Comment";
            colVarComment.Width = 200;
            // 
            // txtLog
            // 
            txtLog.Location = new Point(7, 22);
            txtLog.Margin = new Padding(4, 3, 4, 3);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(891, 97);
            txtLog.TabIndex = 0;
            // 
            // grpLog
            // 
            grpLog.Controls.Add(txtLog);
            grpLog.Location = new Point(14, 374);
            grpLog.Margin = new Padding(4, 3, 4, 3);
            grpLog.Name = "grpLog";
            grpLog.Padding = new Padding(4, 3, 4, 3);
            grpLog.Size = new Size(905, 127);
            grpLog.TabIndex = 5;
            grpLog.TabStop = false;
            grpLog.Text = "Log";
            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new ToolStripItem[] { statusLabel });
            statusStrip.Location = new Point(0, 520);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new Padding(1, 0, 16, 0);
            statusStrip.Size = new Size(933, 22);
            statusStrip.TabIndex = 6;
            statusStrip.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(39, 17);
            statusLabel.Text = "Ready";
            // 
            // timerStatus
            // 
            timerStatus.Interval = 1000;
            timerStatus.Tick += timerStatus_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(933, 542);
            Controls.Add(statusStrip);
            Controls.Add(grpLog);
            Controls.Add(grpVariables);
            Controls.Add(grpDataBlocks);
            Controls.Add(grpStatus);
            Controls.Add(btnNetworkConfig);
            Controls.Add(btnStopPLC);
            Controls.Add(btnStartPLC);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "Form1";
            Text = "Snap7 Soft PLC Server";
            FormClosing += Form1_FormClosing;
            grpStatus.ResumeLayout(false);
            grpStatus.PerformLayout();
            grpDataBlocks.ResumeLayout(false);
            grpVariables.ResumeLayout(false);
            grpLog.ResumeLayout(false);
            grpLog.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnStartPLC;
        private System.Windows.Forms.Button btnStopPLC;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.GroupBox grpStatus;
        private System.Windows.Forms.Label lblClients;
        private System.Windows.Forms.Label lblServerStatus;
        private System.Windows.Forms.GroupBox grpDataBlocks;
        private System.Windows.Forms.Button btnAddDataBlock;
        private System.Windows.Forms.Button btnRemoveDataBlock;
        private System.Windows.Forms.ListBox lstDataBlocks;
        private System.Windows.Forms.GroupBox grpVariables;
        private System.Windows.Forms.Button btnAddVariable;
        private System.Windows.Forms.Button btnGroupAddVariable;
        private System.Windows.Forms.Button btnUpdateValue;
        private System.Windows.Forms.ListView lstVariables;
        private System.Windows.Forms.ColumnHeader colVarName;
        private System.Windows.Forms.ColumnHeader colVarType;
        private System.Windows.Forms.ColumnHeader colVarOffset;
        private System.Windows.Forms.ColumnHeader colVarValue;
        private System.Windows.Forms.ColumnHeader colVarComment;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.GroupBox grpLog;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.Timer timerStatus;
        private System.Windows.Forms.Button btnEditDataBlock;
        private System.Windows.Forms.Button btnCopyDataBlock;
        private System.Windows.Forms.Button btnEditVariable;
        private System.Windows.Forms.Button btnDeleteVariable;
        private System.Windows.Forms.Button btnNetworkConfig;
    }
}
