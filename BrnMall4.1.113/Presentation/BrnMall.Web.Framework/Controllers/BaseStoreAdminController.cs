using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using BrnMall.Core;
using BrnMall.Services;

namespace BrnMall.Web.Framework
{
    /// <summary>
    /// PC店铺后台基础控制器类
    /// </summary>
    public class BaseStoreAdminController : BaseController
    {
        //工作上下文
        public StoreAdminWorkContext WorkContext = new StoreAdminWorkContext();

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            WorkContext.IsHttpAjax = WebHelper.IsAjax();
            WorkContext.IP = WebHelper.GetIP();
            WorkContext.RegionInfo = Regions.GetRegionByIP(WorkContext.IP);
            WorkContext.RegionId = WorkContext.RegionInfo.RegionId;
            WorkContext.Url = WebHelper.GetUrl();
            WorkContext.UrlReferrer = WebHelper.GetUrlReferrer();

            //获得用户唯一标示符sid
            WorkContext.Sid = MallUtils.GetSidCookie();
            if (WorkContext.Sid.Length == 0)
            {
                //生成sid
                WorkContext.Sid = Sessions.GenerateSid();
                //将sid保存到cookie中
                MallUtils.SetSidCookie(WorkContext.Sid);
            }

            PartUserInfo partUserInfo;

            //获得用户id
            int uid = MallUtils.GetUidCookie();
            if (uid < 1)//当用户为游客时
            {
                //创建游客
                partUserInfo = Users.CreatePartGuest();
            }
            else//当用户为会员时
            {
                //获得保存在cookie中的密码
                string encryptPwd = MallUtils.GetCookiePassword();
                //防止用户密码被篡改为危险字符
                if (encryptPwd.Length == 0 || !SecureHelper.IsBase64String(encryptPwd))
                {
                    //创建游客
                    partUserInfo = Users.CreatePartGuest();
                    encryptPwd = string.Empty;
                    MallUtils.SetUidCookie(-1);
                    MallUtils.SetCookiePassword("");
                }
                else
                {
                    partUserInfo = Users.GetPartUserByUidAndPwd(uid, MallUtils.DecryptCookiePassword(encryptPwd));
                    if (partUserInfo != null)
                    {
                        //发放登录积分
                        Credits.SendLoginCredits(ref partUserInfo, DateTime.Now);
                    }
                    else//当会员的账号或密码不正确时，将用户置为游客
                    {
                        partUserInfo = Users.CreatePartGuest();
                        encryptPwd = string.Empty;
                        MallUtils.SetUidCookie(-1);
                        MallUtils.SetCookiePassword("");
                    }
                }
                WorkContext.EncryptPwd = encryptPwd;
            }

            //设置用户等级
            if (UserRanks.IsBanUserRank(partUserInfo.UserRid) && partUserInfo.LiftBanTime <= DateTime.Now)
            {
                UserRankInfo userRankInfo = UserRanks.GetUserRankByCredits(partUserInfo.PayCredits);
                Users.UpdateUserRankByUid(partUserInfo.Uid, userRankInfo.UserRid);
                partUserInfo.UserRid = userRankInfo.UserRid;
            }

            //当用户被禁止访问时重置用户为游客
            if (partUserInfo.UserRid == 1)
            {
                partUserInfo = Users.CreatePartGuest();
                WorkContext.EncryptPwd = string.Empty;
                MallUtils.SetUidCookie(-1);
                MallUtils.SetCookiePassword("");
            }

            WorkContext.PartUserInfo = partUserInfo;

            WorkContext.Uid = partUserInfo.Uid;
            WorkContext.UserName = partUserInfo.UserName;
            WorkContext.UserEmail = partUserInfo.Email;
            WorkContext.UserMobile = partUserInfo.Mobile;
            WorkContext.Password = partUserInfo.Password;
            WorkContext.NickName = partUserInfo.NickName;
            WorkContext.Avatar = partUserInfo.Avatar;

            WorkContext.UserRid = partUserInfo.UserRid;
            WorkContext.UserRankInfo = UserRanks.GetUserRankById(partUserInfo.UserRid);
            WorkContext.UserRTitle = WorkContext.UserRankInfo.Title;
            //设置用户商城管理员组
            WorkContext.MallAGid = partUserInfo.MallAGid;
            WorkContext.MallAdminGroupInfo = MallAdminGroups.GetMallAdminGroupById(partUserInfo.MallAGid);
            WorkContext.MallAGTitle = WorkContext.MallAdminGroupInfo.Title;
            //设置用户店铺信息
            WorkContext.StoreId = partUserInfo.StoreId;
            WorkContext.StoreInfo = Stores.GetStoreById(partUserInfo.StoreId);
            WorkContext.StoreName = WorkContext.StoreId < 1 ? "" : WorkContext.StoreInfo.Name;

            //设置当前控制器类名
            WorkContext.Controller = RouteData.Values["controller"].ToString().ToLower();
            //设置当前动作方法名
            WorkContext.Action = RouteData.Values["action"].ToString().ToLower();

            //设置图片cdn
            WorkContext.ImageCDN = WorkContext.MallConfig.ImageCDN;
            //设置csscdn
            WorkContext.CSSCDN = WorkContext.MallConfig.CSSCDN;
            //设置脚本cdn
            WorkContext.ScriptCDN = WorkContext.MallConfig.ScriptCDN;
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            //不能应用在子方法上
            if (filterContext.IsChildAction)
                return;

            //当用户IP被禁止时
            if (BannedIPs.CheckIP(WorkContext.IP))
            {
                if (WorkContext.IsHttpAjax)
                    filterContext.Result = AjaxResult("404", "您访问的网址不存在");
                else
                    filterContext.Result = new RedirectResult("/");
                return;
            }

            //如果当前用户没有登录
            if (WorkContext.Uid < 1)
            {
                if (WorkContext.IsHttpAjax)
                    filterContext.Result = AjaxResult("404", "您访问的网址不存在");
                else
                    filterContext.Result = new RedirectResult("/");
                return;
            }

            if (WorkContext.StoreId < 1)//当前用户不是店长
            {
                if (WorkContext.IsHttpAjax)
                    filterContext.Result = AjaxResult("404", "您访问的网址不存在");
                else
                    filterContext.Result = new RedirectResult("/");
                return;
            }
            else if (WorkContext.StoreInfo.State == (int)StoreState.Close)//店铺已经关闭
            {
                if (WorkContext.IsHttpAjax)
                    filterContext.Result = AjaxResult("storeclose", "您的店铺已经关闭");
                else
                    filterContext.Result = PromptView("/", "您的店铺已经关闭");
                return;
            }
        }

        /// <summary>
        /// 提示信息视图
        /// </summary>
        /// <param name="message">提示信息</param>
        /// <returns></returns>
        protected ViewResult PromptView(string message)
        {
            return View("prompt", new PromptModel(MallUtils.GetStoreAdminRefererCookie(), message));
        }

        /// <summary>
        /// 添加店铺管理日志
        /// </summary>
        /// <param name="operation">操作行为</param>
        protected void AddStoreAdminLog(string operation)
        {
            AddStoreAdminLog(operation, "");
        }

        /// <summary>
        /// 添加店铺管理日志
        /// </summary>
        /// <param name="operation">操作行为</param>
        /// <param name="description">操作描述</param>
        protected void AddStoreAdminLog(string operation, string description)
        {
            StoreAdminLogs.CreateStoreAdminLog(WorkContext.Uid, WorkContext.NickName, WorkContext.StoreId, WorkContext.StoreName, WorkContext.IP, operation, description);
        }
    }
}
