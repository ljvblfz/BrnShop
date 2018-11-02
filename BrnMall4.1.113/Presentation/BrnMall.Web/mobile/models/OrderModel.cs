using System;
using System.Collections.Generic;

using BrnMall.Core;

namespace BrnMall.Web.Mobile.Models
{
    /// <summary>
    /// 确认订单模型类
    /// </summary>
    public class ConfirmOrderModel
    {
        /// <summary>
        /// 选中的购物车项键列表
        /// </summary>
        public string SelectedCartItemKeyList { get; set; }

        /// <summary>
        /// 支付方式(0代表货到付款,1代表在线支付)
        /// </summary>
        public int PayMode { get; set; }

        /// <summary>
        /// 默认完整用户配送地址
        /// </summary>
        public FullShipAddressInfo DefaultFullShipAddressInfo { get; set; }

        /// <summary>
        /// 支付积分名称
        /// </summary>
        public string PayCreditName { get; set; }
        /// <summary>
        /// 用户支付积分
        /// </summary>
        public int UserPayCredits { get; set; }
        /// <summary>
        /// 最大使用支付积分
        /// </summary>
        public int MaxUsePayCredits { get; set; }

        /// <summary>
        /// 全部配送费用
        /// </summary>
        public int AllShipFee { get; set; }
        /// <summary>
        /// 全部满减
        /// </summary>
        public int AllFullCut { get; set; }
        /// <summary>
        /// 全部商品合计
        /// </summary>
        public decimal AllProductAmount { get; set; }
        /// <summary>
        /// 全部订单合计
        /// </summary>
        public decimal AllOrderAmount { get; set; }

        /// <summary>
        /// 全部商品总数量
        /// </summary>
        public int AllTotalCount { get; set; }
        /// <summary>
        /// 全部商品总重量
        /// </summary>
        public int AllTotalWeight { get; set; }
    }

    /// <summary>
    /// 提交结果模型类
    /// </summary>
    public class SubmitResultModel
    {
        /// <summary>
        /// 订单id列表
        /// </summary>
        public string OidList { get; set; }
        /// <summary>
        /// 订单列表
        /// </summary>
        public List<OrderInfo> OrderList { get; set; }
        /// <summary>
        /// 在线支付订单id列表
        /// </summary>
        public string OnlinePayOidList { get; set; }
        /// <summary>
        /// 所有订单需要支付的金额
        /// </summary>
        public decimal AllPayMoney { get; set; }
    }

    /// <summary>
    /// 支付展示模型类
    /// </summary>
    public class PayShowModel
    {
        /// <summary>
        /// 订单id列表
        /// </summary>
        public string OidList { get; set; }
        /// <summary>
        /// 订单列表
        /// </summary>
        public List<OrderInfo> OrderList { get; set; }
        /// <summary>
        /// 所有订单需要支付的金额
        /// </summary>
        public decimal AllPayMoney { get; set; }
        /// <summary>
        /// 支付插件列表
        /// </summary>
        public List<PluginInfo> PayPluginList { get; set; }
    }

    /// <summary>
    /// 支付结果模型类
    /// </summary>
    public class PayResultModel
    {
        /// <summary>
        /// 状态
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 支付成功订单列表
        /// </summary>
        public List<OrderInfo> SuccessOrderList { get; set; }
        /// <summary>
        /// 支付失败订单列表
        /// </summary>
        public List<OrderInfo> FailOrderList { get; set; }
    }
}