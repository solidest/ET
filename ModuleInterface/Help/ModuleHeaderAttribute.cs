using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;


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
            ModuleFileType = (ETModuleFileTypeEnum)pvs["ModuleFileType"];
        }

         
        /// <summary>
        ///  模块实现时使用的构造函数
        /// </summary>
        /// <param name="moduleKey">ET模块主键</param>
        /// <param name="moduleShowName">ET模块的显示名称</param>
        /// <param name="iLevel">模块等级</param>
        /// <param name="fileType">是否只支持单个文件</param>
        public ModuleHeaderAttribute(String moduleKey, String moduleShowName, Int32 iLevel, ETModuleFileTypeEnum fileType)
        {
            ModuleKey = moduleKey;
            ModuleShowName = moduleShowName;
            ILevel = iLevel;
            ModuleFileType = fileType;
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
        public ETModuleFileTypeEnum ModuleFileType { get; set; }

    }



    /// <summary>
    /// ET模块支持的文件类型
    /// </summary>
    public enum ETModuleFileTypeEnum
    {
        /// <summary>
        /// 多个自定义模块文件
        /// </summary>
        MultiCustom,

        /// <summary>
        /// 仅一份模块文件
        /// </summary>
        OnlyOneFile,

        /// <summary>
        /// 不包含模块文件
        /// </summary>
        NoneFile
    }



}
