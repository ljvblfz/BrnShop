using System;
using System.Data;
using System.Web.Mvc;

using BrnMall.Core;
using BrnMall.Services;
using BrnMall.Web.Framework;
using BrnMall.Web.StoreAdmin.Models;

namespace BrnMall.Web.StoreAdmin.Controllers
{
    /// <summary>
    /// 店铺后台商品评价控制器类
    /// </summary>
    public partial class ProductReviewController : BaseStoreAdminController
    {
        /// <summary>
        /// 商品评价列表
        /// </summary>
        public ActionResult ProductReviewList(string productName, string message, string rateStartTime, string rateEndTime, int pid = -1, int pageNumber = 1, int pageSize = 15)
        {
            string condition = AdminProductReviews.AdminGetProductReviewListCondition(WorkContext.StoreId, pid, message, rateStartTime, rateEndTime);

            PageModel pageModel = new PageModel(pageSize, pageNumber, AdminProductReviews.AdminGetProductReviewCount(condition));
            ProductReviewListModel model = new ProductReviewListModel()
            {
                PageModel = pageModel,
                ProductReviewList = AdminProductReviews.AdminGetProductReviewList(pageModel.PageSize, pageModel.PageNumber, condition),
                Pid = pid,
                ProductName = string.IsNullOrWhiteSpace(productName) ? "选择商品" : productName,
                Message = message,
                StartTime = rateStartTime,
                EndTime = rateEndTime
            };
            MallUtils.SetAdminRefererCookie(string.Format("{0}?pageNumber={1}&pageSize={2}&message={3}&pid={4}&productName={5}&startTime={6}&endTime={7}",
                                                            Url.Action("productreviewlist"),
                                                            pageModel.PageNumber, pageModel.PageSize,
                                                            message,
                                                            pid, productName,
                                                            rateStartTime, rateEndTime));
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
            if (productReviewInfo.StoreId != WorkContext.StoreId)
                return PromptView("不能访问其它店铺的商品评价");

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
            ProductReviewInfo productReviewInfo = AdminProductReviews.AdminGetProductReviewById(reviewId);
            if (productReviewInfo == null || productReviewInfo.StoreId != WorkContext.StoreId)
                return Content("0");

            bool result = AdminProductReviews.ChangeProductReviewState(reviewId, state);
            if (result)
            {
                AddStoreAdminLog("修改商品评价状态", "修改商品评价状态,商品评价ID和状态为:" + reviewId + "_" + state);
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
            ProductReviewInfo productReviewInfo = AdminProductReviews.AdminGetProductReviewById(reviewId);
            if (productReviewInfo == null)
                return PromptView("商品评价不存在");
            if (productReviewInfo.StoreId != WorkContext.StoreId)
                return PromptView("不能删除其它店铺的商品评价");

            AdminProductReviews.DeleteProductReviewById(reviewId);
            AddStoreAdminLog("删除商品评价", "删除商品评价,商品评价ID为:" + reviewId);
            return PromptView("商品评价删除成功");
        }
    }
}
