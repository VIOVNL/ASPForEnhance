namespace ASPForEnhance
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            connectionPanel = new Panel();
            addWebsiteButton = new Button();
            LoginButton = new Button();
            deleteServerButton = new Button();
            editServerButton = new Button();
            addServerButton = new Button();
            passwordLabel = new Label();
            passwordTextBox = new TextBox();
            usernameLabel = new Label();
            usernameTextBox = new TextBox();
            serverIpLabel = new Label();
            serverIpTextBox = new TextBox();
            serversLabel = new Label();
            serversComboBox = new ComboBox();
            mainContentPanel = new Panel();
            tabControl = new TabControl();
            websitesTab = new TabPage();
            websitesDataGridView = new DataGridView();
            websiteContextMenu = new ContextMenuStrip(components);
            getStatusMenuItem = new ToolStripMenuItem();
            restartServiceMenuItem = new ToolStripMenuItem();
            stopServiceMenuItem = new ToolStripMenuItem();
            disableServiceMenuItem = new ToolStripMenuItem();
            viewLogsMenuItem = new ToolStripMenuItem();
            serviceTab = new TabPage();
            splitContainer = new SplitContainer();
            serviceControlPanel = new Panel();
            statusGroupBox = new GroupBox();
            statusLayout = new TableLayoutPanel();
            serviceNameLabelTitle = new Label();
            serviceNameLabel = new Label();
            serviceStatusLabelTitle = new Label();
            serviceStatusLabel = new Label();
            serviceActiveLabelTitle = new Label();
            serviceActiveLabel = new Label();
            lastOperationLabelTitle = new Label();
            lastOperationLabel = new Label();
            buttonPanel = new FlowLayoutPanel();
            getStatusButton = new Button();
            restartButton = new Button();
            stopButton = new Button();
            disableButton = new Button();
            viewLogsButton = new Button();
            serviceLogsBox = new RichTextBox();
            statusLabel = new Label();
            connectionPanel.SuspendLayout();
            mainContentPanel.SuspendLayout();
            tabControl.SuspendLayout();
            websitesTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)websitesDataGridView).BeginInit();
            websiteContextMenu.SuspendLayout();
            serviceTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            serviceControlPanel.SuspendLayout();
            statusGroupBox.SuspendLayout();
            statusLayout.SuspendLayout();
            buttonPanel.SuspendLayout();
            SuspendLayout();
            // 
            // connectionPanel
            // 
            connectionPanel.Controls.Add(addWebsiteButton);
            connectionPanel.Controls.Add(LoginButton);
            connectionPanel.Controls.Add(deleteServerButton);
            connectionPanel.Controls.Add(editServerButton);
            connectionPanel.Controls.Add(addServerButton);
            connectionPanel.Controls.Add(passwordLabel);
            connectionPanel.Controls.Add(passwordTextBox);
            connectionPanel.Controls.Add(usernameLabel);
            connectionPanel.Controls.Add(usernameTextBox);
            connectionPanel.Controls.Add(serverIpLabel);
            connectionPanel.Controls.Add(serverIpTextBox);
            connectionPanel.Controls.Add(serversLabel);
            connectionPanel.Controls.Add(serversComboBox);
            connectionPanel.Dock = DockStyle.Top;
            connectionPanel.Location = new Point(0, 0);
            connectionPanel.Margin = new Padding(3, 4, 3, 4);
            connectionPanel.Name = "connectionPanel";
            connectionPanel.Padding = new Padding(6);
            connectionPanel.Size = new Size(1427, 115);
            connectionPanel.TabIndex = 0;
            // 
            // addWebsiteButton
            // 
            addWebsiteButton.Enabled = false;
            addWebsiteButton.Location = new Point(1240, 25);
            addWebsiteButton.Name = "addWebsiteButton";
            addWebsiteButton.Size = new Size(165, 55);
            addWebsiteButton.TabIndex = 0;
            addWebsiteButton.Text = "Add Website";
            addWebsiteButton.UseVisualStyleBackColor = true;
            addWebsiteButton.Click += AddWebsiteButton_Click;
            // 
            // LoginButton
            // 
            LoginButton.Location = new Point(1052, 65);
            LoginButton.Name = "LoginButton";
            LoginButton.Size = new Size(169, 33);
            LoginButton.TabIndex = 0;
            LoginButton.Text = "Login";
            LoginButton.UseVisualStyleBackColor = true;
            LoginButton.Click += LoginButton_Click;
            // 
            // deleteServerButton
            // 
            deleteServerButton.Enabled = false;
            deleteServerButton.Location = new Point(599, 9);
            deleteServerButton.Name = "deleteServerButton";
            deleteServerButton.Size = new Size(79, 33);
            deleteServerButton.TabIndex = 0;
            deleteServerButton.Text = "Delete";
            deleteServerButton.UseVisualStyleBackColor = true;
            deleteServerButton.Click += DeleteServerButton_Click;
            // 
            // editServerButton
            // 
            editServerButton.Enabled = false;
            editServerButton.Location = new Point(505, 9);
            editServerButton.Name = "editServerButton";
            editServerButton.Size = new Size(88, 33);
            editServerButton.TabIndex = 0;
            editServerButton.Text = "Edit";
            editServerButton.UseVisualStyleBackColor = true;
            editServerButton.Click += EditServerButton_Click;
            // 
            // addServerButton
            // 
            addServerButton.Location = new Point(415, 9);
            addServerButton.Name = "addServerButton";
            addServerButton.Size = new Size(84, 33);
            addServerButton.TabIndex = 0;
            addServerButton.Text = "Add";
            addServerButton.UseVisualStyleBackColor = true;
            addServerButton.Click += AddServerButton_Click;
            // 
            // passwordLabel
            // 
            passwordLabel.AutoSize = true;
            passwordLabel.Location = new Point(733, 68);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Size = new Size(91, 25);
            passwordLabel.TabIndex = 7;
            passwordLabel.Text = "Password:";
            // 
            // passwordTextBox
            // 
            passwordTextBox.Location = new Point(830, 65);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.Size = new Size(216, 31);
            passwordTextBox.TabIndex = 0;
            passwordTextBox.UseSystemPasswordChar = true;
            // 
            // usernameLabel
            // 
            usernameLabel.AutoSize = true;
            usernameLabel.Location = new Point(419, 65);
            usernameLabel.Name = "usernameLabel";
            usernameLabel.Size = new Size(95, 25);
            usernameLabel.TabIndex = 5;
            usernameLabel.Text = "Username:";
            // 
            // usernameTextBox
            // 
            usernameTextBox.Location = new Point(520, 65);
            usernameTextBox.Name = "usernameTextBox";
            usernameTextBox.Size = new Size(207, 31);
            usernameTextBox.TabIndex = 0;
            // 
            // serverIpLabel
            // 
            serverIpLabel.AutoSize = true;
            serverIpLabel.Location = new Point(21, 65);
            serverIpLabel.Name = "serverIpLabel";
            serverIpLabel.Size = new Size(85, 25);
            serverIpLabel.TabIndex = 3;
            serverIpLabel.Text = "Server IP:";
            // 
            // serverIpTextBox
            // 
            serverIpTextBox.Location = new Point(112, 62);
            serverIpTextBox.Name = "serverIpTextBox";
            serverIpTextBox.Size = new Size(297, 31);
            serverIpTextBox.TabIndex = 0;
            // 
            // serversLabel
            // 
            serversLabel.AutoSize = true;
            serversLabel.Location = new Point(21, 12);
            serversLabel.Name = "serversLabel";
            serversLabel.Size = new Size(73, 25);
            serversLabel.TabIndex = 1;
            serversLabel.Text = "Servers:";
            // 
            // serversComboBox
            // 
            serversComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            serversComboBox.FormattingEnabled = true;
            serversComboBox.Location = new Point(112, 9);
            serversComboBox.Name = "serversComboBox";
            serversComboBox.Size = new Size(297, 33);
            serversComboBox.TabIndex = 0;
            serversComboBox.SelectedIndexChanged += ServersComboBox_SelectedIndexChanged;
            // 
            // mainContentPanel
            // 
            mainContentPanel.Controls.Add(tabControl);
            mainContentPanel.Dock = DockStyle.Fill;
            mainContentPanel.Location = new Point(0, 115);
            mainContentPanel.Margin = new Padding(3, 4, 3, 4);
            mainContentPanel.Name = "mainContentPanel";
            mainContentPanel.Size = new Size(1427, 875);
            mainContentPanel.TabIndex = 1;
            // 
            // tabControl
            // 
            tabControl.Controls.Add(websitesTab);
            tabControl.Controls.Add(serviceTab);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 0);
            tabControl.Margin = new Padding(3, 4, 3, 4);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(1427, 875);
            tabControl.TabIndex = 0;
            // 
            // websitesTab
            // 
            websitesTab.Controls.Add(websitesDataGridView);
            websitesTab.Location = new Point(4, 34);
            websitesTab.Margin = new Padding(3, 4, 3, 4);
            websitesTab.Name = "websitesTab";
            websitesTab.Padding = new Padding(3, 4, 3, 4);
            websitesTab.Size = new Size(1419, 837);
            websitesTab.TabIndex = 0;
            websitesTab.Text = "Websites";
            websitesTab.UseVisualStyleBackColor = true;
            // 
            // websitesDataGridView
            // 
            websitesDataGridView.AllowUserToAddRows = false;
            websitesDataGridView.AllowUserToDeleteRows = false;
            websitesDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            websitesDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            websitesDataGridView.ContextMenuStrip = websiteContextMenu;
            websitesDataGridView.Dock = DockStyle.Fill;
            websitesDataGridView.Location = new Point(3, 4);
            websitesDataGridView.Margin = new Padding(3, 4, 3, 4);
            websitesDataGridView.Name = "websitesDataGridView";
            websitesDataGridView.ReadOnly = true;
            websitesDataGridView.RowHeadersWidth = 62;
            websitesDataGridView.Size = new Size(1413, 829);
            websitesDataGridView.TabIndex = 0;
            websitesDataGridView.CellClick += WebsitesDataGridView_CellClick;
            // 
            // websiteContextMenu
            // 
            websiteContextMenu.ImageScalingSize = new Size(24, 24);
            websiteContextMenu.Items.AddRange(new ToolStripItem[] { getStatusMenuItem, restartServiceMenuItem, stopServiceMenuItem, disableServiceMenuItem, viewLogsMenuItem });
            websiteContextMenu.Name = "websiteContextMenu";
            websiteContextMenu.Size = new Size(203, 164);
            // 
            // getStatusMenuItem
            // 
            getStatusMenuItem.Name = "getStatusMenuItem";
            getStatusMenuItem.Size = new Size(202, 32);
            getStatusMenuItem.Text = "Get Status";
            getStatusMenuItem.Click += WebsiteContextMenu_GetStatus;
            // 
            // restartServiceMenuItem
            // 
            restartServiceMenuItem.Name = "restartServiceMenuItem";
            restartServiceMenuItem.Size = new Size(202, 32);
            restartServiceMenuItem.Text = "Restart Service";
            restartServiceMenuItem.Click += WebsiteContextMenu_RestartService;
            // 
            // stopServiceMenuItem
            // 
            stopServiceMenuItem.Name = "stopServiceMenuItem";
            stopServiceMenuItem.Size = new Size(202, 32);
            stopServiceMenuItem.Text = "Stop Service";
            stopServiceMenuItem.Click += WebsiteContextMenu_StopService;
            // 
            // disableServiceMenuItem
            // 
            disableServiceMenuItem.Name = "disableServiceMenuItem";
            disableServiceMenuItem.Size = new Size(202, 32);
            disableServiceMenuItem.Text = "Disable Service";
            disableServiceMenuItem.Click += WebsiteContextMenu_DisableService;
            // 
            // viewLogsMenuItem
            // 
            viewLogsMenuItem.Name = "viewLogsMenuItem";
            viewLogsMenuItem.Size = new Size(202, 32);
            viewLogsMenuItem.Text = "View Logs";
            viewLogsMenuItem.Click += WebsiteContextMenu_ViewLogs;
            // 
            // serviceTab
            // 
            serviceTab.Controls.Add(splitContainer);
            serviceTab.Location = new Point(4, 34);
            serviceTab.Margin = new Padding(3, 4, 3, 4);
            serviceTab.Name = "serviceTab";
            serviceTab.Padding = new Padding(3, 4, 3, 4);
            serviceTab.Size = new Size(1419, 837);
            serviceTab.TabIndex = 1;
            serviceTab.Text = "Service Management";
            serviceTab.UseVisualStyleBackColor = true;
            // 
            // splitContainer
            // 
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Location = new Point(3, 4);
            splitContainer.Margin = new Padding(3, 4, 3, 4);
            splitContainer.Name = "splitContainer";
            splitContainer.Orientation = Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(serviceControlPanel);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(serviceLogsBox);
            splitContainer.Size = new Size(1413, 829);
            splitContainer.SplitterDistance = 217;
            splitContainer.TabIndex = 0;
            // 
            // serviceControlPanel
            // 
            serviceControlPanel.Controls.Add(statusGroupBox);
            serviceControlPanel.Controls.Add(buttonPanel);
            serviceControlPanel.Dock = DockStyle.Fill;
            serviceControlPanel.Location = new Point(0, 0);
            serviceControlPanel.Margin = new Padding(3, 4, 3, 4);
            serviceControlPanel.Name = "serviceControlPanel";
            serviceControlPanel.Size = new Size(1413, 217);
            serviceControlPanel.TabIndex = 0;
            // 
            // statusGroupBox
            // 
            statusGroupBox.Controls.Add(statusLayout);
            statusGroupBox.Dock = DockStyle.Top;
            statusGroupBox.Location = new Point(0, 66);
            statusGroupBox.Margin = new Padding(3, 4, 3, 4);
            statusGroupBox.Name = "statusGroupBox";
            statusGroupBox.Padding = new Padding(3, 4, 3, 4);
            statusGroupBox.Size = new Size(1413, 150);
            statusGroupBox.TabIndex = 1;
            statusGroupBox.TabStop = false;
            statusGroupBox.Text = "Service Status";
            // 
            // statusLayout
            // 
            statusLayout.ColumnCount = 2;
            statusLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            statusLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            statusLayout.Controls.Add(serviceNameLabelTitle, 0, 0);
            statusLayout.Controls.Add(serviceNameLabel, 1, 0);
            statusLayout.Controls.Add(serviceStatusLabelTitle, 0, 1);
            statusLayout.Controls.Add(serviceStatusLabel, 1, 1);
            statusLayout.Controls.Add(serviceActiveLabelTitle, 0, 2);
            statusLayout.Controls.Add(serviceActiveLabel, 1, 2);
            statusLayout.Controls.Add(lastOperationLabelTitle, 0, 3);
            statusLayout.Controls.Add(lastOperationLabel, 1, 3);
            statusLayout.Dock = DockStyle.Fill;
            statusLayout.Location = new Point(3, 28);
            statusLayout.Margin = new Padding(3, 4, 3, 4);
            statusLayout.Name = "statusLayout";
            statusLayout.RowCount = 4;
            statusLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            statusLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            statusLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            statusLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            statusLayout.Size = new Size(1407, 118);
            statusLayout.TabIndex = 0;
            // 
            // serviceNameLabelTitle
            // 
            serviceNameLabelTitle.AutoSize = true;
            serviceNameLabelTitle.Location = new Point(3, 0);
            serviceNameLabelTitle.Name = "serviceNameLabelTitle";
            serviceNameLabelTitle.Size = new Size(123, 25);
            serviceNameLabelTitle.TabIndex = 0;
            serviceNameLabelTitle.Text = "Service Name:";
            // 
            // serviceNameLabel
            // 
            serviceNameLabel.AutoSize = true;
            serviceNameLabel.Location = new Point(425, 0);
            serviceNameLabel.Name = "serviceNameLabel";
            serviceNameLabel.Size = new Size(19, 25);
            serviceNameLabel.TabIndex = 1;
            serviceNameLabel.Tag = "serviceName";
            serviceNameLabel.Text = "-";
            // 
            // serviceStatusLabelTitle
            // 
            serviceStatusLabelTitle.AutoSize = true;
            serviceStatusLabelTitle.Location = new Point(3, 29);
            serviceStatusLabelTitle.Name = "serviceStatusLabelTitle";
            serviceStatusLabelTitle.Size = new Size(64, 25);
            serviceStatusLabelTitle.TabIndex = 2;
            serviceStatusLabelTitle.Text = "Status:";
            // 
            // serviceStatusLabel
            // 
            serviceStatusLabel.AutoSize = true;
            serviceStatusLabel.Location = new Point(425, 29);
            serviceStatusLabel.Name = "serviceStatusLabel";
            serviceStatusLabel.Size = new Size(19, 25);
            serviceStatusLabel.TabIndex = 3;
            serviceStatusLabel.Tag = "serviceStatus";
            serviceStatusLabel.Text = "-";
            // 
            // serviceActiveLabelTitle
            // 
            serviceActiveLabelTitle.AutoSize = true;
            serviceActiveLabelTitle.Location = new Point(3, 58);
            serviceActiveLabelTitle.Name = "serviceActiveLabelTitle";
            serviceActiveLabelTitle.Size = new Size(64, 25);
            serviceActiveLabelTitle.TabIndex = 4;
            serviceActiveLabelTitle.Text = "Active:";
            // 
            // serviceActiveLabel
            // 
            serviceActiveLabel.AutoSize = true;
            serviceActiveLabel.Location = new Point(425, 58);
            serviceActiveLabel.Name = "serviceActiveLabel";
            serviceActiveLabel.Size = new Size(19, 25);
            serviceActiveLabel.TabIndex = 5;
            serviceActiveLabel.Tag = "serviceActive";
            serviceActiveLabel.Text = "-";
            // 
            // lastOperationLabelTitle
            // 
            lastOperationLabelTitle.AutoSize = true;
            lastOperationLabelTitle.Location = new Point(3, 87);
            lastOperationLabelTitle.Name = "lastOperationLabelTitle";
            lastOperationLabelTitle.Size = new Size(132, 25);
            lastOperationLabelTitle.TabIndex = 6;
            lastOperationLabelTitle.Text = "Last Operation:";
            // 
            // lastOperationLabel
            // 
            lastOperationLabel.AutoSize = true;
            lastOperationLabel.Location = new Point(425, 87);
            lastOperationLabel.Name = "lastOperationLabel";
            lastOperationLabel.Size = new Size(19, 25);
            lastOperationLabel.TabIndex = 7;
            lastOperationLabel.Tag = "lastOperation";
            lastOperationLabel.Text = "-";
            // 
            // buttonPanel
            // 
            buttonPanel.Controls.Add(getStatusButton);
            buttonPanel.Controls.Add(restartButton);
            buttonPanel.Controls.Add(stopButton);
            buttonPanel.Controls.Add(disableButton);
            buttonPanel.Controls.Add(viewLogsButton);
            buttonPanel.Dock = DockStyle.Top;
            buttonPanel.Location = new Point(0, 0);
            buttonPanel.Margin = new Padding(3, 4, 3, 4);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Padding = new Padding(6);
            buttonPanel.Size = new Size(1413, 66);
            buttonPanel.TabIndex = 0;
            // 
            // getStatusButton
            // 
            getStatusButton.Enabled = false;
            getStatusButton.Location = new Point(9, 10);
            getStatusButton.Margin = new Padding(3, 4, 3, 4);
            getStatusButton.Name = "getStatusButton";
            getStatusButton.Size = new Size(124, 42);
            getStatusButton.TabIndex = 0;
            getStatusButton.Tag = "serviceButton";
            getStatusButton.Text = "Get Status";
            getStatusButton.UseVisualStyleBackColor = true;
            getStatusButton.Click += GetStatusButton_Click;
            // 
            // restartButton
            // 
            restartButton.Enabled = false;
            restartButton.Location = new Point(139, 10);
            restartButton.Margin = new Padding(3, 4, 3, 4);
            restartButton.Name = "restartButton";
            restartButton.Size = new Size(149, 42);
            restartButton.TabIndex = 1;
            restartButton.Tag = "serviceButton";
            restartButton.Text = "Restart Service";
            restartButton.UseVisualStyleBackColor = true;
            restartButton.Click += RestartServiceButton_Click;
            // 
            // stopButton
            // 
            stopButton.Enabled = false;
            stopButton.Location = new Point(294, 10);
            stopButton.Margin = new Padding(3, 4, 3, 4);
            stopButton.Name = "stopButton";
            stopButton.Size = new Size(124, 42);
            stopButton.TabIndex = 2;
            stopButton.Tag = "serviceButton";
            stopButton.Text = "Stop Service";
            stopButton.UseVisualStyleBackColor = true;
            stopButton.Click += StopServiceButton_Click;
            // 
            // disableButton
            // 
            disableButton.Enabled = false;
            disableButton.Location = new Point(424, 10);
            disableButton.Margin = new Padding(3, 4, 3, 4);
            disableButton.Name = "disableButton";
            disableButton.Size = new Size(149, 42);
            disableButton.TabIndex = 3;
            disableButton.Tag = "serviceButton";
            disableButton.Text = "Disable Service";
            disableButton.UseVisualStyleBackColor = true;
            disableButton.Click += DisableServiceButton_Click;
            // 
            // viewLogsButton
            // 
            viewLogsButton.Enabled = false;
            viewLogsButton.Location = new Point(579, 10);
            viewLogsButton.Margin = new Padding(3, 4, 3, 4);
            viewLogsButton.Name = "viewLogsButton";
            viewLogsButton.Size = new Size(124, 42);
            viewLogsButton.TabIndex = 4;
            viewLogsButton.Tag = "serviceButton";
            viewLogsButton.Text = "View Logs";
            viewLogsButton.UseVisualStyleBackColor = true;
            viewLogsButton.Click += ViewLogsButton_Click;
            // 
            // serviceLogsBox
            // 
            serviceLogsBox.BackColor = Color.Black;
            serviceLogsBox.BorderStyle = BorderStyle.None;
            serviceLogsBox.Dock = DockStyle.Fill;
            serviceLogsBox.Font = new Font("Consolas", 9F);
            serviceLogsBox.ForeColor = Color.PaleGreen;
            serviceLogsBox.Location = new Point(0, 0);
            serviceLogsBox.Margin = new Padding(3, 4, 3, 4);
            serviceLogsBox.Name = "serviceLogsBox";
            serviceLogsBox.ReadOnly = true;
            serviceLogsBox.Size = new Size(1413, 608);
            serviceLogsBox.TabIndex = 0;
            serviceLogsBox.Tag = "serviceLogs";
            serviceLogsBox.Text = "";
            // 
            // statusLabel
            // 
            statusLabel.AutoSize = true;
            statusLabel.Dock = DockStyle.Bottom;
            statusLabel.Location = new Point(0, 965);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(60, 25);
            statusLabel.TabIndex = 2;
            statusLabel.Text = "Ready";
            statusLabel.Visible = false;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1427, 990);
            Controls.Add(statusLabel);
            Controls.Add(mainContentPanel);
            Controls.Add(connectionPanel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(3, 4, 3, 4);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ASP For Enhance";
            Load += MainForm_Load;
            connectionPanel.ResumeLayout(false);
            connectionPanel.PerformLayout();
            mainContentPanel.ResumeLayout(false);
            tabControl.ResumeLayout(false);
            websitesTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)websitesDataGridView).EndInit();
            websiteContextMenu.ResumeLayout(false);
            serviceTab.ResumeLayout(false);
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            serviceControlPanel.ResumeLayout(false);
            statusGroupBox.ResumeLayout(false);
            statusLayout.ResumeLayout(false);
            statusLayout.PerformLayout();
            buttonPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel connectionPanel;
        private Panel mainContentPanel;
        private TabControl tabControl;
        private TabPage websitesTab;
        private TabPage serviceTab;
        private DataGridView websitesDataGridView;

        private SplitContainer splitContainer;
        private Panel serviceControlPanel;
        private GroupBox statusGroupBox;
        private TableLayoutPanel statusLayout;
        private Label serviceNameLabelTitle;
        private Label serviceNameLabel;
        private Label serviceStatusLabelTitle;
        private Label serviceStatusLabel;
        private Label serviceActiveLabelTitle;
        private Label serviceActiveLabel;
        private Label lastOperationLabelTitle;
        private Label lastOperationLabel;
        private FlowLayoutPanel buttonPanel;
        private Button getStatusButton;
        private Button restartButton;
        private Button stopButton;
        private Button disableButton;
        private Button viewLogsButton;
        private RichTextBox serviceLogsBox;
        private ContextMenuStrip websiteContextMenu;
        private ToolStripMenuItem getStatusMenuItem;
        private ToolStripMenuItem restartServiceMenuItem;
        private ToolStripMenuItem stopServiceMenuItem;
        private ToolStripMenuItem disableServiceMenuItem;
        private ToolStripMenuItem viewLogsMenuItem;
        private Label statusLabel;
        private ComboBox serversComboBox;
        private TextBox serverIpTextBox;
        private TextBox usernameTextBox;
        private TextBox passwordTextBox;
        private Button addServerButton;
        private Button editServerButton;
        private Button deleteServerButton;
        private Button LoginButton;
        private Button addWebsiteButton;
        private Label serversLabel;
        private Label serverIpLabel;
        private Label usernameLabel;
        private Label passwordLabel;
    }
}
