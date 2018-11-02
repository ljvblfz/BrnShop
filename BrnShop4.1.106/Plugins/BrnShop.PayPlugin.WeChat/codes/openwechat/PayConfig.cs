using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrnShop.PayPlugin.WeChat;

namespace BrnShop.PayPlugin.OpenWeChat
{
    public class PayConfig
    {
        /// <summary>
        /// 人民币
        /// </summary>
        public static string Tenpay = "1";

        /// <summary>
        /// mchid ， 微信支付商户号
        /// </summary>
        public static string MchId = "XXXXXXXXXXXXXXXXX"; //

        /// <summary>
        /// appid，应用ID， 在微信公众平台中 “开发者中心”栏目可以查看到
        /// </summary>
        public static string AppId = "XXXXXXXXXXXXXXXXX";

        /// <summary>
        /// appsecret ，应用密钥， 在微信公众平台中 “开发者中心”栏目可以查看到
        /// </summary>
        public static string AppSecret = "XXXXXXXXXXXXXXXXX";

        /// <summary>
        /// paysignkey，API密钥，在微信商户平台中“账户设置”--“账户安全”--“设置API密钥”，只能修改不能查看
        /// </summary>
        public static string AppKey = "XXXXXXXXXXXXXXXXX";

        /// <summary>
        /// 支付起点页面地址，也就是send.aspx页面完整地址
        /// 用于获取用户OpenId，支付的时候必须有用户OpenId，如果已知可以不用设置
        /// </summary>
        public static string SendUrl = "http://zousky.com/WXPay/Send.aspx";

        /// <summary>
        /// 支付页面，请注意测试阶段设置授权目录，在微信公众平台中“微信支付”---“开发配置”--修改测试目录   
        /// 注意目录的层次，比如我的：http://zousky.com/WXPay/
        /// </summary>
        public static string PayUrl = "http://zousky.com/WXPay/WeiPay.aspx";

        /// <summary>
        ///  支付通知页面，请注意测试阶段设置授权目录，在微信公众平台中“微信支付”---“开发配置”--修改测试目录   
        /// 支付完成后的回调处理页面,替换成notify_url.asp所在路径
        /// </summary>
        public static string NotifyUrl = "http://zousky.com/WXPay/Notify.aspx";

        static PayConfig()
        {
            MchId = PluginUtils.GetPluginSet().OpenMchId;
            AppId = PluginUtils.GetPluginSet().OpenAppId;
            AppSecret = PluginUtils.GetPluginSet().OpenAppSecret;
            AppKey = PluginUtils.GetPluginSet().OpenAppKey;
        }

        public static void ReSet()
        {
            MchId = PluginUtils.GetPluginSet().OpenMchId;
            AppId = PluginUtils.GetPluginSet().OpenAppId;
            AppSecret = PluginUtils.GetPluginSet().OpenAppSecret;
            AppKey = PluginUtils.GetPluginSet().OpenAppKey;
        }
    }
}
