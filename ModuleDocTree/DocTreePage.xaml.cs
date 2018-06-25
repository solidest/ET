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


        private void trMain_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var n = (trMain.SelectedItem as DocTreeNode);
            if (n == null)
            {
                miReName.IsEnabled = false;
            }
            else
            {
                miReName.IsEnabled = (trMain.SelectedItems.Count == 1) && !n.Parent.IsRoot;
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
            if (e.Parameter?.ToString() == "NewModuleFile")
            {

            }
            else if (e.Parameter?.ToString() == "NewFolder")
            {

            }
        }

        private void CanNewModuleFile(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Parameter?.ToString() == "NewModuleFile" )
            {
                var n = (trMain.SelectedItem as DocTreeNode);
                if (n == null)
                    e.CanExecute = false;
                else
                    e.CanExecute = (trMain.SelectedItems.Count == 1) && n.CanNewFile;
                e.Handled = true;
            }
            else if (e.Parameter?.ToString() == "NewFolder")
            {
                e.CanExecute = (trMain.SelectedItems.Count == 1) && ((trMain.SelectedItem as DocTreeFolderNode) != null);
                e.Handled = true;
            }
        }


        #endregion

    }
}
