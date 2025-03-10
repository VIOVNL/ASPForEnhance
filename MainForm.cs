using Renci.SshNet;

namespace ASPForEnhance
{
    public partial class MainForm : Form
    {
        private SshCommandHelper sshHelper = new SshCommandHelper();
        private ServerManager serverManager = new ServerManager();
        private List<WebsiteInfo> currentWebsites = new List<WebsiteInfo>();
        
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
            
            // Initial state of the add website button (disabled until connected)
            addWebsiteButton.Enabled = false;
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
                addWebsiteButton.Enabled = true; // Enable website addition when connected

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
            addWebsiteButton.Enabled = false; // Disable website addition when disconnected
            UpdateServerButtonStates();
                
            websitesDataGridView.DataSource = null;
            currentWebsites.Clear();
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
                currentWebsites.Clear();
                return;
            }
            
            // Store the websites for later use
            currentWebsites = new List<WebsiteInfo>(e.Websites);
            
            // Bind the websites list to the DataGridView
            websitesDataGridView.DataSource = null; // Clear first to force refresh
            websitesDataGridView.DataSource = currentWebsites;
            
            UpdateStatus($"Found {e.Websites.Count} websites");
            
            // Make sure login button is enabled after website discovery completes
            LoginButton.Enabled = true;
        }

        private void AddWebsiteButton_Click(object sender, EventArgs e)
        {
            if (!sshHelper.IsConnected)
            {
                MessageBox.Show("Please connect to a server first.", "Not Connected", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the next available port
            int nextPort = DetermineNextAvailablePort();
            
            // Get the current server IP
            string serverIp = serverIpTextBox.Text.Trim();
            
            // Show the website form
            using var form = new WebsiteForm(serverIp, nextPort);
            if (form.ShowDialog() == DialogResult.OK)
            {
                // Create and upload the service file
                CreateServiceFile(form.WebsiteInfo, form.AspDllPath, form.FolderPath, form.EnableBlazorSignalR);
            }
        }

        private int DetermineNextAvailablePort()
        {
            const int defaultStartingPort = 5020;
            
            if (currentWebsites == null || currentWebsites.Count == 0)
                return defaultStartingPort;
                
            // Find the highest port in use
            int highestPort = defaultStartingPort - 1; // Default if no ports are in use
            
            foreach (var website in currentWebsites)
            {
                if (int.TryParse(website.Port, out int port))
                {
                    if (port > highestPort)
                        highestPort = port;
                }
            }
            
            // Return the next available port (highest + 1)
            return highestPort + 1;
        }

        private void CreateServiceFile(WebsiteInfo websiteInfo, string aspDllPath, string folderPath, bool enableBlazorSignalR)
        {
            if (!sshHelper.IsConnected)
                return;
                
            try
            {
                // Format the service file content based on the template
                string serviceFileContent = GenerateServiceFileContent(websiteInfo, aspDllPath, folderPath);
                
                // Get the service file name
                string serviceFileName = websiteInfo.FileName;
                
                // Create a temporary file locally
                string tempFilePath = Path.Combine(Path.GetTempPath(), serviceFileName);
                File.WriteAllText(tempFilePath, serviceFileContent);
                
                UpdateStatus($"Creating service file {serviceFileName}...");
                
                // Upload the file to the server using SCP
                using (var scpClient = new ScpClient(
                    serverIpTextBox.Text.Trim(),
                    usernameTextBox.Text.Trim(),
                    passwordTextBox.Text))
                {
                    scpClient.Connect();
                    
                    using (var fileStream = new FileStream(tempFilePath, FileMode.Open))
                    {
                        string remoteFilePath = $"/etc/systemd/system/{serviceFileName}";
                        scpClient.Upload(fileStream, remoteFilePath);
                    }
                    
                    // Now create and upload the Apache vhost config
                    string apacheConfigFileName = $"{websiteInfo.Name}.conf";
                    string apacheConfigContent = GenerateApacheConfigContent(websiteInfo, enableBlazorSignalR);
                    
                    // Create a temporary apache config file
                    string tempApacheConfigPath = Path.Combine(Path.GetTempPath(), apacheConfigFileName);
                    File.WriteAllText(tempApacheConfigPath, apacheConfigContent);
                    
                    UpdateStatus($"Creating Apache config file {apacheConfigFileName}...");
                    
                    // Check if vhost directory exists, create if needed
                    sshHelper.ExecuteCommand("sudo mkdir -p /var/local/enhance/vhost_includes");
                    
                    // Upload Apache config file
                    using (var apacheConfigStream = new FileStream(tempApacheConfigPath, FileMode.Open))
                    {
                        string remoteApacheConfigPath = $"/var/local/enhance/vhost_includes/{apacheConfigFileName}";
                        scpClient.Upload(apacheConfigStream, remoteApacheConfigPath);
                    }
                    
                    // Delete the temporary Apache config file
                    File.Delete(tempApacheConfigPath);
                    
                    scpClient.Disconnect();
                }
                
                // Delete the temporary service file
                File.Delete(tempFilePath);
                
                // Reload systemd daemon and enable the service
                sshHelper.ExecuteCommand($"sudo systemctl daemon-reload && sudo systemctl enable {serviceFileName}");
                
                // Restart Apache to apply the vhost configuration
                UpdateStatus("Restarting Apache...");
                sshHelper.ExecuteCommand("sudo service apache2 restart");
                
                // Show success message
                MessageBox.Show($"Website {websiteInfo.Name} has been added successfully.\n\n" +
                                "To start the service, run:\n" +
                                $"sudo systemctl start {serviceFileName}", 
                                "Website Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Refresh the websites list
                GetWebsites();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating service file: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus($"Error: {ex.Message}");
            }
        }
        
        private string GenerateApacheConfigContent(WebsiteInfo websiteInfo, bool enableBlazorSignalR)
        {
            // Base Apache configuration with proxy settings
            string config = $$"""
                              RequestHeader set "X-Forwarded-Proto" expr=%{REQUEST_SCHEME}
                              ProxyPreserveHost On
                              ProxyPass / http://{{websiteInfo.Ip}}:{{websiteInfo.Port}}/
                              ProxyPassReverse / http://{{websiteInfo.Ip}}:{{websiteInfo.Port}}/

                              """;

            // Add WebSocket support for Blazor/SignalR if enabled
            if (enableBlazorSignalR)
            {
                config += $$"""

                            RewriteEngine On
                            RewriteCond %{HTTP:Upgrade} =websocket [NC]
                            RewriteRule /(.*) ws://{{websiteInfo.Ip}}:{{websiteInfo.Port}}/$1 [P,L]

                            """;
            }

            return config;
        }

        private string GenerateServiceFileContent(WebsiteInfo websiteInfo, string aspDllPath, string folderPath)
        {
            // Extract the working directory (folder path) from the DLL path
            string workingDirectory = Path.GetDirectoryName(aspDllPath)?.Replace('\\', '/') ?? $"/var/www/{websiteInfo.Id}/{folderPath}";
            
            // Extract the DLL filename
            string dllFilename = Path.GetFileName(aspDllPath);
            
            // Format the service file content based on the template
            return $"""
                    [Service]
                    WorkingDirectory={workingDirectory}
                    ExecStart=/usr/bin/dotnet {workingDirectory}/{dllFilename}
                    Restart=always
                    # Restart service after 10 seconds if the dotnet service crashes:
                    RestartSec=10
                    KillSignal=SIGINT
                    SyslogIdentifier=AspForEnhance{websiteInfo.Name.Replace(".", "")}
                    Environment=ASPNETCORE_ENVIRONMENT=Production
                    Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false
                    Environment=ASPNETCORE_URLS=http://{websiteInfo.Ip}:{websiteInfo.Port}
                    [Install]
                    WantedBy=multi-user.target

                    # [ASPForEnhance Name="{websiteInfo.Name}"]
                    # [ASPForEnhance Id="{websiteInfo.Id}"]
                    # [ASPForEnhance Ip="{websiteInfo.Ip}"]
                    # [ASPForEnhance Port="{websiteInfo.Port}"]
                    """;
        }
    }
}
