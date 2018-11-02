using System;
using System.Web;

using BrnMall.Core;

namespace BrnMall.Services
{
    /// <summary>
    /// 上传操作管理类
    /// </summary>
    public partial class Uploads
    {
        private static IUploadStrategy _iuploadstrategy = BMAUpload.Instance;//上传策略

        /// <summary>
        /// 保存上传的用户头像
        /// </summary>
        /// <param name="avatar">用户头像</param>
        /// <returns></returns>
        public static string SaveUploadUserAvatar(HttpPostedFileBase avatar)
        {
            return _iuploadstrategy.SaveUploadUserAvatar(avatar);
        }

        /// <summary>
        /// 保存上传的用户等级头像
        /// </summary>
        /// <param name="avatar">用户等级头像</param>
        /// <returns></returns>
        public static string SaveUploadUserRankAvatar(HttpPostedFileBase avatar)
        {
            return _iuploadstrategy.SaveUploadUserRankAvatar(avatar);
        }

        /// <summary>
        /// 保存上传的品牌logo
        /// </summary>
        /// <param name="logo">品牌logo</param>
        /// <returns></returns>
        public static string SaveUploadBrandLogo(HttpPostedFileBase logo)
        {
            return _iuploadstrategy.SaveUploadBrandLogo(logo);
        }

        /// <summary>
        /// 保存新闻编辑器中的图片
        /// </summary>
        /// <param name="image">新闻图片</param>
        /// <returns></returns>
        public static string SaveNewsEditorImage(HttpPostedFileBase image)
        {
            return _iuploadstrategy.SaveNewsEditorImage(image);
        }

        /// <summary>
        /// 保存帮助编辑器中的图片
        /// </summary>
        /// <param name="image">帮助图片</param>
        /// <returns></returns>
        public static string SaveHelpEditorImage(HttpPostedFileBase image)
        {
            return _iuploadstrategy.SaveHelpEditorImage(image);
        }

        /// <summary>
        /// 保存商品编辑器中的图片
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="image">商品图片</param>
        /// <returns></returns>
        public static string SaveProductEditorImage(int storeId, HttpPostedFileBase image)
        {
            return _iuploadstrategy.SaveProductEditorImage(storeId, image);
        }

        /// <summary>
        /// 保存上传的商品图片
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="image">商品图片</param>
        /// <returns></returns>
        public static string SaveUplaodProductImage(int storeId, HttpPostedFileBase image)
        {
            return _iuploadstrategy.SaveUplaodProductImage(storeId, image);
        }

        /// <summary>
        /// 保存上传的广告图片
        /// </summary>
        /// <param name="image">广告图片</param>
        /// <returns></returns>
        public static string SaveUploadAdvertImage(HttpPostedFileBase image)
        {
            return _iuploadstrategy.SaveUploadAdvertImage(image);
        }

        /// <summary>
        /// 保存上传的友情链接Logo
        /// </summary>
        /// <param name="logo">友情链接logo</param>
        /// <returns></returns>
        public static string SaveUploadFriendLinkLogo(HttpPostedFileBase logo)
        {
            return _iuploadstrategy.SaveUploadFriendLinkLogo(logo);
        }

        /// <summary>
        /// 保存上传的店铺等级头像
        /// </summary>
        /// <param name="avatar">店铺等级头像</param>
        /// <returns></returns>
        public static string SaveUploadStoreRankAvatar(HttpPostedFileBase avatar)
        {
            return _iuploadstrategy.SaveUploadStoreRankAvatar(avatar);
        }

        /// <summary>
        /// 保存上传的店铺logo
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="logo">店铺logo</param>
        /// <returns></returns>
        public static string SaveUploadStoreLogo(int storeId, HttpPostedFileBase logo)
        {
            return _iuploadstrategy.SaveUploadStoreLogo(storeId, logo);
        }

        /// <summary>
        /// 保存上传的店铺banner
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="banner">店铺banner</param>
        /// <returns></returns>
        public static string SaveUploadStoreBanner(int storeId, HttpPostedFileBase banner)
        {
            return _iuploadstrategy.SaveUploadStoreBanner(storeId, banner);
        }
    }
}
