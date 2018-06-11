using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ET.Doc;
using ET.Interface;
namespace ET.Main
{
    public class DocTreeVM : IViewDoc
    {

        private IList<DirNode> _rootNodes = null;
        public DocTreeVM(IList<DirNode> rootNodes)
        {
            _rootNodes = rootNodes;
        }

        #region --IViewDoc--

        public ETPage PageUI => throw new NotImplementedException();

        public string ModuleKey
        {
            get
            {
                return ModuleDocTree.ModuleKey;
            }
        }

        public bool IsAutoSave { get => true; set { if (!value) throw new ETException(ModuleKey, "DocTree的模块文档不能设置为非自动保存！"); } }

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

        #endregion
    }
}
