using System;
using System.Collections.Generic;

namespace BrnMall.Core
{
    /// <summary>
    /// 事件配置信息类
    /// </summary>
    [Serializable]
    public class EventConfigInfo : IConfigInfo
    {
        private int _bmaeventstate;//BrnMall事件状态
        private int _bmaeventperiod;//BrnMall事件执行间隔(单位为分钟)
        private List<EventInfo> _bmaeventlist;//BrnMall事件列表

        /// <summary>
        /// BrnMall事件状态
        /// </summary>
        public int BMAEventState
        {
            get { return _bmaeventstate; }
            set { _bmaeventstate = value; }
        }
        /// <summary>
        /// BrnMall事件执行间隔(单位为分钟)
        /// </summary>
        public int BMAEventPeriod
        {
            get { return _bmaeventperiod; }
            set { _bmaeventperiod = value; }
        }
        /// <summary>
        /// BrnMall事件列表
        /// </summary>
        public List<EventInfo> BMAEventList
        {
            get { return _bmaeventlist; }
            set { _bmaeventlist = value; }
        }
    }
}
