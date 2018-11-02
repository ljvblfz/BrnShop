using System;

namespace BrnMall.Core
{
    /// <summary>
    /// BrnMall支付插件接口
    /// </summary>
    public partial interface IPayPlugin : IPlugin
    {
        /// <summary>
        /// 支付控制器
        /// </summary>
        string PayController { get; }
        /// <summary>
        /// 支付动作方法
        /// </summary>
        string PayAction { get; }

        /// <summary>
        /// 移动支付控制器
        /// </summary>
        string MobPayController { get; }
        /// <summary>
        /// 移动支付动作方法
        /// </summary>
        string MobPayAction { get; }

        /// <summary>
        /// App支付控制器
        /// </summary>
        string AppPayController { get; }
        /// <summary>
        /// App支付动作方法
        /// </summary>
        string AppPayAction { get; }

        //void Refund();退款接口的有无密码问题？？？
    }
}
