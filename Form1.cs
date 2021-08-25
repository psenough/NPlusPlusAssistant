using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Globalization;

namespace N__Assistant
{
    public partial class Form1 : Form
    {
        //TODO: more robust autodetect (in case people are using non default installation directories)
        string steamPath = @"C:\Program Files (x86)\Steam\steamapps\common\N++";
        string profilePath = Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%") + @"\Documents\Metanet\N++";
        string screenshotsPath = @"C:\Program Files (x86)\Steam\userdata\64929984\760\remote\230270\screenshots";
        string savePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\N++Assistant";

        public Form1()
        {
            InitializeComponent();

            // rename linked labels
            steamInstallDir.Text = steamPath;
            profileDir.Text = profilePath;
            screenshotsDir.Text = screenshotsPath;
            backupsDir.Text = savePath;

            // create backup directory if it doesnt exist
            if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);
            if (!Directory.Exists(savePath + @"\Profiles")) Directory.CreateDirectory(savePath + @"\Profiles");
            if (!Directory.Exists(savePath + @"\Sounds")) Directory.CreateDirectory(savePath + @"\Sounds");
            if (!Directory.Exists(savePath + @"\Replays")) Directory.CreateDirectory(savePath + @"\Replays");
            if (!Directory.Exists(savePath + @"\Levels")) Directory.CreateDirectory(savePath + @"\Levels");
            if (!Directory.Exists(savePath + @"\EditorLevels")) Directory.CreateDirectory(savePath + @"\EditorLevels");
            if (!Directory.Exists(savePath + @"\Palettes")) Directory.CreateDirectory(savePath + @"\Palettes");
            if (!Directory.Exists(savePath + @"\GameLevels")) Directory.CreateDirectory(savePath + @"\GameLevels");

            // create palettes directory in game if it doesnt exist (it's needed to place custom palettes)
            if (!Directory.Exists(steamPath + @"\NPP\Palettes")) Directory.CreateDirectory(steamPath + @"\NPP\Palettes");

            // mark backup now checklist items as checked by default
            //TODO: save user settings and reload the same on launch
            // default should probably be just profile and editor levels, not everything
            /*for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, true);
            }*/
            // set checkbox on profile backup default on
            checkedListBox1.SetItemChecked(0, true);
            // set checkbox on level editor backup default on
            checkedListBox1.SetItemChecked(2, true);

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Directory.Exists(steamPath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = steamPath,
                    FileName = "explorer.exe"
                };

                Process.Start(startInfo);
            }
            else
            {
                MessageBox.Show(string.Format("{0} Directory does not exist!", steamPath));
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
            if (checkedListBox1.GetItemCheckState(0) == CheckState.Checked)
            {
                // backup profile
                using (FileStream fs = new FileStream(savePath + @"\Profiles\nprofile" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip", FileMode.Create))
                using (ZipArchive arch = new ZipArchive(fs, ZipArchiveMode.Create))
                {
                    arch.CreateEntryFromFile(profilePath + @"\nprofile", "nprofile");
                }
            }
            if (checkedListBox1.GetItemCheckState(1) == CheckState.Checked)
            {
                // backup soundpack
                ZipFile.CreateFromDirectory(steamPath + @"\NPP\Sounds", savePath + @"\Sounds\Sounds" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip");
            }
            if (checkedListBox1.GetItemCheckState(2) == CheckState.Checked)
            {
                // backup editor levels
                ZipFile.CreateFromDirectory(profilePath + @"\levels", savePath + @"\EditorLevels\EditorLevels" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip");
            }
            if (checkedListBox1.GetItemCheckState(3) == CheckState.Checked)
            {
                // backup replays (attract files)
                ZipFile.CreateFromDirectory(profilePath + @"\attract", savePath + @"\Replays\Replays" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip");
            }
            if (checkedListBox1.GetItemCheckState(4) == CheckState.Checked)
            {
                // backup palettes
                ZipFile.CreateFromDirectory(steamPath + @"\NPP\Palettes", savePath + @"\Palettes\Palettes" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip");
            }
            if (checkedListBox1.GetItemCheckState(4) == CheckState.Checked)
            {
                // backup gamelevels
                ZipFile.CreateFromDirectory(steamPath + @"\NPP\Levels", savePath + @"\GameLevels\GameLevels" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip");
            }

            //TODO: show progress bar and notify when it's done (to prevent button click spamming of clueless user)
            // https://stackoverflow.com/questions/18029658/progress-bar-start-and-stop-on-button-click-using-thread
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

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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
    }
}
