using System;
using System.Web.Mvc;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BrnMall.Core;
using BrnMall.Services;
using BrnMall.Web.Framework;

namespace BrnMall.Web.MallAdmin.Models
{
    /// <summary>
    /// 店铺行业列表模型类
    /// </summary>
    public class StoreIndustryListModel
    {
        /// <summary>
        /// 店铺行业列表
        /// </summary>
        public List<StoreIndustryInfo> StoreIndustryList { get; set; }
    }

    /// <summary>
    /// 店铺行业模型类
    /// </summary>
    public class StoreIndustryModel
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Required(ErrorMessage = "标题不能为空")]
        [StringLength(75, ErrorMessage = "标题长度不能大于75")]
        public string IndustryTitle { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Required(ErrorMessage = "排序不能为空")]
        [DisplayName("排序")]
        public int DisplayOrder { get; set; }
    }
}
