using System;

namespace BrnMall.Core
{
    /// <summary>
    /// 商城后台操作信息类
    /// </summary>
    public class MallAdminActionInfo
    {
        private int _aid;//操作id
        private string _title;//操作标题
        private string _action;//操作行为
        private int _parentid;//父id
        private int _displayorder;//排序

        /// <summary>
        /// 操作id
        /// </summary>
        public int Aid
        {
            get { return _aid; }
            set { _aid = value; }
        }
        /// <summary>
        /// 操作标题
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value.TrimEnd(); }
        }
        /// <summary>
        /// 操作行为
        /// </summary>
        public string Action
        {
            get { return _action; }
            set { _action = value.TrimEnd(); }
        }
        /// <summary>
        /// 父id
        /// </summary>
        public int ParentId
        {
            get { return _parentid; }
            set { _parentid = value; }
        }
        /// <summary>
        /// 排序
        /// </summary>
        public int DisplayOrder
        {
            get { return _displayorder; }
            set { _displayorder = value; }
        }
    }
}
