using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.TreeView;

namespace ET.Main
{
    public abstract class ModuleTreeNode : SharpTreeNode
    {
        //显示节点名
        public abstract string TreeNodeName { get; set; }

        //是否可以包含子节点
        public abstract bool CanContainSubNodes { get; } 

    }
}
