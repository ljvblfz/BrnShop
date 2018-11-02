using System;

namespace BrnMall.Core
{
    /// <summary>
    /// 店铺等级信息类
    /// </summary>
    public class StoreRankInfo
    {
        private int _storerid;//店铺等级id
        private string _title;//标题
        private string _avatar;//头像
        private int _honestieslower;//诚信下限
        private int _honestiesupper;//诚信上限
        private int _productcount;//商品数量

        /// <summary>
        /// 店铺等级id
        /// </summary>
        public int StoreRid
        {
            get { return _storerid; }
            set { _storerid = value; }
        }
        ///<summary>
        ///标题
        ///</summary>
        public string Title
        {
            get { return _title; }
            set { _title = value.TrimEnd(); }
        }
        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar
        {
            get { return _avatar; }
            set { _avatar = value.TrimEnd(); }
        }
        ///<summary>
        ///诚信下限
        ///</summary>
        public int HonestiesLower
        {
            get { return _honestieslower; }
            set { _honestieslower = value; }
        }
        ///<summary>
        ///诚信上限
        ///</summary>
        public int HonestiesUpper
        {
            get { return _honestiesupper; }
            set { _honestiesupper = value; }
        }
        /// <summary>
        /// 商品数量
        /// </summary>
        public int ProductCount
        {
            get { return _productcount; }
            set { _productcount = value; }
        }
    }
}
