using System;
using System.Web.Mvc;
using System.Web.Routing;

using BrnMall.Core;
using BrnMall.Services;

namespace BrnMall.Web.Framework
{
    /// <summary>
    /// App前台基础控制器类
    /// </summary>
    public class BaseAppController : BaseController
    {
        //工作上下文
        public AppWorkContext WorkContext = new AppWorkContext();

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            this.ValidateRequest = false;

            WorkContext.IP = WebHelper.GetIP();
            if (WebHelper.GetQueryString("ip") == WorkContext.IP)
            {
                WorkContext.RegionInfo = Regions.GetRegionById(WebHelper.GetQueryInt("regionid"));
            }
            else
            {
                WorkContext.RegionInfo = IPSeek.Seek(WorkContext.IP);
            }
            if (WorkContext.RegionInfo == null)
            {
                WorkContext.RegionInfo = new RegionInfo() { RegionId = -1, Name = "未知区域" };
            }
            WorkContext.RegionId = WorkContext.RegionInfo.RegionId;

            WorkContext.Url = WebHelper.GetUrl();

            WorkContext.AppType = WebHelper.GetQueryInt("appType");
            WorkContext.AppVersion = WebHelper.GetQueryString("appVersion");
            WorkContext.AppOS = WebHelper.GetQueryString("appOS");

            //获得用户唯一标示符sid
            WorkContext.Sid = WebHelper.GetQueryString("sid");

            if (WorkContext.Sid.Length == 0)
            {
                //生成sid
                WorkContext.Sid = Sessions.GenerateSid();
            }

            PartUserInfo partUserInfo;

            //获得用户id
            int uid = WebHelper.GetQueryInt("uid");
            if (uid < 1)//当用户为游客时
            {
                //创建游客
                partUserInfo = Users.CreatePartGuest();
            }
            else//当用户为会员时
            {
                string encryptPwd = WebHelper.GetQueryString("encryptPwd");
                //防止用户密码被篡改为危险字符
                if (encryptPwd.Length == 0 || !SecureHelper.IsBase64String(encryptPwd))
                {
                    //创建游客
                    partUserInfo = Users.CreatePartGuest();
                    encryptPwd = string.Empty;
                }
                else
                {
                    partUserInfo = Users.GetPartUserByUidAndPwd(uid, MallUtils.DecryptCookiePassword(encryptPwd));
                    if (partUserInfo != null)
                    {
                        //发放登录积分
                        Credits.SendLoginCredits(ref partUserInfo, DateTime.Now, TypeHelper.StringToDateTime(WebHelper.GetQueryString("slctime")), out WorkContext.SLCTime);
                    }
                    else//当会员的账号或密码不正确时，将用户置为游客
                    {
                        partUserInfo = Users.CreatePartGuest();
                        encryptPwd = string.Empty;
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

            WorkContext.PartUserInfo = partUserInfo;

            WorkContext.Uid = partUserInfo.Uid;
            WorkContext.UserName = partUserInfo.UserName;
            WorkContext.UserEmail = partUserInfo.Email;
            WorkContext.UserMobile = partUserInfo.Mobile;
            WorkContext.Password = partUserInfo.Password;
            WorkContext.NickName = partUserInfo.NickName;
            WorkContext.Avatar = partUserInfo.Avatar;
            WorkContext.PayCreditName = Credits.PayCreditName;
            WorkContext.PayCreditCount = partUserInfo.PayCredits;
            WorkContext.RankCreditName = Credits.RankCreditName;
            WorkContext.RankCreditCount = partUserInfo.RankCredits;

            WorkContext.UserRid = partUserInfo.UserRid;
            WorkContext.UserRankInfo = UserRanks.GetUserRankById(partUserInfo.UserRid);
            WorkContext.UserRTitle = WorkContext.UserRankInfo.Title;
            //设置用户商城管理员组
            WorkContext.MallAGid = partUserInfo.MallAGid;
            WorkContext.MallAdminGroupInfo = MallAdminGroups.GetMallAdminGroupById(partUserInfo.MallAGid);
            WorkContext.MallAGTitle = WorkContext.MallAdminGroupInfo.Title;

            //设置当前控制器类名
            WorkContext.Controller = RouteData.Values["controller"].ToString().ToLower();
            //设置当前动作方法名
            WorkContext.Action = RouteData.Values["action"].ToString().ToLower();
            WorkContext.PageKey = string.Format("/{0}/{1}", WorkContext.Controller, WorkContext.Action);

            WorkContext.ImageCDN = WorkContext.MallConfig.ImageCDN;
            WorkContext.CSSCDN = WorkContext.MallConfig.CSSCDN;
            WorkContext.ScriptCDN = WorkContext.MallConfig.ScriptCDN;

            //在线总人数
            WorkContext.OnlineUserCount = OnlineUsers.GetOnlineUserCount();
            //在线游客数
            WorkContext.OnlineGuestCount = OnlineUsers.GetOnlineGuestCount();
            //在线会员数
            WorkContext.OnlineMemberCount = WorkContext.OnlineUserCount - WorkContext.OnlineGuestCount;
            //购物车中商品数量
            WorkContext.CartProductCount = WebHelper.GetQueryInt("cartProductCount");
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            //不能应用在子方法上
            if (filterContext.IsChildAction)
                return;

            //商城已经关闭
            if (WorkContext.MallConfig.IsClosed == 1 && WorkContext.MallAGid == 1 && WorkContext.PageKey != Url.Action("login", "account") && WorkContext.PageKey != Url.Action("logout", "account"))
            {
                WorkContext.SystemState = "closemall";
                WorkContext.SystemStateMsg = WorkContext.MallConfig.CloseReason;
                return;
            }

            //当前时间为禁止访问时间
            if (ValidateHelper.BetweenPeriod(WorkContext.MallConfig.BanAccessTime) && WorkContext.MallAGid == 1 && WorkContext.PageKey != Url.Action("login", "account") && WorkContext.PageKey != Url.Action("logout", "account"))
            {
                WorkContext.SystemState = "banaccesstime";
                WorkContext.SystemStateMsg = "当前时间不能访问本商城";
                return;
            }

            //当用户ip在被禁止的ip列表时
            if (ValidateHelper.InIPList(WorkContext.IP, WorkContext.MallConfig.BanAccessIP))
            {
                WorkContext.SystemState = "banaccessip";
                WorkContext.SystemStateMsg = "您的IP被禁止访问本商城";
                return;
            }

            //当用户ip不在允许的ip列表时
            if (!string.IsNullOrEmpty(WorkContext.MallConfig.AllowAccessIP) && !ValidateHelper.InIPList(WorkContext.IP, WorkContext.MallConfig.AllowAccessIP))
            {
                WorkContext.SystemState = "banaccessip";
                WorkContext.SystemStateMsg = "您的IP被禁止访问本商城";
                return;
            }

            //当用户IP被禁止时
            if (BannedIPs.CheckIP(WorkContext.IP))
            {
                WorkContext.SystemState = "banaccessip";
                WorkContext.SystemStateMsg = "您的IP被禁止访问本商城";
                return;
            }

            //当用户等级是禁止访问等级时
            if (WorkContext.UserRid == 1)
            {
                WorkContext.SystemState = "banuserrank";
                WorkContext.SystemStateMsg = "您的账号当前被锁定,不能访问";
                return;
            }

            //判断目前访问人数是否达到允许的最大人数
            if (WorkContext.OnlineUserCount > WorkContext.MallConfig.MaxOnlineCount && WorkContext.MallAGid == 1 && (WorkContext.Controller != "account" && (WorkContext.Action != "login" || WorkContext.Action != "logout")))
            {
                WorkContext.SystemState = "maxonlinecount";
                WorkContext.SystemStateMsg = "商城人数达到访问上限, 请稍等一会再访问";
                return;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //不能应用在子方法上
            if (filterContext.IsChildAction)
                return;

            //当用户为会员时,更新用户的在线时间
            if (WorkContext.Uid > 0)
                Users.UpdateUserOnlineTime(WorkContext.Uid);

            //更新在线用户
            Asyn.UpdateOnlineUser(WorkContext.Uid, WorkContext.Sid, WorkContext.NickName, WorkContext.IP, WorkContext.RegionId);
            //更新PV统计
            Asyn.UpdatePVStat(WorkContext.StoreId, WorkContext.Uid, WorkContext.RegionId, "app", WorkContext.AppOS);
        }

        /// <summary>
        /// 提示信息视图
        /// </summary>
        /// <param name="message">提示信息</param>
        /// <returns></returns>
        protected ViewResult PromptView(string message)
        {
            return View("prompt", new PromptModel(message));
        }
    }
}
