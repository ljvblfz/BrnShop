using System;
using System.Data;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BrnMall.Core;
using BrnMall.Services;
using BrnMall.Web.Framework;

namespace BrnMall.Web.StoreAdmin.Models
{
    /// <summary>
    /// 订单列表模型类
    /// </summary>
    public class OrderListModel
    {
        /// <summary>
        /// 分页对象
        /// </summary>
        public PageModel PageModel { get; set; }
        /// <summary>
        /// 订单列表
        /// </summary>
        public DataTable OrderList { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OSN { get; set; }
        /// <summary>
        /// 账户名
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        public string Consignee { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public int OrderState { get; set; }
        /// <summary>
        /// 订单状态列表
        /// </summary>
        public List<SelectListItem> OrderStateList { get; set; }
    }

    /// <summary>
    /// 订单信息模型类
    /// </summary>
    public class OrderInfoModel
    {
        /// <summary>
        /// 订单信息
        /// </summary>
        public OrderInfo OrderInfo { get; set; }
        /// <summary>
        /// 区域信息
        /// </summary>
        public RegionInfo RegionInfo { get; set; }
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfo UserInfo { get; set; }
        /// <summary>
        /// 用户等级
        /// </summary>
        public UserRankInfo UserRankInfo { get; set; }
        /// <summary>
        /// 订单商品列表
        /// </summary>
        public List<OrderProductInfo> OrderProductList { get; set; }
        /// <summary>
        /// 订单处理列表
        /// </summary>
        public List<OrderActionInfo> OrderActionList { get; set; }
    }

    /// <summary>
    /// 更新订单配送费用模型类
    /// </summary>
    public class UpdateOrderShipFeeModel
    {
        /// <summary>
        /// 配送费用
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "配送费用不能小于0")]
        [Required(ErrorMessage = "配送费用不能为空")]
        public decimal ShipFee { get; set; }
    }

    /// <summary>
    /// 更新订单折扣模型类
    /// </summary>
    public class UpdateOrderDiscountModel
    {
        /// <summary>
        /// 订单折扣
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "订单折扣不能小于0")]
        [Required(ErrorMessage = "订单折扣不能为空")]
        public decimal Discount { get; set; }
    }

    /// <summary>
    /// 订单发货模型类
    /// </summary>
    public class SendOrderProductModel
    {
        /// <summary>
        /// 配送公司id
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "请选择配送公司")]
        public int ShipCoId { get; set; }

        /// <summary>
        /// 发货单号
        /// </summary>
        [Required(ErrorMessage = "发货单号不能为空")]
        [StringLength(30, ErrorMessage = "发货单号长度不能超过30")]
        public string ShipSN { get; set; }
    }

    /// <summary>
    /// 打印订单模型类
    /// </summary>
    public class PrintOrderModel
    {
        /// <summary>
        /// 订单信息
        /// </summary>
        public OrderInfo OrderInfo { get; set; }
        /// <summary>
        /// 区域信息
        /// </summary>
        public RegionInfo RegionInfo { get; set; }
        /// <summary>
        /// 订单商品列表
        /// </summary>
        public List<OrderProductInfo> OrderProductList { get; set; }
    }

    /// <summary>
    /// 订单退款列表模型类
    /// </summary>
    public class OrderRefundListModel
    {
        /// <summary>
        /// 分页对象
        /// </summary>
        public PageModel PageModel { get; set; }
        /// <summary>
        /// 订单退款列表
        /// </summary>
        public List<OrderRefundInfo> OrderRefundList { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OSN { get; set; }
    }

    /// <summary>
    /// 订单售后服务列表模型类
    /// </summary>
    public class OrderAfterServiceListModel
    {
        /// <summary>
        /// 分页对象
        /// </summary>
        public PageModel PageModel { get; set; }
        /// <summary>
        /// 订单售后服务列表
        /// </summary>
        public DataTable OrderAfterServiceList { get; set; }
        /// <summary>
        /// 订单id
        /// </summary>
        public int Oid { get; set; }
        /// <summary>
        /// 账户名称
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 状态列表
        /// </summary>
        public List<SelectListItem> StateList { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 类型列表
        /// </summary>
        public List<SelectListItem> TypeList { get; set; }
        /// <summary>
        /// 申请开始时间
        /// </summary>
        public string ApplyStartTime { get; set; }
        /// <summary>
        /// 申请结束时间
        /// </summary>
        public string ApplyEndTime { get; set; }
    }

    /// <summary>
    /// 订单售后服务模型类
    /// </summary>
    public class OrderAfterServiceModel
    {
        /// <summary>
        /// 订单售后服务信息
        /// </summary>
        public OrderAfterServiceInfo OrderAfterServiceInfo { get; set; }
        /// <summary>
        /// 区域信息
        /// </summary>
        public RegionInfo RegionInfo { get; set; }
    }

    /// <summary>
    /// 审核订单售后服务模型类
    /// </summary>
    public class CheckOrderAfterServiceModel
    {
        public CheckOrderAfterServiceModel()
        {
            State = 0;
        }

        /// <summary>
        /// 状态
        /// </summary>
        [Range(0, 1, ErrorMessage = "请选择正确的状态")]
        public int State { get; set; }
        /// <summary>
        /// 审核结果
        /// </summary>
        [StringLength(150, ErrorMessage = "最多输入150个字")]
        public string CheckResult { get; set; }
    }

    /// <summary>
    /// 邮寄给客户模型类
    /// </summary>
    public class BackOrderAfterServiceModel
    {
        /// <summary>
        /// 配送公司id
        /// </summary>
        public int ShipCoId { get; set; }
        /// <summary>
        /// 配送单号
        /// </summary>
        public string ShipSN { get; set; }
    }
}
