using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Picturebox_Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();  
        }
        public PictureBox[,] pb; int xe, ye;
        public void Addpic()
        {
            string s;
            int i, j;
            for (j=0;j<nh;j++)
            {
                for (i = 0; i < nw; i++)
                {
                    s = Convert.ToString(nw * j + i+1);
                    pb[i, j] = new PictureBox();
                    pb[i, j].Visible = true;
                    pb[i, j].Name = Convert.ToString(nw * j + i);
                    pb[i, j].Location = new Point(i * width , j * height);
                    pb[i, j].Size = new System.Drawing.Size(width, height);
                        pb[i, j].MouseClick += new MouseEventHandler(ClickHandler);
                        pb[i, j].Paint += new PaintEventHandler(Painter);
                    if (!(i == nw-1 && j == nh-1))
                        Controls.Add(this.pb[i, j]);
                }
                }
            fir = false;
        }
        private void ClickHandler(object sender, System.EventArgs e)
        {
            Bitmap b = new Bitmap(width, height);
            var whichPic = (PictureBox)sender;
            int x, y;
            x = whichPic.Location.X/width;
            y = whichPic.Location.Y/height;
            if ((x - 1 == xe && y == ye) || (x + 1 == xe && y == ye) || (x == xe && y - 1 == ye) || (x == xe && y + 1 == ye))
            {
                whichPic.Location = new Point(xe * width, ye * height);
                xe = x;
                ye = y;
            }
            if (won())
            {
                MessageBox.Show("You Did it!!!");
                reset();
            }
        }
        int nw, nh;
        bool[] firstarr;
        string[] s ;
        bool won()
        {
            int x, y; bool w=true;
            for (int cnt = 0; cnt < nw * nh - 1; cnt++)
            {
                x = (Convert.ToInt32( s[cnt])-1) % nw;
                y = (Convert.ToInt32( s[cnt])-1) / nw;
                if (pb[cnt % nw, cnt/nw].Location.X / width != x || pb[cnt % nw, cnt/nw].Location.Y / height != y)
                    w = false;
            }
            return w;
        }
        private void Painter(object sender, PaintEventArgs e)
        {
            bool first = false;
            for (int i = 0; i < nw * nh; i++)
                first = first | firstarr[i];
             var whichPic = (PictureBox)sender;
            Graphics gr = whichPic.CreateGraphics();
            int x, y;
            
            x = Convert.ToInt32(whichPic.Name)%nw;
            y = Convert.ToInt32(whichPic.Name)/nw;
                    if (first)
                    {
                        e.Graphics.Clear(Color.White);
                        if (!(x == xe && y == ye))
                            e.Graphics.DrawString(s[nw * y + x], new Font("Times New Roman", 40), new SolidBrush(Color.Black), whichPic.Width / 2, whichPic.Height / 2);
                        firstarr[nw * y + x] = false;
                    }
        }
        private void reset()
        {
            if (!fir)
                for (int i = 0; i < nw; i++)
                    for (int j = 0; j < nh; j++)
                        Controls.Remove(pb[i, j]);
            nw = Convert.ToInt32(textBox1.Text);
            nh = Convert.ToInt32(textBox2.Text);
            width = (this.Width - 190) / nw;
            height = (this.Height - 80) / nh;
            pb = new PictureBox[nw, nh];
            Addpic();
            firstarr = new bool[nw * nh];
            Random rnd = new Random();
            for (int i = 0; i < nw * nh; i++)
                firstarr[i] = true;
            xe = nw - 1;
            ye = nh - 1;
            s = new string[nw * nh - 1];
            for (int i = 0; i < nw * nh - 1; i++)
                s[i] = Convert.ToString(i + 1);
            string temp;
            do
            {
                for (int i = 0; i < nw * nh - 2; i++)
                    for (int j = i; j < nw * nh - 1; j++)
                    {
                        int n = rnd.Next(i, nw * nh - 1);
                        temp = s[i];
                        s[i] = s[n];
                        s[n] = temp;
                    }
            } while (won());
        }
        bool fir = true;
        int width, height;
        private void button1_Click(object sender, EventArgs e)
        {
           reset();
        }
    }
}
