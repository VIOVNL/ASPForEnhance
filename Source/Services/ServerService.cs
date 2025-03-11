using ASPForEnhance.SshHelper;

namespace ASPForEnhance.Services
{
    public class ServerService
    {
        private readonly ServerManager _serverManager;
        private readonly SshCommandHelper _sshClient;
        public  string SshPassword { get; set; }
        public string SshIp { get; set; }

        public List<ServerInfo> Servers => _serverManager.Servers;

        public ServerService()
        {
            _serverManager = new ServerManager();
            _sshClient = new SshCommandHelper();
        }

        public void LoadServers()
        {
            _serverManager.LoadServers();
        }

        public void AddServer(ServerInfo server)
        {
            _serverManager.AddServer(server);
        }

        public void UpdateServer(int index, ServerInfo server)
        {
            _serverManager.UpdateServer(index, server);
        }

        public void DeleteServer(int index)
        {
            _serverManager.DeleteServer(index);
        }

        public async Task<ConnectionResult> ConnectAsync(ServerInfo server)
        {
            SshPassword = server.Password;
            SshIp = server.Hostname;
            return await _sshClient.ConnectAsync(server.Hostname, server.Username, server.Password, server.Port);
        }

        public async Task<ConnectionResult> DisconnectAsync()
        {
            return await _sshClient.DisconnectAsync();
        }

        public async Task<WebsiteDiscoveryResult> GetWebsitesAsync()
        {
            return await _sshClient.DiscoverWebsitesAsync();
        }

        public async Task<CommandResult> ExecuteCommandAsync(string command, string? workingDirectory = null)
        {
            return await _sshClient.ExecuteCommandAsync(command, workingDirectory);
        }

        public async Task<ServiceStatusResult> GetServiceStatusAsync(string serviceName)
        {
            return await _sshClient.GetServiceStatusAsync(serviceName);
        }

        public async Task<ServiceLogResult> GetServiceLogsAsync(string serviceName, int lines = 100)
        {
            return await _sshClient.GetServiceLogsAsync(serviceName, lines);
        }

        public async Task<ServiceOperationResult> RestartServiceAsync(string serviceName)
        {
            return await _sshClient.RestartServiceAsync(serviceName);
        }

        public async Task<ServiceOperationResult> StopServiceAsync(string serviceName)
        {
            return await _sshClient.StopServiceAsync(serviceName);
        }

        public async Task<ServiceOperationResult> DisableServiceAsync(string serviceName)
        {
            return await _sshClient.DisableServiceAsync(serviceName);
        }

        public async Task<ServiceOperationResult> EnableServiceAsync(string serviceName)
        {
            return await _sshClient.EnableServiceAsync(serviceName);
        }

        public async Task<ServiceOperationResult> StartServiceAsync(string serviceName)
        {
            return await _sshClient.StartServiceAsync(serviceName);
        }

        public async Task<WebsiteCreationResult> CreateWebsiteAsync(WebsiteInfo websiteInfo, string dllFileName, string folderPath, bool enableBlazorSignalR, string password)
        {
            return await _sshClient.CreateWebsiteAsync(websiteInfo, dllFileName, folderPath, enableBlazorSignalR, password);
        }
    }
}
