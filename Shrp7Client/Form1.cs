using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sharp7;

namespace Shrp7Client
{
    public partial class Form1 : Form
    {
        private S7Client client;
        private bool isConnected = false;

        public Form1()
        {
            InitializeComponent();
            client = new S7Client();
            
            // Set default data type selection
            cmbVarDataType.SelectedIndex = 0; // BOOL
            
            UpdateConnectionStatus();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                string ip = txtIP.Text.Trim();
                int rack = (int)numRack.Value;
                int slot = (int)numSlot.Value;

                LogMessage($"Connecting to {ip}, Rack: {rack}, Slot: {slot}...");

                int result = client.ConnectTo(ip, rack, slot);
                
                if (result == 0)
                {
                    isConnected = true;
                    LogMessage("✓ Connected successfully!");
                    
                    // Get PLC info
                    var cpuInfo = new S7Client.S7CpuInfo();
                    if (client.GetCpuInfo(ref cpuInfo) == 0)
                    {
                        LogMessage($"PLC Info - ModuleTypeName: {cpuInfo.ModuleTypeName}, SerialNumber: {cpuInfo.SerialNumber}");
                    }
                }
                else
                {
                    isConnected = false;
                    LogMessage($"✗ Connection failed: {client.ErrorText(result)}");
                }
            }
            catch (Exception ex)
            {
                isConnected = false;
                LogMessage($"✗ Connection error: {ex.Message}");
            }
            
            UpdateConnectionStatus();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                client.Disconnect();
                isConnected = false;
                LogMessage("Disconnected from PLC");
            }
            catch (Exception ex)
            {
                LogMessage($"Disconnect error: {ex.Message}");
            }
            
            UpdateConnectionStatus();
        }

        private void btnReadDataBlock_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                MessageBox.Show("Not connected to PLC", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int dbNumber = (int)numDBNumber.Value;
                int startByte = (int)numStartByte.Value;
                int length = (int)numLength.Value;

                LogMessage($"Reading DB{dbNumber}, Start: {startByte}, Length: {length} bytes...");

                byte[] buffer = new byte[length];
                int result = client.DBRead(dbNumber, startByte, length, buffer);

                if (result == 0)
                {
                    LogMessage($"✓ Successfully read {length} bytes from DB{dbNumber}");
                    DisplayDataBlockDump(buffer, startByte);
                }
                else
                {
                    LogMessage($"✗ Read failed: {client.ErrorText(result)}");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"✗ Read error: {ex.Message}");
            }
        }

        private void btnReadVariable_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                MessageBox.Show("Not connected to PLC", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int dbNumber = (int)numVarDBNumber.Value;
                int offset = (int)numVarOffset.Value;
                int bitOffset = (int)numVarBitOffset.Value;
                string dataType = cmbVarDataType.SelectedItem?.ToString() ?? "BOOL";

                LogMessage($"Reading variable: DB{dbNumber}.{offset}.{bitOffset} as {dataType}...");

                object value = ReadVariableValue(dbNumber, offset, bitOffset, dataType);
                
                if (value != null)
                {
                    txtVariableValue.Text = value.ToString();
                    LogMessage($"✓ Variable value: {value} ({dataType})");
                }
                else
                {
                    LogMessage("✗ Failed to read variable value");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"✗ Variable read error: {ex.Message}");
            }
        }

        private void btnWriteVariable_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                MessageBox.Show("Not connected to PLC", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int dbNumber = (int)numVarDBNumber.Value;
                int offset = (int)numVarOffset.Value;
                int bitOffset = (int)numVarBitOffset.Value;
                string dataType = cmbVarDataType.SelectedItem?.ToString() ?? "BOOL";
                string valueText = txtVariableValue.Text;

                LogMessage($"Writing variable: DB{dbNumber}.{offset}.{bitOffset} = {valueText} ({dataType})...");

                bool success = WriteVariableValue(dbNumber, offset, bitOffset, dataType, valueText);
                
                if (success)
                {
                    LogMessage($"✓ Variable written successfully");
                }
                else
                {
                    LogMessage("✗ Failed to write variable value");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"✗ Variable write error: {ex.Message}");
            }
        }

        private void cmbVarDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string dataType = cmbVarDataType.SelectedItem?.ToString() ?? "BOOL";
            
            // Show/hide bit offset for BOOL variables
            numVarBitOffset.Visible = dataType == "BOOL";
            lblBitOffset.Visible = dataType == "BOOL";
            
            // Update label text
            if (dataType == "BOOL")
            {
                lblVarAddress.Text = $"Address (DB{numVarDBNumber.Value}.{numVarOffset.Value}.{numVarBitOffset.Value}):";
            }
            else
            {
                lblVarAddress.Text = $"Address (DB{numVarDBNumber.Value}.{numVarOffset.Value}):";
            }
        }

        private void numVarDBNumber_ValueChanged(object sender, EventArgs e) => UpdateVariableAddressLabel();
        private void numVarOffset_ValueChanged(object sender, EventArgs e) => UpdateVariableAddressLabel();
        private void numVarBitOffset_ValueChanged(object sender, EventArgs e) => UpdateVariableAddressLabel();

        private void UpdateVariableAddressLabel()
        {
            string dataType = cmbVarDataType.SelectedItem?.ToString() ?? "BOOL";
            if (dataType == "BOOL")
            {
                lblVarAddress.Text = $"Address (DB{numVarDBNumber.Value}.{numVarOffset.Value}.{numVarBitOffset.Value}):";
            }
            else
            {
                lblVarAddress.Text = $"Address (DB{numVarDBNumber.Value}.{numVarOffset.Value}):";
            }
        }

        private object? ReadVariableValue(int dbNumber, int offset, int bitOffset, string dataType)
        {
            try
            {
                switch (dataType)
                {
                    case "BOOL":
                        byte[] boolBuffer = new byte[1];
                        if (client.DBRead(dbNumber, offset, 1, boolBuffer) == 0)
                        {
                            return (boolBuffer[0] & (1 << bitOffset)) != 0;
                        }
                        break;

                    case "BYTE":
                        byte[] byteBuffer = new byte[1];
                        if (client.DBRead(dbNumber, offset, 1, byteBuffer) == 0)
                        {
                            return byteBuffer[0];
                        }
                        break;

                    case "WORD":
                        byte[] wordBuffer = new byte[2];
                        if (client.DBRead(dbNumber, offset, 2, wordBuffer) == 0)
                        {
                            return S7.GetWordAt(wordBuffer, 0);
                        }
                        break;

                    case "DWORD":
                        byte[] dwordBuffer = new byte[4];
                        if (client.DBRead(dbNumber, offset, 4, dwordBuffer) == 0)
                        {
                            return S7.GetDWordAt(dwordBuffer, 0);
                        }
                        break;

                    case "INT":
                        byte[] intBuffer = new byte[2];
                        if (client.DBRead(dbNumber, offset, 2, intBuffer) == 0)
                        {
                            return S7.GetIntAt(intBuffer, 0);
                        }
                        break;

                    case "DINT":
                        byte[] dintBuffer = new byte[4];
                        if (client.DBRead(dbNumber, offset, 4, dintBuffer) == 0)
                        {
                            return S7.GetDIntAt(dintBuffer, 0);
                        }
                        break;

                    case "REAL":
                        byte[] realBuffer = new byte[4];
                        if (client.DBRead(dbNumber, offset, 4, realBuffer) == 0)
                        {
                            return S7.GetRealAt(realBuffer, 0);
                        }
                        break;

                    case "STRING":
                        byte[] stringBuffer = new byte[256];
                        if (client.DBRead(dbNumber, offset, 256, stringBuffer) == 0)
                        {
                            return S7.GetStringAt(stringBuffer, 0);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Error reading {dataType}: {ex.Message}");
            }

            return null;
        }

        private bool WriteVariableValue(int dbNumber, int offset, int bitOffset, string dataType, string valueText)
        {
            try
            {
                switch (dataType)
                {
                    case "BOOL":
                        if (bool.TryParse(valueText, out bool boolValue))
                        {
                            byte[] boolBuffer = new byte[1];
                            if (client.DBRead(dbNumber, offset, 1, boolBuffer) == 0)
                            {
                                if (boolValue)
                                    boolBuffer[0] |= (byte)(1 << bitOffset);
                                else
                                    boolBuffer[0] &= (byte)~(1 << bitOffset);
                                
                                return client.DBWrite(dbNumber, offset, 1, boolBuffer) == 0;
                            }
                        }
                        break;

                    case "BYTE":
                        if (byte.TryParse(valueText, out byte byteValue))
                        {
                            byte[] buffer = new byte[1];
                            S7.SetByteAt(buffer, 0, byteValue);
                            return client.DBWrite(dbNumber, offset, 1, buffer) == 0;
                        }
                        break;

                    case "WORD":
                        if (ushort.TryParse(valueText, out ushort wordValue))
                        {
                            byte[] buffer = new byte[2];
                            S7.SetWordAt(buffer, 0, wordValue);
                            return client.DBWrite(dbNumber, offset, 2, buffer) == 0;
                        }
                        break;

                    case "DWORD":
                        if (uint.TryParse(valueText, out uint dwordValue))
                        {
                            byte[] buffer = new byte[4];
                            S7.SetDWordAt(buffer, 0, dwordValue);
                            return client.DBWrite(dbNumber, offset, 4, buffer) == 0;
                        }
                        break;

                    case "INT":
                        if (short.TryParse(valueText, out short intValue))
                        {
                            byte[] buffer = new byte[2];
                            S7.SetIntAt(buffer, 0, intValue);
                            return client.DBWrite(dbNumber, offset, 2, buffer) == 0;
                        }
                        break;

                    case "DINT":
                        if (int.TryParse(valueText, out int dintValue))
                        {
                            byte[] buffer = new byte[4];
                            S7.SetDIntAt(buffer, 0, dintValue);
                            return client.DBWrite(dbNumber, offset, 4, buffer) == 0;
                        }
                        break;

                    case "REAL":
                        if (float.TryParse(valueText, out float realValue))
                        {
                            byte[] buffer = new byte[4];
                            S7.SetRealAt(buffer, 0, realValue);
                            return client.DBWrite(dbNumber, offset, 4, buffer) == 0;
                        }
                        break;

                    case "STRING":
                        byte[] stringBuffer = new byte[256];
                        S7.SetStringAt(stringBuffer, 0, valueText.Length, valueText);
                        return client.DBWrite(dbNumber, offset, 256, stringBuffer) == 0;
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Error writing {dataType}: {ex.Message}");
            }

            return false;
        }

        private void DisplayDataBlockDump(byte[] data, int startOffset)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Data Block Dump ({data.Length} bytes starting at offset {startOffset}):");
            sb.AppendLine();
            sb.AppendLine("Offset   00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F   ASCII");
            sb.AppendLine("-------- ----------------------------------------------- ----------------");

            for (int i = 0; i < data.Length; i += 16)
            {
                // Offset
                sb.Append($"{startOffset + i:X8} ");

                // Hex bytes
                for (int j = 0; j < 16; j++)
                {
                    if (i + j < data.Length)
                    {
                        sb.Append($"{data[i + j]:X2} ");
                    }
                    else
                    {
                        sb.Append("   ");
                    }
                }

                sb.Append(" ");

                // ASCII representation
                for (int j = 0; j < 16 && i + j < data.Length; j++)
                {
                    byte b = data[i + j];
                    sb.Append(b >= 32 && b < 127 ? (char)b : '.');
                }

                sb.AppendLine();
            }

            txtDataDump.Text = sb.ToString();
        }

        private void UpdateConnectionStatus()
        {
            btnConnect.Enabled = !isConnected;
            btnDisconnect.Enabled = isConnected;
            btnReadDataBlock.Enabled = isConnected;
            btnReadVariable.Enabled = isConnected;
            btnWriteVariable.Enabled = isConnected;
            
            lblConnectionStatus.Text = isConnected ? "✓ Connected" : "✗ Disconnected";
            lblConnectionStatus.ForeColor = isConnected ? System.Drawing.Color.Green : System.Drawing.Color.Red;
        }

        private void LogMessage(string message)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            txtLog.AppendText($"[{timestamp}] {message}\r\n");
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (isConnected)
            {
                client.Disconnect();
            }
            // No need to explicitly destroy - handled by garbage collection
            base.OnFormClosing(e);
        }
    }
}