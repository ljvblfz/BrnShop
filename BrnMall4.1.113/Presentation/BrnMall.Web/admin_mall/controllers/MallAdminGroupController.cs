using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;

using BrnMall.Core;
using BrnMall.Services;
using BrnMall.Web.Framework;
using BrnMall.Web.MallAdmin.Models;

namespace BrnMall.Web.MallAdmin.Controllers
{
    /// <summary>
    /// 商城后台商城管理员组控制器类
    /// </summary>
    public partial class MallAdminGroupController : BaseMallAdminController
    {
        /// <summary>
        /// 商城管理员组列表
        /// </summary>
        public ActionResult List()
        {
            MallAdminGroupListModel model = new MallAdminGroupListModel()
            {
                MallAdminGroupList = MallAdminGroups.GetCustomerMallAdminGroupList()
            };
            MallUtils.SetAdminRefererCookie(Url.Action("list"));
            return View(model);
        }

        /// <summary>
        /// 添加商城管理员组
        /// </summary>
        [HttpGet]
        public ActionResult Add()
        {
            MallAdminGroupModel model = new MallAdminGroupModel();
            Load();
            return View(model);
        }

        /// <summary>
        /// 添加商城管理员组
        /// </summary>
        [HttpPost]
        public ActionResult Add(MallAdminGroupModel model)
        {
            if (MallAdminGroups.GetMallAdminGroupIdByTitle(model.AdminGroupTitle) > 0)
                ModelState.AddModelError("AdminGroupTitle", "名称已经存在");

            if (ModelState.IsValid)
            {
                MallAdminGroupInfo mallAdminGroupInfo = new MallAdminGroupInfo()
                {
                    Title = model.AdminGroupTitle,
                    ActionList = CommonHelper.StringArrayToString(model.ActionList).ToLower()
                };

                MallAdminGroups.CreateMallAdminGroup(mallAdminGroupInfo);
                AddMallAdminLog("添加商城管理员组", "添加商城管理员组,商城管理员组为:" + model.AdminGroupTitle);
                return PromptView("商城管理员组添加成功");
            }
            Load();
            return View(model);
        }

        /// <summary>
        /// 编辑商城管理员组
        /// </summary>
        [HttpGet]
        public ActionResult Edit(int mallAGid = -1)
        {
            if (mallAGid < 3)
                return PromptView("内置商城管理员组不能修改");

            MallAdminGroupInfo mallAdminGroupInfo = MallAdminGroups.GetMallAdminGroupById(mallAGid);
            if (mallAdminGroupInfo == null)
                return PromptView("商城管理员组不存在");

            MallAdminGroupModel model = new MallAdminGroupModel();
            model.AdminGroupTitle = mallAdminGroupInfo.Title;
            model.ActionList = StringHelper.SplitString(mallAdminGroupInfo.ActionList);

            Load();
            return View(model);
        }

        /// <summary>
        /// 编辑商城管理员组
        /// </summary>
        [HttpPost]
        public ActionResult Edit(MallAdminGroupModel model, int mallAGid = -1)
        {
            if (mallAGid < 3)
                return PromptView("内置商城管理员组不能修改");

            MallAdminGroupInfo mallAdminGroupInfo = MallAdminGroups.GetMallAdminGroupById(mallAGid);
            if (mallAdminGroupInfo == null)
                return PromptView("商城管理员组不存在");

            int mallAGid2 = MallAdminGroups.GetMallAdminGroupIdByTitle(model.AdminGroupTitle);
            if (mallAGid2 > 0 && mallAGid2 != mallAGid)
                ModelState.AddModelError("AdminGroupTitle", "名称已经存在");

            if (ModelState.IsValid)
            {
                mallAdminGroupInfo.Title = model.AdminGroupTitle;
                mallAdminGroupInfo.ActionList = CommonHelper.StringArrayToString(model.ActionList).ToLower();

                MallAdminGroups.UpdateMallAdminGroup(mallAdminGroupInfo);
                AddMallAdminLog("修改商城管理员组", "修改商城管理员组,商城管理员组ID为:" + mallAGid);
                return PromptView("商城管理员组修改成功");
            }

            Load();
            return View(model);
        }

        /// <summary>
        /// 删除商城管理员组
        /// </summary>
        public ActionResult Del(int mallAGid = -1)
        {
            int result = MallAdminGroups.DeleteMallAdminGroupById(mallAGid);
            if (result == -1)
                return PromptView("删除失败请先转移或删除此商城管理员组下的用户");
            else if (result == -2)
                return PromptView("内置商城管理员组不能删除");

            AddMallAdminLog("删除商城管理员组", "删除商城管理员组,商城管理员组ID为:" + mallAGid);
            return PromptView("商城管理员组删除成功");
        }

        private void Load()
        {
            ViewData["mallAdminActionTree"] = MallAdminActions.GetMallAdminActionTree();
            ViewData["referer"] = MallUtils.GetMallAdminRefererCookie();
        }
    }
}
