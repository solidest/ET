using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using ET.Service;

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
                miAddFolder.IsEnabled = false;
                miAddItem.IsEnabled = false;
            }
            else
            {
                miAddFolder.IsEnabled = (trMain.SelectedItems.Count == 1) && ((n as DocTreeFolderNode) != null);
                miAddItem.IsEnabled = (trMain.SelectedItems.Count == 1) && n.CanNewFile;
                miReName.IsEnabled = (trMain.SelectedItems.Count == 1) && !n.Parent.IsRoot;
            }
            miAddItem.Header = miAddItem.IsEnabled ? "新建"+Service.ETService.MainService.ModulesHeaders[n.ModuleKey].ModuleShowName: "新建项";
        }


        private void miReName_Click(object sender, RoutedEventArgs e)
        {
            var n = (trMain.SelectedItem as DocTreeNode);
            if (n != null)
            {
                n.IsEditing = true;
            }
        }
        private void NewModuleFile(object sender, RoutedEventArgs e)
        {
            var n = (trMain.SelectedItem as DocTreeFolderNode);
            var name = ET.Service.ETService.MainService.GetInput("文件名", "", n.validFileName);
            if (string.Empty != name)
            {
                var vm = ETService.MainService.Modules[n.ModuleKey].OpenNewFile(name);
                n.AddChild(new DocTreeFileNode(vm.MFile));
                ETService.MainService.ShowModuleFile(vm);
            }
        }

        private void NewFolder(object sender, RoutedEventArgs e)
        {
            var n = (trMain.SelectedItem as DocTreeFolderNode);
            var name = ET.Service.ETService.MainService.GetInput("文件夹名称", "", n.validFolderName);
            if (string.Empty != name)
            {
                n.AddChild(new DocTreeFolderNode(new DirNode(n.ModuleKey, name)));
            }
            e.Handled = true;
        }

        #endregion

    }
}
