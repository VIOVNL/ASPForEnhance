using Renci.SshNet;

namespace ASPForEnhance
{
    public partial class MainForm : Form
    {
        private SshCommandHelper sshHelper = new SshCommandHelper();
        private ServerManager serverManager = new ServerManager();
        
        public MainForm()
        {
            InitializeComponent();
            
            // Wire up event handlers
            sshHelper.ConnectionStatusChanged += SshHelper_ConnectionStatusChanged;
            sshHelper.WebsitesDiscovered += SshHelper_WebsitesDiscovered;
            sshHelper.ConnectionCompleted += SshHelper_ConnectionCompleted;
            
            // Set up DataGridView
            SetupWebsitesDataGridView();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Load servers and populate ComboBox
            RefreshServersList();
            UpdateServerButtonStates();
            UpdateStatus("Ready");
        }
        
        private void SshHelper_ConnectionStatusChanged(object? sender, ConnectionStatusEventArgs e)
        {
            // Ensure we update the UI from the UI thread
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateStatus(e.Status)));
            }
            else
            {
                UpdateStatus(e.Status);
            }
        }

        private void SshHelper_ConnectionCompleted(object? sender, ConnectionCompletedEventArgs e)
        {
            // Ensure we update the UI from the UI thread
            if (InvokeRequired)
            {
                Invoke(new Action(() => ProcessConnectionResult(e)));
            }
            else
            {
                ProcessConnectionResult(e);
            }
        }

        private void ProcessConnectionResult(ConnectionCompletedEventArgs e)
        {
            if (e.Success)
            {
                // Update UI to show connected state
                LoginButton.Text = "Disconnect";
                serversComboBox.Enabled = false;
                serverIpTextBox.Enabled = false;
                usernameTextBox.Enabled = false;
                passwordTextBox.Enabled = false;
                addServerButton.Enabled = false;
                editServerButton.Enabled = false;
                deleteServerButton.Enabled = false;

                // Start discovering websites
                GetWebsites();
            }
            else
            {
                MessageBox.Show("Failed to connect: " + e.Error, "Connection Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                // Make sure UI is in disconnected state
                UpdateUIForDisconnectedState();
            }
        }

        private void UpdateStatus(string message)
        {
            statusLabel.Text = message;
        }

        private void RefreshServersList()
        {
            serversComboBox.Items.Clear();
            foreach (var server in serverManager.Servers)
            {
                serversComboBox.Items.Add(server);
            }
        }

        private void UpdateServerButtonStates()
        {
            bool serverSelected = serversComboBox.SelectedIndex >= 0;
            editServerButton.Enabled = serverSelected;
            deleteServerButton.Enabled = serverSelected;
        }

        private void ServersComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateServerButtonStates();
            
            // Populate the form with selected server data
            if (serversComboBox.SelectedIndex >= 0 && serversComboBox.SelectedItem is ServerInfo serverInfo)
            {
                serverIpTextBox.Text = serverInfo.Hostname;
                usernameTextBox.Text = serverInfo.Username;
                passwordTextBox.Text = serverInfo.Password;
            }
        }

        private void AddServerButton_Click(object sender, EventArgs e)
        {
            using var form = new ServerForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                serverManager.AddServer(form.ServerInfo);
                RefreshServersList();
                serversComboBox.SelectedIndex = serversComboBox.Items.Count - 1;
            }
        }

        private void EditServerButton_Click(object sender, EventArgs e)
        {
            int selectedIndex = serversComboBox.SelectedIndex;
            if (selectedIndex >= 0 && serversComboBox.SelectedItem is ServerInfo serverInfo)
            {
                using var form = new ServerForm(serverInfo);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    serverManager.UpdateServer(selectedIndex, form.ServerInfo);
                    RefreshServersList();
                    serversComboBox.SelectedIndex = selectedIndex;
                }
            }
        }

        private void DeleteServerButton_Click(object sender, EventArgs e)
        {
            int selectedIndex = serversComboBox.SelectedIndex;
            if (selectedIndex >= 0 && serversComboBox.SelectedItem is ServerInfo serverInfo)
            {
                var result = MessageBox.Show($"Are you sure you want to delete the server '{serverInfo.Name}'?", 
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    serverManager.DeleteServer(selectedIndex);
                    RefreshServersList();
                    UpdateServerButtonStates();
                }
            }
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            // Disable the login button during connection/disconnection process
            LoginButton.Enabled = false;
            
            // Check if we're already connected, disconnect if so
            if (sshHelper.IsConnected)
            {
                // Use the new asynchronous disconnect method
                sshHelper.DisconnectAsync();
                return;
            }

            // Get connection info from textboxes
            string serverIp = serverIpTextBox.Text.Trim();
            string username = usernameTextBox.Text.Trim();
            string password = passwordTextBox.Text;

            // Validate input
            if (string.IsNullOrEmpty(serverIp) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter valid server IP, username, and password.", "Input Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoginButton.Enabled = true;
                return;
            }

            // Connect using the new asynchronous method
            sshHelper.ConnectAsync(serverIp, username, password);
        }

        private void GetWebsites()
        {
            if (!sshHelper.IsConnected)
            {
                MessageBox.Show("Not connected to the server. Please log in first.", "Connection Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            UpdateStatus("Searching for websites...");
            websitesDataGridView.DataSource = null;
            
            // Start the website discovery
            sshHelper.DiscoverWebsites();
        }

        private void DisconnectFromServer()
        {
            if (sshHelper.IsConnected)
            {
                // Use the synchronous disconnect for form closing
                sshHelper.Disconnect();
                UpdateUIForDisconnectedState();
            }
        }

        private void UpdateUIForDisconnectedState()
        {
            // Update UI to show disconnected state
            LoginButton.Text = "Login";
            LoginButton.Enabled = true;
            serversComboBox.Enabled = true;
            serverIpTextBox.Enabled = true;
            usernameTextBox.Enabled = true;
            passwordTextBox.Enabled = true;
            addServerButton.Enabled = true;
            UpdateServerButtonStates();
                
            websitesDataGridView.DataSource = null;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            DisconnectFromServer();
            base.OnFormClosing(e);
        }

        private void SetupWebsitesDataGridView()
        {
            websitesDataGridView.AutoGenerateColumns = false;
            websitesDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Create columns
            var nameColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Website Name",
                DataPropertyName = "Name",
                Width = 160,
            };
            
            var idColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "ID",
                DataPropertyName = "Id",
                Width = 100
            };
            
            var ipColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "IP Address",
                DataPropertyName = "Ip",
                Width = 100
            };
            
            var portColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Port",
                DataPropertyName = "Port",
                Width = 60
            };
            
            var fileNameColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Service File",
                DataPropertyName = "FileName",
                Width = 120
            };
            
      
            
            // Add columns to the DataGridView
            websitesDataGridView.Columns.AddRange(new DataGridViewColumn[]
            {
                nameColumn,
                idColumn,
                ipColumn,
                portColumn,
                fileNameColumn
            });
           
        }

        private void SshHelper_WebsitesDiscovered(object? sender, WebsitesDiscoveredEventArgs e)
        {
            // Ensure we update the UI from the UI thread
            if (InvokeRequired)
            {
                Invoke(new Action(() => ProcessWebsiteDiscoveryResult(e)));
            }
            else
            {
                ProcessWebsiteDiscoveryResult(e);
            }
        }
        
        private void ProcessWebsiteDiscoveryResult(WebsitesDiscoveredEventArgs e)
        {
            if (!e.Success)
            {
                MessageBox.Show($"Failed to discover websites: {e.Error}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus($"Error: {e.Error}");
                return;
            }
            
            if (e.Websites == null || e.Websites.Count == 0)
            {
                UpdateStatus("No websites found");
                MessageBox.Show("No websites were found with ASPForEnhance metadata.", 
                    "No Websites Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            // Bind the websites list to the DataGridView
            websitesDataGridView.DataSource = null; // Clear first to force refresh
            websitesDataGridView.DataSource = e.Websites;
            
            UpdateStatus($"Found {e.Websites.Count} websites");
            
            // Make sure login button is enabled after website discovery completes
            LoginButton.Enabled = true;
        }
    }
}
