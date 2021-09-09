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

namespace N__Assistant
{
    public partial class Form1 : Form
    {
        // more robust autodetect added by YupdanielThatsMe#6492
        private string steamGamePath;
        private string profilePath = Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%") + @"\Documents\Metanet\N++";
        private string screenshotsPath = @"\userdata\64929984\760\remote\230270\screenshots";
        private string savePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\N++Assistant";

        private BackgroundWorker bgwBackupNow = new BackgroundWorker();

        // there is probably a better way to map this but fuck me if i know C# properly
        private List<sheetMap> sheetMapList = new List<sheetMap>();
        private string COMMUNITY_PALETTES = "1I2f87Qhfs6rxzZq5dQRDbLKYyaGLqTdCkLqfNfrw1Mk";

        public Form1()
        {
            InitializeComponent();

            // get steam path
            string steampath = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Valve\\Steam", "InstallPath", "null");
            if (steampath == "null") {
                steampath = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Valve\\Steam", "InstallPath", "null");
            }
            if (steampath == "null") {
                throw new FileNotFoundException("Steam Not Found, WTF Bro is your pc good?");
            }
            if (!Directory.Exists(steampath + "\\steamapps\\")) {
                throw new FileNotFoundException("steamapps Not Found");
            }
            screenshotsPath = steampath + screenshotsPath;
            string[] configLines = File.ReadAllLines(steampath + "\\steamapps\\libraryfolders.vdf");
            List<string> possiblepaths = new List<string>() { steampath };
            foreach (string configline in configLines)
            {
                if (configline.Contains("\t\t\"path\"\t\t\"")) {
                    possiblepaths.Add(configline.Replace("\t\t\"path\"\t\t\"", "").Replace("\\\\", "\\").Replace("\"", ""));
                }
            }
            foreach (string possiblepath in possiblepaths) {
                if (Directory.Exists(possiblepath + "\\steamapps\\common\\N++")) {
                    steamGamePath = possiblepath + "\\steamapps\\common\\N++";
                    break;
                }
            }
            if (steamGamePath == "") {
                throw new FileNotFoundException("N++ not installed in steam");
            }

            // rename linked labels
            steamInstallDir.Text = steamGamePath;
            profileDir.Text = profilePath;
            screenshotsDir.Text = screenshotsPath;
            backupsDir.Text = savePath;

            // create backup directories if they dont exist
            if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);
            if (!Directory.Exists(savePath + @"\Profiles")) Directory.CreateDirectory(savePath + @"\Profiles");
            if (!Directory.Exists(savePath + @"\Sounds")) Directory.CreateDirectory(savePath + @"\Sounds");
            if (!Directory.Exists(savePath + @"\Replays")) Directory.CreateDirectory(savePath + @"\Replays");
            if (!Directory.Exists(savePath + @"\Levels")) Directory.CreateDirectory(savePath + @"\Levels");
            if (!Directory.Exists(savePath + @"\EditorLevels")) Directory.CreateDirectory(savePath + @"\EditorLevels");
            if (!Directory.Exists(savePath + @"\Palettes")) Directory.CreateDirectory(savePath + @"\Palettes");
            if (!Directory.Exists(savePath + @"\GameLevels")) Directory.CreateDirectory(savePath + @"\GameLevels");

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
                    //Directory.CreateDirectory(savePath + @"\Palettes\Palettes");
                    ZipFile.ExtractToDirectory(filename, savePath + @"\Palettes");
                    File.Delete(savePath + @"\Palettes\Palettes.zip");
                } catch (Exception exc) {
                    MessageBox.Show("Couldn't download Metanet Palettes pack because: " + exc.Message);
                }
            }

            // create palettes directory in steam game dir if it doesnt exist (it's needed to install the custom palettes)
            if (!Directory.Exists(steamGamePath + @"\NPP\Palettes")) Directory.CreateDirectory(steamGamePath + @"\NPP\Palettes");

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

            loadProfile.Enabled = false;
            deleteProfile.Enabled = false;
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            TabPage current = (sender as TabControl).SelectedTab;

            // switch to status / home tab
            if (current == tabPage1)
            {
               
            }

            // switch to profile tab
            if (current == tabPage2)
            {
                profileList.Items.Clear();
                PopulateListBoxWithFileType(profileList, savePath + @"\Profiles", "*.zip");
                loadProfile.Enabled = false;
                deleteProfile.Enabled = false;
            }

            // switch to soundpacks tab
            if (current == tabPage3)
            {
                // spreadsheet of new sound packs
                // https://docs.google.com/spreadsheets/d/18PshamVuDNyH396a7U3YDFQmCw18s4gIVZ_WrFODRd4/edit#gid=0

                // latest google sheets api example code
                // https://github.com/popcron/sheets
            }

            // switch to palettes tab
            if (current == tabPage4)
            {
                // populate metanet palettes (useful only for auto-unlock and references when creating palettes)
                // https://cdn.discordapp.com/attachments/197793786389200896/592821804746276864/Palettes.zip
                metanetPalettesList.Items.Clear();
                PopulateListBoxWithSubDirectories(metanetPalettesList, savePath + @"\Palettes\Palettes");
                installMetanetPalette.Enabled = false;

                // populate community palettes from spreadsheet link
                // https://docs.google.com/spreadsheets/d/1I2f87Qhfs6rxzZq5dQRDbLKYyaGLqTdCkLqfNfrw1Mk/edit#gid=0
                communityPalettesList.Items.Clear();
                PopulateListBoxWithSpreadsheetData(communityPalettesList, 0, COMMUNITY_PALETTES, new APIKey().key);
                installCommunityPalette.Enabled = false;

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
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Directory.Exists(steamGamePath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = steamGamePath,
                    FileName = "explorer.exe"
                };

                Process.Start(startInfo);
            }
            else
            {
                MessageBox.Show(string.Format("{0} Directory does not exist!", steamGamePath));
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Directory.Exists(profilePath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = profilePath,
                    FileName = "explorer.exe"
                };

                Process.Start(startInfo);
            }
            else
            {
                MessageBox.Show(string.Format("{0} Directory does not exist!", profilePath));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (DetectNPPRunning() == true)
            {
                MessageBox.Show("Please close N++ before doing backup");
                return;
            }
            backupNow.Enabled = false;
            progressBar1.Show();
            bgwBackupNow.RunWorkerAsync();
        }

        private void bgwBackupNow_DoWork(object sender, DoWorkEventArgs e)
        {
            bgwBackupNow.ReportProgress(0, "Zipping nprofile");
            if (checkedListBox1.GetItemCheckState(0) == CheckState.Checked)
            {
                // backup profile
                using (FileStream fs = new FileStream(savePath + @"\Profiles\nprofile" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip", FileMode.Create))
                using (ZipArchive arch = new ZipArchive(fs, ZipArchiveMode.Create))
                {
                    arch.CreateEntryFromFile(profilePath + @"\nprofile", "nprofile");
                }
            }
            bgwBackupNow.ReportProgress(15, "Zipping Soundpack");
            if (checkedListBox1.GetItemCheckState(1) == CheckState.Checked)
            {
                // backup soundpack
                ZipFile.CreateFromDirectory(steamGamePath + @"\NPP\Sounds", savePath + @"\Sounds\Sounds" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip");
            }
            bgwBackupNow.ReportProgress(30, "Zipping Editor Levels");
            if (checkedListBox1.GetItemCheckState(2) == CheckState.Checked)
            {
                // backup editor levels
                ZipFile.CreateFromDirectory(profilePath + @"\levels", savePath + @"\EditorLevels\EditorLevels" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip");
            }
            bgwBackupNow.ReportProgress(40, "Zipping Replays (attract files)");
            if (checkedListBox1.GetItemCheckState(3) == CheckState.Checked)
            {
                // backup replays (attract files)
                ZipFile.CreateFromDirectory(profilePath + @"\attract", savePath + @"\Replays\Replays" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip");
            }
            bgwBackupNow.ReportProgress(60, "Zipping Palettes");
            if (checkedListBox1.GetItemCheckState(4) == CheckState.Checked)
            {
                // backup palettes
                //ZipFile.CreateFromDirectory(steamGamePath + @"\NPP\Palettes", savePath + @"\Palettes\Palettes" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip");
                try
                {
                    string[] dirs = Directory.GetDirectories(steamGamePath + @"\NPP\Palettes");
                    foreach (string dir in dirs)
                    {
                        string[] splits = dir.Split('\\');
                        string palName = splits[splits.Length - 1];
                        ZipFile.CreateFromDirectory(dir, savePath + @"\Palettes\" + palName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip");
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
                // backup gamelevels
                ZipFile.CreateFromDirectory(steamGamePath + @"\NPP\Levels", savePath + @"\GameLevels\GameLevels" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip");
            }
            bgwBackupNow.ReportProgress(100, "Done with backup!");
        }

        private void bgwBackupNow_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            progressLabel.Text = String.Format("{0}", e.UserState);
        }

        private void bgwBackupNow_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressLabel.Text = "Done with backup!";
            progressBar1.Hide();
            backupNow.Enabled = true;
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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
            DirectoryInfo dinfo = new DirectoryInfo(Folder);
            FileInfo[] Files = dinfo.GetFiles(FileType);
            foreach (FileInfo file in Files)
            {
                lsb.Items.Add(file.Name + " (" + file.Length / 1024 + "Kb)");
            }
        }

        private void PopulateListBoxWithSubDirectories(ListBox lsb, string Folder)
        {
            DirectoryInfo dinfo = new DirectoryInfo(Folder);
            DirectoryInfo[] dirs = dinfo.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                lsb.Items.Add(dir.Name + " (" + dir.LastWriteTime.ToShortDateString() + ")");
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
            profileBackupLabel.Text = "Profile backup started!";

            using (FileStream fs = new FileStream(savePath + @"\Profiles\nprofile" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip", FileMode.Create))
            using (ZipArchive arch = new ZipArchive(fs, ZipArchiveMode.Create))
            {
                arch.CreateEntryFromFile(profilePath + @"\nprofile", "nprofile");
            }
            profileBackupLabel.Text = "Profile backup completed!";

            profileList.Items.Clear();
            PopulateListBoxWithFileType(profileList, savePath + @"\Profiles", "*.zip");
        }

        private void profileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadProfile.Enabled = true;
            deleteProfile.Enabled = true;
        }

        private void deleteProfile_Click(object sender, EventArgs e)
        {
            //TODO: confirmation box
            try
            {
                File.Delete(savePath + @"\Profiles\" + profileList.SelectedItem.ToString().Split(' ')[0]);
                profileList.Items.Remove(profileList.SelectedItem);
                loadProfile.Enabled = false;
                deleteProfile.Enabled = false;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void uninstallPalette_Click(object sender, EventArgs e)
        {
            //TODO: confirmation box
            try
            {
                Directory.Delete(steamGamePath + @"\NPP\Palettes\" + palettesInstalledList.SelectedItem.ToString().Split(' ')[0], true);
                //palettesInstalled.Items.Remove(palettesInstalled.SelectedItem);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

            palettesInstalledList.Items.Remove(palettesInstalledList.SelectedItem);
            //palettesInstalledList.Items.Clear();
            //PopulateListBoxWithSubDirectories(palettesInstalledList, steamGamePath + @"\NPP\Palettes");
            uninstallPalette.Enabled = false;
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
            ZipFile.CreateFromDirectory(steamGamePath + @"\NPP\Palettes\" + palName, savePath + @"\Palettes\" + palName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip");

            // update local backup palettes list
            localBackupPalettesList.Items.Clear();
            PopulateListBoxWithFileType(localBackupPalettesList, savePath + @"\Palettes", "*.zip");
            installBackupPalette.Enabled = false;
            deleteBackupPalette.Enabled = false;

            // notify backup was done
            backupPalette.Enabled = false;
        }

        private void palettesInstalledLinkedLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Directory.Exists(steamGamePath + @"\NPP\Palettes\"))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = steamGamePath + @"\NPP\Palettes\",
                    FileName = "explorer.exe"
                };

                Process.Start(startInfo);
            }
            else
            {
                MessageBox.Show(string.Format("{0} Directory does not exist!", steamGamePath + @"\NPP\Palettes\"));
            }
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

        private async void PopulateListBoxWithSpreadsheetData(ListBox lsb, int sheetsId, string spreadsheetId, string key)
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
                    string parsedDate = data[2, y].Value.Split('/')[2] + "/" + data[2, y].Value.Split('/')[1] + "/" + data[2, y].Value.Split('/')[0];
                    lsb.Items.Add(data[0, y].Value + " by " + data[1, y].Value + " (" + parsedDate + ")");
                }

                bool exists = false;
                foreach (var mapSheet in sheetMapList)
                {
                    if (mapSheet.sheetId.Equals(COMMUNITY_PALETTES) == true)
                    {
                        mapSheet.sheetData = sheet;
                        exists = true;
                    }
                }
                if (exists == false)
                {
                    sheetMapList.Add(new sheetMap(COMMUNITY_PALETTES, sheet));
                }

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
            DirectoryCopy(savePath + @"\Palettes\Palettes\" + palName, steamGamePath + @"\NPP\Palettes\" + palName, false);
            palettesInstalledList.Items.Clear();
            PopulateListBoxWithSubDirectories(palettesInstalledList, steamGamePath + @"\NPP\Palettes");
            installMetanetPalette.Enabled = false;
        }

        private void communityPalettesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            installCommunityPalette.Enabled = true;
        }

        private void installCommunityPalette_Click(object sender, EventArgs e)
        {
            try { 
                string myStringWebResource = null;
                WebClient myWebClient = new WebClient();
                
                // get the url
                foreach (var mapSheet in sheetMapList)
                {
                    if (mapSheet.sheetId.Equals(COMMUNITY_PALETTES) == true)
                    {
                        Cell[,] data = mapSheet.sheetData.Data;
                        myStringWebResource = data[3, communityPalettesList.SelectedIndex+1].Value;
                    }
                }

                // download the file and save it into the current filesystem folder.
                string filename = steamGamePath + @"\NPP\Palettes\" + "nppassisttemppal.zip";
                myWebClient.DownloadFile(myStringWebResource, filename);
                
                // TODO: check if palette has subdir in zip or not (if not create dir and install there instead of Palettes root)
                // extract temp file
                ZipFile.ExtractToDirectory(filename, steamGamePath + @"\NPP\Palettes");
                File.Delete(filename);

                // refresh installed palettes directory
                palettesInstalledList.Items.Clear();
                PopulateListBoxWithSubDirectories(palettesInstalledList, steamGamePath + @"\NPP\Palettes");
                installCommunityPalette.Enabled = false;

            } catch(Exception exc)
            {
                MessageBox.Show("Couldn't install community palette because: " + exc.Message);
            }
        }

        private void installBackupPalette_Click(object sender, EventArgs e)
        {
            // TODO: check if dir exists
            // prompt asking if replacing existing dir or not

            // TODO: install the palette

            // refresh installed palettes list
            palettesInstalledList.Items.Clear();
            PopulateListBoxWithSubDirectories(palettesInstalledList, steamGamePath + @"\NPP\Palettes");
            uninstallPalette.Enabled = false;
            backupPalette.Enabled = false;

            // notify it's done
            installBackupPalette.Enabled = false;
        }

        private void deleteBackupPalette_Click(object sender, EventArgs e)
        {
            //TODO: confirmation box

            string palName = localBackupPalettesList.SelectedItem.ToString().Substring(0, localBackupPalettesList.SelectedItem.ToString().LastIndexOf(' ')).TrimEnd();
            File.Delete(savePath + @"\Palettes\" + palName);

            // remove item instead of refresh whole box, to keep position in view
            localBackupPalettesList.Items.Remove(localBackupPalettesList.SelectedItem);

            // refresh dir
            //localBackupPalettesList.Items.Clear();
            //PopulateListBoxWithFileType(localBackupPalettesList, savePath + @"\Palettes", "*.zip");
            installBackupPalette.Enabled = false;
            deleteBackupPalette.Enabled = false;
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