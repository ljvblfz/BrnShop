using System;
using System.Xml;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Generic;

using BrnMall.Core;
using BrnMall.Services;
using BrnMall.Web.Framework;
using BrnMall.PayPlugin.WeChat;

using Newtonsoft.Json;

namespace BrnMall.Web.Mobile.Controllers
{
    /// <summary>
    /// 移动微信支付控制器类
    /// </summary>
    public class WeChatController : Controller
    {
        /// <summary>
        /// 支付
        /// </summary>
        public ActionResult Pay()
        {
            //订单id列表
            string oidList = WebHelper.GetQueryString("oidList");

            LogUtil.WriteLog("oidList:" + oidList);

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

            string code = Request.QueryString["code"];
            if (string.IsNullOrEmpty(code))
            {
                string code_url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state=lk#wechat_redirect", PayConfig.AppId, string.Format("http://{0}/mob/wechat/pay?oidList=" + oidList, BMAConfig.MallConfig.SiteUrl));
                LogUtil.WriteLog("code_url:" + code_url);
                return Redirect(code_url);
            }

            LogUtil.WriteLog(" ============ 开始 获取微信用户相关信息 =====================");

            #region 获取支付用户 OpenID================
            string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", PayConfig.AppId, PayConfig.AppSecret, code);
            string returnStr = HttpUtil.Send("", url);
            LogUtil.WriteLog("Send 页面  returnStr 第一个：" + returnStr);

            OpenModel openModel = JsonConvert.DeserializeObject<OpenModel>(returnStr);

            url = string.Format("https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}", PayConfig.AppId, openModel.refresh_token);
            returnStr = HttpUtil.Send("", url);
            openModel = JsonConvert.DeserializeObject<OpenModel>(returnStr);

            LogUtil.WriteLog("Send 页面  access_token：" + openModel.access_token);
            LogUtil.WriteLog("Send 页面  openid=" + openModel.openid);

            //url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}", obj.access_token, obj.openid);
            //returnStr = HttpUtil.Send("", url);
            //LogUtil.WriteLog("Send 页面  returnStr：" + returnStr);
            #endregion

            LogUtil.WriteLog(" ============ 结束 获取微信用户相关信息 =====================");

            LogUtil.WriteLog("============ 单次支付开始 ===============");

            #region 支付操作============================


            #region 基本参数===========================
            //商户订单号
            string outTradeNo = oidList.Replace(',', 's') + Randoms.CreateRandomValue(10, true);
            //订单名称
            string subject = BMAConfig.MallConfig.SiteTitle + "购物";
            //付款金额
            string totalFee = ((int)(allSurplusMoney * 100)).ToString();
            //openId
            string openId = openModel.openid;
            //时间戳 
            string timeStamp = TenpayUtil.getTimestamp();
            //随机字符串 
            string nonceStr = TenpayUtil.getNoncestr();
            //服务器异步通知页面路径
            string notifyUrl = string.Format("http://{0}/mob/wechat/notify", BMAConfig.MallConfig.SiteUrl);

            LogUtil.WriteLog("totalFee" + totalFee);

            //创建支付应答对象
            RequestHandler packageReqHandler = new RequestHandler(System.Web.HttpContext.Current);
            //初始化
            packageReqHandler.init();

            //设置package订单参数  具体参数列表请参考官方pdf文档，请勿随意设置
            packageReqHandler.setParameter("body", subject); //商品信息 127字符
            packageReqHandler.setParameter("appid", PayConfig.AppId);
            packageReqHandler.setParameter("mch_id", PayConfig.MchId);
            packageReqHandler.setParameter("nonce_str", nonceStr.ToLower());
            packageReqHandler.setParameter("notify_url", notifyUrl);
            packageReqHandler.setParameter("openid", openId);
            packageReqHandler.setParameter("out_trade_no", outTradeNo); //商家订单号
            packageReqHandler.setParameter("spbill_create_ip", Request.UserHostAddress); //用户的公网ip，不是商户服务器IP
            packageReqHandler.setParameter("total_fee", totalFee); //商品金额,以分为单位(money * 100).ToString()
            packageReqHandler.setParameter("trade_type", "JSAPI");
            //if (!string.IsNullOrEmpty(this.Attach))
            //    packageReqHandler.setParameter("attach", this.Attach);//自定义参数 127字符

            #endregion

            #region sign===============================
            string sign = packageReqHandler.CreateMd5Sign("key", PayConfig.AppKey);
            LogUtil.WriteLog("WeiPay 页面  sign：" + sign);
            #endregion

            #region 获取package包======================
            packageReqHandler.setParameter("sign", sign);

            string data = packageReqHandler.parseXML();
            LogUtil.WriteLog("WeiPay 页面  package（XML）：" + data);

            string prepayXml = HttpUtil.Send(data, "https://api.mch.weixin.qq.com/pay/unifiedorder");
            LogUtil.WriteLog("WeiPay 页面  package（Back_XML）：" + prepayXml);

            //获取预支付ID
            string prepayId = "";
            string package = "";
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(prepayXml);
            XmlNode xn = xdoc.SelectSingleNode("xml");
            XmlNodeList xnl = xn.ChildNodes;
            if (xnl.Count > 7)
            {
                prepayId = xnl[7].InnerText;
                package = string.Format("prepay_id={0}", prepayId);
                LogUtil.WriteLog("WeiPay 页面  package：" + package);
            }
            #endregion

            #region 设置支付参数 输出页面  该部分参数请勿随意修改 ==============
            RequestHandler paySignReqHandler = new RequestHandler(System.Web.HttpContext.Current);
            paySignReqHandler.setParameter("appId", PayConfig.AppId);
            paySignReqHandler.setParameter("timeStamp", timeStamp);
            paySignReqHandler.setParameter("nonceStr", nonceStr);
            paySignReqHandler.setParameter("package", package);
            paySignReqHandler.setParameter("signType", "MD5");
            string paySign = paySignReqHandler.CreateMd5Sign("key", PayConfig.AppKey);

            LogUtil.WriteLog("WeiPay 页面  paySign：" + paySign);
            #endregion
            #endregion

            Dictionary<string, string> model = new Dictionary<string, string>();
            model.Add("oidList", oidList);
            model.Add("timeStamp", timeStamp);
            model.Add("nonceStr", nonceStr);
            model.Add("package", package);
            model.Add("paySign", paySign);
            return PartialView("~/plugins/BrnMall.PayPlugin.WeChat/views/wechat/pay.cshtml", model);
        }

        /// <summary>
        /// 通知调用
        /// </summary>
        public ActionResult Notify()
        {
            //创建ResponseHandler实例
            ResponseHandler resHandler = new ResponseHandler(System.Web.HttpContext.Current);
            resHandler.setKey(PayConfig.AppKey);

            //判断签名
            try
            {
                string error = "";
                if (resHandler.isWXsign(out error))
                {
                    #region 协议参数=====================================
                    //--------------协议参数--------------------------------------------------------
                    //SUCCESS/FAIL此字段是通信标识，非交易标识，交易是否成功需要查
                    string return_code = resHandler.getParameter("return_code");
                    //返回信息，如非空，为错误原因签名失败参数格式校验错误
                    string return_msg = resHandler.getParameter("return_msg");
                    //微信分配的公众账号 ID
                    string appid = resHandler.getParameter("appid");

                    //以下字段在 return_code 为 SUCCESS 的时候有返回--------------------------------
                    //微信支付分配的商户号
                    string mch_id = resHandler.getParameter("mch_id");
                    //微信支付分配的终端设备号
                    string device_info = resHandler.getParameter("device_info");
                    //微信分配的公众账号 ID
                    string nonce_str = resHandler.getParameter("nonce_str");
                    //业务结果 SUCCESS/FAIL
                    string result_code = resHandler.getParameter("result_code");
                    //错误代码 
                    string err_code = resHandler.getParameter("err_code");
                    //结果信息描述
                    string err_code_des = resHandler.getParameter("err_code_des");

                    //以下字段在 return_code 和 result_code 都为 SUCCESS 的时候有返回---------------
                    //-------------业务参数---------------------------------------------------------
                    //用户在商户 appid 下的唯一标识
                    string openid = resHandler.getParameter("openid");
                    //用户是否关注公众账号，Y-关注，N-未关注，仅在公众账号类型支付有效
                    string is_subscribe = resHandler.getParameter("is_subscribe");
                    //JSAPI、NATIVE、MICROPAY、APP
                    string trade_type = resHandler.getParameter("trade_type");
                    //银行类型，采用字符串类型的银行标识
                    string bank_type = resHandler.getParameter("bank_type");
                    //订单总金额，单位为分
                    string total_fee = resHandler.getParameter("total_fee");
                    //货币类型，符合 ISO 4217 标准的三位字母代码，默认人民币：CNY
                    string fee_type = resHandler.getParameter("fee_type");
                    //微信支付订单号
                    string transaction_id = resHandler.getParameter("transaction_id");
                    //商户系统的订单号，与请求一致。
                    string out_trade_no = resHandler.getParameter("out_trade_no");
                    //商家数据包，原样返回
                    string attach = resHandler.getParameter("attach");
                    //支 付 完 成 时 间 ， 格 式 为yyyyMMddhhmmss，如 2009 年12 月27日 9点 10分 10 秒表示为 20091227091010。时区为 GMT+8 beijing。该时间取自微信支付服务器
                    string time_end = resHandler.getParameter("time_end");

                    #endregion
                    //支付成功
                    if (!out_trade_no.Equals("") && return_code.Equals("SUCCESS") && result_code.Equals("SUCCESS"))
                    {
                        LogUtil.WriteLog("Notify 页面  支付成功，支付信息：商家订单号：" + out_trade_no + "、支付金额(分)：" + total_fee + "、自定义参数：" + attach);

                        /**
                         *  这里输入用户逻辑操作，比如更新订单的支付状态
                         * 
                         * **/

                        List<OrderInfo> orderList = new List<OrderInfo>();
                        string oidList = StringHelper.SubString(out_trade_no, out_trade_no.Length - 10);
                        foreach (string oid in StringHelper.SplitString(oidList, "s"))
                        {
                            OrderInfo orderInfo = Orders.GetOrderByOid(TypeHelper.StringToInt(oid));
                            orderList.Add(orderInfo);
                        }
                        decimal allSurplusMoney = 0M;
                        foreach (OrderInfo orderInfo in orderList)
                        {
                            allSurplusMoney += orderInfo.SurplusMoney;
                        }

                        if (orderList.Count > 0 && allSurplusMoney <= TypeHelper.StringToDecimal(total_fee))
                        {
                            foreach (OrderInfo orderInfo in orderList)
                            {
                                if (orderInfo.SurplusMoney > 0 && orderInfo.OrderState == (int)OrderState.WaitPaying)
                                {
                                    Orders.PayOrder(orderInfo.Oid, OrderState.Confirming, transaction_id, "wechatpay", "微信支付", DateTime.Now);
                                    OrderActions.CreateOrderAction(new OrderActionInfo()
                                    {
                                        Oid = orderInfo.Oid,
                                        Uid = orderInfo.Uid,
                                        RealName = "本人",
                                        ActionType = (int)OrderActionType.Pay,
                                        ActionTime = TypeHelper.StringToDateTime(time_end),
                                        ActionDes = "你使用微信支付订单成功，微信交易号为:" + transaction_id
                                    });
                                }
                            }
                        }

                        LogUtil.WriteLog("============ 单次支付结束 ===============");
                        return Content("success");
                    }
                    else
                    {
                        LogUtil.WriteLog("Notify 页面  支付失败，支付信息   total_fee= " + total_fee + "、err_code_des=" + err_code_des + "、result_code=" + result_code);
                        return new EmptyResult();
                    }
                }
                else
                {
                    LogUtil.WriteLog("Notify 页面  isWXsign= false ，错误信息：" + error);
                    return new EmptyResult();
                }
            }
            catch (Exception ee)
            {
                LogUtil.WriteLog("Notify 页面  发送异常错误：" + ee.Message);
                return new EmptyResult();
            }
        }
    }
}
