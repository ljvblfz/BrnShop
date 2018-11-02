using System;
using System.Web;

namespace BrnShop.Core
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
        /// <param name="image">商品图片</param>
        /// <returns></returns>
        string SaveProductEditorImage(HttpPostedFileBase image);

        /// <summary>
        /// 保存上传的商品图片
        /// </summary>
        /// <param name="image">商品图片</param>
        /// <returns></returns>
        string SaveUplaodProductImage(HttpPostedFileBase image);

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
    }
}
