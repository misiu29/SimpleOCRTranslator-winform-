using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using WindowsFormsApplication2;
using System.Threading;
using System.Diagnostics;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        //金山快译引用参考：https://www.lgztx.com/?p=220
        private Form6 returnForm6 = null;

        bool mouseDown = false, havePainted = false;
        Point start, end;
        Point start1, end1;
        Size size = new Size(0, 0);

        

        const string FASTAIT_DLL = @"data\FASTAIT2009\GTS\JapaneseSChinese\JPNSCHSDK.dll";
        const string PATH = @"data\FASTAIT2009\GTS\JapaneseSChinese\";
        const string DEFAULT_DIC = "DCT";

        int buffersize = 0x4f4;
        int key = 0x4f4;

        [DllImport(FASTAIT_DLL)]
        internal static extern int StartSession(
            [MarshalAs(UnmanagedType.LPWStr)] string dicpath,
            IntPtr bufferStart,
            IntPtr bufferStop,
            [MarshalAs(UnmanagedType.LPWStr)] string app
        );

        [DllImport(FASTAIT_DLL)]
        internal static extern int EndSession();

        [DllImport(FASTAIT_DLL)]
        internal static extern int OpenEngine(int key);

        [DllImport(FASTAIT_DLL)]
        internal static extern int CloseEngineM(int key);

        [DllImport(FASTAIT_DLL)]
        internal static extern int SimpleTransSentM(
            int key,
            [MarshalAs(UnmanagedType.LPWStr)] string fr,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder t,
            int unknown,
            int unknown2
            );

        [DllImport(FASTAIT_DLL)]
        internal static extern int SetBasicDictPathW(
            int key,
            [MarshalAs(UnmanagedType.LPWStr)] string fr
            );

        [DllImport(FASTAIT_DLL)]
        internal static extern int SetProfDictPathW(
            int key,
            [MarshalAs(UnmanagedType.LPWStr)] string path,
            int priority
            );
        public Form1(Form6 F6)
        {
            InitializeComponent();
            this.returnForm6 = F6;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReadyToCaptrue();
        }
        private void trans()
        {
            IntPtr buffer = Marshal.AllocHGlobal(buffersize);
            try
            {
                string dicPath = PATH + DEFAULT_DIC;
                StartSession(dicPath, buffer, buffer + buffersize, "DCT");//return 0 成功
                OpenEngine(key); //return 0 成功
                SetBasicDictPathW(key, dicPath);//return 0 成功
                StringBuilder to = new StringBuilder(0x400);
                SimpleTransSentM(key, richTextBox1.Text, to, 0x28, 0x4);//return 0 成功
                //Console.WriteLine(to.ToString());
                richTextBox2.Text = to.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                CloseEngineM(key);
                EndSession();
                Marshal.FreeHGlobal(buffer);
            }
        }

        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
           
        }

        private void button1_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        public string ImageToText(Bitmap bit)
        {
            /* using (var engine = new TesseractEngine("tessdata", "jpn", EngineMode.Default))
             {
                 using (var img = Pix.LoadFromFile(imgPath))
                 {
                     using (var page = engine.Process(img))
                     {
                         return page.GetText();
                     }
                 }
             }*/
            using (var engine = new TesseractEngine("data/tessdata", "jpn", EngineMode.Default))
            {
                    using (var page = engine.Process(bit))
                    {
                        return page.GetText();
                    }
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            trans();
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (size.Width != 0 && size.Height != 0)
            {
                ControlPaint.DrawReversibleFrame(new Rectangle(start1, size), Color.Transparent, FrameStyle.Dashed);
                havePainted = false;
            }
            end = e.Location;
            if (start.X > end.X)
            {
                int temp = end.X;
                end.X = start.X;
                start.X = temp;
            }

            if (start.Y > end.Y)
            {
                int temp = end.Y;
                end.Y = start.Y;
                start.Y = temp;
            }
            this.Opacity = 0.0;
            Thread.Sleep(200);
            if (end.X - start.X > 0 && end.Y - start.Y > 0)
            {
                Bitmap bit = new Bitmap(end.X - start.X, end.Y - start.Y);
                Graphics g = Graphics.FromImage(bit);
                g.CopyFromScreen(start, new Point(0, 0), bit.Size);
                bit.Save(@"cache\ocr.png");
                g.Dispose();
            }
            this.WindowState = FormWindowState.Normal;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Opacity = 1;
            mouseDown = false;
            scan();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            start = e.Location;
            mouseDown = true;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                if (size.Width != 0 && size.Height != 0 && havePainted)
                {
                    ControlPaint.DrawReversibleFrame(new Rectangle(start1, size), Color.Transparent, FrameStyle.Dashed);
                }
                end1 = e.Location;
                size.Width = Math.Abs(end1.X - start.X);
                size.Height = Math.Abs(end1.Y - start.Y);
                start1.X = (start.X > end1.X) ? end1.X : start.X;
                start1.Y = (start.Y > end1.Y) ? end1.Y : start.Y;

                if (size.Width != 0 && size.Height != 0)
                {
                    ControlPaint.DrawReversibleFrame(new Rectangle(start1, size), Color.Transparent, FrameStyle.Dashed);
                    havePainted = true;
                }
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            /*Bitmap bit = new Bitmap(end.X - start.X, end.Y - start.Y);
            Graphics g = Graphics.FromImage(bit);
            g.CopyFromScreen(start, new Point(0, 0), bit.Size);
            bit.Save(@"cache\ocr.png");
            */
            scan();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.returnForm6.Visible = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Form5 f5 = new Form5();
            f5.Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            //scan();
        }
        private void ReadyToCaptrue()
        {
            this.Opacity = 0.01;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
        }
        private void scan()
        {
            Bitmap bit = new Bitmap(end.X - start.X, end.Y - start.Y);
            Graphics g = Graphics.FromImage(bit);
            g.CopyFromScreen(start, new Point(0, 0), bit.Size);
            //var imgPath = @"cache\ocr.png";
            //pictureBox1.Image = Image.FromBitmap(bit);
            pictureBox1.Image = Image.FromHbitmap(bit.GetHbitmap());
            string strResult = ImageToText(bit);
            if (string.IsNullOrEmpty(strResult))
            {
                richTextBox1.Text = "无法识别";
            }
            else
            {
                richTextBox1.Text = strResult;

            }
            trans();
        }
    }
}
