using System.ComponentModel;

namespace ASPForEnhance
{
    public partial class ServerForm : Form
    {
        public ServerInfo ServerInfo { get; private set; }

        public ServerForm(ServerInfo? serverInfo = null)
        {
            InitializeComponent();
            
            if (serverInfo != null)
            {
                ServerInfo = new ServerInfo
                {
                    Name = serverInfo.Name,
                    Hostname = serverInfo.Hostname,
                    Username = serverInfo.Username,
                    Password = serverInfo.Password,
                    Port = serverInfo.Port
                };
                Text = "Edit Server";
            }
            else
            {
                ServerInfo = new ServerInfo();
                Text = "Add Server";
            }

            // Load data into form
            nameTextBox.Text = ServerInfo.Name;
            hostnameTextBox.Text = ServerInfo.Hostname;
            usernameTextBox.Text = ServerInfo.Username;
            passwordTextBox.Text = ServerInfo.Password;
            portNumericUpDown.Value = ServerInfo.Port;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(nameTextBox.Text) || 
                string.IsNullOrWhiteSpace(hostnameTextBox.Text) ||
                string.IsNullOrWhiteSpace(usernameTextBox.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Update server info with form values
            ServerInfo.Name = nameTextBox.Text.Trim();
            ServerInfo.Hostname = hostnameTextBox.Text.Trim();
            ServerInfo.Username = usernameTextBox.Text.Trim();
            ServerInfo.Password = passwordTextBox.Text;
            ServerInfo.Port = (int)portNumericUpDown.Value;

            DialogResult = DialogResult.OK;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
