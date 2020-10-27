using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MousePointerTeleporter
{
    public partial class SettingForm : Form
    {
        public Keys ModdingHotKey { get; set; }
        public HotKeyRegister HotKeyMgr { get; set; }

        public SettingForm()
        {
            InitializeComponent();
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            this.txtKey.Text = (Keys.KeyCode & this.HotKeyMgr.HotKey).ToString();
            this.chkAlt.Checked = (this.HotKeyMgr.HotKey & Keys.Alt) == Keys.Alt;
            this.chkCtrl.Checked = (this.HotKeyMgr.HotKey & Keys.Control) == Keys.Control;
            this.chkShift.Checked = (this.HotKeyMgr.HotKey & Keys.Shift) == Keys.Shift;
            this.ModdingHotKey = this.HotKeyMgr.HotKey;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtKey_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtKey.Text = e.KeyCode.ToString();
            this.chkAlt.Checked = e.Alt;
            this.chkCtrl.Checked = e.Control;
            this.chkShift.Checked = e.Shift;
            e.Handled = true;
            this.ModdingHotKey = e.KeyData;
        }

        private void Save()
        {
            try
            {
                this.HotKeyMgr.HotKey = this.ModdingHotKey;
                this.HotKeyMgr.SaveConfig();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Save();
            this.Close();
        }
    }
}
