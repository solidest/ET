using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace Main
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
         {
             base.OnStartup(e);
 
             //注册Application_Error
             this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
 
         }
 
         //异常处理逻辑
         void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
         {
            if(e.Exception.GetType() == typeof(ET.Interface.ETException))
            {
                MessageBox.Show(e.Exception.ToString());
                e.Handled = true;
            }

         }
    }
}
