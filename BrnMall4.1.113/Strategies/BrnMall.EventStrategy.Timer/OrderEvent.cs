using System;

using BrnMall.Core;
using BrnMall.Services;

namespace BrnMall.EventStrategy.Timer
{
    /// <summary>
    /// 订单事件
    /// </summary>
    public class OrderEvent : IEvent
    {
        public void Execute(object eventInfo)
        {
            EventInfo e = (EventInfo)eventInfo;

            //清空过期的在线支付订单
            DateTime expireTime1 = DateTime.Now.AddHours(-BMAConfig.MallConfig.OnlinePayExpire);
            Orders.ClearExpiredOnlinePayOrder(expireTime1);

            //自动收货
            DateTime expireTime2 = DateTime.Now.AddDays(-BMAConfig.MallConfig.ReceiveExpire);
            Orders.AutoReceiveOrder(expireTime2);

            EventLogs.CreateEventLog(e.Key, e.Title, Environment.MachineName, DateTime.Now);
        }
    }
}
