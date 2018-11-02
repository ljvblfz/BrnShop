using System;

using BrnMall.Core;

namespace BrnMall.Services
{
    /// <summary>
    /// 事件日志操作管理类
    /// </summary>
    public partial class EventLogs
    {
        /// <summary>
        /// 创建事件日志
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="title">标题</param>
        /// <param name="server">服务器名称</param>
        /// <param name="executeTime">执行时间</param>
        public static void CreateEventLog(string key, string title, string server, DateTime executeTime)
        {
            BrnMall.Data.EventLogs.CreateEventLog(key, title, server, executeTime);
        }

        /// <summary>
        /// 获得事件的最后执行时间
        /// </summary>
        /// <param name="key">事件key</param>
        /// <returns></returns>
        public static DateTime GetEventLastExecuteTimeByKey(string key)
        {
            return BrnMall.Data.EventLogs.GetEventLastExecuteTimeByKey(key);
        }
    }
}
