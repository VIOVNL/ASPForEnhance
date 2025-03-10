using Renci.SshNet;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace ASPForEnhance
{
    public class SshCommandHelper
    {
        private SshClient? sshClient;
        private readonly BackgroundWorker worker;

        // Event for command completion
        public event EventHandler<CommandCompletedEventArgs>? CommandCompleted;
        
        // Event for connection status changes
        public event EventHandler<ConnectionStatusEventArgs>? ConnectionStatusChanged;

        // Event for website discovery completion
        public event EventHandler<WebsitesDiscoveredEventArgs>? WebsitesDiscovered;
        
        // New event for connection completion
        public event EventHandler<ConnectionCompletedEventArgs>? ConnectionCompleted;

        public bool IsConnected => sshClient?.IsConnected ?? false;

        public SshCommandHelper()
        {
            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.WorkerSupportsCancellation = true;
        }

        public void Connect(string hostname, string username, string password, int port = 22)
        {
            if (IsConnected)
            {
                Disconnect();
            }

            try
            {
                RaiseConnectionStatusChanged("Connecting...");
                sshClient = new SshClient(hostname, port, username, password);
                sshClient.Connect();
                RaiseConnectionStatusChanged($"Connected to {hostname}");
            }
            catch (Exception ex)
            {
                sshClient = null;
                RaiseConnectionStatusChanged($"Connection failed: {ex.Message}");
                throw;
            }
        }

        // New method for connecting in the background
        public void ConnectAsync(string hostname, string username, string password, int port = 22)
        {
            if (worker.IsBusy)
            {
                RaiseConnectionStatusChanged("Worker is busy, cannot start connection");
                return;
            }

            RaiseConnectionStatusChanged("Connecting...");

            // Create connection parameters
            var parameters = new ConnectionParameters
            {
                OperationType = OperationType.Connect,
                Hostname = hostname,
                Username = username,
                Password = password,
                Port = port
            };

            // Start the background worker
            worker.RunWorkerAsync(parameters);
        }

        public void Disconnect()
        {
            if (sshClient != null && sshClient.IsConnected)
            {
                RaiseConnectionStatusChanged("Disconnecting...");
                
                try
                {
                    sshClient.Disconnect();
                }
                catch (Exception ex)
                {
                    RaiseConnectionStatusChanged($"Disconnect error: {ex.Message}");
                }
                finally
                {
                    sshClient.Dispose();
                    sshClient = null;
                    RaiseConnectionStatusChanged("Disconnected");
                }
            }
        }

        // New method for disconnecting in the background
        public void DisconnectAsync()
        {
            if (!IsConnected)
            {
                RaiseConnectionStatusChanged("Not connected");
                return;
            }

            if (worker.IsBusy)
            {
                RaiseConnectionStatusChanged("Worker is busy, cannot start disconnection");
                return;
            }

            RaiseConnectionStatusChanged("Disconnecting...");

            // Create disconnection parameters
            var parameters = new ConnectionParameters
            {
                OperationType = OperationType.Disconnect
            };

            // Start the background worker
            worker.RunWorkerAsync(parameters);
        }

        public void ExecuteCommand(string command, string workingDirectory = null)
        {
            if (!IsConnected)
            {
                RaiseCommandCompleted(false, "Not connected to SSH server", null);
                return;
            }

            if (worker.IsBusy)
            {
                RaiseCommandCompleted(false, "A command is already running", null);
                return;
            }

            // Create command parameters
            var parameters = new CommandParameters
            {
                Command = command,
                WorkingDirectory = workingDirectory
            };

            // Start the background worker
            worker.RunWorkerAsync(parameters);
        }

        // New method to discover websites from systemd service files
        public void DiscoverWebsites()
        {
            if (!IsConnected)
            {
                RaiseWebsitesDiscovered(false, "Not connected to SSH server", null);
                return;
            }

            if (worker.IsBusy)
            {
                RaiseWebsitesDiscovered(false, "A command is already running", null);
                return;
            }

            // Start the background worker for website discovery
            worker.RunWorkerAsync(new CommandParameters { Command = "discover_websites" });
        }

        private void Worker_DoWork(object? sender, DoWorkEventArgs e)
        {
            // Check if this is a connection operation
            if (e.Argument is ConnectionParameters connectionParams)
            {
                e.Result = ProcessConnectionOperation(connectionParams);
                return;
            }
            
            if (e.Argument is not CommandParameters parameters || sshClient == null)
            {
                e.Result = new CommandResult { Success = false, Error = "Invalid parameters or SSH client" };
                return;
            }

            // Check if this is a website discovery operation
            if (parameters.Command == "discover_websites")
            {
                e.Result = DiscoverWebsitesWorker();
                return;
            }

            try
            {
                string commandText = parameters.Command;
                
                // Add cd command if working directory is specified
                if (!string.IsNullOrEmpty(parameters.WorkingDirectory))
                {
                    // First check if directory exists
                    using var dirCheckCmd = sshClient.CreateCommand($"cd {parameters.WorkingDirectory} && pwd");
                    string dirCheckResult = dirCheckCmd.Execute();
                    
                    if (string.IsNullOrEmpty(dirCheckResult) || dirCheckCmd.ExitStatus != 0)
                    {
                        e.Result = new CommandResult 
                        { 
                            Success = false, 
                            Error = $"Directory not found: {parameters.WorkingDirectory}" 
                        };
                        return;
                    }
                    
                    // Prefix the command with cd
                    commandText = $"cd {parameters.WorkingDirectory} && {commandText}";
                }

                // Execute the command
                using var cmd = sshClient.CreateCommand(commandText);
                string result = cmd.Execute();
                
                // Check exit status
                if (cmd.ExitStatus != 0)
                {
                    e.Result = new CommandResult 
                    { 
                        Success = false, 
                        Error = $"Command failed with exit code: {cmd.ExitStatus}", 
                        Output = result 
                    };
                }
                else
                {
                    e.Result = new CommandResult 
                    { 
                        Success = true, 
                        Output = result 
                    };
                }
            }
            catch (Exception ex)
            {
                e.Result = new CommandResult 
                { 
                    Success = false, 
                    Error = ex.Message 
                };
            }
        }

        private object ProcessConnectionOperation(ConnectionParameters connectionParams)
        {
            if (connectionParams.OperationType == OperationType.Connect)
            {
                try
                {
                    // Create and connect the SSH client
                    if (IsConnected)
                    {
                        // Properly disconnect first if there's an existing connection
                        try
                        {
                            sshClient?.Disconnect();
                            sshClient?.Dispose();
                        }
                        catch { /* Ignore any errors during cleanup */ }
                    }

                    sshClient = new SshClient(
                        connectionParams.Hostname,
                        connectionParams.Port,
                        connectionParams.Username,
                        connectionParams.Password
                    );

                    sshClient.Connect();

                    return new ConnectionResult
                    {
                        Success = true,
                        Hostname = connectionParams.Hostname
                    };
                }
                catch (Exception ex)
                {
                    if (sshClient != null)
                    {
                        sshClient.Dispose();
                        sshClient = null;
                    }

                    return new ConnectionResult
                    {
                        Success = false,
                        Error = ex.Message
                    };
                }
            }
            else if (connectionParams.OperationType == OperationType.Disconnect)
            {
                try
                {
                    if (sshClient != null && sshClient.IsConnected)
                    {
                        sshClient.Disconnect();
                    }

                    return new ConnectionResult { Success = true };
                }
                catch (Exception ex)
                {
                    return new ConnectionResult
                    {
                        Success = false,
                        Error = ex.Message
                    };
                }
                finally
                {
                    if (sshClient != null)
                    {
                        sshClient.Dispose();
                        sshClient = null;
                    }
                }
            }

            return new ConnectionResult
            {
                Success = false,
                Error = "Unknown operation type"
            };
        }

        private CommandResult DiscoverWebsitesWorker()
        {
            if (sshClient == null || !sshClient.IsConnected)
            {
                return new CommandResult { Success = false, Error = "SSH client is not connected" };
            }

            try
            {
                List<WebsiteInfo> websites = new List<WebsiteInfo>();
                string systemdDir = "/etc/systemd/system/";

                // First, check if the directory exists
                using (var dirCheckCmd = sshClient.CreateCommand($"cd {systemdDir} && pwd"))
                {
                    string dirCheckResult = dirCheckCmd.Execute();
                    if (string.IsNullOrEmpty(dirCheckResult) || dirCheckCmd.ExitStatus != 0)
                    {
                        return new CommandResult
                        {
                            Success = false,
                            Error = $"Directory not found: {systemdDir}"
                        };
                    }
                }

                // Get all .service files in the directory (non-recursive)
                using (var findCmd = sshClient.CreateCommand($"find {systemdDir} -maxdepth 1 -type f -name '*.service'"))
                {
                    string findResult = findCmd.Execute();

                    if (findCmd.ExitStatus != 0)
                    {
                        return new CommandResult
                        {
                            Success = false,
                            Error = $"Failed to list files in {systemdDir}: {findResult}"
                        };
                    }

                    // Get list of files
                    string[] fileList = findResult.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                    // Process each file
                    foreach (string filePath in fileList)
                    {
                        var fileName = Path.GetFileName(filePath);

                        // Check if the filename contains "aspforenhance"
                        if (fileName.Contains("aspforenhance", StringComparison.OrdinalIgnoreCase))
                        {
                            // Read the file contents
                            using (var catCmd = sshClient.CreateCommand($"cat {filePath}"))
                            {
                                string fileContent = catCmd.Execute();

                                if (catCmd.ExitStatus != 0)
                                {
                                    continue; // Skip if can't read file
                                }

                                // Parse website info from file content
                                WebsiteInfo? website = ExtractWebsiteInfo(fileContent, filePath);
                                if (website != null)
                                {
                                    websites.Add(website);
                                }
                            }
                        }
                    }
                }

                // Return the result with websites list
                return new WebsiteDiscoveryResult
                {
                    Success = true,
                    Websites = websites
                };
            }
            catch (Exception ex)
            {
                return new CommandResult
                {
                    Success = false,
                    Error = $"Website discovery failed: {ex.Message}"
                };
            }
        }

        private WebsiteInfo? ExtractWebsiteInfo(string fileContent, string filePath)
        {
            // Define regex patterns to extract metadata
            // Pattern to match: # [ASPForEnhance Name = "websitename.com"]
            string namePattern = @"# \[ASPForEnhance Name=""([^""]+)""\]";
            string idPattern = @"# \[ASPForEnhance Id=""([^""]+)""\]";
            string ipPattern = @"# \[ASPForEnhance Ip=""([^""]+)""\]";
            string portPattern = @"# \[ASPForEnhance Port=""([^""]+)""\]";

            // Extract name (required)
            Match nameMatch = Regex.Match(fileContent, namePattern);
            if (!nameMatch.Success)
            {
                return null; // Name is mandatory, skip if not found
            }

            WebsiteInfo website = new WebsiteInfo
            {
                Name = nameMatch.Groups[1].Value,
                FileName = Path.GetFileName(filePath),
            };

            // Extract optional fields if present
            Match idMatch = Regex.Match(fileContent, idPattern);
            if (idMatch.Success)
            {
                website.Id = idMatch.Groups[1].Value;
            }

            Match ipMatch = Regex.Match(fileContent, ipPattern);
            if (ipMatch.Success)
            {
                website.Ip = ipMatch.Groups[1].Value;
            }

            Match portMatch = Regex.Match(fileContent, portPattern);
            if (portMatch.Success)
            {
                website.Port = portMatch.Groups[1].Value;
            }

            return website;
        }

        private void Worker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                RaiseCommandCompleted(false, e.Error.Message, null);
                return;
            }
            
            if (e.Cancelled)
            {
                RaiseCommandCompleted(false, "Command was cancelled", null);
                return;
            }
            
            // Handle connection result
            if (e.Result is ConnectionResult connectionResult)
            {
                RaiseConnectionCompleted(
                    connectionResult.Success,
                    connectionResult.Success ? connectionResult.Hostname : null,
                    connectionResult.Error);
                
                // Also update connection status
                if (connectionResult.Success)
                {
                    RaiseConnectionStatusChanged($"Connected to {connectionResult.Hostname}");
                }
                else
                {
                    RaiseConnectionStatusChanged($"Connection failed: {connectionResult.Error}");
                }
                
                return;
            }
            
            // Handle website discovery result
            if (e.Result is WebsiteDiscoveryResult websiteResult)
            {
                RaiseWebsitesDiscovered(true, null, websiteResult.Websites);
                return;
            }
            
            // Handle regular command result
            if (e.Result is CommandResult result)
            {
                RaiseCommandCompleted(result.Success, result.Error, result.Output);
            }
        }

        private void RaiseCommandCompleted(bool success, string? error, string? output)
        {
            CommandCompleted?.Invoke(this, new CommandCompletedEventArgs(success, error, output));
        }

        private void RaiseConnectionStatusChanged(string status)
        {
            ConnectionStatusChanged?.Invoke(this, new ConnectionStatusEventArgs(status));
        }

        private void RaiseWebsitesDiscovered(bool success, string? error, List<WebsiteInfo>? websites)
        {
            WebsitesDiscovered?.Invoke(this, new WebsitesDiscoveredEventArgs(success, error, websites));
        }

        private void RaiseConnectionCompleted(bool success, string? hostname, string? error)
        {
            ConnectionCompleted?.Invoke(this, new ConnectionCompletedEventArgs(success, hostname, error));
        }

        // Helper classes for parameters and results
        private class CommandParameters
        {
            public string Command { get; set; } = string.Empty;
            public string? WorkingDirectory { get; set; }
        }

        private class ConnectionParameters
        {
            public OperationType OperationType { get; set; }
            public string Hostname { get; set; } = string.Empty;
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public int Port { get; set; } = 22;
        }

        private enum OperationType
        {
            Connect,
            Disconnect
        }

        private class CommandResult
        {
            public bool Success { get; set; }
            public string? Error { get; set; }
            public string? Output { get; set; }
        }

        // Result class for connection operations
        private class ConnectionResult
        {
            public bool Success { get; set; }
            public string? Hostname { get; set; }
            public string? Error { get; set; }
        }

        // Additional result class for website discovery
        private class WebsiteDiscoveryResult : CommandResult
        {
            public List<WebsiteInfo>? Websites { get; set; }
        }
    }

    // Event args classes
    public class CommandCompletedEventArgs : EventArgs
    {
        public bool Success { get; }
        public string? Error { get; }
        public string? Output { get; }
        
        public CommandCompletedEventArgs(bool success, string? error, string? output)
        {
            Success = success;
            Error = error;
            Output = output;
        }
    }

    public class ConnectionStatusEventArgs : EventArgs
    {
        public string Status { get; }
        
        public ConnectionStatusEventArgs(string status)
        {
            Status = status;
        }
    }

    // Event args class for website discovery completion
    public class WebsitesDiscoveredEventArgs : EventArgs
    {
        public bool Success { get; }
        public string? Error { get; }
        public List<WebsiteInfo>? Websites { get; }
        
        public WebsitesDiscoveredEventArgs(bool success, string? error, List<WebsiteInfo>? websites)
        {
            Success = success;
            Error = error;
            Websites = websites;
        }
    }

    // New event args class for connection completion
    public class ConnectionCompletedEventArgs : EventArgs
    {
        public bool Success { get; }
        public string? Hostname { get; }
        public string? Error { get; }
        
        public ConnectionCompletedEventArgs(bool success, string? hostname, string? error)
        {
            Success = success;
            Hostname = hostname;
            Error = error;
        }
    }
}
