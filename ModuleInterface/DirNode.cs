using System;
using System.Collections.Generic;




namespace ET.Doc
{
    /// <summary>
    /// <list type="bullet">
    /// <item>本命名空间下的成员是ET文档内容相关的类库（注：ET文档是由ET设计器生成并存储为 .et格式的二进制文件）</item>
    /// <item>本命名空间内的类对应MVVM模型中的M，即文档模型，并不包含具体的业务逻辑</item>
    /// </list>
    /// <note type="implement">
    /// <list type="number">
    /// <item>本命名空间内的类均需标注[Serializable]属性，以便二进制持久化时使用</item>
    /// <item>本命名空间内的类均与发布版本相关，需保证之前文档版本的兼容性</item>
    /// </list>
    /// </note>
    /// </summary>
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    class NamespaceDoc
    {
    }

    /// <summary>
    /// <para>目录节点类</para>
    /// <para>对应文档结构中的目录节点，每个模块都有自己专属的唯一文档目录树，目录树中的每个节点（不包括模块文件）都是本类的一个实例</para>
    /// </summary>
    public class DirNode
    {
        /// <summary>
        /// 当前节点的名称
        /// </summary>
        public String NodeName { get; set; }

        /// <summary>
        /// 当前节点的父级节点，根节点的该属性值为空
        /// </summary>
        public DirNode ParentNode { get; set; }

        /// <summary>
        /// 当前节点下所有子节点的集合
        /// </summary>
        public List<DirNode> SubDirNodes {get; private set;}

        /// <summary>
        /// 当前节点下所有模块文件的集合
        /// </summary>
        public List<ModuleFile> SubModuleFiles { get; private set; }
    }
}