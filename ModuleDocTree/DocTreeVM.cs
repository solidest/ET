using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using ET.Doc;
using ET.Interface;
namespace ET.Main
{
    public class DocTreeVM : IViewDoc
    {

        private DirNode _rootNode = new DirNode(ModuleDocTree.ModuleKey, ModuleDocTree.ModuleKey);
        private ModuleFile _mfile = new ModuleFile(ModuleDocTree.ModuleKey, ModuleDocTree.ModuleKey, null);
        private DocTreePage _page = new DocTreePage();

        public DocTreeVM(DirNode rootNode)
        {
            _rootNode = rootNode;
            _page.trMain.Root = new DocTreeFolderNode(_rootNode);

        }



        #region --IViewDoc--

        public ETPage PageUI => _page;

        public string ModuleKey
        {
            get
            {
                return ModuleDocTree.ModuleKey;
            }
        }

        public bool IsAutoSave { get => true; set { if (!value) throw new ETException(ModuleKey, "DocTree的模块文档不能设置为非自动保存！"); } }

        public ModuleFile MFile
        {
            get
            {
                return _mfile;
            }
        }

        public bool CanCopy()
        {
            
        }

        public bool CanPaste()
        {
            throw new NotImplementedException();
        }

        public bool CanRedo()
        {
            throw new NotImplementedException();
        }

        public bool CanUndo()
        {
            throw new NotImplementedException();
        }

        public void DoCopy()
        {
            throw new NotImplementedException();
        }

        public void DoPaste()
        {
            throw new NotImplementedException();
        }

        public void DoRedo()
        {
            throw new NotImplementedException();
        }

        public void DoUndo()
        {
            throw new NotImplementedException();
        }

        public byte[] GetDocContent()
        {
            throw new NotImplementedException();
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
