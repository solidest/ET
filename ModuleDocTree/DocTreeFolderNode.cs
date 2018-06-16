using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.TreeView;
using ET.Doc;
using System.Windows;

namespace ET.Main
{
    public class DocTreeFolderNode : DocTreeNode
    {
        private DirNode _dirNode;
        public DocTreeFolderNode(DirNode dirNode)
        {
            _dirNode = dirNode;
            LazyLoading = true;
            _dirNode.SubDirNodes.Add(new DirNode("DocTree", "tset"));
            _dirNode.SubModuleFiles.Add(new ModuleFile("DocTree", "test", null));
        }


        public override object Text
        {
            get
            {
                return _dirNode.NodeName;
            }
        }

        public override string ToString()
        {
            return _dirNode.NodeName;
        }

        public override object Icon
        {
            get
            {
                return ModuleDocTree.GetFolderIcon(_dirNode.ModuleKey);
            }
        }

        public override object ExpandedIcon
        {
            get
            {
                return ModuleDocTree.GetOpenFolderIcon(_dirNode.ModuleKey);
            }
        }

        public override string ModuleKey
        {
            get
            {
                return _dirNode.ModuleKey;
            }
        }

        protected override void LoadChildren()
        {
            try
            {
                //根目录先加载文件
                if(IsRoot)
                {
                    foreach (var p in _dirNode.SubModuleFiles.OrderBy(p=>ET.Service.ETService.MainService.ModulesHeaders[p.ModuleKey].ILevel))
                    {
                        Children.Add(new DocTreeFileNode(p));
                    }
                    foreach(var p in _dirNode.SubDirNodes.OrderBy(p => ET.Service.ETService.MainService.ModulesHeaders[p.ModuleKey].ILevel))
                    {
                        Children.Add(new DocTreeFolderNode(p));
                    }
                }
                else
                {
                    foreach (var p in _dirNode.SubDirNodes.OrderBy(p=>p.NodeName))
                    {
                        Children.Add(new DocTreeFolderNode(p));
                    }
                    foreach (var p in _dirNode.SubModuleFiles.OrderBy(p=>p.FileName))
                    {
                        Children.Add(new DocTreeFileNode(p));
                    }
                }

            }
            catch
            {
            }
        }

        public override bool CanPaste(IDataObject data)
        {
            return data.GetDataPresent(DataFormats.Serializable);
        }

        public override void Paste(IDataObject data)
        {
            if (data.GetData(DataFormats.Serializable) is ModuleFile[] files)
            {
                foreach (var p in files)
                {
                    if (p.ModuleKey == ModuleKey)
                    {
                        //TODO 处理文件重名问题
                        _dirNode.SubModuleFiles.Add(p);
                        Children.Add(new DocTreeFileNode(p));
                    }
                }
            }
        }

        public override void Drop(DragEventArgs e, int index)
        {
            if (e.Data.GetData(DataFormats.Serializable) is ModuleFile[] files)
            {
                foreach (var p in files)
                {
                    if (p.ModuleKey == ModuleKey)
                    {
                        Children.Insert(index++, new DocTreeFileNode(p));
                    }
                }
            }
        }
    }
}

