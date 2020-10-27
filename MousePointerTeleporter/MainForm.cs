using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MousePointerTeleporter
{
    public partial class MainForm : Form
    {
        private HotKeyRegister hotKeyRegister = new HotKeyRegister();

        public MainForm()
        {
            InitializeComponent();
        }

        private void settingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.hotKeyRegister.UnregisterHotKey(this.Handle);

            var f = new SettingForm();
            f.HotKeyMgr = this.hotKeyRegister;
            if (f.ShowDialog() == DialogResult.OK)
            {
                this.hotKeyRegister = f.HotKeyMgr;
            }
            this.hotKeyRegister.RegisterHotKey(this.Handle);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.Visible = false;

            if (this.hotKeyRegister.LoadConfig())
            {
                this.hotKeyRegister.RegisterHotKey(this.Handle);
            }
            else
            {
                settingToolStripMenuItem.PerformClick();
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.hotKeyRegister.UnregisterHotKey(this.Handle);
        }


        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == HotKeyRegister.WM_HOTKEY)
            {
                if (((int)m.WParam) == HotKeyRegister.HOTKEY_ID)
                {
                    var f = new MiniMapForm();
                    f.Show();
                }
            }
        }
    }
}
