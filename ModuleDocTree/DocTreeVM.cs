using System;
using System.Collections.Generic;
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

        private DirNode0 _rootNode = null;
        private ModuleFile0 _mfile = new ModuleFile0(ModuleDocTree.ModuleKey, ModuleDocTree.ModuleShowName, null);

        private DocTreePage _page = null;

        public DocTreeVM(DirNode0 rootNode)
        {
            _rootNode = rootNode;
            UpdateContent();
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


        public ModuleFile0 MFile
        {
            get
            {
                return _mfile;
            }
        }

        public void UpdateContent()
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
