using System;
using System.Web;
using System.Text;
using System.Web.Mvc;
using System.Collections.Generic;

using BrnMall.Core;
using BrnMall.Services;
using BrnMall.Web.Framework;

namespace BrnMall.Web.StoreAdmin.Controllers
{
    /// <summary>
    /// 店铺后台工具控制器类
    /// </summary>
    public partial class ToolController : Controller
    {
        private string ip = "";//ip地址
        private MallConfigInfo mallConfigInfo = BMAConfig.MallConfig;//商城配置信息
        private PartUserInfo partUserInfo = null;//用户信息
        private StoreInfo storeInfo = null;//店铺信息

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            ip = WebHelper.GetIP();
            //当用户ip不在允许的后台访问ip列表时
            if (!string.IsNullOrEmpty(mallConfigInfo.AdminAllowAccessIP) && !ValidateHelper.InIPList(ip, mallConfigInfo.AdminAllowAccessIP))
            {
                filterContext.Result = HttpNotFound();
                return;
            }
            //当用户IP被禁止时
            if (BannedIPs.CheckIP(ip))
            {
                filterContext.Result = HttpNotFound();
                return;
            }

            //获得用户id
            int uid = MallUtils.GetUidCookie();
            if (uid < 1)
                uid = WebHelper.GetRequestInt("uid");

            if (uid < 1)//当用户为游客时
            {
                //创建游客
                partUserInfo = Users.CreatePartGuest();
            }
            else//当用户为会员时
            {
                //获得保存在cookie中的密码
                string encryptPwd = MallUtils.GetCookiePassword();
                if (string.IsNullOrWhiteSpace(encryptPwd))
                    encryptPwd = WebHelper.GetRequestString("password");
                //防止用户密码被篡改为危险字符
                if (encryptPwd.Length == 0 || !SecureHelper.IsBase64String(encryptPwd))
                {
                    //创建游客
                    partUserInfo = Users.CreatePartGuest();
                    MallUtils.SetUidCookie(-1);
                    MallUtils.SetCookiePassword("");
                }
                else
                {
                    partUserInfo = Users.GetPartUserByUidAndPwd(uid, MallUtils.DecryptCookiePassword(encryptPwd));
                    if (partUserInfo == null)
                    {
                        partUserInfo = Users.CreatePartGuest();
                        MallUtils.SetUidCookie(-1);
                        MallUtils.SetCookiePassword("");
                    }
                }
            }

            //当用户等级是禁止访问等级时
            if (partUserInfo.UserRid == 1)
            {
                filterContext.Result = HttpNotFound();
                return;
            }

            //如果当前用户没有登录
            if (partUserInfo.Uid < 1)
            {
                filterContext.Result = HttpNotFound();
                return;
            }

            //店铺不存在或店铺关闭
            storeInfo = AdminStores.GetStoreById(partUserInfo.StoreId);
            if (storeInfo == null || storeInfo.State == (int)StoreState.Close)
            {
                filterContext.Result = HttpNotFound();
                return;
            }
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            string operation = WebHelper.GetQueryString("operation");

            if (operation == "uploadproductimage")//上传商品图片
            {
                HttpPostedFileBase file = Request.Files[0];
                string result = Uploads.SaveUplaodProductImage(storeInfo.StoreId, file);
                return Content(result);
            }
            if (operation == "uploadproducteditorimage")//上传商品编辑器中图片
            {
                HttpPostedFileBase file = Request.Files[0];
                string result = Uploads.SaveProductEditorImage(storeInfo.StoreId, file);
                return Content(string.Format("{3}'url':'upload/store/{0}/product/editor/{1}','state':'{2}','originalName':'','name':'','size':'','type':''{4}", storeInfo.StoreId, result, GetUEState(result), "{", "}"));
            }
            if (operation == "uploadstorebanner")//上传店铺banner
            {
                HttpPostedFileBase file = Request.Files[0];
                string result = Uploads.SaveUploadStoreBanner(storeInfo.StoreId, file);
                return Content(result);
            }
            if (operation == "uploadstorelogo")//上传店铺logo
            {
                HttpPostedFileBase file = Request.Files[0];
                string result = Uploads.SaveUploadStoreLogo(storeInfo.StoreId, file);
                return Content(result);
            }
            return HttpNotFound();
        }

        /// <summary>
        /// 省列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ProvinceList()
        {
            List<RegionInfo> regionList = Regions.GetProvinceList();

            StringBuilder sb = new StringBuilder();

            sb.Append("[");

            foreach (RegionInfo info in regionList)
            {
                sb.AppendFormat("{0}\"id\":\"{1}\",\"name\":\"{2}\"{3},", "{", info.RegionId, info.Name, "}");
            }

            if (regionList.Count > 0)
                sb.Remove(sb.Length - 1, 1);

            sb.Append("]");

            return Content(sb.ToString());
        }

        /// <summary>
        /// 市列表
        /// </summary>
        /// <param name="provinceId">省id</param>
        /// <returns></returns>
        public ActionResult CityList(int provinceId = -1)
        {
            List<RegionInfo> regionList = Regions.GetCityList(provinceId);

            StringBuilder sb = new StringBuilder();

            sb.Append("[");

            foreach (RegionInfo info in regionList)
            {
                sb.AppendFormat("{0}\"id\":\"{1}\",\"name\":\"{2}\"{3},", "{", info.RegionId, info.Name, "}");
            }

            if (regionList.Count > 0)
                sb.Remove(sb.Length - 1, 1);

            sb.Append("]");

            return Content(sb.ToString());
        }

        /// <summary>
        /// 县或区列表
        /// </summary>
        /// <param name="cityId">市id</param>
        /// <returns></returns>
        public ActionResult CountyList(int cityId = -1)
        {
            List<RegionInfo> regionList = Regions.GetCountyList(cityId);

            StringBuilder sb = new StringBuilder();

            sb.Append("[");

            foreach (RegionInfo info in regionList)
            {
                sb.AppendFormat("{0}\"id\":\"{1}\",\"name\":\"{2}\"{3},", "{", info.RegionId, info.Name, "}");
            }

            if (regionList.Count > 0)
                sb.Remove(sb.Length - 1, 1);

            sb.Append("]");

            return Content(sb.ToString());
        }

        /// <summary>
        /// 获得ueditor状态
        /// </summary>
        /// <param name="result">上传结果</param>
        /// <returns></returns>
        private string GetUEState(string result)
        {
            if (result == "-1")
            {
                return "上传图片不能为空";
            }
            else if (result == "-2")
            {
                return "不允许的图片类型";
            }
            else if (result == "-3")
            {
                return "图片大小超出网站限制";
            }
            else
            {
                return "SUCCESS";
            }
        }
    }
}
