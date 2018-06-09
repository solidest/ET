using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ET.Interface
{
    /// <summary>
    /// <para>模块文件控制器接口定义</para>
    /// <para><c>IViewDoc</c>接口对应ET模块内MVVM模型的VM部分，通常为已加载模块文件内容的可视化组件</para>
    /// </summary>
    public interface IViewDoc
    {

        /// <summary>
        /// 获取已加载加载模块文件内容的组件UI
        /// </summary>
        /// <returns>可供XAML窗体加载的可视化组件</returns>
        UIElement GetUI();

        /// <summary>
        /// 获取最新的模块文档内容
        /// </summary>
        /// <returns>二进制序列化后的模块文件内容</returns>
        byte[] GetDocContent();

        /// <summary>
        /// 模块文档被保存后调用 通常用来处理修改状态
        /// </summary>
        void AfterSaved();


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
