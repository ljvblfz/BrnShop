using System;

using BrnMall.Core;
using BrnMall.Services;

namespace BrnMall.EventStrategy.Timer
{
    /// <summary>
    /// 浏览历史事件
    /// </summary>
    public class BrowseHistoryEvent : IEvent
    {
        public void Execute(object eventInfo)
        {
            EventInfo e = (EventInfo)eventInfo;

            //清空过期浏览历史
            BrowseHistories.ClearExpiredBrowseHistory();

            EventLogs.CreateEventLog(e.Key, e.Title, Environment.MachineName, DateTime.Now);
        }
    }
}
