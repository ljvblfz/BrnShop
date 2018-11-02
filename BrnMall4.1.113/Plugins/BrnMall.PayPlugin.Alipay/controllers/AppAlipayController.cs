using System;
using System.Web;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Generic;

using BrnMall.Core;
using BrnMall.Services;
using BrnMall.Web.Framework;
using BrnMall.PayPlugin.Alipay;

namespace BrnMall.Web.App.Controllers
{
    /// <summary>
    /// App支付宝控制器类
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
                    return Content("");
                allSurplusMoney += orderInfo.SurplusMoney;
            }

            if (orderList.Count < 1 || allSurplusMoney == 0M)
                return Content("");

            //支付类型，必填，不能修改
            string paymentType = "1";

            //服务器异步通知页面路径，需http://格式的完整路径，不能加?id=123这类自定义参数
            string notifyUrl = string.Format("http://{0}/app/alipay/notify", BMAConfig.MallConfig.SiteUrl);

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

            Encoding e = Encoding.GetEncoding(AlipayConfig.AppInputCharset);

            //把请求参数打包成数组
            SortedDictionary<string, string> parms = new SortedDictionary<string, string>();
            parms.Add("partner", partner);
            parms.Add("seller_id", sellerEmail);
            parms.Add("out_trade_no", outTradeNo);
            parms.Add("subject", subject);
            parms.Add("body", body);
            parms.Add("total_fee", totalFee);
            parms.Add("notify_url", notifyUrl);
            parms.Add("service", "mobile.securitypay.pay");
            parms.Add("payment_type", paymentType);
            parms.Add("_input_charset", AlipayConfig.AppInputCharset);
            parms.Add("it_b_pay", "30m");
            parms.Add("show_url", "m.alipay.com");

            string sign = AlipayRSAFromPkcs8.sign(AlipayCore.CreateLinkString(AlipayCore.FilterPara(parms)), AlipayConfig.PrivateKey, AlipayConfig.AppInputCharset);
            parms.Add("sign", HttpUtility.UrlEncode(sign, e));
            parms.Add("sign_type", AlipayConfig.AppSignType);

            Dictionary<string, string> dicArray = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> temp in parms)
            {
                dicArray.Add(temp.Key, temp.Value);
            }

            string content = AlipayCore.CreateLinkString(dicArray);
            return Content("{ \"orderString\":\"" + content + "\" }");
        }

        /// <summary>
        /// 返回调用
        /// </summary>
        public ActionResult Return()
        {
            SortedDictionary<string, string> sPara = AlipayCore.GetRequestGet();

            if (sPara.Count > 0)//判断是否有带返回参数
            {
                bool verifyResult = AlipayNotify.AppVerify(sPara, Request.QueryString["notify_id"], Request.QueryString["sign"], AlipayConfig.AppSignType, AlipayConfig.PublicKey, AlipayConfig.AppInputCharset, AlipayConfig.AppVeryfyUrl, AlipayConfig.Partner);
                if (verifyResult && (Request.QueryString["trade_status"] == "TRADE_FINISHED" || Request.QueryString["trade_status"] == "TRADE_SUCCESS"))//验证成功
                {
                    string out_trade_no = Request.QueryString["out_trade_no"];//商户订单号
                    string tradeSN = Request.QueryString["trade_no"];//支付宝交易号
                    decimal tradeMoney = TypeHelper.StringToDecimal(Request.QueryString["total_fee"]);//交易金额
                    DateTime tradeTime = TypeHelper.StringToDateTime(Request.QueryString["gmt_payment"]);//交易时间

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

                    return Content("{" + "\"state\":\"success\"" + "}");
                }
                else//验证失败
                {
                    return Content("{" + "\"state\":\"fail\"" + "}");
                }
            }
            else
            {
                return Content("{" + "\"state\":\"fail\"" + "}");
            }
        }

        /// <summary>
        /// 通知调用
        /// </summary>
        public ActionResult Notify()
        {
            SortedDictionary<string, string> sPara = AlipayCore.GetRequestPost();

            if (sPara.Count > 0)//判断是否有带返回参数
            {
                bool verifyResult = AlipayNotify.AppVerify(sPara, Request.Form["notify_id"], Request.Form["sign"], AlipayConfig.AppSignType, AlipayConfig.PublicKey, AlipayConfig.AppInputCharset, AlipayConfig.AppVeryfyUrl, AlipayConfig.Partner);
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
