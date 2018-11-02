using System;
using System.ComponentModel.DataAnnotations;

using BrnMall.Core;
using BrnMall.Services;

namespace BrnMall.Web.MallAdmin.Models
{
    /// <summary>
    /// 商城管理员组列表模型类
    /// </summary>
    public class MallAdminGroupListModel
    {
        /// <summary>
        /// 商城管理员组列表
        /// </summary>
        public MallAdminGroupInfo[] MallAdminGroupList { get; set; }
    }

    /// <summary>
    /// 商城管理员组模型类
    /// </summary>
    public class MallAdminGroupModel
    {
        /// <summary>
        /// 管理员组标题
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(25, ErrorMessage = "名称长度不能大于25")]
        public string AdminGroupTitle { get; set; }

        /// <summary>
        /// 动作列表
        /// </summary>
        public string[] ActionList { get; set; }
    }
}
