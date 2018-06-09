
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Input;
using ET.Interface;
using ET.Doc;

namespace ET.Main
{

    public partial class MainWindow : Window
    {

        #region --Property--

        //所有模块
        private Modules _ms = null;

        //活动VM
        private IViewDoc _activeVM = null;

        //文档
        private MainDocument _doc = null;

        //打开的操作系统文件全名
        private String __file = "";

        //所有打开的内部文件
        private Dictionary<IViewDoc, ModuleFile> _open_docs = new Dictionary<IViewDoc, ModuleFile>();

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

        #region --Main--

        public MainWindow()
        {
            InitializeComponent();
            LoadAllModules();
        }

        private void LoadAllModules()
        {
            _ms = new Modules();
            _ms.InitialModules();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region --Events--

        private void ActiveModule(object sender, RoutedEventArgs e)
        {
            if (sender == tabPages)
            {

            }
            else if (sender == trDocs)
            {

            }
        }

        private void DeActiveModule(object sender, RoutedEventArgs e)
        {
            _activeVM = null;
        }
        #endregion

        #region --Doc Operation--


        //创建一个空文件
        private MainDocument GetNewMainDoc()
        {
            var mdoc = new MainDocument();

            foreach (var m in _ms.Values)
            {
                var fs = m.GetNewFiles();
                if (fs == null) continue;
                foreach (var f in fs)
                {
                    mdoc.AddFileContent(f);
                }
            }

            return mdoc;
        }

        //从文件打开文档
        private void OpenMainDoc(string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                OpenMainDoc(bf.Deserialize(fs) as MainDocument);
            }
            FileName = fileName;
        }

        //从对象打开文档
        private void OpenMainDoc(MainDocument doc)
        {
            //TODO 判断文件版本 如果需要进行升级处理
            _doc = doc;
            //TODO 打开文件内容
        }

        //保存文件内容
        private void SaveMainDoc(String fname)
        {
            using (var fs = new FileStream(fname, FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, _doc);
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
            _doc = null;
            FileName = "";
            _open_docs.Clear();
            return true;
        }


        #endregion

        #region --Command Operation--

        private void DoNewDoc(object sender, ExecutedRoutedEventArgs e)
        {
            if (CloseMainDoc()) OpenMainDoc(GetNewMainDoc());
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
            e.CanExecute = (_doc != null);
        }

        private void DoSaveAll(object sender, ExecutedRoutedEventArgs e)
        {

            foreach (var f in _open_docs) f.Value.Content = f.Key.GetNewDocContent();
            SaveMainDoc();
            foreach (var vm in _open_docs.Keys) vm.AfterSaved();
        }

        private void CanSaveDoc(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (_activeVM != null);
        }

        private void DoSaveDoc(object sender, ExecutedRoutedEventArgs e)
        {

            if (_activeVM != null) _open_docs[_activeVM].Content = _activeVM.GetNewDocContent();
            SaveMainDoc();
            _activeVM.AfterSaved();
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
