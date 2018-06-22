﻿using ET.Interface;
using ET.Doc;
using System.Collections.Generic;

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
        /// <param name="vm">模块文件控制器</param>
        void OpenModuleFile(IViewDoc vm);

        /// <summary>
        /// 全部可以访问的ET模块
        /// </summary>
        IDictionary<string, ICommModule> Modules { get; }

        /// <summary>
        /// 全部可以访问的ET模块的元数据信息
        /// </summary>
        Dictionary<string, ModuleHeaderAttribute> ModulesHeaders { get; }

    }
}
