using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BrnMall.Web
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
                            new[] { "BrnMall.Web.Controllers" });
            //分类路由
            routes.MapRoute("category",
                            "list/{filterAttr}-{cateId}-{brandId}-{filterPrice}-{onlyStock}-{sortColumn}-{sortDirection}-{page}.html",
                            new { controller = "catalog", action = "category" },
                            new[] { "BrnMall.Web.Controllers" });
            //分类路由
            routes.MapRoute("shortcategory",
                            "list/{cateId}.html",
                            new { controller = "catalog", action = "category" },
                            new[] { "BrnMall.Web.Controllers" });
            //商城搜索路由
            routes.MapRoute("mallsearch",
                            "search",
                            new { controller = "catalog", action = "search" },
                            new[] { "BrnMall.Web.Controllers" });
            //店铺路由
            routes.MapRoute("store",
                            "store/{storeId}.html",
                            new { controller = "store", action = "index" },
                            new[] { "BrnMall.Web.Controllers" });
            //店铺分类路由
            routes.MapRoute("storeclass",
                            "storeclass/{storeId}-{storeCid}-{startPrice}-{endPrice}-{sortColumn}-{sortDirection}-{page}.html",
                            new { controller = "store", action = "class" },
                            new[] { "BrnMall.Web.Controllers" });
            //店铺分类路由
            routes.MapRoute("shortstoreclass",
                            "storeclass/{storeId}-{storeCid}.html",
                            new { controller = "store", action = "class" },
                            new[] { "BrnMall.Web.Controllers" });
            //店铺搜索路由
            routes.MapRoute("storesearch",
                            "searchstore",
                            new { controller = "store", action = "search" },
                            new[] { "BrnMall.Web.Controllers" });
            //默认路由(此路由不能删除)
            routes.MapRoute("default",
                            "{controller}/{action}",
                            new { controller = "home", action = "index" },
                            new[] { "BrnMall.Web.Controllers" });
        }
    }
}