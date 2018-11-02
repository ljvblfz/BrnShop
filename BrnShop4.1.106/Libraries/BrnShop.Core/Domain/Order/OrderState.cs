using System;

namespace BrnShop.Core
{
    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderState
    {
        /// <summary>
        /// 等待付款
        /// </summary>
        WaitPaying = 20,
        /// <summary>
        /// 确认中
        /// </summary>
        Confirming = 40,
        /// <summary>
        /// 已确认
        /// </summary>
        Confirmed = 60,
        /// <summary>
        /// 备货中
        /// </summary>
        PreProducting = 80,
        /// <summary>
        /// 已发货
        /// </summary>
        Sended = 100,
        /// <summary>
        /// 已收货
        /// </summary>
        Received = 120,
        /// <summary>
        /// 锁定
        /// </summary>
        Locked = 140,
        /// <summary>
        /// 取消
        /// </summary>
        Cancelled = 160
    }
}
