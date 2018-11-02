using System;

namespace BrnMall.Core
{
    /// <summary>
    /// 店铺评价信息类
    /// </summary>
    public class StoreReviewInfo
    {
        private int _reviewid;//评价id
        private int _oid;//订单id
        private int _storeid;//店铺id
        private int _descriptionstar;//商品描述星星
        private int _servicestar;//商家服务星星
        private int _shipstar;//商家配送星星
        private int _uid;//用户id
        private DateTime _reviewtime;//评价时间
        private string _ip;//ip地址

        /// <summary>
        /// 评价id
        /// </summary>
        public int ReviewId
        {
            get { return _reviewid; }
            set { _reviewid = value; }
        }
        /// <summary>
        /// 订单id
        /// </summary>
        public int Oid
        {
            get { return _oid; }
            set { _oid = value; }
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
        /// 商品描述星星
        /// </summary>
        public int DescriptionStar
        {
            get { return _descriptionstar; }
            set { _descriptionstar = value; }
        }
        /// <summary>
        /// 商家服务星星
        /// </summary>
        public int ServiceStar
        {
            get { return _servicestar; }
            set { _servicestar = value; }
        }
        /// <summary>
        /// 商家配送星星
        /// </summary>
        public int ShipStar
        {
            get { return _shipstar; }
            set { _shipstar = value; }
        }
        /// <summary>
        /// 用户id
        /// </summary>
        public int Uid
        {
            get { return _uid; }
            set { _uid = value; }
        }
        /// <summary>
        /// 评价时间
        /// </summary>
        public DateTime ReviewTime
        {
            get { return _reviewtime; }
            set { _reviewtime = value; }
        }
        /// <summary>
        /// ip地址
        /// </summary>
        public string IP
        {
            get { return _ip; }
            set { _ip = value; }
        }
    }
}
