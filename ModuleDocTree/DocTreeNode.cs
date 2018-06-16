using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ICSharpCode.TreeView;

namespace ET.Main.DocTree
{
    public abstract class DocTreeNode : ICSharpCode.TreeView.SharpTreeNode
    {
        public abstract string ModuleKey { get; }

        public override bool CanCopy(SharpTreeNode[] nodes)
        {
            return nodes.All(n => (n is DocTreeFileNode && !n.Parent.IsRoot));
        }

        protected override IDataObject GetDataObject(SharpTreeNode[] nodes)
        {
            var data = new DataObject();
            var fs = nodes.OfType<DocTreeFileNode>().Select(n => n.MFile).ToArray();
            data.SetData(DataFormats.Serializable, fs);
            return data;
        }

        public override bool CanDelete(SharpTreeNode[] nodes)
        {
            return nodes.All(n => n is DocTreeFileNode && !n.Parent.IsRoot);
        }

        public override void Delete(SharpTreeNode[] nodes)
        {
            if (MessageBox.Show("确实要删除这" + nodes.Length + " 项吗？", "确认", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
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
                     node.Parent.Children.Remove(node);
                }
            }
        }
    }
}
