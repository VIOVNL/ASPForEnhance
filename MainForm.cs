using Renci.SshNet;

namespace ASPForEnhance
{
    public partial class MainForm : Form
    {
        private SshCommandHelper sshHelper = new SshCommandHelper();
        private ServerManager serverManager = new ServerManager();
        private List<WebsiteInfo> currentWebsites = new List<WebsiteInfo>();
        private string currentServiceName = string.Empty; // Track the current service name
        
        // Control to hold our original connection UI elements
        private Panel connectionPanel;
        // Control to hold our new tab control with websites and service management
        private Panel mainContentPanel;
        
        public MainForm()
        {
            InitializeComponent();
            
            // Wire up event handlers
            sshHelper.ConnectionStatusChanged += SshHelper_ConnectionStatusChanged;
            sshHelper.WebsitesDiscovered += SshHelper_WebsitesDiscovered;
            sshHelper.ConnectionCompleted += SshHelper_ConnectionCompleted;
            sshHelper.WebsiteCreated += SshHelper_WebsiteCreated;
            
            // Wire up new service management event handlers
            sshHelper.ServiceStatusReceived += SshHelper_ServiceStatusReceived;
            sshHelper.ServiceOperationCompleted += SshHelper_ServiceOperationCompleted;
            sshHelper.ServiceLogsReceived += SshHelper_ServiceLogsReceived;
            
            // Set up DataGridView and UI
            SetupWebsitesDataGridView();
            SetupServiceManagementUI();
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
                HeaderText = "FileName",
                DataPropertyName = "FileName",
                Width = 120
            };
            
            var statusColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Status",
                Name = "Status",
                Width = 80
            };
            
            // Add columns to the DataGridView
            websitesDataGridView.Columns.AddRange(new DataGridViewColumn[]
            {
                nameColumn,
                idColumn,
                ipColumn,
                portColumn,
                fileNameColumn,
                statusColumn
            });
            
            // Add context menu for service management
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Get Status", null, WebsiteContextMenu_GetStatus);
            contextMenu.Items.Add("Restart Service", null, WebsiteContextMenu_RestartService);
            contextMenu.Items.Add("Stop Service", null, WebsiteContextMenu_StopService);
            contextMenu.Items.Add("Disable Service", null, WebsiteContextMenu_DisableService);
            contextMenu.Items.Add("View Logs", null, WebsiteContextMenu_ViewLogs);
            
            websitesDataGridView.ContextMenuStrip = contextMenu;
            websitesDataGridView.CellClick += WebsitesDataGridView_CellClick;
        }
        
        private void SetupServiceManagementUI()
        {
            // First, organize the existing form controls into a top panel
            connectionPanel = new Panel();
            connectionPanel.Dock = DockStyle.Top;
            connectionPanel.Height = 120; // Adjust based on your original controls' height
            connectionPanel.Padding = new Padding(5);
            
            // Create a panel for the main content (tabs)
            mainContentPanel = new Panel();
            mainContentPanel.Dock = DockStyle.Fill;
            
            // Reparent existing controls to the connection panel
            // Get all top-level controls and move server-related ones to the connection panel
            List<Control> controlsToMove = new List<Control>();
            foreach (Control control in Controls)
            {
                // Don't move the status bar label or other system controls
                if (
                    !(control is MenuStrip) && 
                    !(control is StatusStrip) && 
                    !(control is Panel && control.Tag?.ToString() == "mainPanel"))
                {
                    controlsToMove.Add(control);
                }
            }
            
            // Move the controls to the connection panel
            foreach (Control control in controlsToMove)
            {
                Controls.Remove(control);
                connectionPanel.Controls.Add(control);
            }
            
            // Create tab control to organize the interface
            var tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;
            
            // Create websites tab
            var websitesTab = new TabPage("Websites");
            websitesTab.Controls.Add(websitesDataGridView);
            websitesDataGridView.Dock = DockStyle.Fill;
            
            // Create service management tab
            var serviceTab = new TabPage("Service Management");
            
            // Create a split container for service management
            var splitContainer = new SplitContainer();
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Orientation = Orientation.Vertical;
            splitContainer.SplitterDistance = 200;
            
            // Service control panel (top panel)
            var serviceControlPanel = new Panel();
            serviceControlPanel.Dock = DockStyle.Fill;
            
            // Service status display (group box)
            var statusGroupBox = new GroupBox();
            statusGroupBox.Text = "Service Status";
            statusGroupBox.Dock = DockStyle.Top;
            statusGroupBox.Height = 120;
            
            // Status properties layout
            var statusLayout = new TableLayoutPanel();
            statusLayout.Dock = DockStyle.Fill;
            statusLayout.ColumnCount = 2;
            statusLayout.RowCount = 4;
            
            // Add status labels
            statusLayout.Controls.Add(new Label { Text = "Service Name:" }, 0, 0);
            var serviceNameLabel = new Label { Text = "-", Tag = "serviceName" };
            statusLayout.Controls.Add(serviceNameLabel, 1, 0);
            
            statusLayout.Controls.Add(new Label { Text = "Status:" }, 0, 1);
            var serviceStatusLabel = new Label { Text = "-", Tag = "serviceStatus" };
            statusLayout.Controls.Add(serviceStatusLabel, 1, 1);
            
            statusLayout.Controls.Add(new Label { Text = "Active:" }, 0, 2);
            var serviceActiveLabel = new Label { Text = "-", Tag = "serviceActive" };
            statusLayout.Controls.Add(serviceActiveLabel, 1, 2);
            
            statusLayout.Controls.Add(new Label { Text = "Last Operation:" }, 0, 3);
            var lastOperationLabel = new Label { Text = "-", Tag = "lastOperation" };
            statusLayout.Controls.Add(lastOperationLabel, 1, 3);
            
            statusGroupBox.Controls.Add(statusLayout);
            serviceControlPanel.Controls.Add(statusGroupBox);
            
            // Service control buttons
            var buttonPanel = new FlowLayoutPanel();
            buttonPanel.Dock = DockStyle.Top;
            buttonPanel.Padding = new Padding(5);
            buttonPanel.Height = 40;
            
            var getStatusButton = new Button();
            getStatusButton.Text = "Get Status";
            getStatusButton.Click += GetStatusButton_Click;
            getStatusButton.Enabled = false;
            getStatusButton.Tag = "serviceButton";
            
            var restartButton = new Button();
            restartButton.Text = "Restart Service";
            restartButton.Click += RestartServiceButton_Click;
            restartButton.Enabled = false;
            restartButton.Tag = "serviceButton";
            
            var stopButton = new Button();
            stopButton.Text = "Stop Service";
            stopButton.Click += StopServiceButton_Click;
            stopButton.Enabled = false;
            stopButton.Tag = "serviceButton";
            
            var disableButton = new Button();
            disableButton.Text = "Disable Service";
            disableButton.Click += DisableServiceButton_Click;
            disableButton.Enabled = false;
            disableButton.Tag = "serviceButton";
            
            var viewLogsButton = new Button();
            viewLogsButton.Text = "View Logs";
            viewLogsButton.Click += ViewLogsButton_Click;
            viewLogsButton.Enabled = false;
            viewLogsButton.Tag = "serviceButton";
            
            buttonPanel.Controls.AddRange(new Control[] { 
                getStatusButton, 
                restartButton, 
                stopButton, 
                disableButton, 
                viewLogsButton
            });
            
            serviceControlPanel.Controls.Add(buttonPanel);
            splitContainer.Panel1.Controls.Add(serviceControlPanel);
            
            // Service logs display (bottom panel)
            var serviceLogsBox = new RichTextBox();
            serviceLogsBox.Dock = DockStyle.Fill;
            serviceLogsBox.ReadOnly = true;
            serviceLogsBox.Font = new Font("Consolas", 9F);
            serviceLogsBox.BackColor = Color.Black;
            serviceLogsBox.ForeColor = Color.LightGray;
            serviceLogsBox.Tag = "serviceLogs";
            
            splitContainer.Panel2.Controls.Add(serviceLogsBox);
            serviceTab.Controls.Add(splitContainer);
            
            // Add tabs to tab control
            tabControl.TabPages.AddRange(new TabPage[] { websitesTab, serviceTab });
            
            // Add tab control to the main content panel
            mainContentPanel.Controls.Add(tabControl);
            
            // Add panels to the form in the correct order
            Controls.Add(mainContentPanel);  // Add main content first (will be at the back)
            Controls.Add(connectionPanel);   // Add connection panel on top
            
            // Store references to controls we'll need to update
            tabControl.Tag = splitContainer;
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
                // Create and upload the service file asynchronously
                UpdateStatus($"Creating website {form.WebsiteInfo.Name}...");
                sshHelper.CreateWebsiteAsync(form.WebsiteInfo, form.AspDllPath, form.FolderPath, form.EnableBlazorSignalR, passwordTextBox.Text);
                
                // Disable the add website button during creation
                addWebsiteButton.Enabled = false;
            }
        }

        private void SshHelper_WebsiteCreated(object? sender, WebsiteCreatedEventArgs e)
        {
            // Ensure we update the UI from the UI thread
            if (InvokeRequired)
            {
                Invoke(new Action(() => ProcessWebsiteCreationResult(e)));
            }
            else
            {
                ProcessWebsiteCreationResult(e);
            }
        }

        private void ProcessWebsiteCreationResult(WebsiteCreatedEventArgs e)
        {
            // Re-enable the add website button
            addWebsiteButton.Enabled = true;
            
            if (!e.Success)
            {
                MessageBox.Show($"Failed to create website: {e.Error}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus($"Error: {e.Error}");
                return;
            }
            
            // Show success message
            MessageBox.Show($"Website {e.Website?.Name} has been added successfully.", 
                            "Website Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            // Refresh the websites list
            GetWebsites();
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

        // Service management event handlers
        private void SshHelper_ServiceStatusReceived(object? sender, ServiceStatusEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateServiceStatus(e)));
            }
            else
            {
                UpdateServiceStatus(e);
            }
        }
        
        private void SshHelper_ServiceOperationCompleted(object? sender, ServiceOperationEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => ProcessServiceOperation(e)));
            }
            else
            {
                ProcessServiceOperation(e);
            }
        }
        
        private void SshHelper_ServiceLogsReceived(object? sender, ServiceLogsEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => DisplayServiceLogs(e)));
            }
            else
            {
                DisplayServiceLogs(e);
            }
        }
        
        // Service management UI methods
        private void UpdateServiceStatus(ServiceStatusEventArgs e)
        {
            // Enable service management buttons
            foreach (Control control in FindControlsByTag(this, "serviceButton"))
            {
                if (control is Button button)
                {
                    button.Enabled = true;
                }
            }
            
            if (!e.Success)
            {
                MessageBox.Show($"Failed to get service status: {e.Error}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus($"Error: {e.Error}");
                return;
            }
            
            // Update service status display
            var serviceNameLabel = FindControlByTag(this, "serviceName") as Label;
            var serviceStatusLabel = FindControlByTag(this, "serviceStatus") as Label;
            var serviceActiveLabel = FindControlByTag(this, "serviceActive") as Label;
            var lastOperationLabel = FindControlByTag(this, "lastOperation") as Label;
            
            if (serviceNameLabel != null && e.Status != null)
                serviceNameLabel.Text = e.Status.ServiceName;
                
            if (serviceStatusLabel != null && e.Status != null)
                serviceStatusLabel.Text = e.Status.State;
                
            if (serviceActiveLabel != null && e.Status != null)
                serviceActiveLabel.Text = e.Status.IsEnabled ? "Enabled" : "Disabled";
                
            if (lastOperationLabel != null)
                lastOperationLabel.Text = "Status retrieved successfully";
                
            // Save the current service name
            if (e.Status != null)
                currentServiceName = e.Status.ServiceName;
                
            // Update the status in the data grid view
            UpdateWebsiteServiceStatus(e.Status);
            
            UpdateStatus($"Service status retrieved: {e.Status?.State}");
        }
        
        private void ProcessServiceOperation(ServiceOperationEventArgs e)
        {
            // Enable service management buttons
            foreach (Control control in FindControlsByTag(this, "serviceButton"))
            {
                if (control is Button button)
                {
                    button.Enabled = true;
                }
            }
            
            var lastOperationLabel = FindControlByTag(this, "lastOperation") as Label;
            
            if (!e.Success)
            {
                if (lastOperationLabel != null)
                    lastOperationLabel.Text = $"Error: {e.Error}";
                    
                MessageBox.Show($"Service operation failed: {e.Error}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus($"Error: {e.Error}");
                return;
            }
            
            // Update operation result
            if (lastOperationLabel != null)
                lastOperationLabel.Text = $"{e.Operation} completed successfully";
                
            UpdateStatus($"Service {e.Operation} completed successfully");
            
            // Get the updated service status
            if (!string.IsNullOrEmpty(currentServiceName))
            {
                sshHelper.GetServiceStatusAsync(currentServiceName);
            }
        }
        
        private void DisplayServiceLogs(ServiceLogsEventArgs e)
        {
            var logsTextBox = FindControlByTag(this, "serviceLogs") as RichTextBox;
            if (logsTextBox == null) return;
            
            if (!e.Success)
            {
                logsTextBox.Text = $"Error retrieving logs: {e.Error}";
                MessageBox.Show($"Failed to retrieve service logs: {e.Error}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus($"Error: {e.Error}");
                return;
            }
            
            // Display logs
            logsTextBox.Clear();
            logsTextBox.Text = e.Logs;
            
            UpdateStatus($"Service logs retrieved for {currentServiceName}");
        }
        
        // Service control button click handlers
        private void GetStatusButton_Click(object sender, EventArgs e)
        {
            if (!sshHelper.IsConnected)
            {
                MessageBox.Show("Please connect to a server first.", "Not Connected", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Get the selected website
            WebsiteInfo? selectedWebsite = GetSelectedWebsite();
            if (selectedWebsite == null) return;
            
            // Disable buttons during operation
            SetServiceButtonsEnabled(false);
            
            // Get the service status
            sshHelper.GetServiceStatusAsync(selectedWebsite.FileName);
        }
        
        private void RestartServiceButton_Click(object sender, EventArgs e)
        {
            if (!sshHelper.IsConnected)
            {
                MessageBox.Show("Please connect to a server first.", "Not Connected", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Get the selected website
            WebsiteInfo? selectedWebsite = GetSelectedWebsite();
            if (selectedWebsite == null) return;
            
            // Confirm restart
            var result = MessageBox.Show($"Are you sure you want to restart the service '{selectedWebsite.Name}'?", 
                "Confirm Restart", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
            if (result != DialogResult.Yes) return;
            
            // Disable buttons during operation
            SetServiceButtonsEnabled(false);
            
            // Restart the service
            UpdateStatus($"Restarting service {selectedWebsite.FileName}...");
            sshHelper.RestartServiceAsync($"{selectedWebsite.FileName}");
        }
        
        private void StopServiceButton_Click(object sender, EventArgs e)
        {
            if (!sshHelper.IsConnected)
            {
                MessageBox.Show("Please connect to a server first.", "Not Connected", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Get the selected website
            WebsiteInfo? selectedWebsite = GetSelectedWebsite();
            if (selectedWebsite == null) return;
            
            // Confirm stop
            var result = MessageBox.Show($"Are you sure you want to stop the service '{selectedWebsite.Name}'?", 
                "Confirm Stop", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
            if (result != DialogResult.Yes) return;
            
            // Disable buttons during operation
            SetServiceButtonsEnabled(false);
            
            // Stop the service
            UpdateStatus($"Stopping service {selectedWebsite.FileName}...");
            sshHelper.StopServiceAsync($"{selectedWebsite.FileName}");
        }
        
        private void DisableServiceButton_Click(object sender, EventArgs e)
        {
            if (!sshHelper.IsConnected)
            {
                MessageBox.Show("Please connect to a server first.", "Not Connected", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Get the selected website
            WebsiteInfo? selectedWebsite = GetSelectedWebsite();
            if (selectedWebsite == null) return;
            
            // Confirm disable
            var result = MessageBox.Show($"Are you sure you want to disable the service '{selectedWebsite.Name}'?\n\nThis will prevent it from starting automatically at boot.", 
                "Confirm Disable", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                
            if (result != DialogResult.Yes) return;
            
            // Disable buttons during operation
            SetServiceButtonsEnabled(false);
            
            // Disable the service
            UpdateStatus($"Disabling service {selectedWebsite.FileName}...");
            sshHelper.DisableServiceAsync($"{selectedWebsite.FileName}");
        }
        
        private void ViewLogsButton_Click(object sender, EventArgs e)
        {
            if (!sshHelper.IsConnected)
            {
                MessageBox.Show("Please connect to a server first.", "Not Connected", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Get the selected website
            WebsiteInfo? selectedWebsite = GetSelectedWebsite();
            if (selectedWebsite == null) return;
            
            // Clear logs display
            var logsTextBox = FindControlByTag(this, "serviceLogs") as RichTextBox;
            if (logsTextBox != null)
                logsTextBox.Text = "Retrieving logs...";
            
            // Get service logs
            UpdateStatus($"Retrieving logs for {selectedWebsite.FileName}...");
            sshHelper.GetServiceLogsAsync(selectedWebsite.FileName, 100);
        }
        
        // Context menu handlers for the websites DataGridView
        private void WebsiteContextMenu_GetStatus(object? sender, EventArgs e)
        {
            GetStatusButton_Click(sender!, e);
        }
        
        private void WebsiteContextMenu_RestartService(object? sender, EventArgs e)
        {
            RestartServiceButton_Click(sender!, e);
        }
        
        private void WebsiteContextMenu_StopService(object? sender, EventArgs e)
        {
            StopServiceButton_Click(sender!, e);
        }
        
        private void WebsiteContextMenu_DisableService(object? sender, EventArgs e)
        {
            DisableServiceButton_Click(sender!, e);
        }
        
        private void WebsiteContextMenu_ViewLogs(object? sender, EventArgs e)
        {
            ViewLogsButton_Click(sender!, e);
        }
        
        // Helper methods for service management UI
        private WebsiteInfo? GetSelectedWebsite()
        {
            if (websitesDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a website first.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
            
            int selectedIndex = websitesDataGridView.SelectedRows[0].Index;
            if (selectedIndex >= 0 && selectedIndex < currentWebsites.Count)
            {
                return currentWebsites[selectedIndex];
            }
            
            MessageBox.Show("Please select a valid website.", "Invalid Selection", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return null;
        }
        
        private void SetServiceButtonsEnabled(bool enabled)
        {
            foreach (Control control in FindControlsByTag(this, "serviceButton"))
            {
                control.Enabled = enabled;
            }
        }
        
        private Control? FindControlByTag(Control parent, string tag)
        {
            if (parent.Tag?.ToString() == tag) return parent;
            
            foreach (Control control in parent.Controls)
            {
                if (control.Tag?.ToString() == tag) return control;
                
                Control? found = FindControlByTag(control, tag);
                if (found != null) return found;
            }
            
            return null;
        }
        
        private List<Control> FindControlsByTag(Control parent, string tag)
        {
            var result = new List<Control>();
            
            if (parent.Tag?.ToString() == tag)
                result.Add(parent);
                
            foreach (Control control in parent.Controls)
            {
                if (control.Tag?.ToString() == tag)
                    result.Add(control);
                    
                result.AddRange(FindControlsByTag(control, tag));
            }
            
            return result;
        }
        
        private void WebsitesDataGridView_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < currentWebsites.Count)
            {
                // Get the selected website
                WebsiteInfo website = currentWebsites[e.RowIndex];
                
                // Update the service management UI
                var serviceNameLabel = FindControlByTag(this, "serviceName") as Label;
                if (serviceNameLabel != null)
                    serviceNameLabel.Text = website.FileName;
                
                // Store current service name
                currentServiceName = website.FileName;
                
                // Enable service management buttons if connected
                SetServiceButtonsEnabled(sshHelper.IsConnected);
                
                // Get service status automatically
                if (sshHelper.IsConnected)
                {
                    sshHelper.GetServiceStatusAsync(website.FileName);
                }
            }
        }
        
        private void UpdateWebsiteServiceStatus(ServiceStatus? status)
        {
            if (status == null) return;
            
            // Find the row with the matching service name
            for (int i = 0; i < websitesDataGridView.Rows.Count; i++)
            {
                var row = websitesDataGridView.Rows[i];
                string? fileName = row.Cells[4].Value?.ToString();
                
                if (fileName == status.ServiceName)
                {
                    // Update the Status cell
                    row.Cells[5].Value = status.State;
                    
                    // Add color based on status
                    if (status.State.Contains("running"))
                        row.Cells[5].Style.ForeColor = Color.Green;
                    else if (status.State.Contains("stopped") || status.State.Contains("inactive"))
                        row.Cells[5].Style.ForeColor = Color.Red;
                    else if (status.State.Contains("failed"))
                        row.Cells[5].Style.ForeColor = Color.Orange;
                    else
                        row.Cells[5].Style.ForeColor = Color.Gray;
                        
                    break;
                }
            }
        }
    }
}
