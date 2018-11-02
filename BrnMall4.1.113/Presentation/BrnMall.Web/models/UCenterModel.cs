using System;
using System.Data;
using System.Collections.Generic;

using BrnMall.Core;
using BrnMall.Services;
using BrnMall.Web.Framework;

namespace BrnMall.Web.Models
{
    /// <summary>
    /// 用户信息模型类
    /// </summary>
    public class UserInfoModel
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfo UserInfo { get; set; }
        /// <summary>
        /// 用户等级信息
        /// </summary>
        public UserRankInfo UserRankInfo { get; set; }
    }

    /// <summary>
    /// 安全验证模型类
    /// </summary>
    public class SafeVerifyModel
    {
        /// <summary>
        /// 动作
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// 验证方式
        /// </summary>
        public string Mode { get; set; }
    }

    /// <summary>
    /// 安全更新模型类
    /// </summary>
    public class SafeUpdateModel
    {
        /// <summary>
        /// 动作
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// V
        /// </summary>
        public string V { get; set; }
    }

    /// <summary>
    /// 安全成功模型类
    /// </summary>
    public class SafeSuccessModel
    {
        /// <summary>
        /// 动作
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

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
        /// 订单商品列表
        /// </summary>
        public List<OrderProductInfo> OrderProductList { get; set; }
        /// <summary>
        /// 开始添加时间
        /// </summary>
        public string StartAddTime { get; set; }
        /// <summary>
        /// 结束添加时间
        /// </summary>
        public string EndAddTime { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public int OrderState { get; set; }
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
        /// 订单商品列表
        /// </summary>
        public List<OrderProductInfo> OrderProductList { get; set; }
        /// <summary>
        /// 订单处理列表
        /// </summary>
        public List<OrderActionInfo> OrderActionList { get; set; }
    }

    /// <summary>
    /// 收藏夹商品列表模型类
    /// </summary>
    public class FavoriteProductListModel
    {
        /// <summary>
        /// 分页对象
        /// </summary>
        public PageModel PageModel { get; set; }
        /// <summary>
        /// 商品列表
        /// </summary>
        public DataTable ProductList { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
    }

    /// <summary>
    /// 收藏夹店铺列表模型类
    /// </summary>
    public class FavoriteStoreListModel
    {
        /// <summary>
        /// 分页对象
        /// </summary>
        public PageModel PageModel { get; set; }
        /// <summary>
        /// 店铺列表
        /// </summary>
        public DataTable StoreList { get; set; }
    }

    /// <summary>
    /// 配送地址列表模型类
    /// </summary>
    public class ShipAddressListModel
    {
        /// <summary>
        /// 配送地址列表
        /// </summary>
        public List<FullShipAddressInfo> ShipAddressList { get; set; }
        /// <summary>
        /// 配送地址数量
        /// </summary>
        public int ShipAddressCount { get; set; }
    }

    /// <summary>
    /// 支付积分日志列表模型类
    /// </summary>
    public class PayCreditLogListModel
    {
        /// <summary>
        /// 分页对象
        /// </summary>
        public PageModel PageModel { get; set; }
        /// <summary>
        /// 列表类型
        /// </summary>
        public int ListType { get; set; }
        /// <summary>
        /// 支付积分日志列表
        /// </summary>
        public List<CreditLogInfo> PayCreditLogList { get; set; }
    }

    /// <summary>
    /// 优惠劵列表模型类
    /// </summary>
    public class CouponListModel
    {
        /// <summary>
        /// 列表类型
        /// </summary>
        public int ListType { get; set; }
        /// <summary>
        /// 优惠劵列表
        /// </summary>
        public DataTable CouponList { get; set; }
    }

    /// <summary>
    /// 用户商品咨询列表模型类
    /// </summary>
    public class UserProductConsultListModel
    {
        /// <summary>
        /// 分页对象
        /// </summary>
        public PageModel PageModel { get; set; }
        /// <summary>
        /// 商品咨询列表
        /// </summary>
        public List<ProductConsultInfo> ProductConsultList { get; set; }
    }

    /// <summary>
    /// 评价订单模型类
    /// </summary>
    public class ReviewOrderModel
    {
        /// <summary>
        /// 订单信息
        /// </summary>
        public OrderInfo OrderInfo { get; set; }
        /// <summary>
        /// 订单商品列表
        /// </summary>
        public List<OrderProductInfo> OrderProductList { get; set; }
        /// <summary>
        /// 店铺评价信息
        /// </summary>
        public StoreReviewInfo StoreReviewInfo { get; set; }
    }

    /// <summary>
    /// 用户商品评价列表模型类
    /// </summary>
    public class UserProductReviewListModel
    {
        /// <summary>
        /// 分页对象
        /// </summary>
        public PageModel PageModel { get; set; }
        /// <summary>
        /// 商品评价列表
        /// </summary>
        public List<ProductReviewInfo> ProductReviewList { get; set; }
    }

    /// <summary>
    /// 订单售后服务模型类
    /// </summary>
    public class OrderAfterServiceModel
    {
        /// <summary>
        /// 订单信息
        /// </summary>
        public OrderInfo OrderInfo { get; set; }
        /// <summary>
        /// 订单商品列表
        /// </summary>
        public List<OrderProductInfo> OrderProductList { get; set; }
        /// <summary>
        /// 订单售后服务列表
        /// </summary>
        public List<OrderAfterServiceInfo> OrderAfterServiceList { get; set; }
    }

    /// <summary>
    /// 申请订单售后服务模型类
    /// </summary>
    public class ApplyOrderAfterServiceModel
    {
        /// <summary>
        /// 订单id
        /// </summary>
        public int Oid { get; set; }
        /// <summary>
        /// 记录id
        /// </summary>
        public int RecordId { get; set; }
        /// <summary>
        /// 订单商品信息
        /// </summary>
        public OrderProductInfo OrderProductInfo { get; set; }
    }

    /// <summary>
    /// 邮寄给商城模型类
    /// </summary>
    public class SendOrderAfterServiceModel
    {
        /// <summary>
        /// 订单售后服务模型类
        /// </summary>
        public OrderAfterServiceInfo OrderAfterServiceInfo { get; set; }
        /// <summary>
        /// 配送公司列表
        /// </summary>
        public List<ShipCompanyInfo> ShipCompanyList { get; set; }
    }

    /// <summary>
    /// 订单商品售后服务列表模型类
    /// </summary>
    public class OrderProductAfterServiceListModel
    {
        /// <summary>
        /// 订单售后服务列表
        /// </summary>
        public List<OrderAfterServiceInfo> OrderAfterServiceList { get; set; }
    }
}