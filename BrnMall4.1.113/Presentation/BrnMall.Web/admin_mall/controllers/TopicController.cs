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
    /// 商城后台活动专题控制器类
    /// </summary>
    public partial class TopicController : BaseMallAdminController
    {
        /// <summary>
        /// 活动专题列表
        /// </summary>
        public ActionResult List(string topicSN, string topicTitle, string startTime, string endTime, int pageSize = 15, int pageNumber = 1)
        {
            string condition = AdminTopic.AdminGetTopicListCondition(topicSN, topicTitle, startTime, endTime);

            PageModel pageModel = new PageModel(pageSize, pageNumber, AdminTopic.AdminGetTopicCount(condition));

            TopicListModel model = new TopicListModel()
            {
                PageModel = pageModel,
                TopicList = AdminTopic.AdminGetTopicList(pageModel.PageSize, pageModel.PageNumber, condition),
                TopicTitle = topicTitle,
                StartTime = startTime,
                EndTime = endTime
            };
            MallUtils.SetAdminRefererCookie(string.Format("{0}?pageNumber={1}&pageSize={2}&topicSN={3}&topicTitle={4}&startTime={5}&endTime={6}",
                                                          Url.Action("list"),
                                                          pageModel.PageNumber,
                                                          pageModel.PageSize,
                                                          topicSN,
                                                          topicTitle,
                                                          startTime,
                                                          endTime));
            return View(model);
        }

        /// <summary>
        /// 添加活动专题
        /// </summary>
        [HttpGet]
        public ActionResult Add()
        {
            TopicModel model = new TopicModel();
            ViewData["referer"] = MallUtils.GetMallAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 添加活动专题
        /// </summary>
        [HttpPost]
        public ActionResult Add(TopicModel model)
        {
            if (ModelState.IsValid)
            {
                string sn = AdminTopic.GenerateTopicSN();
                TopicInfo topicInfo = new TopicInfo()
                {
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    SN = sn,
                    Title = model.Title,
                    HeadHtml = model.HeadHtml ?? "",
                    BodyHtml = model.BodyHtml ?? "",
                    IsShow = model.IsShow
                };

                AdminTopic.CreateTopic(topicInfo);
                AddMallAdminLog("添加活动专题", "添加活动专题,活动专题为:" + model.Title);
                return PromptView("活动专题添加成功");
            }

            ViewData["referer"] = MallUtils.GetMallAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 编辑活动专题
        /// </summary>
        [HttpGet]
        public ActionResult Edit(int topicId = -1)
        {
            TopicInfo topicInfo = AdminTopic.AdminGetTopicById(topicId);
            if (topicInfo == null)
                return PromptView("活动专题不存在");

            TopicModel model = new TopicModel();
            model.StartTime = topicInfo.StartTime;
            model.EndTime = topicInfo.EndTime;
            model.Title = topicInfo.Title;
            model.HeadHtml = topicInfo.HeadHtml;
            model.BodyHtml = topicInfo.BodyHtml;
            model.IsShow = topicInfo.IsShow;

            ViewData["referer"] = MallUtils.GetMallAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 编辑活动专题
        /// </summary>
        [HttpPost]
        public ActionResult Edit(TopicModel model, int topicId = -1)
        {
            TopicInfo topicInfo = AdminTopic.AdminGetTopicById(topicId);
            if (topicInfo == null)
                return PromptView("活动专题不存在");

            if (ModelState.IsValid)
            {
                topicInfo.StartTime = model.StartTime;
                topicInfo.EndTime = model.EndTime;
                topicInfo.Title = model.Title;
                topicInfo.HeadHtml = model.HeadHtml ?? "";
                topicInfo.BodyHtml = model.BodyHtml ?? "";
                topicInfo.IsShow = model.IsShow;

                AdminTopic.UpdateTopic(topicInfo);
                AddMallAdminLog("修改活动专题", "修改活动专题,活动专题ID为:" + topicId);
                return PromptView("活动专题修改成功");
            }

            ViewData["referer"] = MallUtils.GetMallAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 删除活动专题
        /// </summary>
        public ActionResult Del(int topicId)
        {
            AdminTopic.DeleteTopicById(topicId);
            AddMallAdminLog("删除活动专题", "删除活动专题,活动专题ID为:" + topicId);
            return PromptView("活动专题删除成功");
        }
    }
}
