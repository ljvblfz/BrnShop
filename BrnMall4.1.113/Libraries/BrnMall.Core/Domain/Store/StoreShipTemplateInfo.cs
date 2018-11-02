using System;

namespace BrnMall.Core
{
    /// <summary>
    /// 店铺配送模板信息类
    /// </summary>
    public class StoreShipTemplateInfo
    {
        private int _storestid;//店铺配送模板id
        private int _storeid;//店铺id
        private int _free;//是否免运费
        private int _type;//计费类型(0代表按件数计算,1代表按重量计算)
        private string _title;//模板标题

        /// <summary>
        /// 店铺配送模板id
        /// </summary>
        public int StoreSTid
        {
            get { return _storestid; }
            set { _storestid = value; }
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
        /// 是否免运费
        /// </summary>
        public int Free
        {
            get { return _free; }
            set { _free = value; }
        }
        /// <summary>
        /// 计费类型(0代表按件数计算,1代表按重量计算)
        /// </summary>
        public int Type
        {
            get { return _type; }
            set { _type = value; }
        }
        /// <summary>
        /// 模板标题
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value.TrimEnd(); }
        }
    }
}
