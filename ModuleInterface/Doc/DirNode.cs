using System;
using System.Collections.Generic;

namespace ET.Doc
{

    /// <summary>
    /// <list type="bullet">
    /// <item>目录节点类<c>DirNode</c>对应文档结构中的目录节点</item>
    /// <item>每个模块都有自己专属的唯一文档目录树，树的每个节点（不包括模块文件）都是本类的一个实例</item>
    /// </list>
    /// </summary>
    [Serializable]
    public class DirNode0
    {
        /// <summary>
        /// 目录节点类的唯一构造函数
        /// </summary>
        /// <param name="mKey">ET模块主键</param>
        /// <param name="nName">节点名称</param>
        public DirNode0(string mKey, string nName)
        {
            ModuleKey = mKey;
            NodeName = nName;
            SubDirNodes = new List<DirNode0>();
            SubModuleFiles = new List<ModuleFile0>();
        }

        /// <summary>
        /// 模块文件对应模块的主键
        /// </summary>
        public String ModuleKey { get; set; }

        /// <summary>
        /// 当前节点的名称
        /// </summary>
        public String NodeName { get; set; }

        /// <summary>
        /// 当前节点下所有子节点的集合
        /// </summary>
        public List<DirNode0> SubDirNodes {get; private set;}

        /// <summary>
        /// 当前节点下所有模块文件的集合
        /// </summary>
        public List<ModuleFile0> SubModuleFiles { get; private set; }
    }
}