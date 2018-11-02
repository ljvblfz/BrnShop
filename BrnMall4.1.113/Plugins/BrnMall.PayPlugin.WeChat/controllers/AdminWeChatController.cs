using System;
using System.Web;
using System.Web.Mvc;

using BrnMall.Core;
using BrnMall.Web.Framework;
using BrnMall.PayPlugin.WeChat;

namespace BrnMall.Web.MallAdmin.Controllers
{
    /// <summary>
    /// 商城后台微信支付控制器类
    /// </summary>
    public class AdminWeChatController : BaseMallAdminController
    {
        /// <summary>
        /// 配置
        /// </summary>
        [HttpGet]
        [ChildActionOnly]
        public ActionResult Config()
        {
            ConfigModel model = new ConfigModel();

            PluginSetInfo pluginSetInfo = PluginUtils.GetPluginSet();
            model.WPMchId = pluginSetInfo.WPMchId;
            model.WPAppId = pluginSetInfo.WPAppId;
            model.WPAppSecret = pluginSetInfo.WPAppSecret;
            model.WPAppKey = pluginSetInfo.WPAppKey;
            model.OpenMchId = pluginSetInfo.OpenMchId;
            model.OpenAppId = pluginSetInfo.OpenAppId;
            model.OpenAppSecret = pluginSetInfo.OpenAppSecret;
            model.OpenAppKey = pluginSetInfo.OpenAppKey;

            return View("~/plugins/BrnMall.PayPlugin.WeChat/views/adminwechat/config.cshtml", model);
        }

        /// <summary>
        /// 配置
        /// </summary>
        [HttpPost]
        public ActionResult Config(ConfigModel model)
        {
            if (ModelState.IsValid)
            {
                PluginSetInfo pluginSetInfo = new PluginSetInfo();
                pluginSetInfo.WPMchId = model.WPMchId.Trim();
                pluginSetInfo.WPAppId = model.WPAppId.Trim();
                pluginSetInfo.WPAppSecret = model.WPAppSecret.Trim();
                pluginSetInfo.WPAppKey = model.WPAppKey.Trim();
                pluginSetInfo.OpenMchId = model.OpenMchId.Trim();
                pluginSetInfo.OpenAppId = model.OpenAppId.Trim();
                pluginSetInfo.OpenAppSecret = model.OpenAppSecret.Trim();
                pluginSetInfo.OpenAppKey = model.OpenAppKey.Trim();
                PluginUtils.SavePluginSet(pluginSetInfo);

                AddMallAdminLog("修改微信支付插件配置信息");
                return PromptView(Url.Action("config", "plugin", new { configController = "AdminWeChat", configAction = "Config" }), "插件配置修改成功");
            }
            return PromptView(Url.Action("config", "plugin", new { configController = "AdminWeChat", configAction = "Config" }), "信息有误，请重新填写");
        }
    }
}
