using System;
using System.Web;
using System.Data;
using System.Text;
using System.Web.Mvc;
using System.Collections.Generic;

using BrnMall.Core;
using BrnMall.Services;
using BrnMall.Web.Framework;
using BrnMall.Web.StoreAdmin.Models;

namespace BrnMall.Web.StoreAdmin.Controllers
{
    /// <summary>
    /// 店铺后台报表统计控制器类
    /// </summary>
    public partial class StatController : BaseStoreAdminController
    {
        /// <summary>
        /// 商品统计
        /// </summary>
        /// <param name="productName">商品名称</param>
        /// <param name="categoryName">分类名称</param>
        /// <param name="brandName">品牌名称</param>
        /// <param name="storeCid">店铺分类id</param>
        /// <param name="cateId">分类id</param>
        /// <param name="brandId">品牌id</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="pageSize">每页数</param>
        /// <returns></returns>
        public ActionResult ProductStat(string productName, string categoryName, string brandName, int storeCid = -1, int cateId = -1, int brandId = -1, int pageNumber = 1, int pageSize = 15)
        {
            string condition = AdminProducts.AdminGetProductListCondition(WorkContext.StoreId, storeCid, productName, cateId, brandId, -1);

            PageModel pageModel = new PageModel(pageSize, pageNumber, AdminProducts.AdminGetProductCount(condition));

            DataTable productList = AdminProducts.AdminGetProductList(pageModel.PageSize, pageModel.PageNumber, condition);
            StringBuilder pidList = new StringBuilder();
            foreach (DataRow row in productList.Rows)
            {
                pidList.AppendFormat("{0},", row["pid"]);
            }

            List<SelectListItem> storeClassList = new List<SelectListItem>();
            storeClassList.Add(new SelectListItem() { Text = "全部店铺分类", Value = "0" });
            foreach (StoreClassInfo storeClassInfo in AdminStores.GetStoreClassList(WorkContext.StoreId))
            {
                storeClassList.Add(new SelectListItem() { Text = storeClassInfo.Name, Value = storeClassInfo.StoreCid.ToString() });
            }

            ProductStatModel model = new ProductStatModel()
            {
                PageModel = pageModel,
                ProductList = pidList.Length > 0 ? AdminProducts.GetProductSummaryList(pidList.Remove(pidList.Length - 1, 1).ToString()) : new DataTable(),
                ProductName = productName,
                StoreCid = storeCid,
                StoreClassList = storeClassList,
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
        /// <param name="pageNumber">当前页数</param>
        /// <param name="pageSize">每页数</param>
        /// <returns></returns>
        public ActionResult SaleProductList(string startTime, string endTime, int orderState = -1, int pageNumber = 1, int pageSize = 15)
        {
            PageModel pageModel = new PageModel(pageSize, pageNumber, AdminOrders.GetSaleProductCount(WorkContext.StoreId, startTime, endTime, orderState));

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
                SaleProductList = AdminOrders.GetSaleProductList(pageModel.PageSize, pageModel.PageNumber, WorkContext.StoreId, startTime, endTime, orderState),
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
            model.StatItemList = AdminOrders.GetOrderStat(statType, WorkContext.StoreId, startTime, endTime);

            return View(model);
        }
    }
}
