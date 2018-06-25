
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

        //所有打开的模块文件
        private List<ModuleFile0> _open_fs = new List<ModuleFile0>();

        //所有打开的模块文件控制器句柄
        private List<IViewDoc> _open_vms = new List<IViewDoc>();

        //文档版本
        private int _docVersion;


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

        public int DocVersion => _docVersion;

        public void OpenModuleFile(ModuleFile0 mf)
        {
            if(_open_fs.Contains(mf))
            {
                ActivePage(mf);
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
                ActivePage(vd.MFile);
            }
            else
            {
                AddPage(vd);
            }
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
                bool isNew = false;
                string fileName = "";
                for(int i=0; i<pargs.Length; i++)
                {
                    if (pargs[i] == "-n")
                        isNew = true;
                    else if (i == pargs.Length - 1)
                        fileName = pargs[i];
                }

                if (isNew)
                {
                        _docTreeVM = _docTreeModule.OpenNewFile("");
                        tbDocTree.Content = _docTreeVM.PageUI;
                        FileName = "";
              }
                else
                {
                    if (System.IO.File.Exists(fileName))
                    {
                        OpenMainDoc(fileName);
                    }
                }
            }
            
           

        }

        #endregion

        #region --Events--


        private void DocTreeDispHide(object sender, RoutedEventArgs e)
        {
            switch (VSplitter.Visibility)
            {
                case Visibility.Visible:
                    VSplitter.Visibility = Visibility.Collapsed;
                    gridMain.ColumnDefinitions[0].Width = new GridLength(0, GridUnitType.Pixel);
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
           //TODO 更新活动模块控制器句柄
        }

        private void tabPages_TabItemClosing(object sender, Wpf.Controls.TabItemCancelEventArgs e)
        {
            //TODO 提示保存
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


        //新建项目文件
        private void DoNewDoc(object sender, ExecutedRoutedEventArgs e)
        {
            if (_docTreeVM != null)
            {
                System.Diagnostics.Process.Start("main.exe", "-n");
            }
            else
            {
                _docTreeVM = _docTreeModule.OpenNewFile("");
                tbDocTree.Content = _docTreeVM.PageUI;
                FileName = "";
            }
            e.Handled = true;

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

            _docVersion = BitConverter.ToInt32(bver, 0);
            if (_docVersion > Helper.CurrentAppDocVer()) throw new ETException("MAIN", "程序版本过低，打开文档失败！");
            using (var ms = new MemoryStream(content))
            {
                var formatter = new BinaryFormatter();
                var mf = formatter.Deserialize(ms) as ModuleFile0;
                _docTreeVM = _docTreeModule.OpenFile(mf, _docVersion);
            }

            tbDocTree.Content = _docTreeVM.PageUI;
            FileName = fileName;
    }


        //保存文件
        private void SaveMainDoc(String fname)
        {
            _docTreeVM.UpdateContent();

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
                    System.Diagnostics.Process.Start("main.exe",  "\"" + ofd.FileName + "\""  );
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

        private void DoSaveAll(object sender, ExecutedRoutedEventArgs e)
        {
            foreach (var f in _open_vms) f.UpdateContent();
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

        private void DoSaveDoc(object sender, ExecutedRoutedEventArgs e)
        {

            _activeVM?.UpdateContent();
            SaveMainDoc();
            _activeVM?.PageUI.RaiseEvent(new ETEventArgs(ETPage.ETModuleFileSavedEvent, _activeVM.PageUI, _activeVM));
            e.Handled = true;
        }

        #endregion

        #region --Helper--


        private void AddPage(IViewDoc pg)
        {
            tabPages.AddNewTabToEnd = true;
            tabPages.AddTabItem();
            TabItem ti = tabPages.Items[tabPages.Items.Count - 1] as TabItem;

            ti.Content = pg.PageUI;
            
            if (pg.MFile.FileName.Length == 0)
                ti.Header = "未命名";
            else
                ti.Header = pg.MFile.FileName;

            _open_fs.Add(pg.MFile);
            _open_vms.Add(pg);
        }

        private void ActivePage(ModuleFile0 mf)
        {

        }

        private void RemovePage(TabItem ti)
        {
            IViewDoc rem = null;
            foreach(var p in _open_vms)
            {
                if (p.PageUI == ti.Content) rem = p;
            }
            if(rem!=null)
            {
                _open_fs.Remove(rem.MFile);
                _open_vms.Remove(rem);
            }
        }


        #endregion

    }
}
