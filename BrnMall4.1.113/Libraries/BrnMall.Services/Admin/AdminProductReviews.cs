using System;
using System.Data;
using System.Collections.Generic;

using BrnMall.Core;

namespace BrnMall.Services
{
    /// <summary>
    /// 后台商品评价操作管理类
    /// </summary>
    public partial class AdminProductReviews : ProductReviews
    {
        /// <summary>
        /// 后台获得商品评价
        /// </summary>
        /// <param name="reviewId">评价id</param>
        /// <returns></returns>
        public static ProductReviewInfo AdminGetProductReviewById(int reviewId)
        {
            return BrnMall.Data.ProductReviews.AdminGetProductReviewById(reviewId);
        }

        /// <summary>
        /// 删除商品评价
        /// </summary>
        /// <param name="reviewId">评价id</param>
        public static void DeleteProductReviewById(int reviewId)
        {
            if (reviewId > 0)
                BrnMall.Data.ProductReviews.DeleteProductReviewById(reviewId);
        }

        /// <summary>
        /// 后台获得商品评价列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static DataTable AdminGetProductReviewList(int pageSize, int pageNumber, string condition)
        {
            return BrnMall.Data.ProductReviews.AdminGetProductReviewList(pageSize, pageNumber, condition);
        }

        /// <summary>
        /// 后台获得商品评价列表条件
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="pid">商品id</param>
        /// <param name="message">评价内容</param>
        /// <param name="startTime">评价开始时间</param>
        /// <param name="endTime">评价结束时间</param>
        /// <returns></returns>
        public static string AdminGetProductReviewListCondition(int storeId, int pid, string message, string startTime, string endTime)
        {
            return BrnMall.Data.ProductReviews.AdminGetProductReviewListCondition(storeId, pid, message, startTime, endTime);
        }

        /// <summary>
        /// 后台获得商品评价数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static int AdminGetProductReviewCount(string condition)
        {
            return BrnMall.Data.ProductReviews.AdminGetProductReviewCount(condition);
        }

        /// <summary>
        /// 后台获得商品评价回复列表
        /// </summary>
        /// <param name="reviewId">评价id</param>
        /// <returns></returns>
        public static DataTable AdminGetProductReviewReplyList(int reviewId)
        {
            return BrnMall.Data.ProductReviews.AdminGetProductReviewReplyList(reviewId);
        }
    }
}
