using System;

using BrnShop.Core;

namespace BrnShop.PayPlugin.Alipay
{
    /// <summary>
    /// 插件服务类
    /// </summary>
    public class PluginService : IPayPlugin
    {
        /// <summary>
        /// 插件配置控制器
        /// </summary>
        public string ConfigController
        {
            get { return "adminalipay"; }
        }

        /// <summary>
        /// 插件配置动作方法
        /// </summary>
        public string ConfigAction
        {
            get { return "config"; }
        }

        /// <summary>
        /// 支付控制器
        /// </summary>
        public string PayController
        {
            get { return "alipay"; }
        }
        /// <summary>
        /// 支付动作方法
        /// </summary>
        public string PayAction
        {
            get { return "pay"; }
        }

        /// <summary>
        /// 移动支付控制器
        /// </summary>
        public string MobPayController
        {
            get { return "mobalipay"; }
        }
        /// <summary>
        /// 移动支付动作方法
        /// </summary>
        public string MobPayAction
        {
            get { return "pay"; }
        }

        /// <summary>
        /// App支付控制器
        /// </summary>
        public string AppPayController
        {
            get { return "appalipay"; }
        }
        /// <summary>
        /// App支付动作方法
        /// </summary>
        public string AppPayAction
        {
            get { return "pay"; }
        }
    }
}
