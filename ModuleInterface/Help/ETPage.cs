using System;
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
        /// 注册路由事件 模块文件内容被保存后触发该事件
        /// </summary>
        public readonly static RoutedEvent ETModuleFileSavedEvent = 
            EventManager.RegisterRoutedEvent("ETModuleFileSaved", RoutingStrategy.Bubble, typeof(EventHandler<ETEventArgs>), typeof(ETPage));

        /// <summary>
        /// 模块文件内容被保存后触发该事件，通常由ET模块外部触发该事件并在模块内部进行处理
        /// </summary>
        public event RoutedEventHandler ETModuleFileSaved
        {
            add { this.AddHandler(ETModuleFileSavedEvent, value); }
            remove { this.RemoveHandler(ETModuleFileSavedEvent, value); }
        }


        /// <summary>
        /// 注册路由事件 模块文件内容被修改后触发该事件
        /// </summary>
        public readonly static RoutedEvent ETModuleFileModifyEvent =
            EventManager.RegisterRoutedEvent("ETModuleFileModify", RoutingStrategy.Tunnel, typeof(EventHandler<ETEventArgs>), typeof(ETPage));

        /// <summary>
        /// 模块文件内容被修改后触发该事件，通常由ET模块内部触发该事件并向模块外部进行事件广播
        /// </summary>
        public event RoutedEventHandler ETModuleFileModify
        {
            add { this.AddHandler(ETModuleFileModifyEvent, value); }
            remove { this.RemoveHandler(ETModuleFileModifyEvent, value); }
        }



        /// <summary>
        /// 注册路由事件 打开模块文件事件
        /// </summary>
        public readonly static RoutedEvent ETModuleFileOpenEvent =
            EventManager.RegisterRoutedEvent("ETModuleFileOpen", RoutingStrategy.Tunnel, typeof(EventHandler<ETEventArgs>), typeof(ETPage));

        /// <summary>
        /// 模块文件内容被修改后触发该事件，通常由ET模块内部触发该事件并向模块外部进行事件广播
        /// </summary>
        public event RoutedEventHandler ETModuleFileOpen
        {
            add { this.AddHandler(ETModuleFileOpenEvent, value); }
            remove { this.RemoveHandler(ETModuleFileOpenEvent, value); }
        }

    }
}
