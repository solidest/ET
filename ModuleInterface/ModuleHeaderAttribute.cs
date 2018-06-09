using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace ET.Interface
{

    //模块头元数据 延迟加载时只读取模块头
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class)]
    public class ModuleHeaderAttribute : Attribute
    {
        //模块主键
        public String ModuleKey { get; set; }

        //模块显示名称
        public String ModuleShowName { get; set; }

        //模块等级 排序使用
        public Int32 ILevel { get; set; }

        //是否只支持单个文件
        public bool IsOnlyOneFile { get; set; }

        public BitmapImage ModuleIcon { get; set; }

        public BitmapImage FileIcon { get; set; }

    }

}
