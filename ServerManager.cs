using System.Text.Json;

namespace ASPForEnhance
{
    public class ServerManager
    {
        private List<ServerInfo> _servers = new List<ServerInfo>();
        private readonly string _filePath = Path.Combine(Application.StartupPath, "servers.json");

        public List<ServerInfo> Servers => _servers;

        public ServerManager()
        {
            LoadServers();
        }

        public void LoadServers()
        {
            if (File.Exists(_filePath))
            {
                try
                {
                    string json = File.ReadAllText(_filePath);
                    _servers = JsonSerializer.Deserialize<List<ServerInfo>>(json) ?? new List<ServerInfo>();
                }
                catch (Exception)
                {
                    _servers = new List<ServerInfo>();
                }
            }
        }

        public void SaveServers()
        {
            string json = JsonSerializer.Serialize(_servers, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public void AddServer(ServerInfo server)
        {
            _servers.Add(server);
            SaveServers();
        }

        public void UpdateServer(int index, ServerInfo server)
        {
            if (index >= 0 && index < _servers.Count)
            {
                _servers[index] = server;
                SaveServers();
            }
        }

        public void DeleteServer(int index)
        {
            if (index >= 0 && index < _servers.Count)
            {
                _servers.RemoveAt(index);
                SaveServers();
            }
        }
    }
}
