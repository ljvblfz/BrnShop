using System;

namespace BrnMall.Web.Framework
{
    /// <summary>
    /// App前台视图页面基类型
    /// </summary>
    public abstract class AppViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        public AppWorkContext WorkContext;

        public sealed override void InitHelpers()
        {
            base.InitHelpers();
            WorkContext = ((BaseAppController)(this.ViewContext.Controller)).WorkContext;
        }

        public sealed override void Write(object value)
        {
            Output.Write(value);
        }
    }

    /// <summary>
    /// App前台视图页面基类型
    /// </summary>
    public abstract class AppViewPage : AppViewPage<dynamic>
    {
    }
}
