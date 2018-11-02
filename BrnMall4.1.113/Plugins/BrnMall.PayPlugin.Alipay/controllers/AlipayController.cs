using System;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Generic;

using BrnMall.Core;
using BrnMall.Services;
using BrnMall.Web.Framework;
using BrnMall.PayPlugin.Alipay;

namespace BrnMall.Web.Controllers
{
    /// <summary>
    /// 前台支付宝控制器类
    /// </summary>
    public class AlipayController : Controller
    {
        /// <summary>
        /// 支付
        /// </summary>
        public ActionResult Pay()
        {
            //订单id列表
            string oidList = WebHelper.GetQueryString("oidList");

            decimal allSurplusMoney = 0M;
            List<OrderInfo> orderList = new List<OrderInfo>();
            foreach (string oid in StringHelper.SplitString(oidList))
            {
                //订单信息
                OrderInfo orderInfo = Orders.GetOrderByOid(TypeHelper.StringToInt(oid));
                if (orderInfo != null && orderInfo.OrderState == (int)OrderState.WaitPaying)
                    orderList.Add(orderInfo);
                else
                    return Redirect("/");
                allSurplusMoney += orderInfo.SurplusMoney;
            }

            if (orderList.Count < 1 || allSurplusMoney == 0M)
                return Redirect("/");

            //支付类型，必填，不能修改
            string paymentType = "1";

            //服务器异步通知页面路径，需http://格式的完整路径，不能加?id=123这类自定义参数
            string notifyUrl = string.Format("http://{0}/alipay/notify", BMAConfig.MallConfig.SiteUrl);
            //页面跳转同步通知页面路径，需http://格式的完整路径，不能加?id=123这类自定义参数，不能写成http://localhost/
            string returnUrl = string.Format("http://{0}/alipay/return", BMAConfig.MallConfig.SiteUrl);

            //收款支付宝帐户
            string sellerEmail = AlipayConfig.Seller;
            //合作者身份ID
            string partner = AlipayConfig.Partner;
            //交易安全检验码
            string key = AlipayConfig.Key;

            //商户订单号
            string outTradeNo = oidList + Randoms.CreateRandomValue(10, false);
            //订单名称
            string subject = BMAConfig.MallConfig.SiteTitle + "购物";
            //付款金额
            string totalFee = allSurplusMoney.ToString();
            //订单描述
            string body = "";

            //防钓鱼时间戳,若要使用请调用类文件submit中的query_timestamp函数
            string antiPhishingKey = "";
            //客户端的IP地址,非局域网的外网IP地址，如：221.0.0.1
            string exterInvokeIP = "";

            //把请求参数打包成数组
            SortedDictionary<string, string> parms = new SortedDictionary<string, string>();
            parms.Add("partner", partner);
            parms.Add("_input_charset", AlipayConfig.InputCharset);
            parms.Add("service", "create_direct_pay_by_user");
            parms.Add("payment_type", paymentType);
            parms.Add("notify_url", notifyUrl);
            parms.Add("return_url", returnUrl);
            parms.Add("seller_email", sellerEmail);
            parms.Add("out_trade_no", outTradeNo);
            parms.Add("subject", subject);
            parms.Add("total_fee", totalFee);
            parms.Add("body", body);
            parms.Add("show_url", "");
            parms.Add("anti_phishing_key", antiPhishingKey);
            parms.Add("exter_invoke_ip", exterInvokeIP);

            //建立请求
            string sHtmlText = AlipaySubmit.BuildRequest(parms, AlipayConfig.SignType, AlipayConfig.Key, AlipayConfig.Code, AlipayConfig.Gateway, AlipayConfig.InputCharset, "get", "确认");
            return Content(sHtmlText);
        }

        /// <summary>
        /// 返回调用
        /// </summary>
        public ActionResult Return()
        {
            SortedDictionary<string, string> paras = AlipayCore.GetRequestGet();

            if (paras.Count > 0)//判断是否有带返回参数
            {
                bool verifyResult = AlipayNotify.Verify(paras, Request.QueryString["notify_id"], Request.QueryString["sign"], AlipayConfig.SignType, AlipayConfig.Key, AlipayConfig.Code, AlipayConfig.VeryfyUrl, AlipayConfig.Partner);

                if (verifyResult && (Request.QueryString["trade_status"] == "TRADE_FINISHED" || Request.QueryString["trade_status"] == "TRADE_SUCCESS"))//验证成功
                {
                    string out_trade_no = Request.QueryString["out_trade_no"];//商户订单号
                    string tradeSN = Request.QueryString["trade_no"];//支付宝交易号
                    decimal tradeMoney = TypeHelper.StringToDecimal(Request.QueryString["total_fee"]);//交易金额
                    DateTime tradeTime = TypeHelper.StringToDateTime(Request.QueryString["notify_time"]);//交易时间

                    string oidList = StringHelper.SubString(out_trade_no, out_trade_no.Length - 10);//订单id列表
                    List<OrderInfo> orderList = new List<OrderInfo>();
                    foreach (string oid in StringHelper.SplitString(oidList))
                    {
                        OrderInfo orderInfo = Orders.GetOrderByOid(TypeHelper.StringToInt(oid));
                        orderList.Add(orderInfo);
                    }
                    decimal allSurplusMoney = 0M;
                    foreach (OrderInfo orderInfo in orderList)
                    {
                        allSurplusMoney += orderInfo.SurplusMoney;
                    }

                    if (orderList.Count > 0 && allSurplusMoney <= tradeMoney)
                    {
                        foreach (OrderInfo orderInfo in orderList)
                        {
                            if (orderInfo.SurplusMoney > 0 && orderInfo.OrderState == (int)OrderState.WaitPaying)
                            {
                                Orders.PayOrder(orderInfo.Oid, OrderState.Confirming, tradeSN, "alipay", "支付宝", DateTime.Now);
                                OrderActions.CreateOrderAction(new OrderActionInfo()
                                {
                                    Oid = orderInfo.Oid,
                                    Uid = orderInfo.Uid,
                                    RealName = "本人",
                                    ActionType = (int)OrderActionType.Pay,
                                    ActionTime = tradeTime,
                                    ActionDes = "你使用支付宝支付订单成功，支付宝交易号为:" + tradeSN
                                });
                            }
                        }
                    }

                    return RedirectToAction("payresult", "order", new RouteValueDictionary { { "oidList", oidList } });
                }
                else//验证失败
                {
                    return Content("支付失败");
                }
            }
            else
            {
                return Content("支付失败");
            }
        }

        /// <summary>
        /// 通知调用
        /// </summary>
        public ActionResult Notify()
        {
            SortedDictionary<string, string> paras = AlipayCore.GetRequestPost();

            if (paras.Count > 0)//判断是否有带返回参数
            {
                bool verifyResult = AlipayNotify.Verify(paras, Request.Form["notify_id"], Request.Form["sign"], AlipayConfig.SignType, AlipayConfig.Key, AlipayConfig.Code, AlipayConfig.VeryfyUrl, AlipayConfig.Partner);
                if (verifyResult && (Request.Form["trade_status"] == "TRADE_FINISHED" || Request.Form["trade_status"] == "TRADE_SUCCESS"))//验证成功
                {
                    string out_trade_no = Request.Form["out_trade_no"];//商户订单号
                    string tradeSN = Request.Form["trade_no"];//支付宝交易号
                    decimal tradeMoney = TypeHelper.StringToDecimal(Request.Form["total_fee"]);//交易金额
                    DateTime tradeTime = TypeHelper.StringToDateTime(Request.Form["gmt_payment"]);//交易时间

                    List<OrderInfo> orderList = new List<OrderInfo>();
                    foreach (string oid in StringHelper.SplitString(StringHelper.SubString(out_trade_no, out_trade_no.Length - 10)))
                    {
                        OrderInfo orderInfo = Orders.GetOrderByOid(TypeHelper.StringToInt(oid));
                        orderList.Add(orderInfo);
                    }
                    decimal allSurplusMoney = 0M;
                    foreach (OrderInfo orderInfo in orderList)
                    {
                        allSurplusMoney += orderInfo.SurplusMoney;
                    }

                    if (orderList.Count > 0 && allSurplusMoney <= tradeMoney)
                    {
                        foreach (OrderInfo orderInfo in orderList)
                        {
                            if (orderInfo.SurplusMoney > 0 && orderInfo.OrderState == (int)OrderState.WaitPaying)
                            {
                                Orders.PayOrder(orderInfo.Oid, OrderState.Confirming, tradeSN, "alipay", "支付宝", DateTime.Now);
                                OrderActions.CreateOrderAction(new OrderActionInfo()
                                {
                                    Oid = orderInfo.Oid,
                                    Uid = orderInfo.Uid,
                                    RealName = "本人",
                                    ActionType = (int)OrderActionType.Pay,
                                    ActionTime = tradeTime,
                                    ActionDes = "你使用支付宝支付订单成功，支付宝交易号为:" + tradeSN
                                });
                            }
                        }
                    }

                    return Content("success");
                }
                else//验证失败
                {
                    return Content("fail");
                }
            }
            else
            {
                return Content("无通知参数");
            }
        }
    }
}
