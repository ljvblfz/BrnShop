using System;
using System.Text;

using BrnMall.Core;

namespace BrnMall.Services
{
    /// <summary>
    /// 短信操作管理类
    /// </summary>
    public partial class SMSes
    {
        private static object _locker = new object();//锁对象
        private static ISMSStrategy _ismsstrategy = null;//短信策略
        private static SMSConfigInfo _smsconfiginfo = null;//短信配置
        private static MallConfigInfo _mallconfiginfo = null;//商城配置

        static SMSes()
        {
            _ismsstrategy = BMASMS.Instance;
            _smsconfiginfo = BMAConfig.SMSConfig;
            _mallconfiginfo = BMAConfig.MallConfig;
            _ismsstrategy.Url = _smsconfiginfo.Url;
            _ismsstrategy.UserName = _smsconfiginfo.UserName;
            _ismsstrategy.Password = _smsconfiginfo.Password;
        }

        /// <summary>
        /// 重置短信配置
        /// </summary>
        public static void ResetSMS()
        {
            lock (_locker)
            {
                _smsconfiginfo = BMAConfig.SMSConfig;
                _ismsstrategy.Url = _smsconfiginfo.Url;
                _ismsstrategy.UserName = _smsconfiginfo.UserName;
                _ismsstrategy.Password = _smsconfiginfo.Password;
            }
        }

        /// <summary>
        /// 重置商城信息
        /// </summary>
        public static void ResetMall()
        {
            lock (_locker)
            {
                _mallconfiginfo = BMAConfig.MallConfig;
            }
        }

        /// <summary>
        /// 发送找回密码短信
        /// </summary>
        /// <param name="to">接收手机</param>
        /// <param name="code">验证值</param>
        /// <returns></returns>
        public static bool SendFindPwdMobile(string to, string code)
        {
            StringBuilder body = new StringBuilder(_smsconfiginfo.FindPwdBody);
            body.Replace("{mallname}", _mallconfiginfo.MallName);
            body.Replace("{code}", code);
            return _ismsstrategy.Send(to, body.ToString());
        }

        /// <summary>
        /// 安全中心发送验证手机短信
        /// </summary>
        /// <param name="to">接收手机</param>
        /// <param name="code">验证值</param>
        /// <returns></returns>
        public static bool SendSCVerifySMS(string to, string code)
        {
            StringBuilder body = new StringBuilder(_smsconfiginfo.SCVerifyBody);
            body.Replace("{mallname}", _mallconfiginfo.MallName);
            body.Replace("{code}", code);
            return _ismsstrategy.Send(to, body.ToString());
        }

        /// <summary>
        /// 安全中心发送确认更新手机短信
        /// </summary>
        /// <param name="to">接收手机</param>
        /// <param name="code">验证值</param>
        /// <returns></returns>
        public static bool SendSCUpdateSMS(string to, string code)
        {
            StringBuilder body = new StringBuilder(_smsconfiginfo.SCUpdateBody);
            body.Replace("{mallname}", _mallconfiginfo.MallName);
            body.Replace("{code}", code);
            return _ismsstrategy.Send(to, body.ToString());
        }

        /// <summary>
        /// 发送注册欢迎短信
        /// </summary>
        /// <param name="to">接收手机</param>
        /// <returns></returns>
        public static bool SendWebcomeSMS(string to)
        {
            StringBuilder body = new StringBuilder(_smsconfiginfo.WebcomeBody);
            body.Replace("{mallname}", _mallconfiginfo.MallName);
            body.Replace("{regtime}", CommonHelper.GetDateTime());
            body.Replace("{mobile}", to);
            return _ismsstrategy.Send(to, body.ToString());
        }
    }
}
