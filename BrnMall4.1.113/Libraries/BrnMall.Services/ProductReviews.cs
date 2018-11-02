using System;
using System.Data;
using System.Collections.Generic;

using BrnMall.Core;

namespace BrnMall.Services
{
    /// <summary>
    /// 商品评价操作管理类
    /// </summary>
    public partial class ProductReviews
    {
        /// <summary>
        /// 获得商品评价
        /// </summary>
        /// <param name="reviewId">评价id</param>
        /// <returns></returns>
        public static ProductReviewInfo GetProductReviewById(int reviewId)
        {
            return BrnMall.Data.ProductReviews.GetProductReviewById(reviewId);
        }

        /// <summary>
        /// 评价商品
        /// </summary>
        public static void ReviewProduct(ProductReviewInfo productReviewInfo)
        {
            BrnMall.Data.ProductReviews.ReviewProduct(productReviewInfo);
        }

        /// <summary>
        /// 对商品评价投票
        /// </summary>
        /// <param name="reviewId">评价id</param>
        /// <param name="uid">用户id</param>
        /// <param name="voteTime">投票时间</param>
        public static void VoteProductReview(int reviewId, int uid, DateTime voteTime)
        {
            BrnMall.Data.ProductReviews.VoteProductReview(reviewId, uid, voteTime);
        }

        /// <summary>
        /// 是否对商品评价投过票
        /// </summary>
        /// <param name="reviewId">评价id</param>
        /// <param name="uid">用户id</param>
        public static bool IsVoteProductReview(int reviewId, int uid)
        {
            return BrnMall.Data.ProductReviews.IsVoteProductReview(reviewId, uid);
        }

        /// <summary>
        /// 更改商品评价状态
        /// </summary>
        /// <param name="reviewId">评价id</param>
        /// <param name="state">评价状态</param>
        /// <returns></returns>
        public static bool ChangeProductReviewState(int reviewId, int state)
        {
            if (reviewId > 0 && (state == 0 || state == 1))
                return BrnMall.Data.ProductReviews.ChangeProductReviewState(reviewId, state);
            return false;
        }

        /// <summary>
        /// 获得用户商品评价列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public static List<ProductReviewInfo> GetUserProductReviewList(int uid, int pageSize, int pageNumber)
        {
            return BrnMall.Data.ProductReviews.GetUserProductReviewList(uid, pageSize, pageNumber);
        }

        /// <summary>
        /// 获得用户商品评价数量
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public static int GetUserProductReviewCount(int uid)
        {
            return BrnMall.Data.ProductReviews.GetUserProductReviewCount(uid);
        }

        /// <summary>
        /// 获得商品评价列表
        /// </summary>
        /// <param name="pid">商品id</param>
        /// <param name="type">类型(0代表全部评价,1代表好评,2代表中评,3代表差评)</param>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <returns></returns>
        public static DataTable GetProductReviewList(int pid, int type, int pageSize, int pageNumber)
        {
            return BrnMall.Data.ProductReviews.GetProductReviewList(pid, type, pageSize, pageNumber);
        }

        /// <summary>
        /// 获得商品评价数量
        /// </summary>
        /// <param name="pid">商品id</param>
        /// <param name="type">类型(0代表全部评价,1代表好评,2代表中评,3代表差评)</param>
        /// <returns></returns>
        public static int GetProductReviewCount(int pid, int type)
        {
            return BrnMall.Data.ProductReviews.GetProductReviewCount(pid, type);
        }

        /// <summary>
        /// 获得商品评价及其回复
        /// </summary>
        /// <param name="reviewId">评价id</param>
        /// <returns></returns>
        public static DataTable GetProductReviewWithReplyById(int reviewId)
        {
            return BrnMall.Data.ProductReviews.GetProductReviewWithReplyById(reviewId);
        }

        /// <summary>
        /// 获得商品评价列表
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public static DataTable GetProductReviewList(DateTime startTime, DateTime endTime)
        {
            return BrnMall.Data.ProductReviews.GetProductReviewList(startTime, endTime);
        }
    }
}
