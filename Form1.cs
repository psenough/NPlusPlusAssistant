﻿using Microsoft.Win32;
using System;
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

            // create palettes directory in game dir if it doesnt exist (it's needed to place custom palettes)
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
                // TODO: populate official metanet palettes (useful only for auto-unlock and references when creating palettes)
                // https://cdn.discordapp.com/attachments/197793786389200896/592821804746276864/Palettes.zip

                installMetanetPalette.Enabled = false;

                // populate community palettes from spreadsheet link
                // https://docs.google.com/spreadsheets/d/1I2f87Qhfs6rxzZq5dQRDbLKYyaGLqTdCkLqfNfrw1Mk/edit#gid=0
                communityPalettesList.Items.Clear();
                PopulateListBoxWithSpreadsheetData(communityPalettesList, 0, "1I2f87Qhfs6rxzZq5dQRDbLKYyaGLqTdCkLqfNfrw1Mk", new APIKey().key);
                installCommunityPalette.Enabled = false;

                //TODO: list all palettes in local backup dir
                installBackupPalette.Enabled = false;
                deleteBackupPalette.Enabled = false;

                // list installed palettes
                palettesInstalled.Items.Clear();
                PopulateListBoxWithSubDirectories(palettesInstalled, steamGamePath + @"\NPP\Palettes");
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
                ZipFile.CreateFromDirectory(steamGamePath + @"\NPP\Palettes", savePath + @"\Palettes\Palettes" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip");
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
            //label2.Text = String.Format("Total items transfered: {0}", e.UserState);
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
            // backup profile
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
                Directory.Delete(steamGamePath + @"\NPP\Palettes\" + palettesInstalled.SelectedItem.ToString().Split(' ')[0], true);
                //palettesInstalled.Items.Remove(palettesInstalled.SelectedItem);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

            palettesInstalled.Items.Clear();
            PopulateListBoxWithSubDirectories(palettesInstalled, steamGamePath + @"\NPP\Palettes");
            uninstallPalette.Enabled = false;
        }

        private void palettesInstalled_SelectedIndexChanged(object sender, EventArgs e)
        {
            uninstallPalette.Enabled = true;
            //backupPalette.Enabled = true;
        }

        private void backupPalette_Click(object sender, EventArgs e)
        {
            //TODO: backup the selected palette only
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
                Authorization authorization = await Authorization.Authorize(key);
                Spreadsheet spreadsheet = await Spreadsheet.Get(spreadsheetId, authorization);
                Sheet sheet = spreadsheet.Sheets[sheetsId];
                //Debug.Print("Rows: " + sheet.Rows);
                //Debug.Print("Columns: " + sheet.Columns);

                Cell[,] data = sheet.Data;
                
                for (int y = 1; y < sheet.Rows; y++)
                {
                    string parsedDate = data[2, y].Value.Split('/')[2] + "/" + data[2, y].Value.Split('/')[1] + "/" + data[2, y].Value.Split('/')[0];
                    lsb.Items.Add(data[0, y].Value + " by " + data[1, y].Value + " (" + parsedDate + ")");
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
            // TODO
        }

        private void communityPalettesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            installCommunityPalette.Enabled = true;
        }

        private void installCommunityPalette_Click(object sender, EventArgs e)
        {

        }

        private void installBackupPalette_Click(object sender, EventArgs e)
        {
            // TODO
        }

        private void deleteBackupPalette_Click(object sender, EventArgs e)
        {
            // TODO
        }

    }
}