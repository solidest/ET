using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Interface
{
    /// <summary>
    /// ET自定义异常
    /// </summary>
    public class ETException:Exception
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="moduleKey">ET模块主键</param>
        /// <param name="message">异常消息字段</param>
        public ETException(string moduleKey,string message):base(message)
        {
            ModuleKey = moduleKey;
        }

        /// <summary>
        /// ET模块主键
        /// </summary>
        public string ModuleKey { get; private set; }

        /// <summary>
        /// 重载基类的函数，加入ET模块主键信息
        /// </summary>
        /// <returns>描述字符串</returns>
        public override string ToString()
        {
            return "ET模块(" + ModuleKey + ") - " + base.ToString();
        }
    }
}
