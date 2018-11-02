using System;

namespace BrnMall.Web.Framework
{
    /// <summary>
    /// PC商城后台视图页面基类型
    /// </summary>
    public abstract class MallAdminViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        public MallAdminWorkContext WorkContext;

        public sealed override void InitHelpers()
        {
            base.InitHelpers();
            Html.EnableClientValidation(true);//启用客户端验证
            Html.EnableUnobtrusiveJavaScript(true);//启用非侵入式脚本
            WorkContext = ((BaseMallAdminController)(this.ViewContext.Controller)).WorkContext;
        }

        public sealed override void Write(object value)
        {
            Output.Write(value);
        }
    }

    /// <summary>
    /// PC商城后台视图页面基类型
    /// </summary>
    public abstract class MallAdminViewPage : MallAdminViewPage<dynamic>
    {
    }
}
