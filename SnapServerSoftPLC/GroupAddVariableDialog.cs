using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SnapServerSoftPLC
{
    public class GroupAddVariableDialog : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<VariableSpec> Variables { get; private set; } = new List<VariableSpec>();

        private readonly PLCDataBlock dataBlock;

        private TextBox txtBaseName;
        private ComboBox cmbVarType;
        private NumericUpDown numCount;
        private NumericUpDown numStartOffset;
        private ComboBox cmbNamingPattern;
        private TextBox txtCustomPattern;
        private TextBox txtComment;
        private CheckBox chkAutoOffset;
        private CheckBox chkSequentialBits;
        private Button btnPreview;
        private Button btnOK;
        private Button btnCancel;
        private ListBox lstPreview;
        private Label lblMemoryUsage;
        private Label lblValidation;
        
        private Label lblBaseName;
        private Label lblType;
        private Label lblCount;
        private Label lblStartOffset;
        private Label lblNamingPattern;
        private Label lblCustomPattern;
        private Label lblComment;
        private Label lblPreview;
        private Label lblTotalSize;

        private static readonly string[] dataTypes = new[] { "BOOL", "BYTE", "WORD", "DWORD", "INT", "DINT", "REAL", "STRING" };
        private static readonly string[] namingPatterns = new[] { "Numbered (Var1, Var2...)", "Indexed (Var[0], Var[1]...)", "Custom Pattern" };

        public GroupAddVariableDialog(PLCDataBlock dataBlock)
        {
            this.dataBlock = dataBlock;
            InitializeComponent();
            SetupDynamicContent();
        }

        private void SetupDynamicContent()
        {
            if (this.DesignMode) return;

            UpdateMemoryInfo();
            UpdatePreview();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Initialize controls
            txtBaseName = new TextBox();
            cmbVarType = new ComboBox();
            numCount = new NumericUpDown();
            numStartOffset = new NumericUpDown();
            cmbNamingPattern = new ComboBox();
            txtCustomPattern = new TextBox();
            txtComment = new TextBox();
            chkAutoOffset = new CheckBox();
            chkSequentialBits = new CheckBox();
            btnPreview = new Button();
            btnOK = new Button();
            btnCancel = new Button();
            lstPreview = new ListBox();
            lblMemoryUsage = new Label();
            lblValidation = new Label();
            
            lblBaseName = new Label();
            lblType = new Label();
            lblCount = new Label();
            lblStartOffset = new Label();
            lblNamingPattern = new Label();
            lblCustomPattern = new Label();
            lblComment = new Label();
            lblPreview = new Label();
            lblTotalSize = new Label();

            // Form properties
            this.Text = "Add Group of Variables";
            this.Size = new Size(650, 600);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            // Base name
            lblBaseName.Text = "Base Name:";
            lblBaseName.Location = new Point(12, 15);
            lblBaseName.AutoSize = true;
            txtBaseName.Location = new Point(120, 13);
            txtBaseName.Size = new Size(200, 20);
            txtBaseName.Text = "Variable";
            txtBaseName.TextChanged += TxtBaseName_TextChanged;

            // Data type
            lblType.Text = "Data Type:";
            lblType.Location = new Point(12, 45);
            lblType.AutoSize = true;
            cmbVarType.Location = new Point(120, 43);
            cmbVarType.Size = new Size(120, 21);
            cmbVarType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbVarType.Items.AddRange(dataTypes);
            cmbVarType.SelectedIndex = 0; // BOOL
            cmbVarType.SelectedIndexChanged += CmbVarType_SelectedIndexChanged;

            // Count
            lblCount.Text = "Count:";
            lblCount.Location = new Point(260, 45);
            lblCount.AutoSize = true;
            numCount.Location = new Point(310, 43);
            numCount.Size = new Size(80, 20);
            numCount.Minimum = 1;
            numCount.Maximum = 1000;
            numCount.Value = 5;
            numCount.ValueChanged += NumCount_ValueChanged;

            // Start offset
            lblStartOffset.Text = "Start Offset:";
            lblStartOffset.Location = new Point(12, 75);
            lblStartOffset.AutoSize = true;
            numStartOffset.Location = new Point(120, 73);
            numStartOffset.Size = new Size(80, 20);
            numStartOffset.Maximum = 65535;
            numStartOffset.ValueChanged += NumStartOffset_ValueChanged;

            // Auto offset checkbox
            chkAutoOffset.Text = "Auto-calculate offset";
            chkAutoOffset.Location = new Point(220, 75);
            chkAutoOffset.AutoSize = true;
            chkAutoOffset.Checked = true;
            chkAutoOffset.CheckedChanged += ChkAutoOffset_CheckedChanged;

            // Sequential bits (for BOOL only)
            chkSequentialBits.Text = "Sequential bits in bytes (0.0, 0.1, 0.2...)";
            chkSequentialBits.Location = new Point(220, 100);
            chkSequentialBits.Size = new Size(300, 20);
            chkSequentialBits.Checked = true;
            chkSequentialBits.CheckedChanged += ChkSequentialBits_CheckedChanged;

            // Naming pattern
            lblNamingPattern.Text = "Naming Pattern:";
            lblNamingPattern.Location = new Point(12, 130);
            lblNamingPattern.AutoSize = true;
            cmbNamingPattern.Location = new Point(120, 128);
            cmbNamingPattern.Size = new Size(200, 21);
            cmbNamingPattern.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbNamingPattern.Items.AddRange(namingPatterns);
            cmbNamingPattern.SelectedIndex = 0;
            cmbNamingPattern.SelectedIndexChanged += CmbNamingPattern_SelectedIndexChanged;

            // Custom pattern
            lblCustomPattern.Text = "Custom Pattern:";
            lblCustomPattern.Location = new Point(12, 160);
            lblCustomPattern.AutoSize = true;
            txtCustomPattern.Location = new Point(120, 158);
            txtCustomPattern.Size = new Size(300, 20);
            txtCustomPattern.Text = "{BaseName}_{Index}";
            txtCustomPattern.Visible = false;
            txtCustomPattern.TextChanged += TxtCustomPattern_TextChanged;
            lblCustomPattern.Visible = false;

            // Comment
            lblComment.Text = "Comment:";
            lblComment.Location = new Point(12, 190);
            lblComment.AutoSize = true;
            txtComment.Location = new Point(120, 188);
            txtComment.Size = new Size(400, 20);
            txtComment.TextChanged += TxtComment_TextChanged;

            // Total size info
            lblTotalSize.Location = new Point(12, 220);
            lblTotalSize.Size = new Size(500, 20);
            lblTotalSize.Font = new Font(lblTotalSize.Font, FontStyle.Bold);
            lblTotalSize.Text = "Total size: 5 bits";

            // Memory usage info
            lblMemoryUsage.Location = new Point(12, 245);
            lblMemoryUsage.Size = new Size(500, 20);
            lblMemoryUsage.Font = new Font(lblMemoryUsage.Font, FontStyle.Bold);

            // Preview
            lblPreview.Text = "Preview:";
            lblPreview.Location = new Point(12, 275);
            lblPreview.AutoSize = true;

            btnPreview.Text = "Update Preview";
            btnPreview.Location = new Point(520, 273);
            btnPreview.Size = new Size(100, 23);
            btnPreview.Click += BtnPreview_Click;

            lstPreview.Location = new Point(12, 300);
            lstPreview.Size = new Size(600, 180);
            lstPreview.Font = new Font("Consolas", 9);

            // Validation feedback
            lblValidation.Location = new Point(12, 490);
            lblValidation.Size = new Size(600, 40);
            lblValidation.ForeColor = Color.Red;

            // Buttons
            btnOK.Text = "Add Variables";
            btnOK.Location = new Point(457, 540);
            btnOK.Size = new Size(100, 23);
            btnOK.DialogResult = DialogResult.OK;
            btnOK.Click += BtnOK_Click;

            btnCancel.Text = "Cancel";
            btnCancel.Location = new Point(565, 540);
            btnCancel.Size = new Size(75, 23);
            btnCancel.DialogResult = DialogResult.Cancel;

            // Add controls
            this.Controls.AddRange(new Control[] {
                lblBaseName, txtBaseName,
                lblType, cmbVarType, lblCount, numCount,
                lblStartOffset, numStartOffset, chkAutoOffset, chkSequentialBits,
                lblNamingPattern, cmbNamingPattern, lblCustomPattern, txtCustomPattern,
                lblComment, txtComment,
                lblTotalSize, lblMemoryUsage,
                lblPreview, btnPreview, lstPreview,
                lblValidation,
                btnOK, btnCancel
            });

            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
            this.ResumeLayout(false);

            // Initial setup
            UpdateBoolOptionsVisibility();
            CalculateAutoOffset();
            UpdatePreview();
        }

        private void TxtBaseName_TextChanged(object? sender, EventArgs e) => UpdatePreview();
        private void CmbVarType_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdateBoolOptionsVisibility();
            UpdateTotalSize();
            CalculateAutoOffset();
            UpdatePreview();
        }
        private void NumCount_ValueChanged(object? sender, EventArgs e)
        {
            UpdateTotalSize();
            CalculateAutoOffset();
            UpdatePreview();
        }
        private void NumStartOffset_ValueChanged(object? sender, EventArgs e) => UpdatePreview();
        private void ChkAutoOffset_CheckedChanged(object? sender, EventArgs e)
        {
            numStartOffset.Enabled = !chkAutoOffset.Checked;
            if (chkAutoOffset.Checked) CalculateAutoOffset();
            UpdatePreview();
        }
        private void ChkSequentialBits_CheckedChanged(object? sender, EventArgs e) => UpdatePreview();
        private void CmbNamingPattern_SelectedIndexChanged(object? sender, EventArgs e)
        {
            bool isCustom = cmbNamingPattern.SelectedIndex == 2;
            lblCustomPattern.Visible = isCustom;
            txtCustomPattern.Visible = isCustom;
            UpdatePreview();
        }
        private void TxtCustomPattern_TextChanged(object? sender, EventArgs e) => UpdatePreview();
        private void TxtComment_TextChanged(object? sender, EventArgs e) => UpdatePreview();
        private void BtnPreview_Click(object? sender, EventArgs e) => UpdatePreview();

        private void UpdateBoolOptionsVisibility()
        {
            bool isBool = cmbVarType.SelectedItem?.ToString() == "BOOL";
            chkSequentialBits.Visible = isBool;
        }

        private void UpdateTotalSize()
        {
            string dataType = cmbVarType.SelectedItem?.ToString() ?? "BOOL";
            int count = (int)numCount.Value;
            
            if (dataType == "BOOL")
            {
                lblTotalSize.Text = $"Total size: {count} bits";
            }
            else
            {
                var tempVar = new PLCVariable { DataType = dataType };
                int sizePerVar = tempVar.GetSize();
                int totalBytes = sizePerVar * count;
                lblTotalSize.Text = $"Total size: {totalBytes} bytes ({count} × {sizePerVar} bytes each)";
            }
        }

        private void CalculateAutoOffset()
        {
            if (!chkAutoOffset.Checked) return;

            string dataType = cmbVarType.SelectedItem?.ToString() ?? "BOOL";
            int count = (int)numCount.Value;

            if (dataType == "BOOL")
            {
                // For BOOL, find next available bit address
                var (byteOffset, bitOffset) = BitAddressingHelper.GetNextAvailableBitAddress(dataBlock.Variables, dataBlock.Size);
                if (byteOffset >= 0)
                {
                    numStartOffset.Value = byteOffset;
                }
            }
            else
            {
                // For non-BOOL, find next available byte-aligned space
                var tempVar = new PLCVariable { DataType = dataType };
                int sizePerVar = tempVar.GetSize();
                int totalSize = sizePerVar * count;
                
                int offset = BitAddressingHelper.GetNextAvailableOffset(dataBlock.Variables, dataBlock.Size, totalSize);
                if (offset >= 0)
                {
                    numStartOffset.Value = offset;
                }
            }
        }

        private void UpdateMemoryInfo()
        {
            lblMemoryUsage.Text = dataBlock.GetMemoryUsageInfo();
        }

        private void UpdatePreview()
        {
            lstPreview.Items.Clear();
            Variables.Clear();
            lblValidation.Text = "";
            lblValidation.ForeColor = Color.Red;

            try
            {
                string baseName = txtBaseName.Text.Trim();
                string dataType = cmbVarType.SelectedItem?.ToString() ?? "BOOL";
                int count = (int)numCount.Value;
                int startOffset = (int)numStartOffset.Value;
                string comment = txtComment.Text.Trim();

                if (string.IsNullOrEmpty(baseName))
                {
                    lblValidation.Text = "✗ Base name is required";
                    btnOK.Enabled = false;
                    return;
                }

                // Generate variable specifications
                var variables = GenerateVariableSpecs(baseName, dataType, count, startOffset, comment);
                
                // Validate all variables
                bool allValid = true;
                string validationError = "";

                foreach (var varSpec in variables)
                {
                    var validation = ValidateVariable(varSpec);
                    if (!validation.IsValid)
                    {
                        allValid = false;
                        validationError = validation.ErrorMessage;
                        break;
                    }
                }

                if (!allValid)
                {
                    lblValidation.Text = $"✗ {validationError}";
                    btnOK.Enabled = false;
                    Variables.Clear();
                    return;
                }

                // Show preview
                lstPreview.Items.Add($"{"Name",-20} {"Type",-8} {"Address",-10} {"Comment"}");
                lstPreview.Items.Add(new string('-', 70));

                foreach (var varSpec in variables)
                {
                    string address = varSpec.DataType == "BOOL" ? $"{varSpec.Offset}.{varSpec.BitOffset}" : varSpec.Offset.ToString();
                    lstPreview.Items.Add($"{varSpec.Name,-20} {varSpec.DataType,-8} {address,-10} {varSpec.Comment}");
                }

                Variables.AddRange(variables);
                lblValidation.Text = $"✓ All {count} variables are valid";
                lblValidation.ForeColor = Color.Green;
                btnOK.Enabled = true;
            }
            catch (Exception ex)
            {
                lblValidation.Text = $"✗ Error: {ex.Message}";
                btnOK.Enabled = false;
            }
        }

        private List<VariableSpec> GenerateVariableSpecs(string baseName, string dataType, int count, int startOffset, string comment)
        {
            var variables = new List<VariableSpec>();
            int currentOffset = startOffset;
            int currentBitOffset = 0;

            // For BOOL variables, if sequential bits is enabled, start from the first available bit in the start byte
            if (dataType == "BOOL" && chkSequentialBits.Checked)
            {
                var availableBits = BitAddressingHelper.GetAvailableBitsInByte(dataBlock.Variables, currentOffset);
                if (availableBits.Any())
                {
                    currentBitOffset = availableBits.First();
                }
            }

            for (int i = 0; i < count; i++)
            {
                string varName = GenerateVariableName(baseName, i);
                string varComment = string.IsNullOrEmpty(comment) ? $"Auto-generated {dataType} variable {i + 1}" : $"{comment} {i + 1}";

                var varSpec = new VariableSpec
                {
                    Name = varName,
                    DataType = dataType,
                    Offset = currentOffset,
                    BitOffset = dataType == "BOOL" ? currentBitOffset : 0,
                    Comment = varComment
                };

                variables.Add(varSpec);

                // Calculate next address
                if (dataType == "BOOL")
                {
                    if (chkSequentialBits.Checked)
                    {
                        // Sequential bits: 0.0, 0.1, 0.2, ..., 0.7, 1.0, 1.1, ...
                        currentBitOffset++;
                        if (currentBitOffset > 7)
                        {
                            currentBitOffset = 0;
                            currentOffset++;
                        }
                    }
                    else
                    {
                        // Each BOOL gets its own byte: 0.0, 1.0, 2.0, ...
                        currentOffset++;
                    }
                }
                else
                {
                    // Non-BOOL variables: increment by variable size
                    var tempVar = new PLCVariable { DataType = dataType };
                    currentOffset += tempVar.GetSize();
                }
            }

            return variables;
        }

        private string GenerateVariableName(string baseName, int index)
        {
            return cmbNamingPattern.SelectedIndex switch
            {
                0 => $"{baseName}{index + 1}", // Var1, Var2, ...
                1 => $"{baseName}[{index}]", // Var[0], Var[1], ...
                2 => txtCustomPattern.Text.Replace("{BaseName}", baseName).Replace("{Index}", index.ToString()).Replace("{Index1}", (index + 1).ToString()),
                _ => $"{baseName}{index + 1}"
            };
        }

        private (bool IsValid, string ErrorMessage) ValidateVariable(VariableSpec varSpec)
        {
            // Use PLCManager's validation logic
            var tempDataBlock = new PLCDataBlock(dataBlock.Number, dataBlock.Size);
            
            // Copy existing variables
            foreach (var existingVar in dataBlock.Variables)
            {
                tempDataBlock.Variables.Add(new PLCVariable
                {
                    Name = existingVar.Name,
                    DataType = existingVar.DataType,
                    Offset = existingVar.Offset,
                    BitOffset = existingVar.BitOffset,
                    Value = existingVar.Value,
                    Comment = existingVar.Comment
                });
            }

            // Add variables we're planning to create (excluding the current one)
            foreach (var otherVar in Variables.Where(v => v.Name != varSpec.Name))
            {
                tempDataBlock.Variables.Add(new PLCVariable
                {
                    Name = otherVar.Name,
                    DataType = otherVar.DataType,
                    Offset = otherVar.Offset,
                    BitOffset = otherVar.BitOffset,
                    Value = GetDefaultValueForDataType(otherVar.DataType),
                    Comment = otherVar.Comment
                });
            }

            // Use PLCManager validation logic
            var plcManager = new PLCManager();
            return plcManager.ValidateVariableAddition(tempDataBlock, varSpec.Name, varSpec.DataType, varSpec.Offset, varSpec.BitOffset);
        }

        private object GetDefaultValueForDataType(string dataType)
        {
            return dataType switch
            {
                "BOOL" => false,
                "BYTE" => (byte)0,
                "WORD" => (ushort)0,
                "DWORD" => (uint)0,
                "INT" => (short)0,
                "DINT" => 0,
                "REAL" => 0.0f,
                "STRING" => "",
                _ => false
            };
        }

        private void BtnOK_Click(object? sender, EventArgs e)
        {
            if (Variables.Count == 0)
            {
                UpdatePreview();
                return;
            }

            // Final validation
            foreach (var varSpec in Variables)
            {
                var validation = ValidateVariable(varSpec);
                if (!validation.IsValid)
                {
                    MessageBox.Show($"Validation failed for variable '{varSpec.Name}': {validation.ErrorMessage}", 
                                  "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }
    }

    public class VariableSpec
    {
        public string Name { get; set; } = "";
        public string DataType { get; set; } = "";
        public int Offset { get; set; }
        public int BitOffset { get; set; }
        public string Comment { get; set; } = "";
    }
}