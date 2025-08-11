using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace SnapServerSoftPLC
{
    public partial class EditDataBlockDialog : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DBName { get; private set; } = "";
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int DBSize { get; private set; }
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DBComment { get; private set; } = "";

        private readonly int dbNumber;
        private readonly int currentSize;

        private Label lblDBNumber;
        private Label lblDBName;
        private Label lblDBSize;
        private Label lblDBComment;
        private TextBox txtDBName;
        private NumericUpDown numDBSize;
        private TextBox txtDBComment;
        private Button btnOK;
        private Button btnCancel;
        private Label lblSizeWarning;

        public EditDataBlockDialog(int dbNumber, string currentName, int currentSize, string currentComment)
        {
            this.dbNumber = dbNumber;
            this.currentSize = currentSize;
            
            InitializeComponent();
            
            SetupDynamicContent(dbNumber, currentName, currentSize, currentComment);
        }
        
        private void SetupDynamicContent(int dbNumber, string currentName, int currentSize, string currentComment)
        {
            if (this.DesignMode) return;
            
            // Set dynamic content only at runtime
            lblDBNumber.Text = $"Data Block: DB{dbNumber}";
            txtDBName.Text = currentName;
            numDBSize.Value = currentSize;
            txtDBComment.Text = currentComment;
            this.Text = $"Edit Data Block - DB{dbNumber}";
        }

        private void InitializeComponent()
        {
            this.lblDBNumber = new Label();
            this.lblDBName = new Label();
            this.lblDBSize = new Label();
            this.lblDBComment = new Label();
            this.txtDBName = new TextBox();
            this.numDBSize = new NumericUpDown();
            this.txtDBComment = new TextBox();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.lblSizeWarning = new Label();
            this.SuspendLayout();

            // lblDBNumber
            this.lblDBNumber.AutoSize = true;
            this.lblDBNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblDBNumber.Location = new System.Drawing.Point(12, 15);
            this.lblDBNumber.Name = "lblDBNumber";
            this.lblDBNumber.Size = new System.Drawing.Size(67, 13);
            this.lblDBNumber.Text = "Data Block: DB";

            // lblDBName
            this.lblDBName.AutoSize = true;
            this.lblDBName.Location = new System.Drawing.Point(12, 45);
            this.lblDBName.Name = "lblDBName";
            this.lblDBName.Size = new System.Drawing.Size(38, 13);
            this.lblDBName.Text = "Name:";

            // txtDBName
            this.txtDBName.Location = new System.Drawing.Point(100, 43);
            this.txtDBName.Name = "txtDBName";
            this.txtDBName.Size = new System.Drawing.Size(200, 20);

            // lblDBSize
            this.lblDBSize.AutoSize = true;
            this.lblDBSize.Location = new System.Drawing.Point(12, 71);
            this.lblDBSize.Name = "lblDBSize";
            this.lblDBSize.Size = new System.Drawing.Size(68, 13);
            this.lblDBSize.Text = "Size (bytes):";

            // numDBSize
            this.numDBSize.Location = new System.Drawing.Point(100, 69);
            this.numDBSize.Maximum = new decimal(new int[] { 65536, 0, 0, 0 });
            this.numDBSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numDBSize.Name = "numDBSize";
            this.numDBSize.Size = new System.Drawing.Size(120, 20);
            this.numDBSize.Value = new decimal(new int[] { 1024, 0, 0, 0 });
            this.numDBSize.ValueChanged += new System.EventHandler(this.numDBSize_ValueChanged);

            // lblSizeWarning
            this.lblSizeWarning.ForeColor = System.Drawing.Color.Red;
            this.lblSizeWarning.Location = new System.Drawing.Point(100, 92);
            this.lblSizeWarning.Name = "lblSizeWarning";
            this.lblSizeWarning.Size = new System.Drawing.Size(200, 13);
            this.lblSizeWarning.Text = "";
            this.lblSizeWarning.Visible = false;

            // lblDBComment
            this.lblDBComment.AutoSize = true;
            this.lblDBComment.Location = new System.Drawing.Point(12, 113);
            this.lblDBComment.Name = "lblDBComment";
            this.lblDBComment.Size = new System.Drawing.Size(54, 13);
            this.lblDBComment.Text = "Comment:";

            // txtDBComment
            this.txtDBComment.Location = new System.Drawing.Point(100, 111);
            this.txtDBComment.Multiline = true;
            this.txtDBComment.Name = "txtDBComment";
            this.txtDBComment.Size = new System.Drawing.Size(200, 40);

            // btnOK
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(144, 167);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);

            // btnCancel
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(225, 167);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;

            // EditDataBlockDialog
            this.AcceptButton = this.btnOK;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(320, 202);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtDBComment);
            this.Controls.Add(this.lblDBComment);
            this.Controls.Add(this.lblSizeWarning);
            this.Controls.Add(this.numDBSize);
            this.Controls.Add(this.lblDBSize);
            this.Controls.Add(this.txtDBName);
            this.Controls.Add(this.lblDBName);
            this.Controls.Add(this.lblDBNumber);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditDataBlockDialog";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Edit Data Block";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void numDBSize_ValueChanged(object sender, EventArgs e)
        {
            if ((int)numDBSize.Value < currentSize)
            {
                lblSizeWarning.Text = "⚠ Reducing size may affect variables";
                lblSizeWarning.Visible = true;
            }
            else
            {
                lblSizeWarning.Visible = false;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDBName.Text))
            {
                MessageBox.Show("Please enter a data block name.", "Missing Name", 
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDBName.Focus();
                return;
            }

            DBName = txtDBName.Text.Trim();
            DBSize = (int)numDBSize.Value;
            DBComment = txtDBComment.Text;
        }
    }
}