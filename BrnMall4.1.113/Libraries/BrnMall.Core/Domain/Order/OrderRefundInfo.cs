using System;

namespace BrnMall.Core
{
    /// <summary>
    /// 订单退款信息类
    /// </summary>
    public class OrderRefundInfo
    {
        private int _refundid;//退款id
        private int _state = 0;//状态
        private int _storeid;//店铺id
        private string _storename;//店铺名称
        private int _oid;//订单id
        private string _osn;//订单编号
        private int _uid;//用户id
        private int _asid;//售后服务id
        private string _paysystemname;//支付方式系统名
        private string _payfriendname;//支付方式昵称
        private string _paysn;//支付单号
        private decimal _paymoney;//支付金额
        private decimal _refundmoney;//退款金额
        private DateTime _applytime;//申请时间
        private string _refundsn = "";//退款单号
        private DateTime _refundtime = new DateTime(1900, 1, 1);//退款时间

        /// <summary>
        /// 退款id
        /// </summary>
        public int RefundId
        {
            get { return _refundid; }
            set { _refundid = value; }
        }
        /// <summary>
        /// 状态
        /// </summary>
        public int State
        {
            get { return _state; }
            set { _state = value; }
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
        /// 店铺名称
        /// </summary>
        public string StoreName
        {
            get { return _storename; }
            set { _storename = value; }
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
        /// 订单编号
        /// </summary>
        public string OSN
        {
            get { return _osn; }
            set { _osn = value; }
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
        /// 售后服务id
        /// </summary>
        public int ASId
        {
            get { return _asid; }
            set { _asid = value; }
        }
        /// <summary>
        /// 支付方式系统名
        /// </summary>
        public string PaySystemName
        {
            get { return _paysystemname; }
            set { _paysystemname = value; }
        }
        /// <summary>
        /// 支付方式昵称
        /// </summary>
        public string PayFriendName
        {
            get { return _payfriendname; }
            set { _payfriendname = value; }
        }
        /// <summary>
        /// 支付单号
        /// </summary>
        public string PaySN
        {
            get { return _paysn; }
            set { _paysn = value; }
        }
        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal PayMoney
        {
            get { return _paymoney; }
            set { _paymoney = value; }
        }
        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal RefundMoney
        {
            get { return _refundmoney; }
            set { _refundmoney = value; }
        }
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime ApplyTime
        {
            get { return _applytime; }
            set { _applytime = value; }
        }
        /// <summary>
        /// 退款单号
        /// </summary>
        public string RefundSN
        {
            get { return _refundsn; }
            set { _refundsn = value; }
        }
        /// <summary>
        /// 退款时间
        /// </summary>
        public DateTime RefundTime
        {
            get { return _refundtime; }
            set { _refundtime = value; }
        }
    }
}
