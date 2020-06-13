using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WindowsFormsApp1
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.pixiv.net/users/29441639");
        }

        private void label3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.deviantart.com/ool8534");
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            this.pictureBox1.Image = Properties.Resources._01;
            /*string url = "https://i.loli.net/2020/06/10/p8LaWlsuOz6SHCZ.jpg";
            try
            {
                pictureBox1.Load(url);
            }
            catch (Exception ex)
            {
                //显示本地默认图片
                this.pictureBox1.Image = Properties.Resources._01;
            }*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
