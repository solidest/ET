using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Doc
{

    //单个文件的存储格式
    [Serializable]
    public class ModuleFile 
    {
        public String ModuleKey { get; set; }
        public String FileName { get; set; }
        public Byte[] Content { get; set; }

    }
}
