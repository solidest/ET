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
using ET.Doc;
using ET.Interface;

namespace ET.Main.DocTree
{

    public partial class DocTreePage : ETPage
    {
        public DocTreePage()
        {
            InitializeComponent();
            trMain.ShowRoot = false;
        }


        #region --Events--


        private void trMain_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var n = (trMain.SelectedItem as DocTreeFileNode);
            if (n != null)
            {
                 ET.Service.ETService.MainService.OpenModuleFile(n.MFile);
            }
        }

        private void miOpenFile_Click(object sender, RoutedEventArgs e)
        {
            var n = (trMain.SelectedItem as DocTreeFileNode);
            if (n != null)
            {
                ET.Service.ETService.MainService.OpenModuleFile(n.MFile);
            }
        }

        private void trMain_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var n = (trMain.SelectedItem as DocTreeNode);
            if (n == null)
            {
                miOpenFile.IsEnabled = false;
                miReName.IsEnabled = false;
            }
            else
            {
                miOpenFile.IsEnabled = (n.GetType() == typeof(DocTreeFileNode));
                miReName.IsEnabled = !n.Parent.IsRoot;
            }
        }


        private void miReName_Click(object sender, RoutedEventArgs e)
        {
            var n = (trMain.SelectedItem as DocTreeNode);
            if (n != null)
            {
                n.IsEditing = true;
            }
        }

        #endregion


        #region --Commands--

        private void DoNewModuleFile(object sender, ExecutedRoutedEventArgs e)
        {
            //TODO 新建模块文件
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


        #endregion

    }
}
