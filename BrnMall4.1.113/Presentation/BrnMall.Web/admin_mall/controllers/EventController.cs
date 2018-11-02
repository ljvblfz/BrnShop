using System;
using System.Web;
using System.Data;
using System.Web.Mvc;
using System.Threading;
using System.Collections.Generic;

using BrnMall.Core;
using BrnMall.Services;
using BrnMall.Web.Framework;
using BrnMall.Web.MallAdmin.Models;

namespace BrnMall.Web.MallAdmin.Controllers
{
    /// <summary>
    /// 商城后台事件控制器类
    /// </summary>
    public partial class EventController : BaseMallAdminController
    {
        /// <summary>
        /// 事件列表
        /// </summary>
        public ActionResult List()
        {
            EventListModel model = new EventListModel();

            model.BMAEventState = BMAConfig.EventConfig.BMAEventState;
            model.BMAEventList = BMAConfig.EventConfig.BMAEventList;
            model.BMAEventPeriod = BMAConfig.EventConfig.BMAEventPeriod;

            MallUtils.SetAdminRefererCookie(Url.Action("list"));

            return View(model);
        }

        /// <summary>
        /// 添加事件
        /// </summary>
        [HttpGet]
        public ActionResult Add()
        {
            EventModel model = new EventModel();
            ViewData["referer"] = MallUtils.GetMallAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 添加事件
        /// </summary>
        [HttpPost]
        public ActionResult Add(EventModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.Key) && BMAConfig.EventConfig.BMAEventList.Find(x => x.Key == model.Key.Trim().ToLower()) != null)
                ModelState.AddModelError("Key", "键已经存在");

            if (!string.IsNullOrWhiteSpace(model.Title) && BMAConfig.EventConfig.BMAEventList.Find(x => x.Title == model.Title.Trim().ToLower()) != null)
                ModelState.AddModelError("Title", "名称已经存在");

            if (ModelState.IsValid)
            {
                EventInfo eventInfo = new EventInfo()
                {
                    Key = model.Key.Trim().ToLower(),
                    Title = model.Title.Trim().ToLower(),
                    TimeType = model.TimeType,
                    TimeValue = model.TimeValue,
                    ClassName = model.ClassName,
                    Code = model.Code ?? "",
                    Enabled = model.Enabled
                };

                BMAConfig.EventConfig.BMAEventList.Add(eventInfo);
                BMAConfig.SaveEventConfig(BMAConfig.EventConfig);
                AddMallAdminLog("添加事件", "添加事件,事件为:" + model.Title);
                return PromptView("事件添加成功");
            }
            ViewData["referer"] = MallUtils.GetMallAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 编辑事件
        /// </summary>
        [HttpGet]
        public ActionResult Edit(string key = "")
        {
            EventInfo eventInfo = BMAConfig.EventConfig.BMAEventList.Find(x => x.Key == key);
            if (eventInfo == null)
                return PromptView("事件不存在");

            EventModel model = new EventModel();
            model.Key = eventInfo.Key;
            model.Title = eventInfo.Title;
            model.TimeType = eventInfo.TimeType;
            model.TimeValue = eventInfo.TimeValue;
            model.ClassName = eventInfo.ClassName;
            model.Code = eventInfo.Code;
            model.Enabled = eventInfo.Enabled;

            ViewData["referer"] = MallUtils.GetMallAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 编辑事件
        /// </summary>
        [HttpPost]
        public ActionResult Edit(EventModel model)
        {
            EventInfo eventInfo = null;

            if (!string.IsNullOrWhiteSpace(model.Key))
                eventInfo = BMAConfig.EventConfig.BMAEventList.Find(x => x.Key == model.Key);

            if (eventInfo == null)
                return PromptView("事件不存在");

            if (!string.IsNullOrWhiteSpace(model.Title))
            {
                EventInfo temp = BMAConfig.EventConfig.BMAEventList.Find(x => x.Title == model.Title.Trim().ToLower());
                if (temp != null && temp.Key != eventInfo.Key)
                    ModelState.AddModelError("Title", "名称已经存在");
            }

            if (ModelState.IsValid)
            {
                //eventInfo.Key = model.Key.Trim().ToLower(),
                eventInfo.Title = model.Title.Trim().ToLower();
                eventInfo.TimeType = model.TimeType;
                eventInfo.TimeValue = model.TimeValue;
                eventInfo.ClassName = model.ClassName;
                eventInfo.Code = model.Code ?? "";
                eventInfo.Enabled = model.Enabled;

                BMAConfig.SaveEventConfig(BMAConfig.EventConfig);
                AddMallAdminLog("编辑事件", "编辑事件,事件为:" + model.Title);
                return PromptView("事件编辑成功");
            }
            ViewData["referer"] = MallUtils.GetMallAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        public ActionResult Execute(string key = "")
        {
            EventInfo eventInfo = BMAConfig.EventConfig.BMAEventList.Find(x => x.Key == key);
            if (eventInfo == null)
                return PromptView("事件不存在");

            BMAEvent.Execute(eventInfo.Key);

            ViewData["referer"] = MallUtils.GetMallAdminRefererCookie();
            return PromptView("事件执行成功");
        }
    }
}
