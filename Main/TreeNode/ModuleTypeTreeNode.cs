using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Main
{
    class ModuleTypeTreeNode : ModuleTreeNode
    {
        //对应模块的ShowName属性
        public override string TreeNodeName { get; set; }

        //对应模块的Key属性
        public string ModuleKey { get; set; }

        //对应模块的isOnlyOneFile属性
        public bool IsOnlyOneFile { get; set; }

        public override object Icon
        {
            get
            {
                return null;
            }
        }

        public override object ExpandedIcon
        {
            get
            {
                return null;
            }
        }

        public override bool CanContainSubNodes { get { return !IsOnlyOneFile; } }
    }
}
