using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;

namespace WindowsFormsApp1
{
    class Dreyestrans
    {
        //译典通引用参考：https://www.lgztx.com/?p=209
        //const string DREYE_DLL = @"DReye\DreyeMT\SDK\bin\TransCOM.dll";
        const int EC_DAT = 1;   //英中
        const int CE_DAT = 2;   //中英
        const int CJ_DAT = 3;   //中日
        const int JC_DAT = 10;  //日中

        [DllImport("TransCOM.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int MTInitCJ(int dat_index);

        [DllImport("TransCOM.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int MTEndCJ();

        [DllImport("TransCOM.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int TranTextFlowCJ(
            byte[] src,
            byte[] dest,
            int dest_size,
            int dat_index
            );
        public string Translate(string sourceText)
        {
            string ret;
            Encoding shiftjis = Encoding.GetEncoding("shift-jis"); //英中utf-8
            Encoding gbk = Encoding.GetEncoding("gbk");
            
            //string currentpath= Environment.CurrentDirectory;
            string workingDirectory = @"DR.eye\DreyeMT\SDK\bin";
            Directory.SetCurrentDirectory(workingDirectory);
            MTInitCJ(JC_DAT); //返回值为-255
            byte[] src = shiftjis.GetBytes(sourceText);
            byte[] buffer = new byte[3000];
            TranTextFlowCJ(src, buffer, 3000, JC_DAT);
            ret = gbk.GetString(buffer);
            MTEndCJ();
            return ret;
        }
    }
}
