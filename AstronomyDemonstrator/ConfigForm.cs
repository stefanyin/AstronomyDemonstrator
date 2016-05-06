using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace AstronomyDemonstrator
{
    public partial class ConfigForm : Form
    {
        XmlSetting xmlSetting;
        MainWindow mainWin;
        public ConfigForm(MainWindow win)
        {
            InitializeComponent();
            this.mainWin = win;
            InitForm();
            
        }
        private void InitForm()
        {
            xmlSetting = new XmlSetting();
            textBoxFolderPath.Text = xmlSetting.GetValueByName("MoviesPath");
        }
        private void buttonSelect_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog()==DialogResult.OK)
            {
                textBoxFolderPath.Text = fbd.SelectedPath;
            }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            xmlSetting.SetNodeValue("MoviesPath", textBoxFolderPath.Text.Trim());
            //mainWin.InitRelation();
            mainWin.UpdateListBox(textBoxFolderPath.Text.Trim());
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void StartWhenBoot(bool flag)
        {
            try
            {
                RegistryKey key1 = Registry.LocalMachine;
                RegistryKey key2 = key1.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (flag)
                {
                    key2.SetValue("VideoPlayer", Application.ExecutablePath);
                }
                else
                {
                    key2.DeleteValue("VideoPlayer", false);
                }
            }
            catch (System.Exception ex)
            {
            	

            }

            
        }

       

    }
}
