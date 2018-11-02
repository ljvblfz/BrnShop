using System;
using System.Data;

using BrnMall.Core;

namespace BrnMall.Services
{
    /// <summary>
    /// 后台订单操作管理类
    /// </summary>
    public partial class AdminOrders : Orders
    {
        /// <summary>
        /// 获得订单列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static DataTable GetOrderList(int pageSize, int pageNumber, string condition)
        {
            return BrnMall.Data.Orders.GetOrderList(pageSize, pageNumber, condition);
        }

        /// <summary>
        /// 获得列表搜索条件
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="osn">订单编号</param>
        /// <param name="uid">用户id</param>
        /// <param name="consignee">收货人</param>
        /// <param name="orderState">订单状态</param>
        /// <returns></returns>
        public static string GetOrderListCondition(int storeId, string osn, int uid, string consignee, int orderState)
        {
            return BrnMall.Data.Orders.GetOrderListCondition(storeId, osn, uid, consignee, orderState);
        }

        /// <summary>
        /// 获得订单数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static int GetOrderCount(string condition)
        {
            return BrnMall.Data.Orders.GetOrderCount(condition);
        }

        /// <summary>
        /// 获得销售商品列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="storeId">店铺id</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="orderState">订单状态</param>
        /// <returns></returns>
        public static DataTable GetSaleProductList(int pageSize, int pageNumber, int storeId, string startTime, string endTime, int orderState)
        {
            return BrnMall.Data.Orders.GetSaleProductList(pageSize, pageNumber, storeId, startTime, endTime, orderState);
        }

        /// <summary>
        /// 获得销售商品数量
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="orderState">订单状态</param>
        /// <returns></returns>
        public static int GetSaleProductCount(int storeId, string startTime, string endTime, int orderState)
        {
            return BrnMall.Data.Orders.GetSaleProductCount(storeId, startTime, endTime, orderState);
        }

        /// <summary>
        /// 获得订单统计
        /// </summary>
        /// <param name="statType">统计类型(0代表订单数，1代表订单合计)</param>
        /// <param name="storeId">店铺id</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public static DataTable GetOrderStat(int statType, int storeId, string startTime, string endTime)
        {
            return BrnMall.Data.Orders.GetOrderStat(statType, storeId, startTime, endTime);
        }
    }
}