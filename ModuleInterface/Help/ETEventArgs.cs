
using System.Windows;

namespace ET.Interface
{
    /// <summary>
    /// ET路由事件使用的事件参数类
    /// </summary>
    public class ETEventArgs:RoutedEventArgs
    {
        /// <summary>
        /// 重载WPF路由事件参数类的构造函数
        /// </summary>
        /// <param name="routedEvent">路由事件</param>
        /// <param name="source">事件源</param>
        /// <param name="eventHostVM">事件宿主控制器</param>
        /// <param name="tag">带外数据</param>
        public ETEventArgs(RoutedEvent routedEvent, object source, IViewDoc eventHostVM, object tag=null) : base(routedEvent, source)
        {
            EventHostVM = eventHostVM;
            Tag = tag;
        }

        /// <summary>
        ///  事件的宿主模块文件控制器
        /// </summary>
        public IViewDoc EventHostVM { get; private set; }


        /// <summary>
        /// 带外数据
        /// </summary>
        public object Tag { get; set; }
    }
}
