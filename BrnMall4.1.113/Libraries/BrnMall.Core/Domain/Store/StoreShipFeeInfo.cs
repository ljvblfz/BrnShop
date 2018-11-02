using System;

namespace BrnMall.Core
{
    /// <summary>
    /// 店铺配送费用信息类
    /// </summary>
    public class StoreShipFeeInfo
    {
        private int _recordid;//记录id
        private int _storestid;//店铺配送模板id
        private int _regionid;//区域id
        private int _startvalue;//开始值
        private int _startfee;//开始费用
        private int _addvalue;//添加值
        private int _addfee;//添加费用

        /// <summary>
        /// 记录id
        /// </summary>
        public int RecordId
        {
            get { return _recordid; }
            set { _recordid = value; }
        }
        /// <summary>
        /// 店铺配送模板id
        /// </summary>
        public int StoreSTid
        {
            get { return _storestid; }
            set { _storestid = value; }
        }
        /// <summary>
        /// 区域id
        /// </summary>
        public int RegionId
        {
            get { return _regionid; }
            set { _regionid = value; }
        }
        /// <summary>
        /// 开始值
        /// </summary>
        public int StartValue
        {
            get { return _startvalue; }
            set { _startvalue = value; }
        }
        /// <summary>
        /// 开始费用
        /// </summary>
        public int StartFee
        {
            get { return _startfee; }
            set { _startfee = value; }
        }
        /// <summary>
        /// 添加值
        /// </summary>
        public int AddValue
        {
            get { return _addvalue; }
            set { _addvalue = value; }
        }
        /// <summary>
        /// 添加费用
        /// </summary>
        public int AddFee
        {
            get { return _addfee; }
            set { _addfee = value; }
        }
    }
}
