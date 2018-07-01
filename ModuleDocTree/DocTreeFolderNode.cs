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
            int index = (_dirNode.SubModuleFiles.Where(p => p.FileName.CompareTo(n.MFile.FileName) < 0)).Count();
            _dirNode.SubModuleFiles.Insert(index, n.MFile);
            if (!LazyLoading)
            {
                Children.Insert(index + _dirNode.SubDirNodes.Count, n);
            }
            AutoSave();
        }

        //添加子目录
        public void AddChild(DocTreeFolderNode n)
        {
            int index = (_dirNode.SubDirNodes.Where(p => p.NodeName.CompareTo( n.MDir.NodeName)<0)).Count();
            _dirNode.SubDirNodes.Insert(index,n.MDir);
            if (!LazyLoading)
            {
                Children.Insert(index,n);
            }
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


        //加载子节点
        protected override void LoadChildren()
        {
            if (Children.Count > 0) Children.Clear();
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
                    foreach (var p in _dirNode.SubModuleFiles.OrderBy(p => p.FileName))
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

        //粘贴
        public override void Paste(IDataObject data)
        {
            if (data.GetData(ModuleKey) is Object[] files)
            {
                AddChildren(files);
            }
        }

        //放置拖拽
        public override void Drop(DragEventArgs e, int index)
        {
            if (e.Data.GetData(ModuleKey) is Object[] files)
            {
                AddChildren(files);
            }
            e.Handled = true;
        }

        private void AddChildren(Object[] files)
        {
            if (files == null) return;
            foreach (var dn in files.OfType<DirNode>())
            {

                if (dn.ModuleKey == ModuleKey)
                {
                    int i = 0;
                    string fname = dn.NodeName;
                    while (ValidName(fname) != "")
                    {
                        fname = dn.NodeName + "_copy" + ((i==0) ?"":  i.ToString());
                        i += 1;
                    }
                    dn.NodeName = fname;
                    AddChild(new DocTreeFolderNode(dn));
                }
            }
            foreach (var mf in files.OfType<ModuleFile>())
            {

                if (mf.ModuleKey == ModuleKey)
                {
                    int i = 0;
                    string fname = mf.FileName;
                    while (ValidName(fname) != "")
                    {
                        fname = mf.FileName + "_copy" + ((i == 0) ? "" :  i.ToString());
                        i += 1;
                    }
                    mf.FileName = fname;
                    AddChild(new DocTreeFileNode(mf));
                }
            }
        }

        #region --Helper--

        public string ValidName(string input)
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

        public override object GetData()
        {
            return MDir;
        }


        #endregion
    }

}

