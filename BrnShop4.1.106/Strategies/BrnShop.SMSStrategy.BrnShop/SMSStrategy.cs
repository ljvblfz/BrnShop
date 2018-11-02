using System;
using System.Web;
using System.Text;

using BrnShop.Core;

namespace BrnShop.SMSStrategy.BrnShop
{
    /// <summary>
    /// 简单短信策略
    /// </summary>
    public partial class SMSStrategy : ISMSStrategy
    {
        private string _url;
        private string _username;
        private string _password;

        private Encoding _encoding = Encoding.GetEncoding("gbk");

        /// <summary>
        /// 短信服务器地址
        /// </summary>
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        /// <summary>
        /// 短信账号
        /// </summary>
        public string UserName
        {
            get { return _username; }
            set { _username = value; }
        }

        /// <summary>
        /// 短信密码
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="to">接收人号码</param>
        /// <param name="body">短信内容</param>
        /// <returns>是否发送成功</returns>
        public bool Send(string to, string body)
        {
            //此方法适用于国都短信
            string url = string.Format("{0}?OperID={1}&OperPass={2}&DesMobile={3}&Content={4}&ContentType=15", _url, _username, _password, to, HttpUtility.UrlEncode(body, _encoding));
            string content = WebHelper.GetRequestData(url, "get", null);

            //以下各种情况的判断要根据不同平台具体调整
            if (content.Contains("<code>03</code>"))
            {
                return true;
            }
            else
            {
                if (content.Substring(0, 1) == "2") //余额不足
                {
                    //"手机短信余额不足";
                    //TODO
                }
                else
                {
                    //短信发送失败的其他原因
                    //TODO
                }
                return false;
            }
        }
    }
}
