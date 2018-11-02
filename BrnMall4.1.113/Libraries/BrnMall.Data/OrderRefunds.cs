using System;
using System.Data;
using System.Collections.Generic;

using BrnMall.Core;

namespace BrnMall.Data
{
    /// <summary>
    /// 订单退款数据访问类
    /// </summary>
    public partial class OrderRefunds
    {
        #region 辅助方法

        /// <summary>
        /// 从IDataReader创建OrderRefundInfo
        /// </summary>
        public static OrderRefundInfo BuildOrderRefundFromReader(IDataReader reader)
        {
            OrderRefundInfo orderRefundInfo = new OrderRefundInfo();

            orderRefundInfo.RefundId = TypeHelper.ObjectToInt(reader["refundid"]);
            orderRefundInfo.State = TypeHelper.ObjectToInt(reader["state"]);
            orderRefundInfo.StoreId = TypeHelper.ObjectToInt(reader["storeid"]);
            orderRefundInfo.StoreName = reader["storename"].ToString();
            orderRefundInfo.Oid = TypeHelper.ObjectToInt(reader["oid"]);
            orderRefundInfo.OSN = reader["osn"].ToString();
            orderRefundInfo.Uid = TypeHelper.ObjectToInt(reader["uid"]);
            orderRefundInfo.ASId = TypeHelper.ObjectToInt(reader["asid"]);
            orderRefundInfo.PaySystemName = reader["paysystemname"].ToString();
            orderRefundInfo.PayFriendName = reader["payfriendname"].ToString();
            orderRefundInfo.PaySN = reader["paysn"].ToString();
            orderRefundInfo.PayMoney = TypeHelper.ObjectToDecimal(reader["paymoney"]);
            orderRefundInfo.RefundMoney = TypeHelper.ObjectToDecimal(reader["refundmoney"]);
            orderRefundInfo.ApplyTime = TypeHelper.ObjectToDateTime(reader["applytime"]);
            orderRefundInfo.RefundSN = reader["refundsn"].ToString();
            orderRefundInfo.RefundTime = TypeHelper.ObjectToDateTime(reader["refundtime"]);

            return orderRefundInfo;
        }

        #endregion

        /// <summary>
        /// 申请退款
        /// </summary>
        /// <param name="orderRefundInfo">订单退款信息</param>
        public static void ApplyRefund(OrderRefundInfo orderRefundInfo)
        {
            BrnMall.Core.BMAData.RDBS.ApplyRefund(orderRefundInfo);
        }

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="refundId">退款id</param>
        /// <param name="state">状态</param>
        /// <param name="refundSN">退款单号</param>
        /// <param name="refundTime">退款时间</param>
        public static void RefundOrder(int refundId, OrderRefundState state, string refundSN, DateTime refundTime)
        {
            BrnMall.Core.BMAData.RDBS.RefundOrder(refundId, state, refundSN, refundTime);
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
            List<OrderRefundInfo> orderRefundList = new List<OrderRefundInfo>();
            IDataReader reader = BrnMall.Core.BMAData.RDBS.GetOrderRefundList(pageSize, pageNumber, condition);
            while (reader.Read())
            {
                OrderRefundInfo orderRefundInfo = BuildOrderRefundFromReader(reader);
                orderRefundList.Add(orderRefundInfo);
            }

            reader.Close();
            return orderRefundList;
        }

        /// <summary>
        /// 获得订单退款列表条件
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="osn">订单编号</param>
        /// <returns></returns>
        public static string GetOrderRefundListCondition(int storeId, string osn)
        {
            return BrnMall.Core.BMAData.RDBS.GetOrderRefundListCondition(storeId, osn);
        }

        /// <summary>
        /// 获得订单退款数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static int GetOrderRefundCount(string condition)
        {
            return BrnMall.Core.BMAData.RDBS.GetOrderRefundCount(condition);
        }
    }
}
