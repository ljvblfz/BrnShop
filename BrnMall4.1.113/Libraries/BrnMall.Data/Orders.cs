using System;
using System.Data;
using System.Collections.Generic;

using BrnMall.Core;

namespace BrnMall.Data
{
    /// <summary>
    /// 订单数据访问类
    /// </summary>
    public partial class Orders
    {
        private static IOrderNOSQLStrategy _ordernosql = BMAData.OrderNOSQL;//订单非关系型数据库

        #region 辅助方法

        /// <summary>
        /// 从IDataReader创建OrderProductInfo
        /// </summary>
        public static OrderProductInfo BuildOrderProductFromReader(IDataReader reader)
        {
            OrderProductInfo orderProductInfo = new OrderProductInfo();
            orderProductInfo.RecordId = TypeHelper.ObjectToInt(reader["recordid"]);
            orderProductInfo.Oid = TypeHelper.ObjectToInt(reader["oid"]);
            orderProductInfo.Uid = TypeHelper.ObjectToInt(reader["uid"]);
            orderProductInfo.Sid = reader["sid"].ToString();
            orderProductInfo.Pid = TypeHelper.ObjectToInt(reader["pid"]);
            orderProductInfo.PSN = reader["psn"].ToString();
            orderProductInfo.CateId = TypeHelper.ObjectToInt(reader["cateid"]);
            orderProductInfo.BrandId = TypeHelper.ObjectToInt(reader["brandid"]);
            orderProductInfo.StoreId = TypeHelper.ObjectToInt(reader["storeid"]);
            orderProductInfo.StoreCid = TypeHelper.ObjectToInt(reader["storecid"]);
            orderProductInfo.StoreSTid = TypeHelper.ObjectToInt(reader["storestid"]);
            orderProductInfo.Name = reader["name"].ToString();
            orderProductInfo.ShowImg = reader["showimg"].ToString();
            orderProductInfo.DiscountPrice = TypeHelper.ObjectToDecimal(reader["discountprice"]);
            orderProductInfo.ShopPrice = TypeHelper.ObjectToDecimal(reader["shopprice"]);
            orderProductInfo.CostPrice = TypeHelper.ObjectToDecimal(reader["costprice"]);
            orderProductInfo.MarketPrice = TypeHelper.ObjectToDecimal(reader["marketprice"]);
            orderProductInfo.Weight = TypeHelper.ObjectToInt(reader["weight"]);
            orderProductInfo.IsReview = TypeHelper.ObjectToInt(reader["isreview"]);
            orderProductInfo.RealCount = TypeHelper.ObjectToInt(reader["realcount"]);
            orderProductInfo.BuyCount = TypeHelper.ObjectToInt(reader["buycount"]);
            orderProductInfo.SendCount = TypeHelper.ObjectToInt(reader["sendcount"]);
            orderProductInfo.Type = TypeHelper.ObjectToInt(reader["type"]);
            orderProductInfo.PayCredits = TypeHelper.ObjectToInt(reader["paycredits"]);
            orderProductInfo.CouponTypeId = TypeHelper.ObjectToInt(reader["coupontypeid"]);
            orderProductInfo.ExtCode1 = TypeHelper.ObjectToInt(reader["extcode1"]);
            orderProductInfo.ExtCode2 = TypeHelper.ObjectToInt(reader["extcode2"]);
            orderProductInfo.ExtCode3 = TypeHelper.ObjectToInt(reader["extcode3"]);
            orderProductInfo.ExtCode4 = TypeHelper.ObjectToInt(reader["extcode4"]);
            orderProductInfo.ExtCode5 = TypeHelper.ObjectToInt(reader["extcode5"]);
            orderProductInfo.AddTime = TypeHelper.ObjectToDateTime(reader["addtime"]);
            return orderProductInfo;
        }

        /// <summary>
        /// 从IDataReader创建OrderInfo
        /// </summary>
        public static OrderInfo BuildOrderFromReader(IDataReader reader)
        {
            OrderInfo orderInfo = new OrderInfo();

            orderInfo.Oid = TypeHelper.ObjectToInt(reader["oid"]);
            orderInfo.OSN = reader["osn"].ToString();
            orderInfo.Uid = TypeHelper.ObjectToInt(reader["uid"]);

            orderInfo.OrderState = TypeHelper.ObjectToInt(reader["orderstate"]);

            orderInfo.ProductAmount = TypeHelper.ObjectToDecimal(reader["productamount"]);
            orderInfo.OrderAmount = TypeHelper.ObjectToDecimal(reader["orderamount"]);
            orderInfo.SurplusMoney = TypeHelper.ObjectToDecimal(reader["surplusmoney"]);

            orderInfo.IsReview = TypeHelper.ObjectToInt(reader["isreview"]);
            orderInfo.AddTime = TypeHelper.ObjectToDateTime(reader["addtime"]);
            orderInfo.StoreId = TypeHelper.ObjectToInt(reader["storeid"]);
            orderInfo.StoreName = reader["storename"].ToString();
            orderInfo.ShipSN = reader["shipsn"].ToString();
            orderInfo.ShipCoId = TypeHelper.ObjectToInt(reader["shipcoid"]);
            orderInfo.ShipCoName = reader["shipconame"].ToString();
            orderInfo.ShipTime = TypeHelper.ObjectToDateTime(reader["shiptime"]);
            orderInfo.PayMode = TypeHelper.ObjectToInt(reader["paymode"]);
            orderInfo.PaySN = reader["paysn"].ToString();
            orderInfo.PayFriendName = reader["payfriendname"].ToString();
            orderInfo.PaySystemName = reader["paysystemname"].ToString();
            orderInfo.PayTime = TypeHelper.ObjectToDateTime(reader["paytime"]);

            orderInfo.RegionId = TypeHelper.ObjectToInt(reader["regionid"]);
            orderInfo.Consignee = reader["consignee"].ToString();
            orderInfo.Mobile = reader["mobile"].ToString();
            orderInfo.Phone = reader["phone"].ToString();
            orderInfo.Email = reader["email"].ToString();
            orderInfo.ZipCode = reader["zipcode"].ToString();
            orderInfo.Address = reader["address"].ToString();
            orderInfo.BestTime = TypeHelper.ObjectToDateTime(reader["besttime"]);

            orderInfo.ShipFee = TypeHelper.ObjectToDecimal(reader["shipfee"]);
            orderInfo.FullCut = TypeHelper.ObjectToInt(reader["fullcut"]);
            orderInfo.Discount = TypeHelper.ObjectToDecimal(reader["discount"]);
            orderInfo.PayCreditCount = TypeHelper.ObjectToInt(reader["paycreditcount"]);
            orderInfo.PayCreditMoney = TypeHelper.ObjectToDecimal(reader["paycreditmoney"]);
            orderInfo.CouponMoney = TypeHelper.ObjectToInt(reader["couponmoney"]);
            orderInfo.Weight = TypeHelper.ObjectToInt(reader["weight"]);

            orderInfo.BuyerRemark = reader["buyerremark"].ToString();
            orderInfo.IP = reader["ip"].ToString();

            return orderInfo;
        }

        #endregion

        /// <summary>
        /// 获得订单信息
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <returns>订单信息</returns>
        public static OrderInfo GetOrderByOid(int oid)
        {
            OrderInfo orderInfo = null;

            if (_ordernosql != null)
            {
                orderInfo = _ordernosql.GetOrderByOid(oid);
                if (orderInfo == null)
                {
                    IDataReader reader = BrnMall.Core.BMAData.RDBS.GetOrderByOid(oid);
                    if (reader.Read())
                    {
                        orderInfo = BuildOrderFromReader(reader);
                    }
                    reader.Close();
                    if (orderInfo != null)
                        _ordernosql.CreateOrder(orderInfo);
                }
            }
            else
            {
                IDataReader reader = BrnMall.Core.BMAData.RDBS.GetOrderByOid(oid);
                if (reader.Read())
                {
                    orderInfo = BuildOrderFromReader(reader);
                }
                reader.Close();
            }

            return orderInfo;
        }

        /// <summary>
        /// 获得订单信息
        /// </summary>
        /// <param name="osn">订单编号</param>
        /// <returns>订单信息</returns>
        public static OrderInfo GetOrderByOSN(string osn)
        {
            OrderInfo orderInfo = null;
            IDataReader reader = BrnMall.Core.BMAData.RDBS.GetOrderByOSN(osn);
            if (reader.Read())
            {
                orderInfo = BuildOrderFromReader(reader);
            }
            reader.Close();
            return orderInfo;
        }

        /// <summary>
        /// 获得订单数量
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="orderState">订单状态</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public static int GetOrderCountByCondition(int storeId, int orderState, string startTime, string endTime)
        {
            return BrnMall.Core.BMAData.RDBS.GetOrderCountByCondition(storeId, orderState, startTime, endTime);
        }

        /// <summary>
        /// 获得订单列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static DataTable GetOrderList(int pageSize, int pageNumber, string condition)
        {
            return BrnMall.Core.BMAData.RDBS.GetOrderList(pageSize, pageNumber, condition);
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
            return BrnMall.Core.BMAData.RDBS.GetOrderListCondition(storeId, osn, uid, consignee, orderState);
        }

        /// <summary>
        /// 获得订单数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static int GetOrderCount(string condition)
        {
            return BrnMall.Core.BMAData.RDBS.GetOrderCount(condition);
        }

        /// <summary>
        /// 获得订单商品列表
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <returns></returns>
        public static List<OrderProductInfo> GetOrderProductList(int oid)
        {
            List<OrderProductInfo> orderProductList = null;

            if (_ordernosql != null)
            {
                orderProductList = _ordernosql.GetOrderProductList(oid);
                if (orderProductList == null)
                {
                    orderProductList = new List<OrderProductInfo>();
                    IDataReader reader = BrnMall.Core.BMAData.RDBS.GetOrderProductList(oid);
                    while (reader.Read())
                    {
                        OrderProductInfo orderProductInfo = BuildOrderProductFromReader(reader);
                        orderProductList.Add(orderProductInfo);
                    }
                    reader.Close();
                    _ordernosql.CreateOrderProductList(oid, orderProductList);
                }
            }
            else
            {
                orderProductList = new List<OrderProductInfo>();
                IDataReader reader = BrnMall.Core.BMAData.RDBS.GetOrderProductList(oid);
                while (reader.Read())
                {
                    OrderProductInfo orderProductInfo = BuildOrderProductFromReader(reader);
                    orderProductList.Add(orderProductInfo);
                }
                reader.Close();
            }

            return orderProductList;
        }

        /// <summary>
        /// 获得订单商品列表
        /// </summary>
        /// <param name="oidList">订单id列表</param>
        /// <returns></returns>
        public static List<OrderProductInfo> GetOrderProductList(string oidList)
        {
            List<OrderProductInfo> orderProductList = new List<OrderProductInfo>();

            if (_ordernosql != null)
            {
                foreach (string oid in StringHelper.SplitString(oidList))
                    orderProductList.AddRange(GetOrderProductList(TypeHelper.StringToInt(oid)));
            }
            else
            {
                IDataReader reader = BrnMall.Core.BMAData.RDBS.GetOrderProductList(oidList);
                while (reader.Read())
                {
                    OrderProductInfo orderProductInfo = BuildOrderProductFromReader(reader);
                    orderProductList.Add(orderProductInfo);
                }
                reader.Close();
            }

            return orderProductList;
        }

        /// <summary>
        /// 更新订单折扣
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <param name="discount">折扣</param>
        /// <param name="surplusMoney">剩余金额</param>
        public static void UpdateOrderDiscount(int oid, decimal discount, decimal surplusMoney)
        {
            BrnMall.Core.BMAData.RDBS.UpdateOrderDiscount(oid, discount, surplusMoney);
            if (_ordernosql != null)
                _ordernosql.UpdateOrderDiscount(oid, discount, surplusMoney);
        }

        /// <summary>
        /// 更新订单配送费用
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <param name="shipFee">配送费用</param>
        /// <param name="orderAmount">订单合计</param>
        /// <param name="surplusMoney">剩余金额</param>
        public static void UpdateOrderShipFee(int oid, decimal shipFee, decimal orderAmount, decimal surplusMoney)
        {
            BrnMall.Core.BMAData.RDBS.UpdateOrderShipFee(oid, shipFee, orderAmount, surplusMoney);
            if (_ordernosql != null)
                _ordernosql.UpdateOrderShipFee(oid, shipFee, orderAmount, surplusMoney);
        }

        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <param name="orderState">订单状态</param>
        public static void UpdateOrderState(int oid, OrderState orderState)
        {
            BrnMall.Core.BMAData.RDBS.UpdateOrderState(oid, orderState);
            if (_ordernosql != null)
                _ordernosql.UpdateOrderState(oid, orderState);
        }

        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <param name="orderState">订单状态</param>
        /// <param name="shipSN">配送单号</param>
        /// <param name="shipCoId">配送公司id</param>
        /// <param name="shipCoName">配送公司名称</param>
        /// <param name="shipTime">配送时间</param>
        public static void SendOrderProduct(int oid, OrderState orderState, string shipSN, int shipCoId, string shipCoName, DateTime shipTime)
        {
            BrnMall.Core.BMAData.RDBS.SendOrderProduct(oid, orderState, shipSN, shipCoId, shipCoName, shipTime);
            if (_ordernosql != null)
                _ordernosql.SendOrderProduct(oid, orderState, shipSN, shipCoId, shipCoName, shipTime);
        }

        /// <summary>
        /// 付款
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <param name="orderState">订单状态</param>
        /// <param name="paySN">支付单号</param>
        /// <param name="paySystemName">支付系统名称</param>
        /// <param name="payFriendName">支付友好名称</param>
        /// <param name="payTime">支付时间</param>
        public static void PayOrder(int oid, OrderState orderState, string paySN, string paySystemName, string payFriendName, DateTime payTime)
        {
            BrnMall.Core.BMAData.RDBS.PayOrder(oid, orderState, paySN, paySystemName, payFriendName, payTime);
            if (_ordernosql != null)
                _ordernosql.PayOrder(oid, orderState, paySN, payTime);
        }

        /// <summary>
        /// 更新订单的评价
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <param name="isReview">是否评价</param>
        public static void UpdateOrderIsReview(int oid, int isReview)
        {
            BrnMall.Core.BMAData.RDBS.UpdateOrderIsReview(oid, isReview);
            if (_ordernosql != null)
                _ordernosql.UpdateOrderIsReview(oid, isReview);
        }

        /// <summary>
        /// 清空过期的在线支付订单
        /// </summary>
        /// <param name="expireTime">过期时间</param>
        public static void ClearExpiredOnlinePayOrder(DateTime expireTime)
        {
            BrnMall.Core.BMAData.RDBS.ClearExpiredOnlinePayOrder(expireTime);
        }

        /// <summary>
        /// 自动收货
        /// </summary>
        /// <param name="expireTime">过期时间</param>
        public static void AutoReceiveOrder(DateTime expireTime)
        {
            BrnMall.Core.BMAData.RDBS.AutoReceiveOrder(expireTime);
        }

        /// <summary>
        /// 获得用户订单列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="startAddTime">添加开始时间</param>
        /// <param name="endAddTime">添加结束时间</param>
        /// <param name="orderState">订单状态(0代表全部状态)</param>
        /// <returns></returns>
        public static DataTable GetUserOrderList(int uid, int pageSize, int pageNumber, string startAddTime, string endAddTime, int orderState)
        {
            return BrnMall.Core.BMAData.RDBS.GetUserOrderList(uid, pageSize, pageNumber, startAddTime, endAddTime, orderState);
        }

        /// <summary>
        /// 获得用户订单列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="startAddTime">添加开始时间</param>
        /// <param name="endAddTime">添加结束时间</param>
        /// <param name="orderState">订单状态(0代表全部状态)</param>
        /// <returns></returns>
        public static int GetUserOrderCount(int uid, string startAddTime, string endAddTime, int orderState)
        {
            return BrnMall.Core.BMAData.RDBS.GetUserOrderCount(uid, startAddTime, endAddTime, orderState);
        }

        /// <summary>
        /// 获得用户未评价订单列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="startAddTime">添加开始时间</param>
        /// <param name="endAddTime">添加结束时间</param>
        /// <returns></returns>
        public static DataTable GetUserUnReviewOrderList(int uid, int pageSize, int pageNumber, string startAddTime, string endAddTime)
        {
            return BrnMall.Core.BMAData.RDBS.GetUserUnReviewOrderList(uid, pageSize, pageNumber, startAddTime, endAddTime);
        }

        /// <summary>
        /// 获得用户未评价订单列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="startAddTime">添加开始时间</param>
        /// <param name="endAddTime">添加结束时间</param>
        /// <returns></returns>
        public static int GetUserUnReviewOrderCount(int uid, string startAddTime, string endAddTime)
        {
            return BrnMall.Core.BMAData.RDBS.GetUserUnReviewOrderCount(uid, startAddTime, endAddTime);
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
            return BrnMall.Core.BMAData.RDBS.GetSaleProductList(pageSize, pageNumber, storeId, startTime, endTime, orderState);
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
            return BrnMall.Core.BMAData.RDBS.GetSaleProductCount(storeId, startTime, endTime, orderState);
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
            return BrnMall.Core.BMAData.RDBS.GetOrderStat(statType, storeId, startTime, endTime);
        }
    }
}