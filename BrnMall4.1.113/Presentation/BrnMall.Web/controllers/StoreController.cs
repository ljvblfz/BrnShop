using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Generic;

using BrnMall.Core;
using BrnMall.Services;
using BrnMall.Web.Framework;
using BrnMall.Web.Models;

namespace BrnMall.Web.Controllers
{
    /// <summary>
    /// 店铺控制器类
    /// </summary>
    public partial class StoreController : BaseWebController
    {
        /// <summary>
        /// 店铺首页
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 店铺分类
        /// </summary>
        public ActionResult Class()
        {
            //店铺分类id
            int storeCid = GetRouteInt("storeCid");
            if (storeCid == 0)
                storeCid = WebHelper.GetQueryInt("storeCid");
            //排序列
            int sortColumn = GetRouteInt("sortColumn");
            if (sortColumn == 0)
                sortColumn = WebHelper.GetQueryInt("sortColumn");
            //排序方向
            int sortDirection = GetRouteInt("sortDirection");
            if (sortDirection == 0)
                sortDirection = WebHelper.GetQueryInt("sortDirection");
            //当前页数
            int page = GetRouteInt("page");
            if (page == 0)
                page = WebHelper.GetQueryInt("page");

            //店铺分类信息
            StoreClassInfo storeClassInfo = Stores.GetStoreClassByStoreIdAndStoreCid(WorkContext.StoreId, storeCid);
            if (storeClassInfo == null)
                return View("~/views/shared/prompt.cshtml", new PromptModel("/", "此店铺分类不存在"));

            //分页对象
            PageModel pageModel = new PageModel(20, page, Products.GetStoreClassProductCount(storeCid, 0, 0));
            //视图对象
            StoreClassModel model = new StoreClassModel()
            {
                StoreCid = storeCid,
                SortColumn = sortColumn,
                SortDirection = sortDirection,
                PageModel = pageModel,
                ProductList = Products.GetStoreClassProductList(pageModel.PageSize, pageModel.PageNumber, storeCid, 0, 0, sortColumn, sortDirection),
                StoreClassInfo = storeClassInfo
            };

            return View(model);
        }

        /// <summary>
        /// 店铺搜索
        /// </summary>
        public ActionResult Search()
        {
            //搜索词
            string word = WebHelper.GetQueryString("word");
            if (word.Length == 0)
                return View("~/views/shared/prompt.cshtml", new PromptModel(WorkContext.UrlReferrer, "请输入搜索词"));

            //异步保存搜索历史
            Asyn.UpdateSearchHistory(WorkContext.Uid, word);

            //判断搜索词是否为店铺分类名称，如果是则重定向到店铺分类页面
            int storeCid = Stores.GetStoreCidByStoreIdAndName(WorkContext.StoreId, word);
            if (storeCid > 0)
                return Redirect(Url.Action("class", new RouteValueDictionary { { "storeId", WorkContext.StoreId }, { "storeCid", storeCid } }));

            //店铺分类id
            storeCid = WebHelper.GetQueryInt("storeCid");
            //店铺分类信息
            StoreClassInfo storeClassInfo = null;
            if (storeCid > 0)
            {
                storeClassInfo = Stores.GetStoreClassByStoreIdAndStoreCid(WorkContext.StoreId, storeCid);
                if (storeClassInfo == null)
                    return View("~/views/shared/prompt.cshtml", new PromptModel("/", "此店铺分类不存在"));
            }

            //开始价格
            int startPrice = WebHelper.GetQueryInt("startPrice");
            //结束价格
            int endPrice = WebHelper.GetQueryInt("endPrice");
            //排序列
            int sortColumn = WebHelper.GetQueryInt("sortColumn");
            //排序方向
            int sortDirection = WebHelper.GetQueryInt("sortDirection");
            //当前页数
            int page = WebHelper.GetQueryInt("page");

            //检查当前页数
            if (page < 1) page = 1;

            //商品总数量
            int totalCount = 0;
            //商品列表
            List<PartProductInfo> productList = null;
            Searches.SearchStoreProducts(20, page, word, WorkContext.StoreId, storeCid, startPrice, endPrice, sortColumn, sortDirection, ref totalCount, ref productList);

            if (productList == null || productList.Count < 1)
                return View("~/views/shared/prompt.cshtml", new PromptModel(WorkContext.UrlReferrer, "您搜索的商品不存在"));

            //分页对象
            PageModel pageModel = new PageModel(20, page, totalCount);
            //视图对象
            StoreSearchModel model = new StoreSearchModel()
            {
                Word = word,
                StoreCid = storeCid,
                StartPrice = startPrice,
                EndPrice = endPrice,
                SortColumn = sortColumn,
                SortDirection = sortDirection,
                PageModel = pageModel,
                ProductList = productList,
                StoreClassInfo = storeClassInfo
            };

            return View(model);
        }

        /// <summary>
        /// 店铺详情
        /// </summary>
        public ActionResult Details()
        {
            return View();
        }

        protected sealed override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            //店铺id
            WorkContext.StoreId = GetRouteInt("storeId");
            if (WorkContext.StoreId < 1)
                WorkContext.StoreId = WebHelper.GetQueryInt("storeId");

            //店铺信息
            WorkContext.StoreInfo = Stores.GetStoreById(WorkContext.StoreId);
        }

        protected sealed override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            //验证店铺状态
            if (WorkContext.StoreInfo == null || WorkContext.StoreInfo.State != (int)StoreState.Open)
                filterContext.Result = PromptView("/", "你访问的店铺不存在");
        }

        protected sealed override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            //将店铺主题添加到路由中
            RouteData.DataTokens.Add("theme", WorkContext.StoreInfo.Theme);
        }
    }
}
