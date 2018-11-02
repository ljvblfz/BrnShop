using System;
using System.Web;

namespace BrnMall.Core
{
    /// <summary>
    /// 上传策略接口
    /// </summary>
    public partial interface IUploadStrategy
    {
        /// <summary>
        /// 保存上传的用户头像
        /// </summary>
        /// <param name="avatar">用户头像</param>
        /// <returns></returns>
        string SaveUploadUserAvatar(HttpPostedFileBase avatar);

        /// <summary>
        /// 保存上传的用户等级头像
        /// </summary>
        /// <param name="avatar">用户等级头像</param>
        /// <returns></returns>
        string SaveUploadUserRankAvatar(HttpPostedFileBase avatar);

        /// <summary>
        /// 保存上传的品牌logo
        /// </summary>
        /// <param name="logo">品牌logo</param>
        /// <returns></returns>
        string SaveUploadBrandLogo(HttpPostedFileBase logo);

        /// <summary>
        /// 保存新闻编辑器中的图片
        /// </summary>
        /// <param name="image">新闻图片</param>
        /// <returns></returns>
        string SaveNewsEditorImage(HttpPostedFileBase image);

        /// <summary>
        /// 保存帮助编辑器中的图片
        /// </summary>
        /// <param name="image">帮助图片</param>
        /// <returns></returns>
        string SaveHelpEditorImage(HttpPostedFileBase image);

        /// <summary>
        /// 保存商品编辑器中的图片
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="image">商品图片</param>
        /// <returns></returns>
        string SaveProductEditorImage(int storeId, HttpPostedFileBase image);

        /// <summary>
        /// 保存上传的商品图片
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="image">商品图片</param>
        /// <returns></returns>
        string SaveUplaodProductImage(int storeId, HttpPostedFileBase image);

        /// <summary>
        /// 保存上传的广告图片
        /// </summary>
        /// <param name="image">广告图片</param>
        /// <returns></returns>
        string SaveUploadAdvertImage(HttpPostedFileBase image);

        /// <summary>
        /// 保存上传的友情链接Logo
        /// </summary>
        /// <param name="logo">友情链接logo</param>
        /// <returns></returns>
        string SaveUploadFriendLinkLogo(HttpPostedFileBase logo);

        /// <summary>
        /// 保存上传的店铺等级头像
        /// </summary>
        /// <param name="avatar">店铺等级头像</param>
        /// <returns></returns>
        string SaveUploadStoreRankAvatar(HttpPostedFileBase avatar);

        /// <summary>
        /// 保存上传的店铺logo
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="logo">店铺logo</param>
        /// <returns></returns>
        string SaveUploadStoreLogo(int storeId, HttpPostedFileBase logo);

        /// <summary>
        /// 保存上传的店铺banner
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="banner">店铺banner</param>
        /// <returns></returns>
        string SaveUploadStoreBanner(int storeId, HttpPostedFileBase banner);
    }
}
