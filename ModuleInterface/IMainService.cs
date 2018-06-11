using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ET.Doc;

namespace ET.Service
{
    /// <summary>
    /// ET主服务接口定义
    /// </summary>
    public interface IMainService
    {
        /// <summary>
        /// 打开模块文件
        /// </summary>
        /// <param name="mfile">模块文件</param>
        void OpenModuleFile(ModuleFile mfile);



    }
}
