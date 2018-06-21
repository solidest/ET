using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ET.Main.DocTree
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class DocTreePage : ET.Interface.ETPage
    {
        public DocTreePage()
        {
            InitializeComponent();
            trMain.ShowRoot = true;
        }

        private void DoNewModuleFile(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void CanNewModuleFile(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Parameter?.ToString() == "NewModuleFile")
            {
                var n = (trMain.SelectedItem as DocTreeNode);
                if (n == null)
                    e.CanExecute = false;
                else
                    e.CanExecute = n.CanNewFile;
                e.Handled = true;
            }
        }
    }
}
