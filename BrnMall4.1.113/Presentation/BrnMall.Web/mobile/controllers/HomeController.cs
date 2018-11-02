using System;
using System.Web.Mvc;

using BrnMall.Core;
using BrnMall.Services;
using BrnMall.Web.Framework;

namespace BrnMall.Web.Mobile.Controllers
{
    /// <summary>
    /// 首页控制器类
    /// </summary>
    public partial class HomeController : BaseMobileController
    {
        /// <summary>
        /// 首页
        /// </summary>
        public ActionResult Index()
        {
            //首页的数据需要在其视图文件中直接调用，所以此处不再需要视图模型
            return View();
        }
    }
}
