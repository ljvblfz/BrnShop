using System;
using System.Collections.Generic;

using BrnMall.Core;
using BrnMall.Services;
using BrnMall.Web.Framework;

namespace BrnMall.Web.Models
{
    /// <summary>
    /// 店铺分类模型类
    /// </summary>
    public class StoreClassModel
    {
        /// <summary>
        /// 店铺分类id
        /// </summary>
        public int StoreCid { get; set; }
        /// <summary>
        /// 排序列
        /// </summary>
        public int SortColumn { get; set; }
        /// <summary>
        /// 排序方向
        /// </summary>
        public int SortDirection { get; set; }
        /// <summary>
        /// 分页对象
        /// </summary>
        public PageModel PageModel { get; set; }
        /// <summary>
        /// 商品列表
        /// </summary>
        public List<PartProductInfo> ProductList { get; set; }
        /// <summary>
        /// 店铺分类信息
        /// </summary>
        public StoreClassInfo StoreClassInfo { get; set; }
    }

    /// <summary>
    /// 店铺搜索模型类
    /// </summary>
    public class StoreSearchModel
    {
        /// <summary>
        /// 搜索词
        /// </summary>
        public string Word { get; set; }
        /// <summary>
        /// 店铺分类id
        /// </summary>
        public int StoreCid { get; set; }
        /// <summary>
        /// 开始价格
        /// </summary>
        public int StartPrice { get; set; }
        /// <summary>
        /// 结束价格
        /// </summary>
        public int EndPrice { get; set; }
        /// <summary>
        /// 排序列
        /// </summary>
        public int SortColumn { get; set; }
        /// <summary>
        /// 排序方向
        /// </summary>
        public int SortDirection { get; set; }
        /// <summary>
        /// 分页对象
        /// </summary>
        public PageModel PageModel { get; set; }
        /// <summary>
        /// 商品列表
        /// </summary>
        public List<PartProductInfo> ProductList { get; set; }
        /// <summary>
        /// 店铺分类信息
        /// </summary>
        public StoreClassInfo StoreClassInfo { get; set; }
    }
}