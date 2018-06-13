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

        private IList<DirNode> _rootNodes = null;
        private ModuleFile _mfile = new ModuleFile(ModuleDocTree.ModuleKey, ModuleDocTree.ModuleKey, null);

        public DocTreeVM(IList<DirNode> rootNodes)
        {
            _rootNodes = rootNodes;
        }

        #region --IViewDoc--

        public ETPage PageUI
        {
            get
            {
                return new ETPage();
            }
        }

        public string ModuleKey
        {
            get
            {
                return ModuleDocTree.ModuleKey;
            }
        }

        public bool IsAutoSave { get => true; set { if (!value) throw new ETException(ModuleKey, "DocTree的模块文档不能设置为非自动保存！"); } }

        public ModuleFile PageFile
        {
            get
            {
                return _mfile;
            }
        }

        public bool CanCopy()
        {
            throw new NotImplementedException();
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
                formatter.Serialize(ms, _rootNodes);
                _mfile.Content = ms.GetBuffer();
            }

        }

        #endregion
    }
}
