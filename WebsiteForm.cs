using System.ComponentModel;
using System.Text.RegularExpressions;

namespace ASPForEnhance
{
    public partial class WebsiteForm : Form
    {
        public WebsiteInfo WebsiteInfo { get; private set; }
        public string DllFileName { get; private set; }
        public string FolderPath { get; private set; }
        public bool EnableBlazorSignalR { get; private set; }
        private string? ServerIp { get; set; }
        private int NextPort { get; set; }

        public WebsiteForm(string serverIp, int nextPort)
        {
            InitializeComponent();
            ServerIp = serverIp;
            NextPort = nextPort;
            WebsiteInfo = new WebsiteInfo();
            DllFileName = string.Empty;
            FolderPath = "";
            EnableBlazorSignalR = false;
            Text = "Add Website";

            // Set default port in the info label
            UpdatePortInfoLabel();
        }

        private void UpdatePortInfoLabel()
        {
            portInfoLabel.Text = $"Port: {NextPort} (Auto-assigned)";
        }
        private bool ParseFullPath(string fullPath)
        {
            var regex = new Regex(@"^/var/www/(?<id>[a-f0-9\-]+)/(?<folderPath>[^/]+)/(?<dllName>[^/]+)$");
            var match = regex.Match(fullPath);

            if (match.Success)
            {
                WebsiteInfo.Id = match.Groups["id"].Value;
                FolderPath = match.Groups["folderPath"].Value;
                DllFileName = match.Groups["dllName"].Value;
                return true;
            }
            else
            {
                MessageBox.Show("Invalid full path format.", "Parsing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private void SaveButton_Click(object sender, EventArgs e)
        {

            // Validate input
            if (string.IsNullOrWhiteSpace(nameTextBox.Text) ||
                string.IsNullOrWhiteSpace(fullPathTextBox.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ParseFullPath(fullPathTextBox.Text.Trim()) == false)
            {
                return;
            };
            // Update website info with form values
            WebsiteInfo.Name = nameTextBox.Text.Trim();
            WebsiteInfo.Ip = ServerIp ?? string.Empty;
            WebsiteInfo.Port = NextPort.ToString();
            WebsiteInfo.FileName = $"aspforenhance-{WebsiteInfo.Name.ToLower().Replace('.', '-')}.service";
            EnableBlazorSignalR = blazorSignalRCheckBox.Checked;

            DialogResult = DialogResult.OK;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
