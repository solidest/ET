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

        //是否可以复制
        public override bool CanCopy(SharpTreeNode[] nodes)
        {
            return nodes.All(n =>!n.Parent.IsRoot);
        }

        //是否可以删除
        public override bool CanDelete(SharpTreeNode[] nodes)
        {
            return nodes.All(n => !n.Parent.IsRoot);
        }

        //开始拖拽
        public override void StartDrag(DependencyObject dragSource, SharpTreeNode[] nodes)
        {
            foreach (var n in nodes)
            {
                if (n.Parent.IsRoot || n.GetType() == typeof(DocTreeFolderNode)) return;    //根节点和文件夹不能拖拽
            }
            base.StartDrag(dragSource, nodes);
        }

        //获取用于复制或拖拽的数据内容
        protected override IDataObject GetDataObject(SharpTreeNode[] nodes)
        {
            var data = new DataObject();
            var fs = nodes.OfType<DocTreeNode>().Select(n => n.GetData()).ToArray();
            data.SetData(ModuleKey, fs);
            return data;
        }


        //删除节点
        public override void Delete(SharpTreeNode[] nodes)
        {
            string tip = (nodes.Length > 1) ? "确实要删除这" + nodes.Length + " 项吗？" : "确实要删除【" + nodes[0].ToString() + "】吗？";
            if (MessageBox.Show(tip, "确认", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                DeleteWithoutConfirmation(nodes);
            }
        }

        //无提示删除
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

        //重命名
        public abstract void Rename(string newName);

        //节点内容
        public abstract object GetData();

        //保存名称修改
        public override bool SaveEditText(string value)
        {
            var p = (Parent as DocTreeFolderNode);
            if (p.ValidName(value) == "")
            {
                Rename(value);
                AutoSave();
                return true;
            }
            return false;
        }

        //自动保存
        public void AutoSave()
        {
            Service.ETService.MainService.SaveFile();
        }

    }
}
