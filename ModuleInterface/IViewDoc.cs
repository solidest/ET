using System;
using System.Collections.Generic;
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
    public interface IViewDoc
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
        void UpdateContent();

        /// <summary>
        /// 是否可以执行复制
        /// <note type="implement">该函数会被频繁调用</note>
        /// </summary>
        /// <returns>可以复制返回<c>true</c>，否则返回<c>false</c></returns>
        /// <note type="implement">该函数会被频繁调用</note>
        bool CanCopy();

        /// <summary>
        /// 执行复制
        /// </summary>
        void DoCopy();

        /// <summary>
        /// 是否可以执行粘贴
        /// <note type="implement">该函数会被频繁调用</note>
        /// </summary>
        /// <returns>可以粘贴返回<c>true</c>，否则返回<c>false</c></returns>
        bool CanPaste();

        /// <summary>
        /// 执行粘贴
        /// </summary>
        void DoPaste();

        /// <summary>
        /// 是否可以执行重做
        /// <note type="implement">该函数会被频繁调用</note>
        /// </summary>
        /// <returns>可以重做返回<c>true</c>，否则返回<c>false</c></returns>
        /// <note type="implement">该函数会被频繁调用</note>
        bool CanRedo();

        /// <summary>
        /// 执行重做
        /// </summary>
        void DoRedo();

        /// <summary>
        /// 是否可以执行撤销
        /// <note type="implement">该函数会被频繁调用</note>
        /// </summary>
        /// <returns>可以撤销返回<c>true</c>，否则返回<c>false</c></returns>
        /// <note type="implement">该函数会被频繁调用</note>
        bool CanUndo();

        /// <summary>
        /// 执行撤销
        /// </summary>
        void DoUndo();

    }
}
