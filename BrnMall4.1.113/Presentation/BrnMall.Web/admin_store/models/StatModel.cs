using System;
using System.Data;
using System.Web.Mvc;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BrnMall.Core;
using BrnMall.Services;
using BrnMall.Web.Framework;

namespace BrnMall.Web.StoreAdmin.Models
{
    /// <summary>
    /// 商品统计模型类
    /// </summary>
    public class ProductStatModel
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
        /// <summary>
        /// 店铺分类id
        /// </summary>
        public int StoreCid { get; set; }
        /// <summary>
        /// 店铺分类列表
        /// </summary>
        public List<SelectListItem> StoreClassList { get; set; }
        /// <summary>
        /// 分类id
        /// </summary>
        public int CateId { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 品牌id
        /// </summary>
        public int BrandId { get; set; }
        /// <summary>
        /// 品牌名称
        /// </summary>
        public string BrandName { get; set; }
    }

    /// <summary>
    /// 销售商品列表模型类
    /// </summary>
    public class SaleProductListModel
    {
        /// <summary>
        /// 分页对象
        /// </summary>
        public PageModel PageModel { get; set; }
        /// <summary>
        /// 销售商品列表
        /// </summary>
        public DataTable SaleProductList { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }
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
    /// 订单统计模型类
    /// </summary>
    public class OrderStatModel
    {
        /// <summary>
        /// 统计类型(0代表订单数，1代表订单合计)
        /// </summary>
        public int StatType { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 统计项列表
        /// </summary>
        public DataTable StatItemList { get; set; }
    }
}
