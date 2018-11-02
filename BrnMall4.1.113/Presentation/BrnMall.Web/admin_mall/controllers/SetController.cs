using System;
using System.Web;
using System.Text;
using System.Drawing;
using System.Web.Mvc;
using System.Drawing.Text;
using System.Collections.Generic;

using BrnMall.Core;
using BrnMall.Services;
using BrnMall.Web.Framework;
using BrnMall.Web.MallAdmin.Models;

namespace BrnMall.Web.MallAdmin.Controllers
{
    /// <summary>
    /// 商城后台系统设置控制器类
    /// </summary>
    public partial class SetController : BaseMallAdminController
    {
        /// <summary>
        /// 站点设置
        /// </summary>
        [HttpGet]
        public ActionResult Site()
        {
            MallConfigInfo mallConfigInfo = BMAConfig.MallConfig;

            SiteModel model = new SiteModel();

            model.MallName = mallConfigInfo.MallName;
            model.SiteUrl = mallConfigInfo.SiteUrl;
            model.SiteTitle = mallConfigInfo.SiteTitle;
            model.SEOKeyword = mallConfigInfo.SEOKeyword;
            model.SEODescription = mallConfigInfo.SEODescription;
            model.ICP = mallConfigInfo.ICP;
            model.Script = mallConfigInfo.Script;
            model.IsLicensed = mallConfigInfo.IsLicensed;

            return View(model);
        }

        /// <summary>
        /// 站点设置
        /// </summary>
        [HttpPost]
        public ActionResult Site(SiteModel model)
        {
            if (ModelState.IsValid)
            {
                MallConfigInfo mallConfigInfo = BMAConfig.MallConfig;

                mallConfigInfo.MallName = model.MallName == null ? "" : model.MallName;
                mallConfigInfo.SiteUrl = model.SiteUrl == null ? "" : model.SiteUrl;
                mallConfigInfo.SiteTitle = model.SiteTitle == null ? "" : model.SiteTitle;
                mallConfigInfo.SEOKeyword = model.SEOKeyword == null ? "" : model.SEOKeyword;
                mallConfigInfo.SEODescription = model.SEODescription == null ? "" : model.SEODescription;
                mallConfigInfo.ICP = model.ICP == null ? "" : model.ICP;
                mallConfigInfo.Script = model.Script == null ? "" : model.Script;
                mallConfigInfo.IsLicensed = model.IsLicensed;

                BMAConfig.SaveMallConfig(mallConfigInfo);
                Emails.ResetMall();
                SMSes.ResetMall();
                AddMallAdminLog("修改站点信息");
                return PromptView(Url.Action("site"), "修改站点信息成功");
            }
            return View(model);
        }

        /// <summary>
        /// 账号
        /// </summary>
        [HttpGet]
        public ActionResult Account()
        {
            MallConfigInfo mallConfigInfo = BMAConfig.MallConfig;

            AccountModel model = new AccountModel();
            model.RegType = StringToIntArray(mallConfigInfo.RegType);
            model.ReservedName = mallConfigInfo.ReservedName;
            model.RegTimeSpan = mallConfigInfo.RegTimeSpan;
            model.IsWebcomeMsg = mallConfigInfo.IsWebcomeMsg;
            model.WebcomeMsg = mallConfigInfo.WebcomeMsg;
            model.LoginType = StringToIntArray(mallConfigInfo.LoginType);
            model.ShadowName = mallConfigInfo.ShadowName;
            model.IsRemember = mallConfigInfo.IsRemember;
            model.LoginFailTimes = mallConfigInfo.LoginFailTimes;

            return View(model);
        }

        /// <summary>
        /// 账号
        /// </summary>
        [HttpPost]
        public ActionResult Account(AccountModel model)
        {
            if (ModelState.IsValid)
            {
                MallConfigInfo mallConfigInfo = BMAConfig.MallConfig;

                mallConfigInfo.RegType = model.RegType == null ? "" : CommonHelper.IntArrayToString(model.RegType, "");
                mallConfigInfo.ReservedName = model.ReservedName ?? "";
                mallConfigInfo.RegTimeSpan = model.RegTimeSpan;
                mallConfigInfo.IsWebcomeMsg = model.IsWebcomeMsg;
                mallConfigInfo.WebcomeMsg = model.WebcomeMsg ?? "";
                mallConfigInfo.LoginType = model.LoginType == null ? "" : CommonHelper.IntArrayToString(model.LoginType, "");
                mallConfigInfo.ShadowName = model.ShadowName ?? "";
                mallConfigInfo.IsRemember = model.IsRemember;
                mallConfigInfo.LoginFailTimes = model.LoginFailTimes;

                BMAConfig.SaveMallConfig(mallConfigInfo);
                Emails.ResetMall();
                SMSes.ResetMall();
                AddMallAdminLog("修改账号设置");
                return PromptView(Url.Action("account"), "修改账号设置成功");
            }
            return View(model);
        }

        private int[] StringToIntArray(string s)
        {
            if (s != null && s.Length > 0)
            {
                int[] array = new int[s.Length];
                for (int i = 0; i < s.Length; i++)
                {
                    array[i] = TypeHelper.ObjectToInt(s[i]);
                }
                return array;
            }

            return new int[0];
        }

        /// <summary>
        /// 上传
        /// </summary>
        [HttpGet]
        public ActionResult Upload()
        {
            UploadConfigInfo uploadConfigInfo = BMAConfig.UploadConfig;

            UploadModel model = new UploadModel();
            model.UploadImgType = uploadConfigInfo.UploadImgType;
            model.UploadImgSize = uploadConfigInfo.UploadImgSize / 1000;
            model.WatermarkType = uploadConfigInfo.WatermarkType;
            model.WatermarkQuality = uploadConfigInfo.WatermarkQuality;
            model.WatermarkPosition = uploadConfigInfo.WatermarkPosition;
            model.WatermarkImg = uploadConfigInfo.WatermarkImg;
            model.WatermarkImgOpacity = uploadConfigInfo.WatermarkImgOpacity;
            model.WatermarkText = uploadConfigInfo.WatermarkText;
            model.WatermarkTextFont = uploadConfigInfo.WatermarkTextFont;
            model.WatermarkTextSize = uploadConfigInfo.WatermarkTextSize;
            model.BrandThumbSize = uploadConfigInfo.BrandThumbSize;
            model.ProductShowThumbSize = uploadConfigInfo.ProductShowThumbSize;
            model.UserAvatarThumbSize = uploadConfigInfo.UserAvatarThumbSize;
            model.UserRankAvatarThumbSize = uploadConfigInfo.UserRankAvatarThumbSize;
            model.StoreRankAvatarThumbSize = uploadConfigInfo.StoreRankAvatarThumbSize;
            model.StoreLogoThumbSize = uploadConfigInfo.StoreLogoThumbSize;

            LoadFont();
            return View(model);
        }

        /// <summary>
        /// 上传
        /// </summary>
        [HttpPost]
        public ActionResult Upload(UploadModel model)
        {
            if (ModelState.IsValid)
            {
                UploadConfigInfo uploadConfigInfo = BMAConfig.UploadConfig;

                uploadConfigInfo.UploadImgType = model.UploadImgType;
                uploadConfigInfo.UploadImgSize = model.UploadImgSize * 1000;
                uploadConfigInfo.WatermarkType = model.WatermarkType;
                uploadConfigInfo.WatermarkQuality = model.WatermarkQuality;
                uploadConfigInfo.WatermarkPosition = model.WatermarkPosition;
                uploadConfigInfo.WatermarkImg = model.WatermarkImg == null ? "" : model.WatermarkImg;
                uploadConfigInfo.WatermarkImgOpacity = model.WatermarkImgOpacity;
                uploadConfigInfo.WatermarkText = model.WatermarkText == null ? "" : model.WatermarkText;
                uploadConfigInfo.WatermarkTextFont = model.WatermarkTextFont;
                uploadConfigInfo.WatermarkTextSize = model.WatermarkTextSize;
                uploadConfigInfo.BrandThumbSize = model.BrandThumbSize;
                uploadConfigInfo.ProductShowThumbSize = model.ProductShowThumbSize;
                uploadConfigInfo.UserAvatarThumbSize = model.UserAvatarThumbSize;
                uploadConfigInfo.UserRankAvatarThumbSize = model.UserRankAvatarThumbSize;
                uploadConfigInfo.StoreRankAvatarThumbSize = model.StoreRankAvatarThumbSize;
                uploadConfigInfo.StoreLogoThumbSize = model.StoreLogoThumbSize;

                BMAConfig.SaveUploadConfig(uploadConfigInfo);
                AddMallAdminLog("修改上传设置");
                return PromptView(Url.Action("upload"), "修改上传设置成功");
            }

            LoadFont();
            return View(model);
        }

        private void LoadFont()
        {
            List<SelectListItem> itemList = new List<SelectListItem>();
            InstalledFontCollection fontList = new InstalledFontCollection();
            foreach (FontFamily family in fontList.Families)
                itemList.Add(new SelectListItem() { Text = family.Name, Value = family.Name });
            ViewData["fontList"] = itemList;
        }

        /// <summary>
        /// 性能
        /// </summary>
        [HttpGet]
        public ActionResult Performance()
        {
            MallConfigInfo mallConfigInfo = BMAConfig.MallConfig;

            PerformanceModel model = new PerformanceModel();
            model.ImageCDN = mallConfigInfo.ImageCDN;
            model.CSSCDN = mallConfigInfo.CSSCDN;
            model.ScriptCDN = mallConfigInfo.ScriptCDN;
            model.OnlineUserExpire = mallConfigInfo.OnlineUserExpire;
            model.UpdateOnlineTimeSpan = mallConfigInfo.UpdateOnlineTimeSpan;
            model.MaxOnlineCount = mallConfigInfo.MaxOnlineCount;
            model.OnlineCountExpire = mallConfigInfo.OnlineCountExpire;
            model.IsStatBrowser = mallConfigInfo.IsStatBrowser;
            model.IsStatOS = mallConfigInfo.IsStatOS;
            model.IsStatRegion = mallConfigInfo.IsStatRegion;

            return View(model);
        }

        /// <summary>
        /// 性能
        /// </summary>
        [HttpPost]
        public ActionResult Performance(PerformanceModel model)
        {
            if (ModelState.IsValid)
            {
                MallConfigInfo mallConfigInfo = BMAConfig.MallConfig;

                mallConfigInfo.ImageCDN = model.ImageCDN == null ? "" : model.ImageCDN;
                mallConfigInfo.CSSCDN = model.CSSCDN == null ? "" : model.CSSCDN;
                mallConfigInfo.ScriptCDN = model.ScriptCDN == null ? "" : model.ScriptCDN;
                mallConfigInfo.OnlineUserExpire = model.OnlineUserExpire;
                mallConfigInfo.UpdateOnlineTimeSpan = model.UpdateOnlineTimeSpan;
                mallConfigInfo.MaxOnlineCount = model.MaxOnlineCount;
                mallConfigInfo.OnlineCountExpire = model.OnlineCountExpire;
                mallConfigInfo.IsStatBrowser = model.IsStatBrowser;
                mallConfigInfo.IsStatOS = model.IsStatOS;
                mallConfigInfo.IsStatRegion = model.IsStatRegion;

                BMAConfig.SaveMallConfig(mallConfigInfo);
                Emails.ResetMall();
                SMSes.ResetMall();
                AddMallAdminLog("修改性能设置");
                return PromptView(Url.Action("performance"), "修改性能设置成功");
            }
            return View(model);
        }

        /// <summary>
        /// 访问
        /// </summary>
        [HttpGet]
        public ActionResult Access()
        {
            MallConfigInfo mallConfigInfo = BMAConfig.MallConfig;

            AccessModel model = new AccessModel();
            model.IsClosed = mallConfigInfo.IsClosed;
            model.CloseReason = mallConfigInfo.CloseReason;
            model.BanAccessTime = mallConfigInfo.BanAccessTime;
            model.BanAccessIP = mallConfigInfo.BanAccessIP;
            model.AllowAccessIP = mallConfigInfo.AllowAccessIP;
            model.AdminAllowAccessIP = mallConfigInfo.AdminAllowAccessIP;
            model.SecretKey = mallConfigInfo.SecretKey;
            model.CookieDomain = mallConfigInfo.CookieDomain;
            model.RandomLibrary = mallConfigInfo.RandomLibrary;
            model.VerifyPages = StringHelper.SplitString(mallConfigInfo.VerifyPages);
            model.IgnoreWords = mallConfigInfo.IgnoreWords;
            model.AllowEmailProvider = mallConfigInfo.AllowEmailProvider;
            model.BanEmailProvider = mallConfigInfo.BanEmailProvider;

            ViewData["verifyPages"] = mallConfigInfo.VerifyPages;
            return View(model);
        }

        /// <summary>
        /// 访问
        /// </summary>
        [HttpPost]
        public ActionResult Access(AccessModel model)
        {
            if (ModelState.IsValid)
            {
                MallConfigInfo mallConfigInfo = BMAConfig.MallConfig;

                mallConfigInfo.IsClosed = model.IsClosed;
                mallConfigInfo.CloseReason = model.CloseReason == null ? "" : model.CloseReason;
                mallConfigInfo.BanAccessTime = model.BanAccessTime == null ? "" : model.BanAccessTime;
                mallConfigInfo.BanAccessIP = model.BanAccessIP == null ? "" : model.BanAccessIP;
                mallConfigInfo.AllowAccessIP = model.AllowAccessIP == null ? "" : model.AllowAccessIP;
                mallConfigInfo.AdminAllowAccessIP = model.AdminAllowAccessIP == null ? "" : model.AdminAllowAccessIP;
                mallConfigInfo.SecretKey = model.SecretKey;
                mallConfigInfo.CookieDomain = model.CookieDomain == null ? "" : model.CookieDomain.Trim('.');
                mallConfigInfo.RandomLibrary = model.RandomLibrary == null ? "" : model.RandomLibrary;
                mallConfigInfo.VerifyPages = CommonHelper.StringArrayToString(model.VerifyPages);
                mallConfigInfo.IgnoreWords = model.IgnoreWords == null ? "" : model.IgnoreWords;
                mallConfigInfo.AllowEmailProvider = model.AllowEmailProvider == null ? "" : model.AllowEmailProvider;
                mallConfigInfo.BanEmailProvider = model.BanEmailProvider == null ? "" : model.BanEmailProvider;

                BMAConfig.SaveMallConfig(mallConfigInfo);
                Emails.ResetMall();
                SMSes.ResetMall();
                Randoms.ResetRandomLibrary();
                FilterWords.ResetIgnoreWordsRegex();
                AddMallAdminLog("修改访问控制");
                return PromptView(Url.Action("access"), "修改访问控制成功");
            }

            ViewData["verifyPages"] = CommonHelper.StringArrayToString(model.VerifyPages);
            return View(model);
        }

        /// <summary>
        /// 邮箱
        /// </summary>
        [HttpGet]
        public ActionResult Email()
        {
            EmailConfigInfo emailConfigInfo = BMAConfig.EmailConfig;

            EmailModel model = new EmailModel();
            model.Host = emailConfigInfo.Host;
            model.Port = emailConfigInfo.Port;
            model.From = emailConfigInfo.From;
            model.FromName = emailConfigInfo.FromName;
            model.UserName = emailConfigInfo.UserName;
            model.Password = emailConfigInfo.Password;
            model.FindPwdBody = emailConfigInfo.FindPwdBody;
            model.SCVerifyBody = emailConfigInfo.SCVerifyBody;
            model.SCUpdateBody = emailConfigInfo.SCUpdateBody;
            model.WebcomeBody = emailConfigInfo.WebcomeBody;

            return View(model);
        }

        /// <summary>
        /// 邮箱
        /// </summary>
        [HttpPost]
        public ActionResult Email(EmailModel model)
        {
            if (ModelState.IsValid)
            {
                EmailConfigInfo emailConfigInfo = BMAConfig.EmailConfig;

                emailConfigInfo.Host = model.Host;
                emailConfigInfo.Port = model.Port;
                emailConfigInfo.From = model.From;
                emailConfigInfo.FromName = model.FromName;
                emailConfigInfo.UserName = model.UserName;
                emailConfigInfo.Password = model.Password;
                emailConfigInfo.FindPwdBody = model.FindPwdBody;
                emailConfigInfo.SCVerifyBody = model.SCVerifyBody;
                emailConfigInfo.SCUpdateBody = model.SCUpdateBody;
                emailConfigInfo.WebcomeBody = model.WebcomeBody;

                BMAConfig.SaveEmailConfig(emailConfigInfo);
                Emails.ResetEmail();
                AddMallAdminLog("修改邮箱设置");
                return PromptView(Url.Action("email"), "修改邮箱设置成功");
            }
            return View(model);
        }

        /// <summary>
        /// 短信
        /// </summary>
        [HttpGet]
        public ActionResult SMS()
        {
            SMSConfigInfo smsConfigInfo = BMAConfig.SMSConfig;

            SMSModel model = new SMSModel();
            model.Url = smsConfigInfo.Url;
            model.UserName = smsConfigInfo.UserName;
            model.Password = smsConfigInfo.Password;
            model.FindPwdBody = smsConfigInfo.FindPwdBody;
            model.SCVerifyBody = smsConfigInfo.SCVerifyBody;
            model.SCUpdateBody = smsConfigInfo.SCUpdateBody;
            model.WebcomeBody = smsConfigInfo.WebcomeBody;

            return View(model);
        }

        /// <summary>
        /// 短信
        /// </summary>
        [HttpPost]
        public ActionResult SMS(SMSModel model)
        {
            if (ModelState.IsValid)
            {
                SMSConfigInfo smsConfigInfo = BMAConfig.SMSConfig;

                smsConfigInfo.Url = model.Url;
                smsConfigInfo.UserName = model.UserName;
                smsConfigInfo.Password = model.Password;
                smsConfigInfo.FindPwdBody = model.FindPwdBody;
                smsConfigInfo.SCVerifyBody = model.SCVerifyBody;
                smsConfigInfo.SCUpdateBody = model.SCUpdateBody;
                smsConfigInfo.WebcomeBody = model.WebcomeBody;

                BMAConfig.SaveSMSConfig(smsConfigInfo);
                SMSes.ResetSMS();
                AddMallAdminLog("修改短信设置");
                return PromptView(Url.Action("sms"), "修改短信设置成功");
            }
            return View(model);
        }

        /// <summary>
        /// 积分
        /// </summary>
        [HttpGet]
        public ActionResult Credit()
        {
            CreditConfigInfo creditConfigInfo = BMAConfig.CreditConfig;

            CreditModel model = new CreditModel();

            model.PayCreditName = creditConfigInfo.PayCreditName;
            model.PayCreditPrice = creditConfigInfo.PayCreditPrice;
            model.DayMaxSendPayCredits = creditConfigInfo.DayMaxSendPayCredits;
            model.OrderMaxUsePayCredits = creditConfigInfo.OrderMaxUsePayCredits;
            model.RegisterPayCredits = creditConfigInfo.RegisterPayCredits;
            model.LoginPayCredits = creditConfigInfo.LoginPayCredits;
            model.VerifyEmailPayCredits = creditConfigInfo.VerifyEmailPayCredits;
            model.VerifyMobilePayCredits = creditConfigInfo.VerifyMobilePayCredits;
            model.CompleteUserInfoPayCredits = creditConfigInfo.CompleteUserInfoPayCredits;
            model.ReceiveOrderPayCredits = creditConfigInfo.ReceiveOrderPayCredits;
            model.ReviewProductPayCredits = creditConfigInfo.ReviewProductPayCredits;

            model.RankCreditName = creditConfigInfo.RankCreditName;
            model.DayMaxSendRankCredits = creditConfigInfo.DayMaxSendRankCredits;
            model.RegisterRankCredits = creditConfigInfo.RegisterRankCredits;
            model.LoginRankCredits = creditConfigInfo.LoginRankCredits;
            model.VerifyEmailRankCredits = creditConfigInfo.VerifyEmailRankCredits;
            model.VerifyMobileRankCredits = creditConfigInfo.VerifyMobileRankCredits;
            model.CompleteUserInfoRankCredits = creditConfigInfo.CompleteUserInfoRankCredits;
            model.ReceiveOrderRankCredits = creditConfigInfo.ReceiveOrderRankCredits;
            model.ReviewProductRankCredits = creditConfigInfo.ReviewProductRankCredits;

            return View(model);
        }

        /// <summary>
        /// 积分
        /// </summary>
        [HttpPost]
        public ActionResult Credit(CreditModel model)
        {
            if (ModelState.IsValid)
            {
                CreditConfigInfo creditConfigInfo = BMAConfig.CreditConfig;

                creditConfigInfo.PayCreditName = model.PayCreditName;
                creditConfigInfo.PayCreditPrice = model.PayCreditPrice;
                creditConfigInfo.DayMaxSendPayCredits = model.DayMaxSendPayCredits;
                creditConfigInfo.OrderMaxUsePayCredits = model.OrderMaxUsePayCredits;
                creditConfigInfo.RegisterPayCredits = model.RegisterPayCredits;
                creditConfigInfo.LoginPayCredits = model.LoginPayCredits;
                creditConfigInfo.VerifyEmailPayCredits = model.VerifyEmailPayCredits;
                creditConfigInfo.VerifyMobilePayCredits = model.VerifyMobilePayCredits;
                creditConfigInfo.CompleteUserInfoPayCredits = model.CompleteUserInfoPayCredits;
                creditConfigInfo.ReceiveOrderPayCredits = model.ReceiveOrderPayCredits;
                creditConfigInfo.ReviewProductPayCredits = model.ReviewProductPayCredits;

                creditConfigInfo.RankCreditName = model.RankCreditName;
                creditConfigInfo.DayMaxSendRankCredits = model.DayMaxSendRankCredits;
                creditConfigInfo.RegisterRankCredits = model.RegisterRankCredits;
                creditConfigInfo.LoginRankCredits = model.LoginRankCredits;
                creditConfigInfo.VerifyEmailRankCredits = model.VerifyEmailRankCredits;
                creditConfigInfo.VerifyMobileRankCredits = model.VerifyMobileRankCredits;
                creditConfigInfo.CompleteUserInfoRankCredits = model.CompleteUserInfoRankCredits;
                creditConfigInfo.ReceiveOrderRankCredits = model.ReceiveOrderRankCredits;
                creditConfigInfo.ReviewProductRankCredits = model.ReviewProductRankCredits;

                BMAConfig.SaveCreditConfig(creditConfigInfo);
                Credits.ResetCreditConfig();
                AddMallAdminLog("修改积分设置");
                return PromptView(Url.Action("credit"), "修改积分设置成功");
            }
            return View(model);
        }

        /// <summary>
        /// 商城设置
        /// </summary>
        [HttpGet]
        public ActionResult Mall()
        {
            MallConfigInfo mallConfigInfo = BMAConfig.MallConfig;

            MallModel model = new MallModel();
            model.IsGuestSC = mallConfigInfo.IsGuestSC;
            model.SCSubmitType = mallConfigInfo.SCSubmitType;
            model.GuestSCCount = mallConfigInfo.GuestSCCount;
            model.MemberSCCount = mallConfigInfo.MemberSCCount;
            model.SCExpire = mallConfigInfo.SCExpire;
            model.OSNFormat = mallConfigInfo.OSNFormat;
            model.OnlinePayExpire = mallConfigInfo.OnlinePayExpire;
            model.ReceiveExpire = mallConfigInfo.ReceiveExpire;
            model.BroHisCount = mallConfigInfo.BroHisCount;
            model.MaxShipAddress = mallConfigInfo.MaxShipAddress;
            model.FavoriteProductCount = mallConfigInfo.FavoriteProductCount;
            model.FavoriteStoreCount = mallConfigInfo.FavoriteStoreCount;

            return View(model);
        }

        /// <summary>
        /// 商城设置
        /// </summary>
        [HttpPost]
        public ActionResult Mall(MallModel model)
        {
            if (ModelState.IsValid)
            {
                MallConfigInfo mallConfigInfo = BMAConfig.MallConfig;

                mallConfigInfo.IsGuestSC = model.IsGuestSC;
                mallConfigInfo.SCSubmitType = model.SCSubmitType;
                mallConfigInfo.GuestSCCount = model.GuestSCCount;
                mallConfigInfo.MemberSCCount = model.MemberSCCount;
                mallConfigInfo.SCExpire = model.SCExpire;
                mallConfigInfo.OSNFormat = model.OSNFormat;
                mallConfigInfo.OnlinePayExpire = model.OnlinePayExpire;
                mallConfigInfo.ReceiveExpire = model.ReceiveExpire;
                mallConfigInfo.BroHisCount = model.BroHisCount;
                mallConfigInfo.MaxShipAddress = model.MaxShipAddress;
                mallConfigInfo.FavoriteProductCount = model.FavoriteProductCount;
                mallConfigInfo.FavoriteStoreCount = model.FavoriteStoreCount;

                BMAConfig.SaveMallConfig(mallConfigInfo);
                Emails.ResetMall();
                SMSes.ResetMall();
                AddMallAdminLog("修改商城设置");
                return PromptView(Url.Action("mall"), "修改商城设置成功");
            }
            return View(model);
        }

        /// <summary>
        /// 打印订单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PrintOrder()
        {
            string content = System.IO.File.ReadAllText(IOHelper.GetMapPath("/admin_mall/views/order/printorder.cshtml"), Encoding.UTF8);
            return View((object)content);
        }

        /// <summary>
        /// 打印订单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PrintOrder(string content)
        {
            System.IO.File.WriteAllText(IOHelper.GetMapPath("/admin_mall/views/order/printorder.cshtml"), content, Encoding.UTF8);
            return PromptView(Url.Action("printorder"), "修改成功");
        }
    }
}
