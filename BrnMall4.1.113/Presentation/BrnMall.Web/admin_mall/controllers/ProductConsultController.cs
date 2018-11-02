using System;
using System.Web.Mvc;
using System.Collections.Generic;

using BrnMall.Core;
using BrnMall.Services;
using BrnMall.Web.Framework;
using BrnMall.Web.MallAdmin.Models;

namespace BrnMall.Web.MallAdmin.Controllers
{
    /// <summary>
    /// 商城后台商品咨询控制器类
    /// </summary>
    public partial class ProductConsultController : BaseMallAdminController
    {
        /// <summary>
        /// 商品咨询类型列表
        /// </summary>
        public ActionResult ProductConsultTypeList()
        {
            ProductConsultTypeListModel model = new ProductConsultTypeListModel()
            {
                ProductConsultTypeList = AdminProductConsults.GetProductConsultTypeList()
            };
            MallUtils.SetAdminRefererCookie(Url.Action("productconsulttypelist"));
            return View(model);
        }

        /// <summary>
        /// 添加商品咨询类型
        /// </summary>
        [HttpGet]
        public ActionResult AddProductConsultType()
        {
            ProductConsultTypeModel model = new ProductConsultTypeModel();
            ViewData["referer"] = MallUtils.GetMallAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 添加商品咨询类型
        /// </summary>
        [HttpPost]
        public ActionResult AddProductConsultType(ProductConsultTypeModel model)
        {
            if (ModelState.IsValid)
            {
                ProductConsultTypeInfo productConsultTypeInfo = new ProductConsultTypeInfo()
                {
                    Title = model.Title,
                    DisplayOrder = model.DisplayOrder
                };

                AdminProductConsults.CreateProductConsultType(productConsultTypeInfo);
                AddMallAdminLog("添加商品咨询类型", "添加商品咨询类型,商品咨询类型为:" + model.Title.Trim());
                return PromptView("商品咨询类型添加成功");
            }
            ViewData["referer"] = MallUtils.GetMallAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 编辑商品咨询类型
        /// </summary>
        [HttpGet]
        public ActionResult EditProductConsultType(int consultTypeId = -1)
        {
            ProductConsultTypeInfo productConsultTypeInfo = AdminProductConsults.GetProductConsultTypeById(consultTypeId);
            if (productConsultTypeInfo == null)
                return PromptView("商品咨询类型不存在");

            ProductConsultTypeModel model = new ProductConsultTypeModel();
            model.Title = productConsultTypeInfo.Title;
            model.DisplayOrder = productConsultTypeInfo.DisplayOrder;
            ViewData["referer"] = MallUtils.GetMallAdminRefererCookie();

            return View(model);
        }

        /// <summary>
        /// 编辑商品咨询类型
        /// </summary>
        [HttpPost]
        public ActionResult EditProductConsultType(ProductConsultTypeModel model, int consultTypeId = -1)
        {
            ProductConsultTypeInfo productConsultTypeInfo = AdminProductConsults.GetProductConsultTypeById(consultTypeId);
            if (productConsultTypeInfo == null)
                return PromptView("商品咨询类型不存在");

            if (ModelState.IsValid)
            {
                productConsultTypeInfo.Title = model.Title;
                productConsultTypeInfo.DisplayOrder = model.DisplayOrder;

                AdminProductConsults.UpdateProductConsultType(productConsultTypeInfo);
                AddMallAdminLog("修改商品咨询类型", "修改商品咨询类型,商品咨询类型ID为:" + consultTypeId);
                return PromptView("商品咨询类型修改成功");
            }

            ViewData["referer"] = MallUtils.GetMallAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 删除商品咨询类型
        /// </summary>
        public ActionResult DelProductConsultType(int consultTypeId = -1)
        {
            int result = AdminProductConsults.DeleteProductConsultTypeById(consultTypeId);
            if (result == 0)
                return PromptView("删除失败,请先删除此商品咨询类型下的咨询");

            AddMallAdminLog("删除商品咨询类型", "删除商品咨询类型,商品咨询类型ID为:" + consultTypeId);
            return PromptView("商品咨询类型删除成功");
        }







        /// <summary>
        /// 商品咨询列表
        /// </summary>
        public ActionResult ProductConsultList(string storeName, string consultMessage, string consultStartTime, string consultEndTime, int storeId = -1, int pid = 0, int pageNumber = 1, int pageSize = 15)
        {
            string condition = AdminProductConsults.AdminGetProductConsultListCondition(0, storeId, pid, 0, consultMessage, consultStartTime, consultEndTime);

            PageModel pageModel = new PageModel(pageSize, pageNumber, AdminProductConsults.AdminGetProductConsultCount(condition));
            ProductConsultListModel model = new ProductConsultListModel()
            {
                PageModel = pageModel,
                ProductConsultList = AdminProductConsults.AdminGetProductConsultList(pageModel.PageSize, pageModel.PageNumber, condition),
                StoreId = storeId,
                StoreName = string.IsNullOrWhiteSpace(storeName) ? "全部店铺" : storeName,
                Pid = pid,
                ConsultMessage = consultMessage,
                ConsultStartTime = consultStartTime,
                ConsultEndTime = consultEndTime
            };

            MallUtils.SetAdminRefererCookie(string.Format("{0}?pageNumber={1}&pageSize={2}&storeId={3}&storeName={4}&pid={5}&consultMessage={6}&consultStartTime={7}&consultEndTime={8}",
                                                           Url.Action("productconsultlist"),
                                                           pageModel.PageNumber, pageModel.PageSize,
                                                           storeId, storeName, pid, consultMessage,
                                                           consultStartTime, consultEndTime));
            return View(model);
        }

        /// <summary>
        /// 更新商品咨询状态
        /// </summary>
        /// <param name="consultId">商品咨询id</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public ActionResult UpdateProductConsultState(int consultId = -1, int state = -1)
        {
            bool result = AdminProductConsults.UpdateProductConsultState(consultId, state);
            if (result)
            {
                AddMallAdminLog("更新商品咨询状态", "更新商品咨询状态,咨询ID和状态为:" + consultId + "_" + state);
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }

        /// <summary>
        /// 回复商品咨询
        /// </summary>
        [HttpGet]
        public ActionResult Reply(int consultId = -1)
        {
            ProductConsultInfo productConsultInfo = AdminProductConsults.AdminGetProductConsultById(consultId);
            if (productConsultInfo == null)
                return PromptView("商品咨询不存在");

            ReplyProductConsultModel model = new ReplyProductConsultModel();
            model.ReplyMessage = productConsultInfo.ReplyMessage;

            ViewData["productConsultInfo"] = productConsultInfo;
            ViewData["referer"] = MallUtils.GetMallAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 回复商品咨询
        /// </summary>
        [HttpPost]
        public ActionResult Reply(ReplyProductConsultModel model, int consultId = -1)
        {
            ProductConsultInfo productConsultInfo = AdminProductConsults.AdminGetProductConsultById(consultId);
            if (productConsultInfo == null)
                return PromptView("商品咨询不存在");

            if (ModelState.IsValid)
            {
                AdminProductConsults.ReplyProductConsult(consultId, WorkContext.Uid, DateTime.Now, model.ReplyMessage, WorkContext.NickName, WorkContext.IP);
                AddMallAdminLog("回复商品咨询", "回复商品咨询,商品咨询为:" + consultId);
                return PromptView("商品咨询回复成功");
            }

            ViewData["productConsultInfo"] = productConsultInfo;
            ViewData["referer"] = MallUtils.GetMallAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 删除商品咨询
        /// </summary>
        /// <param name="consultIdList">商品咨询id列表</param>
        /// <returns></returns>
        public ActionResult DelProductConsult(int[] consultIdList)
        {
            AdminProductConsults.DeleteProductConsultById(consultIdList);
            AddMallAdminLog("删除商品咨询", "删除商品咨询,商品咨询ID为:" + CommonHelper.IntArrayToString(consultIdList));
            return PromptView("商品咨询删除成功");
        }
    }
}
