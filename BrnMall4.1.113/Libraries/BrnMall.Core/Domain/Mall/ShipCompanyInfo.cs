using System;

namespace BrnMall.Core
{
    /// <summary>
    /// 配送公司信息类
    /// </summary>
    public class ShipCompanyInfo
    {
        private int _shipcoid;//配送公司id
        private string _name;//名称
        private int _displayorder;//排序

        /// <summary>
        /// 配送公司id
        /// </summary>
        public int ShipCoId
        {
            get { return _shipcoid; }
            set { _shipcoid = value; }
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value.TrimEnd(); }
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
