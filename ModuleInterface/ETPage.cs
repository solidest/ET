﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ET.Interface
{
    /// <summary>
    /// <para>ET页面类</para>
    /// <para>ET页面继承自WPF的布局控件<c>Syste.Winows.Controls.Grid</c>，并加入ET自定义路由事件</para>
    /// <para>ET页面是ET模块中UI接口的统一包装器</para>
    /// </summary>
    public class ETPage :Grid
    {

        /// <summary>
        /// 模块文件内容被保存后触发该事件
        /// </summary>
        public readonly static RoutedEvent ETModuleFileSavedEvent = 
            EventManager.RegisterRoutedEvent("ETModuleFileSaved", RoutingStrategy.Bubble, typeof(EventHandler<ETEventArgs>), typeof(ETPage));

        /// <summary>
        /// CLR事件包装器
        /// </summary>
        public event RoutedEventHandler ETModuleFileSaved
        {
            add { this.AddHandler(ETModuleFileSavedEvent, value); }
            remove { this.RemoveHandler(ETModuleFileSavedEvent, value); }
        }


        /// <summary>
        /// 模块文件内容被修改后触发该事件
        /// </summary>
        public readonly static RoutedEvent ETModuleFileModifyEvent =
            EventManager.RegisterRoutedEvent("ETModuleFileModify", RoutingStrategy.Tunnel, typeof(EventHandler<ETEventArgs>), typeof(ETPage));

        /// <summary>
        /// CLR事件包装器
        /// </summary>
        public event RoutedEventHandler ETModuleFileModify
        {
            add { this.AddHandler(ETModuleFileModifyEvent, value); }
            remove { this.RemoveHandler(ETModuleFileModifyEvent, value); }
        }
    }
}
