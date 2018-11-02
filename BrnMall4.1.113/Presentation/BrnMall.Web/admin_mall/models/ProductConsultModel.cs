using System;
using System.Data;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using BrnMall.Core;
using BrnMall.Services;
using BrnMall.Web.Framework;

namespace BrnMall.Web.MallAdmin.Models
{
    /// <summary>
    /// 商品咨询类型列表模型类
    /// </summary>
    public class ProductConsultTypeListModel
    {
        /// <summary>
        /// 商品咨询类型列表
        /// </summary>
        public ProductConsultTypeInfo[] ProductConsultTypeList { get; set; }
    }

    /// <summary>
    /// 商品咨询类型模型类
    /// </summary>
    public class ProductConsultTypeModel
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Required(ErrorMessage = "标题不能为空")]
        [StringLength(15, ErrorMessage = "最多输入15个字")]
        public string Title { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [DisplayName("排序")]
        public int DisplayOrder { get; set; }
    }

    /// <summary>
    /// 商品咨询列表模型类
    /// </summary>
    public class ProductConsultListModel
    {
        /// <summary>
        /// 分页对象
        /// </summary>
        public PageModel PageModel { get; set; }
        /// <summary>
        /// 商品咨询列表
        /// </summary>
        public DataTable ProductConsultList { get; set; }
        /// <summary>
        /// 店铺id
        /// </summary>
        public int StoreId { get; set; }
        /// <summary>
        /// 店铺名称
        /// </summary>
        public string StoreName { get; set; }
        /// <summary>
        /// 商品id
        /// </summary>
        public int Pid { get; set; }
        /// <summary>
        /// 咨询信息
        /// </summary>
        public string ConsultMessage { get; set; }
        /// <summary>
        /// 咨询开始时间
        /// </summary>
        public string ConsultStartTime { get; set; }
        /// <summary>
        /// 咨询结束时间
        /// </summary>
        public string ConsultEndTime { get; set; }
    }

    /// <summary>
    /// 回复商品咨询模型类
    /// </summary>
    public class ReplyProductConsultModel
    {
        /// <summary>
        /// 回复内容
        /// </summary>
        [Required(ErrorMessage = "回复内容不能为空")]
        [StringLength(100, ErrorMessage = "最多只能输入100个字")]
        public string ReplyMessage { get; set; }
    }
}
