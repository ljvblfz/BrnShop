using System;

namespace BrnMall.Core
{
    /// <summary>
    /// 店长信息类
    /// </summary>
    public class StoreKeeperInfo
    {
        private int _storeid;//店铺id
        private int _type;//店长类型(0代表个人,1代表公司)
        private string _name;//名称
        private string _idcard;//标识号
        private string _address;//地址

        /// <summary>
        /// 店铺id
        /// </summary>
        public int StoreId
        {
            get { return _storeid; }
            set { _storeid = value; }
        }
        /// <summary>
        /// 店长类型(0代表个人,1代表公司)
        /// </summary>
        public int Type
        {
            get { return _type; }
            set { _type = value; }
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// 标识号
        /// </summary>
        public string IdCard
        {
            get { return _idcard; }
            set { _idcard = value; }
        }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }
    }
}
