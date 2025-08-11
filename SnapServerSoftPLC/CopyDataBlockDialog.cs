using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace SnapServerSoftPLC
{
    public partial class CopyDataBlockDialog : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TargetDBNumber { get; private set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string NewDBName { get; private set; } = "";

        private readonly int sourceDbNumber;
        private readonly string sourceDbName;

        private Label lblSource;
        private Label lblTargetNumber;
        private Label lblNewName;
        private NumericUpDown numTargetDB;
        private TextBox txtNewName;
        private Button btnOK;
        private Button btnCancel;

        public CopyDataBlockDialog(int sourceDbNumber, string sourceDbName)
        {
            this.sourceDbNumber = sourceDbNumber;
            this.sourceDbName = sourceDbName;

            InitializeComponent();
            
            SetupDynamicContent();
        }
        
        private void SetupDynamicContent()
        {
            if (this.DesignMode) return;
            
            // Set dynamic content only at runtime
            lblSource.Text = $"Copy DB {sourceDbNumber} to:";
            txtNewName.Text = $"{sourceDbName}_Copy";
            numTargetDB.Value = sourceDbNumber + 1;
            this.Text = $"Copy Data Block - DB{sourceDbNumber}";
        }

        private void InitializeComponent()
        {
            this.lblSource = new Label();
            this.lblTargetNumber = new Label();
            this.lblNewName = new Label();
            this.numTargetDB = new NumericUpDown();
            this.txtNewName = new TextBox();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.SuspendLayout();

            // lblSource
            this.lblSource.AutoSize = true;
            this.lblSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblSource.Location = new System.Drawing.Point(12, 15);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(150, 13);
            this.lblSource.Text = "Copy DB to:";

            // lblTargetNumber
            this.lblTargetNumber.AutoSize = true;
            this.lblTargetNumber.Location = new System.Drawing.Point(12, 45);
            this.lblTargetNumber.Name = "lblTargetNumber";
            this.lblTargetNumber.Size = new System.Drawing.Size(84, 13);
            this.lblTargetNumber.Text = "Target DB Number:";

            // numTargetDB
            this.numTargetDB.Location = new System.Drawing.Point(120, 43);
            this.numTargetDB.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            this.numTargetDB.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numTargetDB.Name = "numTargetDB";
            this.numTargetDB.Size = new System.Drawing.Size(120, 20);
            this.numTargetDB.Value = new decimal(new int[] { 1, 0, 0, 0 });

            // lblNewName
            this.lblNewName.AutoSize = true;
            this.lblNewName.Location = new System.Drawing.Point(12, 71);
            this.lblNewName.Name = "lblNewName";
            this.lblNewName.Size = new System.Drawing.Size(63, 13);
            this.lblNewName.Text = "New Name:";

            // txtNewName
            this.txtNewName.Location = new System.Drawing.Point(120, 69);
            this.txtNewName.Name = "txtNewName";
            this.txtNewName.Size = new System.Drawing.Size(180, 20);

            // btnOK
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(144, 107);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.Text = "Copy";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);

            // btnCancel
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(225, 107);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;

            // CopyDataBlockDialog
            this.AcceptButton = this.btnOK;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(320, 142);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtNewName);
            this.Controls.Add(this.lblNewName);
            this.Controls.Add(this.numTargetDB);
            this.Controls.Add(this.lblTargetNumber);
            this.Controls.Add(this.lblSource);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CopyDataBlockDialog";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Copy Data Block";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            TargetDBNumber = (int)numTargetDB.Value;
            NewDBName = string.IsNullOrWhiteSpace(txtNewName.Text) ? $"DB{TargetDBNumber}" : txtNewName.Text.Trim();

            if (TargetDBNumber == sourceDbNumber)
            {
                MessageBox.Show("Target DB number cannot be the same as source DB number.", "Invalid Target",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numTargetDB.Focus();
                DialogResult = DialogResult.None;
                return;
            }
        }
    }
}