using System;
using System.Web;
using System.Data;
using System.Text;
using System.Web.Mvc;
using System.Collections.Generic;

using BrnMall.Core;
using BrnMall.Services;
using BrnMall.Web.Framework;
using BrnMall.Web.MallAdmin.Models;

namespace BrnMall.Web.MallAdmin.Controllers
{
    /// <summary>
    /// 商城后台促销活动控制器类
    /// </summary>
    public partial class PromotionController : BaseMallAdminController
    {
        /// <summary>
        /// 单品促销活动列表
        /// </summary>
        /// <param name="storeName">店铺名称</param>
        /// <param name="promotionName">活动名称</param>
        /// <param name="promotionTime">活动时间</param>
        /// <param name="storeId">店铺id</param>
        /// <param name="pid">商品id</param>
        /// <returns></returns>
        public ActionResult SinglePromotionList(string storeName, string promotionName, string promotionTime, int storeId = -1, int pid = -1, int pageSize = 15, int pageNumber = 1)
        {
            string condition = AdminPromotions.AdminGetSinglePromotionListCondition(storeId, pid, promotionName, promotionTime);

            PageModel pageModel = new PageModel(pageSize, pageNumber, AdminPromotions.AdminGetSinglePromotionCount(condition));

            SinglePromotionListModel model = new SinglePromotionListModel()
            {
                PageModel = pageModel,
                SinglePromotionList = AdminPromotions.AdminGetSinglePromotionList(pageModel.PageSize, pageModel.PageNumber, condition),
                StoreId = storeId,
                StoreName = string.IsNullOrWhiteSpace(storeName) ? "全部店铺" : storeName,
                Pid = pid,
                PromotionName = promotionName,
                PromotionTime = promotionTime
            };

            MallUtils.SetAdminRefererCookie(string.Format("{0}?pageNumber={1}&pageSize={2}&storeId={3}&storeName={4}&pid={5}&promotionName={6}&promotionTime={7}",
                                                          Url.Action("singlepromotionlist"),
                                                          pageModel.PageNumber, pageModel.PageSize,
                                                          storeId, storeName, pid, promotionName, promotionTime));
            return View(model);
        }

        /// <summary>
        /// 添加单品促销活动
        /// </summary>
        [HttpGet]
        public ActionResult AddSinglePromotion()
        {
            SinglePromotionModel model = new SinglePromotionModel();
            LoadSinglePromotion();
            return View(model);
        }

        /// <summary>
        /// 添加单品促销活动
        /// </summary>
        /// <param name="model">促销活动模型</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddSinglePromotion(SinglePromotionModel model)
        {
            PartProductInfo partProductInfo = AdminProducts.AdminGetPartProductById(model.Pid);
            if (partProductInfo == null)
            {
                ModelState.AddModelError("Pid", "商品不存在");
            }
            else
            {
                if (AdminPromotions.AdminIsExistSinglePromotion(model.Pid, model.StartTime1, model.EndTime1) > 0)
                    ModelState.AddModelError("EndTime1", "此时间段内已经存在单品促销活动");

                if (model.StartTime2 != null && model.EndTime2 != null && AdminPromotions.AdminIsExistSinglePromotion(model.Pid, model.StartTime2.Value, model.EndTime2.Value) > 0)
                    ModelState.AddModelError("EndTime2", "此时间段内已经存在单品促销活动");

                if (model.StartTime3 != null && model.EndTime3 != null && AdminPromotions.AdminIsExistSinglePromotion(model.Pid, model.StartTime3.Value, model.EndTime3.Value) > 0)
                    ModelState.AddModelError("EndTime3", "此时间段内已经存在单品促销活动");
            }

            if (ModelState.IsValid)
            {
                DateTime noTime = new DateTime(1900, 1, 1);

                SinglePromotionInfo singlePromotionInfo = new SinglePromotionInfo()
                {
                    Pid = model.Pid,
                    StoreId = partProductInfo.StoreId,
                    StartTime1 = model.StartTime1,
                    EndTime1 = model.EndTime1,
                    StartTime2 = model.StartTime2.HasValue ? model.StartTime2.Value : noTime,
                    EndTime2 = model.EndTime2.HasValue ? model.EndTime2.Value : noTime,
                    StartTime3 = model.StartTime3.HasValue ? model.StartTime3.Value : noTime,
                    EndTime3 = model.EndTime3.HasValue ? model.EndTime3.Value : noTime,
                    UserRankLower = model.UserRankLower,
                    State = model.State,
                    Name = model.PromotionName,
                    Slogan = model.Slogan == null ? "" : model.Slogan,
                    DiscountType = model.DiscountType,
                    DiscountValue = model.DiscountValue,
                    CouponTypeId = model.CouponTypeId,
                    PayCredits = model.PayCredits,
                    IsStock = model.IsStock,
                    Stock = model.Stock,
                    QuotaLower = model.QuotaLower,
                    QuotaUpper = model.QuotaUpper,
                    AllowBuyCount = model.AllowBuyCount
                };

                AdminPromotions.CreateSinglePromotion(singlePromotionInfo);
                AddMallAdminLog("添加单品促销", "添加单品促销,单品促销为:" + model.Pid + "_" + model.PromotionName);
                return PromptView("单品促销添加成功");
            }

            LoadSinglePromotion();
            return View(model);
        }

        /// <summary>
        /// 编辑单品促销活动
        /// </summary>
        /// <param name="pmId">促销活动id.</param>
        [HttpGet]
        public ActionResult EditSinglePromotion(int pmId = -1)
        {
            SinglePromotionInfo singlePromotionInfo = AdminPromotions.AdminGetSinglePromotionById(pmId);
            if (singlePromotionInfo == null)
                return PromptView("单品促销不存在");

            DateTime? nullTime = null;
            DateTime noTime = new DateTime(1900, 1, 1);

            SinglePromotionModel model = new SinglePromotionModel();
            model.Pid = singlePromotionInfo.Pid;
            model.StartTime1 = singlePromotionInfo.StartTime1;
            model.EndTime1 = singlePromotionInfo.EndTime1;
            model.StartTime2 = singlePromotionInfo.StartTime2 == noTime ? nullTime : singlePromotionInfo.StartTime2;
            model.EndTime2 = singlePromotionInfo.EndTime2 == noTime ? nullTime : singlePromotionInfo.EndTime2;
            model.StartTime3 = singlePromotionInfo.StartTime3 == noTime ? nullTime : singlePromotionInfo.StartTime3;
            model.EndTime3 = singlePromotionInfo.EndTime3 == noTime ? nullTime : singlePromotionInfo.EndTime3;
            model.UserRankLower = singlePromotionInfo.UserRankLower;
            model.State = singlePromotionInfo.State;
            model.PromotionName = singlePromotionInfo.Name;
            model.Slogan = singlePromotionInfo.Slogan;
            model.DiscountType = singlePromotionInfo.DiscountType;
            model.DiscountValue = singlePromotionInfo.DiscountValue;
            model.CouponTypeId = singlePromotionInfo.CouponTypeId;
            model.PayCredits = singlePromotionInfo.PayCredits;
            model.IsStock = singlePromotionInfo.IsStock;
            model.Stock = singlePromotionInfo.Stock;
            model.QuotaLower = singlePromotionInfo.QuotaLower;
            model.QuotaUpper = singlePromotionInfo.QuotaUpper;
            model.AllowBuyCount = singlePromotionInfo.AllowBuyCount;

            LoadSinglePromotion();

            return View(model);
        }

        /// <summary>
        /// 编辑单品促销活动
        /// </summary>
        /// <param name="model">促销活动模型</param>
        /// <param name="pmId">促销活动id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditSinglePromotion(SinglePromotionModel model, int pmId = -1)
        {
            SinglePromotionInfo singlePromotionInfo = AdminPromotions.AdminGetSinglePromotionById(pmId);
            if (singlePromotionInfo == null)
                return PromptView("单品促销不存在");

            int temp1 = AdminPromotions.AdminIsExistSinglePromotion(model.Pid, model.StartTime1, model.EndTime1);
            if (temp1 > 0 && temp1 != pmId)
                ModelState.AddModelError("EndTime1", "此时间段内已经存在单品促销活动");

            if (model.StartTime2 != null && model.EndTime2 != null)
            {
                temp1 = AdminPromotions.AdminIsExistSinglePromotion(model.Pid, model.StartTime2.Value, model.EndTime2.Value);
                if (temp1 > 0 && temp1 != pmId)
                    ModelState.AddModelError("EndTime2", "此时间段内已经存在单品促销活动");
            }

            if (model.StartTime3 != null && model.EndTime3 != null)
            {
                temp1 = AdminPromotions.AdminIsExistSinglePromotion(model.Pid, model.StartTime3.Value, model.EndTime3.Value);
                if (temp1 > 0 && temp1 != pmId)
                    ModelState.AddModelError("EndTime3", "此时间段内已经存在单品促销活动");
            }

            if (ModelState.IsValid)
            {
                DateTime noTime = new DateTime(1900, 1, 1);

                singlePromotionInfo.StartTime1 = model.StartTime1;
                singlePromotionInfo.EndTime1 = model.EndTime1;
                singlePromotionInfo.StartTime2 = model.StartTime2.HasValue ? model.StartTime2.Value : noTime;
                singlePromotionInfo.EndTime2 = model.EndTime2.HasValue ? model.EndTime2.Value : noTime;
                singlePromotionInfo.StartTime3 = model.StartTime3.HasValue ? model.StartTime3.Value : noTime;
                singlePromotionInfo.EndTime3 = model.EndTime3.HasValue ? model.EndTime3.Value : noTime;
                singlePromotionInfo.UserRankLower = model.UserRankLower;
                singlePromotionInfo.State = model.State;
                singlePromotionInfo.Name = model.PromotionName;
                singlePromotionInfo.Slogan = model.Slogan == null ? "" : model.Slogan;
                singlePromotionInfo.DiscountType = model.DiscountType;
                singlePromotionInfo.DiscountValue = model.DiscountValue;
                singlePromotionInfo.CouponTypeId = model.CouponTypeId;
                singlePromotionInfo.PayCredits = model.PayCredits;
                singlePromotionInfo.IsStock = model.IsStock;
                singlePromotionInfo.Stock = model.Stock;
                singlePromotionInfo.QuotaLower = model.QuotaLower;
                singlePromotionInfo.QuotaUpper = model.QuotaUpper;
                singlePromotionInfo.AllowBuyCount = model.AllowBuyCount;

                AdminPromotions.UpdateSinglePromotion(singlePromotionInfo);
                AddMallAdminLog("修改单品促销", "修改单品促销,单品促销ID为:" + pmId);
                return PromptView("单品促销修改成功");
            }

            LoadSinglePromotion();
            return View(model);
        }

        /// <summary>
        /// 删除单品促销活动
        /// </summary>
        /// <param name="pmIdList">促销活动id列表</param>
        public ActionResult DelSinglePromotion(int[] pmIdList)
        {
            AdminPromotions.DeleteSinglePromotionById(pmIdList);
            AddMallAdminLog("删除单品促销", "删除单品促销,单品促销ID为:" + CommonHelper.IntArrayToString(pmIdList));
            return PromptView("单品促销删除成功");
        }

        private void LoadSinglePromotion()
        {
            List<SelectListItem> discountTypeList = new List<SelectListItem>();
            discountTypeList.Add(new SelectListItem() { Text = "折扣", Value = "0" });
            discountTypeList.Add(new SelectListItem() { Text = "直降", Value = "1" });
            discountTypeList.Add(new SelectListItem() { Text = "折后价", Value = "2" });
            ViewData["discountTypeList"] = discountTypeList;

            List<SelectListItem> userRankList = new List<SelectListItem>();
            foreach (UserRankInfo userRankInfo in AdminUserRanks.GetCustomerUserRankList())
            {
                userRankList.Add(new SelectListItem() { Text = userRankInfo.Title, Value = userRankInfo.UserRid.ToString() });
            }
            ViewData["userRankList"] = userRankList;

            ViewData["referer"] = MallUtils.GetMallAdminRefererCookie();
        }






        /// <summary>
        /// 买送促销活动列表
        /// </summary>
        /// <returns></returns>
        public ActionResult BuySendPromotionList(string storeName, string promotionName, string promotionTime, int storeId = -1, int pageSize = 15, int pageNumber = 1)
        {
            string condition = AdminPromotions.AdminGetBuySendPromotionListCondition(storeId, promotionName, promotionTime);

            PageModel pageModel = new PageModel(pageSize, pageNumber, AdminPromotions.AdminGetBuySendPromotionCount(condition));

            BuySendPromotionListModel model = new BuySendPromotionListModel()
            {
                PageModel = pageModel,
                BuySendPromotionList = AdminPromotions.AdminGetBuySendPromotionList(pageModel.PageSize, pageModel.PageNumber, condition),
                StoreId = storeId,
                StoreName = string.IsNullOrWhiteSpace(storeName) ? "全部店铺" : storeName,
                PromotionName = promotionName,
                PromotionTime = promotionTime
            };

            MallUtils.SetAdminRefererCookie(string.Format("{0}?pageNumber={1}&pageSize={2}&storeId={3}&storeName={4}&promotionName={5}&promotionTime={6}",
                                                          Url.Action("buysendpromotionlist"),
                                                          pageModel.PageNumber,
                                                          pageModel.PageSize,
                                                          storeId, storeName,
                                                          promotionName, promotionTime));
            return View(model);
        }

        /// <summary>
        /// 添加买送促销活动
        /// </summary>
        [HttpGet]
        public ActionResult AddBuySendPromotion()
        {
            BuySendPromotionModel model = new BuySendPromotionModel();
            LoadBuySendPromotion();
            return View(model);
        }

        /// <summary>
        /// 添加买送促销活动
        /// </summary>
        /// <param name="model">活动模型</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddBuySendPromotion(BuySendPromotionModel model)
        {
            if (ModelState.IsValid)
            {
                BuySendPromotionInfo buySendPromotionInfo = new BuySendPromotionInfo()
                {
                    StoreId = model.StoreId,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    UserRankLower = model.UserRankLower,
                    State = model.State,
                    Name = model.PromotionName,
                    Type = model.Type,
                    BuyCount = model.BuyCount,
                    SendCount = model.SendCount
                };

                AdminPromotions.CreateBuySendPromotion(buySendPromotionInfo);
                AddMallAdminLog("添加买送促销", "添加买送促销,买送促销为:" + model.PromotionName);
                return PromptView("买送促销添加成功");
            }

            LoadBuySendPromotion();
            return View(model);
        }

        /// <summary>
        /// 编辑买送促销活动
        /// </summary>
        /// <param name="pmId">活动id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditBuySendPromotion(int pmId = -1)
        {
            BuySendPromotionInfo buySendPromotionInfo = AdminPromotions.AdminGetBuySendPromotionById(pmId);
            if (buySendPromotionInfo == null)
                return PromptView("买送促销不存在");

            BuySendPromotionModel model = new BuySendPromotionModel();
            model.StartTime = buySendPromotionInfo.StartTime;
            model.EndTime = buySendPromotionInfo.EndTime;
            model.UserRankLower = buySendPromotionInfo.UserRankLower;
            model.State = buySendPromotionInfo.State;
            model.PromotionName = buySendPromotionInfo.Name;
            model.Type = buySendPromotionInfo.Type;
            model.BuyCount = buySendPromotionInfo.BuyCount;
            model.SendCount = buySendPromotionInfo.SendCount;

            model.StoreId = buySendPromotionInfo.StoreId;
            model.StoreName = AdminStores.GetStoreById(buySendPromotionInfo.StoreId).Name;

            LoadBuySendPromotion();
            return View(model);
        }

        /// <summary>
        /// 编辑买送促销活动
        /// </summary>
        /// <param name="model">活动模型</param>
        /// <param name="pmId">活动id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditBuySendPromotion(BuySendPromotionModel model, int pmId = -1)
        {
            BuySendPromotionInfo buySendPromotionInfo = AdminPromotions.AdminGetBuySendPromotionById(pmId);
            if (buySendPromotionInfo == null)
                return PromptView("买送促销不存在");

            if (ModelState.IsValid)
            {
                buySendPromotionInfo.StartTime = model.StartTime;
                buySendPromotionInfo.EndTime = model.EndTime;
                buySendPromotionInfo.UserRankLower = model.UserRankLower;
                buySendPromotionInfo.State = model.State;
                buySendPromotionInfo.Name = model.PromotionName;
                buySendPromotionInfo.Type = model.Type;
                buySendPromotionInfo.BuyCount = model.BuyCount;
                buySendPromotionInfo.SendCount = model.SendCount;

                AdminPromotions.UpdateBuySendPromotion(buySendPromotionInfo);
                AddMallAdminLog("修改买送促销", "修改买送促销,买送促销ID为:" + pmId);
                return PromptView("买送促销修改成功");
            }

            LoadBuySendPromotion();
            return View(model);
        }

        /// <summary>
        /// 删除买送促销活动
        /// </summary>
        /// <param name="pmIdList">活动id</param>
        /// <returns></returns>
        public ActionResult DelBuySendPromotion(int[] pmIdList)
        {
            AdminPromotions.DeleteBuySendPromotionById(pmIdList);
            AddMallAdminLog("删除买送促销", "删除买送促销,买送促销ID为:" + CommonHelper.IntArrayToString(pmIdList));
            return PromptView("买送促销删除成功");
        }

        private void LoadBuySendPromotion()
        {
            List<SelectListItem> userRankList = new List<SelectListItem>();
            foreach (UserRankInfo userRankInfo in AdminUserRanks.GetCustomerUserRankList())
            {
                userRankList.Add(new SelectListItem() { Text = userRankInfo.Title, Value = userRankInfo.UserRid.ToString() });
            }
            ViewData["userRankList"] = userRankList;

            ViewData["referer"] = MallUtils.GetMallAdminRefererCookie();
        }






        /// <summary>
        /// 买送商品列表
        /// </summary>
        /// <param name="pmId">活动id</param>
        /// <returns></returns>
        public ActionResult BuySendProductList(string productName, int pmId = -1, int pid = -1, int pageSize = 15, int pageNumber = 1)
        {
            BuySendPromotionInfo buySendPromotionInfo = AdminPromotions.AdminGetBuySendPromotionById(pmId);
            if (buySendPromotionInfo == null)
                return PromptView("买送促销不存在");

            PageModel pageModel = new PageModel(pageSize, pageNumber, AdminPromotions.AdminGetBuySendProductCount(pmId, pid));

            BuySendProductListModel model = new BuySendProductListModel()
            {
                PageModel = pageModel,
                BuySendProductList = AdminPromotions.AdminGetBuySendProductList(pageSize, pageNumber, pmId, pid),
                PmId = pmId,
                StoreId = buySendPromotionInfo.StoreId,
                Pid = pid,
                ProductName = string.IsNullOrWhiteSpace(productName) ? "选择商品" : productName
            };

            MallUtils.SetAdminRefererCookie(string.Format("{0}?pageNumber={1}&pageSize={2}&pmId={3}&pid={4}&productName={5}",
                                                          Url.Action("buysendproductlist"),
                                                          pageModel.PageNumber,
                                                          pageModel.PageSize,
                                                          pmId,
                                                          pid,
                                                          productName));
            return View(model);
        }

        /// <summary>
        /// 添加买送商品
        /// </summary>
        /// <returns></returns>
        public ActionResult AddBuySendProduct(int pmId = -1, int pid = 1)
        {
            BuySendPromotionInfo buySendPromotionInfo = AdminPromotions.AdminGetBuySendPromotionById(pmId);
            if (buySendPromotionInfo == null)
                return PromptView("买送促销不存在");
            else if (buySendPromotionInfo.Type == 0)
                return PromptView("全场买送促销不需要添加商品");

            PartProductInfo partProductInfo = AdminProducts.AdminGetPartProductById(pid);
            if (partProductInfo == null)
                return PromptView("此商品不存在");

            if (buySendPromotionInfo.StoreId != partProductInfo.StoreId)
                return PromptView(Url.Action("buysendproductlist", new { pmId = pmId }), "只能关联同一店铺的商品");

            if (AdminPromotions.IsExistBuySendProduct(pmId, pid))
                return PromptView("此商品已经存在");

            AdminPromotions.AddBuySendProduct(pmId, pid);
            AddMallAdminLog("添加买送商品", "添加买送商品,商品为:" + partProductInfo.Name);
            return PromptView("买送商品添加成功");
        }

        /// <summary>
        /// 删除买送商品
        /// </summary>
        /// <param name="recordId">记录id</param>
        /// <returns></returns>
        public ActionResult DelBuySendProduct(int recordId)
        {
            bool result = AdminPromotions.DeleteBuySendProductByRecordId(new[] { recordId });
            if (result)
            {
                AddMallAdminLog("删除买送商品", "删除买送商品,商品ID:" + recordId);
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }

        /// <summary>
        /// 删除买送商品
        /// </summary>
        /// <param name="recordIdList">记录id</param>
        /// <returns></returns>
        public ActionResult DelBuySendProduct(int[] recordIdList)
        {
            AdminPromotions.DeleteBuySendProductByRecordId(recordIdList);
            AddMallAdminLog("删除买送商品", "删除买送商品,商品ID:" + CommonHelper.IntArrayToString(recordIdList));
            return PromptView("买送商品删除成功");
        }







        /// <summary>
        /// 赠品促销活动列表
        /// </summary>
        /// <param name="storeName">店铺名称</param>
        /// <param name="promotionName">活动名称</param>
        /// <param name="promotionTime">活动时间</param>
        /// <param name="storeId">店铺id</param>
        /// <param name="pid">商品id</param>
        /// <returns></returns>
        public ActionResult GiftPromotionList(string storeName, string promotionName, string promotionTime, int storeId = -1, int pid = -1, int pageSize = 15, int pageNumber = 1)
        {
            string condition = AdminPromotions.AdminGetGiftPromotionListCondition(storeId, pid, promotionName, promotionTime);

            PageModel pageModel = new PageModel(pageSize, pageNumber, AdminPromotions.AdminGetGiftPromotionCount(condition));

            GiftPromotionListModel model = new GiftPromotionListModel()
            {
                PageModel = pageModel,
                GiftPromotionList = AdminPromotions.AdminGetGiftPromotionList(pageModel.PageSize, pageModel.PageNumber, condition),
                StoreId = storeId,
                StoreName = string.IsNullOrWhiteSpace(storeName) ? "全部店铺" : storeName,
                Pid = pid,
                PromotionName = promotionName,
                PromotionTime = promotionTime
            };

            MallUtils.SetAdminRefererCookie(string.Format("{0}?pageNumber={1}&pageSize={2}&storeId={3}&storeName={4}&pid={5}&promotionName={6}&promotionTime={7}",
                                                          Url.Action("giftpromotionlist"),
                                                          pageModel.PageNumber, pageModel.PageSize,
                                                          storeId, storeName, pid, promotionName, promotionTime));
            return View(model);
        }

        /// <summary>
        /// 添加赠品促销活动
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddGiftPromotion()
        {
            GiftPromotionModel model = new GiftPromotionModel();
            LoadGiftPromotion();
            return View(model);
        }

        /// <summary>
        /// 添加赠品促销活动
        /// </summary>
        /// <param name="model">活动模型</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddGiftPromotion(GiftPromotionModel model)
        {
            PartProductInfo partProductInfo = AdminProducts.AdminGetPartProductById(model.Pid);
            if (partProductInfo == null)
            {
                ModelState.AddModelError("Pid", "商品不存在");
            }
            else
            {
                if (AdminPromotions.AdminIsExistGiftPromotion(model.Pid, model.StartTime1, model.EndTime1) > 0)
                    ModelState.AddModelError("EndTime1", "此时间段内已经存在赠品促销活动");

                if (model.StartTime2 != null && AdminPromotions.AdminIsExistGiftPromotion(model.Pid, model.StartTime2.Value, model.EndTime2.Value) > 0)
                    ModelState.AddModelError("EndTime2", "此时间段内已经存在赠品促销活动");

                if (model.StartTime3 != null && AdminPromotions.AdminIsExistGiftPromotion(model.Pid, model.StartTime3.Value, model.EndTime3.Value) > 0)
                    ModelState.AddModelError("EndTime3", "此时间段内已经存在赠品促销活动");
            }

            if (ModelState.IsValid)
            {
                DateTime noTime = new DateTime(1900, 1, 1);

                GiftPromotionInfo giftPromotionInfo = new GiftPromotionInfo()
                {
                    Pid = model.Pid,
                    StoreId = partProductInfo.StoreId,
                    StartTime1 = model.StartTime1,
                    EndTime1 = model.EndTime1,
                    StartTime2 = model.StartTime2.HasValue ? model.StartTime2.Value : noTime,
                    EndTime2 = model.EndTime2.HasValue ? model.EndTime2.Value : noTime,
                    StartTime3 = model.StartTime3.HasValue ? model.StartTime3.Value : noTime,
                    EndTime3 = model.EndTime3.HasValue ? model.EndTime3.Value : noTime,
                    UserRankLower = model.UserRankLower,
                    State = model.State,
                    Name = model.PromotionName,
                    QuotaUpper = model.QuotaUpper
                };

                AdminPromotions.CreateGiftPromotion(giftPromotionInfo);
                AddMallAdminLog("添加赠品促销活动", "添加赠品促销活动,赠品促销活动为:" + model.PromotionName);
                return PromptView("赠品促销活动添加成功");
            }

            LoadGiftPromotion();
            return View(model);
        }

        /// <summary>
        /// 编辑赠品促销活动
        /// </summary>
        /// <param name="pmId">活动id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditGiftPromotion(int pmId = -1)
        {
            GiftPromotionInfo giftPromotionInfo = AdminPromotions.AdminGetGiftPromotionById(pmId);
            if (giftPromotionInfo == null)
                return PromptView("赠品促销活动不存在");

            DateTime? nullTime = null;
            DateTime noTime = new DateTime(1900, 1, 1);

            GiftPromotionModel model = new GiftPromotionModel();
            model.Pid = giftPromotionInfo.Pid;
            model.StartTime1 = giftPromotionInfo.StartTime1;
            model.EndTime1 = giftPromotionInfo.EndTime1;
            model.StartTime2 = giftPromotionInfo.StartTime2 == noTime ? nullTime : giftPromotionInfo.StartTime2;
            model.EndTime2 = giftPromotionInfo.EndTime2 == noTime ? nullTime : giftPromotionInfo.EndTime2;
            model.StartTime3 = giftPromotionInfo.StartTime3 == noTime ? nullTime : giftPromotionInfo.StartTime3;
            model.EndTime3 = giftPromotionInfo.EndTime3 == noTime ? nullTime : giftPromotionInfo.EndTime3;
            model.UserRankLower = giftPromotionInfo.UserRankLower;
            model.State = giftPromotionInfo.State;
            model.PromotionName = giftPromotionInfo.Name;
            model.QuotaUpper = giftPromotionInfo.QuotaUpper;

            LoadGiftPromotion();

            return View(model);
        }

        /// <summary>
        /// 编辑赠品促销活动
        /// </summary>
        /// <param name="model">活动模型</param>
        /// <param name="pmId">活动id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditGiftPromotion(GiftPromotionModel model, int pmId = -1)
        {
            GiftPromotionInfo giftPromotionInfo = AdminPromotions.AdminGetGiftPromotionById(pmId);
            if (giftPromotionInfo == null)
                return PromptView("赠品促销活动不存在");

            int temp1 = AdminPromotions.AdminIsExistGiftPromotion(model.Pid, model.StartTime1, model.EndTime1);
            if (temp1 > 0 && temp1 != pmId)
                ModelState.AddModelError("EndTime1", "此时间段内已经存在赠品促销活动");

            if (model.StartTime2 != null)
            {
                temp1 = AdminPromotions.AdminIsExistGiftPromotion(model.Pid, model.StartTime2.Value, model.EndTime2.Value);
                if (temp1 > 0 && temp1 != pmId)
                    ModelState.AddModelError("EndTime2", "此时间段内已经存在赠品促销活动");
            }

            if (model.StartTime3 != null)
            {
                temp1 = AdminPromotions.AdminIsExistGiftPromotion(model.Pid, model.StartTime3.Value, model.EndTime3.Value);
                if (temp1 > 0 && temp1 != pmId)
                    ModelState.AddModelError("EndTime3", "此时间段内已经存在赠品促销活动");
            }

            if (ModelState.IsValid)
            {
                DateTime noTime = new DateTime(1900, 1, 1);

                //giftPromotionInfo.Pid = model.Pid;
                giftPromotionInfo.StartTime1 = model.StartTime1;
                giftPromotionInfo.EndTime1 = model.EndTime1;
                giftPromotionInfo.StartTime2 = model.StartTime2.HasValue ? model.StartTime2.Value : noTime;
                giftPromotionInfo.EndTime2 = model.EndTime2.HasValue ? model.EndTime2.Value : noTime;
                giftPromotionInfo.StartTime3 = model.StartTime3.HasValue ? model.StartTime3.Value : noTime;
                giftPromotionInfo.EndTime3 = model.EndTime3.HasValue ? model.EndTime3.Value : noTime;
                giftPromotionInfo.UserRankLower = model.UserRankLower;
                giftPromotionInfo.State = model.State;
                giftPromotionInfo.Name = model.PromotionName;
                giftPromotionInfo.QuotaUpper = model.QuotaUpper;

                AdminPromotions.UpdateGiftPromotion(giftPromotionInfo);
                AddMallAdminLog("修改赠品促销活动", "修改赠品促销活动,赠品促销活动ID为:" + pmId);
                return PromptView("赠品促销活动修改成功");
            }

            LoadGiftPromotion();
            return View(model);
        }

        /// <summary>
        /// 删除赠品促销
        /// </summary>
        /// <param name="pmIdList">活动id</param>
        /// <returns></returns>
        public ActionResult DelGiftPromotion(int[] pmIdList)
        {
            AdminPromotions.DeleteGiftPromotionById(pmIdList);
            AddMallAdminLog("删除赠品促销活动", "删除赠品促销活动,赠品促销活动ID为:" + CommonHelper.IntArrayToString(pmIdList));
            return PromptView("赠品促销活动删除成功");
        }

        private void LoadGiftPromotion()
        {
            List<SelectListItem> userRankList = new List<SelectListItem>();
            foreach (UserRankInfo userRankInfo in AdminUserRanks.GetCustomerUserRankList())
            {
                userRankList.Add(new SelectListItem() { Text = userRankInfo.Title, Value = userRankInfo.UserRid.ToString() });
            }
            ViewData["userRankList"] = userRankList;

            ViewData["referer"] = MallUtils.GetMallAdminRefererCookie();
        }





        /// <summary>
        /// 赠品列表
        /// </summary>
        /// <param name="pmId">赠品活动id</param>
        /// <returns></returns>
        public ActionResult GiftList(int pmId = -1)
        {
            GiftPromotionInfo giftPromotionInfo = AdminPromotions.AdminGetGiftPromotionById(pmId);
            if (giftPromotionInfo == null)
                return PromptView("此赠品促销不存在");

            GiftListModel model = new GiftListModel()
            {
                ExtGiftList = AdminPromotions.AdminGetExtGiftList(pmId),
                PmId = pmId,
                StoreId = giftPromotionInfo.StoreId
            };

            ViewData["referer"] = MallUtils.GetMallAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 添加赠品
        /// </summary>
        /// <param name="pmId">促销活动id</param>
        /// <param name="giftId">赠品id</param>
        /// <param name="number">数量</param>
        /// <returns></returns>
        public ActionResult AddGift(int pmId = -1, int giftId = -1, int number = -1)
        {
            GiftPromotionInfo giftPromotionInfo = AdminPromotions.AdminGetGiftPromotionById(pmId);
            if (giftPromotionInfo == null)
                return PromptView("此赠品促销不存在");

            PartProductInfo partProductInfo = AdminProducts.AdminGetPartProductById(giftId);
            if (partProductInfo == null)
                return PromptView("此赠品不存在");

            if (giftPromotionInfo.StoreId != partProductInfo.StoreId)
                return PromptView(Url.Action("giftlist", new { pmId = pmId }), "只能关联同一店铺的商品");

            if (Promotions.IsExistGift(pmId, giftId))
                return PromptView(Url.Action("giftlist", new { pmId = pmId }), "此赠品已经存在");

            if (number < 1)
                return PromptView(Url.Action("giftlist", new { pmId = pmId }), "赠品数量不能小于0");

            AdminPromotions.AddGift(pmId, giftId, number, giftPromotionInfo.Pid);
            AddMallAdminLog("添加赠品", "添加赠品,赠品为:" + partProductInfo.Name);
            return PromptView(Url.Action("giftlist", new { pmId = pmId }), "赠品添加成功");
        }

        /// <summary>
        /// 修改赠品数量
        /// </summary>
        /// <param name="pmId">促销活动id</param>
        /// <param name="giftId">赠品id</param>
        /// <param name="number">数量</param>
        /// <returns></returns>
        public ActionResult UpdateGiftNumber(int pmId = -1, int giftId = -1, int number = 1)
        {
            bool result = AdminPromotions.UpdateGiftNumber(pmId, giftId, number);
            if (result)
            {
                AddMallAdminLog("修改赠品数量", "修改赠品" + pmId + "_" + giftId + "数量,数量为:" + number);
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }

        /// <summary>
        /// 删除赠品
        /// </summary>
        /// <param name="pmId">促销活动id</param>
        /// <param name="giftId">赠品id</param>
        /// <returns></returns>
        public ActionResult DelGift(int pmId = -1, int giftId = -1)
        {
            bool result = AdminPromotions.DeleteGiftByPmIdAndGiftId(pmId, giftId);
            if (result)
            {
                AddMallAdminLog("删除赠品", "删除赠品,赠品ID为" + pmId + "_" + giftId);
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }







        /// <summary>
        /// 套装促销活动列表
        /// </summary>
        /// <returns></returns>
        public ActionResult SuitPromotionList(string storeName, string promotionName, string promotionTime, int storeId = -1, int pid = -1, int pageSize = 15, int pageNumber = 1)
        {
            string condition = AdminPromotions.AdminGetSuitPromotionListCondition(storeId, pid, promotionName, promotionTime);

            PageModel pageModel = new PageModel(pageSize, pageNumber, AdminPromotions.AdminGetSuitPromotionCount(condition));

            SuitPromotionListModel model = new SuitPromotionListModel()
            {
                PageModel = pageModel,
                SuitPromotionList = AdminPromotions.AdminGetSuitPromotionList(pageModel.PageSize, pageModel.PageNumber, condition),
                StoreId = storeId,
                StoreName = string.IsNullOrWhiteSpace(storeName) ? "选择店铺" : storeName,
                Pid = pid,
                PromotionName = promotionName,
                PromotionTime = promotionTime
            };

            MallUtils.SetAdminRefererCookie(string.Format("{0}?pageNumber={1}&pageSize={2}&storeId={3}&storeName={4}&pid={5}&promotionName={6}&promotionTime={7}",
                                                          Url.Action("suitpromotionlist"),
                                                          pageModel.PageNumber,
                                                          pageModel.PageSize,
                                                          storeId, storeName,
                                                          pid, promotionName, promotionTime));
            return View(model);
        }

        /// <summary>
        /// 添加套装促销活动
        /// </summary>
        [HttpGet]
        public ActionResult AddSuitPromotion()
        {
            SuitPromotionModel model = new SuitPromotionModel();
            LoadSuitPromotion();
            return View(model);
        }

        /// <summary>
        /// 添加套装促销活动
        /// </summary>
        /// <param name="model">活动模型</param>
        [HttpPost]
        public ActionResult AddSuitPromotion(SuitPromotionModel model)
        {
            if (ModelState.IsValid)
            {
                DateTime noTime = new DateTime(1900, 1, 1);

                SuitPromotionInfo suitPromotionInfo = new SuitPromotionInfo()
                {
                    StoreId = model.StoreId,
                    StartTime1 = model.StartTime1,
                    EndTime1 = model.EndTime1,
                    StartTime2 = model.StartTime2.HasValue ? model.StartTime2.Value : noTime,
                    EndTime2 = model.EndTime2.HasValue ? model.EndTime2.Value : noTime,
                    StartTime3 = model.StartTime3.HasValue ? model.StartTime3.Value : noTime,
                    EndTime3 = model.EndTime3.HasValue ? model.EndTime3.Value : noTime,
                    UserRankLower = model.UserRankLower,
                    State = model.State,
                    Name = model.PromotionName,
                    QuotaUpper = model.QuotaUpper,
                    OnlyOnce = model.OnlyOnce
                };

                AdminPromotions.CreateSuitPromotion(suitPromotionInfo);
                AddMallAdminLog("添加套装促销活动", "添加套装促销活动,套装促销活动为:" + model.PromotionName);
                return PromptView("套装促销活动添加成功");
            }

            LoadSuitPromotion();
            return View(model);
        }

        /// <summary>
        /// 编辑套装促销活动
        /// </summary>
        /// <param name="pmId">活动id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditSuitPromotion(int pmId = -1)
        {
            SuitPromotionInfo suitPromotionInfo = AdminPromotions.AdminGetSuitPromotionById(pmId);
            if (suitPromotionInfo == null)
                return PromptView("套装促销活动不存在");

            DateTime? nullTime = null;
            DateTime noTime = new DateTime(1900, 1, 1);

            SuitPromotionModel model = new SuitPromotionModel();
            model.StartTime1 = suitPromotionInfo.StartTime1;
            model.EndTime1 = suitPromotionInfo.EndTime1;
            model.StartTime2 = suitPromotionInfo.StartTime2 == noTime ? nullTime : suitPromotionInfo.StartTime2;
            model.EndTime2 = suitPromotionInfo.EndTime2 == noTime ? nullTime : suitPromotionInfo.EndTime2;
            model.StartTime3 = suitPromotionInfo.StartTime3 == noTime ? nullTime : suitPromotionInfo.StartTime3;
            model.EndTime3 = suitPromotionInfo.EndTime3 == noTime ? nullTime : suitPromotionInfo.EndTime3;
            model.UserRankLower = suitPromotionInfo.UserRankLower;
            model.State = suitPromotionInfo.State;
            model.PromotionName = suitPromotionInfo.Name;
            model.QuotaUpper = suitPromotionInfo.QuotaUpper;
            model.OnlyOnce = suitPromotionInfo.OnlyOnce;

            model.StoreId = suitPromotionInfo.StoreId;
            model.StoreName = AdminStores.GetStoreById(suitPromotionInfo.StoreId).Name;

            LoadSuitPromotion();

            return View(model);
        }

        /// <summary>
        /// 编辑套装促销活动
        /// </summary>
        /// <param name="model">活动模型</param>
        /// <param name="pmId">活动id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditSuitPromotion(SuitPromotionModel model, int pmId = -1)
        {
            SuitPromotionInfo suitPromotionInfo = AdminPromotions.AdminGetSuitPromotionById(pmId);
            if (suitPromotionInfo == null)
                return PromptView("套装促销活动不存在");

            if (ModelState.IsValid)
            {
                DateTime noTime = new DateTime(1900, 1, 1);

                suitPromotionInfo.StartTime1 = model.StartTime1;
                suitPromotionInfo.EndTime1 = model.EndTime1;
                suitPromotionInfo.StartTime2 = model.StartTime2.HasValue ? model.StartTime2.Value : noTime;
                suitPromotionInfo.EndTime2 = model.EndTime2.HasValue ? model.EndTime2.Value : noTime;
                suitPromotionInfo.StartTime3 = model.StartTime3.HasValue ? model.StartTime3.Value : noTime;
                suitPromotionInfo.EndTime3 = model.EndTime3.HasValue ? model.EndTime3.Value : noTime;
                suitPromotionInfo.UserRankLower = model.UserRankLower;
                suitPromotionInfo.State = model.State;
                suitPromotionInfo.Name = model.PromotionName;
                suitPromotionInfo.QuotaUpper = model.QuotaUpper;
                suitPromotionInfo.OnlyOnce = model.OnlyOnce;

                AdminPromotions.UpdateSuitPromotion(suitPromotionInfo);
                AddMallAdminLog("修改套装促销活动", "修改套装促销活动,套装促销活动ID为:" + pmId);
                return PromptView("套装促销活动修改成功");
            }

            LoadSuitPromotion();
            return View(model);
        }

        /// <summary>
        /// 删除套装促销活动
        /// </summary>
        /// <param name="pmIdList">活动id</param>
        /// <returns></returns>
        public ActionResult DelSuitPromotion(int[] pmIdList)
        {
            AdminPromotions.DeleteSuitPromotionById(pmIdList);
            AddMallAdminLog("删除套装促销活动", "删除套装促销活动,套装促销活动ID为:" + CommonHelper.IntArrayToString(pmIdList));
            return PromptView("套装促销活动删除成功");
        }

        private void LoadSuitPromotion()
        {
            List<SelectListItem> userRankList = new List<SelectListItem>();
            foreach (UserRankInfo userRankInfo in AdminUserRanks.GetCustomerUserRankList())
            {
                userRankList.Add(new SelectListItem() { Text = userRankInfo.Title, Value = userRankInfo.UserRid.ToString() });
            }
            ViewData["userRankList"] = userRankList;

            ViewData["referer"] = MallUtils.GetMallAdminRefererCookie();
        }





        /// <summary>
        /// 套装商品列表
        /// </summary>
        /// <param name="pmId">促销活动id</param>
        /// <returns></returns>
        public ActionResult SuitProductList(int pmId = -1)
        {
            SuitPromotionInfo suitPromotionInfo = AdminPromotions.AdminGetSuitPromotionById(pmId);
            if (suitPromotionInfo == null)
                return PromptView("此套装促销活动不存在");

            SuitProductListModel model = new SuitProductListModel()
            {
                ExtSuitProductList = AdminPromotions.AdminGetExtSuitProductList(pmId),
                PmId = pmId,
                StoreId = suitPromotionInfo.StoreId
            };

            ViewData["referer"] = MallUtils.GetMallAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 添加套装商品
        /// </summary>
        [HttpPost]
        public ActionResult AddSuitProduct(int pmId = -1, int pid = -1, int discount = -1, int number = 1)
        {
            SuitPromotionInfo suitPromotionInfo = AdminPromotions.AdminGetSuitPromotionById(pmId);
            if (suitPromotionInfo == null)
                return PromptView("此套装促销活动不存在");

            PartProductInfo partProductInfo = AdminProducts.AdminGetPartProductById(pid);
            if (partProductInfo == null)
                return PromptView("此商品不存在");

            if (suitPromotionInfo.StoreId != partProductInfo.StoreId)
                return PromptView(Url.Action("suitproductlist", new { pmId = pmId }), "只能关联同一店铺的商品");

            if (AdminPromotions.IsExistSuitProduct(pmId, pid))
                return PromptView(Url.Action("suitproductlist", new { pmId = pmId }), "此套装商品已经存在");

            if (discount < 0)
                return PromptView(Url.Action("suitproductlist", new { pmId = pmId }), "折扣值不能小于0");

            if (number < 1)
                return PromptView(Url.Action("suitproductlist", new { pmId = pmId }), "数量必须大于0");

            AdminPromotions.AddSuitProduct(pmId, pid, discount, number);
            AddMallAdminLog("添加套装商品", "添加套装商品,套装商品为:" + partProductInfo.Name);
            return PromptView(Url.Action("suitproductlist", new { pmId = pmId }), "套装商品添加成功");
        }

        /// <summary>
        /// 修改套装商品的数量
        /// </summary>
        /// <param name="pmId">促销活动id</param>
        /// <param name="pid">商品id</param>
        /// <param name="number">数量</param>
        /// <returns></returns>
        public ActionResult UpdateSuitProductNumber(int pmId = -1, int pid = -1, int number = 1)
        {
            bool result = AdminPromotions.UpdateSuitProductNumber(pmId, pid, number);
            if (result)
            {
                AddMallAdminLog("修改套装商品数量", "修改套装商品数量,数量为:" + number);
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }

        /// <summary>
        /// 修改套装商品折扣
        /// </summary>
        /// <param name="pmId">促销活动id</param>
        /// <param name="pid">商品id</param>
        /// <param name="discount">折扣</param>
        /// <returns></returns>
        public ActionResult UpdateSuitProductDiscountValue(int pmId = -1, int pid = -1, int discount = 0)
        {
            bool result = AdminPromotions.UpdateSuitProductDiscount(pmId, pid, discount);
            if (result)
            {
                AddMallAdminLog("修改套装商品折扣", "修改套装商品折扣,折扣为:" + discount);
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }

        /// <summary>
        /// 删除套装商品
        /// </summary>
        /// <param name="pmId">促销活动id</param>
        /// <param name="pid">商品id</param>
        /// <returns></returns>
        public ActionResult DelSuitProduct(int pmId = -1, int pid = -1)
        {
            bool result = AdminPromotions.DeleteSuitProductByPmIdAndPid(pmId, pid);
            if (result)
            {
                AddMallAdminLog("删除套装商品", "删除套装商品,套装商品ID为" + pmId + "_" + pid);
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }






        /// <summary>
        /// 满赠促销活动列表
        /// </summary>
        public ActionResult FullSendPromotionList(string storeName, string promotionName, string promotionTime, int storeId = -1, int pageSize = 15, int pageNumber = 1)
        {
            string condition = AdminPromotions.AdminGetFullSendPromotionListCondition(storeId, promotionName, promotionTime);

            PageModel pageModel = new PageModel(pageSize, pageNumber, AdminPromotions.AdminGetFullSendPromotionCount(condition));

            FullSendPromotionListModel model = new FullSendPromotionListModel()
            {
                PageModel = pageModel,
                FullSendPromotionList = AdminPromotions.AdminGetFullSendPromotionList(pageModel.PageSize, pageModel.PageNumber, condition),
                StoreId = storeId,
                StoreName = string.IsNullOrWhiteSpace(storeName) ? "选择店铺" : storeName,
                PromotionName = promotionName,
                PromotionTime = promotionTime
            };

            MallUtils.SetAdminRefererCookie(string.Format("{0}?pageNumber={1}&pageSize={2}&storeId={3}&storeName={4}&promotionName={5}&promotionTime={6}",
                                                          Url.Action("fullsendpromotionlist"),
                                                          pageModel.PageNumber,
                                                          pageModel.PageSize,
                                                          storeId, storeName,
                                                          promotionName, promotionTime));
            return View(model);
        }

        /// <summary>
        /// 添加满赠促销活动
        /// </summary>
        [HttpGet]
        public ActionResult AddFullSendPromotion()
        {
            FullSendPromotionModel model = new FullSendPromotionModel();
            LoadFullSendPromotion();
            return View(model);
        }

        /// <summary>
        /// 添加满赠促销活动
        /// </summary>
        /// <param name="model">活动模型</param>
        [HttpPost]
        public ActionResult AddFullSendPromotion(FullSendPromotionModel model)
        {
            if (ModelState.IsValid)
            {
                FullSendPromotionInfo fullSendPromotionInfo = new FullSendPromotionInfo()
                {
                    StoreId = model.StoreId,
                    Name = model.PromotionName,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    UserRankLower = model.UserRankLower,
                    State = model.State,
                    LimitMoney = model.LimitMoney,
                    AddMoney = model.AddMoney
                };

                AdminPromotions.CreateFullSendPromotion(fullSendPromotionInfo);
                AddMallAdminLog("添加满赠促销活动", "添加满赠促销活动,满赠促销活动为:" + model.PromotionName);
                return PromptView("满赠促销活动添加成功");
            }

            LoadFullSendPromotion();
            return View(model);
        }

        /// <summary>
        /// 编辑满赠促销活动
        /// </summary>
        /// <param name="pmId">活动id</param>
        [HttpGet]
        public ActionResult EditFullSendPromotion(int pmId = -1)
        {
            FullSendPromotionInfo fullSendPromotionInfo = AdminPromotions.AdminGetFullSendPromotionById(pmId);
            if (fullSendPromotionInfo == null)
                return PromptView("满赠促销活动不存在");

            FullSendPromotionModel model = new FullSendPromotionModel();
            model.PromotionName = fullSendPromotionInfo.Name;
            model.StartTime = fullSendPromotionInfo.StartTime;
            model.EndTime = fullSendPromotionInfo.EndTime;
            model.UserRankLower = fullSendPromotionInfo.UserRankLower;
            model.State = fullSendPromotionInfo.State;
            model.LimitMoney = fullSendPromotionInfo.LimitMoney;
            model.AddMoney = fullSendPromotionInfo.AddMoney;

            model.StoreId = fullSendPromotionInfo.StoreId;
            model.StoreName = AdminStores.GetStoreById(fullSendPromotionInfo.StoreId).Name;

            LoadFullSendPromotion();

            return View(model);
        }

        /// <summary>
        /// 编辑满赠促销活动
        /// </summary>
        /// <param name="model">活动模型</param>
        /// <param name="pmId">活动id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditFullSendPromotion(FullSendPromotionModel model, int pmId = -1)
        {
            FullSendPromotionInfo fullSendPromotionInfo = AdminPromotions.AdminGetFullSendPromotionById(pmId);
            if (fullSendPromotionInfo == null)
                return PromptView("满赠促销活动不存在");

            if (ModelState.IsValid)
            {
                fullSendPromotionInfo.Name = model.PromotionName;
                fullSendPromotionInfo.StartTime = model.StartTime;
                fullSendPromotionInfo.EndTime = model.EndTime;
                fullSendPromotionInfo.UserRankLower = model.UserRankLower;
                fullSendPromotionInfo.State = model.State;
                fullSendPromotionInfo.LimitMoney = model.LimitMoney;
                fullSendPromotionInfo.AddMoney = model.AddMoney;

                AdminPromotions.UpdateFullSendPromotion(fullSendPromotionInfo);
                AddMallAdminLog("修改满赠促销活动", "修改满赠促销活动,满赠促销活动ID为:" + pmId);
                return PromptView("满赠促销活动修改成功");
            }

            LoadFullSendPromotion();
            return View(model);
        }

        /// <summary>
        /// 删除满赠促销活动
        /// </summary>
        /// <param name="pmIdList">活动id</param>
        public ActionResult DelFullSendPromotion(int[] pmIdList)
        {
            AdminPromotions.DeleteFullSendPromotionById(pmIdList);
            AddMallAdminLog("删除满赠促销活动", "删除满赠促销活动,满赠促销活动ID为:" + CommonHelper.IntArrayToString(pmIdList));
            return PromptView("满赠促销活动删除成功");
        }

        private void LoadFullSendPromotion()
        {
            List<SelectListItem> userRankList = new List<SelectListItem>();
            foreach (UserRankInfo userRankInfo in AdminUserRanks.GetCustomerUserRankList())
            {
                userRankList.Add(new SelectListItem() { Text = userRankInfo.Title, Value = userRankInfo.UserRid.ToString() });
            }
            ViewData["userRankList"] = userRankList;

            ViewData["referer"] = MallUtils.GetMallAdminRefererCookie();
        }






        /// <summary>
        /// 满赠商品列表
        /// </summary>
        /// <param name="pmId">活动id</param>
        public ActionResult FullSendProductList(int pmId = -1, int type = -1, int pageSize = 15, int pageNumber = 1)
        {
            FullSendPromotionInfo fullSendPromotionInfo = AdminPromotions.AdminGetFullSendPromotionById(pmId);
            if (fullSendPromotionInfo == null)
                return PromptView("满赠促销活动不存在");

            PageModel pageModel = new PageModel(pageSize, pageNumber, AdminPromotions.AdminGetFullSendProductCount(pmId, type));

            FullSendProductListModel model = new FullSendProductListModel()
            {
                PageModel = pageModel,
                FullSendProductList = AdminPromotions.AdminGetFullSendProductList(pageSize, pageNumber, pmId, type),
                PmId = pmId,
                StoreId = fullSendPromotionInfo.StoreId,
                Type = type
            };

            MallUtils.SetAdminRefererCookie(string.Format("{0}?pageNumber={1}&pageSize={2}&pmId={3}&type={4}",
                                                          Url.Action("fullsendproductlist"),
                                                          pageModel.PageNumber,
                                                          pageModel.PageSize,
                                                          pmId,
                                                          type));
            return View(model);
        }

        /// <summary>
        /// 添加满赠商品
        /// </summary>
        [HttpPost]
        public ActionResult AddFullSendProduct(int pmId = -1, int pid = -1, int type = 0)
        {
            FullSendPromotionInfo fullSendPromotionInfo = AdminPromotions.AdminGetFullSendPromotionById(pmId);
            if (fullSendPromotionInfo == null)
                return PromptView("此满赠促销不存在");

            PartProductInfo partProductInfo = AdminProducts.AdminGetPartProductById(pid);
            if (partProductInfo == null)
                return PromptView("此商品不存在");

            if (fullSendPromotionInfo.StoreId != partProductInfo.StoreId)
                return PromptView(Url.Action("fullsendproductlist", new { pmId = pmId, type = type }), "只能关联同一店铺的商品");

            if (Promotions.IsExistFullSendProduct(pmId, pid))
                return PromptView(Url.Action("fullsendproductlist", new { pmId = pmId, type = type }), "此商品已经存在");


            AdminPromotions.AddFullSendProduct(pmId, pid, type);
            AddMallAdminLog("添加满赠商品", "添加满赠商品,满赠商品为:" + partProductInfo.Name);
            return PromptView(Url.Action("fullsendproductlist", new { pmId = pmId, type = type }), "满赠商品添加成功");
        }

        /// <summary>
        /// 删除满赠商品
        /// </summary>
        /// <param name="recordId">记录id</param>
        /// <returns></returns>
        public ActionResult DelFullSendProduct(int recordId)
        {
            bool result = AdminPromotions.DeleteFullSendProductByRecordId(new[] { recordId });
            if (result)
            {
                AddMallAdminLog("删除满赠商品", "删除满赠商品,商品ID:" + recordId);
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }

        /// <summary>
        /// 删除满赠商品
        /// </summary>
        /// <param name="recordIdList">记录id</param>
        /// <returns></returns>
        public ActionResult DelFullSendProduct(int[] recordIdList)
        {
            AdminPromotions.DeleteFullSendProductByRecordId(recordIdList);
            AddMallAdminLog("删除满赠商品", "删除满赠商品,商品ID:" + CommonHelper.IntArrayToString(recordIdList));
            return PromptView("满赠商品删除成功");
        }






        /// <summary>
        /// 满减促销活动列表
        /// </summary>
        /// <returns></returns>
        public ActionResult FullCutPromotionList(string storeName, string promotionName, string promotionTime, int storeId = -1, int pageSize = 15, int pageNumber = 1)
        {
            string condition = AdminPromotions.AdminGetFullCutPromotionListCondition(storeId, promotionName, promotionTime);

            PageModel pageModel = new PageModel(pageSize, pageNumber, AdminPromotions.AdminGetFullCutPromotionCount(condition));

            FullCutPromotionListModel model = new FullCutPromotionListModel()
            {
                PageModel = pageModel,
                FullCutPromotionList = AdminPromotions.AdminGetFullCutPromotionList(pageModel.PageSize, pageModel.PageNumber, condition),
                StoreId = storeId,
                StoreName = string.IsNullOrWhiteSpace(storeName) ? "选择店铺" : storeName,
                PromotionName = promotionName,
                PromotionTime = promotionTime
            };

            MallUtils.SetAdminRefererCookie(string.Format("{0}?pageNumber={1}&pageSize={2}&storeId={3}&storeName={4}&promotionName={5}&promotionTime={6}",
                                                          Url.Action("fullcutpromotionlist"),
                                                          pageModel.PageNumber,
                                                          pageModel.PageSize,
                                                          storeId, storeName,
                                                          promotionName, promotionTime));
            return View(model);
        }

        /// <summary>
        /// 添加满减促销活动
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddFullCutPromotion()
        {
            FullCutPromotionModel model = new FullCutPromotionModel();
            LoadFullCutPromotion();
            return View(model);
        }

        /// <summary>
        /// 添加满减促销活动
        /// </summary>
        /// <param name="model">活动模型</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddFullCutPromotion(FullCutPromotionModel model)
        {
            if (ModelState.IsValid)
            {
                FullCutPromotionInfo fullCutPromotionInfo = new FullCutPromotionInfo()
                {
                    StoreId = model.StoreId,
                    Name = model.PromotionName,
                    Type = model.Type,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    UserRankLower = model.UserRankLower,
                    State = model.State,
                    LimitMoney1 = model.LimitMoney1,
                    CutMoney1 = model.CutMoney1,
                    LimitMoney2 = model.LimitMoney2,
                    CutMoney2 = model.CutMoney2,
                    LimitMoney3 = model.LimitMoney3,
                    CutMoney3 = model.CutMoney3
                };

                AdminPromotions.CreateFullCutPromotion(fullCutPromotionInfo);
                AddMallAdminLog("添加满减促销活动", "添加满减促销活动,满减促销活动为:" + model.PromotionName);
                return PromptView("满减促销活动添加成功");
            }

            LoadFullCutPromotion();
            return View(model);
        }

        /// <summary>
        /// 编辑满减促销活动
        /// </summary>
        /// <param name="pmId">活动id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditFullCutPromotion(int pmId = -1)
        {
            FullCutPromotionInfo fullCutPromotionInfo = AdminPromotions.AdminGetFullCutPromotionById(pmId);
            if (fullCutPromotionInfo == null)
                return PromptView("满减促销活动不存在");

            FullCutPromotionModel model = new FullCutPromotionModel();
            model.PromotionName = fullCutPromotionInfo.Name;
            model.Type = fullCutPromotionInfo.Type;
            model.StartTime = fullCutPromotionInfo.StartTime;
            model.EndTime = fullCutPromotionInfo.EndTime;
            model.UserRankLower = fullCutPromotionInfo.UserRankLower;
            model.State = fullCutPromotionInfo.State;
            model.LimitMoney1 = fullCutPromotionInfo.LimitMoney1;
            model.CutMoney1 = fullCutPromotionInfo.CutMoney1;
            model.LimitMoney2 = fullCutPromotionInfo.LimitMoney2;
            model.CutMoney2 = fullCutPromotionInfo.CutMoney2;
            model.LimitMoney3 = fullCutPromotionInfo.LimitMoney3;
            model.CutMoney3 = fullCutPromotionInfo.CutMoney3;

            model.StoreId = fullCutPromotionInfo.StoreId;
            model.StoreName = AdminStores.GetStoreById(fullCutPromotionInfo.StoreId).Name;

            LoadFullCutPromotion();

            return View(model);
        }

        /// <summary>
        /// 编辑满减促销活动
        /// </summary>
        /// <param name="model">活动模型</param>
        /// <param name="pmId">活动id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditFullCutPromotion(FullCutPromotionModel model, int pmId = -1)
        {
            FullCutPromotionInfo fullCutPromotionInfo = AdminPromotions.AdminGetFullCutPromotionById(pmId);
            if (fullCutPromotionInfo == null)
                return PromptView("满减促销不存在");

            if (ModelState.IsValid)
            {
                fullCutPromotionInfo.Name = model.PromotionName;
                fullCutPromotionInfo.Type = model.Type;
                fullCutPromotionInfo.StartTime = model.StartTime;
                fullCutPromotionInfo.EndTime = model.EndTime;
                fullCutPromotionInfo.UserRankLower = model.UserRankLower;
                fullCutPromotionInfo.State = model.State;
                fullCutPromotionInfo.LimitMoney1 = model.LimitMoney1;
                fullCutPromotionInfo.CutMoney1 = model.CutMoney1;
                fullCutPromotionInfo.LimitMoney2 = model.LimitMoney2;
                fullCutPromotionInfo.CutMoney2 = model.CutMoney2;
                fullCutPromotionInfo.LimitMoney3 = model.LimitMoney3;
                fullCutPromotionInfo.CutMoney3 = model.CutMoney3;

                AdminPromotions.UpdateFullCutPromotion(fullCutPromotionInfo);
                AddMallAdminLog("修改满减促销活动", "修改满减促销活动,满减促销活动ID为:" + pmId);
                return PromptView("满减促销活动修改成功");
            }

            LoadFullCutPromotion();
            return View(model);
        }

        /// <summary>
        /// 删除满减促销活动
        /// </summary>
        /// <param name="pmIdList">活动id</param>
        /// <returns></returns>
        public ActionResult DelFullCutPromotion(int[] pmIdList)
        {
            AdminPromotions.DeleteFullCutPromotionById(pmIdList);
            AddMallAdminLog("删除满减促销活动", "删除满减促销活动,满减促销活动ID为:" + CommonHelper.IntArrayToString(pmIdList));
            return PromptView("满减促销活动删除成功");
        }

        private void LoadFullCutPromotion()
        {
            List<SelectListItem> userRankList = new List<SelectListItem>();
            foreach (UserRankInfo userRankInfo in AdminUserRanks.GetCustomerUserRankList())
            {
                userRankList.Add(new SelectListItem() { Text = userRankInfo.Title, Value = userRankInfo.UserRid.ToString() });
            }
            ViewData["userRankList"] = userRankList;

            ViewData["referer"] = MallUtils.GetMallAdminRefererCookie();
        }






        /// <summary>
        /// 满减商品列表
        /// </summary>
        /// <param name="pmId">活动id</param>
        /// <returns></returns>
        public ActionResult FullCutProductList(int pmId = -1, int pageSize = 15, int pageNumber = 1)
        {
            FullCutPromotionInfo fullCutPromotionInfo = AdminPromotions.AdminGetFullCutPromotionById(pmId);
            if (fullCutPromotionInfo == null)
                return PromptView("此满减促销不存在");

            PageModel pageModel = new PageModel(pageSize, pageNumber, AdminPromotions.AdminGetFullCutProductCount(pmId));

            FullCutProductListModel model = new FullCutProductListModel()
            {
                PageModel = pageModel,
                FullCutProductList = AdminPromotions.AdminGetFullCutProductList(pageSize, pageNumber, pmId),
                PmId = pmId,
                StoreId = fullCutPromotionInfo.StoreId
            };

            MallUtils.SetAdminRefererCookie(string.Format("{0}?pageNumber={1}&pageSize={2}&pmId={3}",
                                                          Url.Action("fullcutproductlist"),
                                                          pageModel.PageNumber,
                                                          pageModel.PageSize,
                                                          pmId));
            return View(model);
        }

        /// <summary>
        /// 添加满减商品
        /// </summary>
        /// <param name="pmId">商品模型</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddFullCutProduct(int pmId = -1, int pid = -1, int type = 0)
        {
            FullCutPromotionInfo fullCutPromotionInfo = AdminPromotions.AdminGetFullCutPromotionById(pmId);
            if (fullCutPromotionInfo == null)
                return PromptView("此满减促销不存在");

            PartProductInfo partProductInfo = AdminProducts.AdminGetPartProductById(pid);
            if (partProductInfo == null)
                return PromptView("此商品不存在");

            if (fullCutPromotionInfo.StoreId != partProductInfo.StoreId)
                return PromptView(Url.Action("fullcutproductlist", new { pmId = pmId }), "只能关联同一店铺的商品");

            if (Promotions.IsExistFullCutProduct(pmId, pid))
                return PromptView(Url.Action("fullcutproductlist", new { pmId = pmId, type = type }), "此商品已经存在");


            AdminPromotions.AddFullCutProduct(pmId, pid);
            AddMallAdminLog("添加满减商品", "添加满减商品,满减商品为:" + partProductInfo.Name);
            return PromptView(Url.Action("fullcutproductlist", new { pmId = pmId, type = type }), "满减商品添加成功");
        }

        /// <summary>
        /// 删除满减商品
        /// </summary>
        /// <param name="recordId">记录id</param>
        /// <returns></returns>
        public ActionResult DelFullCutProduct(int recordId)
        {
            bool result = AdminPromotions.DeleteFullCutProductByRecordId(new[] { recordId });
            if (result)
            {
                AddMallAdminLog("删除满减商品", "删除满减商品,商品ID:" + recordId);
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }

        /// <summary>
        /// 删除满减商品
        /// </summary>
        /// <param name="recordIdList">记录id</param>
        /// <returns></returns>
        public ActionResult DelFullCutProduct(int[] recordIdList)
        {
            AdminPromotions.DeleteFullCutProductByRecordId(recordIdList);
            AddMallAdminLog("删除满减商品", "删除满减商品,商品ID:" + CommonHelper.IntArrayToString(recordIdList));
            return PromptView("满减商品删除成功");
        }
    }
}
