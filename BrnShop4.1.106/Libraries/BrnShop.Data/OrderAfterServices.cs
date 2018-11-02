using System;
using System.Data;
using System.Collections.Generic;

using BrnShop.Core;

namespace BrnShop.Data
{
    /// <summary>
    /// 订单售后服务数据访问类
    /// </summary>
    public partial class OrderAfterServices
    {
        #region 辅助方法

        /// <summary>
        /// 通过IDataReader创建OrderAfterServiceInfo
        /// </summary>
        public static OrderAfterServiceInfo BuildOrderAfterServiceFromReader(IDataReader reader)
        {
            OrderAfterServiceInfo orderAfterServiceInfo = new OrderAfterServiceInfo();

            orderAfterServiceInfo.ASId = TypeHelper.ObjectToInt(reader["asid"]);
            orderAfterServiceInfo.State = TypeHelper.ObjectToInt(reader["state"]);
            orderAfterServiceInfo.Uid = TypeHelper.ObjectToInt(reader["uid"]);
            orderAfterServiceInfo.Oid = TypeHelper.ObjectToInt(reader["oid"]);
            orderAfterServiceInfo.RecordId = TypeHelper.ObjectToInt(reader["recordid"]);
            orderAfterServiceInfo.Pid = TypeHelper.ObjectToInt(reader["pid"]);
            orderAfterServiceInfo.CateId = TypeHelper.ObjectToInt(reader["cateid"]);
            orderAfterServiceInfo.BrandId = TypeHelper.ObjectToInt(reader["brandid"]);
            orderAfterServiceInfo.PName = reader["pname"].ToString();
            orderAfterServiceInfo.PShowImg = reader["pshowimg"].ToString();
            orderAfterServiceInfo.Count = TypeHelper.ObjectToInt(reader["count"]);
            orderAfterServiceInfo.Money = TypeHelper.ObjectToDecimal(reader["money"]);
            orderAfterServiceInfo.Type = TypeHelper.ObjectToInt(reader["type"]);
            orderAfterServiceInfo.ApplyReason = reader["applyreason"].ToString();
            orderAfterServiceInfo.ApplyTime = TypeHelper.ObjectToDateTime(reader["applytime"]);
            orderAfterServiceInfo.CheckResult = reader["checkresult"].ToString();
            orderAfterServiceInfo.CheckTime = TypeHelper.ObjectToDateTime(reader["checktime"]);
            orderAfterServiceInfo.ShipCoId1 = TypeHelper.ObjectToInt(reader["shipcoid1"]);
            orderAfterServiceInfo.ShipCoName1 = reader["shipconame1"].ToString();
            orderAfterServiceInfo.ShipSN1 = reader["shipsn1"].ToString();
            orderAfterServiceInfo.RegionId = TypeHelper.ObjectToInt(reader["regionid"]);
            orderAfterServiceInfo.Consignee = reader["consignee"].ToString();
            orderAfterServiceInfo.Mobile = reader["mobile"].ToString();
            orderAfterServiceInfo.Phone = reader["phone"].ToString();
            orderAfterServiceInfo.Email = reader["email"].ToString();
            orderAfterServiceInfo.ZipCode = reader["zipcode"].ToString();
            orderAfterServiceInfo.Address = reader["address"].ToString();
            orderAfterServiceInfo.SendTime = TypeHelper.ObjectToDateTime(reader["sendtime"]);
            orderAfterServiceInfo.ReceiveTime = TypeHelper.ObjectToDateTime(reader["receivetime"]);
            orderAfterServiceInfo.ShipCoId2 = TypeHelper.ObjectToInt(reader["shipcoid2"]);
            orderAfterServiceInfo.ShipCoName2 = reader["shipconame2"].ToString();
            orderAfterServiceInfo.ShipSN2 = reader["shipsn2"].ToString();
            orderAfterServiceInfo.BackTime = TypeHelper.ObjectToDateTime(reader["backtime"]);

            return orderAfterServiceInfo;
        }

        #endregion

        /// <summary>
        /// 申请订单售后服务
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="uid">用户id</param>
        /// <param name="oid">订单id</param>
        /// <param name="recordId">记录id</param>
        /// <param name="pid">商品id</param>
        /// <param name="cateId">分类id</param>
        /// <param name="brandId">品牌id</param>
        /// <param name="pname">商品名称</param>
        /// <param name="pshowImg">商品图片</param>
        /// <param name="count">数量</param>
        /// <param name="money">金钱</param>
        /// <param name="type">类型(0代表退货,1代表换货,2代表维修)</param>
        /// <param name="applyReason">申请原因</param>
        /// <param name="applyTime">申请时间</param>
        public static void ApplyOrderAfterService(OrderAfterServiceState state, int uid, int oid, int recordId, int pid, int cateId, int brandId, string pname, string pshowImg, int count, decimal money, int type, string applyReason, DateTime applyTime)
        {
            BrnShop.Core.BSPData.RDBS.ApplyOrderAfterService(state, uid, oid, recordId, pid, cateId, brandId, pname, pshowImg, count, money, type, applyReason, applyTime);
        }

        /// <summary>
        /// 审核订单售后服务
        /// </summary>
        /// <param name="asId">售后服务id</param>
        /// <param name="state">状态</param>
        /// <param name="checkResult">审核结果</param>
        /// <param name="checkTime">审核时间</param>
        public static void CheckOrderAfterService(int asId, OrderAfterServiceState state, string checkResult, DateTime checkTime)
        {
            BrnShop.Core.BSPData.RDBS.CheckOrderAfterService(asId, state, checkResult, checkTime);
        }

        /// <summary>
        /// 邮寄给商城
        /// </summary>
        /// <param name="asId">售后服务id</param>
        /// <param name="state">状态</param>
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
        public static void SendOrderAfterService(int asId, OrderAfterServiceState state, int shipCoId, string shipCoName, string shipSN, int regionId, string consignee, string mobile, string phone, string email, string zipCode, string address, DateTime sendTime)
        {
            BrnShop.Core.BSPData.RDBS.SendOrderAfterService(asId, state, shipCoId, shipCoName, shipSN, regionId, consignee, mobile, phone, email, zipCode, address, sendTime);
        }

        /// <summary>
        /// 商城收到客户邮寄的商品
        /// </summary>
        /// <param name="asId">售后服务id</param>
        /// <param name="state">状态</param>
        /// <param name="receiveTime">收货时间</param>
        public static void ReceiveOrderAfterService(int asId, OrderAfterServiceState state, DateTime receiveTime)
        {
            BrnShop.Core.BSPData.RDBS.ReceiveOrderAfterService(asId, state, receiveTime);
        }

        /// <summary>
        /// 邮寄给客户
        /// </summary>
        /// <param name="asId">售后服务id</param>
        /// <param name="state">状态</param>
        /// <param name="shipCoId">配送公司id</param>
        /// <param name="shipCoName">配送公司名称</param>
        /// <param name="shipSN">配送单号</param>
        /// <param name="backTime">邮寄给客户时间</param>
        public static void BackOrderAfterService(int asId, OrderAfterServiceState state, int shipCoId, string shipCoName, string shipSN, DateTime backTime)
        {
            BrnShop.Core.BSPData.RDBS.BackOrderAfterService(asId, state, shipCoId, shipCoName, shipSN, backTime);
        }

        /// <summary>
        /// 订单售后服务完成
        /// </summary>
        /// <param name="asId">售后服务id</param>
        /// <param name="state">状态</param>
        public static void CompleteOrderAfterService(int asId, OrderAfterServiceState state)
        {
            BrnShop.Core.BSPData.RDBS.CompleteOrderAfterService(asId, state);
        }

        /// <summary>
        /// 获得订单售后服务列表
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <returns></returns>
        public static List<OrderAfterServiceInfo> GetOrderAfterServiceList(int oid)
        {
            List<OrderAfterServiceInfo> orderAfterServiceList = new List<OrderAfterServiceInfo>();
            IDataReader reader = BrnShop.Core.BSPData.RDBS.GetOrderAfterServiceList(oid);
            while (reader.Read())
            {
                OrderAfterServiceInfo orderAfterServiceInfo = BuildOrderAfterServiceFromReader(reader);
                orderAfterServiceList.Add(orderAfterServiceInfo);
            }
            reader.Close();
            return orderAfterServiceList;
        }

        /// <summary>
        /// 获得订单商品售后服务列表
        /// </summary>
        /// <param name="recordId">记录id</param>
        /// <returns></returns>
        public static List<OrderAfterServiceInfo> GetOrderProductAfterServiceList(int recordId)
        {
            List<OrderAfterServiceInfo> orderAfterServiceList = new List<OrderAfterServiceInfo>();
            IDataReader reader = BrnShop.Core.BSPData.RDBS.GetOrderProductAfterServiceList(recordId);
            while (reader.Read())
            {
                OrderAfterServiceInfo orderAfterServiceInfo = BuildOrderAfterServiceFromReader(reader);
                orderAfterServiceList.Add(orderAfterServiceInfo);
            }
            reader.Close();
            return orderAfterServiceList;
        }

        /// <summary>
        /// 获得订单商品售后服务列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static DataTable GetOrderProductAfterServiceList(int pageSize, int pageNumber, string condition)
        {
            return BrnShop.Core.BSPData.RDBS.GetOrderProductAfterServiceList(pageSize, pageNumber, condition);
        }

        /// <summary>
        /// 获得订单商品售后服务数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static int GetOrderProductAfterServiceCount(string condition)
        {
            return BrnShop.Core.BSPData.RDBS.GetOrderProductAfterServiceCount(condition);
        }

        /// <summary>
        /// 获得订单商品售后服务列表搜索条件
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="oid">订单id</param>
        /// <param name="state">状态</param>
        /// <param name="type">类型</param>
        /// <param name="applyStartTime">申请开始时间</param>
        /// <param name="applyEndTime">申请结束时间</param>
        /// <returns></returns>
        public static string GetOrderProductAfterServiceListCondition(int uid, int oid, int state, int type, string applyStartTime, string applyEndTime)
        {
            return BrnShop.Core.BSPData.RDBS.GetOrderProductAfterServiceListCondition(uid, oid, state, type, applyStartTime, applyEndTime);
        }

        /// <summary>
        /// 获得订单售后服务
        /// </summary>
        /// <param name="asId">售后服务id</param>
        /// <returns></returns>
        public static OrderAfterServiceInfo GetOrderAfterServiceByASId(int asId)
        {
            OrderAfterServiceInfo orderAfterServiceInfo = null;
            IDataReader reader = BrnShop.Core.BSPData.RDBS.GetOrderAfterServiceByASId(asId);
            if (reader.Read())
            {
                orderAfterServiceInfo = BuildOrderAfterServiceFromReader(reader);
            }
            reader.Close();
            return orderAfterServiceInfo;
        }
    }
}
