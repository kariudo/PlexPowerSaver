using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlexPowerSaver
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            plexUserTextBox.Text = Properties.Settings.Default.Username;
            plexPasswordTextBox.Text = Settings.ToInsecureString(Settings.DecryptString(Properties.Settings.Default.Password));
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(plexUserTextBox.Text) && !string.IsNullOrEmpty(plexPasswordTextBox.Text))
            {
                Settings.SaveUserSettings(plexUserTextBox.Text, plexPasswordTextBox.Text);
                Close();
            }
            else
            {
                MessageBox.Show("Invalid Entry", "Username & Password for Plex are required.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
