namespace ASPForEnhance
{
    partial class WebsiteForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            nameTextBox = new TextBox();
            fullPathTextBox = new TextBox();
            saveButton = new Button();
            cancelButton = new Button();
            label1 = new Label();
            label3 = new Label();
            portInfoLabel = new Label();
            blazorSignalRCheckBox = new CheckBox();
            SuspendLayout();
            // 
            // nameTextBox
            // 
            nameTextBox.Location = new Point(178, 20);
            nameTextBox.Name = "nameTextBox";
            nameTextBox.PlaceholderText = "website.com";
            nameTextBox.Size = new Size(695, 31);
            nameTextBox.TabIndex = 0;
            // 
            // fullPathTextBox
            // 
            fullPathTextBox.Location = new Point(178, 69);
            fullPathTextBox.Name = "fullPathTextBox";
            fullPathTextBox.PlaceholderText = "/var/www/677c5d2b-d623-450a-81c5-ad3dee239df4/public_html/AspProject.dll";
            fullPathTextBox.Size = new Size(695, 31);
            fullPathTextBox.TabIndex = 2;
            // 
            // saveButton
            // 
            saveButton.Location = new Point(761, 175);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(112, 34);
            saveButton.TabIndex = 6;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += SaveButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.Location = new Point(643, 175);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(112, 34);
            cancelButton.TabIndex = 7;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += CancelButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(20, 20);
            label1.Name = "label1";
            label1.Size = new Size(131, 25);
            label1.TabIndex = 8;
            label1.Text = "Domain Name:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(20, 69);
            label3.Name = "label3";
            label3.Size = new Size(84, 25);
            label3.TabIndex = 10;
            label3.Text = "Full path:";
            // 
            // portInfoLabel
            // 
            portInfoLabel.AutoSize = true;
            portInfoLabel.Location = new Point(178, 147);
            portInfoLabel.Name = "portInfoLabel";
            portInfoLabel.Size = new Size(179, 25);
            portInfoLabel.TabIndex = 11;
            portInfoLabel.Text = "Port: (Auto-assigned)";
            // 
            // blazorSignalRCheckBox
            // 
            blazorSignalRCheckBox.AutoSize = true;
            blazorSignalRCheckBox.Location = new Point(178, 106);
            blazorSignalRCheckBox.Name = "blazorSignalRCheckBox";
            blazorSignalRCheckBox.Size = new Size(209, 29);
            blazorSignalRCheckBox.TabIndex = 5;
            blazorSignalRCheckBox.Text = "Enable Blazor/SignalR";
            blazorSignalRCheckBox.UseVisualStyleBackColor = true;
            // 
            // WebsiteForm
            // 
            AcceptButton = saveButton;
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = cancelButton;
            ClientSize = new Size(893, 227);
            Controls.Add(blazorSignalRCheckBox);
            Controls.Add(portInfoLabel);
            Controls.Add(label3);
            Controls.Add(label1);
            Controls.Add(cancelButton);
            Controls.Add(saveButton);
            Controls.Add(fullPathTextBox);
            Controls.Add(nameTextBox);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "WebsiteForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Website Details";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox nameTextBox;
        private TextBox fullPathTextBox;
        private Button saveButton;
        private Button cancelButton;
        private Label label1;
        private Label label3;
        private Label portInfoLabel;
        private CheckBox blazorSignalRCheckBox;
    }
}
