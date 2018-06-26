using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Service
{
    /// <summary>
    /// 输出信息
    /// </summary>
    public class OutPutInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="info">信息内容</param>
        /// <param name="mkey">模块主键</param>
        /// <param name="fname">文件名</param>
        /// <param name="lno">行号</param>
        /// <param name="itype">信息类型</param>
        public OutPutInfo(string info, string mkey, string fname, int lno, OutPutInfoTypeEnum itype)
        {
            Info = info;
            ModuleKey = mkey;
            FileName = fname;
            LineNo = lno;
            InfoType = itype;
        }

        /// <summary>
        /// 模块主键
        /// </summary>
        public string ModuleKey { get; private set; }

        /// <summary>
        /// 输出信息对应的文件名
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// 输出信息对应的行号
        /// </summary>
        public int LineNo { get; private set; }

        /// <summary>
        /// 输出信息的类型
        /// </summary>
        public OutPutInfoTypeEnum InfoType { get; private set; }

        /// <summary>
        /// 输出信息的提示内容
        /// </summary>
        public string Info { get; private set; }
    }

    /// <summary>
    /// 输出信息的类型枚举
    /// </summary>
    public enum OutPutInfoTypeEnum
    {
        /// <summary>
        /// 普通信息
        /// </summary>
        NormalInfo,

        /// <summary>
        /// 运行时信息
        /// </summary>
        RuntimeInfo,

        /// <summary>
        /// 用户输出信息 
        /// </summary>
        UserInfo,

        /// <summary>
        /// 编译信息
        /// </summary>
        CompileInfo
    }
}
