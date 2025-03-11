using System.Text.RegularExpressions;
using Renci.SshNet;

namespace ASPForEnhance.SshHelper
{
    public class SshCommandHelper
    {
        private SshClient? sshClient;

        public bool IsConnected => sshClient?.IsConnected ?? false;

        public SshCommandHelper() { }

        // Async method for connecting
        public async Task<ConnectionResult> ConnectAsync(string hostname, string username, string password, int port = 22)
        {
            return await Task.Run(() => 
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

                    sshClient = new SshClient(hostname, port, username, password);
                    sshClient.Connect();

                    return new ConnectionResult
                    {
                        Success = true,
                        Hostname = hostname
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
            });
        }

        // Async method for disconnecting
        public async Task<ConnectionResult> DisconnectAsync()
        {
            return await Task.Run(() =>
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
            });
        }

        // Async method to execute SSH command
        public async Task<CommandResult> ExecuteCommandAsync(string command, string? workingDirectory = null)
        {
            return await Task.Run(() =>
            {
                if (sshClient == null || !sshClient.IsConnected)
                {
                    return new CommandResult { Success = false, Error = "Not connected to SSH server" };
                }

                try
                {
                    string commandText = command;
                    
                    // Add cd command if working directory is specified
                    if (!string.IsNullOrEmpty(workingDirectory))
                    {
                        // First check if directory exists
                        using var dirCheckCmd = sshClient.CreateCommand($"cd {workingDirectory} && pwd");
                        string dirCheckResult = dirCheckCmd.Execute();
                        
                        if (string.IsNullOrEmpty(dirCheckResult) || dirCheckCmd.ExitStatus != 0)
                        {
                            return new CommandResult 
                            { 
                                Success = false, 
                                Error = $"Directory not found: {workingDirectory}" 
                            };
                        }
                        
                        // Prefix the command with cd
                        commandText = $"cd {workingDirectory} && {commandText}";
                    }

                    // Execute the command
                    using var cmd = sshClient.CreateCommand(commandText);
                    string result = cmd.Execute();
                    
                    // Check exit status
                    if (cmd.ExitStatus != 0)
                    {
                        return new CommandResult 
                        { 
                            Success = false, 
                            Error = $"Command failed with exit code: {cmd.ExitStatus}", 
                            Output = result 
                        };
                    }
                    else
                    {
                        return new CommandResult 
                        { 
                            Success = true, 
                            Output = result 
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new CommandResult 
                    { 
                        Success = false, 
                        Error = ex.Message 
                    };
                }
            });
        }

        // Async method to discover websites
        public async Task<WebsiteDiscoveryResult> DiscoverWebsitesAsync()
        {
            return await Task.Run(() =>
            {
                if (sshClient == null || !sshClient.IsConnected)
                {
                    return new WebsiteDiscoveryResult { Success = false, Error = "SSH client is not connected" };
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
                            return new WebsiteDiscoveryResult
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
                            return new WebsiteDiscoveryResult
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
                    return new WebsiteDiscoveryResult
                    {
                        Success = false,
                        Error = $"Website discovery failed: {ex.Message}"
                    };
                }
            });
        }

        // Async method for creating websites
        public async Task<WebsiteCreationResult> CreateWebsiteAsync(WebsiteInfo websiteInfo, string dllFileName, string folderPath, bool enableBlazorSignalR, string password)
        {
            return await Task.Run(() =>
            {
                if (sshClient == null || !sshClient.IsConnected)
                {
                    return new WebsiteCreationResult { Success = false, Error = "SSH client is not connected" };
                }

                try
                {
                    // Format the service file content based on the template
                    string serviceFileContent = GenerateServiceFileContent(websiteInfo, dllFileName, folderPath);
                    
                    // Get the service file name
                    string serviceFileName = websiteInfo.FileName;
                    
                    // Create a temporary file locally
                    string tempFilePath = Path.Combine(Path.GetTempPath(), serviceFileName);
                    File.WriteAllText(tempFilePath, serviceFileContent);
                    
                    // Upload the file to the server using SCP
                    using (var scpClient = new ScpClient(
                        sshClient.ConnectionInfo.Host,
                        sshClient.ConnectionInfo.Username,
                        password))
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
                        
                        // Check if vhost directory exists, create if needed
                        using (var cmd = sshClient.CreateCommand("sudo mkdir -p /var/local/enhance/apache/vhost_includes"))
                        {
                            cmd.Execute();
                        }
                        
                        // Upload Apache config file
                        using (var apacheConfigStream = new FileStream(tempApacheConfigPath, FileMode.Open))
                        {
                            string remoteApacheConfigPath = $"/var/local/enhance/apache/vhost_includes/{apacheConfigFileName}";
                            scpClient.Upload(apacheConfigStream, remoteApacheConfigPath);
                        }
                        
                        // Delete the temporary Apache config file
                        File.Delete(tempApacheConfigPath);
                        
                        scpClient.Disconnect();
                    }
                    
                    // Delete the temporary service file
                    File.Delete(tempFilePath);
                    
                    // Reload systemd daemon and enable the service
                    using (var cmd = sshClient.CreateCommand($"sudo systemctl daemon-reload && sudo systemctl enable {serviceFileName}"))
                    {
                        cmd.Execute();
                    }

                    // Add ssh ip to firewall allow all
                    using (var cmd = sshClient.CreateCommand($"sudo ufw allow from {sshClient.ConnectionInfo.Host}"))
                    {
                        cmd.Execute();
                    }

                    // Restart Apache to apply the vhost configuration
                    using (var cmd = sshClient.CreateCommand("sudo service apache2 restart"))
                    {
                        cmd.Execute();
                    }

                    // Start service now
                    using (var cmd = sshClient.CreateCommand($"sudo systemctl start {serviceFileName}"))
                    {
                        cmd.Execute();
                    }

                    // Return the result with the created website
                    return new WebsiteCreationResult
                    {
                        Success = true,
                        Website = websiteInfo
                    };
                }
                catch (Exception ex)
                {
                    return new WebsiteCreationResult
                    {
                        Success = false,
                        Error = $"Website creation failed: {ex.Message}"
                    };
                }
            });
        }

        // Async method to get service status
        public async Task<ServiceStatusResult> GetServiceStatusAsync(string serviceName)
        {
            return await Task.Run(() =>
            {
                if (sshClient == null || !sshClient.IsConnected)
                {
                    return new ServiceStatusResult { Success = false, Error = "SSH client is not connected" };
                }

                try
                {
                    string command = $"sudo systemctl status {serviceName}";
                    using (var cmd = sshClient.CreateCommand(command))
                    {
                        string output = cmd.Execute();
                        
                        // Parse the output to create a ServiceStatus object
                        ServiceStatus status = ParseServiceStatusOutput(output, serviceName);
                        
                        return new ServiceStatusResult
                        {
                            Success = true,
                            Status = status
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new ServiceStatusResult
                    {
                        Success = false,
                        Error = ex.Message
                    };
                }
            });
        }

        // Async method to restart a service
        public async Task<ServiceOperationResult> RestartServiceAsync(string serviceName)
        {
            return await ExecuteServiceOperationAsync(serviceName, ServiceOperation.Restart);
        }

        // Async method to stop a service
        public async Task<ServiceOperationResult> StopServiceAsync(string serviceName)
        {
            return await ExecuteServiceOperationAsync(serviceName, ServiceOperation.Stop);
        }

        // Async method to disable a service
        public async Task<ServiceOperationResult> DisableServiceAsync(string serviceName)
        {
            return await ExecuteServiceOperationAsync(serviceName, ServiceOperation.Disable);
        }

        // Async method to enable a service
        public async Task<ServiceOperationResult> EnableServiceAsync(string serviceName)
        {
            return await ExecuteServiceOperationAsync(serviceName, ServiceOperation.Enable);
        }

        // Async method to start a service
        public async Task<ServiceOperationResult> StartServiceAsync(string serviceName)
        {
            return await ExecuteServiceOperationAsync(serviceName, ServiceOperation.Start);
        }

        // Helper method for service operations
        private async Task<ServiceOperationResult> ExecuteServiceOperationAsync(string serviceName, ServiceOperation operation)
        {
            return await Task.Run(() =>
            {
                if (sshClient == null || !sshClient.IsConnected)
                {
                    return new ServiceOperationResult 
                    { 
                        Success = false, 
                        Error = "SSH client is not connected",
                        Operation = operation
                    };
                }

                try
                {
                    string command;
                    
                    // Build the appropriate command based on operation
                    switch (operation)
                    {
                        case ServiceOperation.Restart:
                            command = $"sudo systemctl restart {serviceName}";
                            break;
                        
                        case ServiceOperation.Stop:
                            command = $"sudo systemctl stop {serviceName}";
                            break;
                        
                        case ServiceOperation.Disable:
                            command = $"sudo systemctl disable {serviceName}";
                            break;
                        
                        case ServiceOperation.Enable:
                            command = $"sudo systemctl enable {serviceName}";
                            break;
                        
                        case ServiceOperation.Start:
                            command = $"sudo systemctl start {serviceName}";
                            break;
                        
                        default:
                            return new ServiceOperationResult
                            {
                                Success = false,
                                Error = "Unknown service operation",
                                Operation = operation
                            };
                    }
                    
                    // Execute the command
                    using (var cmd = sshClient.CreateCommand(command))
                    {
                        string output = cmd.Execute();
                        
                        bool success = cmd.ExitStatus == 0;
                        string? error = success ? null : $"Command failed with exit code: {cmd.ExitStatus}. Output: {output}";
                        
                        return new ServiceOperationResult
                        {
                            Success = success,
                            Error = error,
                            Output = output,
                            Operation = operation
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new ServiceOperationResult
                    {
                        Success = false,
                        Error = ex.Message,
                        Operation = operation
                    };
                }
            });
        }

        // Async method to get service logs
        public async Task<ServiceLogResult> GetServiceLogsAsync(string serviceName, int lines = 100)
        {
            return await Task.Run(() =>
            {
                if (sshClient == null || !sshClient.IsConnected)
                {
                    return new ServiceLogResult { Success = false, Error = "SSH client is not connected" };
                }

                try
                {
                    // Build the command to get logs with journalctl
                    string command = $"sudo journalctl -u {serviceName} -n {lines} --no-pager";
                    
                    using (var cmd = sshClient.CreateCommand(command))
                    {
                        string output = cmd.Execute();
                        
                        bool success = cmd.ExitStatus == 0;
                        string? error = success ? null : $"Command failed with exit code: {cmd.ExitStatus}";
                        
                        return new ServiceLogResult
                        {
                            Success = success,
                            Error = error,
                            Logs = success ? output : null
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new ServiceLogResult
                    {
                        Success = false,
                        Error = ex.Message
                    };
                }
            });
        }

        // Helper methods that can remain largely the same
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

        // Parse service status output into a ServiceStatus object
        private ServiceStatus ParseServiceStatusOutput(string output, string serviceName)
        {
            ServiceStatus status = new ServiceStatus
            {
                ServiceName = serviceName,
                RawOutput = output
            };
            
            // Extract status (active, inactive, failed, etc.)
            Match activeMatch = Regex.Match(output, @"Active:\s+(\w+)");
            if (activeMatch.Success)
            {
                status.State = activeMatch.Groups[1].Value;
            }
            
            // Extract whether the service is enabled
            Match enabledMatch = Regex.Match(output, @"Loaded:.*?;\s+(\w+);");
            if (enabledMatch.Success)
            {
                status.IsEnabled = enabledMatch.Groups[1].Value == "enabled";
            }

            // Extract other useful information
            // Capture memory usage and peak memory usage
            Match memoryMatch = Regex.Match(output, @"Memory:\s+(\d+\.\d+\s*\w+)\s+\(peak:\s+(\d+\.\d+\s*\w+)\)");
            if (memoryMatch.Success)
            {
                status.MemoryUsage = memoryMatch.Groups[1].Value;
                status.PeakMemoryUsage = memoryMatch.Groups[2].Value;
            }
            // Capture CPU usage (e.g., "292ms")
            Match cpuMatch = Regex.Match(output, @"CPU:\s+(\d+\.?\d*\s*\w+)");
            if (cpuMatch.Success)
            {
                status.CpuUsage = cpuMatch.Groups[1].Value;
            }
            Match pidMatch = Regex.Match(output, @"Main PID:\s+(\d+)");
            if (pidMatch.Success)
            {
                if (int.TryParse(pidMatch.Groups[1].Value, out int pid))
                {
                    status.PID = pid;
                }
            }
            
            return status;
        }

        // Methods to generate service file and Apache config
        private string GenerateServiceFileContent(WebsiteInfo websiteInfo, string dllFileName, string folderPath)
        {
            // Extract the working directory (folder path) from the DLL path
            string workingDirectory = $"/var/www/{websiteInfo.Id}/{folderPath}";
            
            // Format the service file content based on the template
            return $"""
                    [Service]
                    WorkingDirectory={workingDirectory}
                    ExecStart=/usr/bin/dotnet {workingDirectory}/{dllFileName}
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
    }

    // Public result classes for return values
    public class CommandResult
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public string? Output { get; set; }
    }

    // Result class for connection operations
    public class ConnectionResult
    {
        public bool Success { get; set; }
        public string? Hostname { get; set; }
        public string? Error { get; set; }
    }

    // Additional result class for website discovery
    public class WebsiteDiscoveryResult : CommandResult
    {
        public List<WebsiteInfo>? Websites { get; set; }
    }

    // Result class for website creation
    public class WebsiteCreationResult : CommandResult
    {
        public WebsiteInfo? Website { get; set; }
    }

    // Service status result
    public class ServiceStatusResult
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public ServiceStatus? Status { get; set; }
    }

    // Service operation result
    public class ServiceOperationResult : CommandResult
    {
        public ServiceOperation Operation { get; set; }
    }

    // Service log result
    public class ServiceLogResult
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public string? Logs { get; set; }
    }

    // Service operation type enum
    public enum ServiceOperation
    {
        Unknown,
        Status,
        Restart,
        Stop,
        Disable,
        Enable,
        Start
    }

    // Service status class
    public class ServiceStatus
    {
        public string ServiceName { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public bool IsEnabled { get; set; }
        public int? PID { get; set; }
        public string? MemoryUsage { get; set; }
        public string RawOutput { get; set; } = string.Empty;
        public string CpuUsage { get; set; } = string.Empty;
        public string PeakMemoryUsage { get; set; } = string.Empty;
    }
}
