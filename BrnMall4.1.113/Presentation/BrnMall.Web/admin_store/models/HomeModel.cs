using System;

namespace BrnMall.Web.StoreAdmin.Models
{
    /// <summary>
    /// 店铺运行信息模型类
    /// </summary>
    public class StoreRunInfoModel
    {
        /// <summary>
        /// 待确认订单数
        /// </summary>
        public int WaitConfirmCount { get; set; }
        /// <summary>
        /// 待备货订单数
        /// </summary>
        public int WaitPreProductCount { get; set; }
        /// <summary>
        /// 待发货订单数
        /// </summary>
        public int WaitSendCount { get; set; }
        /// <summary>
        /// 待支付订单数
        /// </summary>
        public int WaitPayCount { get; set; }
    }
}
