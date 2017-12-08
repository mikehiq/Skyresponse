namespace Skyresponse.Forms
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.browseSoundFile = new System.Windows.Forms.Button();
            this.logoutButton = new System.Windows.Forms.Button();
            this.deviceOutComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // browseSoundFile
            // 
            this.browseSoundFile.Location = new System.Drawing.Point(35, 51);
            this.browseSoundFile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.browseSoundFile.Name = "browseSoundFile";
            this.browseSoundFile.Size = new System.Drawing.Size(101, 41);
            this.browseSoundFile.TabIndex = 0;
            this.browseSoundFile.Text = "Browse...";
            this.browseSoundFile.UseVisualStyleBackColor = true;
            this.browseSoundFile.Click += new System.EventHandler(this.BrowseSoundFile_Click);
            // 
            // logoutButton
            // 
            this.logoutButton.Location = new System.Drawing.Point(35, 126);
            this.logoutButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.logoutButton.Name = "logoutButton";
            this.logoutButton.Size = new System.Drawing.Size(90, 41);
            this.logoutButton.TabIndex = 1;
            this.logoutButton.Text = "Logout";
            this.logoutButton.UseVisualStyleBackColor = true;
            this.logoutButton.Click += new System.EventHandler(this.LogoutButton_Click);
            // 
            // deviceOutComboBox
            // 
            this.deviceOutComboBox.FormattingEnabled = true;
            this.deviceOutComboBox.Location = new System.Drawing.Point(235, 51);
            this.deviceOutComboBox.Name = "deviceOutComboBox";
            this.deviceOutComboBox.Size = new System.Drawing.Size(114, 28);
            this.deviceOutComboBox.TabIndex = 2;
            this.deviceOutComboBox.SelectedIndexChanged += new System.EventHandler(this.deviceOutComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(32, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Browse sound file";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(232, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Choose sound output";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 199);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.deviceOutComboBox);
            this.Controls.Add(this.logoutButton);
            this.Controls.Add(this.browseSoundFile);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Skyresponse ljudmonitor";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button browseSoundFile;
        private System.Windows.Forms.Button logoutButton;
        private System.Windows.Forms.ComboBox deviceOutComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

