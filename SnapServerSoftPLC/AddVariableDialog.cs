using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace SnapServerSoftPLC
{
    public partial class AddVariableDialog : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string VarName { get; private set; } = "";
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string VarType { get; private set; } = "BOOL";
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int VarOffset { get; private set; }
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string VarComment { get; private set; } = "";

        private TextBox txtVarName;
        private ComboBox cmbVarType;
        private NumericUpDown numVarOffset;
        private TextBox txtVarComment;
        private Button btnOK;
        private Button btnCancel;
        private Label lblVarName;
        private Label lblVarType;
        private Label lblVarOffset;
        private Label lblVarComment;

        public AddVariableDialog()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.txtVarName = new TextBox();
            this.cmbVarType = new ComboBox();
            this.numVarOffset = new NumericUpDown();
            this.txtVarComment = new TextBox();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.lblVarName = new Label();
            this.lblVarType = new Label();
            this.lblVarOffset = new Label();
            this.lblVarComment = new Label();
            this.SuspendLayout();

            // lblVarName
            this.lblVarName.AutoSize = true;
            this.lblVarName.Location = new System.Drawing.Point(12, 15);
            this.lblVarName.Name = "lblVarName";
            this.lblVarName.Size = new System.Drawing.Size(38, 13);
            this.lblVarName.Text = "Name:";

            // txtVarName
            this.txtVarName.Location = new System.Drawing.Point(100, 13);
            this.txtVarName.Name = "txtVarName";
            this.txtVarName.Size = new System.Drawing.Size(200, 20);

            // lblVarType
            this.lblVarType.AutoSize = true;
            this.lblVarType.Location = new System.Drawing.Point(12, 41);
            this.lblVarType.Name = "lblVarType";
            this.lblVarType.Size = new System.Drawing.Size(61, 13);
            this.lblVarType.Text = "Data Type:";

            // cmbVarType
            this.cmbVarType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbVarType.FormattingEnabled = true;
            this.cmbVarType.Items.AddRange(new object[] {
                "BOOL",
                "BYTE",
                "WORD",
                "DWORD",
                "INT",
                "DINT",
                "REAL",
                "STRING"
            });
            this.cmbVarType.Location = new System.Drawing.Point(100, 39);
            this.cmbVarType.Name = "cmbVarType";
            this.cmbVarType.Size = new System.Drawing.Size(120, 21);
            this.cmbVarType.SelectedIndex = 0;

            // lblVarOffset
            this.lblVarOffset.AutoSize = true;
            this.lblVarOffset.Location = new System.Drawing.Point(12, 67);
            this.lblVarOffset.Name = "lblVarOffset";
            this.lblVarOffset.Size = new System.Drawing.Size(41, 13);
            this.lblVarOffset.Text = "Offset:";

            // numVarOffset
            this.numVarOffset.Location = new System.Drawing.Point(100, 65);
            this.numVarOffset.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            this.numVarOffset.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            this.numVarOffset.Name = "numVarOffset";
            this.numVarOffset.Size = new System.Drawing.Size(120, 20);

            // lblVarComment
            this.lblVarComment.AutoSize = true;
            this.lblVarComment.Location = new System.Drawing.Point(12, 93);
            this.lblVarComment.Name = "lblVarComment";
            this.lblVarComment.Size = new System.Drawing.Size(54, 13);
            this.lblVarComment.Text = "Comment:";

            // txtVarComment
            this.txtVarComment.Location = new System.Drawing.Point(100, 91);
            this.txtVarComment.Multiline = true;
            this.txtVarComment.Name = "txtVarComment";
            this.txtVarComment.Size = new System.Drawing.Size(200, 40);

            // btnOK
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(144, 147);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);

            // btnCancel
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(225, 147);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;

            // AddVariableDialog
            this.AcceptButton = this.btnOK;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(320, 182);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtVarComment);
            this.Controls.Add(this.lblVarComment);
            this.Controls.Add(this.numVarOffset);
            this.Controls.Add(this.lblVarOffset);
            this.Controls.Add(this.cmbVarType);
            this.Controls.Add(this.lblVarType);
            this.Controls.Add(this.txtVarName);
            this.Controls.Add(this.lblVarName);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddVariableDialog";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Add Variable";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtVarName.Text))
            {
                MessageBox.Show("Please enter a variable name.", "Missing Name", 
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtVarName.Focus();
                return;
            }

            VarName = txtVarName.Text.Trim();
            VarType = cmbVarType.SelectedItem?.ToString() ?? "BOOL";
            VarOffset = (int)numVarOffset.Value;
            VarComment = txtVarComment.Text;
        }
    }
}