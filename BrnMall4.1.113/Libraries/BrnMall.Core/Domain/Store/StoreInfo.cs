using System;

namespace BrnMall.Core
{
    /// <summary>
    /// 店铺信息类
    /// </summary>
    public class StoreInfo
    {
        private int _storeid;//店铺id
        private int _state;//状态(0代表营业,1代表关闭)
        private string _name;//名称
        private int _regionid;//区域id
        private int _storerid;//等级id
        private int _storeiid;//行业id
        private string _logo;//logo
        private DateTime _createtime;//创建时间
        private string _mobile;//手机
        private string _phone;//固定电话
        private string _qq;//qq
        private string _ww;//阿里旺旺
        private decimal _depoint;//商品描述评分
        private decimal _sepoint;//商家服务评分
        private decimal _shpoint;//商家配送评分
        private int _honesties;//店铺诚信值
        private DateTime _stateendtime;//状态结束时间
        private string _theme;//主题
        private string _banner;//Banner
        private string _announcement;//公告
        private string _description;//描述

        /// <summary>
        /// 店铺id
        /// </summary>
        public int StoreId
        {
            get { return _storeid; }
            set { _storeid = value; }
        }
        /// <summary>
        /// 状态(0代表营业,1代表关闭)
        /// </summary>
        public int State
        {
            get { return _state; }
            set { _state = value; }
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
        /// 区域id
        /// </summary>
        public int RegionId
        {
            get { return _regionid; }
            set { _regionid = value; }
        }
        /// <summary>
        /// 等级id
        /// </summary>
        public int StoreRid
        {
            get { return _storerid; }
            set { _storerid = value; }
        }
        /// <summary>
        /// 行业id
        /// </summary>
        public int StoreIid
        {
            get { return _storeiid; }
            set { _storeiid = value; }
        }
        /// <summary>
        /// logo
        /// </summary>
        public string Logo
        {
            get { return _logo; }
            set { _logo = value.TrimEnd(); }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }
        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile
        {
            get { return _mobile; }
            set { _mobile = value.TrimEnd(); }
        }
        /// <summary>
        /// 固定电话
        /// </summary>
        public string Phone
        {
            get { return _phone; }
            set { _phone = value.TrimEnd(); }
        }
        /// <summary>
        /// qq
        /// </summary>
        public string QQ
        {
            get { return _qq; }
            set { _qq = value.TrimEnd(); }
        }
        /// <summary>
        /// 阿里旺旺
        /// </summary>
        public string WW
        {
            get { return _ww; }
            set { _ww = value.TrimEnd(); }
        }
        /// <summary>
        /// 商品描述评分
        /// </summary>
        public decimal DePoint
        {
            get { return _depoint; }
            set { _depoint = value; }
        }
        /// <summary>
        /// 商家服务评分
        /// </summary>
        public decimal SePoint
        {
            get { return _sepoint; }
            set { _sepoint = value; }
        }
        /// <summary>
        /// 商家配送评分
        /// </summary>
        public decimal ShPoint
        {
            get { return _shpoint; }
            set { _shpoint = value; }
        }
        /// <summary>
        /// 店铺诚信值
        /// </summary>
        public int Honesties
        {
            get { return _honesties; }
            set { _honesties = value; }
        }
        /// <summary>
        /// 状态结束时间
        /// </summary>
        public DateTime StateEndTime
        {
            get { return _stateendtime; }
            set { _stateendtime = value; }
        }
        /// <summary>
        /// 主题
        /// </summary>
        public string Theme
        {
            get { return _theme; }
            set { _theme = value.TrimEnd(); }
        }
        /// <summary>
        /// Banner
        /// </summary>
        public string Banner
        {
            get { return _banner; }
            set { _banner = value.TrimEnd(); }
        }
        /// <summary>
        /// 公告
        /// </summary>
        public string Announcement
        {
            get { return _announcement; }
            set { _announcement = value.TrimEnd(); }
        }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value.TrimEnd(); }
        }
    }
}
