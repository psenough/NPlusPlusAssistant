
namespace N__Assistant
{
    partial class Form1
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.progressLabel = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.screenshotsDir = new System.Windows.Forms.LinkLabel();
            this.ScreenshotsLabel = new System.Windows.Forms.Label();
            this.backupsDir = new System.Windows.Forms.LinkLabel();
            this.nppAssistantLabel = new System.Windows.Forms.Label();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.backupNow = new System.Windows.Forms.Button();
            this.gameProfileLabel = new System.Windows.Forms.Label();
            this.steamInstallDirLabel = new System.Windows.Forms.Label();
            this.profileDir = new System.Windows.Forms.LinkLabel();
            this.steamInstallDir = new System.Windows.Forms.LinkLabel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.profileList = new System.Windows.Forms.ListBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Location = new System.Drawing.Point(9, 10);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(564, 273);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.progressLabel);
            this.tabPage1.Controls.Add(this.progressBar1);
            this.tabPage1.Controls.Add(this.screenshotsDir);
            this.tabPage1.Controls.Add(this.ScreenshotsLabel);
            this.tabPage1.Controls.Add(this.backupsDir);
            this.tabPage1.Controls.Add(this.nppAssistantLabel);
            this.tabPage1.Controls.Add(this.checkedListBox1);
            this.tabPage1.Controls.Add(this.backupNow);
            this.tabPage1.Controls.Add(this.gameProfileLabel);
            this.tabPage1.Controls.Add(this.steamInstallDirLabel);
            this.tabPage1.Controls.Add(this.profileDir);
            this.tabPage1.Controls.Add(this.steamInstallDir);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(556, 247);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Status";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // progressLabel
            // 
            this.progressLabel.AutoSize = true;
            this.progressLabel.Location = new System.Drawing.Point(155, 182);
            this.progressLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(0, 13);
            this.progressLabel.TabIndex = 11;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(152, 216);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(2);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(154, 19);
            this.progressBar1.TabIndex = 10;
            this.progressBar1.Visible = false;
            // 
            // screenshotsDir
            // 
            this.screenshotsDir.AutoSize = true;
            this.screenshotsDir.Location = new System.Drawing.Point(118, 72);
            this.screenshotsDir.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.screenshotsDir.Name = "screenshotsDir";
            this.screenshotsDir.Size = new System.Drawing.Size(30, 13);
            this.screenshotsDir.TabIndex = 9;
            this.screenshotsDir.TabStop = true;
            this.screenshotsDir.Text = "temp";
            this.screenshotsDir.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel4_LinkClicked);
            // 
            // ScreenshotsLabel
            // 
            this.ScreenshotsLabel.AutoSize = true;
            this.ScreenshotsLabel.Location = new System.Drawing.Point(5, 72);
            this.ScreenshotsLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ScreenshotsLabel.Name = "ScreenshotsLabel";
            this.ScreenshotsLabel.Size = new System.Drawing.Size(111, 13);
            this.ScreenshotsLabel.TabIndex = 8;
            this.ScreenshotsLabel.Text = "Screenshots Directory";
            // 
            // backupsDir
            // 
            this.backupsDir.AutoSize = true;
            this.backupsDir.Location = new System.Drawing.Point(118, 99);
            this.backupsDir.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.backupsDir.Name = "backupsDir";
            this.backupsDir.Size = new System.Drawing.Size(30, 13);
            this.backupsDir.TabIndex = 7;
            this.backupsDir.TabStop = true;
            this.backupsDir.Text = "temp";
            this.backupsDir.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
            // 
            // nppAssistantLabel
            // 
            this.nppAssistantLabel.AutoSize = true;
            this.nppAssistantLabel.Location = new System.Drawing.Point(5, 99);
            this.nppAssistantLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.nppAssistantLabel.Name = "nppAssistantLabel";
            this.nppAssistantLabel.Size = new System.Drawing.Size(117, 13);
            this.nppAssistantLabel.TabIndex = 6;
            this.nppAssistantLabel.Text = "N++ Assistant Directory";
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Items.AddRange(new object[] {
            "profile",
            "soundpack",
            "editor levels",
            "attract files",
            "palettes",
            "game levels"});
            this.checkedListBox1.Location = new System.Drawing.Point(8, 141);
            this.checkedListBox1.Margin = new System.Windows.Forms.Padding(2);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(128, 94);
            this.checkedListBox1.TabIndex = 5;
            // 
            // backupNow
            // 
            this.backupNow.Location = new System.Drawing.Point(152, 141);
            this.backupNow.Margin = new System.Windows.Forms.Padding(2);
            this.backupNow.Name = "backupNow";
            this.backupNow.Size = new System.Drawing.Size(154, 23);
            this.backupNow.TabIndex = 4;
            this.backupNow.Text = "Backup Now";
            this.backupNow.UseVisualStyleBackColor = true;
            this.backupNow.Click += new System.EventHandler(this.button2_Click);
            // 
            // gameProfileLabel
            // 
            this.gameProfileLabel.AutoSize = true;
            this.gameProfileLabel.Location = new System.Drawing.Point(5, 46);
            this.gameProfileLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.gameProfileLabel.Name = "gameProfileLabel";
            this.gameProfileLabel.Size = new System.Drawing.Size(112, 13);
            this.gameProfileLabel.TabIndex = 3;
            this.gameProfileLabel.Text = "Game Profile Directory";
            // 
            // steamInstallDirLabel
            // 
            this.steamInstallDirLabel.AutoSize = true;
            this.steamInstallDirLabel.Location = new System.Drawing.Point(5, 17);
            this.steamInstallDirLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.steamInstallDirLabel.Name = "steamInstallDirLabel";
            this.steamInstallDirLabel.Size = new System.Drawing.Size(112, 13);
            this.steamInstallDirLabel.TabIndex = 2;
            this.steamInstallDirLabel.Text = "Steam Install Directory";
            // 
            // profileDir
            // 
            this.profileDir.AutoSize = true;
            this.profileDir.Location = new System.Drawing.Point(118, 46);
            this.profileDir.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.profileDir.Name = "profileDir";
            this.profileDir.Size = new System.Drawing.Size(30, 13);
            this.profileDir.TabIndex = 1;
            this.profileDir.TabStop = true;
            this.profileDir.Text = "temp";
            this.profileDir.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // steamInstallDir
            // 
            this.steamInstallDir.AutoSize = true;
            this.steamInstallDir.Location = new System.Drawing.Point(118, 17);
            this.steamInstallDir.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.steamInstallDir.Name = "steamInstallDir";
            this.steamInstallDir.Size = new System.Drawing.Size(30, 13);
            this.steamInstallDir.TabIndex = 0;
            this.steamInstallDir.TabStop = true;
            this.steamInstallDir.Text = "temp";
            this.steamInstallDir.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.profileList);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(438, 247);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Profile";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(221, 49);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(56, 19);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // profileList
            // 
            this.profileList.FormattingEnabled = true;
            this.profileList.Location = new System.Drawing.Point(4, 49);
            this.profileList.Margin = new System.Windows.Forms.Padding(2);
            this.profileList.Name = "profileList";
            this.profileList.ScrollAlwaysVisible = true;
            this.profileList.Size = new System.Drawing.Size(213, 186);
            this.profileList.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(438, 247);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Soundpacks";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(438, 247);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Palettes";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(438, 247);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Practice Maps";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tabPage6
            // 
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(438, 247);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "Map Packs";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 292);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "N++ Assistant";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.LinkLabel steamInstallDir;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.LinkLabel profileDir;
        private System.Windows.Forms.Label gameProfileLabel;
        private System.Windows.Forms.Label steamInstallDirLabel;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Button backupNow;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox profileList;
        private System.Windows.Forms.LinkLabel backupsDir;
        private System.Windows.Forms.Label nppAssistantLabel;
        private System.Windows.Forms.LinkLabel screenshotsDir;
        private System.Windows.Forms.Label ScreenshotsLabel;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label progressLabel;
    }
}

