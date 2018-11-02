using System;

namespace BrnMall.Web.MallAdmin
{
    public class AreaRegistration : System.Web.Mvc.AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "malladmin";
            }
        }

        public override void RegisterArea(System.Web.Mvc.AreaRegistrationContext context)
        {
            //此路由不能删除
            context.MapRoute("malladmin_default",
                              "malladmin/{controller}/{action}",
                              new { controller = "home", action = "index", area = "malladmin" },
                              new[] { "BrnMall.Web.MallAdmin.Controllers" });

        }
    }
}
