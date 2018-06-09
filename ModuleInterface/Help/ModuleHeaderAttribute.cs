using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace ET.Interface
{

    /// <summary>
    /// ET模块的元数据信息，延迟加载时主程序只读取该属性信息
    /// </summary>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class)]
    public class ModuleHeaderAttribute : Attribute
    {
        /// <summary>
        /// ET模块被加载的时候使用
        /// </summary>
        /// <param name="pvs">键值对表示的属性赋值</param>
        public ModuleHeaderAttribute(IDictionary<String, Object> pvs)
        {
            ModuleKey = pvs["ModuleKey"].ToString();
            ModuleShowName = pvs["ModuleShowName"].ToString();
            ILevel = (Int32)pvs["ILevel"];
            IsOnlyOneFile = (Boolean)pvs["IsOnlyOneFile"];
        }

         
        /// <summary>
        ///  模块实现时使用的构造函数
        /// </summary>
        /// <param name="moduleKey">ET模块主键</param>
        /// <param name="moduleShowName">ET模块的显示名称</param>
        /// <param name="iLevel">模块等级</param>
        /// <param name="isOnlyOneFile">是否只支持单个文件</param>
        public ModuleHeaderAttribute(String moduleKey, String moduleShowName, Int32 iLevel, Boolean isOnlyOneFile)
        {
            ModuleKey = moduleKey;
            ModuleShowName = moduleShowName;
            ILevel = iLevel;
            IsOnlyOneFile = isOnlyOneFile;
        }

        /// <summary>
        /// ET模块主键
        /// </summary>
        public String ModuleKey { get; set; }

        /// <summary>
        /// ET模块显示名称
        /// </summary>
        public String ModuleShowName { get; set; }

        /// <summary>
        /// 模块等级，被用来在用户界面显示时排序
        /// </summary>
        public Int32 ILevel { get; set; }

        /// <summary>
        /// 是否只支持单个文件
        /// </summary>
        public bool IsOnlyOneFile { get; set; }

        /// <summary>
        /// ET模块图标
        /// </summary>
        public BitmapImage ModuleIcon { get; set; }

        /// <summary>
        /// 模块文件
        /// </summary>
        public BitmapImage ModuleFileIcon { get; set; }

    }

}
