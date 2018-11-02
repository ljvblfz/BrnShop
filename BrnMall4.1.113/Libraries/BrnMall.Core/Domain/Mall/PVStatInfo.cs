using System;

namespace BrnMall.Core
{
    /// <summary>
    /// PV统计信息类
    /// </summary>
    public class PVStatInfo
    {
        private int _recordid;//记录id
        private int _storeid;//店铺id
        private string _category;//类别
        private string _value;//值
        private int _count;//数量

        /// <summary>
        /// 记录id
        /// </summary>
        public int RecordId
        {
            get { return _recordid; }
            set { _recordid = value; }
        }
        /// <summary>
        /// 店铺id
        /// </summary>
        public int StoreId
        {
            get { return _storeid; }
            set { _storeid = value; }
        }
        /// <summary>
        /// 类别
        /// </summary>
        public string Category
        {
            get { return _category; }
            set { _category = value.TrimEnd(); }
        }
        /// <summary>
        /// 值
        /// </summary>
        public string Value
        {
            get { return _value; }
            set { _value = value.TrimEnd(); }
        }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }
    }
}
