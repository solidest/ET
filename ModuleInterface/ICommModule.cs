using System;
using ET.Doc;
using System.Windows.Media.Imaging;

namespace ET.Interface
{
    /// <summary>
    /// <para>ET模块的通用接口定义，所有的ET模块均需要实现此接口</para>
    /// <para>ET设计器的每个功能均由独立的ET模块实现，ET模块内部采用MVVM模型实现其功能</para>
    /// </summary>
    public interface ICommModule
    {
        /// <summary>
        /// 用户选择新建文件时被调用
        /// </summary>
        /// <returns>新创建的模块文件数组</returns>
        IViewDoc OpenNewFile(string name);

        /// <summary>
        /// 打开模块文件
        /// </summary>
        /// <param name="mf">模块文件</param>
        /// <param name="version">主文件的版本，每个ET模块的实现中需保证模块文件内容向后兼容</param>
        /// <returns>返回加载内容后的<c>IViewDoc</c>接口</returns>
        IViewDoc OpenFile(ModuleFile0 mf, Int32 version);


        /// <summary>
        /// 加载模块文件
        /// </summary>
        /// <param name="mf">模块文件</param>
        /// <param name="version">主文件的版本，每个ET模块的实现中需保证模块文件内容向后兼容</param>
        /// <returns>返回模块文件的文档对象</returns>
        object LoadFile(ModuleFile0 mf, Int32 version);


        /// <summary>
        /// ET模块的图标
        /// </summary>
        BitmapImage ModuleIcon { get; }

        /// <summary>
        /// 模块文件的图标
        /// </summary>
        BitmapImage FileIcon { get;}

        /// <summary>
        /// 模块打开时的图标
        /// </summary>
        BitmapImage OpenModuleIcon { get; }

    }

}

