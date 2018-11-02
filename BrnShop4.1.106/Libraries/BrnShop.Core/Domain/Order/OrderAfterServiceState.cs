using System;

namespace BrnShop.Core
{
    /// <summary>
    /// 订单售后服务状态
    /// </summary>
    public enum OrderAfterServiceState
    {
        /// <summary>
        /// 审核中
        /// </summary>
        Checking = 10,
        /// <summary>
        /// 审核同意
        /// </summary>
        CheckAgree = 20,
        /// <summary>
        /// 审核拒绝
        /// </summary>
        CheckRefuse = 30,
        /// <summary>
        /// 客户已邮寄
        /// </summary>
        Sended = 40,
        /// <summary>
        /// 商城已收货
        /// </summary>
        Received = 50,
        /// <summary>
        /// 商城已发货
        /// </summary>
        Backed = 60,
        /// <summary>
        /// 售后完成
        /// </summary>
        Completed = 70
    }
}
