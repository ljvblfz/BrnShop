using System;

namespace BrnShop.Core
{
    /// <summary>
    /// 日志策略接口
    /// </summary>
    public partial interface ILogStrategy
    {
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="message">消息</param>
        void Write(string message);
    }
}
