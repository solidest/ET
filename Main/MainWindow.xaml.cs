
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Input;
using ET.Interface;
using ET.Doc;
using ET.Service;
using System.Windows.Controls;
using System.Windows.Data;

namespace ET.Main
{

    public partial class MainWindow : Window, IMainService
    {

        //所有模块
        private Modules _ms = null;

        //活动VM
        private IViewDoc _activeVM = null;

        //文档结构树VM和Doc
        private ICommModule _docTreeModule = null;
        private IViewDoc _docTreeVM = null;

        //打开的操作系统文件全名
        private String __file = RevisionClass.DocVer;

        //所有打开的模块文件控制器句柄
        private List<IViewDoc> _open_vms = new List<IViewDoc>();

        //文档版本
        private int _docVersion = Convert.ToInt32( RevisionClass.DocVer);

        //输出信息
        private List<OutPutInfo> _outinfos = new List<OutPutInfo>();


        private String FileName
        {
            get
            {
                return __file;
            }
            set
            {
                __file = value;
                this.Title = Path.GetFileName(value) + " - " + RevisionClass.ProductName;
            }

        }


        #region --IMainService--


        //获取用户输入的一个字符串
        public string GetInput(string inputName, string defaultStr, ETService.ValidateStringCallBack valid)
        {
            var dlg = new InputDlg();
            dlg.caption.Text = "请在下面输入" + inputName + ":";
            dlg.input.Text = defaultStr;
            dlg.Validate = valid;
            dlg.Owner = this;
            if (dlg.ShowDialog() == true)
                return dlg.input.Text;
            else
                return "";
        }


        public IDictionary<string, ICommModule> Modules
        {
            get
            {
                return _ms;
            }
        }

        public Dictionary<string, ModuleHeaderAttribute> ModulesHeaders
        {
            get
            {
                return _ms.ModulesHeaders;
            }
        }

        //关闭模块文件
        public void CloseModuleFile(ModuleFile mf)
        {
            var vm = FindVM(mf);
            if(vm != null)
            {
                TabItem rmi = null;
                foreach(TabItem ti in tabPages.Items)
                {
                    if (ti.Tag == vm)
                    {
                        rmi = ti;
                        break;
                    }
                }
                if(rmi != null)
                {
                    tabPages.Items.Remove(rmi);
                    _open_vms.Remove(vm);
                }
            }
        }


        //打开模块文件
        public void OpenModuleFile(ModuleFile mf)
        {
            var vm = FindVM(mf);
            if(vm != null)
            {
                ActivePage(vm);
            }
            else
            {
                var pg = _ms[mf.ModuleKey].OpenFile(mf, _docVersion);
                AddPage(pg);
            }
        }

        public void ShowModuleFile(IViewDoc vd)
        {
            if (_open_vms.Contains(vd))
            {
                ActivePage(vd);
            }
            else
            {
                AddPage(vd);
            }
        }

        public void PrintInfo(OutPutInfo info, bool reset = false)
        {
            if(reset)
            {
                txtOut.Text = "";
                _outinfos.Clear();
            }
            txtOut.AppendText(info.Info + Environment.NewLine);
            _outinfos.Add(info);
        }


        #endregion

        #region --Main--

        public MainWindow()
        {
            InitializeComponent();
            IinitialMain();
        }

        private void IinitialMain()
        {
            //加载ET模块
            _ms = new Modules();

            //保存文档树模块
            _docTreeModule = _ms["DocTree"];

            //安装主服务
            ETService.SetupMainService(this);

            //分析命令行参数
            string[] pargs = Environment.GetCommandLineArgs();
            if(pargs!=null && pargs.Length>1)
            {
                string  fileName = pargs[pargs.Length-1];
                if (fileName.Length>3 && fileName.Substring(fileName.Length-3).Equals(".et"))
                {
                    if (System.IO.File.Exists(fileName))
                        OpenMainDoc(fileName);
                    else
                        OpenNewFile(fileName);
                }
            }
        }

        #endregion

        #region --Events--

        //窗体加载 UI初始化
        private void MainFormLoaded(object sender, RoutedEventArgs e)
        {
            var toolBarThumb = MainToolBar.Template.FindName("ToolBarThumb", MainToolBar) as FrameworkElement;
            if (toolBarThumb != null)
            {
                toolBarThumb.Visibility = Visibility.Collapsed;
            }

            var mainPanelBorder = MainToolBar.Template.FindName("MainPanelBorder", MainToolBar) as FrameworkElement;
            if (mainPanelBorder != null)
            {
                mainPanelBorder.Margin = new Thickness(0);
            }


        }


        private void DocTreeDispHide(object sender, RoutedEventArgs e)
        {
            switch (VSplitter.Visibility)
            {
                case Visibility.Visible:
                    VSplitter.Visibility = Visibility.Collapsed;
                    gridMain.ColumnDefinitions[0].Width = new GridLength(0, GridUnitType.Pixel);
                    tabPages.Focus();
                    break;

                case Visibility.Collapsed:
                    VSplitter.Visibility = Visibility.Visible;
                    gridMain.ColumnDefinitions[0].Width = new GridLength(300, GridUnitType.Pixel);
                    break;

            }
        }


        private void OutDispHide(object sender, RoutedEventArgs e)
        {
            switch (this.HSplitter.Visibility)
            {
                case Visibility.Visible:
                    HSplitter.Visibility = Visibility.Collapsed;
                    gridMain.RowDefinitions[3].Height = new GridLength(0, GridUnitType.Pixel);
                    break;
                case Visibility.Collapsed:
                    HSplitter.Visibility = Visibility.Visible;
                    gridMain.RowDefinitions[3].Height = new GridLength(300, GridUnitType.Pixel);
                    break;
            }
        }

        private void ActiveModule(object sender, RoutedEventArgs e)
        {
            if(tabPages.SelectedItem == null)
            {
                _activeVM = null;
            }
            else
            {
                _activeVM = (tabPages.SelectedItem as TabItem).Tag as IViewDoc;
            }
        }

        private void tabPages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabPages.SelectedItem == null)
            {
                _activeVM = null;
            }
            else
            {
                _activeVM = (tabPages.SelectedItem as TabItem).Tag as IViewDoc;
            }
        }

        //tab页面关闭之前
        private void tabPages_TabItemClosing(object sender, Wpf.Controls.TabItemCancelEventArgs e)
        {
            var vm = (e.TabItem.Tag as IViewDoc);
            if(vm != null && vm.IsModify)
            {
                //TODO MessageBox.Show("文件未保存!");
            }
            RemovePage(e.TabItem);
        }

        private void DeActiveModule(object sender, RoutedEventArgs e)
        {
            _activeVM = null;
        }


        public void SaveFile()
        {
            if (FileName != "") SaveMainDoc(FileName);
        }

        #endregion

        #region --Doc Operation--

        //新建文件
        private void OpenNewFile(string fileName)
        {
            _docTreeVM = _docTreeModule.OpenNewFile(fileName);
            tbDocTree.Content = _docTreeVM.PageUI;
            FileName = fileName;
            SaveFile();
        }

        //打开文件
        private void OpenMainDoc(string fileName)
        {
            byte[] bver = new byte[4];
            byte[] content = null;

            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                byte[] blen = new byte[4];

                fs.Read(bver, 0, 4);
                fs.Read(blen, 0, 4);
                content = new byte[BitConverter.ToInt32(blen,0)];
                fs.Read(content, 0, content.Length);
            }

            int _docVersion = BitConverter.ToInt32(bver, 0);
            if (_docVersion > Helper.CurrentAppDocVer()) throw new ETException("MAIN", "程序版本过低，打开文档失败！");
            using (var ms = new MemoryStream(content))
            {
                var formatter = new BinaryFormatter();
                var mf = formatter.Deserialize(ms) as ModuleFile;
                _docTreeVM = _docTreeModule.OpenFile(mf, _docVersion);
            }

            tbDocTree.Content = _docTreeVM.PageUI;
            FileName = fileName;
    }


        //保存文件
        private void SaveMainDoc(String fname)
        {
            _docTreeVM.SaveContent();

            byte[] content = null;
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, _docTreeVM.MFile);
                content = ms.GetBuffer();
            }

            using (var fs = new FileStream(fname, FileMode.Create))
            {
                Int32 version = Convert.ToInt32(RevisionClass.DocVer);
                fs.Write(Helper.ConvertIntToByteArray(version), 0, 4);          //写入文档版本
                fs.Write(Helper.ConvertIntToByteArray(content.Length), 0, 4);   //写入内容长度
                fs.Write(content, 0, content.Length);                           //写入文档内容
            }

            FileName = fname;
        }


        private void SaveMainDoc()
        {
            if (FileName == "")
            {
                var sfd = new Microsoft.Win32.SaveFileDialog
                {
                    DefaultExt = ".et",
                    Filter = "ET file|*.et",
                    Title = "保存"
                };
                if (sfd.ShowDialog() == true)
                {
                    SaveMainDoc(sfd.FileName);
                }
            }
            else
                SaveMainDoc(FileName);
        }

        #endregion

        #region --Command Operation--


        //新建项目文件
        private void DoNewProject(object sender, ExecutedRoutedEventArgs e)
        {
            var sfd = new Microsoft.Win32.SaveFileDialog
            {
                DefaultExt = ".et",
                Filter = "ET file|*.et",
                Title = "新建"
            };
            if (sfd.ShowDialog() == true)
            {
                if (File.Exists(sfd.FileName)) File.Delete(sfd.FileName);
                if (_docTreeVM != null)
                {
                    System.Diagnostics.Process.Start(this.GetType().Assembly.Location, sfd.FileName);
                }
                else
                {
                    OpenNewFile(sfd.FileName);
                }

            }
            e.Handled = true;

        }

        private void CanNewProject(object sender, CanExecuteRoutedEventArgs e)
        {
           if(e.Parameter.ToString() == "NewProject")
            {
                e.CanExecute = true;
                e.Handled = true;
            }
        }


        private void DoOpenDoc(object sender, ExecutedRoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".et",
                Filter = "ET file|*.et"
            };
            if (ofd.ShowDialog() == true && ofd.FileName != FileName)
            {
                if(_docTreeVM != null)
                    System.Diagnostics.Process.Start(this.GetType().Assembly.Location,  "\"" + ofd.FileName + "\""  );
                else
                    OpenMainDoc(ofd.FileName);
            }
            e.Handled = true;
        }


        private void CanSaveAll(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (_docTreeVM != null);
            e.Handled = true;
        }

        //全部保存
        private void DoSaveAll(object sender, ExecutedRoutedEventArgs e)
        {
            foreach (var f in _open_vms) f.SaveContent();
            SaveMainDoc();
            foreach (var vm in _open_vms)
            {
                vm.PageUI.RaiseEvent(new ETEventArgs(ETPage.ETModuleFileSavedEvent, vm.PageUI, vm));
            }
            e.Handled = true;
        }

        private void CanSaveDoc(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (_activeVM != null && _activeVM != _docTreeVM);
            e.Handled = true;
        }

        //保存当前
        private void DoSaveDoc(object sender, ExecutedRoutedEventArgs e)
        {

            _activeVM?.SaveContent();
            SaveMainDoc();
            _activeVM?.PageUI.RaiseEvent(new ETEventArgs(ETPage.ETModuleFileSavedEvent, _activeVM.PageUI, _activeVM));
            e.Handled = true;
        }

        #endregion

        #region --Helper--

        private IViewDoc FindVM(ModuleFile mf)
        {
            foreach(var vm in _open_vms)
            {
                if (vm.MFile == mf) return vm;
            }
            return null;
        }

        //新建Tab页面 
        private void AddPage(IViewDoc pg)
        {
            tabPages.AddNewTabToEnd = true;
            tabPages.AddTabItem();
            TabItem ti = tabPages.Items[tabPages.Items.Count - 1] as TabItem;

            ti.Content = pg.PageUI;
            ti.Tag = pg;

            var mb = new MultiBinding() { Mode = BindingMode.OneWay};
            mb.Bindings.Add(new Binding("FileName") { Source = pg.MFile});
            mb.Bindings.Add(new Binding("IsModify") { Source = pg});
            mb.Converter = new HeaderMutiBindingConverter();
            ti.SetBinding(TabItem.HeaderProperty, mb);

            _open_vms.Add(pg);
        }

        private void ActivePage(IViewDoc vm)
        {
            foreach(TabItem ti in tabPages.Items)
            {
                if (ti.Tag == vm)
                {
                    tabPages.SelectedItem = ti;
                    break;
                }
            }
        }

        private void RemovePage(TabItem ti)
        {
            IViewDoc rem = null;
            foreach(var p in _open_vms)
            {
                if (p == ti.Tag) rem = p;
            }
            if(rem!=null)
            {
                _open_vms.Remove(rem);
            }
        }

        private void LoadDocTree(ETPage pg)
        {
            tbDocTree.Content = pg;
            pg.GotFocus += ActiveDocTree;
            pg.LostFocus += DeActiveDocTree;
        }

        private void DeActiveDocTree(object sender, RoutedEventArgs e)
        {
            _activeVM = null;
        }

        private void ActiveDocTree(object sender, RoutedEventArgs e)
        {
            _activeVM = _docTreeVM;
        }



        #endregion

    }
}
