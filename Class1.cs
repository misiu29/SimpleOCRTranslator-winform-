using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2
{
    class TransObj
    {
        //注意：就是此类的三个属性名称必须和json数据里面的key一致
        public string from { set; get; }
        public string to { set; get; }
        public List<TransResult> trans_result { set; get; }
    }
    class TransResult
    {
        public string src { set; get; }
        public string dst { set; get; }
    }
}
