using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;
using Tesseract;
using Newtonsoft.Json;
using System.Threading;

namespace WindowsFormsApp1
{
    
    public partial class Form4 : Form
    {
        private Form6 returnForm6 = null;

        string path = "tessdata";
        private TesseractEngine OCR;
        bool mouseDown = false, havePainted = false;
        Point start, end;
        Point start1, end1;
        Size size = new Size(0, 0);
        public Form4(Form6 F6)
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
            string src = richTextBox1.Text;
            string aftertrans;
            Dreyestrans dr = new Dreyestrans();
            aftertrans=dr.Translate(src);
            richTextBox2.Text = aftertrans;
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
            OCR oc = new OCR();

            string strResult = oc.ImageToText(bit);
            if (string.IsNullOrEmpty(strResult))
            {
                richTextBox1.Text = "无法识别";
            }
            else
            {
                richTextBox1.Text = strResult;
                trans();

            }
            
        }
        /*
        public string ImageToText(Bitmap img)
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
            /*using (var engine = new TesseractEngine("tessdata", "jpn", EngineMode.Default))
            {
                using (var page = engine.Process(bit))
                {
                    return page.GetText();
                }
            }*/
            /*
            OCR = new TesseractEngine(path, "jpn", EngineMode.Default);
            var page =OCR.Process(img);
            string res = page.GetText();
            page.Dispose();
            return res;
        }
            */
    
        private void Form4_MouseUp(object sender, MouseEventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            trans();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            scan();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.returnForm6.Visible = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Form5 f5 = new Form5();
            f5.Show();
        }

        private void Form4_MouseDown(object sender, MouseEventArgs e)
        {
            start = e.Location;
            mouseDown = true;
        }

        private void Form4_MouseMove(object sender, MouseEventArgs e)
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

        private void Form4_MouseUp_1(object sender, MouseEventArgs e)
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
                //bit.Save(@"cache\ocr.png");
                g.Dispose();
            }
            this.WindowState = FormWindowState.Normal;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Opacity = 1;
            mouseDown = false;
            scan();
        }
    }
}
