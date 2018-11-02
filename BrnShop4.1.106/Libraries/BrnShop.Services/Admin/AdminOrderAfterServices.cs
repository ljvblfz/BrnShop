using System;
using System.Data;

using BrnShop.Core;

namespace BrnShop.Services
{
    /// <summary>
    /// 后台订单售后服务操作管理类
    /// </summary>
    public partial class AdminOrderAfterServices : OrderAfterServices
    {
        /// <summary>
        /// 审核同意订单售后服务
        /// </summary>
        /// <param name="asId">售后服务id</param>
        /// <param name="checkResult">审核结果</param>
        /// <param name="checkTime">审核时间</param>
        public static void CheckAgreeOrderAfterService(int asId, string checkResult, DateTime checkTime)
        {
            CheckOrderAfterService(asId, OrderAfterServiceState.CheckAgree, checkResult, checkTime);
        }

        /// <summary>
        /// 审核拒绝订单售后服务
        /// </summary>
        /// <param name="asId">售后服务id</param>
        /// <param name="checkResult">审核结果</param>
        /// <param name="checkTime">审核时间</param>
        public static void CheckRefuseOrderAfterService(int asId, string checkResult, DateTime checkTime)
        {
            CheckOrderAfterService(asId, OrderAfterServiceState.CheckRefuse, checkResult, checkTime);
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
            BrnShop.Data.OrderAfterServices.CheckOrderAfterService(asId, state, checkResult, checkTime);
        }

        /// <summary>
        /// 商城收到客户邮寄的商品
        /// </summary>
        /// <param name="asId">售后服务id</param>
        /// <param name="receiveTime">收货时间</param>
        public static void ReceiveOrderAfterService(int asId, DateTime receiveTime)
        {
            BrnShop.Data.OrderAfterServices.ReceiveOrderAfterService(asId, OrderAfterServiceState.Received, receiveTime);
        }

        /// <summary>
        /// 邮寄给客户
        /// </summary>
        /// <param name="asId">售后服务id</param>
        /// <param name="shipCoId">配送公司id</param>
        /// <param name="shipCoName">配送公司名称</param>
        /// <param name="shipSN">配送单号</param>
        public static void BackOrderAfterService(int asId, int shipCoId, string shipCoName, string shipSN)
        {
            BrnShop.Data.OrderAfterServices.BackOrderAfterService(asId, OrderAfterServiceState.Backed, shipCoId, shipCoName, shipSN, DateTime.Now);
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
            return BrnShop.Data.OrderAfterServices.GetOrderProductAfterServiceList(pageSize, pageNumber, condition);
        }

        /// <summary>
        /// 获得订单商品售后服务数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static int GetOrderProductAfterServiceCount(string condition)
        {
            return BrnShop.Data.OrderAfterServices.GetOrderProductAfterServiceCount(condition);
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
            return BrnShop.Data.OrderAfterServices.GetOrderProductAfterServiceListCondition(uid, oid, state, type, applyStartTime, applyEndTime);
        }
    }
}
