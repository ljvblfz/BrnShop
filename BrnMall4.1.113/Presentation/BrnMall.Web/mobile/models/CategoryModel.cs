using System;
using System.Collections.Generic;

using BrnMall.Core;
using BrnMall.Services;
using BrnMall.Web.Framework;

namespace BrnMall.Web.Mobile.Models
{
    /// <summary>
    /// 分类列表模型类
    /// </summary>
    public class CategoryListModel
    {
        public CategoryInfo CategoryInfo { get; set; }
        public List<CategoryInfo> CategoryList { get; set; }
    }
}