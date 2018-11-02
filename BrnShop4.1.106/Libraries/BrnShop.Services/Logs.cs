using System;

using BrnShop.Core;

namespace BrnShop.Services
{
    /// <summary>
    /// 日志操作管理类
    /// </summary>
    public partial class Logs
    {
        private static ILogStrategy _ilogstrategy = BSPLog.Instance;//日志策略

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="message">消息</param>
        public static void Write(string message)
        {
            _ilogstrategy.Write(message);
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="ex">异常对象</param>
        public static void Write(Exception ex)
        {
            _ilogstrategy.Write(string.Format("方法:{0},异常信息:{1}", ex.TargetSite, ex.Message));
        }
    }
}
