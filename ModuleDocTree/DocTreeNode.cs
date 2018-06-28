using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ICSharpCode.TreeView;

namespace ET.Main.DocTree
{
    public abstract class DocTreeNode : ICSharpCode.TreeView.SharpTreeNode
    {
        public abstract string ModuleKey { get; }

        public abstract bool CanNewFile { get; }

        public override bool CanCopy(SharpTreeNode[] nodes)
        {
            return nodes.All(n => (n is DocTreeFileNode && !n.Parent.IsRoot));
        }

        protected override IDataObject GetDataObject(SharpTreeNode[] nodes)
        {
            var data = new DataObject();
            var fs = nodes.OfType<DocTreeFileNode>().Select(n => n.MFile).ToArray();
            data.SetData(ModuleKey, fs);
            return data;
        }

        public override bool CanDelete(SharpTreeNode[] nodes)
        {
            return nodes.All(n => !n.Parent.IsRoot);
        }


        public override void Delete(SharpTreeNode[] nodes)
        {
            string tip = (nodes.Length > 1) ? "确实要删除这" + nodes.Length + " 项吗？" : "确实要删除【" + nodes[0].ToString() + "】吗？";
            if (MessageBox.Show(tip, "确认", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                DeleteWithoutConfirmation(nodes);
            }
        }

        public override void DeleteWithoutConfirmation(SharpTreeNode[] nodes)
        {
            foreach (var node in nodes)
            {
                if (node.Parent != null)
                {
                    var rfn = (node as DocTreeFileNode);
                    if (rfn != null)
                    {
                         (node.Parent as DocTreeFolderNode).DelChild(rfn);
                    }
                    else
                    {
                        (node.Parent as DocTreeFolderNode).DelChild((node as DocTreeFolderNode));
                    }

                }
            }
        }

        public abstract void Rename(string newName);

        //保存名称修改
        public override bool SaveEditText(string value)
        {
            var p = (Parent as DocTreeFolderNode);
            if (p.validName(value) == "")
            {
                Rename(value);
                AutoSave();
                return true;
            }
            return false;
        }

        public void AutoSave()
        {
            Service.ETService.MainService.SaveFile();
        }

    }
}
