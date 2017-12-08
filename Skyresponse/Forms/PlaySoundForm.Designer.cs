namespace Skyresponse.Forms
{
    partial class PlaySoundForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlaySoundForm));
            this.browseSoundFile = new System.Windows.Forms.Button();
            this.logoutButton = new System.Windows.Forms.Button();
            this.deviceOutComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // browseSoundFile
            // 
            this.browseSoundFile.Location = new System.Drawing.Point(294, 163);
            this.browseSoundFile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.browseSoundFile.Name = "browseSoundFile";
            this.browseSoundFile.Size = new System.Drawing.Size(114, 45);
            this.browseSoundFile.TabIndex = 0;
            this.browseSoundFile.Text = "Browse...";
            this.browseSoundFile.UseVisualStyleBackColor = true;
            this.browseSoundFile.Click += new System.EventHandler(this.BrowseSoundFile_Click);
            // 
            // logoutButton
            // 
            this.logoutButton.Location = new System.Drawing.Point(296, 323);
            this.logoutButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.logoutButton.Name = "logoutButton";
            this.logoutButton.Size = new System.Drawing.Size(112, 35);
            this.logoutButton.TabIndex = 1;
            this.logoutButton.Text = "Logout";
            this.logoutButton.UseVisualStyleBackColor = true;
            this.logoutButton.Click += new System.EventHandler(this.LogoutButton_Click);
            // 
            // deviceOutComboBox
            // 
            this.deviceOutComboBox.FormattingEnabled = true;
            this.deviceOutComboBox.Location = new System.Drawing.Point(33, 73);
            this.deviceOutComboBox.Name = "deviceOutComboBox";
            this.deviceOutComboBox.Size = new System.Drawing.Size(256, 28);
            this.deviceOutComboBox.TabIndex = 2;
            this.deviceOutComboBox.SelectedIndexChanged += new System.EventHandler(this.deviceOutComboBox_SelectedIndexChanged);
            // 
            // PlaySoundForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 402);
            this.Controls.Add(this.deviceOutComboBox);
            this.Controls.Add(this.logoutButton);
            this.Controls.Add(this.browseSoundFile);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "PlaySoundForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Skyresponse ljudmonitor";
            this.Load += new System.EventHandler(this.PlaySoundForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button browseSoundFile;
        private System.Windows.Forms.Button logoutButton;
        private System.Windows.Forms.ComboBox deviceOutComboBox;
    }
}

