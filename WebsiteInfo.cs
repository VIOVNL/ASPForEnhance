namespace ASPForEnhance
{
    public class WebsiteInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string Ip { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;

        public override string ToString()
        {
            return Name;
        }
    }
}
