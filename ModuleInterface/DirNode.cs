using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Doc
{
    /// <summary>
    /// 文档结构中的目录节点，每个模块具有自己唯一的根节点，根节点名称为空
    /// </summary>
    public class DirNode
    {
        /// <summary>
        /// 节点名称
        /// </summary>
        public String NodeName { get; set; }

        /// <summary>
        /// 子节点集合
        /// </summary>
        public List<DirNode> SubDirNodes {get; private set;}

        /// <summary>
        /// 本节点下的文件
        /// </summary>
        public List<ModuleFile> SubModuleFiles { get; private set; }
    }
}