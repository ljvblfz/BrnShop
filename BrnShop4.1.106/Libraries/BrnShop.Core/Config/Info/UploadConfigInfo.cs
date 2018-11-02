using System;

namespace BrnShop.Core
{
    /// <summary>
    /// 上传配置信息类
    /// </summary>
    [Serializable]
    public class UploadConfigInfo : IConfigInfo
    {
        private string _uploadimgtype = "";//上传的图片类型,例如".jpg"
        private int _uploadimgsize = 22;//上传图片的大小(单位为字节)
        private int _watermarktype = 0;//水印类型(0代表没有水印，1代表文字水印，2代表图片水印)
        private int _watermarkquality = 0;//水印质量(必须位于0到100之间)
        private int _watermarkposition = 0;//水印位置(1代表上左，2代表上中，3代表上右，4代表中左，5代表中中，6代表中右，7代表下左，8代表下中，9代表下右)
        private string _watermarkimg = "";//水印图片
        private int _watermarkimgopacity = 0;//水印图片透明度(必须位于1到10之间)
        private string _watermarktext = "";//水印文字
        private string _watermarktextfont = "";//水印文字字体
        private int _watermarktextsize = 0;//水印文字大小
        private string _brandthumbsize = "";//品牌缩略图大小
        private string _productshowthumbsize = "";//商品展示缩略图大小
        private string _useravatarthumbsize = "";//用户头像缩略图大小
        private string _userrankavatarthumbsize = "";//用户等级头像缩略图大小

        /// <summary>
        /// 上传的图片类型,例如".jpg"
        /// </summary>
        public string UploadImgType
        {
            get { return _uploadimgtype; }
            set { _uploadimgtype = value; }
        }

        /// <summary>
        /// 上传图片的大小(单位为字节)
        /// </summary>
        public int UploadImgSize
        {
            get { return _uploadimgsize; }
            set { _uploadimgsize = value; }
        }

        /// <summary>
        /// 水印类型(0代表没有水印，1代表文字水印，2代表图片水印)
        /// </summary>
        public int WatermarkType
        {
            get { return _watermarktype; }
            set { _watermarktype = value; }
        }

        /// <summary>
        /// 水印质量(必须位于0到100之间)
        /// </summary>
        public int WatermarkQuality
        {
            get { return _watermarkquality; }
            set { _watermarkquality = value; }
        }

        /// <summary>
        /// 水印位置(1代表上左，2代表上中，3代表上右，4代表中左，5代表中中，6代表中右，7代表下左，8代表下中，9代表下右)
        /// </summary>
        public int WatermarkPosition
        {
            get { return _watermarkposition; }
            set { _watermarkposition = value; }
        }

        /// <summary>
        /// 水印图片
        /// </summary>
        public string WatermarkImg
        {
            get { return _watermarkimg; }
            set { _watermarkimg = value; }
        }

        /// <summary>
        /// 水印图片透明度(必须位于1到10之间)
        /// </summary>
        public int WatermarkImgOpacity
        {
            get { return _watermarkimgopacity; }
            set { _watermarkimgopacity = value; }
        }

        /// <summary>
        /// 水印文字
        /// </summary>
        public string WatermarkText
        {
            get { return _watermarktext; }
            set { _watermarktext = value; }
        }

        /// <summary>
        /// 水印文字字体
        /// </summary>
        public string WatermarkTextFont
        {
            get { return _watermarktextfont; }
            set { _watermarktextfont = value; }
        }

        /// <summary>
        /// 水印文字大小
        /// </summary>
        public int WatermarkTextSize
        {
            get { return _watermarktextsize; }
            set { _watermarktextsize = value; }
        }

        /// <summary>
        /// 品牌缩略图大小
        /// </summary>
        public string BrandThumbSize
        {
            get { return _brandthumbsize; }
            set { _brandthumbsize = value; }
        }

        /// <summary>
        /// 商品展示缩略图大小
        /// </summary>
        public string ProductShowThumbSize
        {
            get { return _productshowthumbsize; }
            set { _productshowthumbsize = value; }
        }

        /// <summary>
        /// 用户头像缩略图大小
        /// </summary>
        public string UserAvatarThumbSize
        {
            get { return _useravatarthumbsize; }
            set { _useravatarthumbsize = value; }
        }

        /// <summary>
        /// 用户等级头像缩略图大小
        /// </summary>
        public string UserRankAvatarThumbSize
        {
            get { return _userrankavatarthumbsize; }
            set { _userrankavatarthumbsize = value; }
        }
    }
}
