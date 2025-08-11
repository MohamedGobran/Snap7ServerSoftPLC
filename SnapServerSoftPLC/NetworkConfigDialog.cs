using System;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;

namespace SnapServerSoftPLC
{
    public partial class NetworkConfigDialog : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Port { get; private set; } = 102;
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string BindAddress { get; private set; } = "0.0.0.0";
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Rack { get; private set; } = 0;
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Slot { get; private set; } = 1;
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AutoStart { get; private set; } = false;
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ServerName { get; private set; } = "Snap7 Soft PLC";

        public NetworkConfigDialog(int port = 102, string bindAddress = "0.0.0.0", int rack = 0, int slot = 1, bool autoStart = false, string serverName = "Snap7 Soft PLC")
        {
            InitializeComponent();
            
            numPort.Value = port;
            txtBindAddress.Text = bindAddress;
            numRack.Value = rack;
            numSlot.Value = slot;
            chkAutoStart.Checked = autoStart;
            txtServerName.Text = serverName;
            
            // Populate bind address dropdown with available IPs
            PopulateBindAddresses();
        }

        private void PopulateBindAddresses()
        {
            txtBindAddress.Items.Clear();
            txtBindAddress.Items.Add("0.0.0.0 (All interfaces)");
            txtBindAddress.Items.Add("127.0.0.1 (Localhost)");

            try
            {
                string hostName = Dns.GetHostName();
                IPHostEntry hostEntry = Dns.GetHostEntry(hostName);
                
                foreach (IPAddress ip in hostEntry.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        txtBindAddress.Items.Add($"{ip} ({hostName})");
                    }
                }
            }
            catch (Exception ex)
            {
                // If we can't get host IPs, just continue with defaults
                System.Diagnostics.Debug.WriteLine($"Error getting host IPs: {ex.Message}");
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Validate port
            if (numPort.Value < 1 || numPort.Value > 65535)
            {
                MessageBox.Show("Port must be between 1 and 65535.", "Invalid Port", 
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate bind address
            string bindAddr = txtBindAddress.Text.Trim();
            if (bindAddr.Contains("("))
            {
                bindAddr = bindAddr.Split('(')[0].Trim();
            }

            if (!IPAddress.TryParse(bindAddr, out _))
            {
                MessageBox.Show("Please enter a valid IP address.", "Invalid IP Address", 
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate server name
            if (string.IsNullOrWhiteSpace(txtServerName.Text))
            {
                MessageBox.Show("Server name cannot be empty.", "Invalid Server Name", 
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Set properties
            Port = (int)numPort.Value;
            BindAddress = bindAddr;
            Rack = (int)numRack.Value;
            Slot = (int)numSlot.Value;
            AutoStart = chkAutoStart.Checked;
            ServerName = txtServerName.Text.Trim();

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnTestBinding_Click(object sender, EventArgs e)
        {
            string bindAddr = txtBindAddress.Text.Trim();
            if (bindAddr.Contains("("))
            {
                bindAddr = bindAddr.Split('(')[0].Trim();
            }

            if (!IPAddress.TryParse(bindAddr, out IPAddress? ipAddress))
            {
                MessageBox.Show("Please enter a valid IP address first.", "Invalid IP Address", 
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Try to create a test socket on the address/port
                using var testSocket = new System.Net.Sockets.Socket(
                    System.Net.Sockets.AddressFamily.InterNetwork, 
                    System.Net.Sockets.SocketType.Stream, 
                    System.Net.Sockets.ProtocolType.Tcp);
                
                testSocket.Bind(new IPEndPoint(ipAddress, (int)numPort.Value));
                testSocket.Close();
                
                MessageBox.Show($"Successfully bound to {bindAddr}:{numPort.Value}", "Binding Test", 
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to bind to {bindAddr}:{numPort.Value}\n\nError: {ex.Message}", 
                              "Binding Test Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}