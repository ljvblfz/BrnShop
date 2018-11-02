using System;
using System.Web;
using System.Web.Mvc;

using BrnMall.Core;
using BrnMall.Services;
using BrnMall.Web.Framework;
using BrnMall.Web.MallAdmin.Models;

namespace BrnMall.Web.MallAdmin.Controllers
{
    /// <summary>
    /// 商城后台店铺行业控制器类
    /// </summary>
    public partial class StoreIndustryController : BaseMallAdminController
    {
        /// <summary>
        /// 店铺行业列表
        /// </summary>
        public ActionResult List()
        {
            StoreIndustryListModel model = new StoreIndustryListModel()
            {
                StoreIndustryList = AdminStoreIndustries.GetStoreIndustryList()
            };

            MallUtils.SetAdminRefererCookie(Url.Action("list"));
            return View(model);
        }

        /// <summary>
        /// 添加店铺行业
        /// </summary>
        [HttpGet]
        public ActionResult Add()
        {
            StoreIndustryModel model = new StoreIndustryModel();
            ViewData["referer"] = MallUtils.GetMallAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 添加店铺行业
        /// </summary>
        [HttpPost]
        public ActionResult Add(StoreIndustryModel model)
        {
            if (AdminStoreIndustries.GetStoreIidByTitle(model.IndustryTitle) > 0)
                ModelState.AddModelError("IndustryTitle", "行业标题已经存在");

            if (ModelState.IsValid)
            {
                StoreIndustryInfo storeIndustryInfo = new StoreIndustryInfo()
                {
                    Title = model.IndustryTitle,
                    DisplayOrder = model.DisplayOrder
                };

                AdminStoreIndustries.CreateStoreIndustry(storeIndustryInfo);
                AddMallAdminLog("添加店铺行业", "添加店铺行业,店铺行业为:" + model.IndustryTitle);
                return PromptView("店铺行业添加成功");
            }
            ViewData["referer"] = MallUtils.GetMallAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 编辑店铺行业
        /// </summary>
        [HttpGet]
        public ActionResult Edit(int storeIid = -1)
        {
            StoreIndustryInfo storeIndustryInfo = AdminStoreIndustries.GetStoreIndustryById(storeIid);
            if (storeIndustryInfo == null)
                return PromptView("店铺行业不存在");

            StoreIndustryModel model = new StoreIndustryModel();
            model.IndustryTitle = storeIndustryInfo.Title;
            model.DisplayOrder = storeIndustryInfo.DisplayOrder;

            ViewData["referer"] = MallUtils.GetMallAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 编辑店铺行业
        /// </summary>
        [HttpPost]
        public ActionResult Edit(StoreIndustryModel model, int storeIid = -1)
        {
            StoreIndustryInfo storeIndustryInfo = AdminStoreIndustries.GetStoreIndustryById(storeIid);
            if (storeIndustryInfo == null)
                return PromptView("店铺行业不存在");

            int storeIid2 = AdminStoreIndustries.GetStoreIidByTitle(model.IndustryTitle);
            if (storeIid2 > 0 && storeIid2 != storeIid)
                ModelState.AddModelError("IndustryTitle", "行业标题已经存在");

            if (ModelState.IsValid)
            {
                storeIndustryInfo.Title = model.IndustryTitle;
                storeIndustryInfo.DisplayOrder = model.DisplayOrder;

                AdminStoreIndustries.UpdateStoreIndustry(storeIndustryInfo);
                AddMallAdminLog("修改店铺行业", "修改店铺行业,店铺行业ID为:" + storeIid);
                return PromptView("店铺行业修改成功");
            }

            ViewData["referer"] = MallUtils.GetMallAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 删除店铺行业
        /// </summary>
        public ActionResult Del(int storeIid)
        {
            int result = AdminStoreIndustries.DeleteStoreIndustryById(storeIid);
            if (result == -1)
                return PromptView("删除失败请先转移或删除此店铺行业下的店铺");

            AddMallAdminLog("删除店铺行业", "删除店铺行业,店铺行业ID为:" + storeIid);
            return PromptView("店铺行业删除成功");
        }
    }
}
