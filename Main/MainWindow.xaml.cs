﻿
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Input;
using ET.Interface;
using ET.Doc;
using ET.Service;

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

        public List<ModuleHeaderAttribute> ModulesHeaders
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
            _ms.InitialModules();

            //保存文档树模块
            _docTreeModule = _ms["DocTree"];

            //安装主服务
            ETService.SetupMainService(this);

        }

        #endregion

        #region --Events--


        private void ActiveModule(object sender, RoutedEventArgs e)
        {
           
        }

        private void DeActiveModule(object sender, RoutedEventArgs e)
        {
            _activeVM = null;
        }
        #endregion

        #region --Doc Operation--

        [Serializable]
        private class MainDoc
        {
            public int DocVersion
            {
                get
                {
                    return Convert.ToInt32(RevisionClass.DocVer);
                }
            }
            public byte[] Content { get; set; }
        }

        //显示文档结构树
        private void ShowDocTree()
        {
            if (panelDocTree.Children.Count == 0)
                panelDocTree.Children.Add(_docTreeVM.PageUI);
        }


        //打开文件
        private void OpenMainDoc(string fileName)
        {
            MainDoc md = null;
            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                md = bf.Deserialize(fs) as MainDoc;
            }

            if (md.DocVersion > Convert.ToInt32(RevisionClass.DocVer)) throw new ETException("MAIN", "程序版本过低，打开文档失败！");
            using (var ms = new MemoryStream(md.Content))
            {
                var formatter = new BinaryFormatter();
                var mf = formatter.Deserialize(ms) as ModuleFile;
                _docTreeVM = _docTreeModule.OpenFile(mf, md.DocVersion);
            }

            FileName = fileName;
            ShowDocTree();
        }

        //保存文件
        private void SaveMainDoc(String fname)
        {
            _docTreeVM.UpdateContent();

            var md = new MainDoc();
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, _docTreeVM.PageFile);
                md.Content = ms.GetBuffer();
            }

            using (var fs = new FileStream(fname, FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, md);
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
            panelDocTree.Children.Clear();
            return true;
        }


        #endregion

        #region --Command Operation--

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
                var ete = new ETEventArgs(ETPage.ETModuleFileSavedEvent, vm.PageUI, vm);
                vm.PageUI.RaiseEvent(ete);
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
            var ete = new ETEventArgs(ETPage.ETModuleFileSavedEvent, _activeVM.PageUI, _activeVM);
            _activeVM.PageUI.RaiseEvent(ete);
        }
        private void CanCopy(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _activeVM == null ? false : _activeVM.CanCopy();
        }

        private void DoCopy(object sender, ExecutedRoutedEventArgs e)
        {
            _activeVM?.DoCopy();
        }

        private void CanPaste(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _activeVM == null ? false : _activeVM.CanPaste();
        }
        private void DoPaste(object sender, ExecutedRoutedEventArgs e)
        {
            _activeVM?.DoPaste();
        }

        private void CanRedo(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _activeVM == null ? false : _activeVM.CanRedo();
        }

        private void DoRedo(object sender, ExecutedRoutedEventArgs e)
        {
            _activeVM?.DoRedo();
        }

        private void CanUndo(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _activeVM == null ? false : _activeVM.CanUndo();
        }

        private void DoUndo(object sender, ExecutedRoutedEventArgs e)
        {
            _activeVM?.DoUndo();
        }

        #endregion

    }
}
