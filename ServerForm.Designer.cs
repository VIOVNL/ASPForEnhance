namespace ASPForEnhance
{
    partial class ServerForm
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
            hostnameTextBox = new TextBox();
            usernameTextBox = new TextBox();
            passwordTextBox = new TextBox();
            portNumericUpDown = new NumericUpDown();
            saveButton = new Button();
            cancelButton = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            ((System.ComponentModel.ISupportInitialize)portNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // nameTextBox
            // 
            nameTextBox.Location = new Point(150, 20);
            nameTextBox.Name = "nameTextBox";
            nameTextBox.Size = new Size(300, 31);
            nameTextBox.TabIndex = 0;
            // 
            // hostnameTextBox
            // 
            hostnameTextBox.Location = new Point(150, 60);
            hostnameTextBox.Name = "hostnameTextBox";
            hostnameTextBox.Size = new Size(300, 31);
            hostnameTextBox.TabIndex = 1;
            // 
            // usernameTextBox
            // 
            usernameTextBox.Location = new Point(150, 100);
            usernameTextBox.Name = "usernameTextBox";
            usernameTextBox.Size = new Size(300, 31);
            usernameTextBox.TabIndex = 2;
            // 
            // passwordTextBox
            // 
            passwordTextBox.Location = new Point(150, 140);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.PasswordChar = '*';
            passwordTextBox.Size = new Size(300, 31);
            passwordTextBox.TabIndex = 3;
            // 
            // portNumericUpDown
            // 
            portNumericUpDown.Location = new Point(150, 180);
            portNumericUpDown.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            portNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            portNumericUpDown.Name = "portNumericUpDown";
            portNumericUpDown.Size = new Size(150, 31);
            portNumericUpDown.TabIndex = 4;
            portNumericUpDown.Value = new decimal(new int[] { 22, 0, 0, 0 });
            // 
            // saveButton
            // 
            saveButton.Location = new Point(150, 230);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(112, 34);
            saveButton.TabIndex = 5;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += SaveButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.Location = new Point(280, 230);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(112, 34);
            cancelButton.TabIndex = 6;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += CancelButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(20, 20);
            label1.Name = "label1";
            label1.Size = new Size(113, 25);
            label1.TabIndex = 7;
            label1.Text = "Server Name:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(20, 60);
            label2.Name = "label2";
            label2.Size = new Size(119, 25);
            label2.TabIndex = 8;
            label2.Text = "Hostname/IP:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(20, 100);
            label3.Name = "label3";
            label3.Size = new Size(95, 25);
            label3.TabIndex = 9;
            label3.Text = "Username:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(20, 140);
            label4.Name = "label4";
            label4.Size = new Size(91, 25);
            label4.TabIndex = 10;
            label4.Text = "Password:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(20, 180);
            label5.Name = "label5";
            label5.Size = new Size(50, 25);
            label5.TabIndex = 11;
            label5.Text = "Port:";
            // 
            // ServerForm
            // 
            AcceptButton = saveButton;
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = cancelButton;
            ClientSize = new Size(478, 294);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(cancelButton);
            Controls.Add(saveButton);
            Controls.Add(portNumericUpDown);
            Controls.Add(passwordTextBox);
            Controls.Add(usernameTextBox);
            Controls.Add(hostnameTextBox);
            Controls.Add(nameTextBox);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ServerForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Server Details";
            ((System.ComponentModel.ISupportInitialize)portNumericUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox nameTextBox;
        private TextBox hostnameTextBox;
        private TextBox usernameTextBox;
        private TextBox passwordTextBox;
        private NumericUpDown portNumericUpDown;
        private Button saveButton;
        private Button cancelButton;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
    }
}
