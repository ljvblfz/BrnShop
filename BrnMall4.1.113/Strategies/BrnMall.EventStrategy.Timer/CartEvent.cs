using System;

using BrnMall.Core;
using BrnMall.Services;

namespace BrnMall.EventStrategy.Timer
{
    /// <summary>
    /// 购物车事件
    /// </summary>
    public class CartEvent : IEvent
    {
        public void Execute(object eventInfo)
        {
            EventInfo e = (EventInfo)eventInfo;

            //清空过期购物车
            DateTime expireTime = DateTime.Now.AddDays(-BMAConfig.MallConfig.SCExpire);
            Carts.ClearExpiredCart(expireTime);

            EventLogs.CreateEventLog(e.Key, e.Title, Environment.MachineName, DateTime.Now);
        }
    }
}
