using System;
using System.ComponentModel.DataAnnotations;

namespace BrnMall.PayPlugin.WeChat
{
    /// <summary>
    /// 配置模型类
    /// </summary>
    public class ConfigModel
    {
        /// <summary>
        /// WPMchId
        /// </summary>
        [Required(ErrorMessage = "WPMchId不能为空！")]
        public string WPMchId { get; set; }
        /// <summary>
        /// WPAppId
        /// </summary>
        [Required(ErrorMessage = "WPAppId不能为空！")]
        public string WPAppId { get; set; }
        /// <summary>
        /// WPAppSecret
        /// </summary>
        [Required(ErrorMessage = "WPAppSecret不能为空！")]
        public string WPAppSecret { get; set; }
        /// <summary>
        /// WPAppKey
        /// </summary>
        [Required(ErrorMessage = "WPAppKey不能为空！")]
        public string WPAppKey { get; set; }
        /// <summary>
        /// OpenMchId
        /// </summary>
        [Required(ErrorMessage = "OpenMchId不能为空！")]
        public string OpenMchId { get; set; }
        /// <summary>
        /// OpenAppId
        /// </summary>
        [Required(ErrorMessage = "OpenAppId不能为空！")]
        public string OpenAppId { get; set; }
        /// <summary>
        /// OpenAppSecret
        /// </summary>
        [Required(ErrorMessage = "OpenAppSecret不能为空！")]
        public string OpenAppSecret { get; set; }
        /// <summary>
        /// OpenAppKey
        /// </summary>
        [Required(ErrorMessage = "OpenAppKey不能为空！")]
        public string OpenAppKey { get; set; }
    }
}
