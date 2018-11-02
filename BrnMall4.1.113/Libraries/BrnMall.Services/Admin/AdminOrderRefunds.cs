using System;
using System.Collections.Generic;

using BrnMall.Core;

namespace BrnMall.Services
{
    /// <summary>
    /// 后台订单退款操作管理类
    /// </summary>
    public partial class AdminOrderRefunds : OrderRefunds
    {
        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="refundId">退款id</param>
        /// <param name="state">状态</param>
        /// <param name="refundSN">退款单号</param>
        /// <param name="refundTime">退款时间</param>
        public static void RefundOrder(int refundId, OrderRefundState state, string refundSN, DateTime refundTime)
        {
            BrnMall.Data.OrderRefunds.RefundOrder(refundId, state, refundSN, refundTime);
        }

        /// <summary>
        /// 获得订单退款列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static List<OrderRefundInfo> GetOrderRefundList(int pageSize, int pageNumber, string condition)
        {
            return BrnMall.Data.OrderRefunds.GetOrderRefundList(pageSize, pageNumber, condition);
        }

        /// <summary>
        /// 获得订单退款列表条件
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="osn">订单编号</param>
        /// <returns></returns>
        public static string GetOrderRefundListCondition(int storeId, string osn)
        {
            return BrnMall.Data.OrderRefunds.GetOrderRefundListCondition(storeId, osn);
        }

        /// <summary>
        /// 获得订单退款数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static int GetOrderRefundCount(string condition)
        {
            return BrnMall.Data.OrderRefunds.GetOrderRefundCount(condition);
        }
    }
}
