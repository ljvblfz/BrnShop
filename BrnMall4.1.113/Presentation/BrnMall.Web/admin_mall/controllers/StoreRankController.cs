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
    /// 商城后台店铺等级控制器类
    /// </summary>
    public partial class StoreRankController : BaseMallAdminController
    {
        /// <summary>
        /// 店铺等级列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            StoreRankListModel model = new StoreRankListModel()
            {
                StoreRankList = AdminStoreRanks.GetStoreRankList()
            };
            MallUtils.SetAdminRefererCookie(Url.Action("list"));
            return View(model);
        }

        /// <summary>
        /// 添加店铺等级
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Add()
        {
            StoreRankModel model = new StoreRankModel();
            Load();
            return View(model);
        }

        /// <summary>
        /// 添加店铺等级
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(StoreRankModel model)
        {
            if (AdminStoreRanks.GetStoreRidByTitle(model.RankTitle) > 0)
                ModelState.AddModelError("RankTitle", "名称已经存在");

            if (ModelState.IsValid)
            {
                StoreRankInfo storeRankInfo = new StoreRankInfo()
                {
                    Title = model.RankTitle,
                    Avatar = model.Avatar ?? "",
                    HonestiesLower = model.HonestiesLower,
                    HonestiesUpper = model.HonestiesUpper,
                    ProductCount = model.ProductCount
                };

                AdminStoreRanks.CreateStoreRank(storeRankInfo);
                AddMallAdminLog("添加店铺等级", "添加店铺等级,店铺等级为:" + model.RankTitle);
                return PromptView("店铺等级添加成功");
            }
            Load();
            return View(model);
        }

        /// <summary>
        /// 编辑店铺等级
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int storeRid = -1)
        {
            StoreRankInfo storeRankInfo = AdminStoreRanks.GetStoreRankById(storeRid);
            if (storeRankInfo == null)
                return PromptView("店铺等级不存在");

            StoreRankModel model = new StoreRankModel();
            model.RankTitle = storeRankInfo.Title;
            model.Avatar = storeRankInfo.Avatar;
            model.HonestiesLower = storeRankInfo.HonestiesLower;
            model.HonestiesUpper = storeRankInfo.HonestiesUpper;
            model.ProductCount = storeRankInfo.ProductCount;

            Load();
            return View(model);
        }

        /// <summary>
        /// 编辑店铺等级
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(StoreRankModel model, int storeRid = -1)
        {
            StoreRankInfo storeRankInfo = AdminStoreRanks.GetStoreRankById(storeRid);
            if (storeRankInfo == null)
                return PromptView("店铺等级不存在");

            int storeRid2 = AdminStoreRanks.GetStoreRidByTitle(model.RankTitle);
            if (storeRid2 > 0 && storeRid2 != storeRid)
                ModelState.AddModelError("RankTitle", "名称已经存在");

            if (ModelState.IsValid)
            {
                storeRankInfo.Title = model.RankTitle;
                storeRankInfo.Avatar = model.Avatar ?? "";
                storeRankInfo.HonestiesLower = model.HonestiesLower;
                storeRankInfo.HonestiesUpper = model.HonestiesUpper;
                storeRankInfo.ProductCount = model.ProductCount;

                AdminStoreRanks.UpdateStoreRank(storeRankInfo);
                AddMallAdminLog("修改店铺等级", "修改店铺等级,店铺等级ID为:" + storeRid);
                return PromptView("店铺等级修改成功");
            }

            Load();
            return View(model);
        }

        /// <summary>
        /// 删除店铺等级
        /// </summary>
        /// <returns></returns>
        public ActionResult Del(int storeRid = -1)
        {
            int result = AdminStoreRanks.DeleteStoreRankById(storeRid);
            if (result == -1)
                return PromptView("删除失败请先转移或删除此店铺等级下的店铺");

            AddMallAdminLog("删除店铺等级", "删除店铺等级,店铺等级ID为:" + storeRid);
            return PromptView("店铺等级删除成功");
        }

        private void Load()
        {
            ViewData["allowImgType"] = BMAConfig.UploadConfig.UploadImgType.Replace(".", "");
            ViewData["maxImgSize"] = BMAConfig.UploadConfig.UploadImgSize;
            ViewData["referer"] = MallUtils.GetMallAdminRefererCookie();
        }
    }
}
