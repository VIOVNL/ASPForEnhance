using System.ComponentModel;

namespace ASPForEnhance
{
    public partial class WebsiteForm : Form
    {
        public WebsiteInfo WebsiteInfo { get; private set; }
        public string AspDllPath { get; private set; }
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
            AspDllPath = string.Empty;
            FolderPath = "api";
            EnableBlazorSignalR = false;
            Text = "Add Website";

            // Set default port in the info label
            UpdatePortInfoLabel();
        }

        private void UpdatePortInfoLabel()
        {
            portInfoLabel.Text = $"Port: {NextPort} (Auto-assigned)";
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(nameTextBox.Text) ||
                string.IsNullOrWhiteSpace(idTextBox.Text) ||
                string.IsNullOrWhiteSpace(aspDllPathTextBox.Text) ||
                string.IsNullOrWhiteSpace(folderPathTextBox.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Update website info with form values
            WebsiteInfo.Name = nameTextBox.Text.Trim();
            WebsiteInfo.Id = idTextBox.Text.Trim();
            WebsiteInfo.Ip = ServerIp ?? string.Empty;
            WebsiteInfo.Port = NextPort.ToString();
            WebsiteInfo.FileName = $"aspforenhance-{WebsiteInfo.Name.ToLower().Replace('.', '-')}.service";
            AspDllPath = aspDllPathTextBox.Text.Trim();
            FolderPath = folderPathTextBox.Text.Trim();
            EnableBlazorSignalR = blazorSignalRCheckBox.Checked;

            DialogResult = DialogResult.OK;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void GenerateIdButton_Click(object sender, EventArgs e)
        {
            // Generate a new GUID for the website ID
            idTextBox.Text = Guid.NewGuid().ToString();
        }
    }
}
