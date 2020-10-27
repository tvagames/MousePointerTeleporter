using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MousePointerTeleporter
{
    public class HotKeyRegister
    {
        public Keys HotKey { get; set; }

        public const int MOD_ALT = 0x0001;
        public const int MOD_CONTROL = 0x0002;
        public const int MOD_SHIFT = 0x0004;
        public const int WM_HOTKEY = 0x0312;
        public const int HOTKEY_ID = 0x0001;

        [DllImport("user32.dll")]
        extern static int RegisterHotKey(IntPtr hWnd, int id, int modKey, int key);

        [DllImport("user32.dll")]
        extern static int UnregisterHotKey(IntPtr hWnd, int id);

        public void RegisterHotKey(IntPtr Handle)
        {
            var keyCode = (Keys.KeyCode & this.HotKey);
            var opt = 0;
            if ((this.HotKey & Keys.Alt) == Keys.Alt)
            {
                opt |= MOD_ALT;
            }
            if ((this.HotKey & Keys.Control) == Keys.Control)
            {
                opt |= MOD_CONTROL;
            }
            if ((this.HotKey & Keys.Shift) == Keys.Shift)
            {
                opt |= MOD_SHIFT;
            }

            if (RegisterHotKey(Handle, HOTKEY_ID, opt, (int)keyCode) == 0)
            {
                MessageBox.Show("既に他のアプリで使用されています。");
            }
        }

        public void UnregisterHotKey(IntPtr Handle)
        {
            UnregisterHotKey(Handle, HOTKEY_ID);
        }


        public string GetConfigPath()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            var fi = new FileInfo(assembly.Location);
            return Path.Combine(fi.DirectoryName, "MousePointerTeleporter.config");
        }

        public bool LoadConfig()
        {
            string path = this.GetConfigPath();
            if (File.Exists(path))
            {
                using (var sr = new System.IO.StreamReader(path))
                {
                    var r = sr.ReadLine();
                    this.HotKey = (Keys)Enum.Parse(typeof(Keys), r, true);
                }
                return true;
            }
            return false;
        }

        public void SaveConfig()
        {
            try
            {
                string path = this.GetConfigPath();
                using (var sw = new System.IO.StreamWriter(path))
                {
                    sw.WriteLine(this.HotKey.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

    }
}
