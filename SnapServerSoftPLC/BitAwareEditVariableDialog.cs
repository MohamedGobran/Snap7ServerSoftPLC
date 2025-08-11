using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SnapServerSoftPLC
{
    public class BitAwareEditVariableDialog : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string VarName { get; private set; } = "";

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string VarType { get; private set; } = "BOOL";

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int VarOffset { get; private set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int VarBitOffset { get; private set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string VarComment { get; private set; } = "";

        private readonly PLCDataBlock dataBlock;
        private readonly string originalVariableName;

        private TextBox txtVarName;
        private ComboBox cmbVarType;
        private NumericUpDown numVarOffset;
        private ComboBox cmbBitOffset;
        private TextBox txtVarComment;
        private Button btnOK;
        private Button btnCancel;
        private Button btnAutoOffset;
        private Label lblMemoryUsage;
        private Label lblBitUsage;
        private Label lblValidation;
        private ListBox lstAvailableRegions;
        private Label lblOffsetInstruction;
        private Label lblName;
        private Label lblType;
        private Label lblOffset;
        private Label lblBitOffset;
        private Label lblSizeInfo;
        private Label lblComment;
        private Label lblAvailableRegions;
        private static readonly string[] items = new[] { "BOOL", "BYTE", "WORD", "DWORD", "INT", "DINT", "REAL", "STRING" };
        private static readonly string[] itemsArray = new[] { "0", "1", "2", "3", "4", "5", "6", "7" };

        public BitAwareEditVariableDialog(PLCDataBlock dataBlock, string variableName, string dataType, int offset, string comment, int bitOffset = 0)
        {
            this.dataBlock = dataBlock;
            this.originalVariableName = variableName;
            
            InitializeComponent();
            SetupDynamicContent();
            
            // Initialize with current values
            txtVarName.Text = variableName;
            cmbVarType.SelectedItem = dataType;
            numVarOffset.Value = offset;
            cmbBitOffset.SelectedIndex = bitOffset;
            txtVarComment.Text = comment;
        }

        private void SetupDynamicContent()
        {
            if (this.DesignMode) return;

            UpdateMemoryInfo();
            UpdateAvailableRegions();
            UpdateBitOffsetVisibility();
            UpdateSizeInfo();
            UpdateAddressDisplay();
            UpdateBitUsageInfo();
            ValidateInput();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Initialize controls
            txtVarName = new TextBox();
            cmbVarType = new ComboBox();
            numVarOffset = new NumericUpDown();
            cmbBitOffset = new ComboBox();
            txtVarComment = new TextBox();
            btnOK = new Button();
            btnCancel = new Button();
            btnAutoOffset = new Button();
            lblMemoryUsage = new Label();
            lblBitUsage = new Label();
            lblValidation = new Label();
            lstAvailableRegions = new ListBox();
            lblOffsetInstruction = new Label();
            lblName = new Label();
            lblType = new Label();
            lblOffset = new Label();
            lblBitOffset = new Label();
            lblSizeInfo = new Label();
            lblComment = new Label();
            lblAvailableRegions = new Label();

            // Form properties
            this.Text = "Edit Variable - Bit Addressing";
            this.Size = new Size(550, 520);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            // Variable name
            lblName.Text = "Variable Name:";
            lblName.Location = new Point(12, 15);
            lblName.AutoSize = true;
            txtVarName.Location = new Point(120, 13);
            txtVarName.Size = new Size(200, 20);
            txtVarName.TextChanged += TxtVarName_TextChanged;

            // Data type
            lblType.Text = "Data Type:";
            lblType.Location = new Point(12, 45);
            lblType.AutoSize = true;
            cmbVarType.Location = new Point(120, 43);
            cmbVarType.Size = new Size(120, 21);
            cmbVarType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbVarType.Items.AddRange(items);
            cmbVarType.SelectedIndex = 0;
            cmbVarType.SelectedIndexChanged += CmbVarType_SelectedIndexChanged;

            // Offset with auto-suggest
            lblOffset.Text = "Byte Offset:";
            lblOffset.Location = new Point(12, 75);
            lblOffset.AutoSize = true;
            numVarOffset.Location = new Point(120, 73);
            numVarOffset.Size = new Size(80, 20);
            numVarOffset.Maximum = 65535;
            numVarOffset.ValueChanged += NumVarOffset_ValueChanged;

            // Bit offset (for BOOL variables)
            lblBitOffset.Text = "Bit (0-7):";
            lblBitOffset.Location = new Point(210, 75);
            lblBitOffset.AutoSize = true;
            lblBitOffset.Name = "lblBitOffset";
            cmbBitOffset.Location = new Point(270, 73);
            cmbBitOffset.Size = new Size(50, 21);
            cmbBitOffset.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBitOffset.Items.AddRange(itemsArray);
            cmbBitOffset.SelectedIndex = 0;
            cmbBitOffset.SelectedIndexChanged += CmbBitOffset_SelectedIndexChanged;
            cmbBitOffset.Name = "cmbBitOffset";

            btnAutoOffset.Text = "Auto";
            btnAutoOffset.Location = new Point(330, 72);
            btnAutoOffset.Size = new Size(50, 23);
            btnAutoOffset.Click += BtnAutoOffset_Click;

            // Address instruction
            lblOffsetInstruction.Location = new Point(390, 75);
            lblOffsetInstruction.Size = new Size(150, 20);
            lblOffsetInstruction.Text = "Address: 0.0";
            lblOffsetInstruction.Font = new Font(lblOffsetInstruction.Font, FontStyle.Bold);
            lblOffsetInstruction.Name = "lblOffsetInstruction";

            // Size info label
            lblSizeInfo.Text = "Size: 1 bit";
            lblSizeInfo.Location = new Point(12, 105);
            lblSizeInfo.AutoSize = true;
            lblSizeInfo.Name = "lblSizeInfo";

            // Comment
            lblComment.Text = "Comment:";
            lblComment.Location = new Point(12, 135);
            lblComment.AutoSize = true;
            txtVarComment.Location = new Point(120, 133);
            txtVarComment.Size = new Size(400, 20);

            // Memory usage info
            lblMemoryUsage.Location = new Point(12, 165);
            lblMemoryUsage.Size = new Size(500, 20);
            lblMemoryUsage.Font = new Font(lblMemoryUsage.Font, FontStyle.Bold);

            // Bit usage info (for selected byte)
            lblBitUsage.Location = new Point(12, 190);
            lblBitUsage.Size = new Size(500, 20);
            lblBitUsage.ForeColor = Color.Blue;
            lblBitUsage.Name = "lblBitUsage";

            // Available regions
            lblAvailableRegions.Text = "Available Addresses:";
            lblAvailableRegions.Location = new Point(12, 220);
            lblAvailableRegions.AutoSize = true;

            lstAvailableRegions.Location = new Point(12, 240);
            lstAvailableRegions.Size = new Size(500, 120);
            lstAvailableRegions.Font = new Font("Consolas", 9);
            lstAvailableRegions.DoubleClick += LstAvailableRegions_DoubleClick;

            // Validation feedback
            lblValidation.Location = new Point(12, 370);
            lblValidation.Size = new Size(500, 40);
            lblValidation.ForeColor = Color.Red;

            // Buttons
            btnOK.Text = "OK";
            btnOK.Location = new Point(357, 430);
            btnOK.Size = new Size(75, 23);
            btnOK.DialogResult = DialogResult.OK;
            btnOK.Click += BtnOK_Click;

            btnCancel.Text = "Cancel";
            btnCancel.Location = new Point(440, 430);
            btnCancel.Size = new Size(75, 23);
            btnCancel.DialogResult = DialogResult.Cancel;

            // Add controls
            this.Controls.AddRange(new Control[] {
                lblName, txtVarName,
                lblType, cmbVarType,
                lblOffset, numVarOffset, lblBitOffset, cmbBitOffset, btnAutoOffset, lblOffsetInstruction,
                lblSizeInfo,
                lblComment, txtVarComment,
                lblMemoryUsage, lblBitUsage,
                lblAvailableRegions, lstAvailableRegions,
                lblValidation,
                btnOK, btnCancel
            });

            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
            this.ResumeLayout(false);

            // Update displays
            UpdateSizeInfo();
            UpdateAddressDisplay();
        }

        private void TxtVarName_TextChanged(object? sender, EventArgs e)
        {
            ValidateInput();
        }

        private void CmbVarType_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdateBitOffsetVisibility();
            UpdateSizeInfo();
            UpdateAddressDisplay();
            UpdateAvailableRegions();
            ValidateInput();
        }

        private void NumVarOffset_ValueChanged(object? sender, EventArgs e)
        {
            UpdateBitUsageInfo();
            UpdateAddressDisplay();
            ValidateInput();
        }

        private void CmbBitOffset_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdateAddressDisplay();
            ValidateInput();
        }

        private void BtnAutoOffset_Click(object? sender, EventArgs e)
        {
            if (cmbVarType.SelectedItem?.ToString() == "BOOL")
            {
                var (byteOffset, bitOffset) = BitAddressingHelper.GetNextAvailableBitAddress(dataBlock.Variables, dataBlock.Size);

                if (byteOffset >= 0)
                {
                    numVarOffset.Value = byteOffset;
                    cmbBitOffset.SelectedIndex = bitOffset;
                    lblValidation.Text = $"✓ Auto-selected address {byteOffset}.{bitOffset}";
                    lblValidation.ForeColor = Color.Green;
                }
                else
                {
                    lblValidation.Text = "✗ No available bit addresses";
                    lblValidation.ForeColor = Color.Red;
                }
            }
            else
            {
                // For non-BOOL variables, find next byte-aligned address
                int requiredSize = GetCurrentDataTypeSize();
                for (int offset = 0; offset <= dataBlock.Size - requiredSize; offset++)
                {
                    if (IsOffsetAvailableForNonBool(offset, requiredSize, originalVariableName))
                    {
                        numVarOffset.Value = offset;
                        lblValidation.Text = $"✓ Auto-selected offset {offset}";
                        lblValidation.ForeColor = Color.Green;
                        return;
                    }
                }

                lblValidation.Text = "✗ No available space for this data type";
                lblValidation.ForeColor = Color.Red;
            }
        }

        private void LstAvailableRegions_DoubleClick(object? sender, EventArgs e)
        {
            if (lstAvailableRegions.SelectedItem is string selectedText)
            {
                // Parse address from selected item
                if (selectedText.Contains("."))
                {
                    // Bit address (e.g., "5.3")
                    var parts = selectedText.Split(' ')[0].Split('.');
                    if (parts.Length == 2 &&
                        int.TryParse(parts[0], out int byteAddr) &&
                        int.TryParse(parts[1], out int bitAddr))
                    {
                        numVarOffset.Value = byteAddr;
                        cmbBitOffset.SelectedIndex = bitAddr;
                    }
                }
                else
                {
                    // Byte address
                    var parts = selectedText.Split(' ');
                    if (parts.Length > 0 && int.TryParse(parts[0], out int addr))
                    {
                        numVarOffset.Value = addr;
                    }
                }
            }
        }

        private void BtnOK_Click(object? sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            VarName = txtVarName.Text.Trim();
            VarType = cmbVarType.SelectedItem?.ToString() ?? "BOOL";
            VarOffset = (int)numVarOffset.Value;
            VarBitOffset = VarType == "BOOL" ? cmbBitOffset.SelectedIndex : 0;
            VarComment = txtVarComment.Text.Trim();
        }

        private void UpdateBitOffsetVisibility()
        {
            bool isBool = cmbVarType.SelectedItem?.ToString() == "BOOL";

            var lblBitOffset = this.Controls.Find("lblBitOffset", true).FirstOrDefault();
            var cmbBitOffsetCtrl = this.Controls.Find("cmbBitOffset", true).FirstOrDefault();

            if (lblBitOffset != null) lblBitOffset.Visible = isBool;
            if (cmbBitOffsetCtrl != null) cmbBitOffsetCtrl.Visible = isBool;
        }

        private void UpdateSizeInfo()
        {
            var lblSizeInfo = this.Controls.Find("lblSizeInfo", false).FirstOrDefault() as Label;
            if (lblSizeInfo != null)
            {
                if (cmbVarType.SelectedItem?.ToString() == "BOOL")
                {
                    lblSizeInfo.Text = "Size: 1 bit";
                }
                else
                {
                    int size = GetCurrentDataTypeSize();
                    lblSizeInfo.Text = $"Size: {size} byte{(size != 1 ? "s" : "")} ({size * 8} bits)";
                }
            }
        }

        private void UpdateAddressDisplay()
        {
            var lblAddress = this.Controls.Find("lblOffsetInstruction", false).FirstOrDefault() as Label;
            if (lblAddress != null)
            {
                if (cmbVarType.SelectedItem?.ToString() == "BOOL")
                {
                    lblAddress.Text = $"Address: {numVarOffset.Value}.{cmbBitOffset.SelectedIndex}";
                }
                else
                {
                    lblAddress.Text = $"Address: {numVarOffset.Value}";
                }
            }
        }

        private void UpdateBitUsageInfo()
        {
            var lblBitUsageCtrl = this.Controls.Find("lblBitUsage", false).FirstOrDefault() as Label;
            if (lblBitUsageCtrl != null && cmbVarType.SelectedItem?.ToString() == "BOOL")
            {
                int byteOffset = (int)numVarOffset.Value;
                lblBitUsageCtrl.Text = dataBlock.GetByteUsageInfo(byteOffset);
            }
        }

        private void UpdateMemoryInfo()
        {
            lblMemoryUsage.Text = dataBlock.GetMemoryUsageInfo();
        }

        private void UpdateAvailableRegions()
        {
            lstAvailableRegions.Items.Clear();

            if (cmbVarType.SelectedItem?.ToString() == "BOOL")
            {
                // Show available bit addresses
                lstAvailableRegions.Items.Add($"{"Address",-10} {"Description",-30}");
                lstAvailableRegions.Items.Add(new string('-', 50));

                int count = 0;
                for (int byteAddr = 0; byteAddr < dataBlock.Size && count < 20; byteAddr++)
                {
                    if (BitAddressingHelper.IsByteCompletelyOccupied(dataBlock.Variables, byteAddr))
                        continue;

                    var availableBits = BitAddressingHelper.GetAvailableBitsInByte(dataBlock.Variables, byteAddr);
                    foreach (int bitAddr in availableBits)
                    {
                        if (count++ >= 20) break;
                        lstAvailableRegions.Items.Add($"{byteAddr}.{bitAddr,-8} Available bit address");
                    }
                }

                if (count == 0)
                    lstAvailableRegions.Items.Add("No available bit addresses");
            }
            else
            {
                // Show available byte ranges
                int requiredSize = GetCurrentDataTypeSize();
                lstAvailableRegions.Items.Add($"{"Offset",-10} {"Size",-8} {"Description",-25}");
                lstAvailableRegions.Items.Add(new string('-', 55));

                int count = 0;
                for (int offset = 0; offset <= dataBlock.Size - requiredSize && count < 15; offset++)
                {
                    if (IsOffsetAvailableForNonBool(offset, requiredSize, originalVariableName))
                    {
                        lstAvailableRegions.Items.Add($"{offset,-10} {requiredSize,-8} Available for {cmbVarType.SelectedItem}");
                        count++;
                    }
                }

                if (count == 0)
                    lstAvailableRegions.Items.Add("No available space for this data type");
            }
        }

        private bool ValidateInput()
        {
            lblValidation.Text = "";
            lblValidation.ForeColor = Color.Red;

            // Name validation
            if (string.IsNullOrWhiteSpace(txtVarName.Text))
            {
                lblValidation.Text = "✗ Variable name is required";
                btnOK.Enabled = false;
                return false;
            }

            // Name uniqueness (exclude current variable being edited)
            if (dataBlock.Variables.Any(v => v.Name.Equals(txtVarName.Text.Trim(), StringComparison.OrdinalIgnoreCase) && v.Name != originalVariableName))
            {
                lblValidation.Text = $"✗ Variable '{txtVarName.Text.Trim()}' already exists";
                btnOK.Enabled = false;
                return false;
            }

            // Address validation
            int byteOffset = (int)numVarOffset.Value;
            string dataType = cmbVarType.SelectedItem?.ToString() ?? "BOOL";

            if (dataType == "BOOL")
            {
                int bitOffset = cmbBitOffset.SelectedIndex;
                if (!BitAddressingHelper.IsBitAddressAvailable(dataBlock.Variables, byteOffset, bitOffset, dataBlock.Size, originalVariableName))
                {
                    var conflict = dataBlock.Variables.FirstOrDefault(v =>
                        v.Name != originalVariableName && (
                        (v.DataType == "BOOL" && v.Offset == byteOffset && v.BitOffset == bitOffset) ||
                        (v.DataType != "BOOL" && v.Offset <= byteOffset && v.Offset + v.GetSize() > byteOffset)));

                    if (conflict != null)
                    {
                        lblValidation.Text = $"✗ Address {byteOffset}.{bitOffset} conflicts with '{conflict.Name}'";
                    }
                    else
                    {
                        lblValidation.Text = $"✗ Address {byteOffset}.{bitOffset} is out of bounds";
                    }
                    btnOK.Enabled = false;
                    return false;
                }
            }
            else
            {
                int size = GetCurrentDataTypeSize();
                if (!IsOffsetAvailableForNonBool(byteOffset, size, originalVariableName))
                {
                    if (byteOffset + size > dataBlock.Size)
                    {
                        lblValidation.Text = $"✗ Variable exceeds data block size";
                    }
                    else
                    {
                        lblValidation.Text = $"✗ Offset {byteOffset} conflicts with existing variables";
                    }
                    btnOK.Enabled = false;
                    return false;
                }
            }

            // All validation passed
            lblValidation.Text = "✓ Valid configuration";
            lblValidation.ForeColor = Color.Green;
            btnOK.Enabled = true;
            return true;
        }

        private int GetCurrentDataTypeSize()
        {
            return (cmbVarType.SelectedItem?.ToString()) switch
            {
                "BOOL" => 0, // Handled specially
                "BYTE" => 1,
                "WORD" => 2,
                "DWORD" => 4,
                "INT" => 2,
                "DINT" => 4,
                "REAL" => 4,
                "STRING" => 256,
                _ => 1
            };
        }

        private bool IsOffsetAvailableForNonBool(int offset, int size, string? excludeVariableName = null)
        {
            if (offset < 0 || offset + size > dataBlock.Size)
                return false;

            // Check conflicts with existing variables
            foreach (var variable in dataBlock.Variables)
            {
                if (variable.Name == excludeVariableName)
                    continue; // Skip the variable we're editing

                if (variable.DataType == "BOOL")
                {
                    // Check if any byte in our range conflicts with BOOL variables
                    for (int i = 0; i < size; i++)
                    {
                        if (variable.Offset == offset + i)
                            return false; // Byte has BOOL variables
                    }
                }
                else
                {
                    // Check byte-level overlap
                    int varEnd = variable.Offset + variable.GetSize();
                    if (offset < varEnd && variable.Offset < offset + size)
                        return false;
                }
            }

            return true;
        }
    }
}