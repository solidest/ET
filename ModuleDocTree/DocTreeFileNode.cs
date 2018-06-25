using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ET.Doc;
using ICSharpCode.TreeView;

namespace ET.Main.DocTree
{
    public class DocTreeFileNode : DocTreeNode
    {
        public DocTreeFileNode(ModuleFile0 f)
        {
            MFile = f;
        }

        public ModuleFile0 MFile { get;  }
        public override string ModuleKey
        {
            get
            {
                return MFile.ModuleKey;
            }
        }


        public override bool CanNewFile => false;
        public override string ToString()
        {
            return MFile.FileName;
        }

        public override object Text
        {
            get
            {
                return MFile.FileName;
            }
        }

        public override object Icon
        {
            get
            {
                return ModuleDocTree.GetFileIcon(MFile.ModuleKey);
            }
        }


        public override bool IsEditable
        {
            get
            {
                return !Parent.IsRoot;
            }
        }

        public override string LoadEditText()
        {
            return MFile.FileName;
        }

        public override bool SaveEditText(string value)
        {
            //TODO:判断文件名合法性
            return true;
        }

        public override bool CanPaste(IDataObject data)
        {
            return Parent.CanPaste(data);
        }

        public override void Paste(IDataObject data)
        {
            Parent.Paste(data);
        }
    }
}
