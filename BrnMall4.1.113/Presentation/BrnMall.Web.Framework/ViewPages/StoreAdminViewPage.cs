using System;

namespace BrnMall.Web.Framework
{
    /// <summary>
    /// PC店铺后台视图页面基类型
    /// </summary>
    public abstract class StoreAdminViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        public StoreAdminWorkContext WorkContext;

        public sealed override void InitHelpers()
        {
            base.InitHelpers();
            Html.EnableClientValidation(true);//启用客户端验证
            Html.EnableUnobtrusiveJavaScript(true);//启用非侵入式脚本
            WorkContext = ((BaseStoreAdminController)(this.ViewContext.Controller)).WorkContext;
        }

        public sealed override void Write(object value)
        {
            Output.Write(value);
        }
    }

    /// <summary>
    /// PC店铺后台视图页面基类型
    /// </summary>
    public abstract class StoreAdminViewPage : StoreAdminViewPage<dynamic>
    {
    }
}
