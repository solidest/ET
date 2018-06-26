using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Service
{
    /// <summary>
    /// ET服务访问器，以静态成员形式封装全部ET服务
    /// </summary>
    public class ETService
    {
        /// <summary>
        /// 主服务
        /// </summary>
        static public IMainService MainService;

        /// <summary>
        /// 验证输入字符串是否有效的委托
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns>验证成功返回空，否则返回错误信息</returns>
        public delegate string ValidateStringCallBack(string inputStr);


        /// <summary>
        /// 安装主服务
        /// </summary>
        /// <param name="mainService">主服务实现对象</param>
        static public void SetupMainService(IMainService mainService)
        {
            MainService = mainService;
        }
    }
}
