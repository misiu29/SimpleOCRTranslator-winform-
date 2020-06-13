using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;

namespace WindowsFormsApp1
{
    class OCR
    {
        private TesseractEngine OCR1;
        
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
            /*string path = Application.StartupPath + @"../../../";
            System.IO.Directory.SetCurrentDirectory(path);
            string a = System.IO.Directory.GetCurrentDirectory();*/
            string a = Application.StartupPath + "\\data";
            Directory.SetCurrentDirectory(a);//路径偏移 固定路径
            /*OCR1 = new TesseractEngine(a + "\\tessdata", "jpn", EngineMode.Default);
            var page = OCR1.Process(img);
            string res = page.GetText();
            page.Dispose();*///不自动处理内存 不使用
            using (var engine = new TesseractEngine(a + "\\tessdata", "jpn", EngineMode.Default))
            {
                using (var page = engine.Process(img))
                {
                    return page.GetText();
                }
            }
            Directory.SetCurrentDirectory(a);
        }
    }
}
