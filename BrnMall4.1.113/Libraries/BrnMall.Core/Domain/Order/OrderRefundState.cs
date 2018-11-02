using System;

namespace BrnMall.Core
{
    /// <summary>
    /// 订单退款状态
    /// </summary>
    public enum OrderRefundState
    {
        /// <summary>
        /// 已申请
        /// </summary>
        Applied = 10,
        /// <summary>
        /// 已转账
        /// </summary>
        Sended = 20,
        /// <summary>
        /// 已到账
        /// </summary>
        Reached = 30
    }
}
