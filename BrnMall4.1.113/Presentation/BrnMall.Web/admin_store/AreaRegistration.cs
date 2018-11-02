using System;

namespace BrnMall.Web.StoreAdmin
{
    public class AreaRegistration : System.Web.Mvc.AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "storeadmin";
            }
        }

        public override void RegisterArea(System.Web.Mvc.AreaRegistrationContext context)
        {
            //此路由不能删除
            context.MapRoute("storeadmin_default",
                              "storeadmin/{controller}/{action}",
                              new { controller = "home", action = "index", area = "storeadmin" },
                              new[] { "BrnMall.Web.StoreAdmin.Controllers" });

        }
    }
}
