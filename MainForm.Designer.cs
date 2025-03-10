namespace ASPForEnhance
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            serverIpTextBox = new TextBox();
            usernameTextBox = new TextBox();
            passwordTextBox = new TextBox();
            LoginButton = new Button();
            label1 = new Label();
            serversComboBox = new ComboBox();
            addServerButton = new Button();
            editServerButton = new Button();
            deleteServerButton = new Button();
            label2 = new Label();
            statusStrip = new StatusStrip();
            statusLabel = new ToolStripStatusLabel();
            websitesDataGridView = new DataGridView();
            addWebsiteButton = new Button();
            statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)websitesDataGridView).BeginInit();
            SuspendLayout();
            // 
            // serverIpTextBox
            // 
            serverIpTextBox.Location = new Point(22, 62);
            serverIpTextBox.Name = "serverIpTextBox";
            serverIpTextBox.PlaceholderText = "Server IP";
            serverIpTextBox.Size = new Size(262, 31);
            serverIpTextBox.TabIndex = 1;
            // 
            // usernameTextBox
            // 
            usernameTextBox.Location = new Point(290, 62);
            usernameTextBox.Name = "usernameTextBox";
            usernameTextBox.PlaceholderText = "Server Username";
            usernameTextBox.Size = new Size(262, 31);
            usernameTextBox.TabIndex = 2;
            // 
            // passwordTextBox
            // 
            passwordTextBox.Location = new Point(558, 62);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.PasswordChar = '*';
            passwordTextBox.PlaceholderText = "Server Password";
            passwordTextBox.Size = new Size(262, 31);
            passwordTextBox.TabIndex = 3;
            // 
            // LoginButton
            // 
            LoginButton.Location = new Point(826, 62);
            LoginButton.Name = "LoginButton";
            LoginButton.Size = new Size(112, 34);
            LoginButton.TabIndex = 4;
            LoginButton.Text = "Login";
            LoginButton.UseVisualStyleBackColor = true;
            LoginButton.Click += LoginButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(22, 116);
            label1.Name = "label1";
            label1.Size = new Size(83, 25);
            label1.TabIndex = 5;
            label1.Text = "Websites";
            // 
            // serversComboBox
            // 
            serversComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            serversComboBox.FormattingEnabled = true;
            serversComboBox.Location = new Point(22, 22);
            serversComboBox.Name = "serversComboBox";
            serversComboBox.Size = new Size(400, 33);
            serversComboBox.TabIndex = 0;
            serversComboBox.SelectedIndexChanged += ServersComboBox_SelectedIndexChanged;
            // 
            // addServerButton
            // 
            addServerButton.Location = new Point(428, 22);
            addServerButton.Name = "addServerButton";
            addServerButton.Size = new Size(112, 34);
            addServerButton.TabIndex = 7;
            addServerButton.Text = "Add";
            addServerButton.UseVisualStyleBackColor = true;
            addServerButton.Click += AddServerButton_Click;
            // 
            // editServerButton
            // 
            editServerButton.Location = new Point(546, 22);
            editServerButton.Name = "editServerButton";
            editServerButton.Size = new Size(112, 34);
            editServerButton.TabIndex = 8;
            editServerButton.Text = "Edit";
            editServerButton.UseVisualStyleBackColor = true;
            editServerButton.Click += EditServerButton_Click;
            // 
            // deleteServerButton
            // 
            deleteServerButton.Location = new Point(664, 22);
            deleteServerButton.Name = "deleteServerButton";
            deleteServerButton.Size = new Size(112, 34);
            deleteServerButton.TabIndex = 9;
            deleteServerButton.Text = "Delete";
            deleteServerButton.UseVisualStyleBackColor = true;
            deleteServerButton.Click += DeleteServerButton_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(782, 26);
            label2.Name = "label2";
            label2.Size = new Size(122, 25);
            label2.TabIndex = 10;
            label2.Text = "Saved Servers";
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(24, 24);
            statusStrip.Items.AddRange(new ToolStripItem[] { statusLabel });
            statusStrip.Location = new Point(0, 790);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(954, 32);
            statusStrip.TabIndex = 11;
            statusStrip.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(72, 25);
            statusLabel.Text = "Ready...";
            // 
            // websitesDataGridView
            // 
            websitesDataGridView.AllowUserToAddRows = false;
            websitesDataGridView.AllowUserToDeleteRows = false;
            websitesDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            websitesDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            websitesDataGridView.Location = new Point(22, 153);
            websitesDataGridView.Name = "websitesDataGridView";
            websitesDataGridView.ReadOnly = true;
            websitesDataGridView.RowHeadersWidth = 62;
            websitesDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            websitesDataGridView.Size = new Size(916, 609);
            websitesDataGridView.TabIndex = 13;
            // 
            // addWebsiteButton
            // 
            addWebsiteButton.Location = new Point(111, 112);
            addWebsiteButton.Name = "addWebsiteButton";
            addWebsiteButton.Size = new Size(112, 34);
            addWebsiteButton.TabIndex = 14;
            addWebsiteButton.Text = "Add";
            addWebsiteButton.UseVisualStyleBackColor = true;
            addWebsiteButton.Click += AddWebsiteButton_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(954, 822);
            Controls.Add(addWebsiteButton);
            Controls.Add(websitesDataGridView);
            Controls.Add(statusStrip);
            Controls.Add(label2);
            Controls.Add(deleteServerButton);
            Controls.Add(editServerButton);
            Controls.Add(addServerButton);
            Controls.Add(serversComboBox);
            Controls.Add(label1);
            Controls.Add(LoginButton);
            Controls.Add(passwordTextBox);
            Controls.Add(usernameTextBox);
            Controls.Add(serverIpTextBox);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainForm";
            Text = "ASP For Enhance.com";
            Load += MainForm_Load;
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)websitesDataGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox serverIpTextBox;
        private TextBox usernameTextBox;
        private TextBox passwordTextBox;
        private Button LoginButton;
        private Label label1;
        private ComboBox serversComboBox;
        private Button addServerButton;
        private Button editServerButton;
        private Button deleteServerButton;
        private Label label2;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;
        private DataGridView websitesDataGridView;
        private Button addWebsiteButton;
    }
}
