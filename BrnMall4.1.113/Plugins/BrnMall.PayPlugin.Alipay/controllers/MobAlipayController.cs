using System;
using System.Web;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Generic;
using System.Collections.Specialized;

using BrnMall.Core;
using BrnMall.Services;
using BrnMall.Web.Framework;

using Com.Alipay;

namespace BrnMall.Web.Mobile.Controllers
{
    /// <summary>
    /// 移动支付宝控制器类
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
                    return Redirect("/mob");
                allSurplusMoney += orderInfo.SurplusMoney;
            }

            if (orderList.Count < 1 || allSurplusMoney == 0M)
                return Redirect("/mob");

            //支付类型
            string payment_type = "1";

            //服务器异步通知页面路径，需http://格式的完整路径，不能加?id=123这类自定义参数
            string notify_url = string.Format("http://{0}/mob/alipay/notify", BMAConfig.MallConfig.SiteUrl);
            //页面跳转同步通知页面路径，需http://格式的完整路径，不能加?id=123这类自定义参数，不能写成http://localhost/
            string return_url = string.Format("http://{0}/mob/alipay/return", BMAConfig.MallConfig.SiteUrl);

            //商户订单号
            string out_trade_no = oidList + Randoms.CreateRandomValue(10, false);
            //商户网站订单系统中唯一订单号，必填

            //订单名称
            string subject = BMAConfig.MallConfig.SiteTitle + "购物";
            //必填

            //付款金额
            string total_fee = allSurplusMoney.ToString();
            //必填

            //商品展示地址
            string show_url = string.Format("http://{0}/images/alipay.jgp", BMAConfig.MallConfig.SiteUrl);
            //必填，需以http://开头的完整路径，例如：http://www.商户网址.com/myorder.html

            //订单描述
            string body = "";
            //选填

            //超时时间
            string it_b_pay = "";
            //选填

            //钱包token
            string extern_token = "";
            //选填


            ////////////////////////////////////////////////////////////////////////////////////////////////

            //把请求参数打包成数组
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner", Config.Partner);
            sParaTemp.Add("seller_id", Config.Seller_id);
            sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
            sParaTemp.Add("service", "alipay.wap.create.direct.pay.by.user");
            sParaTemp.Add("payment_type", payment_type);
            sParaTemp.Add("notify_url", notify_url);
            sParaTemp.Add("return_url", return_url);
            sParaTemp.Add("out_trade_no", out_trade_no);
            sParaTemp.Add("subject", subject);
            sParaTemp.Add("total_fee", total_fee);
            sParaTemp.Add("show_url", show_url);
            sParaTemp.Add("body", body);
            sParaTemp.Add("it_b_pay", it_b_pay);
            sParaTemp.Add("extern_token", extern_token);

            //建立请求
            string sHtmlText = Submit.BuildRequest(sParaTemp, "get", "确认");
            return Content(sHtmlText);
        }

        /// <summary>
        /// 返回调用
        /// </summary>
        public ActionResult Return()
        {
            SortedDictionary<string, string> sPara = GetRequestGet();

            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.Verify(sPara, Request.QueryString["notify_id"], Request.QueryString["sign"]);

                if (verifyResult)//验证成功
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //请在这里加上商户的业务逻辑程序代码


                    //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                    //获取支付宝的通知返回参数，可参考技术文档中页面跳转同步通知参数列表

                    //商户订单号

                    string out_trade_no = Request.QueryString["out_trade_no"];

                    //支付宝交易号

                    string tradeSN = Request.QueryString["trade_no"];

                    //交易状态
                    string trade_status = Request.QueryString["trade_status"];


                    if (Request.QueryString["trade_status"] == "TRADE_FINISHED" || Request.QueryString["trade_status"] == "TRADE_SUCCESS")
                    {
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
            SortedDictionary<string, string> sPara = GetRequestPost();

            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.Verify(sPara, Request.Form["notify_id"], Request.Form["sign"]);

                if (verifyResult)//验证成功
                {
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
                    return Content("fail");
                }
            }
            else
            {
                return Content("无通知参数");
            }
        }

        /// <summary>
        /// 获取支付宝GET过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestGet()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.QueryString;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
            }

            return sArray;
        }

        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }

            return sArray;
        }
    }
}
