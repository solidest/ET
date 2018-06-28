using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.TreeView;
using ET.Doc;
using System.Windows;
using System.Text.RegularExpressions;

namespace ET.Main.DocTree
{
    public class DocTreeFolderNode : DocTreeNode
    {
        private DirNode _dirNode;
        public DocTreeFolderNode(DirNode dirNode)
        {
            _dirNode = dirNode;
            LazyLoading = true;
        }

        public DirNode MDir
        {
            get
            {
                return _dirNode;
            }
        }


        //添加子文件
        public void AddChild(DocTreeFileNode n)
        {
            _dirNode.SubModuleFiles.Add(n.MFile);
            if(!LazyLoading) Children.Add(n);
            AutoSave();
        }

        //添加子目录
        public void AddChild(DocTreeFolderNode n)
        {
            _dirNode.SubDirNodes.Add(n.MDir);
            if (!LazyLoading) Children.Add(n);
            AutoSave();
        }

        //删除子文件
        public void DelChild(DocTreeFileNode n)
        {
            _dirNode.SubModuleFiles.Remove(n.MFile);
            if (!LazyLoading) Children.Remove(n);
            ET.Service.ETService.MainService.CloseModuleFile(n.MFile);
            AutoSave();
        }

        //删除子目录
        public void DelChild(DocTreeFolderNode n)
        {
            _dirNode.SubDirNodes.Remove(n.MDir);
            if (!LazyLoading) Children.Remove(n);
            AutoSave();
        }


        public override bool CanNewFile
        {
            get
            {
                switch(Service.ETService.MainService.ModulesHeaders[ModuleKey].ModuleFileType)
                {
                    case Interface.ETModuleFileTypeEnum.CustomMulti:
                    case Interface.ETModuleFileTypeEnum.DefaultMulti:
                        return true;
                    default:
                        return false;
                }
            }
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

        public override bool IsEditable
        {
            get
            {
                return !Parent.IsRoot;
            }
        }

        public override string LoadEditText()
        {
            return MDir.NodeName;
        }


        public override bool CanPaste(IDataObject data)
        {
            return data.GetDataPresent(ModuleKey, false);
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
                        AddChild(new DocTreeFileNode(p));
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
                        //TODO 处理文件重名问题
                        AddChild(new DocTreeFileNode(p));
                    }
                }
            }
        }

        #region --Helper--

        public string validName(string input)
        {
            if (!Regex.Match(input, @"^[\u4e00-\u9fa5_a-zA-Z0-9]+$").Success) return "输入的名称无效！";
            if (HaveName(input)) return "名称重复！";
            return "";
        }

        private bool HaveName(string name)
        {
            foreach (var f in  MDir.SubModuleFiles)
            {
                if (f.FileName == name) return true;
            }
            foreach (var n in MDir.SubDirNodes)
            {
                if (n.NodeName == name) return true;
            }
            return false;
        }

        public override void Rename(string newName)
        {
            MDir.NodeName = newName;
        }


        #endregion
    }

}

