using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


// 命名空间文档 为编译器使用的类
namespace ET.Doc
{
    /// <summary>
    /// <list type="bullet">
    /// <item>本命名空间下的成员是ET文档内容相关的类库（注：ET文档是由ET设计器生成并存储为 .et格式的二进制文件）</item>
    /// <item>本命名空间内的类对应MVVM模型中的M，即文档模型，并不包含具体的业务逻辑</item>
    /// </list>
    /// <note type="implement">
    /// <list type="number">
    /// <item>本命名空间内的类均需标注<c>[Serializable]</c>属性，以便二进制持久化时使用</item>
    /// <item>本命名空间内的类在发布后不应再进行修改，但可以添加新类, 新类需实现向后兼容</item>
    /// </list>
    /// </note>
    /// </summary>
    [System.Runtime.CompilerServices.CompilerGenerated()]
    class NamespaceDoc
    {
    }

}

namespace ET.Interface
{
    /// <summary>
    /// <list type="bullet">
    /// <item>本命名空间下的成员是ET模块的抽象接口定义（释：ET模块是ET设计器的功能组件）</item>
    /// <item>本命名空间的抽象接口仅在ModuleInterface程序集中进行定义，接口被ET主程序和ET模块各自独立引用</item>
    /// </list>
    /// </summary>
    [System.Runtime.CompilerServices.CompilerGenerated()]
    class NamespaceDoc
    {
    }
}
