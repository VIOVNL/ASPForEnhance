using Renci.SshNet;
using System.Data;

namespace ASPForEnhance
{
    public partial class MainForm : Form
    {
        private SshCommandHelper sshHelper = new SshCommandHelper();
        private ServerManager serverManager = new ServerManager();
        private List<WebsiteInfo> currentWebsites = new List<WebsiteInfo>();
        private string currentServiceName = string.Empty; // Track the current service name
        
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

            // Set DataGridView properties
            websitesDataGridView.ReadOnly = true;
            websitesDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Load servers and populate ComboBox
            RefreshServersList();
            UpdateServerButtonStates();
            UpdateStatus("Ready");
            this.Height = 172;
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
                if (e.Hostname != null)
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
                    this.Height = 1046;
                    addWebsiteButton.Enabled = true; // Enable website addition when connected

                    // Start discovering websites
                    GetWebsites();
                }
                else
                {
                    UpdateStatus("Disconnected");
                    UpdateUIForDisconnectedState();
                }
              
            }
            else
            {
                UpdateStatus("Failed to connect");
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
                serverManager.DeleteServer(selectedIndex);
                RefreshServersList();
                UpdateServerButtonStates();
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
                statusLabel.Visible = false;
                return;
            }

       
            statusLabel.Visible = true;
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
                sshHelper.DisconnectAsync();
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
            this.Height = 172;
            UpdateServerButtonStates();
                
            websitesDataGridView.DataSource = null;
            currentWebsites.Clear();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            DisconnectFromServer();
            base.OnFormClosing(e);
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
              
                UpdateStatus($"Error: {e.Error}");
                return;
            }
            
            if (e.Websites == null || e.Websites.Count == 0)
            {
                UpdateStatus("No websites found");
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
                UpdateStatus("Please connect to a server first.");
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
                sshHelper.CreateWebsiteAsync(form.WebsiteInfo, form.DllFileName, form.FolderPath, form.EnableBlazorSignalR, passwordTextBox.Text);
                
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
                UpdateStatus($"Failed to get service status");
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
            // Scroll to the end of the logs
            logsTextBox.SelectionStart = logsTextBox.Text.Length;
            logsTextBox.ScrollToCaret();
            logsTextBox.Focus();
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
        
     
    }
}
