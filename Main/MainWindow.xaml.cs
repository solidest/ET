
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

        #region --Property--

        //所有模块
        private Modules _ms = null;

        //活动VM
        private IViewDoc _activeVM = null;

        //文档结构树VM和Doc
        private ICommModule _docTreeModule = null;
        private IViewDoc _docTreeVM = null;

        //打开的操作系统文件全名
        private String __file = "";

        //所有打开的模块文件控制器句柄
        private List<IViewDoc>  _open_vms = new List<IViewDoc>();

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

        #endregion


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

        public void OpenModuleFile(ModuleFile mfile)
        {
            //TODO:打开模块文件
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
           
        }

        private void DeActiveModule(object sender, RoutedEventArgs e)
        {
            _activeVM = null;
        }
        #endregion

        #region --Doc Operation--

        //显示文档结构树
        private void ShowDocTree()
        {
            gridMain.Children.Add(_docTreeVM.PageUI);
            Grid.SetRow(_docTreeVM.PageUI, 1);
            Grid.SetColumn(_docTreeVM.PageUI, 0);
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

            Int32 iver = BitConverter.ToInt32(bver, 0);
            if (iver > Helper.CurrentAppDocVer()) throw new ETException("MAIN", "程序版本过低，打开文档失败！");
            using (var ms = new MemoryStream(content))
            {
                var formatter = new BinaryFormatter();
                var mf = formatter.Deserialize(ms) as ModuleFile;
                _docTreeVM = _docTreeModule.OpenFile(mf, iver);
            }

            FileName = fileName;
            ShowDocTree();
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

        //关闭当前文档
        private bool CloseMainDoc()
        {
            //TODO 提示保存

            //清理内部变量
            _activeVM = null;
            _docTreeVM = null;
            FileName = "";
            _open_vms.Clear();
            
            return true;
        }


        #endregion

        #region --Command Operation--


        //新建文件
        private void DoNewDoc(object sender, ExecutedRoutedEventArgs e)
        {
            if (CloseMainDoc())
            {
                _docTreeVM = _docTreeModule.OpenNewFile();
                ShowDocTree();
            }
        }


        private void DoOpenDoc(object sender, ExecutedRoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".et",
                Filter = "ET file|*.et"
            };
            if (ofd.ShowDialog() == true)
            {
                if (CloseMainDoc()) OpenMainDoc(ofd.FileName);
            }
        }


        private void CanSaveAll(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (_docTreeVM != null);
        }

        private void DoSaveAll(object sender, ExecutedRoutedEventArgs e)
        {
            foreach (var f in _open_vms) f.UpdateContent();
            SaveMainDoc();
            foreach (var vm in _open_vms)
            {
                vm.PageUI.RaiseEvent(new ETEventArgs(ETPage.ETModuleFileSavedEvent, vm.PageUI, vm));
            }
        }

        private void CanSaveDoc(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (_activeVM != null);
        }

        private void DoSaveDoc(object sender, ExecutedRoutedEventArgs e)
        {

            if (_activeVM != null) _activeVM.UpdateContent();
            SaveMainDoc();
            _activeVM.PageUI.RaiseEvent(new ETEventArgs(ETPage.ETModuleFileSavedEvent, _activeVM.PageUI, _activeVM));
        }
       
        #endregion

    }
}
