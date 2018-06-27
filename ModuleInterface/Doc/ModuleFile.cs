using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Doc
{

    /// <summary>
    /// <list type="bullet">
    /// <item>模块文件类封装模块文件的外部属性和内部内容（释：模块文件是ET模块产生的内容块）</item>
    /// <item>该类是模块文件的通用属性封装</item>
    /// </list>
    /// </summary>
    [Serializable]
    public class ModuleFile
    {
        /// <summary>
        /// 模块文件类唯一的构造函数
        /// </summary>
        /// <param name="mKey">对应模块的主键</param>
        /// <param name="fName">模块文件名</param>
        /// <param name="content">模块文件的内容，默认为空</param>
        public ModuleFile(String mKey, String fName, Byte[] content=null )
        {
            ModuleKey = mKey;
            FileName = fName;
            Content = content;
        }

        /// <summary>
        /// 模块文件对应模块的主键
        /// </summary>
        public String ModuleKey { get; set; }

        /// <summary>
        /// 模块文件的文件名
        /// </summary>
        public String FileName { get; set; }

        /// <summary>
        /// 模块文件的具体内容
        /// <note type="implement">
        ///<c>Content</c>返回的字节数组是具体的模块文件内容进行二进制序列化得到的，可用于持久化和反序列化
        /// </note>
        /// </summary>
        public Byte[] Content { get; set; }
    }
}
