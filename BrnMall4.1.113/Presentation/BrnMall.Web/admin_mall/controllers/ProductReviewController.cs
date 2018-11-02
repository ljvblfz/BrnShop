using System;
using System.Data;
using System.Web.Mvc;

using BrnMall.Core;
using BrnMall.Services;
using BrnMall.Web.Framework;
using BrnMall.Web.MallAdmin.Models;

namespace BrnMall.Web.MallAdmin.Controllers
{
    /// <summary>
    /// 商城后台商品评价控制器类
    /// </summary>
    public partial class ProductReviewController : BaseMallAdminController
    {
        /// <summary>
        /// 商品评价列表
        /// </summary>
        public ActionResult ProductReviewList(string storeName, string message, string rateStartTime, string rateEndTime, int storeId = -1, int pid = 0, int pageNumber = 1, int pageSize = 15)
        {
            string condition = AdminProductReviews.AdminGetProductReviewListCondition(storeId, pid, message, rateStartTime, rateEndTime);

            PageModel pageModel = new PageModel(pageSize, pageNumber, AdminProductReviews.AdminGetProductReviewCount(condition));
            ProductReviewListModel model = new ProductReviewListModel()
            {
                PageModel = pageModel,
                ProductReviewList = AdminProductReviews.AdminGetProductReviewList(pageModel.PageSize, pageModel.PageNumber, condition),
                StoreId = storeId,
                StoreName = string.IsNullOrWhiteSpace(storeName) ? "全部店铺" : storeName,
                Pid = pid,
                Message = message,
                StartTime = rateStartTime,
                EndTime = rateEndTime
            };
            MallUtils.SetAdminRefererCookie(string.Format("{0}?pageNumber={1}&pageSize={2}&storeId={3}&storeName={4}&pid={5}&message={6}&startTime={7}&endTime={8}",
                                                            Url.Action("productreviewlist"),
                                                            pageModel.PageNumber, pageModel.PageSize,
                                                            storeId, storeName, pid,
                                                            message, rateStartTime, rateEndTime));
            return View(model);
        }

        /// <summary>
        /// 商品评价回复列表
        /// </summary>
        public ActionResult ProductReviewReplyList(int reviewId = -1)
        {
            ProductReviewInfo productReviewInfo = AdminProductReviews.AdminGetProductReviewById(reviewId);
            if (productReviewInfo == null)
                return PromptView("商品评价不存在");

            ProductReviewReplyListModel model = new ProductReviewReplyListModel()
            {
                ProductReviewReplyList = AdminProductReviews.AdminGetProductReviewReplyList(reviewId),
            };
            MallUtils.SetAdminRefererCookie(string.Format("{0}?reviewId={1}", Url.Action("productreviewreplylist"), reviewId));
            return View(model);
        }

        /// <summary>
        /// 改变商品评价的状态
        /// </summary>
        public ActionResult ChangeProductReviewState(int reviewId = -1, int state = -1)
        {
            bool result = AdminProductReviews.ChangeProductReviewState(reviewId, state);
            if (result)
            {
                AddMallAdminLog("修改商品评价状态", "修改商品评价状态,商品评价ID和状态为:" + reviewId + "_" + state);
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }

        /// <summary>
        /// 删除商品评价
        /// </summary>
        public ActionResult DelProductReview(int reviewId)
        {
            AdminProductReviews.DeleteProductReviewById(reviewId);
            AddMallAdminLog("删除商品评价", "删除商品评价,商品评价ID为:" + reviewId);
            return PromptView("商品评价删除成功");
        }
    }
}
