using System;
using System.Data;
using System.Collections.Generic;

using BrnMall.Core;

namespace BrnMall.Services
{
    /// <summary>
    /// 订单售后服务操作管理类
    /// </summary>
    public partial class OrderAfterServices
    {
        /// <summary>
        /// 申请订单售后服务
        /// </summary>
        /// <param name="orderProductInfo">订单商品信息</param>
        /// <param name="count">数量</param>
        /// <param name="type">类型(0代表退货,1代表换货,2代表维修)</param>
        /// <param name="applyReason">申请原因</param>
        public static void ApplyOrderAfterService(OrderProductInfo orderProductInfo, int count, int type, string applyReason)
        {
            decimal money = type == 0 ? (orderProductInfo.DiscountPrice * (count < orderProductInfo.BuyCount ? count : orderProductInfo.BuyCount)) : 0M;
            ApplyOrderAfterService(orderProductInfo.Uid, orderProductInfo.Oid, orderProductInfo.RecordId, orderProductInfo.Pid, orderProductInfo.CateId, orderProductInfo.BrandId, orderProductInfo.StoreId, orderProductInfo.Name, orderProductInfo.ShowImg, count, money, type, applyReason, DateTime.Now);
        }

        /// <summary>
        /// 申请订单售后服务
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="oid">订单id</param>
        /// <param name="recordId">记录id</param>
        /// <param name="pid">商品id</param>
        /// <param name="cateId">分类id</param>
        /// <param name="brandId">品牌id</param>
        /// <param name="storeId">店铺id</param>
        /// <param name="pname">商品名称</param>
        /// <param name="pshowImg">商品图片</param>
        /// <param name="count">数量</param>
        /// <param name="money">金钱</param>
        /// <param name="type">类型(0代表退货,1代表换货,2代表维修)</param>
        /// <param name="applyReason">申请原因</param>
        /// <param name="applyTime">申请时间</param>
        public static void ApplyOrderAfterService(int uid, int oid, int recordId, int pid, int cateId, int brandId, int storeId, string pname, string pshowImg, int count, decimal money, int type, string applyReason, DateTime applyTime)
        {
            BrnMall.Data.OrderAfterServices.ApplyOrderAfterService(OrderAfterServiceState.Checking, uid, oid, recordId, pid, cateId, brandId, storeId, pname, pshowImg, count, money, type, applyReason, applyTime);
        }

        /// <summary>
        /// 邮寄给商城
        /// </summary>
        /// <param name="asId">售后服务id</param>
        /// <param name="shipCoId">配送公司id</param>
        /// <param name="shipCoName">配送公司名称</param>
        /// <param name="shipSN">配送单号</param>
        /// <param name="regionId">收货区域id</param>
        /// <param name="consignee">收货人</param>
        /// <param name="mobile">手机</param>
        /// <param name="address">收货详情地址</param>
        public static void SendOrderAfterService(int asId, int shipCoId, string shipCoName, string shipSN, int regionId, string consignee, string mobile, string address)
        {
            SendOrderAfterService(asId, shipCoId, shipCoName, shipSN, regionId, consignee, mobile, "", "", "", address, DateTime.Now);
        }

        /// <summary>
        /// 邮寄给商城
        /// </summary>
        /// <param name="asId">售后服务id</param>
        /// <param name="shipCoId">配送公司id</param>
        /// <param name="shipCoName">配送公司名称</param>
        /// <param name="shipSN">配送单号</param>
        /// <param name="regionId">收货区域id</param>
        /// <param name="consignee">收货人</param>
        /// <param name="mobile">手机</param>
        /// <param name="phone">固话</param>
        /// <param name="email">邮箱</param>
        /// <param name="zipCode">邮件编码</param>
        /// <param name="address">收货详情地址</param>
        /// <param name="sendTime">邮寄给商城时间</param>
        public static void SendOrderAfterService(int asId, int shipCoId, string shipCoName, string shipSN, int regionId, string consignee, string mobile, string phone, string email, string zipCode, string address, DateTime sendTime)
        {
            BrnMall.Data.OrderAfterServices.SendOrderAfterService(asId, OrderAfterServiceState.Sended, shipCoId, shipCoName, shipSN, regionId, consignee, mobile, phone, email, zipCode, address, sendTime);
        }

        /// <summary>
        /// 订单售后服务完成
        /// </summary>
        /// <param name="asId">售后服务id</param>
        public static void CompleteOrderAfterService(int asId)
        {
            BrnMall.Data.OrderAfterServices.CompleteOrderAfterService(asId, OrderAfterServiceState.Completed);
        }

        /// <summary>
        /// 获得订单售后服务列表
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <returns></returns>
        public static List<OrderAfterServiceInfo> GetOrderAfterServiceList(int oid)
        {
            return BrnMall.Data.OrderAfterServices.GetOrderAfterServiceList(oid);
        }

        /// <summary>
        /// 获得订单商品售后服务列表
        /// </summary>
        /// <param name="recordId">记录id</param>
        /// <returns></returns>
        public static List<OrderAfterServiceInfo> GetOrderProductAfterServiceList(int recordId)
        {
            return BrnMall.Data.OrderAfterServices.GetOrderProductAfterServiceList(recordId);
        }

        /// <summary>
        /// 获得订单售后服务
        /// </summary>
        /// <param name="asId">售后服务id</param>
        /// <returns></returns>
        public static OrderAfterServiceInfo GetOrderAfterServiceByASId(int asId)
        {
            return BrnMall.Data.OrderAfterServices.GetOrderAfterServiceByASId(asId);
        }
    }
}
