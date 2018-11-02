using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Specialized;

using BrnMall.Core;
using BrnMall.Services;
using BrnMall.Web.Framework;
using BrnMall.OAuthPlugin.QQ;

using Newtonsoft.Json;

namespace BrnMall.Web.Controllers
{
    /// <summary>
    /// 前台QQ开放授权控制器类
    /// </summary>
    public class QQOAuthController : BaseWebController
    {
        /// <summary>
        /// 登录
        /// </summary>
        public ActionResult Login()
        {
            //返回url
            string returnUrl = WebHelper.GetQueryString("returnUrl");
            if (returnUrl.Length == 0)
                returnUrl = "/";

            if (WorkContext.MallConfig.LoginType == "")
                return PromptView(returnUrl, "商城目前已经关闭登录功能!");
            if (WorkContext.Uid > 0)
                return PromptView(returnUrl, "您已经登录，无须重复登录!");

            PluginSetInfo pluginSetInfo = PluginUtils.GetPluginSet();
            //生成随机值，防止CSRF攻击
            string salt = Randoms.CreateRandomValue(16);
            //获取Authorization Code地址
            string url = string.Format("{0}/oauth2.0/authorize?client_id={1}&response_type=code&redirect_uri=http://{2}{3}&state={4}",
                                        pluginSetInfo.AuthUrl, pluginSetInfo.AppKey, BMAConfig.MallConfig.SiteUrl, Url.Action("CallBack"), salt);
            Sessions.SetItem(WorkContext.Sid, "qqAuthLoginSalt", salt);
            return Redirect(url);
        }

        /// <summary>
        /// 回调
        /// </summary>
        public ActionResult CallBack()
        {
            //返回url
            string returnUrl = WebHelper.GetQueryString("returnUrl");
            if (returnUrl.Length == 0)
                returnUrl = "/";

            if (WorkContext.MallConfig.LoginType == "")
                return PromptView(returnUrl, "商城目前已经关闭登录功能!");
            if (WorkContext.Uid > 0)
                return PromptView(returnUrl, "您已经登录，无须重复登录!");

            //返回的随机值
            string backSalt = WebHelper.GetQueryString("state");
            //Authorization Code
            string code = WebHelper.GetQueryString("code");
            //保存在session中随机值
            string salt = Sessions.GetValueString(WorkContext.Sid, "qqAuthLoginSalt");

            if (backSalt.Length > 0 && code.Length > 0 && salt.Length > 0 && backSalt == salt)
            {
                //清空session中随机值
                Sessions.SetItem(WorkContext.Sid, "qqAuthLoginSalt", null);

                PluginSetInfo pluginSetInfo = PluginUtils.GetPluginSet();

                //构建获取Access Token的url
                string requestUrl = string.Format("{0}/oauth2.0/token?grant_type=authorization_code&code={1}&client_id={2}&client_secret={3}&redirect_uri=http://{4}{5}",
                                                    pluginSetInfo.AuthUrl, code, pluginSetInfo.AppKey, pluginSetInfo.AppSecret, BMAConfig.MallConfig.SiteUrl, Url.Action("CallBack"));
                //发送获得Access Token的请求
                string requestResult = WebHelper.GetRequestData(requestUrl, "get", null);
                //将返回结果解析成参数列表
                NameValueCollection parmList = WebHelper.GetParmList(requestResult);
                //Access Token值
                string access_token = parmList["access_token"];

                //通过上一步获取的Access Token，构建获得对应用户身份的OpenID的url
                requestUrl = string.Format("{0}/oauth2.0/me?access_token={1}", pluginSetInfo.AuthUrl, access_token);
                //发送获得OpenID的请求
                requestResult = WebHelper.GetRequestData(requestUrl, "get", null);
                //移除返回结果开头的“callback(”和结尾的“);”字符串
                string json = StringHelper.TrimEnd(StringHelper.TrimStart(requestResult, "callback("), ");");
                //OpenID值
                string openId = JsonConvert.DeserializeObject<PartOAuthUser>(json).OpenId;


                //判断此用户是否已经存在
                int uid = OAuths.GetUidByOpenIdAndServer(openId, pluginSetInfo.Server);
                if (uid > 0)//存在时
                {
                    PartUserInfo partUserInfo = Users.GetPartUserById(uid);
                    //更新用户最后访问
                    Users.UpdateUserLastVisit(partUserInfo.Uid, DateTime.Now, WorkContext.IP, WorkContext.RegionId);
                    //更新购物车中用户id
                    Carts.UpdateCartUidBySid(partUserInfo.Uid, WorkContext.Sid);
                    MallUtils.SetUserCookie(partUserInfo, -1);

                    return Redirect("/");
                }
                else
                {
                    //获取用户信息的url
                    requestUrl = string.Format("{0}/user/get_user_info?access_token={1}&oauth_consumer_key={2}&openid={3}",
                                                pluginSetInfo.AuthUrl, access_token, pluginSetInfo.AppKey, openId);
                    //发送获取用户信息的请求
                    requestResult = WebHelper.GetRequestData(requestUrl, "get", null);
                    try
                    {
                        throw new Exception();
                    }
                    catch
                    {
                        Logs.Write(requestResult);
                    }
                    //将返回结果序列化为对象
                    OAuthUser oAuthUser = JsonConvert.DeserializeObject<OAuthUser>(requestResult);
                    Logs.Write(oAuthUser.Ret.ToString());
                    if (oAuthUser.Ret == 0)//当没有错误时
                    {
                        UserInfo userInfo = OAuths.CreateOAuthUser(oAuthUser.Nickname, pluginSetInfo.UNamePrefix, openId, pluginSetInfo.Server, WorkContext.RegionId);
                        if (userInfo != null)
                        {
                            //发放注册积分
                            Credits.SendRegisterCredits(ref userInfo, DateTime.Now);
                            //更新购物车中用户id
                            Carts.UpdateCartUidBySid(userInfo.Uid, WorkContext.Sid);
                            MallUtils.SetUserCookie(userInfo, -1);
                            return Redirect("/");
                        }
                        else
                        {
                            return Redirect("/?1");
                            //return PartialView("用户创建失败");
                        }
                    }
                    else
                    {
                        return Redirect("/?2");
                        //return PartialView("QQ授权登录失败");
                    }
                }
            }
            else
            {
                return Redirect("/");
            }
        }
    }

    public class PartOAuthUser
    {
        private string _openid = "";//开放id

        /// <summary>
        /// 开放id
        /// </summary>
        public string OpenId
        {
            get { return _openid; }
            set { _openid = value; }
        }
    }

    public class OAuthUser
    {
        private int _ret = -1;//状态码
        private string _msg = "";//错误信息
        private string _nickname = "";//用户昵称

        /// <summary>
        /// 状态码
        /// </summary>
        public int Ret
        {
            get { return _ret; }
            set { _ret = value; }
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Msg
        {
            get { return _msg; }
            set { _msg = value; }
        }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string Nickname
        {
            get { return _nickname; }
            set { _nickname = value; }
        }
    }
}
