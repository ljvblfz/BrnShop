using System;
using System.Data;

namespace BrnShop.Core
{
    /// <summary>
    /// BrnShop关系数据库策略之订单分部接口
    /// </summary>
    public partial interface IRDBSStrategy
    {
        #region 订单

        /// <summary>
        /// 获得订单信息
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <returns>订单信息</returns>
        IDataReader GetOrderByOid(int oid);

        /// <summary>
        /// 获得订单信息
        /// </summary>
        /// <param name="osn">订单编号</param>
        /// <returns>订单信息</returns>
        IDataReader GetOrderByOSN(string osn);

        /// <summary>
        /// 根据下单时间获得订单数量
        /// </summary>
        /// <param name="orderState">订单状态</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        int GetOrderCountByOrderStateAndAddTime(int orderState, string startTime, string endTime);

        /// <summary>
        /// 获得订单列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        DataTable GetOrderList(int pageSize, int pageNumber, string condition);

        /// <summary>
        /// 获得列表搜索条件
        /// </summary>
        /// <param name="osn">订单编号</param>
        /// <param name="uid">用户id</param>
        /// <param name="consignee">收货人</param>
        /// <param name="orderState">订单状态</param>
        /// <returns></returns>
        string GetOrderListCondition(string osn, int uid, string consignee, int orderState);

        /// <summary>
        /// 获得订单数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        int GetOrderCount(string condition);

        /// <summary>
        /// 获得订单商品列表
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <returns></returns>
        IDataReader GetOrderProductList(int oid);

        /// <summary>
        /// 获得订单商品列表
        /// </summary>
        /// <param name="oidList">订单id列表</param>
        /// <returns></returns>
        IDataReader GetOrderProductList(string oidList);

        /// <summary>
        /// 更新订单折扣
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <param name="discount">折扣</param>
        /// <param name="orderAmount">订单合计</param>
        /// <param name="surplusMoney">剩余金额</param>
        void UpdateOrderDiscount(int oid, decimal discount, decimal orderAmount, decimal surplusMoney);

        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <param name="orderState">订单状态</param>
        void UpdateOrderState(int oid, OrderState orderState);

        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <param name="orderState">订单状态</param>
        /// <param name="shipSN">配送单号</param>
        /// <param name="shipTime">配送时间</param>
        void SendOrderProduct(int oid, OrderState orderState, string shipSN, DateTime shipTime);

        /// <summary>
        /// 付款
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <param name="orderState">订单状态</param>
        /// <param name="paySN">支付单号</param>
        /// <param name="paySystemName">支付系统名称</param>
        /// <param name="payFriendName">支付友好名称</param>
        /// <param name="payTime">支付时间</param>
        void PayOrder(int oid, OrderState orderState, string paySN, string paySystemName, string payFriendName, DateTime payTime);

        /// <summary>
        /// 更新订单的评价
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <param name="isReview">是否评价</param>
        void UpdateOrderIsReview(int oid, int isReview);

        /// <summary>
        /// 清空过期的在线支付订单
        /// </summary>
        /// <param name="expireTime">过期时间</param>
        void ClearExpiredOnlinePayOrder(DateTime expireTime);

        /// <summary>
        /// 自动收货
        /// </summary>
        /// <param name="expireTime">过期时间</param>
        void AutoReceiveOrder(DateTime expireTime);

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
        DataTable GetUserOrderList(int uid, int pageSize, int pageNumber, string startAddTime, string endAddTime, int orderState);

        /// <summary>
        /// 获得用户订单列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="startAddTime">添加开始时间</param>
        /// <param name="endAddTime">添加结束时间</param>
        /// <param name="orderState">订单状态(0代表全部状态)</param>
        /// <returns></returns>
        int GetUserOrderCount(int uid, string startAddTime, string endAddTime, int orderState);

        /// <summary>
        /// 获得销售商品列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="orderState">订单状态</param>
        /// <returns></returns>
        DataTable GetSaleProductList(int pageSize, int pageNumber, string startTime, string endTime, int orderState);

        /// <summary>
        /// 获得销售商品数量
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="orderState">订单状态</param>
        /// <returns></returns>
        int GetSaleProductCount(string startTime, string endTime, int orderState);

        /// <summary>
        /// 获得订单统计
        /// </summary>
        /// <param name="statType">统计类型(0代表订单数，1代表订单合计)</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        DataTable GetOrderStat(int statType, string startTime, string endTime);

        #endregion

        #region 订单处理

        /// <summary>
        /// 创建订单处理
        /// </summary>
        /// <param name="orderActionInfo">订单处理信息</param>
        void CreateOrderAction(OrderActionInfo orderActionInfo);

        /// <summary>
        /// 获得订单处理列表
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <returns></returns>
        IDataReader GetOrderActionList(int oid);

        /// <summary>
        /// 获得订单id列表
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="orderActionType">订单操作类型</param>
        /// <returns></returns>
        DataTable GetOrderIdList(DateTime startTime, DateTime endTime, int orderActionType);

        #endregion

        #region 订单售后服务

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
        void ApplyOrderAfterService(OrderAfterServiceState state, int uid, int oid, int recordId, int pid, int cateId, int brandId, string pname, string pshowImg, int count, decimal money, int type, string applyReason, DateTime applyTime);

        /// <summary>
        /// 审核订单售后服务
        /// </summary>
        /// <param name="asId">售后服务id</param>
        /// <param name="state">状态</param>
        /// <param name="checkResult">审核结果</param>
        /// <param name="checkTime">审核时间</param>
        void CheckOrderAfterService(int asId, OrderAfterServiceState state, string checkResult, DateTime checkTime);

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
        void SendOrderAfterService(int asId, OrderAfterServiceState state, int shipCoId, string shipCoName, string shipSN, int regionId, string consignee, string mobile, string phone, string email, string zipCode, string address, DateTime sendTime);

        /// <summary>
        /// 商城收到客户邮寄的商品
        /// </summary>
        /// <param name="asId">售后服务id</param>
        /// <param name="state">状态</param>
        /// <param name="receiveTime">收货时间</param>
        void ReceiveOrderAfterService(int asId, OrderAfterServiceState state, DateTime receiveTime);

        /// <summary>
        /// 邮寄给客户
        /// </summary>
        /// <param name="asId">售后服务id</param>
        /// <param name="state">状态</param>
        /// <param name="shipCoId">配送公司id</param>
        /// <param name="shipCoName">配送公司名称</param>
        /// <param name="shipSN">配送单号</param>
        /// <param name="backTime">邮寄给客户时间</param>
        void BackOrderAfterService(int asId, OrderAfterServiceState state, int shipCoId, string shipCoName, string shipSN, DateTime backTime);

        /// <summary>
        /// 订单售后服务完成
        /// </summary>
        /// <param name="asId">售后服务id</param>
        /// <param name="state">状态</param>
        void CompleteOrderAfterService(int asId, OrderAfterServiceState state);

        /// <summary>
        /// 获得订单售后服务列表
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <returns></returns>
        IDataReader GetOrderAfterServiceList(int oid);

        /// <summary>
        /// 获得订单商品售后服务列表
        /// </summary>
        /// <param name="recordId">记录id</param>
        /// <returns></returns>
        IDataReader GetOrderProductAfterServiceList(int recordId);

        /// <summary>
        /// 获得订单商品售后服务列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        DataTable GetOrderProductAfterServiceList(int pageSize, int pageNumber, string condition);

        /// <summary>
        /// 获得订单商品售后服务数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        int GetOrderProductAfterServiceCount(string condition);

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
        string GetOrderProductAfterServiceListCondition(int uid, int oid, int state, int type, string applyStartTime, string applyEndTime);

        /// <summary>
        /// 获得订单售后服务
        /// </summary>
        /// <param name="asId">售后服务id</param>
        /// <returns></returns>
        IDataReader GetOrderAfterServiceByASId(int asId);

        #endregion

        #region 订单退款

        /// <summary>
        /// 申请退款
        /// </summary>
        /// <param name="orderRefundInfo">订单退款信息</param>
        void ApplyRefund(OrderRefundInfo orderRefundInfo);

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="refundId">退款id</param>
        /// <param name="state">状态</param>
        /// <param name="refundSN">退款单号</param>
        /// <param name="refundTime">退款时间</param>
        void RefundOrder(int refundId, OrderRefundState state, string refundSN, DateTime refundTime);

        /// <summary>
        /// 获得订单退款列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        IDataReader GetOrderRefundList(int pageSize, int pageNumber, string condition);

        /// <summary>
        /// 获得列表搜索条件
        /// </summary>
        /// <param name="osn">订单编号</param>
        /// <returns></returns>
        string GetOrderRefundListCondition(string osn);

        /// <summary>
        /// 获得订单退款数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        int GetOrderRefundCount(string condition);

        #endregion
    }
}
