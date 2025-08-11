using System;
using System.Linq;
using System.Windows.Forms;

namespace SnapServerSoftPLC
{
    public partial class Form1 : Form
    {
        private PLCManager plcManager;

        public Form1()
        {
            InitializeComponent();
            
            plcManager = new PLCManager();
            plcManager.StatusChanged += PLCManager_StatusChanged;
            plcManager.DataBlocksChanged += PLCManager_DataBlocksChanged;
            
            RefreshDataBlocksList();
            timerStatus.Start();
            
            // Update UI to reflect current PLC state (in case it auto-started)
            UpdateUIForPLCState();
        }

        private void PLCManager_StatusChanged(object? sender, string message)
        {
            this.Invoke((MethodInvoker)delegate {
                LogMessage(message);
                statusLabel.Text = message;
            });
        }

        private void PLCManager_DataBlocksChanged(object? sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate {
                RefreshDataBlocksList();
            });
        }

        private void LogMessage(string message)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            txtLog.AppendText($"[{timestamp}] {message}\r\n");
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
        }

        private void btnStartPLC_Click(object sender, EventArgs e)
        {
            if (plcManager.StartPLC())
            {
                btnStartPLC.Enabled = false;
                btnStopPLC.Enabled = true;
                lblStatus.Text = "Running";
                lblStatus.ForeColor = System.Drawing.Color.Green;
            }
        }

        private void btnStopPLC_Click(object sender, EventArgs e)
        {
            if (plcManager.StopPLC())
            {
                btnStartPLC.Enabled = true;
                btnStopPLC.Enabled = false;
                lblStatus.Text = "Stopped";
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void btnAddDataBlock_Click(object sender, EventArgs e)
        {
            using (var dialog = new AddDataBlockDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (plcManager.AddDataBlock(dialog.DBNumber, dialog.DBSize, dialog.DBName, dialog.DBComment))
                    {
                        RefreshDataBlocksList();
                    }
                }
            }
        }

        private void btnRemoveDataBlock_Click(object sender, EventArgs e)
        {
            if (lstDataBlocks.SelectedItem != null)
            {
                string selectedText = lstDataBlocks.SelectedItem.ToString() ?? "";
                if (selectedText.StartsWith("DB"))
                {
                    string dbNumberStr = selectedText.Split(' ')[0].Substring(2);
                    if (int.TryParse(dbNumberStr, out int dbNumber))
                    {
                        if (MessageBox.Show($"Are you sure you want to remove {selectedText}?", 
                                          "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            plcManager.RemoveDataBlock(dbNumber);
                            RefreshDataBlocksList();
                            RefreshVariablesList();
                        }
                    }
                }
            }
        }

        private void lstDataBlocks_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshVariablesList();
        }

        private void btnAddVariable_Click(object sender, EventArgs e)
        {
            if (lstDataBlocks.SelectedItem != null)
            {
                string selectedText = lstDataBlocks.SelectedItem.ToString() ?? "";
                if (selectedText.StartsWith("DB"))
                {
                    string dbNumberStr = selectedText.Split(' ')[0].Substring(2);
                    if (int.TryParse(dbNumberStr, out int dbNumber))
                    {
                        var dataBlock = plcManager.GetDataBlock(dbNumber);
                        if (dataBlock != null)
                        {
                            // Use bit-aware dialog with full bit addressing support
                            using (var dialog = new BitAwareAddVariableDialog(dataBlock))
                            {
                                if (dialog.ShowDialog() == DialogResult.OK)
                                {
                                    if (plcManager.AddVariableToDataBlock(dbNumber, dialog.VarName, 
                                        dialog.VarType, dialog.VarOffset, dialog.VarComment, dialog.VarBitOffset))
                                    {
                                        RefreshVariablesList();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a data block first.", "No Data Block Selected", 
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnGroupAddVariable_Click(object sender, EventArgs e)
        {
            if (lstDataBlocks.SelectedItem != null)
            {
                string selectedText = lstDataBlocks.SelectedItem.ToString() ?? "";
                if (selectedText.StartsWith("DB"))
                {
                    string dbNumberStr = selectedText.Split(' ')[0].Substring(2);
                    if (int.TryParse(dbNumberStr, out int dbNumber))
                    {
                        var dataBlock = plcManager.GetDataBlock(dbNumber);
                        if (dataBlock != null)
                        {
                            // Use group add dialog for adding multiple variables
                            using (var dialog = new GroupAddVariableDialog(dataBlock))
                            {
                                if (dialog.ShowDialog() == DialogResult.OK)
                                {
                                    // Add all variables from the group
                                    int successCount = 0;
                                    int failCount = 0;
                                    
                                    foreach (var varSpec in dialog.Variables)
                                    {
                                        if (plcManager.AddVariableToDataBlock(dbNumber, varSpec.Name, 
                                            varSpec.DataType, varSpec.Offset, varSpec.Comment, varSpec.BitOffset))
                                        {
                                            successCount++;
                                        }
                                        else
                                        {
                                            failCount++;
                                        }
                                    }
                                    
                                    RefreshVariablesList();
                                    
                                    // Show result summary
                                    if (failCount == 0)
                                    {
                                        MessageBox.Show($"Successfully added {successCount} variables.", 
                                                      "Group Add Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        MessageBox.Show($"Added {successCount} variables successfully.\n{failCount} variables failed to add.", 
                                                      "Group Add Complete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a data block first.", "No Data Block Selected", 
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnUpdateValue_Click(object sender, EventArgs e)
        {
            if (lstVariables.SelectedItems.Count > 0 && lstDataBlocks.SelectedItem != null)
            {
                var selectedVar = lstVariables.SelectedItems[0];
                string varName = selectedVar.Text;
                string varType = selectedVar.SubItems[1].Text;
                string currentValue = selectedVar.SubItems[3].Text;
                
                string selectedText = lstDataBlocks.SelectedItem.ToString() ?? "";
                if (selectedText.StartsWith("DB"))
                {
                    string dbNumberStr = selectedText.Split(' ')[0].Substring(2);
                    if (int.TryParse(dbNumberStr, out int dbNumber))
                    {
                        using (var dialog = new UpdateValueDialog(varName, varType, currentValue))
                        {
                            if (dialog.ShowDialog() == DialogResult.OK)
                            {
                                if (plcManager.SetVariableValue(dbNumber, varName, dialog.NewValue))
                                {
                                    RefreshVariablesList();
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a variable to update.", "No Variable Selected", 
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void RefreshDataBlocksList()
        {
            lstDataBlocks.Items.Clear();
            foreach (var dataBlock in plcManager.DataBlocks.Values)
            {
                string status = dataBlock.IsRegistered ? "✓" : "✗";
                lstDataBlocks.Items.Add($"DB{dataBlock.Number} - {dataBlock.Name} [{dataBlock.Size} bytes] {status}");
            }
        }

        private void RefreshVariablesList()
        {
            lstVariables.Items.Clear();
            
            if (lstDataBlocks.SelectedItem != null)
            {
                string selectedText = lstDataBlocks.SelectedItem.ToString() ?? "";
                if (selectedText.StartsWith("DB"))
                {
                    string dbNumberStr = selectedText.Split(' ')[0].Substring(2);
                    if (int.TryParse(dbNumberStr, out int dbNumber))
                    {
                        var dataBlock = plcManager.GetDataBlock(dbNumber);
                        if (dataBlock != null)
                        {
                            foreach (var variable in dataBlock.Variables)
                            {
                                var item = new ListViewItem(variable.Name);
                                item.SubItems.Add(variable.DataType);
                                item.SubItems.Add(variable.FullAddress); // Shows bit addressing for BOOL variables
                                item.SubItems.Add(variable.Value?.ToString() ?? "");
                                item.SubItems.Add(variable.Comment);
                                lstVariables.Items.Add(item);
                            }
                        }
                    }
                }
            }
        }

        private void UpdateUIForPLCState()
        {
            // Update UI buttons and status to reflect current PLC state
            if (plcManager.IsRunning)
            {
                btnStartPLC.Enabled = false;
                btnStopPLC.Enabled = true;
                lblStatus.Text = "Running";
                lblStatus.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                btnStartPLC.Enabled = true;
                btnStopPLC.Enabled = false;
                lblStatus.Text = "Stopped";
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void timerStatus_Tick(object sender, EventArgs e)
        {
            var status = plcManager.GetServerStatus();
            
            lblServerStatus.Text = status.IsRunning ? "Server: Running" : "Server: Stopped";
            lblClients.Text = $"Clients: {status.ClientsCount}";
            
            if (status.IsRunning && status.ClientsCount > 0)
            {
                lblClients.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblClients.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void btnEditDataBlock_Click(object sender, EventArgs e)
        {
            if (lstDataBlocks.SelectedItem != null)
            {
                string selectedText = lstDataBlocks.SelectedItem.ToString() ?? "";
                if (selectedText.StartsWith("DB"))
                {
                    string dbNumberStr = selectedText.Split(' ')[0].Substring(2);
                    if (int.TryParse(dbNumberStr, out int dbNumber))
                    {
                        var dataBlock = plcManager.GetDataBlock(dbNumber);
                        if (dataBlock != null)
                        {
                            using (var dialog = new EditDataBlockDialog(dbNumber, dataBlock.Name, dataBlock.Size, dataBlock.Comment))
                            {
                                if (dialog.ShowDialog() == DialogResult.OK)
                                {
                                    if (plcManager.UpdateDataBlock(dbNumber, dialog.DBName, dialog.DBSize, dialog.DBComment))
                                    {
                                        RefreshDataBlocksList();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a data block to edit.", "No Data Block Selected", 
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCopyDataBlock_Click(object sender, EventArgs e)
        {
            if (lstDataBlocks.SelectedItem != null)
            {
                string selectedText = lstDataBlocks.SelectedItem.ToString() ?? "";
                if (selectedText.StartsWith("DB"))
                {
                    string dbNumberStr = selectedText.Split(' ')[0].Substring(2);
                    if (int.TryParse(dbNumberStr, out int dbNumber))
                    {
                        var dataBlock = plcManager.GetDataBlock(dbNumber);
                        if (dataBlock != null)
                        {
                            using (var dialog = new CopyDataBlockDialog(dbNumber, dataBlock.Name))
                            {
                                if (dialog.ShowDialog() == DialogResult.OK)
                                {
                                    if (plcManager.CopyDataBlock(dbNumber, dialog.TargetDBNumber, dialog.NewDBName))
                                    {
                                        RefreshDataBlocksList();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a data block to copy.", "No Data Block Selected", 
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEditVariable_Click(object sender, EventArgs e)
        {
            if (lstVariables.SelectedItems.Count > 0 && lstDataBlocks.SelectedItem != null)
            {
                var selectedVar = lstVariables.SelectedItems[0];
                string varName = selectedVar.Text;
                string varType = selectedVar.SubItems[1].Text;
                string address = selectedVar.SubItems[2].Text;
                string varComment = selectedVar.SubItems[4].Text;
                
                // Parse address (could be "5" or "5.3" for BOOL)
                int varOffset;
                if (address.Contains("."))
                {
                    var parts = address.Split('.');
                    varOffset = int.Parse(parts[0]);
                }
                else
                {
                    varOffset = int.Parse(address);
                }
                
                string selectedText = lstDataBlocks.SelectedItem.ToString() ?? "";
                if (selectedText.StartsWith("DB"))
                {
                    string dbNumberStr = selectedText.Split(' ')[0].Substring(2);
                    if (int.TryParse(dbNumberStr, out int dbNumber))
                    {
                        var dataBlock = plcManager.GetDataBlock(dbNumber);
                        if (dataBlock != null)
                        {
                            // Find the actual variable to get bit offset
                            var variable = dataBlock.Variables.Find(v => v.Name == varName);
                            int bitOffset = variable?.BitOffset ?? 0;
                            
                            // Use bit-aware dialog with full bit addressing support
                            using (var dialog = new BitAwareEditVariableDialog(dataBlock, varName, varType, varOffset, varComment, bitOffset))
                            {
                                if (dialog.ShowDialog() == DialogResult.OK)
                                {
                                    if (plcManager.UpdateVariable(dbNumber, varName, dialog.VarName, dialog.VarType, dialog.VarOffset, dialog.VarComment, dialog.VarBitOffset))
                                    {
                                        RefreshVariablesList();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a variable to edit.", "No Variable Selected", 
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDeleteVariable_Click(object sender, EventArgs e)
        {
            if (lstVariables.SelectedItems.Count > 0 && lstDataBlocks.SelectedItem != null)
            {
                var selectedVar = lstVariables.SelectedItems[0];
                string varName = selectedVar.Text;
                
                string selectedText = lstDataBlocks.SelectedItem.ToString() ?? "";
                if (selectedText.StartsWith("DB"))
                {
                    string dbNumberStr = selectedText.Split(' ')[0].Substring(2);
                    if (int.TryParse(dbNumberStr, out int dbNumber))
                    {
                        if (MessageBox.Show($"Are you sure you want to delete variable '{varName}'?", 
                                          "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            if (plcManager.DeleteVariable(dbNumber, varName))
                            {
                                RefreshVariablesList();
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a variable to delete.", "No Variable Selected", 
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnNetworkConfig_Click(object sender, EventArgs e)
        {
            var (port, autoStart, serverName, bindAddress, rack, slot) = plcManager.GetConfiguration();
            
            using (var dialog = new NetworkConfigDialog(port, bindAddress, rack, slot, autoStart, serverName))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    plcManager.SetNetworkConfiguration(dialog.Port, dialog.BindAddress, dialog.Rack, dialog.Slot, dialog.AutoStart, dialog.ServerName);
                    
                    // Show restart message if server is running
                    if (plcManager.IsRunning)
                    {
                        MessageBox.Show("Network configuration updated. Please restart the PLC server for changes to take effect.", 
                                      "Configuration Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            plcManager?.Dispose();
        }
    }
}
