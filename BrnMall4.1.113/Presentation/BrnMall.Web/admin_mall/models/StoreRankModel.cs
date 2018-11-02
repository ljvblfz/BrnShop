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
    /// 店铺等级列表模型类
    /// </summary>
    public class StoreRankListModel
    {
        /// <summary>
        /// 店铺等级列表
        /// </summary>
        public List<StoreRankInfo> StoreRankList { get; set; }
    }

    /// <summary>
    /// 店铺等级模型类
    /// </summary>
    public class StoreRankModel : IValidatableObject
    {
        /// <summary>
        /// 店铺等级标题
        /// </summary>
        [Required(ErrorMessage = "标题不能为空")]
        [StringLength(25, ErrorMessage = "标题长度不能大于25")]
        public string RankTitle { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [StringLength(50, ErrorMessage = "头像文件名长度不能大于50")]
        public string Avatar { get; set; }

        /// <summary>
        /// 诚信上限
        /// </summary>
        [Range(-1, int.MaxValue, ErrorMessage = "诚信上限不能小于-1")]
        [DisplayName("诚信上限")]
        public int HonestiesUpper { get; set; }

        /// <summary>
        /// 诚信下限
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "诚信下限不能为负数")]
        [DisplayName("诚信下限")]
        public int HonestiesLower { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        [Required(ErrorMessage = "商品数量不能为空")]
        [Range(0, int.MaxValue, ErrorMessage = "商品数量不能为负数")]
        [DisplayName("商品数量")]
        public int ProductCount { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errorList = new List<ValidationResult>();

            if (HonestiesUpper > 0 && HonestiesUpper <= HonestiesLower)
                errorList.Add(new ValidationResult("诚信上限必须大于诚信下限!", new string[] { "HonestiesUpper" }));

            return errorList;
        }
    }
}
