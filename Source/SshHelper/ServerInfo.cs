namespace ASPForEnhance.SshHelper
{
    public class ServerInfo
    {
        public string Name { get; set; } = "";
        public string Hostname { get; set; } = "";
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public int Port { get; set; } = 22;

        public override string ToString()
        {
            return Name;
        }
    }
}
