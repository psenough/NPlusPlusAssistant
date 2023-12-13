﻿using System;
using System.Windows.Forms;

namespace N__Assistant
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            } catch (Exception exc)
            {
                MessageBox.Show("Unexpected Exception is unexpected! " + exc.Message);
            }
            
        }
    }
}
