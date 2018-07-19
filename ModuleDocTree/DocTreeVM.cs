using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using ET.Doc;
using ET.Interface;

namespace ET.Main.DocTree
{
    public class DocTreeVM : IViewDoc
    {

        private DirNode _rootNode = null;
        private ModuleFile _mfile = new ModuleFile(ModuleDocTree.ModuleKey, ModuleDocTree.ModuleShowName, null);
        private bool _isModify;

        private DocTreePage _page = null;

        public DocTreeVM(DirNode rootNode)
        {
            _rootNode = rootNode;
            SaveContent();

            _page = (DocTreePage)System.Windows.Application.LoadComponent(new Uri("/ModuleDocTree;component/DocTreePage.xaml", System.UriKind.Relative));
            _page.trMain.Root = new DocTreeFolderNode(_rootNode);
            IsAutoSave = true;
        }

        #region --IViewDoc--

        public ETPage PageUI
        {
            get
            {
                return _page;
            }
        }


        public bool IsAutoSave { get => true; set { if (!value) throw new ETException(ModuleKey, "DocTree的模块文档不能设置为非自动保存！"); } }

        public string ModuleKey
        {
            get
            {
                return ModuleDocTree.ModuleKey;
            }
        }


        public ModuleFile MFile
        {
            get
            {
                return _mfile;
            }
        }

        public bool IsModify
        {
            get
            {
                return false;
            }
            set
            {
                if(_isModify != value)
                {
                    _isModify = value;
                    if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("IsModify"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void SaveContent()
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, _rootNode);
                _mfile.Content = ms.GetBuffer();
            }
        }

        #endregion

    }
}
