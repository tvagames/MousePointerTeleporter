using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MousePointerTeleporter
{
    public partial class MiniMapForm : Form
    {
        public MiniMapForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var margin = 50;
            var rate = 0.1;
            var top = Screen.AllScreens.Min(s => s.Bounds.Top);
            var left = Screen.AllScreens.Min(s => s.Bounds.Left);
            var right = Screen.AllScreens.Max(s => s.Bounds.Right);
            var bottom = Screen.AllScreens.Max(s => s.Bounds.Bottom);
            var width = right - left;
            var height = bottom - top;
            this.Height = (int)Math.Round(height * rate);
            this.Width = (int)Math.Round(width * rate);

            this.Top = MousePosition.Y - this.Height / 2;
            this.Left = MousePosition.X - this.Width / 2;
            
            if (this.Right > right)
            {
                this.Left = right - this.Width - margin;
            }
            if (this.Bottom > bottom)
            {
                this.Top = bottom - this.Height - margin;
            }

            int offset = 0;
            foreach (var screen in Screen.AllScreens)
            {
                uint dpiX;
                uint dpiY;
                ScreenExtentions.GetDpi(screen, DpiType.Effective, out dpiX, out dpiY);
                uint dpiX2;
                uint dpiY2;
                ScreenExtentions.GetDpi(screen, DpiType.Angular, out dpiX2, out dpiY2);
                double dipXrate = 1; //(double)dpiX2 / (double)dpiX;
                double dipYrate = 1; //(double)dpiY2 / (double)dpiY;

                var panel = new PictureBox();
                panel.Top = (int)Math.Round((screen.Bounds.Top - top) * rate) + offset;
                //offset += 10;
                panel.Left = (int)Math.Round((screen.Bounds.Left - left) * rate);
                panel.Width = (int)Math.Round(screen.Bounds.Width * rate * dipXrate);
                panel.Height = (int)Math.Round(screen.Bounds.Height * rate * dipYrate);
                panel.BorderStyle = BorderStyle.FixedSingle;
                panel.BackColor = Color.White;
                panel.Click += Panel_Click;
                panel.Tag = screen;
                panel.MouseClick += Panel_MouseClick;

                Bitmap bmp = new Bitmap(screen.Bounds.Width, screen.Bounds.Height);
                //Graphicsの作成
                Graphics g = Graphics.FromImage(bmp);
                //画面全体をコピーする
                g.CopyFromScreen(screen.Bounds.X, screen.Bounds.Y, 0, 0, bmp.Size);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.DrawImage(bmp, 0, 0, panel.Width, panel.Height);
                //解放
                g.Dispose();
                panel.Image = bmp;

                this.Controls.Add(panel);
                
            }
        }

        private void Panel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Control panel = sender as Control;
                Screen screen = panel.Tag as Screen;
                Cursor.Position = new System.Drawing.Point(e.X * 10 + screen.Bounds.Left, e.Y * 10 + screen.Bounds.Top);
            }
            this.Close();
        }

        private void Panel_Click(object sender, EventArgs e)
        {
           // throw new NotImplementedException();
        }

        private void frmMain_Leave(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmMain_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }
    }



}
