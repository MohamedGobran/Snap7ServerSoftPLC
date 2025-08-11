using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace SnapServerSoftPLC
{
    public partial class AddDataBlockDialog : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int DBNumber { get; private set; }
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int DBSize { get; private set; }
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DBName { get; private set; } = "";
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DBComment { get; private set; } = "";

        private NumericUpDown numDBNumber;
        private NumericUpDown numDBSize;
        private TextBox txtDBName;
        private TextBox txtDBComment;
        private Button btnOK;
        private Button btnCancel;
        private Label lblDBNumber;
        private Label lblDBSize;
        private Label lblDBName;
        private Label lblDBComment;

        public AddDataBlockDialog()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.numDBNumber = new NumericUpDown();
            this.numDBSize = new NumericUpDown();
            this.txtDBName = new TextBox();
            this.txtDBComment = new TextBox();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.lblDBNumber = new Label();
            this.lblDBSize = new Label();
            this.lblDBName = new Label();
            this.lblDBComment = new Label();
            this.SuspendLayout();

            // lblDBNumber
            this.lblDBNumber.AutoSize = true;
            this.lblDBNumber.Location = new System.Drawing.Point(12, 15);
            this.lblDBNumber.Name = "lblDBNumber";
            this.lblDBNumber.Size = new System.Drawing.Size(67, 13);
            this.lblDBNumber.Text = "DB Number:";

            // numDBNumber
            this.numDBNumber.Location = new System.Drawing.Point(100, 13);
            this.numDBNumber.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            this.numDBNumber.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numDBNumber.Name = "numDBNumber";
            this.numDBNumber.Size = new System.Drawing.Size(120, 20);
            this.numDBNumber.Value = new decimal(new int[] { 1, 0, 0, 0 });

            // lblDBSize
            this.lblDBSize.AutoSize = true;
            this.lblDBSize.Location = new System.Drawing.Point(12, 41);
            this.lblDBSize.Name = "lblDBSize";
            this.lblDBSize.Size = new System.Drawing.Size(68, 13);
            this.lblDBSize.Text = "Size (bytes):";

            // numDBSize
            this.numDBSize.Location = new System.Drawing.Point(100, 39);
            this.numDBSize.Maximum = new decimal(new int[] { 65536, 0, 0, 0 });
            this.numDBSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numDBSize.Name = "numDBSize";
            this.numDBSize.Size = new System.Drawing.Size(120, 20);
            this.numDBSize.Value = new decimal(new int[] { 1024, 0, 0, 0 });

            // lblDBName
            this.lblDBName.AutoSize = true;
            this.lblDBName.Location = new System.Drawing.Point(12, 67);
            this.lblDBName.Name = "lblDBName";
            this.lblDBName.Size = new System.Drawing.Size(38, 13);
            this.lblDBName.Text = "Name:";

            // txtDBName
            this.txtDBName.Location = new System.Drawing.Point(100, 65);
            this.txtDBName.Name = "txtDBName";
            this.txtDBName.Size = new System.Drawing.Size(200, 20);

            // lblDBComment
            this.lblDBComment.AutoSize = true;
            this.lblDBComment.Location = new System.Drawing.Point(12, 93);
            this.lblDBComment.Name = "lblDBComment";
            this.lblDBComment.Size = new System.Drawing.Size(54, 13);
            this.lblDBComment.Text = "Comment:";

            // txtDBComment
            this.txtDBComment.Location = new System.Drawing.Point(100, 91);
            this.txtDBComment.Multiline = true;
            this.txtDBComment.Name = "txtDBComment";
            this.txtDBComment.Size = new System.Drawing.Size(200, 40);

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

            // AddDataBlockDialog
            this.AcceptButton = this.btnOK;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(320, 182);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtDBComment);
            this.Controls.Add(this.lblDBComment);
            this.Controls.Add(this.txtDBName);
            this.Controls.Add(this.lblDBName);
            this.Controls.Add(this.numDBSize);
            this.Controls.Add(this.lblDBSize);
            this.Controls.Add(this.numDBNumber);
            this.Controls.Add(this.lblDBNumber);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddDataBlockDialog";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Add Data Block";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DBNumber = (int)numDBNumber.Value;
            DBSize = (int)numDBSize.Value;
            DBName = string.IsNullOrEmpty(txtDBName.Text) ? $"DB{DBNumber}" : txtDBName.Text;
            DBComment = txtDBComment.Text;
        }
    }
}