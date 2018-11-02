using System;
using System.Web;
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
    /// 后台配送公司控制器类
    /// </summary>
    public partial class ShipCompanyController : BaseAdminController
    {
        /// <summary>
        /// 配送公司列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            ShipCompanyListModel model = new ShipCompanyListModel()
            {
                ShipCompanyList = AdminShipCompanies.GetShipCompanyList()
            };
            ShopUtils.SetAdminRefererCookie(Url.Action("list"));
            return View(model);
        }

        /// <summary>
        /// 添加配送公司
        /// </summary>
        [HttpGet]
        public ActionResult Add()
        {
            ShipCompanyModel model = new ShipCompanyModel();
            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 添加配送公司
        /// </summary>
        [HttpPost]
        public ActionResult Add(ShipCompanyModel model)
        {
            if (AdminShipCompanies.GetShipCoIdByName(model.CompanyName) > 0)
                ModelState.AddModelError("CompanyName", "名称已经存在");

            if (ModelState.IsValid)
            {
                ShipCompanyInfo shipCompanyInfo = new ShipCompanyInfo()
                {
                    Name = model.CompanyName,
                    DisplayOrder = model.DisplayOrder
                };

                AdminShipCompanies.CreateShipCompany(shipCompanyInfo);
                AddAdminOperateLog("添加配送公司", "添加配送公司,配送公司为:" + model.CompanyName);
                return PromptView("配送公司添加成功");
            }
            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 编辑配送公司
        /// </summary>
        [HttpGet]
        public ActionResult Edit(int shipCoId = -1)
        {
            ShipCompanyInfo shipCompanyInfo = AdminShipCompanies.GetShipCompanyById(shipCoId);
            if (shipCompanyInfo == null)
                return PromptView("配送公司不存在");

            ShipCompanyModel model = new ShipCompanyModel();
            model.DisplayOrder = shipCompanyInfo.DisplayOrder;
            model.CompanyName = shipCompanyInfo.Name;
            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();

            return View(model);
        }

        /// <summary>
        /// 编辑配送公司
        /// </summary>
        [HttpPost]
        public ActionResult Edit(ShipCompanyModel model, int shipCoId = -1)
        {
            ShipCompanyInfo shipCompanyInfo = AdminShipCompanies.GetShipCompanyById(shipCoId);
            if (shipCompanyInfo == null)
                return PromptView("配送公司不存在");

            int shipCoId2 = AdminShipCompanies.GetShipCoIdByName(model.CompanyName);
            if (shipCoId2 > 0 && shipCoId2 != shipCoId)
                ModelState.AddModelError("CompanyName", "名称已经存在");

            if (ModelState.IsValid)
            {
                shipCompanyInfo.DisplayOrder = model.DisplayOrder;
                shipCompanyInfo.Name = model.CompanyName;

                AdminShipCompanies.UpdateShipCompany(shipCompanyInfo);
                AddAdminOperateLog("修改配送公司", "修改配送公司,配送公司ID为:" + shipCoId);
                return PromptView("配送公司修改成功");
            }

            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 删除配送公司
        /// </summary>
        public ActionResult Del(int shipCoId = -1)
        {
            AdminShipCompanies.DeleteShipCompanyById(shipCoId);
            AddAdminOperateLog("删除配送公司", "删除配送公司,配送公司ID为:" + shipCoId);
            return PromptView("配送公司删除成功");
        }
    }
}
