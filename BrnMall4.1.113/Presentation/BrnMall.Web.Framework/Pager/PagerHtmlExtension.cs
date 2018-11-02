using System;
using System.Web.Mvc;

namespace BrnMall.Web.Framework
{
    /// <summary>
    /// 分页Html扩展
    /// </summary>
    public static class PagerHtmlExtension
    {
        /// <summary>
        /// 商城前台分页
        /// </summary>
        /// <param name="helper">HtmlHelper</param>
        /// <param name="pageModel">分页对象</param>
        /// <returns></returns>
        public static WebPager WebPager(this HtmlHelper helper, PageModel pageModel)
        {
            return new WebPager(pageModel, helper.ViewContext);
        }

        /// <summary>
        /// 店铺后台分页
        /// </summary>
        /// <param name="helper">HtmlHelper</param>
        /// <param name="pageModel">分页对象</param>
        /// <returns></returns>
        public static StoreAdminPager StoreAdminPager(this HtmlHelper helper, PageModel pageModel)
        {
            return new StoreAdminPager(pageModel);
        }

        /// <summary>
        /// 商城后台分页
        /// </summary>
        /// <param name="helper">HtmlHelper</param>
        /// <param name="pageModel">分页对象</param>
        /// <returns></returns>
        public static MallAdminPager MallAdminPager(this HtmlHelper helper, PageModel pageModel)
        {
            return new MallAdminPager(pageModel);
        }
    }
}
