
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
            this.tabStatus = new System.Windows.Forms.TabPage();
            this.progressLabel = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.screenshotsDir = new System.Windows.Forms.LinkLabel();
            this.backupsDir = new System.Windows.Forms.LinkLabel();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.backupNow = new System.Windows.Forms.Button();
            this.profileDir = new System.Windows.Forms.LinkLabel();
            this.steamInstallDir = new System.Windows.Forms.LinkLabel();
            this.tabProfile = new System.Windows.Forms.TabPage();
            this.linkProfileFolder = new System.Windows.Forms.LinkLabel();
            this.linkBackupProfileFolder = new System.Windows.Forms.LinkLabel();
            this.deleteProfile = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.profileBackupLabel = new System.Windows.Forms.Label();
            this.loadProfile = new System.Windows.Forms.Button();
            this.backupProfile = new System.Windows.Forms.Button();
            this.profileList = new System.Windows.Forms.ListBox();
            this.tabSoundpacks = new System.Windows.Forms.TabPage();
            this.previewSoundsLabel = new System.Windows.Forms.Label();
            this.previewSoundsList = new System.Windows.Forms.ListBox();
            this.installSpreadsheetSoundpack = new System.Windows.Forms.Button();
            this.deleteSoundpackBackupButton = new System.Windows.Forms.Button();
            this.installSoundpackButton = new System.Windows.Forms.Button();
            this.soundpackBackups = new System.Windows.Forms.ListBox();
            this.backupSoundpack = new System.Windows.Forms.Button();
            this.spreadsheetSoundpacks = new System.Windows.Forms.ListBox();
            this.tabPalettes = new System.Windows.Forms.TabPage();
            this.onlineEditorLink = new System.Windows.Forms.LinkLabel();
            this.countCustomPalettesInstalled = new System.Windows.Forms.Label();
            this.palettesInstalledLinkedLabel = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.metanetPalettes = new System.Windows.Forms.TabPage();
            this.installMetanetPalette = new System.Windows.Forms.Button();
            this.metanetPalettesList = new System.Windows.Forms.ListBox();
            this.communityPalettes = new System.Windows.Forms.TabPage();
            this.installCommunityPalette = new System.Windows.Forms.Button();
            this.communityPalettesList = new System.Windows.Forms.ListBox();
            this.localBackupPalettes = new System.Windows.Forms.TabPage();
            this.deleteBackupPalette = new System.Windows.Forms.Button();
            this.installBackupPalette = new System.Windows.Forms.Button();
            this.localBackupPalettesList = new System.Windows.Forms.ListBox();
            this.backupPalette = new System.Windows.Forms.Button();
            this.uninstallPalette = new System.Windows.Forms.Button();
            this.palettesInstalledList = new System.Windows.Forms.ListBox();
            this.tabEditorMaps = new System.Windows.Forms.TabPage();
            this.tabControlEditorMaps = new System.Windows.Forms.TabControl();
            this.tabMetanetMaps = new System.Windows.Forms.TabPage();
            this.metanetMapsList = new System.Windows.Forms.ListBox();
            this.installMetanetMAp = new System.Windows.Forms.Button();
            this.tabLocalBackups = new System.Windows.Forms.TabPage();
            this.deleteSelectedMaps = new System.Windows.Forms.Button();
            this.backupSelectedMaps = new System.Windows.Forms.Button();
            this.listEditorMaps = new System.Windows.Forms.ListBox();
            this.tabMapPacks = new System.Windows.Forms.TabPage();
            this.localEditorLevelsBackupsList = new System.Windows.Forms.ListBox();
            this.localEditorMaps = new System.Windows.Forms.ListBox();
            this.installBackupMap = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabStatus.SuspendLayout();
            this.tabProfile.SuspendLayout();
            this.tabSoundpacks.SuspendLayout();
            this.tabPalettes.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.metanetPalettes.SuspendLayout();
            this.communityPalettes.SuspendLayout();
            this.localBackupPalettes.SuspendLayout();
            this.tabEditorMaps.SuspendLayout();
            this.tabControlEditorMaps.SuspendLayout();
            this.tabMetanetMaps.SuspendLayout();
            this.tabLocalBackups.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabStatus);
            this.tabControl1.Controls.Add(this.tabProfile);
            this.tabControl1.Controls.Add(this.tabSoundpacks);
            this.tabControl1.Controls.Add(this.tabPalettes);
            this.tabControl1.Controls.Add(this.tabEditorMaps);
            this.tabControl1.Controls.Add(this.tabMapPacks);
            this.tabControl1.Location = new System.Drawing.Point(9, 10);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(566, 401);
            this.tabControl1.TabIndex = 0;
            // 
            // tabStatus
            // 
            this.tabStatus.Controls.Add(this.progressLabel);
            this.tabStatus.Controls.Add(this.progressBar1);
            this.tabStatus.Controls.Add(this.screenshotsDir);
            this.tabStatus.Controls.Add(this.backupsDir);
            this.tabStatus.Controls.Add(this.checkedListBox1);
            this.tabStatus.Controls.Add(this.backupNow);
            this.tabStatus.Controls.Add(this.profileDir);
            this.tabStatus.Controls.Add(this.steamInstallDir);
            this.tabStatus.Location = new System.Drawing.Point(4, 22);
            this.tabStatus.Margin = new System.Windows.Forms.Padding(2);
            this.tabStatus.Name = "tabStatus";
            this.tabStatus.Padding = new System.Windows.Forms.Padding(2);
            this.tabStatus.Size = new System.Drawing.Size(558, 375);
            this.tabStatus.TabIndex = 0;
            this.tabStatus.Text = "Status";
            this.tabStatus.UseVisualStyleBackColor = true;
            // 
            // progressLabel
            // 
            this.progressLabel.AutoSize = true;
            this.progressLabel.Location = new System.Drawing.Point(377, 66);
            this.progressLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(0, 13);
            this.progressLabel.TabIndex = 11;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(374, 100);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(2);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(154, 19);
            this.progressBar1.TabIndex = 10;
            this.progressBar1.Visible = false;
            // 
            // screenshotsDir
            // 
            this.screenshotsDir.AutoSize = true;
            this.screenshotsDir.Location = new System.Drawing.Point(17, 76);
            this.screenshotsDir.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.screenshotsDir.Name = "screenshotsDir";
            this.screenshotsDir.Size = new System.Drawing.Size(151, 13);
            this.screenshotsDir.TabIndex = 9;
            this.screenshotsDir.TabStop = true;
            this.screenshotsDir.Text = "Steam N++ Screenshots folder";
            this.screenshotsDir.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.screenshotsPathLabel_LinkClicked);
            // 
            // backupsDir
            // 
            this.backupsDir.AutoSize = true;
            this.backupsDir.Location = new System.Drawing.Point(17, 106);
            this.backupsDir.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.backupsDir.Name = "backupsDir";
            this.backupsDir.Size = new System.Drawing.Size(146, 13);
            this.backupsDir.TabIndex = 7;
            this.backupsDir.TabStop = true;
            this.backupsDir.Text = "N++ Assistant Backups folder";
            this.backupsDir.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
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
            this.checkedListBox1.Location = new System.Drawing.Point(249, 25);
            this.checkedListBox1.Margin = new System.Windows.Forms.Padding(2);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(109, 94);
            this.checkedListBox1.TabIndex = 5;
            // 
            // backupNow
            // 
            this.backupNow.Location = new System.Drawing.Point(374, 25);
            this.backupNow.Margin = new System.Windows.Forms.Padding(2);
            this.backupNow.Name = "backupNow";
            this.backupNow.Size = new System.Drawing.Size(154, 23);
            this.backupNow.TabIndex = 4;
            this.backupNow.Text = "Backup Now";
            this.backupNow.UseVisualStyleBackColor = true;
            this.backupNow.Click += new System.EventHandler(this.backupNow_Click);
            // 
            // profileDir
            // 
            this.profileDir.AutoSize = true;
            this.profileDir.Location = new System.Drawing.Point(17, 48);
            this.profileDir.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.profileDir.Name = "profileDir";
            this.profileDir.Size = new System.Drawing.Size(119, 13);
            this.profileDir.TabIndex = 1;
            this.profileDir.TabStop = true;
            this.profileDir.Text = "N++ Game Profile folder";
            this.profileDir.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.profileLink_LinkClicked);
            // 
            // steamInstallDir
            // 
            this.steamInstallDir.AutoSize = true;
            this.steamInstallDir.Location = new System.Drawing.Point(17, 21);
            this.steamInstallDir.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.steamInstallDir.Name = "steamInstallDir";
            this.steamInstallDir.Size = new System.Drawing.Size(119, 13);
            this.steamInstallDir.TabIndex = 0;
            this.steamInstallDir.TabStop = true;
            this.steamInstallDir.Text = "Steam N++ Install folder";
            this.steamInstallDir.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.steamGamePath_LinkClicked);
            // 
            // tabProfile
            // 
            this.tabProfile.Controls.Add(this.linkProfileFolder);
            this.tabProfile.Controls.Add(this.linkBackupProfileFolder);
            this.tabProfile.Controls.Add(this.deleteProfile);
            this.tabProfile.Controls.Add(this.richTextBox1);
            this.tabProfile.Controls.Add(this.profileBackupLabel);
            this.tabProfile.Controls.Add(this.loadProfile);
            this.tabProfile.Controls.Add(this.backupProfile);
            this.tabProfile.Controls.Add(this.profileList);
            this.tabProfile.Location = new System.Drawing.Point(4, 22);
            this.tabProfile.Margin = new System.Windows.Forms.Padding(2);
            this.tabProfile.Name = "tabProfile";
            this.tabProfile.Padding = new System.Windows.Forms.Padding(2);
            this.tabProfile.Size = new System.Drawing.Size(558, 375);
            this.tabProfile.TabIndex = 1;
            this.tabProfile.Text = "Profile";
            this.tabProfile.UseVisualStyleBackColor = true;
            // 
            // linkProfileFolder
            // 
            this.linkProfileFolder.AutoSize = true;
            this.linkProfileFolder.Location = new System.Drawing.Point(435, 24);
            this.linkProfileFolder.Name = "linkProfileFolder";
            this.linkProfileFolder.Size = new System.Drawing.Size(64, 13);
            this.linkProfileFolder.TabIndex = 17;
            this.linkProfileFolder.TabStop = true;
            this.linkProfileFolder.Text = "profile folder";
            this.linkProfileFolder.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkProfileFolder_LinkClicked);
            // 
            // linkBackupProfileFolder
            // 
            this.linkBackupProfileFolder.AutoSize = true;
            this.linkBackupProfileFolder.Location = new System.Drawing.Point(289, 24);
            this.linkBackupProfileFolder.Name = "linkBackupProfileFolder";
            this.linkBackupProfileFolder.Size = new System.Drawing.Size(72, 13);
            this.linkBackupProfileFolder.TabIndex = 16;
            this.linkBackupProfileFolder.TabStop = true;
            this.linkBackupProfileFolder.Text = "backup folder";
            this.linkBackupProfileFolder.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkBackupProfileFolder_LinkClicked);
            // 
            // deleteProfile
            // 
            this.deleteProfile.Location = new System.Drawing.Point(121, 324);
            this.deleteProfile.Name = "deleteProfile";
            this.deleteProfile.Size = new System.Drawing.Size(96, 34);
            this.deleteProfile.TabIndex = 15;
            this.deleteProfile.Text = "Delete";
            this.deleteProfile.UseVisualStyleBackColor = true;
            this.deleteProfile.Click += new System.EventHandler(this.deleteProfile_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Enabled = false;
            this.richTextBox1.Location = new System.Drawing.Point(233, 62);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(309, 251);
            this.richTextBox1.TabIndex = 14;
            this.richTextBox1.Text = "";
            // 
            // profileBackupLabel
            // 
            this.profileBackupLabel.AutoSize = true;
            this.profileBackupLabel.Location = new System.Drawing.Point(230, 24);
            this.profileBackupLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.profileBackupLabel.Name = "profileBackupLabel";
            this.profileBackupLabel.Size = new System.Drawing.Size(0, 13);
            this.profileBackupLabel.TabIndex = 13;
            // 
            // loadProfile
            // 
            this.loadProfile.Location = new System.Drawing.Point(4, 324);
            this.loadProfile.Name = "loadProfile";
            this.loadProfile.Size = new System.Drawing.Size(101, 34);
            this.loadProfile.TabIndex = 2;
            this.loadProfile.Text = "Load to N++";
            this.loadProfile.UseVisualStyleBackColor = true;
            // 
            // backupProfile
            // 
            this.backupProfile.Location = new System.Drawing.Point(4, 15);
            this.backupProfile.Margin = new System.Windows.Forms.Padding(2);
            this.backupProfile.Name = "backupProfile";
            this.backupProfile.Size = new System.Drawing.Size(213, 30);
            this.backupProfile.TabIndex = 1;
            this.backupProfile.Text = "Backup Current Profile";
            this.backupProfile.UseVisualStyleBackColor = true;
            this.backupProfile.Click += new System.EventHandler(this.backupProfile_Click);
            // 
            // profileList
            // 
            this.profileList.FormattingEnabled = true;
            this.profileList.Location = new System.Drawing.Point(4, 62);
            this.profileList.Margin = new System.Windows.Forms.Padding(2);
            this.profileList.Name = "profileList";
            this.profileList.ScrollAlwaysVisible = true;
            this.profileList.Size = new System.Drawing.Size(213, 251);
            this.profileList.TabIndex = 0;
            this.profileList.SelectedIndexChanged += new System.EventHandler(this.profileList_SelectedIndexChanged);
            // 
            // tabSoundpacks
            // 
            this.tabSoundpacks.Controls.Add(this.previewSoundsLabel);
            this.tabSoundpacks.Controls.Add(this.previewSoundsList);
            this.tabSoundpacks.Controls.Add(this.installSpreadsheetSoundpack);
            this.tabSoundpacks.Controls.Add(this.deleteSoundpackBackupButton);
            this.tabSoundpacks.Controls.Add(this.installSoundpackButton);
            this.tabSoundpacks.Controls.Add(this.soundpackBackups);
            this.tabSoundpacks.Controls.Add(this.backupSoundpack);
            this.tabSoundpacks.Controls.Add(this.spreadsheetSoundpacks);
            this.tabSoundpacks.Location = new System.Drawing.Point(4, 22);
            this.tabSoundpacks.Margin = new System.Windows.Forms.Padding(2);
            this.tabSoundpacks.Name = "tabSoundpacks";
            this.tabSoundpacks.Size = new System.Drawing.Size(558, 375);
            this.tabSoundpacks.TabIndex = 2;
            this.tabSoundpacks.Text = "Soundpacks";
            this.tabSoundpacks.UseVisualStyleBackColor = true;
            // 
            // previewSoundsLabel
            // 
            this.previewSoundsLabel.AutoSize = true;
            this.previewSoundsLabel.Location = new System.Drawing.Point(315, 21);
            this.previewSoundsLabel.Name = "previewSoundsLabel";
            this.previewSoundsLabel.Size = new System.Drawing.Size(84, 13);
            this.previewSoundsLabel.TabIndex = 8;
            this.previewSoundsLabel.Text = "Preview Sounds";
            // 
            // previewSoundsList
            // 
            this.previewSoundsList.FormattingEnabled = true;
            this.previewSoundsList.Location = new System.Drawing.Point(318, 38);
            this.previewSoundsList.Name = "previewSoundsList";
            this.previewSoundsList.Size = new System.Drawing.Size(221, 316);
            this.previewSoundsList.TabIndex = 7;
            this.previewSoundsList.SelectedIndexChanged += new System.EventHandler(this.previewSoundsList_SelectedIndexChanged);
            // 
            // installSpreadsheetSoundpack
            // 
            this.installSpreadsheetSoundpack.Location = new System.Drawing.Point(12, 334);
            this.installSpreadsheetSoundpack.Name = "installSpreadsheetSoundpack";
            this.installSpreadsheetSoundpack.Size = new System.Drawing.Size(114, 29);
            this.installSpreadsheetSoundpack.TabIndex = 6;
            this.installSpreadsheetSoundpack.Text = "Install Soundpack";
            this.installSpreadsheetSoundpack.UseVisualStyleBackColor = true;
            this.installSpreadsheetSoundpack.Click += new System.EventHandler(this.installSpreadsheetSoundpack_Click);
            // 
            // deleteSoundpackBackupButton
            // 
            this.deleteSoundpackBackupButton.Location = new System.Drawing.Point(194, 139);
            this.deleteSoundpackBackupButton.Name = "deleteSoundpackBackupButton";
            this.deleteSoundpackBackupButton.Size = new System.Drawing.Size(90, 32);
            this.deleteSoundpackBackupButton.TabIndex = 5;
            this.deleteSoundpackBackupButton.Text = "Delete Backup";
            this.deleteSoundpackBackupButton.UseVisualStyleBackColor = true;
            this.deleteSoundpackBackupButton.Click += new System.EventHandler(this.deleteSoundpackBackupButton_Click);
            // 
            // installSoundpackButton
            // 
            this.installSoundpackButton.Location = new System.Drawing.Point(12, 139);
            this.installSoundpackButton.Name = "installSoundpackButton";
            this.installSoundpackButton.Size = new System.Drawing.Size(104, 32);
            this.installSoundpackButton.TabIndex = 4;
            this.installSoundpackButton.Text = "Install Soundpack";
            this.installSoundpackButton.UseVisualStyleBackColor = true;
            this.installSoundpackButton.Click += new System.EventHandler(this.installSoundpackButton_Click);
            // 
            // soundpackBackups
            // 
            this.soundpackBackups.FormattingEnabled = true;
            this.soundpackBackups.Location = new System.Drawing.Point(12, 51);
            this.soundpackBackups.Name = "soundpackBackups";
            this.soundpackBackups.Size = new System.Drawing.Size(272, 82);
            this.soundpackBackups.TabIndex = 3;
            this.soundpackBackups.SelectedIndexChanged += new System.EventHandler(this.soundpackBackups_SelectedIndexChanged);
            // 
            // backupSoundpack
            // 
            this.backupSoundpack.Location = new System.Drawing.Point(12, 11);
            this.backupSoundpack.Name = "backupSoundpack";
            this.backupSoundpack.Size = new System.Drawing.Size(272, 33);
            this.backupSoundpack.TabIndex = 2;
            this.backupSoundpack.Text = "Backup Current Soundpack";
            this.backupSoundpack.UseVisualStyleBackColor = true;
            this.backupSoundpack.Click += new System.EventHandler(this.backupSoundpack_Click);
            // 
            // spreadsheetSoundpacks
            // 
            this.spreadsheetSoundpacks.FormattingEnabled = true;
            this.spreadsheetSoundpacks.Location = new System.Drawing.Point(12, 190);
            this.spreadsheetSoundpacks.Name = "spreadsheetSoundpacks";
            this.spreadsheetSoundpacks.Size = new System.Drawing.Size(272, 134);
            this.spreadsheetSoundpacks.TabIndex = 0;
            this.spreadsheetSoundpacks.SelectedIndexChanged += new System.EventHandler(this.spreadsheetSoundpacks_SelectedIndexChanged);
            // 
            // tabPalettes
            // 
            this.tabPalettes.Controls.Add(this.onlineEditorLink);
            this.tabPalettes.Controls.Add(this.countCustomPalettesInstalled);
            this.tabPalettes.Controls.Add(this.palettesInstalledLinkedLabel);
            this.tabPalettes.Controls.Add(this.label2);
            this.tabPalettes.Controls.Add(this.tabControl2);
            this.tabPalettes.Controls.Add(this.backupPalette);
            this.tabPalettes.Controls.Add(this.uninstallPalette);
            this.tabPalettes.Controls.Add(this.palettesInstalledList);
            this.tabPalettes.Location = new System.Drawing.Point(4, 22);
            this.tabPalettes.Margin = new System.Windows.Forms.Padding(2);
            this.tabPalettes.Name = "tabPalettes";
            this.tabPalettes.Size = new System.Drawing.Size(558, 375);
            this.tabPalettes.TabIndex = 3;
            this.tabPalettes.Text = "Palettes";
            this.tabPalettes.UseVisualStyleBackColor = true;
            // 
            // onlineEditorLink
            // 
            this.onlineEditorLink.AutoSize = true;
            this.onlineEditorLink.Location = new System.Drawing.Point(475, 229);
            this.onlineEditorLink.Name = "onlineEditorLink";
            this.onlineEditorLink.Size = new System.Drawing.Size(48, 13);
            this.onlineEditorLink.TabIndex = 9;
            this.onlineEditorLink.TabStop = true;
            this.onlineEditorLink.Text = "npc-web";
            this.onlineEditorLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.onlineEditorLink_LinkClicked);
            // 
            // countCustomPalettesInstalled
            // 
            this.countCustomPalettesInstalled.AutoSize = true;
            this.countCustomPalettesInstalled.Location = new System.Drawing.Point(472, 340);
            this.countCustomPalettesInstalled.Name = "countCustomPalettesInstalled";
            this.countCustomPalettesInstalled.Size = new System.Drawing.Size(13, 13);
            this.countCustomPalettesInstalled.TabIndex = 8;
            this.countCustomPalettesInstalled.Text = "0";
            // 
            // palettesInstalledLinkedLabel
            // 
            this.palettesInstalledLinkedLabel.AutoSize = true;
            this.palettesInstalledLinkedLabel.Location = new System.Drawing.Point(362, 31);
            this.palettesInstalledLinkedLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.palettesInstalledLinkedLabel.Name = "palettesInstalledLinkedLabel";
            this.palettesInstalledLinkedLabel.Size = new System.Drawing.Size(33, 13);
            this.palettesInstalledLinkedLabel.TabIndex = 7;
            this.palettesInstalledLinkedLabel.TabStop = true;
            this.palettesInstalledLinkedLabel.Text = "folder";
            this.palettesInstalledLinkedLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.palettesInstalledLinkedLabel_LinkClicked);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(270, 31);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Palettes Installed";
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.metanetPalettes);
            this.tabControl2.Controls.Add(this.communityPalettes);
            this.tabControl2.Controls.Add(this.localBackupPalettes);
            this.tabControl2.Location = new System.Drawing.Point(10, 26);
            this.tabControl2.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(256, 337);
            this.tabControl2.TabIndex = 4;
            // 
            // metanetPalettes
            // 
            this.metanetPalettes.Controls.Add(this.installMetanetPalette);
            this.metanetPalettes.Controls.Add(this.metanetPalettesList);
            this.metanetPalettes.Location = new System.Drawing.Point(4, 22);
            this.metanetPalettes.Margin = new System.Windows.Forms.Padding(2);
            this.metanetPalettes.Name = "metanetPalettes";
            this.metanetPalettes.Padding = new System.Windows.Forms.Padding(2);
            this.metanetPalettes.Size = new System.Drawing.Size(248, 311);
            this.metanetPalettes.TabIndex = 0;
            this.metanetPalettes.Text = "Metanet";
            this.metanetPalettes.UseVisualStyleBackColor = true;
            // 
            // installMetanetPalette
            // 
            this.installMetanetPalette.Location = new System.Drawing.Point(72, 271);
            this.installMetanetPalette.Margin = new System.Windows.Forms.Padding(2);
            this.installMetanetPalette.Name = "installMetanetPalette";
            this.installMetanetPalette.Size = new System.Drawing.Size(96, 34);
            this.installMetanetPalette.TabIndex = 9;
            this.installMetanetPalette.Text = "Install Palette";
            this.installMetanetPalette.UseVisualStyleBackColor = true;
            this.installMetanetPalette.Click += new System.EventHandler(this.installMetanetPalette_Click);
            // 
            // metanetPalettesList
            // 
            this.metanetPalettesList.FormattingEnabled = true;
            this.metanetPalettesList.Location = new System.Drawing.Point(2, 3);
            this.metanetPalettesList.Margin = new System.Windows.Forms.Padding(2);
            this.metanetPalettesList.Name = "metanetPalettesList";
            this.metanetPalettesList.Size = new System.Drawing.Size(242, 264);
            this.metanetPalettesList.TabIndex = 1;
            this.metanetPalettesList.SelectedIndexChanged += new System.EventHandler(this.metanetPalettesList_SelectedIndexChanged);
            // 
            // communityPalettes
            // 
            this.communityPalettes.Controls.Add(this.installCommunityPalette);
            this.communityPalettes.Controls.Add(this.communityPalettesList);
            this.communityPalettes.Location = new System.Drawing.Point(4, 22);
            this.communityPalettes.Margin = new System.Windows.Forms.Padding(2);
            this.communityPalettes.Name = "communityPalettes";
            this.communityPalettes.Padding = new System.Windows.Forms.Padding(2);
            this.communityPalettes.Size = new System.Drawing.Size(248, 311);
            this.communityPalettes.TabIndex = 1;
            this.communityPalettes.Text = "Community";
            this.communityPalettes.UseVisualStyleBackColor = true;
            // 
            // installCommunityPalette
            // 
            this.installCommunityPalette.Location = new System.Drawing.Point(72, 271);
            this.installCommunityPalette.Margin = new System.Windows.Forms.Padding(2);
            this.installCommunityPalette.Name = "installCommunityPalette";
            this.installCommunityPalette.Size = new System.Drawing.Size(96, 34);
            this.installCommunityPalette.TabIndex = 8;
            this.installCommunityPalette.Text = "Install Palette";
            this.installCommunityPalette.UseVisualStyleBackColor = true;
            this.installCommunityPalette.Click += new System.EventHandler(this.installCommunityPalette_Click);
            // 
            // communityPalettesList
            // 
            this.communityPalettesList.FormattingEnabled = true;
            this.communityPalettesList.Location = new System.Drawing.Point(2, 3);
            this.communityPalettesList.Margin = new System.Windows.Forms.Padding(2);
            this.communityPalettesList.Name = "communityPalettesList";
            this.communityPalettesList.Size = new System.Drawing.Size(242, 264);
            this.communityPalettesList.TabIndex = 2;
            this.communityPalettesList.SelectedIndexChanged += new System.EventHandler(this.communityPalettesList_SelectedIndexChanged);
            // 
            // localBackupPalettes
            // 
            this.localBackupPalettes.Controls.Add(this.deleteBackupPalette);
            this.localBackupPalettes.Controls.Add(this.installBackupPalette);
            this.localBackupPalettes.Controls.Add(this.localBackupPalettesList);
            this.localBackupPalettes.Location = new System.Drawing.Point(4, 22);
            this.localBackupPalettes.Margin = new System.Windows.Forms.Padding(2);
            this.localBackupPalettes.Name = "localBackupPalettes";
            this.localBackupPalettes.Size = new System.Drawing.Size(248, 311);
            this.localBackupPalettes.TabIndex = 2;
            this.localBackupPalettes.Text = "Local Backups";
            this.localBackupPalettes.UseVisualStyleBackColor = true;
            // 
            // deleteBackupPalette
            // 
            this.deleteBackupPalette.Location = new System.Drawing.Point(136, 271);
            this.deleteBackupPalette.Margin = new System.Windows.Forms.Padding(2);
            this.deleteBackupPalette.Name = "deleteBackupPalette";
            this.deleteBackupPalette.Size = new System.Drawing.Size(96, 34);
            this.deleteBackupPalette.TabIndex = 10;
            this.deleteBackupPalette.Text = "Delete Backup";
            this.deleteBackupPalette.UseVisualStyleBackColor = true;
            this.deleteBackupPalette.Click += new System.EventHandler(this.deleteBackupPalette_Click);
            // 
            // installBackupPalette
            // 
            this.installBackupPalette.Location = new System.Drawing.Point(15, 271);
            this.installBackupPalette.Margin = new System.Windows.Forms.Padding(2);
            this.installBackupPalette.Name = "installBackupPalette";
            this.installBackupPalette.Size = new System.Drawing.Size(96, 34);
            this.installBackupPalette.TabIndex = 9;
            this.installBackupPalette.Text = "Install Palette";
            this.installBackupPalette.UseVisualStyleBackColor = true;
            this.installBackupPalette.Click += new System.EventHandler(this.installBackupPalette_Click);
            // 
            // localBackupPalettesList
            // 
            this.localBackupPalettesList.FormattingEnabled = true;
            this.localBackupPalettesList.Location = new System.Drawing.Point(2, 3);
            this.localBackupPalettesList.Margin = new System.Windows.Forms.Padding(2);
            this.localBackupPalettesList.Name = "localBackupPalettesList";
            this.localBackupPalettesList.Size = new System.Drawing.Size(244, 264);
            this.localBackupPalettesList.TabIndex = 2;
            this.localBackupPalettesList.SelectedIndexChanged += new System.EventHandler(this.localBackupPalettesList_SelectedIndexChanged);
            // 
            // backupPalette
            // 
            this.backupPalette.Location = new System.Drawing.Point(470, 50);
            this.backupPalette.Margin = new System.Windows.Forms.Padding(2);
            this.backupPalette.Name = "backupPalette";
            this.backupPalette.Size = new System.Drawing.Size(76, 44);
            this.backupPalette.TabIndex = 3;
            this.backupPalette.Text = "Backup Palette";
            this.backupPalette.UseVisualStyleBackColor = true;
            this.backupPalette.Click += new System.EventHandler(this.backupPalette_Click);
            // 
            // uninstallPalette
            // 
            this.uninstallPalette.Location = new System.Drawing.Point(470, 105);
            this.uninstallPalette.Margin = new System.Windows.Forms.Padding(2);
            this.uninstallPalette.Name = "uninstallPalette";
            this.uninstallPalette.Size = new System.Drawing.Size(76, 42);
            this.uninstallPalette.TabIndex = 2;
            this.uninstallPalette.Text = "Uninstall Palette";
            this.uninstallPalette.UseVisualStyleBackColor = true;
            this.uninstallPalette.Click += new System.EventHandler(this.uninstallPalette_Click);
            // 
            // palettesInstalledList
            // 
            this.palettesInstalledList.FormattingEnabled = true;
            this.palettesInstalledList.Location = new System.Drawing.Point(270, 50);
            this.palettesInstalledList.Margin = new System.Windows.Forms.Padding(2);
            this.palettesInstalledList.Name = "palettesInstalledList";
            this.palettesInstalledList.Size = new System.Drawing.Size(197, 303);
            this.palettesInstalledList.TabIndex = 1;
            this.palettesInstalledList.SelectedIndexChanged += new System.EventHandler(this.palettesInstalled_SelectedIndexChanged);
            // 
            // tabEditorMaps
            // 
            this.tabEditorMaps.Controls.Add(this.tabControlEditorMaps);
            this.tabEditorMaps.Controls.Add(this.deleteSelectedMaps);
            this.tabEditorMaps.Controls.Add(this.backupSelectedMaps);
            this.tabEditorMaps.Controls.Add(this.listEditorMaps);
            this.tabEditorMaps.Location = new System.Drawing.Point(4, 22);
            this.tabEditorMaps.Margin = new System.Windows.Forms.Padding(2);
            this.tabEditorMaps.Name = "tabEditorMaps";
            this.tabEditorMaps.Size = new System.Drawing.Size(558, 375);
            this.tabEditorMaps.TabIndex = 4;
            this.tabEditorMaps.Text = "EditorMaps";
            this.tabEditorMaps.UseVisualStyleBackColor = true;
            // 
            // tabControlEditorMaps
            // 
            this.tabControlEditorMaps.Controls.Add(this.tabMetanetMaps);
            this.tabControlEditorMaps.Controls.Add(this.tabLocalBackups);
            this.tabControlEditorMaps.Location = new System.Drawing.Point(11, 24);
            this.tabControlEditorMaps.Margin = new System.Windows.Forms.Padding(2);
            this.tabControlEditorMaps.Name = "tabControlEditorMaps";
            this.tabControlEditorMaps.SelectedIndex = 0;
            this.tabControlEditorMaps.Size = new System.Drawing.Size(329, 336);
            this.tabControlEditorMaps.TabIndex = 5;
            // 
            // tabMetanetMaps
            // 
            this.tabMetanetMaps.Controls.Add(this.metanetMapsList);
            this.tabMetanetMaps.Controls.Add(this.installMetanetMAp);
            this.tabMetanetMaps.Location = new System.Drawing.Point(4, 22);
            this.tabMetanetMaps.Margin = new System.Windows.Forms.Padding(2);
            this.tabMetanetMaps.Name = "tabMetanetMaps";
            this.tabMetanetMaps.Padding = new System.Windows.Forms.Padding(2);
            this.tabMetanetMaps.Size = new System.Drawing.Size(310, 310);
            this.tabMetanetMaps.TabIndex = 0;
            this.tabMetanetMaps.Text = "Metanet Maps";
            this.tabMetanetMaps.UseVisualStyleBackColor = true;
            // 
            // metanetMapsList
            // 
            this.metanetMapsList.FormattingEnabled = true;
            this.metanetMapsList.Location = new System.Drawing.Point(4, 5);
            this.metanetMapsList.Margin = new System.Windows.Forms.Padding(2);
            this.metanetMapsList.Name = "metanetMapsList";
            this.metanetMapsList.Size = new System.Drawing.Size(302, 251);
            this.metanetMapsList.TabIndex = 0;
            // 
            // installMetanetMAp
            // 
            this.installMetanetMAp.Location = new System.Drawing.Point(111, 265);
            this.installMetanetMAp.Margin = new System.Windows.Forms.Padding(2);
            this.installMetanetMAp.Name = "installMetanetMAp";
            this.installMetanetMAp.Size = new System.Drawing.Size(100, 34);
            this.installMetanetMAp.TabIndex = 4;
            this.installMetanetMAp.Text = "Install";
            this.installMetanetMAp.UseVisualStyleBackColor = true;
            // 
            // tabLocalBackups
            // 
            this.tabLocalBackups.Controls.Add(this.installBackupMap);
            this.tabLocalBackups.Controls.Add(this.localEditorMaps);
            this.tabLocalBackups.Controls.Add(this.localEditorLevelsBackupsList);
            this.tabLocalBackups.Location = new System.Drawing.Point(4, 22);
            this.tabLocalBackups.Margin = new System.Windows.Forms.Padding(2);
            this.tabLocalBackups.Name = "tabLocalBackups";
            this.tabLocalBackups.Padding = new System.Windows.Forms.Padding(2);
            this.tabLocalBackups.Size = new System.Drawing.Size(321, 310);
            this.tabLocalBackups.TabIndex = 1;
            this.tabLocalBackups.Text = "Local Backups";
            this.tabLocalBackups.UseVisualStyleBackColor = true;
            // 
            // deleteSelectedMaps
            // 
            this.deleteSelectedMaps.Location = new System.Drawing.Point(473, 312);
            this.deleteSelectedMaps.Margin = new System.Windows.Forms.Padding(2);
            this.deleteSelectedMaps.Name = "deleteSelectedMaps";
            this.deleteSelectedMaps.Size = new System.Drawing.Size(74, 39);
            this.deleteSelectedMaps.TabIndex = 3;
            this.deleteSelectedMaps.Text = "Delete Selected";
            this.deleteSelectedMaps.UseVisualStyleBackColor = true;
            // 
            // backupSelectedMaps
            // 
            this.backupSelectedMaps.Location = new System.Drawing.Point(357, 312);
            this.backupSelectedMaps.Margin = new System.Windows.Forms.Padding(2);
            this.backupSelectedMaps.Name = "backupSelectedMaps";
            this.backupSelectedMaps.Size = new System.Drawing.Size(76, 39);
            this.backupSelectedMaps.TabIndex = 2;
            this.backupSelectedMaps.Text = "Backup Selected";
            this.backupSelectedMaps.UseVisualStyleBackColor = true;
            this.backupSelectedMaps.Click += new System.EventHandler(this.backupSelectedMaps_Click);
            // 
            // listEditorMaps
            // 
            this.listEditorMaps.FormattingEnabled = true;
            this.listEditorMaps.Location = new System.Drawing.Point(357, 24);
            this.listEditorMaps.Margin = new System.Windows.Forms.Padding(2);
            this.listEditorMaps.Name = "listEditorMaps";
            this.listEditorMaps.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listEditorMaps.Size = new System.Drawing.Size(190, 277);
            this.listEditorMaps.TabIndex = 1;
            this.listEditorMaps.SelectedIndexChanged += new System.EventHandler(this.listEditorMaps_SelectedIndexChanged);
            // 
            // tabMapPacks
            // 
            this.tabMapPacks.Location = new System.Drawing.Point(4, 22);
            this.tabMapPacks.Margin = new System.Windows.Forms.Padding(2);
            this.tabMapPacks.Name = "tabMapPacks";
            this.tabMapPacks.Size = new System.Drawing.Size(558, 375);
            this.tabMapPacks.TabIndex = 5;
            this.tabMapPacks.Text = "Map Packs";
            this.tabMapPacks.UseVisualStyleBackColor = true;
            // 
            // localEditorLevelsBackupsList
            // 
            this.localEditorLevelsBackupsList.FormattingEnabled = true;
            this.localEditorLevelsBackupsList.Location = new System.Drawing.Point(5, 5);
            this.localEditorLevelsBackupsList.Name = "localEditorLevelsBackupsList";
            this.localEditorLevelsBackupsList.Size = new System.Drawing.Size(151, 251);
            this.localEditorLevelsBackupsList.TabIndex = 0;
            this.localEditorLevelsBackupsList.SelectedIndexChanged += new System.EventHandler(this.localEditorLevelsBackupsList_SelectedIndexChanged);
            // 
            // localEditorMaps
            // 
            this.localEditorMaps.FormattingEnabled = true;
            this.localEditorMaps.Location = new System.Drawing.Point(162, 5);
            this.localEditorMaps.Name = "localEditorMaps";
            this.localEditorMaps.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.localEditorMaps.Size = new System.Drawing.Size(154, 251);
            this.localEditorMaps.TabIndex = 1;
            // 
            // installBackupMap
            // 
            this.installBackupMap.Location = new System.Drawing.Point(196, 266);
            this.installBackupMap.Name = "installBackupMap";
            this.installBackupMap.Size = new System.Drawing.Size(85, 33);
            this.installBackupMap.TabIndex = 2;
            this.installBackupMap.Text = "Install";
            this.installBackupMap.UseVisualStyleBackColor = true;
            this.installBackupMap.Click += new System.EventHandler(this.installBackupMap_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 419);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "N++ Assistant";
            this.tabControl1.ResumeLayout(false);
            this.tabStatus.ResumeLayout(false);
            this.tabStatus.PerformLayout();
            this.tabProfile.ResumeLayout(false);
            this.tabProfile.PerformLayout();
            this.tabSoundpacks.ResumeLayout(false);
            this.tabSoundpacks.PerformLayout();
            this.tabPalettes.ResumeLayout(false);
            this.tabPalettes.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.metanetPalettes.ResumeLayout(false);
            this.communityPalettes.ResumeLayout(false);
            this.localBackupPalettes.ResumeLayout(false);
            this.tabEditorMaps.ResumeLayout(false);
            this.tabControlEditorMaps.ResumeLayout(false);
            this.tabMetanetMaps.ResumeLayout(false);
            this.tabLocalBackups.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabStatus;
        private System.Windows.Forms.LinkLabel steamInstallDir;
        private System.Windows.Forms.TabPage tabProfile;
        private System.Windows.Forms.TabPage tabSoundpacks;
        private System.Windows.Forms.TabPage tabPalettes;
        private System.Windows.Forms.TabPage tabEditorMaps;
        private System.Windows.Forms.TabPage tabMapPacks;
        private System.Windows.Forms.LinkLabel profileDir;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Button backupNow;
        private System.Windows.Forms.Button backupProfile;
        private System.Windows.Forms.ListBox profileList;
        private System.Windows.Forms.LinkLabel backupsDir;
        private System.Windows.Forms.LinkLabel screenshotsDir;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label progressLabel;
        private System.Windows.Forms.Button loadProfile;
        private System.Windows.Forms.Label profileBackupLabel;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button deleteProfile;
        private System.Windows.Forms.ListBox palettesInstalledList;
        private System.Windows.Forms.Button uninstallPalette;
        private System.Windows.Forms.Button backupPalette;
        private System.Windows.Forms.LinkLabel palettesInstalledLinkedLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage metanetPalettes;
        private System.Windows.Forms.ListBox metanetPalettesList;
        private System.Windows.Forms.TabPage communityPalettes;
        private System.Windows.Forms.ListBox communityPalettesList;
        private System.Windows.Forms.TabPage localBackupPalettes;
        private System.Windows.Forms.ListBox localBackupPalettesList;
        private System.Windows.Forms.Button installCommunityPalette;
        private System.Windows.Forms.Button installMetanetPalette;
        private System.Windows.Forms.Button deleteBackupPalette;
        private System.Windows.Forms.Button installBackupPalette;
        private System.Windows.Forms.Label countCustomPalettesInstalled;
        private System.Windows.Forms.LinkLabel onlineEditorLink;
        private System.Windows.Forms.LinkLabel linkProfileFolder;
        private System.Windows.Forms.LinkLabel linkBackupProfileFolder;
        private System.Windows.Forms.Button installSpreadsheetSoundpack;
        private System.Windows.Forms.Button deleteSoundpackBackupButton;
        private System.Windows.Forms.Button installSoundpackButton;
        private System.Windows.Forms.ListBox soundpackBackups;
        private System.Windows.Forms.Button backupSoundpack;
        private System.Windows.Forms.ListBox spreadsheetSoundpacks;
        private System.Windows.Forms.Label previewSoundsLabel;
        private System.Windows.Forms.ListBox previewSoundsList;
        private System.Windows.Forms.ListBox metanetMapsList;
        private System.Windows.Forms.TabControl tabControlEditorMaps;
        private System.Windows.Forms.TabPage tabMetanetMaps;
        private System.Windows.Forms.Button installMetanetMAp;
        private System.Windows.Forms.TabPage tabLocalBackups;
        private System.Windows.Forms.Button deleteSelectedMaps;
        private System.Windows.Forms.Button backupSelectedMaps;
        private System.Windows.Forms.ListBox listEditorMaps;
        private System.Windows.Forms.Button installBackupMap;
        private System.Windows.Forms.ListBox localEditorMaps;
        private System.Windows.Forms.ListBox localEditorLevelsBackupsList;
    }
}

