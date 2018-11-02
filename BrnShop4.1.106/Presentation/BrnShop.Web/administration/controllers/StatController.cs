using System;
using System.Web;
using System.Data;
using System.Text;
using System.Web.Mvc;
using System.Collections.Generic;

using BrnShop.Core;
using BrnShop.Services;
using BrnShop.Web.Framework;
using BrnShop.Web.Admin.Models;

namespace BrnShop.Web.Admin.Controllers
{
    /// <summary>
    /// 后台报表统计控制器类
    /// </summary>
    public partial class StatController : BaseAdminController
    {
        /// <summary>
        /// 在线用户列表
        /// </summary>
        /// <param name="provinceId">省id</param>
        /// <param name="cityId">市id</param>
        /// <param name="regionId">区/县id</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="pageSize">每页数</param>
        /// <returns></returns>
        public ActionResult OnlineUserList(int provinceId = -1, int cityId = -1, int regionId = -1, int pageNumber = 1, int pageSize = 15)
        {
            int locationType = 0, locationId = 0;
            if (regionId > 0)
            {
                locationType = 2;
                locationId = regionId;
            }
            else if (cityId > 0)
            {
                locationType = 1;
                locationId = cityId;
            }
            else if (provinceId > 0)
            {
                locationType = 0;
                locationId = provinceId;
            }

            PageModel pageModel = new PageModel(pageSize, pageNumber, OnlineUsers.GetOnlineUserCount(locationType, locationId));

            OnlineUserListModel model = new OnlineUserListModel()
            {
                PageModel = pageModel,
                OnlineUserList = OnlineUsers.GetOnlineUserList(pageModel.PageSize, pageModel.PageNumber, locationType, locationId),
                ProvinceId = provinceId,
                CityId = cityId,
                RegionId = regionId
            };

            ShopUtils.SetAdminRefererCookie(string.Format("{0}?pageNumber={1}&pageSize={2}&provinceId={3}&cityId={4}&regionId={5}",
                                                          Url.Action("onlineuserlist"),
                                                          pageModel.PageNumber, pageModel.PageSize,
                                                          provinceId, cityId, regionId));
            return View(model);
        }

        /// <summary>
        /// 在线用户趋势
        /// </summary>
        /// <returns></returns>
        public ActionResult OnlineUserTrend()
        {
            OnlineUserTrendModel model = new OnlineUserTrendModel();

            model.PVStatList = PVStats.GetTodayHourPVStatList();

            return View(model);
        }

        /// <summary>
        /// 搜索词统计列表
        /// </summary>
        /// <param name="word">搜索词</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="pageSize">每页数</param>
        /// <returns></returns>
        public ActionResult SearchWordStatList(string word, int pageNumber = 1, int pageSize = 15)
        {
            PageModel pageModel = new PageModel(pageSize, pageNumber, AdminSearchHistories.GetSearchWordStatCount(word));

            SearchWordStatListModel model = new SearchWordStatListModel()
            {
                PageModel = pageModel,
                SearchWordStatList = AdminSearchHistories.GetSearchWordStatList(pageModel.PageSize, pageModel.PageNumber, word),
                Word = word
            };
            return View(model);
        }

        /// <summary>
        /// 客户端统计
        /// </summary>
        /// <returns></returns>
        public ActionResult ClientStat()
        {
            ClientStatModel model = new ClientStatModel();

            model.BrowserStat = PVStats.GetBrowserStat();
            model.OSStat = PVStats.GetOSStat();

            return View(model);
        }

        /// <summary>
        /// 地区统计
        /// </summary>
        /// <returns></returns>
        public ActionResult RegionStat()
        {
            RegionStatModel model = new RegionStatModel();

            model.RegionStat = PVStats.GetProvinceRegionStat();

            return View(model);
        }

        /// <summary>
        /// 商品统计
        /// </summary>
        /// <param name="productName">商品名称</param>
        /// <param name="categoryName">分类名称</param>
        /// <param name="brandName">品牌名称</param>
        /// <param name="cateId">分类id</param>
        /// <param name="brandId">品牌id</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="pageSize">每页数</param>
        /// <returns></returns>
        public ActionResult ProductStat(string productName, string categoryName, string brandName, int cateId = -1, int brandId = -1, int pageNumber = 1, int pageSize = 15)
        {
            string condition = AdminProducts.AdminGetProductListCondition(productName, cateId, brandId, -1);

            PageModel pageModel = new PageModel(pageSize, pageNumber, AdminProducts.AdminGetProductCount(condition));

            DataTable productList = AdminProducts.AdminGetProductList(pageModel.PageSize, pageModel.PageNumber, condition);
            StringBuilder pidList = new StringBuilder();
            foreach (DataRow row in productList.Rows)
            {
                pidList.AppendFormat("{0},", row["pid"]);
            }

            ProductStatModel model = new ProductStatModel()
            {
                PageModel = pageModel,
                ProductList = pidList.Length > 0 ? AdminProducts.GetProductSummaryList(pidList.Remove(pidList.Length - 1, 1).ToString()) : new DataTable(),
                ProductName = productName,
                CateId = cateId,
                CategoryName = string.IsNullOrWhiteSpace(categoryName) ? "全部分类" : categoryName,
                BrandId = brandId,
                BrandName = string.IsNullOrWhiteSpace(brandName) ? "全部品牌" : brandName
            };
            return View(model);
        }

        /// <summary>
        /// 销售商品列表
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="orderState">订单状态</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="pageSize">每页数</param>
        /// <returns></returns>
        public ActionResult SaleProductList(string startTime, string endTime, int orderState = -1, int pageNumber = 1, int pageSize = 15)
        {
            PageModel pageModel = new PageModel(pageSize, pageNumber, AdminOrders.GetSaleProductCount(startTime, endTime, orderState));

            List<SelectListItem> orderStateList = new List<SelectListItem>();
            orderStateList.Add(new SelectListItem() { Text = "全部", Value = "0" });
            orderStateList.Add(new SelectListItem() { Text = "等待付款", Value = ((int)OrderState.WaitPaying).ToString() });
            orderStateList.Add(new SelectListItem() { Text = "待确认", Value = ((int)OrderState.Confirming).ToString() });
            orderStateList.Add(new SelectListItem() { Text = "已确认", Value = ((int)OrderState.Confirmed).ToString() });
            orderStateList.Add(new SelectListItem() { Text = "备货中", Value = ((int)OrderState.PreProducting).ToString() });
            orderStateList.Add(new SelectListItem() { Text = "已发货", Value = ((int)OrderState.Sended).ToString() });
            orderStateList.Add(new SelectListItem() { Text = "已收货", Value = ((int)OrderState.Received).ToString() });
            orderStateList.Add(new SelectListItem() { Text = "已锁定", Value = ((int)OrderState.Locked).ToString() });
            orderStateList.Add(new SelectListItem() { Text = "已取消", Value = ((int)OrderState.Cancelled).ToString() });

            SaleProductListModel model = new SaleProductListModel()
            {
                PageModel = pageModel,
                SaleProductList = AdminOrders.GetSaleProductList(pageModel.PageSize, pageModel.PageNumber, startTime, endTime, orderState),
                StartTime = startTime,
                EndTime = endTime,
                OrderState = orderState,
                OrderStateList = orderStateList
            };
            return View(model);
        }

        /// <summary>
        /// 订单统计
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="statType">统计类型(0代表订单数，1代表订单合计)</param>
        /// <returns></returns>
        public ActionResult OrderStat(string startTime, string endTime, int statType = 0)
        {
            if (string.IsNullOrWhiteSpace(startTime))
                startTime = DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss");
            if (string.IsNullOrWhiteSpace(endTime))
                endTime = DateTime.Now.AddDays(1).Date.ToString("yyyy-MM-dd HH:mm:ss");

            OrderStatModel model = new OrderStatModel();

            model.StatType = statType;
            model.StartTime = startTime;
            model.EndTime = endTime;
            model.StatItemList = AdminOrders.GetOrderStat(statType, startTime, endTime);

            return View(model);
        }
    }
}
