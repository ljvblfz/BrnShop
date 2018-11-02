using System;
using System.IO;
using System.Web;

using BrnMall.Core;

namespace BrnMall.UploadStrategy.LocalServer
{
    /// <summary>
    /// 本地服务器上传策略
    /// </summary>
    public partial class UploadStrategy : IUploadStrategy
    {
        /// <summary>
        /// 保存上传的用户头像
        /// </summary>
        /// <param name="avatar">用户头像</param>
        /// <returns></returns>
        public string SaveUploadUserAvatar(HttpPostedFileBase avatar)
        {
            if (avatar == null)
                return "-1";

            UploadConfigInfo uploadConfigInfo = BMAConfig.UploadConfig;

            string fileName = avatar.FileName;
            string extension = Path.GetExtension(fileName);
            if (!ValidateHelper.IsImgFileName(fileName) || !CommonHelper.IsInArray(extension, uploadConfigInfo.UploadImgType))
                return "-2";

            int fileSize = avatar.ContentLength;
            if (fileSize > uploadConfigInfo.UploadImgSize)
                return "-3";

            string dirPath = IOHelper.GetMapPath("/upload/user/");
            string newFileName = string.Format("ua_{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfffffff"), extension);
            string[] sizeList = StringHelper.SplitString(uploadConfigInfo.UserAvatarThumbSize);

            string sourceDirPath = dirPath + "source/";
            if (!Directory.Exists(sourceDirPath))
                Directory.CreateDirectory(sourceDirPath);

            string sourcePath = sourceDirPath + newFileName;
            avatar.SaveAs(sourcePath);

            foreach (string size in sizeList)
            {
                string thumbDirPath = string.Format("{0}thumb{1}/", dirPath, size);
                if (!Directory.Exists(thumbDirPath))
                    Directory.CreateDirectory(thumbDirPath);
                string[] widthAndHeight = StringHelper.SplitString(size, "_");
                IOHelper.GenerateThumb(sourcePath,
                                       thumbDirPath + newFileName,
                                       TypeHelper.StringToInt(widthAndHeight[0]),
                                       TypeHelper.StringToInt(widthAndHeight[1]),
                                       "H");
            }
            return newFileName;
        }

        /// <summary>
        /// 保存上传的用户等级头像
        /// </summary>
        /// <param name="avatar">用户等级头像</param>
        /// <returns></returns>
        public string SaveUploadUserRankAvatar(HttpPostedFileBase avatar)
        {
            if (avatar == null)
                return "-1";

            UploadConfigInfo uploadConfigInfo = BMAConfig.UploadConfig;

            string fileName = avatar.FileName;
            string extension = Path.GetExtension(fileName);
            if (!ValidateHelper.IsImgFileName(fileName) || !CommonHelper.IsInArray(extension, uploadConfigInfo.UploadImgType))
                return "-2";

            int fileSize = avatar.ContentLength;
            if (fileSize > uploadConfigInfo.UploadImgSize)
                return "-3";

            string dirPath = IOHelper.GetMapPath("/upload/userrank/");
            string newFileName = string.Format("ura_{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfffffff"), extension);
            string[] sizeList = StringHelper.SplitString(uploadConfigInfo.UserRankAvatarThumbSize);

            string sourceDirPath = dirPath + "source/";
            if (!Directory.Exists(sourceDirPath))
                Directory.CreateDirectory(sourceDirPath);

            string sourcePath = sourceDirPath + newFileName;
            avatar.SaveAs(sourcePath);

            foreach (string size in sizeList)
            {
                string thumbDirPath = string.Format("{0}thumb{1}/", dirPath, size);
                if (!Directory.Exists(thumbDirPath))
                    Directory.CreateDirectory(thumbDirPath);
                string[] widthAndHeight = StringHelper.SplitString(size, "_");
                IOHelper.GenerateThumb(sourcePath,
                                       thumbDirPath + newFileName,
                                       TypeHelper.StringToInt(widthAndHeight[0]),
                                       TypeHelper.StringToInt(widthAndHeight[1]),
                                       "H");
            }
            return newFileName;
        }

        /// <summary>
        /// 保存上传的品牌logo
        /// </summary>
        /// <param name="logo">品牌logo</param>
        /// <returns></returns>
        public string SaveUploadBrandLogo(HttpPostedFileBase logo)
        {
            if (logo == null)
                return "-1";

            UploadConfigInfo uploadConfigInfo = BMAConfig.UploadConfig;

            string fileName = logo.FileName;
            string extension = Path.GetExtension(fileName);
            if (!ValidateHelper.IsImgFileName(fileName) || !CommonHelper.IsInArray(extension, uploadConfigInfo.UploadImgType))
                return "-2";

            int fileSize = logo.ContentLength;
            if (fileSize > uploadConfigInfo.UploadImgSize)
                return "-3";

            string dirPath = IOHelper.GetMapPath("/upload/brand/");
            string newFileName = string.Format("b_{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfffffff"), extension);
            string[] sizeList = StringHelper.SplitString(uploadConfigInfo.BrandThumbSize);

            string sourceDirPath = dirPath + "source/";
            if (!Directory.Exists(sourceDirPath))
                Directory.CreateDirectory(sourceDirPath);

            string sourcePath = sourceDirPath + newFileName;
            logo.SaveAs(sourcePath);

            foreach (string size in sizeList)
            {
                string thumbDirPath = string.Format("{0}thumb{1}/", dirPath, size);
                if (!Directory.Exists(thumbDirPath))
                    Directory.CreateDirectory(thumbDirPath);
                string[] widthAndHeight = StringHelper.SplitString(size, "_");
                IOHelper.GenerateThumb(sourcePath,
                                       thumbDirPath + newFileName,
                                       TypeHelper.StringToInt(widthAndHeight[0]),
                                       TypeHelper.StringToInt(widthAndHeight[1]),
                                       "H");
            }
            return newFileName;
        }

        /// <summary>
        /// 保存新闻编辑器中的图片
        /// </summary>
        /// <param name="image">新闻图片</param>
        /// <returns></returns>
        public string SaveNewsEditorImage(HttpPostedFileBase image)
        {
            if (image == null)
                return "-1";

            UploadConfigInfo uploadConfigInfo = BMAConfig.UploadConfig;

            string fileName = image.FileName;
            string extension = Path.GetExtension(fileName);
            if (!ValidateHelper.IsImgFileName(fileName) || !CommonHelper.IsInArray(extension, uploadConfigInfo.UploadImgType))
                return "-2";

            int fileSize = image.ContentLength;
            if (fileSize > uploadConfigInfo.UploadImgSize)
                return "-3";

            string dirPath = IOHelper.GetMapPath("/upload/news/");
            string newFileName = string.Format("n_{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfffffff"), extension);//生成文件名

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            image.SaveAs(dirPath + newFileName);

            return newFileName;
        }

        /// <summary>
        /// 保存帮助编辑器中的图片
        /// </summary>
        /// <param name="image">帮助图片</param>
        /// <returns></returns>
        public string SaveHelpEditorImage(HttpPostedFileBase image)
        {
            if (image == null)
                return "-1";

            UploadConfigInfo uploadConfigInfo = BMAConfig.UploadConfig;

            string fileName = image.FileName;
            string extension = Path.GetExtension(fileName);
            if (!ValidateHelper.IsImgFileName(fileName) || !CommonHelper.IsInArray(extension, uploadConfigInfo.UploadImgType))
                return "-2";

            int fileSize = image.ContentLength;
            if (fileSize > uploadConfigInfo.UploadImgSize)
                return "-3";

            string dirPath = IOHelper.GetMapPath("/upload/help/");
            string newFileName = string.Format("h_{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfffffff"), extension);//生成文件名

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            image.SaveAs(dirPath + newFileName);

            return newFileName;
        }

        /// <summary>
        /// 保存商品编辑器中的图片
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="image">商品图片</param>
        /// <returns></returns>
        public string SaveProductEditorImage(int storeId, HttpPostedFileBase image)
        {
            if (image == null)
                return "-1";

            UploadConfigInfo uploadConfigInfo = BMAConfig.UploadConfig;

            string fileName = image.FileName;
            string extension = Path.GetExtension(fileName);
            if (!ValidateHelper.IsImgFileName(fileName) || !CommonHelper.IsInArray(extension, uploadConfigInfo.UploadImgType))
                return "-2";

            int fileSize = image.ContentLength;
            if (fileSize > uploadConfigInfo.UploadImgSize)
                return "-3";

            string dirPath = IOHelper.GetMapPath(string.Format("/upload/store/{0}/product/editor/", storeId));
            string newFileName = string.Format("pe_{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfffffff"), extension);//生成文件名

            string sourceDirPath = dirPath + "source/";
            if (!Directory.Exists(sourceDirPath))
                Directory.CreateDirectory(sourceDirPath);
            string sourcePath = sourceDirPath + newFileName;
            image.SaveAs(sourcePath);

            string path = dirPath + newFileName;
            if (uploadConfigInfo.WatermarkType == 1)//文字水印
            {
                IOHelper.GenerateTextWatermark(sourcePath, path, uploadConfigInfo.WatermarkText, uploadConfigInfo.WatermarkTextSize, uploadConfigInfo.WatermarkTextFont, uploadConfigInfo.WatermarkPosition, uploadConfigInfo.WatermarkQuality);
            }
            else if (uploadConfigInfo.WatermarkType == 2)//图片水印
            {
                string watermarkPath = IOHelper.GetMapPath("/watermarks/" + uploadConfigInfo.WatermarkImg);
                IOHelper.GenerateImageWatermark(sourcePath, watermarkPath, path, uploadConfigInfo.WatermarkPosition, uploadConfigInfo.WatermarkImgOpacity, uploadConfigInfo.WatermarkQuality);
            }
            else
            {
                image.SaveAs(path);
            }

            return newFileName;
        }

        /// <summary>
        /// 保存上传的商品图片
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="image">商品图片</param>
        /// <returns></returns>
        public string SaveUplaodProductImage(int storeId, HttpPostedFileBase image)
        {
            if (image == null)
                return "-1";

            UploadConfigInfo uploadConfigInfo = BMAConfig.UploadConfig;

            string fileName = image.FileName;
            string extension = Path.GetExtension(fileName);
            if (!ValidateHelper.IsImgFileName(fileName) || !CommonHelper.IsInArray(extension, uploadConfigInfo.UploadImgType))
                return "-2";

            int fileSize = image.ContentLength;
            if (fileSize > uploadConfigInfo.UploadImgSize)
                return "-3";

            string dirPath = IOHelper.GetMapPath(string.Format("/upload/store/{0}/product/show/", storeId));
            string name = "ps_" + DateTime.Now.ToString("yyMMddHHmmssfffffff");
            string newFileName = name + extension;
            string[] sizeList = StringHelper.SplitString(uploadConfigInfo.ProductShowThumbSize);

            string sourceDirPath = string.Format("{0}source/", dirPath);
            if (!Directory.Exists(sourceDirPath))
                Directory.CreateDirectory(sourceDirPath);
            string sourcePath = sourceDirPath + newFileName;
            image.SaveAs(sourcePath);

            if (uploadConfigInfo.WatermarkType == 1)//文字水印
            {
                string path = string.Format("{0}{1}_text{2}", sourceDirPath, name, extension);
                IOHelper.GenerateTextWatermark(sourcePath, path, uploadConfigInfo.WatermarkText, uploadConfigInfo.WatermarkTextSize, uploadConfigInfo.WatermarkTextFont, uploadConfigInfo.WatermarkPosition, uploadConfigInfo.WatermarkQuality);
                sourcePath = path;
            }
            else if (uploadConfigInfo.WatermarkType == 2)//图片水印
            {
                string path = string.Format("{0}{1}_img{2}", sourceDirPath, name, extension);
                string watermarkPath = IOHelper.GetMapPath("/watermarks/" + uploadConfigInfo.WatermarkImg);
                IOHelper.GenerateImageWatermark(sourcePath, watermarkPath, path, uploadConfigInfo.WatermarkPosition, uploadConfigInfo.WatermarkImgOpacity, uploadConfigInfo.WatermarkQuality);
                sourcePath = path;
            }

            foreach (string size in sizeList)
            {
                string thumbDirPath = string.Format("{0}thumb{1}/", dirPath, size);
                if (!Directory.Exists(thumbDirPath))
                    Directory.CreateDirectory(thumbDirPath);
                string[] widthAndHeight = StringHelper.SplitString(size, "_");
                IOHelper.GenerateThumb(sourcePath,
                                       thumbDirPath + newFileName,
                                       TypeHelper.StringToInt(widthAndHeight[0]),
                                       TypeHelper.StringToInt(widthAndHeight[1]),
                                       "H");
            }
            return newFileName;
        }

        /// <summary>
        /// 保存上传的广告图片
        /// </summary>
        /// <param name="image">广告图片</param>
        /// <returns></returns>
        public string SaveUploadAdvertImage(HttpPostedFileBase image)
        {
            if (image == null)
                return "-1";

            UploadConfigInfo uploadConfigInfo = BMAConfig.UploadConfig;

            string fileName = image.FileName;
            string extension = Path.GetExtension(fileName);
            if (!ValidateHelper.IsImgFileName(fileName) || !CommonHelper.IsInArray(extension, uploadConfigInfo.UploadImgType))
                return "-2";

            int fileSize = image.ContentLength;
            if (fileSize > uploadConfigInfo.UploadImgSize)
                return "-3";

            string dirPath = IOHelper.GetMapPath("/upload/adv/");
            string newFileName = string.Format("ad_{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfffffff"), extension);//生成文件名

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            image.SaveAs(dirPath + newFileName);

            return newFileName;
        }

        /// <summary>
        /// 保存上传的友情链接Logo
        /// </summary>
        /// <param name="logo">友情链接logo</param>
        /// <returns></returns>
        public string SaveUploadFriendLinkLogo(HttpPostedFileBase logo)
        {
            if (logo == null)
                return "-1";

            UploadConfigInfo uploadConfigInfo = BMAConfig.UploadConfig;

            string fileName = logo.FileName;
            string extension = Path.GetExtension(fileName);
            if (!ValidateHelper.IsImgFileName(fileName) || !CommonHelper.IsInArray(extension, uploadConfigInfo.UploadImgType))
                return "-2";

            int fileSize = logo.ContentLength;
            if (fileSize > uploadConfigInfo.UploadImgSize)
                return "-3";

            string dirPath = IOHelper.GetMapPath("/upload/friendlink/");
            string newFileName = string.Format("fr_{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfffffff"), extension);//生成文件名

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            logo.SaveAs(dirPath + newFileName);

            return newFileName;
        }

        /// <summary>
        /// 保存上传的店铺等级头像
        /// </summary>
        /// <param name="avatar">店铺等级头像</param>
        /// <returns></returns>
        public string SaveUploadStoreRankAvatar(HttpPostedFileBase avatar)
        {
            if (avatar == null)
                return "-1";

            UploadConfigInfo uploadConfigInfo = BMAConfig.UploadConfig;

            string fileName = avatar.FileName;
            string extension = Path.GetExtension(fileName);
            if (!ValidateHelper.IsImgFileName(fileName) || !CommonHelper.IsInArray(extension, uploadConfigInfo.UploadImgType))
                return "-2";

            int fileSize = avatar.ContentLength;
            if (fileSize > uploadConfigInfo.UploadImgSize)
                return "-3";

            string dirPath = IOHelper.GetMapPath("/upload/storerank/");
            string newFileName = string.Format("sra_{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfffffff"), extension);
            string[] sizeList = StringHelper.SplitString(uploadConfigInfo.StoreRankAvatarThumbSize);

            string sourceDirPath = dirPath + "source/";
            if (!Directory.Exists(sourceDirPath))
                Directory.CreateDirectory(sourceDirPath);

            string sourcePath = sourceDirPath + newFileName;
            avatar.SaveAs(sourcePath);

            foreach (string size in sizeList)
            {
                string thumbDirPath = string.Format("{0}thumb{1}/", dirPath, size);
                if (!Directory.Exists(thumbDirPath))
                    Directory.CreateDirectory(thumbDirPath);
                string[] widthAndHeight = StringHelper.SplitString(size, "_");
                IOHelper.GenerateThumb(sourcePath,
                                       thumbDirPath + newFileName,
                                       TypeHelper.StringToInt(widthAndHeight[0]),
                                       TypeHelper.StringToInt(widthAndHeight[1]),
                                       "H");
            }
            return newFileName;
        }

        /// <summary>
        /// 保存上传的店铺logo
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="logo">店铺logo</param>
        /// <returns></returns>
        public string SaveUploadStoreLogo(int storeId, HttpPostedFileBase logo)
        {
            if (logo == null)
                return "-1";

            UploadConfigInfo uploadConfigInfo = BMAConfig.UploadConfig;

            string fileName = logo.FileName;
            string extension = Path.GetExtension(fileName);
            if (!ValidateHelper.IsImgFileName(fileName) || !CommonHelper.IsInArray(extension, uploadConfigInfo.UploadImgType))
                return "-2";

            int fileSize = logo.ContentLength;
            if (fileSize > uploadConfigInfo.UploadImgSize)
                return "-3";

            string dirPath = IOHelper.GetMapPath(string.Format("/upload/store/{0}/logo/", storeId));
            string newFileName = string.Format("s_{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfffffff"), extension);
            string[] sizeList = StringHelper.SplitString(uploadConfigInfo.StoreLogoThumbSize);

            string sourceDirPath = dirPath + "source/";
            if (!Directory.Exists(sourceDirPath))
                Directory.CreateDirectory(sourceDirPath);

            string sourcePath = sourceDirPath + newFileName;
            logo.SaveAs(sourcePath);

            foreach (string size in sizeList)
            {
                string thumbDirPath = string.Format("{0}thumb{1}/", dirPath, size);
                if (!Directory.Exists(thumbDirPath))
                    Directory.CreateDirectory(thumbDirPath);
                string[] widthAndHeight = StringHelper.SplitString(size, "_");
                IOHelper.GenerateThumb(sourcePath,
                                       thumbDirPath + newFileName,
                                       TypeHelper.StringToInt(widthAndHeight[0]),
                                       TypeHelper.StringToInt(widthAndHeight[1]),
                                       "H");
            }
            return newFileName;
        }

        /// <summary>
        /// 保存上传的店铺banner
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="banner">店铺banner</param>
        /// <returns></returns>
        public string SaveUploadStoreBanner(int storeId, HttpPostedFileBase banner)
        {
            if (banner == null)
                return "-1";

            UploadConfigInfo uploadConfigInfo = BMAConfig.UploadConfig;

            string fileName = banner.FileName;
            string extension = Path.GetExtension(fileName);
            if (!ValidateHelper.IsImgFileName(fileName) || !CommonHelper.IsInArray(extension, uploadConfigInfo.UploadImgType))
                return "-2";

            int fileSize = banner.ContentLength;
            if (fileSize > uploadConfigInfo.UploadImgSize)
                return "-3";

            string dirPath = IOHelper.GetMapPath(string.Format("/upload/store/{0}/banner/", storeId));
            string newFileName = string.Format("sb_{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfffffff"), extension);//生成文件名

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            banner.SaveAs(dirPath + newFileName);

            return newFileName;
        }
    }
}
