using Microsoft.Win32;
using System;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using Newtonsoft.Json;
using Popcron.Sheets;
using System.Globalization;
using System.Threading;
using System.Collections;
using System.Text;

namespace N__Assistant
{
    public partial class Form1 : Form
    {
        // more robust autodetect added by YupdanielThatsMe#6492
        private string steamGamePath;
        //private string profilePath = Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%") + @"\Documents\Metanet\N++";
        private string profilePath = (string)Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Shell Folders", "Personal", "null") + @"\Metanet\N++";
        private string screenshotsPath = @"\userdata\64929984\760\remote\230270\screenshots";
        //private string savePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\N++Assistant";
        private string savePath = (string)Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Shell Folders", "Local AppData", "null") + @"\N++Assistant";

        static Thread MyLoadingThread = null;
        static Thread MyReadingTextThread = null;
        private BackgroundWorker bgwBackupNow = new BackgroundWorker();

        // there is probably a better way to map this but fuck me if i know C# properly
        private List<sheetMap> sheetMapList = new List<sheetMap>();
        private string COMMUNITY_PALETTES = "1I2f87Qhfs6rxzZq5dQRDbLKYyaGLqTdCkLqfNfrw1Mk";
        private string COMMUNITY_SOUNDPACKS = "18PshamVuDNyH396a7U3YDFQmCw18s4gIVZ_WrFODRd4";
        //private string COMMUNITY_MAPPACKS = "1M9W3_jk3nULledALJNzRDRRpNhIofeTD2SF8ES6vCy8";
        private string COMMUNITY_MAPPACKS = "18PshamVuDNyH396a7U3YDFQmCw18s4gIVZ_WrFODRd4";


        public Form1()
        {
            try
            {
                InitializeComponent();

                // get rid of "The request was aborted: Could not create SSL/TLS secure channel" error that happens on some versions of windows
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                // get steam path
                string steampath = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Valve\\Steam", "InstallPath", "null");
                if (steampath == "null")
                {
                    steampath = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Valve\\Steam", "InstallPath", "null");
                }
                if (steampath == "null")
                {
                    throw new FileNotFoundException("Steam Not Found, WTF Bro is your pc good?");
                }
                if (!Directory.Exists(steampath + "\\steamapps\\"))
                {
                    throw new FileNotFoundException("steamapps Not Found");
                }
                screenshotsPath = steampath + screenshotsPath;
                string[] configLines = File.ReadAllLines(steampath + "\\steamapps\\libraryfolders.vdf");
                List<string> possiblepaths = new List<string>() { steampath };
                foreach (string configline in configLines)
                {
                    if (configline.Contains("\t\t\"path\"\t\t\""))
                    {
                        possiblepaths.Add(configline.Replace("\t\t\"path\"\t\t\"", "").Replace("\\\\", "\\").Replace("\"", ""));
                    }
                }
                foreach (string possiblepath in possiblepaths)
                {
                    if (Directory.Exists(possiblepath + "\\steamapps\\common\\N++"))
                    {
                        steamGamePath = possiblepath + "\\steamapps\\common\\N++";
                        break;
                    }
                }
                if (steamGamePath == "")
                {
                    throw new FileNotFoundException("N++ not installed in steam");
                }

                // create backup directories if they dont exist
                if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);
                if (!Directory.Exists(savePath + @"\Profiles")) Directory.CreateDirectory(savePath + @"\Profiles");
                if (!Directory.Exists(savePath + @"\Sounds")) Directory.CreateDirectory(savePath + @"\Sounds");
                if (!Directory.Exists(savePath + @"\Replays")) Directory.CreateDirectory(savePath + @"\Replays");
                if (!Directory.Exists(savePath + @"\Levels")) Directory.CreateDirectory(savePath + @"\Levels");
                if (!Directory.Exists(savePath + @"\Maps")) Directory.CreateDirectory(savePath + @"\Maps");
                if (!Directory.Exists(savePath + @"\Palettes")) Directory.CreateDirectory(savePath + @"\Palettes");
                if (!Directory.Exists(savePath + @"\MapPacks")) Directory.CreateDirectory(savePath + @"\MapPacks");
                if (!Directory.Exists(savePath + @"\NPPDLL")) Directory.CreateDirectory(savePath + @"\NPPDLL");

                MyLoadingThread = new Thread(new ThreadStart(DownloadStuff));
                MyLoadingThread.IsBackground = true;
                MyLoadingThread.Start();

                // create palettes directory in steam game dir if it doesnt exist (it's needed to install the custom palettes)
                if (!Directory.Exists(steamGamePath + @"\NPP\Palettes")) Directory.CreateDirectory(steamGamePath + @"\NPP\Palettes");

                // copy npp.dll into \NPPDLL if it doesn't exist
                if (!File.Exists(savePath + @"\NPPDLL\npp.dll")) File.Copy(steamGamePath + @"\npp.dll", savePath + @"\NPPDLL\npp.dll");

                MyReadingTextThread = new Thread(new ThreadStart(ReadNPPTextLogs));
                MyReadingTextThread.IsBackground = true;
                MyReadingTextThread.Start();

                //TODO: save user settings and reload the same on launch
                // set checkbox on profile backup default on
                checkedListBox1.SetItemChecked(0, true);
                // set checkbox on level editor backup default on
                checkedListBox1.SetItemChecked(2, true);

                // initialize background worker for backup now button
                bgwBackupNow.DoWork += new DoWorkEventHandler(bgwBackupNow_DoWork);
                bgwBackupNow.ProgressChanged += new ProgressChangedEventHandler(bgwBackupNow_ProgressChanged);
                bgwBackupNow.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwBackupNow_RunWorkerCompleted);
                bgwBackupNow.WorkerReportsProgress = true;

                tabControl1.Selecting += new TabControlCancelEventHandler(tabControl1_Selecting);

                updateHighDPIFixButton();
                tabControlDebugTabs.Selecting += new TabControlCancelEventHandler(tabControlLogDebug_Selecting);

            }
            catch (Exception exc) {
                MessageBox.Show("Coulnd't initialize because: " + exc.Message);
            }
        }

        private void DownloadStuff()
        {
            try {
                // download default Metanet Palettes.zip pack to backup dir
                if (!Directory.Exists(savePath + @"\Palettes\Palettes"))
                {
                    try
                    {
                        string myStringWebResource = "https://cdn.discordapp.com/attachments/197793786389200896/592821804746276864/Palettes.zip";
                        WebClient myWebClient = new WebClient();
                        string filename = savePath + @"\Palettes\Palettes.zip";
                        myWebClient.DownloadFile(myStringWebResource, filename);
                        myWebClient.Dispose();
                        ZipFile.ExtractToDirectory(filename, savePath + @"\Palettes");
                        File.Delete(savePath + @"\Palettes\Palettes.zip");
                        statusLabel.Text = "Done downloading Metanet allpalettes.zip";
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("Couldn't download Metanet Palettes pack because: " + exc.Message);
                    }
                }

                // download NPP_AllLevels.zip to backup dir
                if (!Directory.Exists(savePath + @"\Maps\NPP_AllLevels"))
                {
                    try
                    {
                        string myStringWebResource = "https://cdn.discordapp.com/attachments/592913929630384138/890428300911050752/NPP_AllLevels.zip";
                        WebClient myWebClient = new WebClient();
                        string filename = savePath + @"\Maps\NPP_AllLevels.zip";
                        myWebClient.DownloadFile(myStringWebResource, filename);
                        myWebClient.Dispose();
                        Directory.CreateDirectory(savePath + @"\Maps\NPP_AllLevels");
                        ZipFile.ExtractToDirectory(filename, savePath + @"\Maps\NPP_AllLevels");
                        File.Delete(savePath + @"\Maps\NPP_AllLevels.zip");
                        statusLabel.Text = "Done downloading NPP_AllLevels.zip";
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("Couldn't download NPP_AllLevels.zip because: " + exc.Message);
                    }
                }

                // list spreadsheet of new sound packs
                // https://docs.google.com/spreadsheets/d/18PshamVuDNyH396a7U3YDFQmCw18s4gIVZ_WrFODRd4/edit#gid=0
                if (spreadsheetSoundpacks.Items.Count == 0)
                {
                    //spreadsheetSoundpacks.Items.Clear();
                    PopulateListBoxWithSpreadsheetData(spreadsheetSoundpacks, 0, COMMUNITY_SOUNDPACKS, new APIKey().key, "Sound Packs");
                    installSpreadsheetSoundpack.Enabled = false;
                    statusLabel.Text = "Getting community sound packs spreadsheet data ...";
                }

                // populate community palettes from spreadsheet link
                // https://docs.google.com/spreadsheets/d/1I2f87Qhfs6rxzZq5dQRDbLKYyaGLqTdCkLqfNfrw1Mk/edit#gid=0
                if (communityPalettesList.Items.Count == 0)
                {
                    //communityPalettesList.Items.Clear();
                    PopulateListBoxWithSpreadsheetData(communityPalettesList, 0, COMMUNITY_PALETTES, new APIKey().key, "Palettes");
                    installCommunityPalette.Enabled = false;
                    statusLabel.Text = "Getting community palettes spreadsheet data ...";
                }

                // populate community map packs from spreadsheet link
                // https://docs.google.com/spreadsheets/d/1M9W3_jk3nULledALJNzRDRRpNhIofeTD2SF8ES6vCy8/edit#gid=0
                // https://docs.google.com/spreadsheets/d/18PshamVuDNyH396a7U3YDFQmCw18s4gIVZ_WrFODRd4/edit#gid=1470738075
                if (communityMapPacksList.Items.Count == 0)
                {
                    //communityMapPacksList.Items.Clear();
                    PopulateListBoxWithSpreadsheetData(communityMapPacksList, 2, COMMUNITY_MAPPACKS, new APIKey().key, "Map Packs");
                    installCommunityMapPack.Enabled = false;
                    patchLeaderboardsForMapPack.Enabled = false;
                    statusLabel.Text = "Getting community map packs spreadsheet data ...";
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Coulnd't download because: " + exc.Message);
            }
        }

        private string Readnppconf()
        {
            string nppconfText_Text = "";
            try
            {
                nppconfText_Text = File.ReadAllText(profilePath + @"\npp.conf");
            }
            catch (Exception)
            {
                try
                {
                    nppconfText_Text = File.ReadAllText(steamGamePath + @"\NPP\npp.conf");
                }
                catch (Exception exc2)
                {
                    MessageBox.Show("Couldn't read any npp.conf! \n" + exc2.Message);
                }
            }
            return nppconfText_Text;
        }
        private string Readkeysvars()
        {
            string text = "";
            try
            {
                text = File.ReadAllText(profilePath + @"\keys.vars");
            }
            catch (Exception)
            {
                try
                {
                    text = File.ReadAllText(steamGamePath + @"\NPP\keys.vars");
                }
                catch (Exception exc2)
                {
                    MessageBox.Show("Couldn't read any keys.vars! \n" + exc2.Message);
                }
            }
            return text;
        }
        private string Readnpplog()
        {
            string npplogText_Text = "";
            try
            {
                npplogText_Text = File.ReadAllText(profilePath + @"\NPPLog.txt");
            }
            catch (Exception exc)
            {
                MessageBox.Show("Couldn't read NPPLog.txt! \n" + exc.Message);
            }
            return npplogText_Text;
        }

        private void ReadNPPTextLogs()
        {
            string nppconfText_Text = Readnppconf();
            string npplogText_Text = Readnpplog();
            string keysvarsText_Text = Readkeysvars();

            if (InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate {
                    nppconfText.Text = nppconfText_Text;
                    npplogText.Text = npplogText_Text;
                    keysvarsText.Text = keysvarsText_Text;
                    statusLabel.Text = "Done loading NPP config and log files";
                }));
                return;
            }
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            TabPage current = (sender as TabControl).SelectedTab;

            // switch to status / home tab
            if (current == tabStatus)
            {

            }

            // switch to profile tab
            if (current == tabProfile)
            {
                profileList.Items.Clear();
                PopulateListBoxWithFileType(profileList, savePath + @"\Profiles", "*.zip");
                loadProfile.Enabled = false;
                deleteProfile.Enabled = false;
            }

            // switch to soundpacks tab
            if (current == tabSoundpacks)
            {
                // list backups
                soundpackBackups.Items.Clear();
                PopulateListBoxWithFileType(soundpackBackups, savePath + @"\Sounds", "*.zip");
                installSoundpackButton.Enabled = false;
                deleteSoundpackBackupButton.Enabled = false;

                // list preview
                previewSoundsList.Items.Clear();
                PopulateListBoxWithFileType(previewSoundsList, steamGamePath + @"\NPP\Sounds", "*.wav");
            }

            // switch to palettes tab
            if (current == tabPalettes)
            {
                // populate metanet palettes (useful only for auto-unlock and references when creating palettes)
                // https://cdn.discordapp.com/attachments/197793786389200896/592821804746276864/Palettes.zip
                metanetPalettesList.Items.Clear();
                PopulateListBoxWithSubDirectories(metanetPalettesList, savePath + @"\Palettes\Palettes");
                installMetanetPalette.Enabled = false;

                // list all palettes in local backup dir
                localBackupPalettesList.Items.Clear();
                PopulateListBoxWithFileType(localBackupPalettesList, savePath + @"\Palettes", "*.zip");
                installBackupPalette.Enabled = false;
                deleteBackupPalette.Enabled = false;

                // list installed palettes
                palettesInstalledList.Items.Clear();
                PopulateListBoxWithSubDirectories(palettesInstalledList, steamGamePath + @"\NPP\Palettes");
                uninstallPalette.Enabled = false;
                backupPalette.Enabled = false;

                updateCustomPalleteInstalledCounter();
            }

            // switch to practice maps tab
            if (current == tabMaps)
            {
                // list metanetMapsList
                // https://cdn.discordapp.com/attachments/197765375503368192/580483404533989396/NPP_AllLevels.zip
                string dir = savePath + @"\Maps\NPP_AllLevels";
                metanetMapsList.Nodes.Clear();
                LoadMapFiles(dir + @"\SI", "SI-", 5, 5, 0, metanetMapsList.Nodes.Add("Solo Intro"));
                LoadMapFiles(dir + @"\S", "S-", 20, 6, 0, metanetMapsList.Nodes.Add("Solo N++"));
                LoadMapFiles(dir + @"\S2", "SU-", 20, 6, 0, metanetMapsList.Nodes.Add("Solo Ultimate"));
                LoadMapFiles(dir + @"\SL", "SL-", 20, 6, 0, metanetMapsList.Nodes.Add("Solo Legacy"));
                LoadMapFiles(dir + @"\SL2", "SD-", 20, 6, 0, metanetMapsList.Nodes.Add("Solo Legacy Discarded"));
                LoadMapFiles(dir + @"\SS", "?-", 4, 6, 0, metanetMapsList.Nodes.Add("Solo ?"));
                LoadMapFiles(dir + @"\SS2", "!-", 4, 6, 0, metanetMapsList.Nodes.Add("Solo !"));
                LoadMapFiles(dir + @"\CI", "CI-", 2, 5, 0, metanetMapsList.Nodes.Add("Co-op Intro"));
                LoadMapFiles(dir + @"\C", "C-", 10, 6, 0, metanetMapsList.Nodes.Add("Co-op N++"));
                LoadMapFiles(dir + @"\C2", "CU-", 10, 6, 10, metanetMapsList.Nodes.Add("Co-op Ultimate"));
                LoadMapFiles(dir + @"\CL", "CL-", 5, 5, 0, metanetMapsList.Nodes.Add("Co-op Legacy"));
                LoadMapFiles(dir + @"\CL2", "CL-", 6, 6, 5, metanetMapsList.Nodes.Add("Co-op Legacy Ultimate"));
                LoadMapFiles(dir + @"\RI", "RI-", 1, 5, 0, metanetMapsList.Nodes.Add("Race Intro"));
                LoadMapFiles(dir + @"\R", "R-", 10, 5, 0, metanetMapsList.Nodes.Add("Race N++"));
                LoadMapFiles(dir + @"\R2", "RU-", 10, 5, 10, metanetMapsList.Nodes.Add("Race Ultimate"));
                LoadMapFiles(dir + @"\RL", "RL-", 10, 5, 0, metanetMapsList.Nodes.Add("Race Legacy"));
                LoadMapFiles(dir + @"\RL2", "RL-", 10, 5, 10, metanetMapsList.Nodes.Add("Race Legacy Ultimate"));
                installMetanetMap.Enabled = false;

                // list local backups
                localMapsBackupsList.Items.Clear();
                PopulateListBoxWithFileType(localMapsBackupsList, savePath + @"\Maps", "*.zip");
                installBackupMap.Enabled = false;

                // list editor maps
                RefreshListEditorMaps();
            }

            if (current == tabMapPacks)
            {
                // list local map packs backups
                localBackupsMapPacksList.Items.Clear();
                PopulateListBoxWithFileType(localBackupsMapPacksList, savePath + @"\MapPacks", "*.zip");
                renameLocalBackupMapPack.Enabled = false;
                installLocalBackupMapPack.Enabled = false;
                deleteLocalBackupMapPack.Enabled = false;
                installLocalBackupMapPackWithProfile.Enabled = false;

                // list local profile backups
                profileMapBackupList.Items.Clear();
                PopulateListBoxWithFileType(profileMapBackupList, savePath + @"\Profiles", "*.zip");
                renameProfileBackup.Enabled = false;
                installBackupMapPackProfile.Enabled = false;
                deleteBackupMapPackProfile.Enabled = false;
            }
        }

        private void steamGamePath_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            launchExplorer(steamGamePath);
        }

        private void profileLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            launchExplorer(profilePath);
        }

        private void backupNow_Click(object sender, EventArgs e)
        {
            if (DetectNPPRunning() == true)
            {
                MessageBox.Show("Please close N++ before doing backup");
                return;
            }
            backupNow.Enabled = false;
            //progressBar1.Show();
            bgwBackupNow.RunWorkerAsync();
        }

        private void bgwBackupNow_DoWork(object sender, DoWorkEventArgs e)
        {
            bgwBackupNow.ReportProgress(0, "Zipping nprofile");
            if (checkedListBox1.GetItemCheckState(0) == CheckState.Checked)
            {
                // backup profile
                using (FileStream fs = new FileStream(savePath + @"\Profiles\nprofile" + DateTime.Now.ToString("yyMMddHHmm") + ".zip", FileMode.Create))
                using (ZipArchive arch = new ZipArchive(fs, ZipArchiveMode.Create))
                {
                    arch.CreateEntryFromFile(profilePath + @"\nprofile", "nprofile");
                }
            }
            bgwBackupNow.ReportProgress(15, "Zipping Soundpack");
            if (checkedListBox1.GetItemCheckState(1) == CheckState.Checked)
            {
                // backup soundpack
                ZipFile.CreateFromDirectory(steamGamePath + @"\NPP\Sounds", savePath + @"\Sounds\Sounds" + DateTime.Now.ToString("yyMMddHHmm") + ".zip");
            }
            bgwBackupNow.ReportProgress(30, "Zipping Editor Levels");
            if (checkedListBox1.GetItemCheckState(2) == CheckState.Checked)
            {
                // backup editor levels
                ZipFile.CreateFromDirectory(profilePath + @"\levels", savePath + @"\Maps\Maps" + DateTime.Now.ToString("yyMMddHHmm") + ".zip");
            }
            bgwBackupNow.ReportProgress(40, "Zipping Replays (attract files)");
            if (checkedListBox1.GetItemCheckState(3) == CheckState.Checked)
            {
                // backup replays (attract files)
                ZipFile.CreateFromDirectory(profilePath + @"\attract", savePath + @"\Replays\Replays" + DateTime.Now.ToString("yyMMddHHmm") + ".zip");
            }
            bgwBackupNow.ReportProgress(60, "Zipping Palettes");
            if (checkedListBox1.GetItemCheckState(4) == CheckState.Checked)
            {
                // backup palettes
                //ZipFile.CreateFromDirectory(steamGamePath + @"\NPP\Palettes", savePath + @"\Palettes\Palettes" + DateTime.Now.ToString("yyMMddHHmm") + ".zip");
                try
                {
                    string[] dirs = Directory.GetDirectories(steamGamePath + @"\NPP\Palettes");
                    foreach (string dir in dirs)
                    {
                        string[] splits = dir.Split('\\');
                        string palName = splits[splits.Length - 1];
                        ZipFile.CreateFromDirectory(dir, savePath + @"\Palettes\" + palName + DateTime.Now.ToString("yyMMddHHmm") + ".zip");
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Failed to zip palettes: {0}", exc.ToString());
                }
            }
            bgwBackupNow.ReportProgress(80, "Zipping Game Levels");
            if (checkedListBox1.GetItemCheckState(5) == CheckState.Checked)
            {
                // backup map packs
                ZipFile.CreateFromDirectory(steamGamePath + @"\NPP\Levels", savePath + @"\MapPacks\MapPacks" + DateTime.Now.ToString("yyMMddHHmm") + ".zip");
            }
            bgwBackupNow.ReportProgress(100, "Done with backup!");
        }

        private void bgwBackupNow_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            statusLabel.Text = String.Format("{0}", e.UserState);
        }

        private void bgwBackupNow_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            statusLabel.Text = "Done with backup!";
            //progressBar.Hide();
            backupNow.Enabled = true;
        }

        private void savePathLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Directory.Exists(savePath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = savePath,
                    FileName = "explorer.exe"
                };

                Process.Start(startInfo);
            }
            else
            {
                MessageBox.Show(string.Format("{0} Directory does not exist!", savePath));
            }
        }

        private void screenshotsPathLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Directory.Exists(screenshotsPath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = screenshotsPath,
                    FileName = "explorer.exe"
                };

                Process.Start(startInfo);
            }
            else
            {
                MessageBox.Show(string.Format("{0} Directory does not exist!", screenshotsPath));
            }
        }

        private void PopulateListBoxWithFileType(ListBox lsb, string Folder, string FileType)
        {
            try
            {
                DirectoryInfo dinfo = new DirectoryInfo(Folder);
                FileInfo[] Files = dinfo.GetFiles(FileType);
                foreach (FileInfo file in Files)
                {
                    lsb.Items.Add(file.Name + " (" + file.Length / 1024 + "Kb)");
                }
            }
            catch (System.IO.DirectoryNotFoundException exc) {
                MessageBox.Show(exc.Message);
            }

        }

        private void PopulateListBoxWithSubDirectories(ListBox lsb, string Folder)
        {
            DirectoryInfo dinfo = new DirectoryInfo(Folder);
            DirectoryInfo[] dirs = dinfo.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                // yyyy/MM/dd format is coherent with what's coming from google sheets
                lsb.Items.Add(dir.Name + " (" + dir.LastWriteTime.ToString("yyyy/MM/dd") + ")");
            }
        }

        private bool DetectNPPRunning()
        {
            Process[] pname = Process.GetProcessesByName("N++");
            if (pname.Length == 0)
                return false;
            else
                return true;
        }

        private void backupProfile_Click(object sender, EventArgs e)
        {
            statusLabel.Text = "Profile backup started!";

            using (FileStream fs = new FileStream(savePath + @"\Profiles\nprofile" + DateTime.Now.ToString("yyMMddHHmm") + ".zip", FileMode.Create))
            using (ZipArchive arch = new ZipArchive(fs, ZipArchiveMode.Create))
            {
                arch.CreateEntryFromFile(profilePath + @"\nprofile", "nprofile");
            }
            statusLabel.Text = "Profile backup completed!";

            profileList.Items.Clear();
            PopulateListBoxWithFileType(profileList, savePath + @"\Profiles", "*.zip");
        }

        private void profileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadProfile.Enabled = true;
            deleteProfile.Enabled = true;
        }

        private void loadProfileBackup_Click(object sender, EventArgs e)
        {
            if (DetectNPPRunning() == true)
            {
                MessageBox.Show("Please close N++ before installing a profile backup");
                statusLabel.Text = "Aborted loading profile because N++ was running.";
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Are you sure you wish to replace your game profile with the selected backup? This process is irreversible if you haven't done a recent backup.", "Replace Existing nprofile?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                File.Delete(profilePath + @"\nprofile");
                string zipPath = savePath + @"\Profiles\" + profileList.SelectedItem.ToString().Substring(0, profileList.SelectedItem.ToString().LastIndexOf(' ')).TrimEnd();
                string extractPath = profilePath + @"\nprofile";
                using (ZipArchive archive = ZipFile.OpenRead(zipPath))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (entry.Name.Equals("nprofile"))
                        {
                            entry.ExtractToFile(extractPath, true);
                            statusLabel.Text = "Replaced current N++ game profile with " + profileList.SelectedItem.ToString().Substring(0, profileList.SelectedItem.ToString().LastIndexOf(' ')).TrimEnd();
                        }
                    }
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                // do nothing
            }
        }

        private void deleteProfileBackup_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you wish to delete the selected backup? This process is irreversible.", "Delete Selected File?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    string filename = profileList.SelectedItem.ToString().Split(' ')[0];
                    File.Delete(savePath + @"\Profiles\" + filename);
                    profileList.Items.Remove(profileList.SelectedItem);
                    loadProfile.Enabled = false;
                    deleteProfile.Enabled = false;
                    statusLabel.Text = "Deleted profile backup " + filename;
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                    statusLabel.Text = "Exception trying to delete profile backup " + profileList.SelectedItem.ToString().Split(' ')[0];
                }
            }
        }

        private void uninstallPalette_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you wish to uninstall the selected palette? This process is irreversible if you haven't done a recent backup.", "Uninstall Palette?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    string palName = palettesInstalledList.SelectedItem.ToString().Split(' ')[0];
                    Directory.Delete(steamGamePath + @"\NPP\Palettes\" + palName, true);
                    palettesInstalledList.Items.Remove(palettesInstalledList.SelectedItem);

                    uninstallPalette.Enabled = false;
                    backupPalette.Enabled = false;

                    updateCustomPalleteInstalledCounter();
                    statusLabel.Text = "Done uninstalling " + palName;

                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }

            }
        }

        private void palettesInstalled_SelectedIndexChanged(object sender, EventArgs e)
        {
            uninstallPalette.Enabled = true;
            backupPalette.Enabled = true;
        }

        private void backupPalette_Click(object sender, EventArgs e)
        {
            // backup the selected palette only
            string palName = palettesInstalledList.SelectedItem.ToString().Substring(0, palettesInstalledList.SelectedItem.ToString().Length - 13);
            ZipFile.CreateFromDirectory(steamGamePath + @"\NPP\Palettes\" + palName, savePath + @"\Palettes\" + palName + DateTime.Now.ToString("yyMMddHHmm") + ".zip");

            // update local backup palettes list
            localBackupPalettesList.Items.Clear();
            PopulateListBoxWithFileType(localBackupPalettesList, savePath + @"\Palettes", "*.zip");
            installBackupPalette.Enabled = false;
            deleteBackupPalette.Enabled = false;

            // notify backup was done
            backupPalette.Enabled = false;
            statusLabel.Text = "Done with backup of palette " + palName;
        }

        private void palettesInstalledLinkedLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            launchExplorer(steamGamePath + @"\NPP\Palettes\");
        }

        public class JSONSerializer : SheetsSerializer
        {
            public override T DeserializeObject<T>(string data)
            {
                return JsonConvert.DeserializeObject<T>(data);
            }

            public override string SerializeObject(object data)
            {
                return JsonConvert.SerializeObject(data);
            }
        }

        private async void PopulateListBoxWithSpreadsheetData(ListBox lsb, int sheetsId, string spreadsheetId, string key, string sht)
        {
            // test url https://sheets.googleapis.com/v4/spreadsheets/spreadsheetid?key=key

            try
            {
                SheetsSerializer.Serializer = new JSONSerializer();
                Popcron.Sheets.Authorization authorization = await Popcron.Sheets.Authorization.Authorize(key);
                Spreadsheet spreadsheet = await Spreadsheet.Get(spreadsheetId, authorization);
                Sheet sheet = spreadsheet.Sheets[sheetsId];
                Cell[,] data = sheet.Data;

                for (int y = 1; y < sheet.Rows; y++)
                {
                    string parsedDate = data[2, y].Value;
                    if ((data[2, y].Value != null) && (data[2, y].Value.Split('/').Length > 3))
                    {
                        parsedDate = data[2, y].Value.Split('/')[2] + "/" + data[2, y].Value.Split('/')[1] + "/" + data[2, y].Value.Split('/')[0];
                    }
                    if (data[0, y].Value != null && data[0, y].Value != "")
                        lsb.Items.Add(data[0, y].Value + " by " + data[1, y].Value + " (" + parsedDate + ")");
                }

                bool exists = false;
                foreach (var mapSheet in sheetMapList)
                {
                    if (mapSheet.sheetId.Equals(spreadsheetId) == true)
                    {
                        mapSheet.sheetData = sheet;
                        exists = true;
                    }
                }
                if (exists == false)
                {
                    sheetMapList.Add(new sheetMap(spreadsheetId, sheet));
                }

                statusLabel.Text = "Done retrieving spreadsheet data " + sht;

            }
            catch (System.Net.WebException webexc)
            {
                MessageBox.Show(webexc.Message);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void metanetPalettesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            installMetanetPalette.Enabled = true;
        }

        private void installMetanetPalette_Click(object sender, EventArgs e)
        {
            string palName = metanetPalettesList.SelectedItem.ToString().Substring(0, metanetPalettesList.SelectedItem.ToString().Length - 13);

            // check if it already exists
            if (Directory.Exists(steamGamePath + @"\NPP\Palettes\" + palName))
            {
                DialogResult dialogResult = MessageBox.Show("A palette already exists with this name, would you like to replace it?", "Replace Existing?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Directory.Delete(steamGamePath + @"\NPP\Palettes\" + palName, true);
                    DirectoryCopy(savePath + @"\Palettes\Palettes\" + palName, steamGamePath + @"\NPP\Palettes\" + palName, false);
                    palettesInstalledList.Items.Clear();
                    PopulateListBoxWithSubDirectories(palettesInstalledList, steamGamePath + @"\NPP\Palettes");
                    installMetanetPalette.Enabled = false;
                    statusLabel.Text = "Done installing palette " + palName;
                }
            }
            else
            {
                DirectoryCopy(savePath + @"\Palettes\Palettes\" + palName, steamGamePath + @"\NPP\Palettes\" + palName, false);
                palettesInstalledList.Items.Clear();
                PopulateListBoxWithSubDirectories(palettesInstalledList, steamGamePath + @"\NPP\Palettes");
                installMetanetPalette.Enabled = false;
                statusLabel.Text = "Done installing palette " + palName;
            }
        }

        private void communityPalettesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            installCommunityPalette.Enabled = true;
        }

        private void installCommunityPalette_Click(object sender, EventArgs e)
        {
            if (DetectNPPRunning() == true)
            {
                MessageBox.Show("Please close N++ before installing palettes");
                return;
            }

            try
            {
                string myStringWebResource = null;
                WebClient myWebClient = new WebClient();

                // get the url
                foreach (var mapSheet in sheetMapList)
                {
                    if (mapSheet.sheetId.Equals(COMMUNITY_PALETTES) == true)
                    {
                        Cell[,] data = mapSheet.sheetData.Data;
                        myStringWebResource = data[3, communityPalettesList.SelectedIndex + 1].raw.hyperlink;
                    }
                }

                // download the file and save it into the current filesystem folder.
                string filename = steamGamePath + @"\NPP\Palettes\" + "nppassisttemppal.zip";
                myWebClient.DownloadFile(myStringWebResource, filename);

                // check if palette has subdir in zip or not
                var zip = ZipFile.OpenRead(filename);
                var names = zip.Entries[0].FullName;
                zip.Dispose();
                if (names.ToString().StartsWith("background.tga"))
                {
                    // assume the palette should have same name as archive and create + extract into that directory
                    string dirname = myStringWebResource.Substring(myStringWebResource.LastIndexOf('/') + 1, myStringWebResource.LastIndexOf('.') - myStringWebResource.LastIndexOf('/') - 1).TrimEnd();

                    // check if it already exists
                    if (Directory.Exists(steamGamePath + @"\NPP\Palettes\" + dirname))
                    {
                        DialogResult dialogResult = MessageBox.Show("A palette already exists with this name, would you like to replace it?", "Replace Existing?", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            Directory.Delete(steamGamePath + @"\NPP\Palettes\" + dirname, true);
                            Directory.CreateDirectory(steamGamePath + @"\NPP\Palettes\" + dirname);
                            ZipFile.ExtractToDirectory(filename, steamGamePath + @"\NPP\Palettes\" + dirname);
                            File.Delete(filename);
                            statusLabel.Text = "Done replacing palette " + dirname;
                        }
                        else
                        {
                            // do nothing
                        }
                    }
                    else
                    {
                        Directory.CreateDirectory(steamGamePath + @"\NPP\Palettes\" + dirname);
                        ZipFile.ExtractToDirectory(filename, steamGamePath + @"\NPP\Palettes\" + dirname);
                        File.Delete(filename);
                    }
                } else {
                    // assume it has a subdir with same name, extract in root of palettes

                    if (Directory.Exists(steamGamePath + @"\NPP\Palettes\" + names))
                    {
                        DialogResult dialogResult = MessageBox.Show("A palette already exists with this name, would you like to replace it?", "Replace Existing?", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            Directory.Delete(steamGamePath + @"\NPP\Palettes\" + names, true);
                            ZipFile.ExtractToDirectory(filename, steamGamePath + @"\NPP\Palettes");
                            File.Delete(filename);
                            statusLabel.Text = "Done replacing palette " + names;
                        }
                        else
                        {
                            // do nothing
                        }
                    }
                    else
                    {
                        ZipFile.ExtractToDirectory(filename, steamGamePath + @"\NPP\Palettes");
                        File.Delete(filename);
                        statusLabel.Text = "Done installing palette " + filename;
                    }
                }

                // refresh installed palettes directory
                palettesInstalledList.Items.Clear();
                PopulateListBoxWithSubDirectories(palettesInstalledList, steamGamePath + @"\NPP\Palettes");
                installCommunityPalette.Enabled = false;

                updateCustomPalleteInstalledCounter();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Couldn't install community palette because: " + exc.Message);
            }
        }

        private void installBackupPalette_Click(object sender, EventArgs e)
        {
            string palName = localBackupPalettesList.SelectedItem.ToString().Substring(0, localBackupPalettesList.SelectedItem.ToString().LastIndexOf('.') - 14).TrimEnd();
            string filename = localBackupPalettesList.SelectedItem.ToString().Substring(0, localBackupPalettesList.SelectedItem.ToString().LastIndexOf(' ')).TrimEnd();

            // check if destination dir exists
            if (Directory.Exists(steamGamePath + @"\NPP\Palettes\" + palName))
            {
                // prompt asking if replacing existing dir or not
                DialogResult dialogResult = MessageBox.Show("A palette with this name is already installed, do you want to replace it?", "Replace Existing?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    // delete installed palette
                    Directory.Delete(steamGamePath + @"\NPP\Palettes\" + palName, true);

                    // install new palette from backup
                    Directory.CreateDirectory(steamGamePath + @"\NPP\Palettes\" + palName);
                    ZipFile.ExtractToDirectory(savePath + @"\Palettes\" + filename, steamGamePath + @"\NPP\Palettes\" + palName);

                    // refresh installed palettes list
                    palettesInstalledList.Items.Clear();
                    PopulateListBoxWithSubDirectories(palettesInstalledList, steamGamePath + @"\NPP\Palettes");
                    uninstallPalette.Enabled = false;
                    backupPalette.Enabled = false;

                    // notify it's done
                    installBackupPalette.Enabled = false;
                    statusLabel.Text = "Done replacing palette " + palName;
                }
                else if (dialogResult == DialogResult.No)
                {
                    // do nothing
                }
            }
            else
            {
                // install palette
                Directory.CreateDirectory(steamGamePath + @"\NPP\Palettes\" + palName);
                ZipFile.ExtractToDirectory(savePath + @"\Palettes\" + filename, steamGamePath + @"\NPP\Palettes\" + palName);

                // refresh installed palettes list
                palettesInstalledList.Items.Clear();
                PopulateListBoxWithSubDirectories(palettesInstalledList, steamGamePath + @"\NPP\Palettes");
                uninstallPalette.Enabled = false;
                backupPalette.Enabled = false;

                // notify it's done
                installBackupPalette.Enabled = false;

                updateCustomPalleteInstalledCounter();
                statusLabel.Text = "Done installing palette " + palName;
            }

        }

        private void deleteBackupPalette_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you wish to delete this backup? This process is irreversible.", "Delete backup?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string palName = localBackupPalettesList.SelectedItem.ToString().Substring(0, localBackupPalettesList.SelectedItem.ToString().LastIndexOf(' ')).TrimEnd();
                File.Delete(savePath + @"\Palettes\" + palName);

                // remove item instead of refreshing whole list, to keep position in view
                localBackupPalettesList.Items.Remove(localBackupPalettesList.SelectedItem);

                installBackupPalette.Enabled = false;
                deleteBackupPalette.Enabled = false;

                statusLabel.Text = "Done deleting backup " + palName;
            }
        }

        private void updateCustomPalleteInstalledCounter()
        {
            int count = 0;
            foreach (string item in palettesInstalledList.Items)
            {
                string f1 = item.Substring(0, item.LastIndexOf(' ')).TrimEnd();
                bool isMetanet = false;
                foreach (string item2 in metanetPalettesList.Items)
                {
                    string f2 = item2.Substring(0, item2.LastIndexOf(' ')).TrimEnd();
                    if (f1.Equals(f2)) isMetanet = true;
                }
                if (!isMetanet) count++;
            }

            countCustomPalettesInstalled.Text = "Custom: " + count.ToString();
            if (count > 132)
            {
                countCustomPalettesInstalled.ForeColor = System.Drawing.Color.Red;
                MessageBox.Show("You've reached maximum number of custom palettes the game can handle, here be dragons if you don't revert");
            }
            else
            {
                countCustomPalettesInstalled.ForeColor = System.Drawing.Color.Black;
            }

        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }

        private void localBackupPalettesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            installBackupPalette.Enabled = true;
            deleteBackupPalette.Enabled = true;
        }

        private void onlineEditorLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://edelkas.github.io/npc-web/");
        }

        private void linkBackupProfileFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            launchExplorer(savePath + @"\Profiles\");
        }

        private void linkProfileFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            launchExplorer(profilePath);
        }

        private void launchExplorer(string folder)
        {
            if (Directory.Exists(folder))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = folder,
                    FileName = "explorer.exe"
                };

                Process.Start(startInfo);
            }
            else
            {
                MessageBox.Show(string.Format("{0} Directory does not exist!", folder));
            }
        }

        private void backupSoundpack_Click(object sender, EventArgs e)
        {
            // backup current soundpack folder
            ZipFile.CreateFromDirectory(steamGamePath + @"\NPP\Sounds", savePath + @"\Sounds\Sounds" + DateTime.Now.ToString("yyMMddHHmm") + ".zip");
            soundpackBackups.Items.Clear();
            PopulateListBoxWithFileType(soundpackBackups, savePath + @"\Sounds", "*.zip");
            installSoundpackButton.Enabled = false;
            deleteSoundpackBackupButton.Enabled = false;
            statusLabel.Text = "Done backup of current sound pack!";
        }

        private void soundpackBackups_SelectedIndexChanged(object sender, EventArgs e)
        {
            installSoundpackButton.Enabled = true;
            deleteSoundpackBackupButton.Enabled = true;
        }

        private void spreadsheetSoundpacks_SelectedIndexChanged(object sender, EventArgs e)
        {
            installSpreadsheetSoundpack.Enabled = true;
        }

        private void installSpreadsheetSoundpack_Click(object sender, EventArgs e)
        {
            if (DetectNPPRunning() == true)
            {
                MessageBox.Show("Please close N++ before installing soundpack");
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Are you sure you wish to replace your current soundpack with this one? This process is irreversible if you haven't done a recent backup.", "Replace Existing Sounds?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    string myStringWebResource = null;
                    WebClient myWebClient = new WebClient();

                    // get the url
                    foreach (var mapSheet in sheetMapList)
                    {
                        if (mapSheet.sheetId.Equals(COMMUNITY_SOUNDPACKS) == true)
                        {
                            Cell[,] data = mapSheet.sheetData.Data;
                            myStringWebResource = data[3, spreadsheetSoundpacks.SelectedIndex + 1].Value;
                        }
                    }

                    // download the file and save it into the current filesystem folder.
                    string filename = steamGamePath + @"\NPP\" + "nppassisttempsounds.zip";
                    myWebClient.DownloadFile(myStringWebResource, filename);

                    // clean \Sounds\
                    Directory.Delete(steamGamePath + @"\NPP\Sounds", true);
                    Directory.CreateDirectory(steamGamePath + @"\NPP\Sounds");

                    // extract temp file
                    ZipFile.ExtractToDirectory(filename, steamGamePath + @"\NPP\Sounds");
                    File.Delete(filename);

                    // refresh installed soundpack directory
                    previewSoundsList.Items.Clear();
                    PopulateListBoxWithFileType(previewSoundsList, steamGamePath + @"\NPP\Sounds", "*.wav");

                    installSpreadsheetSoundpack.Enabled = false;
                    statusLabel.Text = "Done installing sound pack " + spreadsheetSoundpacks.SelectedItem.ToString();

                }
                catch (Exception exc)
                {
                    MessageBox.Show("Couldn't install community soundpack because: " + exc.Message);
                }
            }
        }

        private void installSoundpackButton_Click(object sender, EventArgs e)
        {
            if (DetectNPPRunning() == true)
            {
                MessageBox.Show("Please close N++ before installing soundpack");
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Are you sure you wish to replace your current soundpack with this one? This process is irreversible if you haven't done a recent backup.", "Replace Existing Sounds?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                // install
                string sfxName = soundpackBackups.SelectedItem.ToString().Substring(0, soundpackBackups.SelectedItem.ToString().LastIndexOf('.') - 14).TrimEnd();
                string filename = soundpackBackups.SelectedItem.ToString().Substring(0, soundpackBackups.SelectedItem.ToString().LastIndexOf(' ')).TrimEnd();
                Directory.Delete(steamGamePath + @"\NPP\Sounds", true);
                Directory.CreateDirectory(steamGamePath + @"\NPP\Sounds");
                ZipFile.ExtractToDirectory(savePath + @"\Sounds\" + filename, steamGamePath + @"\NPP\Sounds");

                // refresh soundpack preview
                previewSoundsList.Items.Clear();
                PopulateListBoxWithFileType(previewSoundsList, steamGamePath + @"\NPP\Sounds", "*.wav");

                // give some feedback it's done
                installSoundpackButton.Enabled = false;

                statusLabel.Text = "Done Installing Sound Pack!";
            }
        }

        private void deleteSoundpackBackupButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you wish to delete this sound pack backup? This process is irreversible.", "Delete Backup?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    File.Delete(savePath + @"\Sounds\" + soundpackBackups.SelectedItem.ToString().Split(' ')[0]);
                    soundpackBackups.Items.Remove(soundpackBackups.SelectedItem);
                    installSoundpackButton.Enabled = false;
                    deleteSoundpackBackupButton.Enabled = false;

                    statusLabel.Text = "Done Deleting Sound Pack Backup!";
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Couldn't delete backup because: " + exc.Message);
                }
            }
        }

        private void previewSoundsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(steamGamePath + @"\NPP\Sounds\" + previewSoundsList.SelectedItem.ToString().Split(' ')[0]);
            player.Play();
        }

        private void listEditorMaps_SelectedIndexChanged(object sender, EventArgs e)
        {
            backupSelectedMaps.Enabled = true;
            deleteSelectedMaps.Enabled = true;
        }

        private void backupSelectedMaps_Click(object sender, EventArgs e)
        {
            try
            {
                using (FileStream fs = new FileStream(savePath + @"\Maps\Maps" + DateTime.Now.ToString("yyMMddHHmm") + ".zip", FileMode.Create))
                using (ZipArchive arch = new ZipArchive(fs, ZipArchiveMode.Create))
                {
                    foreach (var item in listEditorMaps.SelectedItems)
                    {
                        string filename = item.ToString().Substring(0, item.ToString().LastIndexOf(' ')).TrimEnd();
                        arch.CreateEntryFromFile(profilePath + @"\levels\" + filename, filename);
                    }
                }

                // refresh localbackupslist
                localMapsBackupsList.Items.Clear();
                PopulateListBoxWithFileType(localMapsBackupsList, savePath + @"\Maps", "*.zip");
                installBackupMap.Enabled = false;

                // disable the button to avoid duplicate backups
                backupSelectedMaps.Enabled = false;

                statusLabel.Text = "Done backup of selected maps!";
            }
            catch (Exception exc)
            {
                MessageBox.Show("Something went wrong with the backup: " + exc.Message);
            }

        }

        private void localMapsBackupsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            localEditorMaps.Items.Clear();
            string filename = localMapsBackupsList.SelectedItem.ToString().Substring(0, localMapsBackupsList.SelectedItem.ToString().LastIndexOf(' ')).TrimEnd();
            string zipPath = savePath + @"\Maps\" + filename;
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    localEditorMaps.Items.Add(entry.FullName);
                }
            }
        }

        private void installBackupMap_Click(object sender, EventArgs e)
        {
            // disable install button to avoid dupes
            installBackupMap.Enabled = false;

            // multiple select
            // go through all selected
            foreach (var item in localEditorMaps.SelectedItems)
            {
                string mapName = item.ToString();
                string zipPath = savePath + @"\Maps\" + localMapsBackupsList.SelectedItem.ToString().Substring(0, localMapsBackupsList.SelectedItem.ToString().LastIndexOf(' ')).TrimEnd();
                string extractPath = profilePath + @"\levels\" + mapName;

                if (File.Exists(extractPath))
                {
                    DialogResult dialogResult = MessageBox.Show("A map already exists with this filename, do you want to replace it?", "Replace Existing?", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        using (ZipArchive archive = ZipFile.OpenRead(zipPath))
                        {
                            foreach (ZipArchiveEntry entry in archive.Entries)
                            {
                                if (entry.Name.Equals(mapName))
                                {
                                    entry.ExtractToFile(extractPath, true);
                                    statusLabel.Text = "Done replacing map " + mapName;
                                }
                            }
                        }
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        // do nothing
                    }
                }
                else
                {
                    using (ZipArchive archive = ZipFile.OpenRead(zipPath))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            if (entry.Name.Equals(mapName))
                            {
                                entry.ExtractToFile(extractPath);
                                statusLabel.Text = "Done installing map " + mapName;
                            }
                        }
                    }
                }
            }

            // refresh list of editor maps
            RefreshListEditorMaps();
        }

        private void metanetMapsList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (metanetMapsList.SelectedNode.Level == 1) installMetanetMap.Enabled = true;
            else installMetanetMap.Enabled = false;
        }

        /*private void LoadSubDirectories(string dir, TreeNode td)
        {
            // Get all subdirectories  
            string[] subdirectoryEntries = Directory.GetDirectories(dir);
            // Loop through them to see if they have any other subdirectories  
            foreach (string subdirectory in subdirectoryEntries)
            {
                DirectoryInfo di = new DirectoryInfo(subdirectory);
                string dirname = di.Name;
                if (di.Name == "C")
                {
                    dirname = "Co-op N++";
                }
                if (di.Name == "C2")
                {
                    dirname = "Co-op Ultimate";
                }
                if (di.Name == "CI")
                {
                    dirname = "Co-op Intro";
                }
                if (di.Name == "CL")
                {
                    dirname = "Co-op Legacy";
                }
                if (di.Name == "CL2")
                {
                    dirname = "Co-op Legacy Ultimate";
                }
                if (di.Name == "R")
                {
                    dirname = "Race N++";
                }
                if (di.Name == "R2")
                {
                    dirname = "Race Ultimate";
                }
                if (di.Name == "RI")
                {
                    dirname = "Race Intro";
                }
                if (di.Name == "RL")
                {
                    dirname = "Race Legacy";
                }
                if (di.Name == "RL2")
                {
                    dirname = "Race Legacy Ultimate";
                }
                TreeNode tds = td.Nodes.Add(dirname);
                tds.StateImageIndex = 0;
                tds.Tag = di.FullName;
                LoadFiles(subdirectory, tds);
                LoadSubDirectories(subdirectory, tds);
                //UpdateProgress();

            }
        }*/

        private void LoadMapFiles(string dir, string shortname, int cols, int rows, int startRowIndex, TreeNode td)
        {
            string[] Files = Directory.GetFiles(dir, "*.*");

            int counter = 0;
            int row = 0;
            int col = startRowIndex;

            // Loop through them to see files  
            foreach (string file in Files)
            {

                string rowchar = ((char)(65 + row)).ToString();
                if (row == 5) rowchar = "X";
                string idname = "[" + shortname + rowchar + "-" + col.ToString().PadLeft(2, '0') + "-" + (counter).ToString().PadLeft(2, '0') + "] ";

                counter++;
                if (counter >= 5)
                {
                    counter = 0;
                    row++;
                    if (row >= rows)
                    {
                        row = 0;
                        col++;
                    }
                }

                FileInfo fi = new FileInfo(file);
                TreeNode tds = td.Nodes.Add(idname + fi.Name);
                tds.Tag = fi.FullName;
                tds.StateImageIndex = 1;
                //UpdateProgress();
            }
        }

        private void installMetanetMap_Click(object sender, EventArgs e)
        {
            if (DetectNPPRunning() == true)
            {
                MessageBox.Show("Please close N++ before installing maps to the editor");
                return;
            }

            string mapPath = metanetMapsList.SelectedNode.Tag.ToString();
            string mapName = mapPath.Substring(mapPath.LastIndexOf('\\'));
            if (File.Exists(profilePath + @"\levels\" + mapName))
            {
                DialogResult dialogResult = MessageBox.Show("A map already exists with this filename, do you want to replace it?", "Replace Existing?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    File.Copy(mapPath, profilePath + @"\levels\" + mapName, true);
                    RefreshListEditorMaps();
                    statusLabel.Text = "Done replacing the map file " + mapName;
                }
                else if (dialogResult == DialogResult.No)
                {
                    // do nothing
                    // File.Copy(mapPath, profilePath + @"\levels\" + mapName, false); // throws io exception file already exists
                }
            } else
            {
                if (Directory.Exists(profilePath + @"\levels\") == false)
                {
                    Directory.CreateDirectory(profilePath + @"\levels\");
                }
                File.Copy(mapPath, profilePath + @"\levels\" + mapName);
                RefreshListEditorMaps();
                statusLabel.Text = "Done installing the map file " + mapName;
            }
        }

        private void RefreshListEditorMaps()
        {
            listEditorMaps.Items.Clear();
            PopulateListBoxWithFileType(listEditorMaps, profilePath + @"\levels\", "*");
            deleteSelectedMaps.Enabled = false;
            backupSelectedMaps.Enabled = false;
        }

        private void localEditorMaps_SelectedIndexChanged(object sender, EventArgs e)
        {
            installBackupMap.Enabled = true;
        }

        private void deleteSelectedMaps_Click(object sender, EventArgs e)
        {
            if (DetectNPPRunning() == true)
            {
                MessageBox.Show("Please close N++ before deleting editor maps");
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Are you sure you wish to delete the selected maps? This process is irreversible if you haven't done a recent backup.", "Delete Selected Maps?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                foreach (var item in listEditorMaps.SelectedItems)
                {
                    string filename = item.ToString().Substring(0, item.ToString().LastIndexOf(' ')).TrimEnd();
                    File.Delete(profilePath + @"\levels\" + filename);
                }

                var selectedItems = listEditorMaps.SelectedItems;
                if (listEditorMaps.SelectedIndex != -1)
                {
                    for (int i = selectedItems.Count - 1; i >= 0; i--)
                        listEditorMaps.Items.Remove(selectedItems[i]);
                }

                statusLabel.Text = "Done deleting the selected maps!";
            }
        }

        private void linkMapsInEditor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            launchExplorer(profilePath + @"\levels");
        }

        private void linkSoundFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            launchExplorer(steamGamePath + @"\NPP\Sounds\");
        }

        private void npplogRefresh_Click(object sender, EventArgs e)
        {
            Readnpplog();
            statusLabel.Text = "Done Refreshing NPPLog.txt";
        }

        private void launchNPP_Click(object sender, EventArgs e)
        {
            statusLabel.Text = "Launching N++";
            if (DetectNPPRunning() == true)
            {
                MessageBox.Show("Please close N++ before launching a new instance!");
                statusLabel.Text = "N++ was already running!";
                return;
            } else
            {
                statusLabel.Text = "Launching N++";
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    //Arguments = steamGamePath,
                    FileName = steamGamePath + "\\N++.exe"
                };

                Process.Start(startInfo);
            }

        }

        private void backupCurrentMapPack_Click(object sender, EventArgs e)
        {
            // backup the map pack
            //string mapPackName = communityMapPacksList.SelectedItem.ToString().Substring(0, communityMapPacksList.SelectedItem.ToString().Length - 13);
            ZipFile.CreateFromDirectory(steamGamePath + @"\NPP\Levels", savePath + @"\MapPacks\MapPack" + DateTime.Now.ToString("yyMMddHHmm") + ".zip");

            // update local backup map packs list
            localBackupsMapPacksList.Items.Clear();
            PopulateListBoxWithFileType(localBackupsMapPacksList, savePath + @"\MapPacks", "*.zip");
            renameLocalBackupMapPack.Enabled = false;
            installLocalBackupMapPack.Enabled = false;
            deleteLocalBackupMapPack.Enabled = false;
            installLocalBackupMapPackWithProfile.Enabled = false;

            // notify backup is done
            statusLabel.Text = "Done with backup of active map pack ";
        }

        private void communityMapPacksList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // check if map has a download url, allow installing if it does
            try
            {
                string myStringWebResource = null;
                WebClient myWebClient = new WebClient();

                // get the url
                foreach (var mapSheet in sheetMapList)
                {
                    if (mapSheet.sheetId.Equals(COMMUNITY_MAPPACKS) == true)
                    {
                        Cell[,] data = mapSheet.sheetData.Data;
                        myStringWebResource = data[3, communityMapPacksList.SelectedIndex + 1].Value;
                    }
                }

                if (myStringWebResource != null && !myStringWebResource.Equals(""))
                {
                    installCommunityMapPack.Enabled = true;
                }
                else
                {
                    installCommunityMapPack.Enabled = false;
                }

            }
            catch (Exception exc)
            {
                MessageBox.Show("Error: " + exc.Message);
            }


            // check if map has a custom leaderboard url, allow patching if it does
            try
            {
                string myStringWebResource = null;
                WebClient myWebClient = new WebClient();

                // get the url
                foreach (var mapSheet in sheetMapList)
                {
                    if (mapSheet.sheetId.Equals(COMMUNITY_MAPPACKS) == true)
                    {
                        Cell[,] data = mapSheet.sheetData.Data;
                        myStringWebResource = data[5, communityMapPacksList.SelectedIndex + 1].Value;
                    }
                }

                if (myStringWebResource != null && !myStringWebResource.Equals(""))
                {
                    patchLeaderboardsForMapPack.Enabled = true;
                } else {
                    patchLeaderboardsForMapPack.Enabled = false;
                }

            }
            catch (Exception exc)
            {
                MessageBox.Show("Error: " + exc.Message);
            }
        }

        private void backupActiveProfile_Click(object sender, EventArgs e)
        {
            statusLabel.Text = "Active Profile backup started!";

            using (FileStream fs = new FileStream(savePath + @"\Profiles\MapPack" + DateTime.Now.ToString("yyMMddHHmm") + ".zip", FileMode.Create))
            using (ZipArchive arch = new ZipArchive(fs, ZipArchiveMode.Create))
            {
                arch.CreateEntryFromFile(profilePath + @"\nprofile", "nprofile");
            }

            profileMapBackupList.Items.Clear();
            PopulateListBoxWithFileType(profileMapBackupList, savePath + @"\Profiles", "*.zip");
            renameProfileBackup.Enabled = false;
            installBackupMapPackProfile.Enabled = false;
            deleteBackupMapPackProfile.Enabled = false;

            statusLabel.Text = "Active Profile backup completed!";
        }

        private void localBackupsMapPacksList_SelectedIndexChanged(object sender, EventArgs e)
        {
            renameLocalBackupMapPack.Enabled = true;
            installLocalBackupMapPack.Enabled = true;
            deleteLocalBackupMapPack.Enabled = true;
            if (File.Exists(savePath + @"\Profiles\" + localBackupsMapPacksList.SelectedItem.ToString().Substring(0, localBackupsMapPacksList.SelectedItem.ToString().LastIndexOf(' ')).TrimEnd()) == true)
            {
                installLocalBackupMapPackWithProfile.Enabled = true;
            }
            else
            {
                installLocalBackupMapPackWithProfile.Enabled = false;
            }
        }

        private void mapPacksFolderLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            launchExplorer(steamGamePath + @"\NPP\Levels\");
        }

        private void linkMapPacksBackupFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            launchExplorer(savePath + @"\MapPacks\");
        }

        private void profileFolderLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            launchExplorer(profilePath);
        }

        private void profilesFolderLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            launchExplorer(savePath + @"\Profiles\");
        }

        private void backupMapPackAndProfile_Click(object sender, EventArgs e)
        {
            try
            {
                // backup the map pack
                string tsp = DateTime.Now.ToString("yyMMddHHmm");
                ZipFile.CreateFromDirectory(steamGamePath + @"\NPP\Levels", savePath + @"\MapPacks\MapPack" + tsp + ".zip");

                // update local backup map packs list
                localBackupsMapPacksList.Items.Clear();
                PopulateListBoxWithFileType(localBackupsMapPacksList, savePath + @"\MapPacks", "*.zip");
                renameLocalBackupMapPack.Enabled = false;
                installLocalBackupMapPack.Enabled = false;
                deleteLocalBackupMapPack.Enabled = false;
                installLocalBackupMapPackWithProfile.Enabled = false;

                // backup profile with same timestamp
                using (FileStream fs = new FileStream(savePath + @"\Profiles\MapPack" + tsp + ".zip", FileMode.Create))
                using (ZipArchive arch = new ZipArchive(fs, ZipArchiveMode.Create))
                {
                    arch.CreateEntryFromFile(profilePath + @"\nprofile", "nprofile");
                }

                // update local backup profile list
                profileMapBackupList.Items.Clear();
                PopulateListBoxWithFileType(profileMapBackupList, savePath + @"\Profiles", "*.zip");
                renameProfileBackup.Enabled = false;
                installBackupMapPackProfile.Enabled = false;
                deleteBackupMapPackProfile.Enabled = false;

                // notify backup is done
                statusLabel.Text = "Done backup of active map pack and profile";

            } catch (Exception exc)
            {
                MessageBox.Show("Failed to do backup because: " + exc.Message);
            }
        }

        private void profileMapBackupList_SelectedIndexChanged(object sender, EventArgs e)
        {
            renameProfileBackup.Enabled = true;
            installBackupMapPackProfile.Enabled = true;
            deleteBackupMapPackProfile.Enabled = true;
        }

        private void installLocalBackupMapPack_Click(object sender, EventArgs e)
        {
            if (DetectNPPRunning() == true)
            {
                MessageBox.Show("Please close N++ before installing map pack");
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Are you sure you wish to replace your current levels map pack with this one? This process is irreversible if you haven't done a recent backup.", "Replace Existing Levels Map Pack?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                // install
                string filename = localBackupsMapPacksList.SelectedItem.ToString().Substring(0, localBackupsMapPacksList.SelectedItem.ToString().LastIndexOf(' ')).TrimEnd();
                Directory.Delete(steamGamePath + @"\NPP\Levels", true);
                Directory.CreateDirectory(steamGamePath + @"\NPP\Levels");
                ZipFile.ExtractToDirectory(savePath + @"\MapPacks\" + filename, steamGamePath + @"\NPP\Levels");

                // give some feedback it's done
                installLocalBackupMapPack.Enabled = false;
                statusLabel.Text = "Done Installing Map Pack!";
            }
        }

        private void deleteLocalBackupMapPack_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you wish to delete this map pack backup? This process is irreversible.", "Delete Backup?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    File.Delete(savePath + @"\MapPacks\" + localBackupsMapPacksList.SelectedItem.ToString().Split(' ')[0]);
                    localBackupsMapPacksList.Items.Remove(localBackupsMapPacksList.SelectedItem);
                    renameLocalBackupMapPack.Enabled = false;
                    installLocalBackupMapPack.Enabled = false;
                    deleteLocalBackupMapPack.Enabled = false;
                    installLocalBackupMapPackWithProfile.Enabled = false;

                    statusLabel.Text = "Done Deleting Map Pack Backup!";
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Couldn't delete backup because: " + exc.Message);
                }
            }
        }

        private void installLocalBackupMapPackWithProfile_Click(object sender, EventArgs e)
        {
            if (DetectNPPRunning() == true)
            {
                MessageBox.Show("Please close N++ before installing map pack");
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Are you sure you wish to replace your current levels map pack with this one? This process is irreversible if you haven't done a recent backup.", "Replace Existing Levels Map Pack?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                // install
                string filename = localBackupsMapPacksList.SelectedItem.ToString().Substring(0, localBackupsMapPacksList.SelectedItem.ToString().LastIndexOf(' ')).TrimEnd();
                Directory.Delete(steamGamePath + @"\NPP\Levels", true);
                Directory.CreateDirectory(steamGamePath + @"\NPP\Levels");
                ZipFile.ExtractToDirectory(savePath + @"\MapPacks\" + filename, steamGamePath + @"\NPP\Levels");

                // check if profile exists with same timestamp
                if (File.Exists(savePath + @"\Profiles\" + filename) == true)
                {
                    File.Delete(profilePath + @"\nprofile");
                    string zipPath = savePath + @"\Profiles\" + filename;
                    string extractPath = profilePath + @"\nprofile";
                    using (ZipArchive archive = ZipFile.OpenRead(zipPath))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            if (entry.Name.Equals("nprofile"))
                            {
                                entry.ExtractToFile(extractPath, true);
                                statusLabel.Text = "Done installing Map Pack with Profile of same timestamp: " + filename;
                            }
                        }
                    }
                } else {
                    statusLabel.Text = "Done installing Map Pack " + filename + " but could not find a profile with the same timestamp";
                }

                // give some feedback it's done
                installLocalBackupMapPackWithProfile.Enabled = false;
            }
        }

        private void installBackupMapPackProfile_Click(object sender, EventArgs e)
        {
            if (DetectNPPRunning() == true)
            {
                MessageBox.Show("Please close N++ before installing a profile backup");
                statusLabel.Text = "Aborted loading profile because N++ was running.";
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Are you sure you wish to replace your game profile with the selected backup? This process is irreversible if you haven't done a recent backup.", "Replace Existing nprofile?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                File.Delete(profilePath + @"\nprofile");
                string zipPath = savePath + @"\Profiles\" + profileMapBackupList.SelectedItem.ToString().Substring(0, profileMapBackupList.SelectedItem.ToString().LastIndexOf(' ')).TrimEnd();
                string extractPath = profilePath + @"\nprofile";
                using (ZipArchive archive = ZipFile.OpenRead(zipPath))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (entry.Name.Equals("nprofile"))
                        {
                            entry.ExtractToFile(extractPath, true);
                            statusLabel.Text = "Replaced current N++ game profile with " + profileMapBackupList.SelectedItem.ToString().Substring(0, profileMapBackupList.SelectedItem.ToString().LastIndexOf(' ')).TrimEnd();
                        }
                    }
                }
            }
        }

        private void installCommunityMapPack_Click(object sender, EventArgs e)
        {
            if (DetectNPPRunning() == true)
            {
                MessageBox.Show("Please close N++ before installing map pack");
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Are you sure you wish to replace your current game levels map pack with this one? This process is irreversible if you haven't done a recent backup.", "Install Map Pack?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    string myStringWebResource = null;
                    WebClient myWebClient = new WebClient();

                    // get the url
                    foreach (var mapSheet in sheetMapList)
                    {
                        if (mapSheet.sheetId.Equals(COMMUNITY_MAPPACKS) == true)
                        {
                            Cell[,] data = mapSheet.sheetData.Data;
                            myStringWebResource = data[3, communityMapPacksList.SelectedIndex + 1].Value;
                        }
                    }

                    // download the file and save it into the current filesystem folder.
                    string filename = steamGamePath + @"\NPP\" + "nppassisttempmappacks.zip";
                    myWebClient.DownloadFile(myStringWebResource, filename);

                    // clean \Levels\
                    Directory.Delete(steamGamePath + @"\NPP\Levels", true);
                    Directory.CreateDirectory(steamGamePath + @"\NPP\Levels");

                    // extract temp file
                    ZipFile.ExtractToDirectory(filename, steamGamePath + @"\NPP\Levels");
                    File.Delete(filename);

                    if (File.Exists(steamGamePath + @"\NPP\Levels\nprofile") == true)
                    {
                        DialogResult dialogResult2 = MessageBox.Show("This map pack comes with a default profile, do you wish to install it? This process will backup the current active profile before replacing it.", "Install Custom Profile?", MessageBoxButtons.YesNo);
                        if (dialogResult2 == DialogResult.Yes)
                        {
                            try
                            {
                                if (File.Exists(profilePath + @"\nprofile"))
                                {
                                    // backup old profile
                                    using (FileStream fs = new FileStream(savePath + @"\Profiles\MapPack" + DateTime.Now.ToString("yyMMddHHmm") + ".zip", FileMode.Create))
                                    using (ZipArchive arch = new ZipArchive(fs, ZipArchiveMode.Create))
                                    {
                                        arch.CreateEntryFromFile(profilePath + @"\nprofile", "nprofile");
                                    }
                                }

                                // replace profile with the new
                                if (File.Exists(profilePath + @"\nprofile"))
                                {
                                    File.Delete(profilePath + @"\nprofile");
                                }
                                if (File.Exists(steamGamePath + @"\NPP\Levels\nprofile")) {
                                    File.Move(steamGamePath + @"\NPP\Levels\nprofile", profilePath + @"\nprofile");
                                }
                            } catch (Exception ex) { MessageBox.Show("Couldn't replace profile: " + ex.Message); }
                        }
                    }

                    installCommunityMapPack.Enabled = false;
                    statusLabel.Text = "Done installing map pack " + communityMapPacksList.SelectedItem.ToString();
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Couldn't install map pack because: " + exc.Message);
                }
            }
        }

        private void ResetProfile()
        {
            if (DetectNPPRunning() == true)
            {
                MessageBox.Show("Please close N++ before resetting your profile");
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Are you sure you wish to reset your profile? This process is irreversible if you haven't done a recent backup.", "Reset Profile?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                if (File.Exists(profilePath + @"\nprofile") == true)
                {
                    File.Delete(profilePath + @"\nprofile");
                    statusLabel.Text = "Profile deleted!";
                }
                if (File.Exists(profilePath + @"\nprofile-old") == true)
                {
                    File.Delete(profilePath + @"\nprofile-old");
                }
            }
        }

        private void resetProfile_Click(object sender, EventArgs e)
        {
            ResetProfile();
        }

        private void searchMapName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                //string myString = searchMapName.Text;
                SearchRecursive(metanetMapsList, metanetMapsList.Nodes, searchMapName.Text);
                //MessageBox.Show("Results for: " + searchMapName.Text);
            }
        }

        private bool SearchRecursive(TreeView treeView1, IEnumerable nodes, string searchFor)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Text.ToLower().Contains(searchFor.ToLower()))
                {
                    treeView1.SelectedNode = node;
                    node.BackColor = System.Drawing.Color.Yellow;
                } else
                {
                    node.BackColor = System.Drawing.Color.Transparent;
                }
                if (SearchRecursive(treeView1, node.Nodes, searchFor))
                    return true;
            }
            return false;
        }

        private void searchMapName_Enter(object sender, EventArgs e)
        {
            string myString = searchMapName.Text;
            if (myString.CompareTo("search") == 0) {
                searchMapName.Text = "";
            }
        }

        private void searchTextInMetanetMaps_Click(object sender, EventArgs e)
        {
            SearchRecursive(metanetMapsList, metanetMapsList.Nodes, searchMapName.Text);
        }

        private void backupPalettes_Click(object sender, EventArgs e)
        {
            try
            {
                string[] dirs = Directory.GetDirectories(steamGamePath + @"\NPP\Palettes");
                foreach (string dir in dirs)
                {
                    string[] splits = dir.Split('\\');
                    string palName = splits[splits.Length - 1];
                    ZipFile.CreateFromDirectory(dir, savePath + @"\Palettes\" + palName + DateTime.Now.ToString("yyMMddHHmm") + ".zip");
                }
                localBackupPalettesList.Items.Clear();
                PopulateListBoxWithFileType(localBackupPalettesList, savePath + @"\Palettes", "*.zip");
                installBackupPalette.Enabled = false;
                deleteBackupPalette.Enabled = false;
                statusLabel.Text = "Done with Backup of Installed Palettes!";
            }
            catch (Exception exc)
            {
                MessageBox.Show("Failed to zip palettes: {0}", exc.ToString());
            }
        }

        private void linkSoundpackSpreadsheet_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://docs.google.com/spreadsheets/d/18PshamVuDNyH396a7U3YDFQmCw18s4gIVZ_WrFODRd4/edit#gid=0");
        }

        private void linkPalettesSpreadsheet_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://docs.google.com/spreadsheets/d/1I2f87Qhfs6rxzZq5dQRDbLKYyaGLqTdCkLqfNfrw1Mk/edit#gid=0");
        }

        private void linkMappacksSpreadsheet_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://docs.google.com/spreadsheets/d/18PshamVuDNyH396a7U3YDFQmCw18s4gIVZ_WrFODRd4/edit#gid=1470738075");
        }

        void ReplaceTextInFile(string fileName, string oldText, string newText)
        {
            byte[] fileBytes = File.ReadAllBytes(fileName),
                oldBytes = Encoding.UTF8.GetBytes(oldText);

            byte[] newBytes = new byte[oldBytes.Length];
            byte[] newStringBytes = Encoding.UTF8.GetBytes("45.32.150.168:8126/" + newText);
            for (int i = 0; i < newStringBytes.Length; i++)
            {
                newBytes[i] = newStringBytes[i];
            }

            int index = IndexOfBytes(fileBytes, oldBytes);

            if (index < 0)
            {
                // Text was not found
                return;
            }

            byte[] newFileBytes =
                new byte[fileBytes.Length + newBytes.Length - oldBytes.Length];

            Buffer.BlockCopy(fileBytes, 0, newFileBytes, 0, index);
            Buffer.BlockCopy(newBytes, 0, newFileBytes, index, newBytes.Length);
            Buffer.BlockCopy(fileBytes, index + oldBytes.Length,
                newFileBytes, index + newBytes.Length,
                fileBytes.Length - index - oldBytes.Length);

            File.WriteAllBytes(fileName, newFileBytes);
        }

        int IndexOfBytes(byte[] searchBuffer, byte[] bytesToFind)
        {
            for (int i = 0; i < searchBuffer.Length - bytesToFind.Length; i++)
            {
                bool success = true;

                for (int j = 0; j < bytesToFind.Length; j++)
                {
                    if (searchBuffer[i + j] != bytesToFind[j])
                    {
                        success = false;
                        break;
                    }
                }

                if (success)
                {
                    return i;
                }
            }

            return -1;
        }

        private void patchLeaderboardsForMapPack_Click(object sender, EventArgs e)
        {

            if (DetectNPPRunning() == true)
            {
                MessageBox.Show("Please close N++ before installing map pack");
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Are you sure you wish to patch your N++ for this leaderboard?", "Patch N++?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    string myStringWebResource = null;
                    WebClient myWebClient = new WebClient();

                    // get the url
                    foreach (var mapSheet in sheetMapList)
                    {
                        if (mapSheet.sheetId.Equals(COMMUNITY_MAPPACKS) == true)
                        {
                            Cell[,] data = mapSheet.sheetData.Data;
                            myStringWebResource = data[5, communityMapPacksList.SelectedIndex + 1].Value;
                        }
                    }

                    // check myStringWebResource is not null and has a string with correct length
                    if (myStringWebResource != null && myStringWebResource.Length > 0)
                    {
                        // copy npp.dll into \NPPDLL if it doesn't exist
                        if (!File.Exists(savePath + @"\NPPDLL\npp.dll")) File.Copy(steamGamePath + @"\npp.dll", savePath + @"\NPPDLL\npp.dll");

                        File.Copy(savePath + @"\NPPDLL\npp.dll", savePath + @"\NPPDLL\npp_topatch.dll");

                        // replace "https://dojo.nplusplus.ninja" with myStringWebResource
                        ReplaceTextInFile(savePath + @"\NPPDLL\npp_topatch.dll", "https://dojo.nplusplus.ninja", myStringWebResource);

                        File.Delete(steamGamePath + @"\npp.dll");

                        File.Copy(savePath + @"\NPPDLL\npp_topatch.dll", steamGamePath + @"\npp.dll");

                        File.Delete(savePath + @"\NPPDLL\npp_topatch.dll");

                        patchLeaderboardsForMapPack.Enabled = false;
                        statusLabel.Text = "Done patching leaderboards on N++ for map pack " + communityMapPacksList.SelectedItem.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Could not retrieve a valid patch string: " + myStringWebResource.ToString() + " (" + myStringWebResource.Length + ")");
                    }

                }
                catch (Exception exc)
                {
                    MessageBox.Show("Couldn't install map pack because: " + exc.Message);
                }
            }

        }

        private void resetGameProfile_Click(object sender, EventArgs e)
        {
            ResetProfile();
        }

        private void originalMapPack_Click(object sender, EventArgs e)
        {
            // TODO: revert to original metanet maps
        }

        private void metanetLeaderboards_Click(object sender, EventArgs e)
        {
            if (DetectNPPRunning() == true)
            {
                MessageBox.Show("Please close N++ before installing map pack");
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Are you sure you wish to revert to the original Metanet leaderboards?", "Revert?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    if (File.Exists(savePath + @"\NPPDLL\npp.dll")) {
                        File.Delete(steamGamePath + @"\npp.dll");
                        File.Copy(savePath + @"\NPPDLL\npp.dll", steamGamePath + @"\npp.dll");
                    }
                    statusLabel.Text = "Done reverting to the original Metanet leaderboards!";
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Couldn't revert to original Metanet leaderboards because: " + exc.Message);
                }
            }

        }

        private void nppconfRefresh_Click(object sender, EventArgs e)
        {
            Readnppconf();
            statusLabel.Text = "Done Refreshing npp.conf";
        }

        private void keysvars_refresh_Click(object sender, EventArgs e)
        {
            Readkeysvars();
            statusLabel.Text = "Done Refreshing keys.vars";
        }

        private void buttonResetnppconf_Click(object sender, EventArgs e)
        {
            if (File.Exists(profilePath + @"\npp.conf"))
            {
                try {
                    File.Delete(profilePath + @"\npp.conf");
                    statusLabel.Text = "npp.conf set to default";
                } catch (Exception exc)
                {
                    MessageBox.Show("Coulnd't delete custom npp.conf: " + exc.Message);
                }
            } else
            {
                statusLabel.Text = "npp.conf already in default";
            }
        }

        private void buttonHighDPIFix_Click(object sender, EventArgs e)
        {
            string check = (string)Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows NT\\CurrentVersion\\AppCompatFlags\\Layers", "C:\\Program Files (x86)\\Steam\\steamapps\\common\\N++\\N++.exe", "null");
            if (String.Equals(check, "~ HIGHDPIAWARE") == true)
            {
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows NT\\CurrentVersion\\AppCompatFlags\\Layers", "C:\\Program Files (x86)\\Steam\\steamapps\\common\\N++\\N++.exe", "null");
                statusLabel.Text = "Fix Reverted";
            }
            else
            {
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows NT\\CurrentVersion\\AppCompatFlags\\Layers", "C:\\Program Files (x86)\\Steam\\steamapps\\common\\N++\\N++.exe", "~ HIGHDPIAWARE");
                statusLabel.Text = "Fix Added to Registry";
            }
            updateHighDPIFixButton();
        }

        private void updateHighDPIFixButton()
        {
            string check = (string)Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows NT\\CurrentVersion\\AppCompatFlags\\Layers", "C:\\Program Files (x86)\\Steam\\steamapps\\common\\N++\\N++.exe", "null");
            if (String.Equals(check, "~ HIGHDPIAWARE") == true)
            {
                buttonHighDPIFix.Text = "Revert Fix";
            }
            else
            {
                buttonHighDPIFix.Text = "Fix";
            }
        }

        private void tabControlLogDebug_Selecting(object sender, TabControlCancelEventArgs e)
        {
            TabPage current = (sender as TabControl).SelectedTab;

            // switch to status / home tab
            if (current == tabPageDisplayResolution)
            {
                updateHighDPIFixButton();
            }
        }

    }
    public class sheetMap
    {
        public sheetMap(string sId, Sheet sD)
        {
            sheetId = sId;
            sheetData = sD;
        }
        public string sheetId { get; set; }
        public Sheet sheetData { get; set; }
    }
}