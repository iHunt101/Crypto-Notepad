﻿using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Reflection;
using System.Windows.Forms;

namespace Crypto_Notepad
{
    public partial class SettingsForm : Form
    {
        Properties.Settings ps = Properties.Settings.Default;
        MainWindow ma = new MainWindow();

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            foreach (FontFamily fonts in FontFamily.Families)
            {
                comboBox1.Items.Add(fonts.Name);
            }

            comboBox1.Text = ps.RichTextFont;
            comboBox2.Text = ps.RichTextSize.ToString();
            comboBox4.Text = ps.HashAlgorithm;
            comboBox3.Text = ps.KeySize.ToString();
            textBox1.Text = ps.TheSalt;
            textBox2.Text = ps.PasswordIterations.ToString();
            checkBox1.Checked = ps.AssociateCheck;

            panel1.BackColor = ps.RichForeColor;
            panel2.BackColor = ps.RichBackColor;
            panel3.BackColor = ps.HighlightsColor;
        }

        private void saveSettingsButton_Click(object sender, EventArgs e)
        {
            SetSettings("Save");
        }

        public static void AssociateExtension(string applicationExecutablePath, string extension)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Classes", true);
                key.CreateSubKey("." + extension).SetValue(string.Empty, extension + "_auto_file");
                key = key.CreateSubKey(extension + "_auto_file");
                key.CreateSubKey("DefaultIcon").SetValue(string.Empty, Path.GetDirectoryName(Application.ExecutablePath) + "\\Crypto Notepad.exe" + ",0");
                key = key.CreateSubKey("Shell");
                key.SetValue(string.Empty, "Open");
                key = key.CreateSubKey("Open");
                key.CreateSubKey("Command").SetValue(string.Empty, "\"" + applicationExecutablePath + "\" \"%1\"");
                key.CreateSubKey("ddeexec\\Topic").SetValue(string.Empty, "System");
            }
            catch (Exception)
            {

            }       
        }

        public static void DissociateExtension(string applicationExecutablePath, string extension)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Classes", true);
                key.DeleteSubKeyTree(extension + "_auto_file");
                key.DeleteSubKeyTree("." + extension);
            }
            catch (Exception)
            {

            }

        }

        private void panel1_Click_1(object sender, EventArgs e)
        {
            colorDialog1.Color = panel1.BackColor;
            using (new CenterWinDialog(this))
            {
                colorDialog1.ShowDialog();
            }
            panel1.BackColor = colorDialog1.Color;
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = panel2.BackColor;
            using (new CenterWinDialog(this))
            {
                colorDialog1.ShowDialog();
            }
            panel2.BackColor = colorDialog1.Color;
        }

        private void panel3_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = panel3.BackColor;
            using (new CenterWinDialog(this))
            {
                colorDialog1.ShowDialog();
            }
            panel3.BackColor = colorDialog1.Color;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((tabControl1.SelectedTab == tabControl1.TabPages["tabPage2"]) && (ps.WarningMsg == false))
            {
                SystemSounds.Beep.Play();
                WarningMsgBox w = new WarningMsgBox();
                w.ShowDialog(this);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (new CenterWinDialog(this))
            {
                DialogResult result = MessageBox.Show("Reset to defaults?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

                if (result == DialogResult.Yes)
                {
                    SetSettings("Default");
                }
            }
        }

        private void SetSettings(string value)
        {
            if (value == "Save")
            {
                if (checkBox1.Checked == true)
                {
                    AssociateExtension(Assembly.GetEntryAssembly().Location, "enp");
                }

                if (checkBox1.Checked == false)
                {
                    DissociateExtension(Assembly.GetEntryAssembly().Location, "enp");
                }

                ps.RichForeColor = panel1.BackColor;
                ps.RichBackColor = panel2.BackColor;
                ps.HighlightsColor = panel3.BackColor;
                ps.RichTextFont = comboBox1.Text;
                ps.RichTextSize = Convert.ToInt32(comboBox2.Text.ToString());
                ps.AssociateCheck = checkBox1.Checked;
                ps.HashAlgorithm = comboBox4.Text;
                ps.KeySize = Convert.ToInt32(comboBox3.Text.ToString());
                ps.TheSalt = textBox1.Text;
                ps.PasswordIterations = Convert.ToInt32(textBox2.Text.ToString());
                ps.Save();

                MainWindow.settingsChanged = true;

                this.Hide();
            }

            if (value == "Default")
            {
                checkBox1.Checked = false;
                panel1.BackColor = Color.FromArgb(228, 228, 228);
                panel2.BackColor = Color.FromArgb(56, 56, 56);
                panel3.BackColor = Color.FromArgb(101, 51, 6);
                comboBox1.Text = "Consolas";
                comboBox2.Text = 11.ToString();
                checkBox1.Checked = false;
                comboBox4.Text = "SHA1";
                comboBox3.Text = 192.ToString();
                textBox2.Text = 2.ToString();
            }
        }
    }
}