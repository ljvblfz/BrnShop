using System;
using System.ComponentModel.DataAnnotations;

namespace BrnMall.PayPlugin.Alipay
{
    /// <summary>
    /// 配置模型类
    /// </summary>
    public class ConfigModel
    {
        /// <summary>
        /// 合作者身份ID
        /// </summary>
        [Required(ErrorMessage = "合作者身份ID不能为空！")]
        public string Partner { get; set; }
        /// <summary>
        /// 交易安全检验码
        /// </summary>
        [Required(ErrorMessage = "交易安全检验码不能为空！")]
        public string Key { get; set; }
        /// <summary>
        /// 私钥
        /// </summary>
        [Required(ErrorMessage = "私钥不能为空！")]
        public string PrivateKey { get; set; }
        /// <summary>
        /// 收款支付宝帐户
        /// </summary>
        [Required(ErrorMessage = "收款支付宝帐户不能为空！")]
        public string Seller { get; set; }
    }
}
