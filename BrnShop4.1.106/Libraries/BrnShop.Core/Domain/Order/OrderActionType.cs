using System;

namespace BrnShop.Core
{
    /// <summary>
    /// 订单处理类型
    /// </summary>
    public enum OrderActionType
    {
        /// <summary>
        /// 提交
        /// </summary>
        Submit = 20,
        /// <summary>
        /// 支付
        /// </summary>
        Pay = 40,
        /// <summary>
        /// 确认
        /// </summary>
        Confirm = 60,
        /// <summary>
        /// 备货
        /// </summary>
        PreProduct = 80,
        /// <summary>
        /// 发货
        /// </summary>
        Send = 100,
        /// <summary>
        /// 收货
        /// </summary>
        Receive = 120,
        /// <summary>
        /// 锁定
        /// </summary>
        Lock = 140,
        /// <summary>
        /// 取消
        /// </summary>
        Cancel = 160,
        /// <summary>
        /// 更新配送费用
        /// </summary>
        UpdateShipFee = 180,
        /// <summary>
        /// 更新折扣
        /// </summary>
        UpdateDiscount = 200
    }
}
