using System;
using System.Web;
using System.Data;
using System.Drawing;
using System.Web.Mvc;
using System.Drawing.Text;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BrnMall.Core;

namespace BrnMall.Web.MallAdmin.Models
{
    /// <summary>
    /// 站点模型类
    /// </summary>
    public class SiteModel
    {
        /// <summary>
        /// 商城名称
        /// </summary>
        public string MallName { get; set; }

        /// <summary>
        /// 网站网址
        /// </summary>
        [AllowHtml]
        public string SiteUrl { get; set; }

        /// <summary>
        /// 网站标题
        /// </summary>
        public string SiteTitle { get; set; }

        /// <summary>
        /// seo关键字
        /// </summary>
        [AllowHtml]
        public string SEOKeyword { get; set; }

        /// <summary>
        /// seo描述
        /// </summary>
        [AllowHtml]
        public string SEODescription { get; set; }

        /// <summary>
        /// 备案编号
        /// </summary>
        public string ICP { get; set; }

        /// <summary>
        /// 脚本
        /// </summary>
        [AllowHtml]
        public string Script { get; set; }

        /// <summary>
        /// 是否显示版权(0代表不显示，1代表显示)
        /// </summary>
        [Range(0, 1, ErrorMessage = "请选择正确的选项")]
        public int IsLicensed { get; set; }
    }

    /// <summary>
    /// 账号模型类
    /// </summary>
    public class AccountModel : IValidatableObject
    {
        /// <summary>
        /// 注册类型(1代表用户名注册，2代表邮箱注册，3代表手机注册，空字符串代表不允许注册)
        /// </summary>
        public int[] RegType { get; set; }

        /// <summary>
        /// 保留用户名
        /// </summary>
        public string ReservedName { get; set; }

        /// <summary>
        /// 注册时间间隔(单位为秒，0代表无限制)
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "注册时间间隔不能为负数")]
        [DisplayName("注册时间间隔")]
        public int RegTimeSpan { get; set; }

        /// <summary>
        /// 是否发送欢迎信息(0代表不发送，1代表发送)
        /// </summary>
        [Range(0, 1, ErrorMessage = "请选择正确的类型")]
        [DisplayName("是否发送欢迎信息")]
        public int IsWebcomeMsg { get; set; }

        /// <summary>
        /// 欢迎信息
        /// </summary>
        [AllowHtml]
        public string WebcomeMsg { get; set; }

        /// <summary>
        /// 登录类型(1代表用户名登录，2代表邮箱登录，3代表手机登录，空字符串代表不允许登录)
        /// </summary>
        public int[] LoginType { get; set; }

        /// <summary>
        /// 影子登录名
        /// </summary>
        public string ShadowName { get; set; }

        /// <summary>
        /// 是否记住密码(0代表不记住，1代表记住)
        /// </summary>
        [Range(0, 1, ErrorMessage = "请选择正确的类型")]
        [DisplayName("是否记住密码")]
        public int IsRemember { get; set; }

        /// <summary>
        /// 登录失败次数
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "登录失败次数不能小于0")]
        [DisplayName("登录失败次数")]
        public int LoginFailTimes { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errorList = new List<ValidationResult>();

            if (IsWebcomeMsg == 1 && string.IsNullOrWhiteSpace(WebcomeMsg))
                errorList.Add(new ValidationResult("欢迎信息不能为空!", new string[] { "WebcomeMsg" }));

            return errorList;
        }
    }

    /// <summary>
    /// 上传模型类
    /// </summary>
    public class UploadModel : IValidatableObject
    {
        /// <summary>
        /// 上传的图片类型
        /// </summary>
        [Required(ErrorMessage = "图片类型不能为空")]
        public string UploadImgType { get; set; }

        /// <summary>
        /// 上传图片的大小(单位为字节)
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "图片大小必须大于0")]
        [DisplayName("图片大小")]
        public int UploadImgSize { get; set; }

        /// <summary>
        /// 水印类型(0代表没有水印，1代表文字水印，2代表图片水印)
        /// </summary>
        [Range(0, 2, ErrorMessage = "请选择正确的水印类型")]
        [DisplayName("水印类型")]
        public int WatermarkType { get; set; }

        /// <summary>
        /// 水印质量(必须位于0到100之间)
        /// </summary>
        [Range(0, 100, ErrorMessage = "水印质量只能位于0到100")]
        [DisplayName("水印质量")]
        public int WatermarkQuality { get; set; }

        /// <summary>
        /// 水印位置(1代表上左，2代表上中，3代表上右，4代表中左，5代表中中，6代表中右，7代表下左，8代表下中，9代表下右)
        /// </summary>
        [Range(1, 9, ErrorMessage = "请选择正确的水印位置")]
        [DisplayName("水印位置")]
        public int WatermarkPosition { get; set; }

        /// <summary>
        /// 水印图片
        /// </summary>
        public string WatermarkImg { get; set; }

        /// <summary>
        /// 水印图片透明度(必须位于1到10之间)
        /// </summary>
        [Range(1, 10, ErrorMessage = "水印图片透明度必须位于1到10之间")]
        [DisplayName("水印图片透明度")]
        public int WatermarkImgOpacity { get; set; }

        /// <summary>
        /// 水印文字
        /// </summary>
        public string WatermarkText { get; set; }

        /// <summary>
        /// 水印文字字体
        /// </summary>
        public string WatermarkTextFont { get; set; }

        /// <summary>
        /// 水印文字大小
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "水印文字大小必须大于0")]
        [DisplayName("水印文字大小")]
        public int WatermarkTextSize { get; set; }

        /// <summary>
        /// 品牌缩略图大小
        /// </summary>
        [Required(ErrorMessage = "请输入内容")]
        public string BrandThumbSize { get; set; }

        /// <summary>
        /// 商品展示缩略图大小
        /// </summary>
        [Required(ErrorMessage = "请输入内容")]
        public string ProductShowThumbSize { get; set; }

        /// <summary>
        /// 用户头像缩略图大小
        /// </summary>
        [Required(ErrorMessage = "请输入内容")]
        public string UserAvatarThumbSize { get; set; }

        /// <summary>
        /// 用户等级头像缩略图大小
        /// </summary>
        [Required(ErrorMessage = "请输入内容")]
        public string UserRankAvatarThumbSize { get; set; }

        /// <summary>
        /// 店铺等级头像缩略图大小
        /// </summary>
        [Required(ErrorMessage = "请输入内容")]
        public string StoreRankAvatarThumbSize { get; set; }

        /// <summary>
        /// 店铺Logo缩略图大小
        /// </summary>
        [Required(ErrorMessage = "请输入内容")]
        public string StoreLogoThumbSize { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errorList = new List<ValidationResult>();

            if (WatermarkType == 1 && string.IsNullOrWhiteSpace(WatermarkText))
                errorList.Add(new ValidationResult("水印文字不能为空!", new string[] { "WatermarkText" }));
            else if (WatermarkType == 2 && string.IsNullOrWhiteSpace(WatermarkImg))
                errorList.Add(new ValidationResult("水印图片不能为空!", new string[] { "WatermarkImg" }));


            return errorList;
        }
    }

    /// <summary>
    /// 性能模型类
    /// </summary>
    public class PerformanceModel
    {
        /// <summary>
        /// 图片cdn
        /// </summary>
        public string ImageCDN { get; set; }

        /// <summary>
        /// csscdn
        /// </summary>
        public string CSSCDN { get; set; }

        /// <summary>
        /// 脚本cdn
        /// </summary>
        public string ScriptCDN { get; set; }

        /// <summary>
        /// 在线用户过期时间(单位为分钟)
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "在线用户过期时间不能小于0")]
        [DisplayName("在线用户过期时间")]
        public int OnlineUserExpire { get; set; }

        /// <summary>
        /// 更新用户在线时间间隔(单位为分钟,0代表不更新)
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "更新用户在线时间间隔不能小于0")]
        [DisplayName("更新用户在线时间间隔")]
        public int UpdateOnlineTimeSpan { get; set; }

        /// <summary>
        /// 最大在线人数
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "最大在线用户不能小于0")]
        [DisplayName("最大在线人数")]
        public int MaxOnlineCount { get; set; }

        /// <summary>
        /// 在线人数缓存时间(单位为分钟,0代表即时数量)
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "在线人数缓存时间不能小于0")]
        [DisplayName("在线人数缓存时间")]
        public int OnlineCountExpire { get; set; }

        /// <summary>
        /// 是否统计浏览器(0代表不统计，1代表统计)
        /// </summary>
        [Range(0, 1, ErrorMessage = "请选择正确的选项")]
        [DisplayName("是否统计浏览器")]
        public int IsStatBrowser { get; set; }

        /// <summary>
        /// 是否统计操作系统(0代表不统计，1代表统计)
        /// </summary>
        [Range(0, 1, ErrorMessage = "请选择正确的选项")]
        [DisplayName("是否统计操作系统")]
        public int IsStatOS { get; set; }

        /// <summary>
        /// 是否统计区域(0代表不统计，1代表统计)
        /// </summary>
        [Range(0, 1, ErrorMessage = "请选择正确的选项")]
        [DisplayName("是否统计区域")]
        public int IsStatRegion { get; set; }
    }

    /// <summary>
    /// 访问模型类
    /// </summary>
    public class AccessModel : IValidatableObject
    {
        /// <summary>
        /// 是否关闭商城(0代表打开，1代表关闭)
        /// </summary>
        [Range(0, 1, ErrorMessage = "请选择正确的选项")]
        public int IsClosed { get; set; }

        /// <summary>
        /// 商城关闭原因
        /// </summary>
        [AllowHtml]
        public string CloseReason { get; set; }

        /// <summary>
        /// 禁止访问时间
        /// </summary>
        public string BanAccessTime { get; set; }

        /// <summary>
        /// 禁止访问ip
        /// </summary>
        public string BanAccessIP { get; set; }

        /// <summary>
        /// 允许访问ip
        /// </summary>
        public string AllowAccessIP { get; set; }

        /// <summary>
        /// 后台允许访问ip
        /// </summary>
        public string AdminAllowAccessIP { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        [StringLength(32, MinimumLength = 8, ErrorMessage = "密钥长度必须大于7且小于33")]
        [Required(ErrorMessage = "密钥不能为空")]
        public string SecretKey { get; set; }

        /// <summary>
        /// cookie的有效域
        /// </summary>
        public string CookieDomain { get; set; }

        /// <summary>
        /// 随机库
        /// </summary>
        public string RandomLibrary { get; set; }

        /// <summary>
        /// 使用验证码的页面
        /// </summary>
        public string[] VerifyPages { get; set; }

        /// <summary>
        /// 忽略词
        /// </summary>
        public string IgnoreWords { get; set; }

        /// <summary>
        /// 允许的邮箱提供者
        /// </summary>
        public string AllowEmailProvider { get; set; }

        /// <summary>
        /// 禁止的邮箱提供者
        /// </summary>
        public string BanEmailProvider { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errorList = new List<ValidationResult>();

            if (IsClosed == 1 && string.IsNullOrWhiteSpace(CloseReason))
                errorList.Add(new ValidationResult("请填写商城关闭原因!", new string[] { "CloseReason" }));

            if (CookieDomain != null && (!CookieDomain.Contains(".") || !WebHelper.GetHost().Contains(CookieDomain)))
                errorList.Add(new ValidationResult("cookie域不合法!", new string[] { "CookieDomain" }));

            //if (!string.IsNullOrWhiteSpace(BanAccessTime))
            //{
            //    string[] timeList = StringHelper.SplitString(BanAccessTime, "\r\n");
            //    foreach (string time in timeList)
            //    {
            //        string[] startTimeAndEndTime = StringHelper.SplitString(time, "-");
            //        if (startTimeAndEndTime.Length == 2)
            //        {
            //            foreach (string item in startTimeAndEndTime)
            //            {
            //                string[] hourAndMinute = StringHelper.SplitString(item, ":");
            //                if (hourAndMinute.Length == 2)
            //                {
            //                    if (!CheckTime(hourAndMinute[0], hourAndMinute[1]))
            //                    {
            //                        errorList.Add(new ValidationResult("时间格式不正确!", new string[] { "BanAccessTime" }));
            //                        break;
            //                    }
            //                }
            //                else
            //                {
            //                    errorList.Add(new ValidationResult("时间格式不正确!", new string[] { "BanAccessTime" }));
            //                    break;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            errorList.Add(new ValidationResult("时间格式不正确!", new string[] { "BanAccessTime" }));
            //            break;
            //        }
            //    }
            //}

            return errorList;
        }

        private bool CheckTime(string hour, string minute)
        {
            if (hour.Length == 1 || hour.Length == 2)
            {
                int hourInt = TypeHelper.StringToInt(hour.Trim('0'));
                if (hourInt < 0 || hourInt > 23)
                    return false;
            }
            else
            {
                return false;
            }

            if (minute.Length == 1 || minute.Length == 2)
            {
                int minuteInt = TypeHelper.StringToInt(minute.Trim('0'));
                if (minuteInt < 0 || minuteInt > 59)
                    return false;
            }
            else
            {
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// 邮箱模型类
    /// </summary>
    public class EmailModel
    {
        /// <summary>
        /// 邮件服务器地址
        /// </summary>
        [Required(ErrorMessage = "主机不能为空")]
        public string Host { get; set; }

        /// <summary>
        /// 邮件服务器端口
        /// </summary>
        [Required(ErrorMessage = "端口不能为空")]
        public int Port { get; set; }

        /// <summary>
        /// 发送邮件
        /// </summary>
        [Required(ErrorMessage = "发送邮箱不能为空")]
        public string From { get; set; }

        /// <summary>
        /// 发送邮件的昵称
        /// </summary>
        [Required(ErrorMessage = "发送邮箱昵称不能为空")]
        public string FromName { get; set; }

        /// <summary>
        /// 发送邮件的账号
        /// </summary>
        [Required(ErrorMessage = "账号不能为空")]
        public string UserName { get; set; }

        /// <summary>
        /// 发送邮件的密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }

        /// <summary>
        /// 找回密码内容
        /// </summary>
        [AllowHtml]
        [Required(ErrorMessage = "内容不能为空")]
        public string FindPwdBody { get; set; }

        /// <summary>
        /// 安全中心验证邮箱内容
        /// </summary>
        [AllowHtml]
        [Required(ErrorMessage = "内容不能为空")]
        public string SCVerifyBody { get; set; }

        /// <summary>
        /// 安全中心确认更新邮箱内容
        /// </summary>
        [AllowHtml]
        [Required(ErrorMessage = "内容不能为空")]
        public string SCUpdateBody { get; set; }

        /// <summary>
        /// 注册欢迎信息
        /// </summary>
        [AllowHtml]
        [Required(ErrorMessage = "内容不能为空")]
        public string WebcomeBody { get; set; }
    }

    /// <summary>
    /// 短信模型类
    /// </summary>
    public class SMSModel
    {
        /// <summary>
        /// 短信服务器地址
        /// </summary>
        [Required(ErrorMessage = "地址不能为空")]
        public string Url { get; set; }

        /// <summary>
        /// 短信账号
        /// </summary>
        [Required(ErrorMessage = "账号不能为空")]
        public string UserName { get; set; }

        /// <summary>
        /// 短信密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }

        /// <summary>
        /// 找回密码内容
        /// </summary>
        [Required(ErrorMessage = "内容不能为空")]
        public string FindPwdBody { get; set; }

        /// <summary>
        /// 安全中心验证手机内容
        /// </summary>
        [Required(ErrorMessage = "内容不能为空")]
        public string SCVerifyBody { get; set; }

        /// <summary>
        /// 安全中心确认更新手机内容
        /// </summary>
        [Required(ErrorMessage = "内容不能为空")]
        public string SCUpdateBody { get; set; }

        /// <summary>
        /// 注册欢迎信息
        /// </summary>
        [Required(ErrorMessage = "内容不能为空")]
        public string WebcomeBody { get; set; }
    }

    /// <summary>
    /// 积分模型类
    /// </summary>
    public class CreditModel
    {
        /// <summary>
        /// 支付积分名称
        /// </summary>
        [Required(ErrorMessage = "支付积分名称不能为空")]
        public string PayCreditName { get; set; }

        /// <summary>
        /// 支付积分价格(单位为100个)
        /// </summary>
        [Range(1, 100, ErrorMessage = "支付积分价格必须位于1和100之间")]
        [DisplayName("支付积分价格")]
        public int PayCreditPrice { get; set; }

        /// <summary>
        /// 每天最大发放支付积分
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "每天最大发放支付积分不能小于0")]
        [DisplayName("每天最大发放支付积分")]
        public int DayMaxSendPayCredits { get; set; }

        /// <summary>
        /// 每笔订单最大使用支付积分
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "每笔订单最大使用支付积分不能小于0")]
        [DisplayName("每笔订单最大使用支付积分")]
        public int OrderMaxUsePayCredits { get; set; }

        /// <summary>
        /// 注册支付积分
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "注册支付积分不能小于0")]
        [DisplayName("注册支付积分")]
        public int RegisterPayCredits { get; set; }

        /// <summary>
        /// 每天登录支付积分
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "每天登录支付积分不能小于0")]
        [DisplayName("每天登录支付积分")]
        public int LoginPayCredits { get; set; }

        /// <summary>
        /// 验证邮箱支付积分
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "验证邮箱支付积分不能小于0")]
        [DisplayName("验证邮箱支付积分")]
        public int VerifyEmailPayCredits { get; set; }

        /// <summary>
        /// 验证手机支付积分
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "验证手机支付积分不能小于0")]
        [DisplayName("验证手机支付积分")]
        public int VerifyMobilePayCredits { get; set; }

        /// <summary>
        /// 完善用户信息支付积分
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "完善用户信息支付积分不能小于0")]
        [DisplayName("完善用户信息支付积分")]
        public int CompleteUserInfoPayCredits { get; set; }

        /// <summary>
        /// 收货支付积分(以订单金额的百分比计算)
        /// </summary>
        [Range(0, 100, ErrorMessage = "收货支付积分必须位于0和100之间")]
        [DisplayName("收货支付积分")]
        public int ReceiveOrderPayCredits { get; set; }

        /// <summary>
        /// 评价商品支付积分
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "评价商品支付积分不能小于0")]
        [DisplayName("评价商品支付积分")]
        public int ReviewProductPayCredits { get; set; }

        /// <summary>
        /// 等级积分名称
        /// </summary>
        [Required(ErrorMessage = "等级积分名称不能为空")]
        public string RankCreditName { get; set; }

        /// <summary>
        /// 每天最大发放等级积分
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "每天最大发放等级积分不能小于0")]
        [DisplayName("每天最大发放等级积分")]
        public int DayMaxSendRankCredits { get; set; }

        /// <summary>
        /// 注册等级积分
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "注册等级积分不能小于0")]
        [DisplayName("注册等级积分")]
        public int RegisterRankCredits { get; set; }

        /// <summary>
        /// 每天登录等级积分
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "每天登录等级积分不能小于0")]
        [DisplayName("每天登录等级积分")]
        public int LoginRankCredits { get; set; }

        /// <summary>
        /// 验证邮箱等级积分
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "验证邮箱等级积分不能小于0")]
        [DisplayName("验证邮箱等级积分")]
        public int VerifyEmailRankCredits { get; set; }

        /// <summary>
        /// 验证手机等级积分
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "验证手机等级积分不能小于0")]
        [DisplayName("验证手机等级积分")]
        public int VerifyMobileRankCredits { get; set; }

        /// <summary>
        /// 完善用户信息等级积分
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "完善用户信息等级积分不能小于0")]
        [DisplayName("完善用户信息等级积分")]
        public int CompleteUserInfoRankCredits { get; set; }

        /// <summary>
        /// 收货等级积分(以订单金额的百分比计算)
        /// </summary>
        [Range(0, 100, ErrorMessage = "收货等级积分必须位于0和100之间")]
        [DisplayName("收货等级积分")]
        public int ReceiveOrderRankCredits { get; set; }

        /// <summary>
        /// 评价商品等级积分
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "评价商品等级积分不能小于0")]
        [DisplayName("评价商品等级积分")]
        public int ReviewProductRankCredits { get; set; }
    }

    /// <summary>
    /// 商城模型类
    /// </summary>
    public class MallModel
    {
        /// <summary>
        /// 是否允许游客使用购物车
        /// </summary>
        public int IsGuestSC { get; set; }

        /// <summary>
        /// 购物车的提交方式(0代表跳转到提示页面，1代表跳转到列表页面，2代表ajax提交)
        /// </summary>
        public int SCSubmitType { get; set; }

        /// <summary>
        /// 游客购物车最大商品数量
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "数量必须大于0")]
        [DisplayName("游客购物车最大商品数量")]
        public int GuestSCCount { get; set; }

        /// <summary>
        /// 会员购物车最大商品数量
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "数量必须大于0")]
        [DisplayName("会员购物车最大商品数量")]
        public int MemberSCCount { get; set; }

        /// <summary>
        /// 购物车过期时间(单位为天)
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "过期时间必须大于0")]
        [DisplayName("购物车过期时间")]
        public int SCExpire { get; set; }

        /// <summary>
        /// 订单编号格式
        /// </summary>
        [Required(ErrorMessage = "订单编号格式不能为空")]
        public string OSNFormat { get; set; }

        /// <summary>
        /// 在线支付过期时间
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "时间必须大于0")]
        [DisplayName("在线支付过期时间")]
        public int OnlinePayExpire { get; set; }

        /// <summary>
        /// 收货过期时间
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "时间必须大于0")]
        [DisplayName("收货过期时间")]
        public int ReceiveExpire { get; set; }

        /// <summary>
        /// 浏览历史数量
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "数量不能小于0")]
        [DisplayName("浏览历史数量")]
        public int BroHisCount { get; set; }

        /// <summary>
        /// 最大配送地址
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "数量必须大于0")]
        [DisplayName("最大配送地址")]
        public int MaxShipAddress { get; set; }

        /// <summary>
        /// 商品收藏夹最大容量
        /// </summary>
        public int FavoriteProductCount { get; set; }

        /// <summary>
        /// 店铺收藏夹最大容量
        /// </summary>
        public int FavoriteStoreCount { get; set; }
    }
}
