using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Main
{
    class ModuleFileTreeNode : ModuleTreeNode
    {
        public override string TreeNodeName { get; set; }

        public override bool CanContainSubNodes => false;

        public override object Icon
        {
            get
            {
                return null;
            }
        }
    }
}
