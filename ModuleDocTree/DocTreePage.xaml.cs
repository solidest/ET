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
            var name = ET.Service.ETService.MainService.GetInput("文件名", "", this.validFileName);
            if (string.Empty != name)
            {
                var n = (trMain.SelectedItem as DocTreeFolderNode);
                var vm = ETService.MainService.Modules[n.ModuleKey].OpenNewFile(name);
                n.AddChild(new DocTreeFileNode(vm.MFile));
                ETService.MainService.ShowModuleFile(vm);
            }
        }

        private void NewFolder(object sender, RoutedEventArgs e)
        {
            var name = ET.Service.ETService.MainService.GetInput("文件夹名称", "", this.validFolderName);
            if (string.Empty != name)
            {

            }
            e.Handled = true;
        }

        #endregion

        #region --Helper--
        private string validFileName(string input)
        {
            return "";
        }

        private string validFolderName(string input)
        {
            return "输入无效";
        }

        #endregion

    }
}
