using System;
using System.Web.Mvc;

using BrnMall.Core;
using BrnMall.Services;
using BrnMall.Web.Framework;
using BrnMall.Web.Mobile.Models;

namespace BrnMall.Web.Mobile.Controllers
{
    /// <summary>
    /// 优惠劵控制器类
    /// </summary>
    public partial class CouponController : BaseMobileController
    {
        private static object _locker = new object();//锁对象

        /// <summary>
        /// 领取优惠劵
        /// </summary>
        public ActionResult GetCoupon()
        {
            lock (_locker)
            {
                //判断用户是否登录
                if (WorkContext.Uid < 1)
                    return AjaxResult("login", "请先登录");

                //优惠劵类型id
                int couponTypeId = GetRouteInt("couponTypeId");
                if (couponTypeId == 0)
                    couponTypeId = WebHelper.GetQueryInt("couponTypeId");

                CouponTypeInfo couponTypeInfo = Coupons.GetCouponTypeById(couponTypeId);
                //判断优惠劵类型是否存在
                if (couponTypeInfo == null || couponTypeInfo.SendMode != 0)
                    return AjaxResult("noexist", "优惠劵不存在");
                //判断优惠劵类型是否开始领取
                if (couponTypeInfo.SendStartTime > DateTime.Now)
                    return AjaxResult("unstart", "优惠劵还未开始");
                //判断优惠劵类型是否结束领取
                if (couponTypeInfo.SendEndTime <= DateTime.Now)
                    return AjaxResult("expired", "优惠劵已过期");

                //判断优惠劵类型是否已经领取
                if ((couponTypeInfo.GetMode == 1 && Coupons.GetSendUserCouponCount(WorkContext.Uid, couponTypeId) > 1) || (couponTypeInfo.GetMode == 2 && Coupons.GetTodaySendUserCouponCount(WorkContext.Uid, couponTypeId, DateTime.Now) > 1))
                    return AjaxResult("alreadyget", "优惠劵已经被领取");

                //判断优惠劵是否已经领尽
                int sendCount = Coupons.GetSendCouponCount(couponTypeId);
                if (sendCount >= couponTypeInfo.Count)
                    return AjaxResult("stockout", "优惠劵已领尽");

                string couponSN = Coupons.PullCoupon(WorkContext.PartUserInfo, couponTypeInfo, DateTime.Now, WorkContext.IP);
                return AjaxResult("success", couponSN);
            }
        }
    }
}
