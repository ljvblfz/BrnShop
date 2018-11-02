using System;

namespace BrnMall.Core
{
    /// <summary>
    /// 店铺行业信息类
    /// </summary>
    public class StoreIndustryInfo
    {
        private int _storeiid;//店铺行业id
        private string _title;//标题
        private int _displayorder;//排序

        /// <summary>
        /// 店铺行业id
        /// </summary>
        public int StoreIid
        {
            get { return _storeiid; }
            set { _storeiid = value; }
        }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value.TrimEnd(); }
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
