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
            idTextBox = new TextBox();
            aspDllPathTextBox = new TextBox();
            saveButton = new Button();
            cancelButton = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            portInfoLabel = new Label();
            generateIdButton = new Button();
            blazorSignalRCheckBox = new CheckBox();
            folderPathTextBox = new TextBox();
            label4 = new Label();
            SuspendLayout();
            // 
            // nameTextBox
            // 
            nameTextBox.Location = new Point(150, 20);
            nameTextBox.Name = "nameTextBox";
            nameTextBox.Size = new Size(300, 31);
            nameTextBox.TabIndex = 0;
            // 
            // idTextBox
            // 
            idTextBox.Location = new Point(150, 60);
            idTextBox.Name = "idTextBox";
            idTextBox.Size = new Size(300, 31);
            idTextBox.TabIndex = 1;
            // 
            // aspDllPathTextBox
            // 
            aspDllPathTextBox.Location = new Point(150, 100);
            aspDllPathTextBox.Name = "aspDllPathTextBox";
            aspDllPathTextBox.Size = new Size(300, 31);
            aspDllPathTextBox.TabIndex = 2;
            // 
            // saveButton
            // 
            saveButton.Location = new Point(150, 245);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(112, 34);
            saveButton.TabIndex = 6;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += SaveButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.Location = new Point(280, 245);
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
            label1.Size = new Size(129, 25);
            label1.TabIndex = 8;
            label1.Text = "Website Name:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(20, 60);
            label2.Name = "label2";
            label2.Size = new Size(104, 25);
            label2.TabIndex = 9;
            label2.Text = "Website ID:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(20, 100);
            label3.Name = "label3";
            label3.Size = new Size(117, 25);
            label3.TabIndex = 10;
            label3.Text = "ASP DLL Path:";
            // 
            // portInfoLabel
            // 
            portInfoLabel.AutoSize = true;
            portInfoLabel.Location = new Point(150, 180);
            portInfoLabel.Name = "portInfoLabel";
            portInfoLabel.Size = new Size(172, 25);
            portInfoLabel.TabIndex = 11;
            portInfoLabel.Text = "Port: (Auto-assigned)";
            // 
            // generateIdButton
            // 
            generateIdButton.Location = new Point(460, 60);
            generateIdButton.Name = "generateIdButton";
            generateIdButton.Size = new Size(112, 34);
            generateIdButton.TabIndex = 3;
            generateIdButton.Text = "Generate";
            generateIdButton.UseVisualStyleBackColor = true;
            generateIdButton.Click += GenerateIdButton_Click;
            // 
            // blazorSignalRCheckBox
            // 
            blazorSignalRCheckBox.AutoSize = true;
            blazorSignalRCheckBox.Location = new Point(150, 210);
            blazorSignalRCheckBox.Name = "blazorSignalRCheckBox";
            blazorSignalRCheckBox.Size = new Size(210, 29);
            blazorSignalRCheckBox.TabIndex = 5;
            blazorSignalRCheckBox.Text = "Enable Blazor/SignalR";
            blazorSignalRCheckBox.UseVisualStyleBackColor = true;
            // 
            // folderPathTextBox
            // 
            folderPathTextBox.Location = new Point(150, 140);
            folderPathTextBox.Name = "folderPathTextBox";
            folderPathTextBox.Size = new Size(300, 31);
            folderPathTextBox.TabIndex = 4;
            folderPathTextBox.Text = "api";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(20, 140);
            label4.Name = "label4";
            label4.Size = new Size(105, 25);
            label4.TabIndex = 12;
            label4.Text = "Folder Path:";
            // 
            // WebsiteForm
            // 
            AcceptButton = saveButton;
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = cancelButton;
            ClientSize = new Size(588, 294);
            Controls.Add(label4);
            Controls.Add(folderPathTextBox);
            Controls.Add(blazorSignalRCheckBox);
            Controls.Add(generateIdButton);
            Controls.Add(portInfoLabel);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(cancelButton);
            Controls.Add(saveButton);
            Controls.Add(aspDllPathTextBox);
            Controls.Add(idTextBox);
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
        private TextBox idTextBox;
        private TextBox aspDllPathTextBox;
        private Button saveButton;
        private Button cancelButton;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label portInfoLabel;
        private Button generateIdButton;
        private CheckBox blazorSignalRCheckBox;
        private TextBox folderPathTextBox;
        private Label label4;
    }
}
