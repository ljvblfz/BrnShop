using System;

namespace BrnMall.Core
{
    /// <summary>
    /// BrnMall开放授权插件接口
    /// </summary>
    public partial interface IOAuthPlugin : IPlugin
    {
        /// <summary>
        /// 登录控制器
        /// </summary>
        string LoginController { get; }

        /// <summary>
        /// 登录动作方法
        /// </summary>
        string LoginAction { get; }
    }
}
