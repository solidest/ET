using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using ET.Doc;

namespace ET.Interface
{
    /// <summary>
    /// <para>模块文件控制器接口定义</para>
    /// <para><c>IViewDoc</c>接口对应ET模块内MVVM模型的VM部分，通常为已加载模块文件内容的可视化组件</para>
    /// </summary>
    public interface IViewDoc : INotifyPropertyChanged
    {

        /// <summary>
        /// ET页面，可供XAML窗体加载，对应已加载加载模块文件内容的UI
        /// </summary>
        ETPage PageUI { get; }

        /// <summary>
        /// 模块文件
        /// </summary>
        ModuleFile MFile { get; }
        
        /// <summary>
        /// 所属的ET模块主键
        /// </summary>
        String ModuleKey { get; }

        /// <summary>
        /// 模块文件是否自动保存
        /// </summary>
        bool IsAutoSave { get; set; }

        /// <summary>
        /// 更新最新的模块文档内容，即:将内容持久化并保存到<c>PageFile.Content</c>
        /// </summary>
        void SaveContent();

        /// <summary>
        /// 文档是否发生改变
        /// </summary>
        bool IsModify { get; set; }
        

    }
}
