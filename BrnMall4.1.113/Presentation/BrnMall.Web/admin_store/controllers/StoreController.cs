using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;

using BrnMall.Core;
using BrnMall.Services;
using BrnMall.Web.Framework;
using BrnMall.Web.StoreAdmin.Models;

namespace BrnMall.Web.StoreAdmin.Controllers
{
    /// <summary>
    /// 店铺后台店铺控制器类
    /// </summary>
    public partial class StoreController : BaseStoreAdminController
    {
        /// <summary>
        /// 编辑店铺
        /// </summary>
        [HttpGet]
        public ActionResult EditStore()
        {
            StoreModel model = new StoreModel();

            model.StoreName = WorkContext.StoreInfo.Name;
            model.RegionId = WorkContext.StoreInfo.RegionId;
            model.StoreIid = WorkContext.StoreInfo.StoreIid;
            model.Logo = WorkContext.StoreInfo.Logo;
            model.Mobile = WorkContext.StoreInfo.Mobile;
            model.Phone = WorkContext.StoreInfo.Phone;
            model.QQ = WorkContext.StoreInfo.QQ;
            model.WW = WorkContext.StoreInfo.WW;
            model.Theme = WorkContext.StoreInfo.Theme;
            model.Banner = WorkContext.StoreInfo.Banner;
            model.Announcement = WorkContext.StoreInfo.Announcement;
            model.Description = WorkContext.StoreInfo.Description;

            LoadStore(model.RegionId);
            return View(model);
        }

        /// <summary>
        /// 编辑店铺
        /// </summary>
        [HttpPost]
        public ActionResult EditStore(StoreModel model)
        {
            int storeId = AdminStores.GetStoreIdByName(model.StoreName);
            if (storeId > 0 && storeId != WorkContext.StoreId)
                ModelState.AddModelError("StoreName", "店铺名已经存在");

            if (ModelState.IsValid)
            {
                WorkContext.StoreInfo.Name = model.StoreName;
                WorkContext.StoreInfo.RegionId = model.RegionId;
                WorkContext.StoreInfo.StoreIid = model.StoreIid;
                WorkContext.StoreInfo.Logo = model.Logo == null ? "" : model.Logo;
                WorkContext.StoreInfo.Mobile = model.Mobile == null ? "" : model.Mobile;
                WorkContext.StoreInfo.Phone = model.Phone == null ? "" : model.Phone;
                WorkContext.StoreInfo.QQ = model.QQ == null ? "" : model.QQ;
                WorkContext.StoreInfo.WW = model.WW == null ? "" : model.WW;
                WorkContext.StoreInfo.Theme = model.Theme;
                WorkContext.StoreInfo.Banner = model.Banner == null ? "" : model.Banner;
                WorkContext.StoreInfo.Announcement = model.Announcement == null ? "" : model.Announcement;
                WorkContext.StoreInfo.Description = model.Description == null ? "" : model.Description;

                AdminStores.UpdateStore(WorkContext.StoreInfo);
                AddStoreAdminLog("修改店铺", "修改店铺");
                return PromptView(Url.Action("editstore"), "店铺修改成功");
            }

            LoadStore(model.RegionId);
            return View(model);
        }

        private void LoadStore(int regionId)
        {
            List<SelectListItem> storeIndustryList = new List<SelectListItem>();
            storeIndustryList.Add(new SelectListItem() { Text = "选择店铺行业", Value = "-1" });
            foreach (StoreIndustryInfo storeIndustryInfo in AdminStoreIndustries.GetStoreIndustryList())
            {
                storeIndustryList.Add(new SelectListItem() { Text = storeIndustryInfo.Title, Value = storeIndustryInfo.StoreIid.ToString() });
            }
            ViewData["storeIndustryList"] = storeIndustryList;

            List<SelectListItem> themeList = new List<SelectListItem>();
            DirectoryInfo dir = new DirectoryInfo(IOHelper.GetMapPath("/themes"));
            foreach (DirectoryInfo themeDir in dir.GetDirectories())
            {
                themeList.Add(new SelectListItem() { Text = themeDir.Name, Value = themeDir.Name });
            }
            ViewData["themeList"] = themeList;

            RegionInfo regionInfo = Regions.GetRegionById(regionId);
            if (regionInfo != null)
            {
                ViewData["provinceId"] = regionInfo.ProvinceId;
                ViewData["cityId"] = regionInfo.CityId;
                ViewData["countyId"] = regionInfo.RegionId;
            }
            else
            {
                ViewData["provinceId"] = -1;
                ViewData["cityId"] = -1;
                ViewData["countyId"] = -1;
            }

            ViewData["allowImgType"] = BMAConfig.UploadConfig.UploadImgType.Replace(".", "");
            ViewData["maxImgSize"] = BMAConfig.UploadConfig.UploadImgSize;
        }





        /// <summary>
        /// 店铺分类列表
        /// </summary>
        public ActionResult StoreClassList()
        {
            StoreClassListModel model = new StoreClassListModel()
            {
                StoreClassList = AdminStores.GetStoreClassList(WorkContext.StoreId)
            };
            MallUtils.SetAdminRefererCookie(Url.Action("storeclasslist"));
            return View(model);
        }

        /// <summary>
        /// 添加店铺分类
        /// </summary>
        [HttpGet]
        public ViewResult AddStoreClass()
        {
            StoreClassModel model = new StoreClassModel();
            LoadStoreClass(WorkContext.StoreId);
            return View(model);
        }

        /// <summary>
        /// 添加店铺分类
        /// </summary>
        [HttpPost]
        public ActionResult AddStoreClass(StoreClassModel model)
        {
            if (AdminStores.GetStoreCidByStoreIdAndName(WorkContext.StoreId, model.StoreClassName) > 0)
                ModelState.AddModelError("StoreClassName", "名称已经存在");

            if (model.ParentId != 0 && AdminStores.GetStoreClassByStoreIdAndStoreCid(WorkContext.StoreId, model.ParentId) == null)
                ModelState.AddModelError("ParentId", "父分类不存在");

            if (ModelState.IsValid)
            {
                StoreClassInfo storeClassInfo = new StoreClassInfo()
                {
                    StoreId = WorkContext.StoreId,
                    DisplayOrder = model.DisplayOrder,
                    Name = model.StoreClassName,
                    ParentId = model.ParentId
                };

                AdminStores.CreateStoreClass(storeClassInfo);
                AddStoreAdminLog("添加店铺分类", "添加店铺分类,店铺分类为:" + model.StoreClassName);
                return PromptView("店铺分类添加成功");
            }

            LoadStoreClass(WorkContext.StoreId);
            return View(model);
        }

        /// <summary>
        /// 编辑店铺分类
        /// </summary>
        [HttpGet]
        public ActionResult EditStoreClass(int storeCid = -1)
        {
            StoreClassInfo storeClassInfo = AdminStores.GetStoreClassByStoreIdAndStoreCid(WorkContext.StoreId, storeCid);
            if (storeClassInfo == null)
                return PromptView("此店铺分类不存在");

            StoreClassModel model = new StoreClassModel();
            model.StoreClassName = storeClassInfo.Name;
            model.ParentId = storeClassInfo.ParentId;
            model.DisplayOrder = storeClassInfo.DisplayOrder;

            LoadStoreClass(WorkContext.StoreId);
            return View(model);
        }

        /// <summary>
        /// 编辑店铺分类
        /// </summary>
        [HttpPost]
        public ActionResult EditStoreClass(StoreClassModel model, int storeCid = -1)
        {
            StoreClassInfo storeClassInfo = AdminStores.GetStoreClassByStoreIdAndStoreCid(WorkContext.StoreId, storeCid);
            if (storeClassInfo == null)
                return PromptView("此店铺分类不存在");

            int storeCid2 = AdminStores.GetStoreCidByStoreIdAndName(WorkContext.StoreId, model.StoreClassName);
            if (storeCid2 > 0 && storeCid2 != storeCid)
                ModelState.AddModelError("StoreClassName", "名称已经存在");

            if (model.ParentId == storeClassInfo.StoreCid)
                ModelState.AddModelError("ParentId", "不能将自己作为父分类");

            if (model.ParentId != 0 && AdminStores.GetStoreClassByStoreIdAndStoreCid(WorkContext.StoreId, model.ParentId) == null)
                ModelState.AddModelError("ParentId", "父分类不存在");

            if (model.ParentId != 0 && AdminStores.GetChildStoreClassList(WorkContext.StoreId, storeCid, true).Exists(x => x.StoreCid == model.ParentId))
                ModelState.AddModelError("ParentId", "不能将分类调整到自己的子分类下");

            if (ModelState.IsValid)
            {
                int oldParentId = storeClassInfo.ParentId;

                storeClassInfo.ParentId = model.ParentId;
                storeClassInfo.Name = model.StoreClassName;
                storeClassInfo.DisplayOrder = model.DisplayOrder;

                AdminStores.UpdateStoreClass(storeClassInfo, oldParentId);
                AddStoreAdminLog("修改店铺分类", "修改店铺分类,店铺分类ID为:" + storeCid);
                return PromptView("商品修改成功");
            }

            LoadStoreClass(WorkContext.StoreId);
            return View(model);
        }

        /// <summary>
        /// 删除店铺分类
        /// </summary>
        public ActionResult DelStoreClass(int storeCid = -1)
        {
            int result = AdminStores.DeleteStoreClassByStoreIdAndStoreCid(WorkContext.StoreId, storeCid);
            if (result == -2)
                return PromptView("此店铺分类不存在");
            else if (result == -1)
                return PromptView("删除失败请先转移或删除此店铺分类下的店铺分类");
            else if (result == 0)
                return PromptView("删除失败请先转移或删除此店铺分类下的商品");
            AddStoreAdminLog("删除店铺分类", "删除店铺分类,店铺分类ID为:" + storeCid);
            return PromptView("店铺分类删除成功");
        }

        private void LoadStoreClass(int storeId)
        {
            ViewData["storeClassList"] = AdminStores.GetStoreClassList(storeId);
            ViewData["referer"] = MallUtils.GetStoreAdminRefererCookie();
        }





        /// <summary>
        /// 店铺配送模板列表
        /// </summary>
        public ActionResult StoreShipTemplateList()
        {
            StoreShipTemplateListModel model = new StoreShipTemplateListModel()
            {
                StoreShipTemplateList = AdminStores.GetStoreShipTemplateList(WorkContext.StoreId)
            };

            MallUtils.SetAdminRefererCookie(Url.Action("storeshiptemplatelist"));
            return View(model);
        }

        /// <summary>
        /// 添加店铺配送模板
        /// </summary>
        [HttpGet]
        public ActionResult AddStoreShipTemplate()
        {
            AddStoreShipTemplateModel model = new AddStoreShipTemplateModel();
            ViewData["referer"] = MallUtils.GetStoreAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 添加店铺配送模板
        /// </summary>
        [HttpPost]
        public ActionResult AddStoreShipTemplate(AddStoreShipTemplateModel model)
        {
            if (ModelState.IsValid)
            {
                StoreShipTemplateInfo storeShipTemplateInfo = new StoreShipTemplateInfo()
                {
                    StoreId = WorkContext.StoreId,
                    Title = model.TemplateTitle,
                    Free = model.Free,
                    Type = model.Type
                };

                int storeSTid = AdminStores.CreateStoreShipTemplate(storeShipTemplateInfo);
                if (storeSTid > 0)
                {
                    StoreShipFeeInfo storeShipFeeInfo = new StoreShipFeeInfo()
                    {
                        StoreSTid = storeSTid,
                        RegionId = -1,
                        StartValue = model.StartValue,
                        StartFee = model.StartFee,
                        AddValue = model.AddValue,
                        AddFee = model.AddFee
                    };
                    AdminStores.CreateStoreShipFee(storeShipFeeInfo);
                    AddStoreAdminLog("添加店铺配送模板", "添加店铺配送模板,店铺配送模板为:" + model.TemplateTitle);
                    return PromptView("店铺配送模板添加成功");
                }
                else
                {
                    return PromptView("店铺配送模板添加失败");
                }
            }
            ViewData["referer"] = MallUtils.GetStoreAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 编辑店铺配送模板
        /// </summary>
        [HttpGet]
        public ActionResult EditStoreShipTemplate(int storeSTid = -1)
        {
            StoreShipTemplateInfo storeShipTemplateInfo = AdminStores.GetStoreShipTemplateById(storeSTid);
            if (storeShipTemplateInfo == null)
                return PromptView("店铺配送模板不存在");
            if (storeShipTemplateInfo.StoreId != WorkContext.StoreId)
                return PromptView("不能操作其它店铺的配送模板");

            EditStoreShipTemplateModel model = new EditStoreShipTemplateModel();
            model.TemplateTitle = storeShipTemplateInfo.Title;
            model.Free = storeShipTemplateInfo.Free;
            model.Type = storeShipTemplateInfo.Type;

            ViewData["referer"] = MallUtils.GetStoreAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 编辑店铺配送模板
        /// </summary>
        [HttpPost]
        public ActionResult EditStoreShipTemplate(EditStoreShipTemplateModel model, int storeSTid = -1)
        {
            StoreShipTemplateInfo storeShipTemplateInfo = AdminStores.GetStoreShipTemplateById(storeSTid);
            if (storeShipTemplateInfo == null)
                return PromptView("店铺配送模板不存在");
            if (storeShipTemplateInfo.StoreId != WorkContext.StoreId)
                return PromptView("不能操作其它店铺的配送模板");

            if (ModelState.IsValid)
            {
                storeShipTemplateInfo.Title = model.TemplateTitle;
                storeShipTemplateInfo.Free = model.Free;
                storeShipTemplateInfo.Type = model.Type;

                AdminStores.UpdateStoreShipTemplate(storeShipTemplateInfo);
                AddStoreAdminLog("修改店铺配送模板", "修改店铺配送模板,店铺配送模板ID为:" + storeSTid);
                return PromptView("店铺配送模板修改成功");
            }

            ViewData["referer"] = MallUtils.GetStoreAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 删除店铺配送模板
        /// </summary>
        public ActionResult DelStoreShipTemplate(int storeSTid = -1)
        {
            StoreShipTemplateInfo storeShipTemplateInfo = AdminStores.GetStoreShipTemplateById(storeSTid);
            if (storeShipTemplateInfo == null)
                return PromptView("店铺配送模板不存在");
            if (storeShipTemplateInfo.StoreId != WorkContext.StoreId)
                return PromptView("不能操作其它店铺的配送模板");

            int result = AdminStores.DeleteStoreShipTemplateById(storeSTid);
            if (result == -1)
                return PromptView("请先移除此模板下的商品");
            AddStoreAdminLog("删除店铺配送模板", "删除店铺配送模板,店铺配送模板ID为:" + storeSTid);
            return PromptView("店铺配送模板删除成功");
        }





        /// <summary>
        /// 店铺配送费用列表
        /// </summary>
        public ActionResult StoreShipFeeList(int storeSTid = -1)
        {
            StoreShipTemplateInfo storeShipTemplateInfo = AdminStores.GetStoreShipTemplateById(storeSTid);
            if (storeShipTemplateInfo == null)
                return PromptView("店铺配送模板不存在");
            if (storeShipTemplateInfo.StoreId != WorkContext.StoreId)
                return PromptView("不能操作其它店铺的配送模板");

            StoreShipFeeListModel model = new StoreShipFeeListModel()
            {
                StoreSTid = storeSTid,
                StoreShipFeeList = AdminStores.AdminGetStoreShipFeeList(storeSTid)
            };

            MallUtils.SetAdminRefererCookie(string.Format("{0}?storeSTid={1}", Url.Action("storeshipfeelist"), storeSTid));
            return View(model);
        }

        /// <summary>
        /// 添加店铺配送费用
        /// </summary>
        [HttpGet]
        public ActionResult AddStoreShipFee(int storeSTid = -1)
        {
            StoreShipTemplateInfo storeShipTemplateInfo = AdminStores.GetStoreShipTemplateById(storeSTid);
            if (storeShipTemplateInfo == null)
                return PromptView("店铺配送模板不存在");
            if (storeShipTemplateInfo.StoreId != WorkContext.StoreId)
                return PromptView("不能操作其它店铺的配送模板");

            StoreShipFeeModel model = new StoreShipFeeModel();
            LoadStoreShipFee(0);
            return View(model);
        }

        /// <summary>
        /// 添加店铺配送费用
        /// </summary>
        [HttpPost]
        public ActionResult AddStoreShipFee(StoreShipFeeModel model, int storeSTid = -1)
        {
            StoreShipTemplateInfo storeShipTemplateInfo = AdminStores.GetStoreShipTemplateById(storeSTid);
            if (storeShipTemplateInfo == null)
                return PromptView("店铺配送模板不存在");
            if (storeShipTemplateInfo.StoreId != WorkContext.StoreId)
                return PromptView("不能操作其它店铺的配送模板");

            if (ModelState.IsValid)
            {
                StoreShipFeeInfo storeShipFeeInfo = new StoreShipFeeInfo()
                {
                    StoreSTid = storeSTid,
                    RegionId = model.RegionId,
                    StartValue = model.StartValue,
                    StartFee = model.StartFee,
                    AddValue = model.AddValue,
                    AddFee = model.AddFee
                };
                AdminStores.CreateStoreShipFee(storeShipFeeInfo);
                AddStoreAdminLog("添加店铺配送费用", "添加店铺配送费用");
                return PromptView("店铺配送费用添加成功");
            }
            LoadStoreShipFee(model.RegionId);
            return View(model);
        }

        /// <summary>
        /// 编辑店铺配送费用
        /// </summary>
        [HttpGet]
        public ActionResult EditStoreShipFee(int recordId = -1)
        {
            StoreShipFeeInfo storeShipFeeInfo = AdminStores.GetStoreShipFeeById(recordId);
            if (storeShipFeeInfo == null)
                return PromptView("店铺配送费用不存在");
            StoreShipTemplateInfo storeShipTemplateInfo = AdminStores.GetStoreShipTemplateById(storeShipFeeInfo.StoreSTid);
            if (storeShipTemplateInfo == null)
                return PromptView("店铺配送模板不存在");
            if (storeShipTemplateInfo.StoreId != WorkContext.StoreId)
                return PromptView("不能操作其它店铺的配送模板");

            StoreShipFeeModel model = new StoreShipFeeModel();
            model.RegionId = storeShipFeeInfo.RegionId;
            model.StartValue = storeShipFeeInfo.StartValue;
            model.StartFee = storeShipFeeInfo.StartFee;
            model.AddValue = storeShipFeeInfo.AddValue;
            model.AddFee = storeShipFeeInfo.AddFee;

            LoadStoreShipFee(model.RegionId);
            return View(model);
        }

        /// <summary>
        /// 编辑店铺配送费用
        /// </summary>
        [HttpPost]
        public ActionResult EditStoreShipFee(StoreShipFeeModel model, int recordId = -1)
        {
            StoreShipFeeInfo storeShipFeeInfo = AdminStores.GetStoreShipFeeById(recordId);
            if (storeShipFeeInfo == null)
                return PromptView("店铺配送费用不存在");
            StoreShipTemplateInfo storeShipTemplateInfo = AdminStores.GetStoreShipTemplateById(storeShipFeeInfo.StoreSTid);
            if (storeShipTemplateInfo == null)
                return PromptView("店铺配送模板不存在");
            if (storeShipTemplateInfo.StoreId != WorkContext.StoreId)
                return PromptView("不能操作其它店铺的配送模板");

            if (ModelState.IsValid)
            {
                storeShipFeeInfo.RegionId = model.RegionId;
                storeShipFeeInfo.StartValue = model.StartValue;
                storeShipFeeInfo.StartFee = model.StartFee;
                storeShipFeeInfo.AddValue = model.AddValue;
                storeShipFeeInfo.AddFee = model.AddFee;

                AdminStores.UpdateStoreShipFee(storeShipFeeInfo);
                AddStoreAdminLog("修改店铺配送费用", "修改店铺配送费用,店铺配送费用ID为:" + recordId);
                return PromptView("店铺配送费用修改成功");
            }

            LoadStoreShipFee(model.RegionId);
            return View(model);
        }

        /// <summary>
        /// 删除店铺配送费用
        /// </summary>
        public ActionResult DelStoreShipFee(int recordId = -1)
        {
            StoreShipFeeInfo storeShipFeeInfo = AdminStores.GetStoreShipFeeById(recordId);
            if (storeShipFeeInfo == null)
                return PromptView("店铺配送费用不存在");
            StoreShipTemplateInfo storeShipTemplateInfo = AdminStores.GetStoreShipTemplateById(storeShipFeeInfo.StoreSTid);
            if (storeShipTemplateInfo == null)
                return PromptView("店铺配送模板不存在");
            if (storeShipTemplateInfo.StoreId != WorkContext.StoreId)
                return PromptView("不能操作其它店铺的配送模板");
            if (storeShipFeeInfo.RegionId == -1)
                return PromptView("默认店铺配送费用不能删除");

            AdminStores.DeleteStoreShipFeeById(recordId);
            AddStoreAdminLog("删除店铺配送费用", "删除店铺配送费用,店铺配送费用ID为:" + recordId);
            return PromptView("店铺配送费用删除成功");
        }

        private void LoadStoreShipFee(int regionId)
        {
            RegionInfo regionInfo = Regions.GetRegionById(regionId);
            if (regionInfo != null)
            {
                if (regionInfo.Layer == 1)
                {
                    ViewData["provinceId"] = regionInfo.ProvinceId;
                    ViewData["cityId"] = 0;
                }
                else
                {
                    ViewData["provinceId"] = regionInfo.ProvinceId;
                    ViewData["cityId"] = regionInfo.RegionId;
                }
            }
            else
            {
                ViewData["provinceId"] = 0;
                ViewData["cityId"] = 0;
            }

            ViewData["referer"] = MallUtils.GetMallAdminRefererCookie();
        }
    }
}
