using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BrnShop.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //商品路由
            routes.MapRoute("product",
                            "{pid}.html",
                            new { controller = "catalog", action = "product" },
                            new[] { "BrnShop.Web.Controllers" });
            //分类路由
            routes.MapRoute("category",
                            "list/{filterAttr}-{cateId}-{brandId}-{filterPrice}-{onlyStock}-{sortColumn}-{sortDirection}-{page}.html",
                            new { controller = "catalog", action = "category" },
                            new[] { "BrnShop.Web.Controllers" });
            //分类路由
            routes.MapRoute("shortcategory",
                            "list/{cateId}.html",
                            new { controller = "catalog", action = "category" },
                            new[] { "BrnShop.Web.Controllers" });
            //搜索路由
            routes.MapRoute("search",
                            "search",
                            new { controller = "catalog", action = "search" },
                            new[] { "BrnShop.Web.Controllers" });
            //默认路由(此路由不能删除)
            routes.MapRoute("default",
                            "{controller}/{action}",
                            new { controller = "home", action = "index" },
                            new[] { "BrnShop.Web.Controllers" });
        }
    }
}