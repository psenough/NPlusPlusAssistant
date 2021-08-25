
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
            this.listBox1 = new System.Windows.Forms.ListBox();
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
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(776, 426);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
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
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(768, 397);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Status";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // screenshotsDir
            // 
            this.screenshotsDir.AutoSize = true;
            this.screenshotsDir.Location = new System.Drawing.Point(158, 89);
            this.screenshotsDir.Name = "screenshotsDir";
            this.screenshotsDir.Size = new System.Drawing.Size(39, 17);
            this.screenshotsDir.TabIndex = 9;
            this.screenshotsDir.TabStop = true;
            this.screenshotsDir.Text = "temp";
            this.screenshotsDir.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel4_LinkClicked);
            // 
            // ScreenshotsLabel
            // 
            this.ScreenshotsLabel.AutoSize = true;
            this.ScreenshotsLabel.Location = new System.Drawing.Point(7, 89);
            this.ScreenshotsLabel.Name = "ScreenshotsLabel";
            this.ScreenshotsLabel.Size = new System.Drawing.Size(148, 17);
            this.ScreenshotsLabel.TabIndex = 8;
            this.ScreenshotsLabel.Text = "Screenshots Directory";
            // 
            // backupsDir
            // 
            this.backupsDir.AutoSize = true;
            this.backupsDir.Location = new System.Drawing.Point(158, 122);
            this.backupsDir.Name = "backupsDir";
            this.backupsDir.Size = new System.Drawing.Size(39, 17);
            this.backupsDir.TabIndex = 7;
            this.backupsDir.TabStop = true;
            this.backupsDir.Text = "temp";
            this.backupsDir.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
            // 
            // nppAssistantLabel
            // 
            this.nppAssistantLabel.AutoSize = true;
            this.nppAssistantLabel.Location = new System.Drawing.Point(7, 122);
            this.nppAssistantLabel.Name = "nppAssistantLabel";
            this.nppAssistantLabel.Size = new System.Drawing.Size(156, 17);
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
            this.checkedListBox1.Location = new System.Drawing.Point(10, 174);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(170, 123);
            this.checkedListBox1.TabIndex = 5;
            // 
            // backupNow
            // 
            this.backupNow.Location = new System.Drawing.Point(203, 174);
            this.backupNow.Name = "backupNow";
            this.backupNow.Size = new System.Drawing.Size(205, 28);
            this.backupNow.TabIndex = 4;
            this.backupNow.Text = "Backup Now";
            this.backupNow.UseVisualStyleBackColor = true;
            this.backupNow.Click += new System.EventHandler(this.button2_Click);
            // 
            // gameProfileLabel
            // 
            this.gameProfileLabel.AutoSize = true;
            this.gameProfileLabel.Location = new System.Drawing.Point(7, 56);
            this.gameProfileLabel.Name = "gameProfileLabel";
            this.gameProfileLabel.Size = new System.Drawing.Size(151, 17);
            this.gameProfileLabel.TabIndex = 3;
            this.gameProfileLabel.Text = "Game Profile Directory";
            // 
            // steamInstallDirLabel
            // 
            this.steamInstallDirLabel.AutoSize = true;
            this.steamInstallDirLabel.Location = new System.Drawing.Point(7, 21);
            this.steamInstallDirLabel.Name = "steamInstallDirLabel";
            this.steamInstallDirLabel.Size = new System.Drawing.Size(149, 17);
            this.steamInstallDirLabel.TabIndex = 2;
            this.steamInstallDirLabel.Text = "Steam Install Directory";
            // 
            // profileDir
            // 
            this.profileDir.AutoSize = true;
            this.profileDir.Location = new System.Drawing.Point(158, 56);
            this.profileDir.Name = "profileDir";
            this.profileDir.Size = new System.Drawing.Size(39, 17);
            this.profileDir.TabIndex = 1;
            this.profileDir.TabStop = true;
            this.profileDir.Text = "temp";
            this.profileDir.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // steamInstallDir
            // 
            this.steamInstallDir.AutoSize = true;
            this.steamInstallDir.Location = new System.Drawing.Point(158, 21);
            this.steamInstallDir.Name = "steamInstallDir";
            this.steamInstallDir.Size = new System.Drawing.Size(39, 17);
            this.steamInstallDir.TabIndex = 0;
            this.steamInstallDir.TabStop = true;
            this.steamInstallDir.Text = "temp";
            this.steamInstallDir.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.listBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(768, 397);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Profile";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(6, 60);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(120, 84);
            this.listBox1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(768, 397);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Soundpacks";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(768, 397);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Palettes";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Location = new System.Drawing.Point(4, 25);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(768, 397);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Practice Maps";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tabPage6
            // 
            this.tabPage6.Location = new System.Drawing.Point(4, 25);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(768, 397);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "Map Packs";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
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
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.LinkLabel backupsDir;
        private System.Windows.Forms.Label nppAssistantLabel;
        private System.Windows.Forms.LinkLabel screenshotsDir;
        private System.Windows.Forms.Label ScreenshotsLabel;
    }
}

