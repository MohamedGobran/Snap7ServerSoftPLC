using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

namespace SnapServerSoftPLC
{
    public partial class UpdateValueDialog : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object NewValue { get; private set; } = false;

        private string variableName;
        private string dataType;
        private string currentValue;

        private Label lblVariableName;
        private Label lblDataType;
        private Label lblCurrentValue;
        private Label lblNewValue;
        private TextBox txtNewValue;
        private CheckBox chkBoolValue;
        private Button btnOK;
        private Button btnCancel;

        public UpdateValueDialog(string varName, string varType, string currentVal)
        {
            variableName = varName;
            dataType = varType;
            currentValue = currentVal;
            InitializeComponent();
            SetupDynamicContent();
        }
        
        private void SetupDynamicContent()
        {
            if (this.DesignMode) return;
            
            // Set dynamic content only at runtime
            lblVariableName.Text = $"Variable: {variableName}";
            lblDataType.Text = $"Type: {dataType}";
            lblCurrentValue.Text = $"Current Value: {currentValue}";
            txtNewValue.Text = currentValue;
            
            SetupValueInput();
        }

        private void InitializeComponent()
        {
            this.lblVariableName = new Label();
            this.lblDataType = new Label();
            this.lblCurrentValue = new Label();
            this.lblNewValue = new Label();
            this.txtNewValue = new TextBox();
            this.chkBoolValue = new CheckBox();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.SuspendLayout();

            // lblVariableName
            this.lblVariableName.AutoSize = true;
            this.lblVariableName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblVariableName.Location = new System.Drawing.Point(12, 15);
            this.lblVariableName.Name = "lblVariableName";
            this.lblVariableName.Size = new System.Drawing.Size(53, 13);
            this.lblVariableName.Text = "Variable:";

            // lblDataType
            this.lblDataType.AutoSize = true;
            this.lblDataType.Location = new System.Drawing.Point(12, 35);
            this.lblDataType.Name = "lblDataType";
            this.lblDataType.Size = new System.Drawing.Size(61, 13);
            this.lblDataType.Text = "Type:";

            // lblCurrentValue
            this.lblCurrentValue.AutoSize = true;
            this.lblCurrentValue.Location = new System.Drawing.Point(12, 55);
            this.lblCurrentValue.Name = "lblCurrentValue";
            this.lblCurrentValue.Size = new System.Drawing.Size(80, 13);
            this.lblCurrentValue.Text = "Current Value:";

            // lblNewValue
            this.lblNewValue.AutoSize = true;
            this.lblNewValue.Location = new System.Drawing.Point(12, 85);
            this.lblNewValue.Name = "lblNewValue";
            this.lblNewValue.Size = new System.Drawing.Size(65, 13);
            this.lblNewValue.Text = "New Value:";

            // txtNewValue
            this.txtNewValue.Location = new System.Drawing.Point(100, 83);
            this.txtNewValue.Name = "txtNewValue";
            this.txtNewValue.Size = new System.Drawing.Size(150, 20);
            this.txtNewValue.Text = "";

            // chkBoolValue
            this.chkBoolValue.AutoSize = true;
            this.chkBoolValue.Location = new System.Drawing.Point(100, 85);
            this.chkBoolValue.Name = "chkBoolValue";
            this.chkBoolValue.Size = new System.Drawing.Size(15, 14);
            this.chkBoolValue.UseVisualStyleBackColor = true;
            this.chkBoolValue.Visible = false;

            // btnOK
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(95, 120);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);

            // btnCancel
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(176, 120);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;

            // UpdateValueDialog
            this.AcceptButton = this.btnOK;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(280, 160);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.chkBoolValue);
            this.Controls.Add(this.txtNewValue);
            this.Controls.Add(this.lblNewValue);
            this.Controls.Add(this.lblCurrentValue);
            this.Controls.Add(this.lblDataType);
            this.Controls.Add(this.lblVariableName);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdateValueDialog";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Update Variable Value";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void SetupValueInput()
        {
            if (this.DesignMode) return;
            
            if (dataType == "BOOL")
            {
                txtNewValue.Visible = false;
                chkBoolValue.Visible = true;
                chkBoolValue.Checked = bool.TryParse(currentValue, out bool boolVal) && boolVal;
            }
            else
            {
                txtNewValue.Visible = true;
                chkBoolValue.Visible = false;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                switch (dataType)
                {
                    case "BOOL":
                        NewValue = chkBoolValue.Checked;
                        break;
                    case "BYTE":
                        if (byte.TryParse(txtNewValue.Text, out byte byteVal))
                            NewValue = byteVal;
                        else
                            throw new FormatException("Invalid byte value");
                        break;
                    case "WORD":
                        if (ushort.TryParse(txtNewValue.Text, out ushort wordVal))
                            NewValue = wordVal;
                        else
                            throw new FormatException("Invalid word value");
                        break;
                    case "DWORD":
                        if (uint.TryParse(txtNewValue.Text, out uint dwordVal))
                            NewValue = dwordVal;
                        else
                            throw new FormatException("Invalid dword value");
                        break;
                    case "INT":
                        if (short.TryParse(txtNewValue.Text, out short intVal))
                            NewValue = intVal;
                        else
                            throw new FormatException("Invalid int value");
                        break;
                    case "DINT":
                        if (int.TryParse(txtNewValue.Text, out int dintVal))
                            NewValue = dintVal;
                        else
                            throw new FormatException("Invalid dint value");
                        break;
                    case "REAL":
                        if (float.TryParse(txtNewValue.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out float realVal))
                            NewValue = realVal;
                        else
                            throw new FormatException("Invalid real value");
                        break;
                    case "STRING":
                        NewValue = txtNewValue.Text;
                        break;
                    default:
                        NewValue = txtNewValue.Text;
                        break;
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Invalid value for {dataType}: {ex.Message}", "Invalid Value", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;
                return;
            }
        }
    }
}