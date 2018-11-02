using System;

namespace BrnShop.Core
{
    /// <summary>
    /// 订单售后服务信息类
    /// </summary>
    public class OrderAfterServiceInfo
    {
        private int _asid;//售后服务id
        private int _state;//状态
        private int _uid;//用户id
        private int _oid;//订单id
        private int _recordid;//记录id
        private int _pid;//商品id
        private int _cateid;//分类id
        private int _brandid;//品牌id
        private string _pname;//商品名称
        private string _pshowimg;//商品图片
        private int _count;//数量
        private decimal _money;//金钱
        private int _type;//类型(0代表退货,1代表换货,2代表维修)
        private string _applyreason;//申请原因
        private DateTime _applytime;//申请时间
        private string _checkresult;//审核结果
        private DateTime _checktime;//审核时间
        private int _shipcoid1;//配送公司id
        private string _shipconame1;//配送公司名称
        private string _shipsn1;//配送单号
        private int _regionid;//收货区域id
        private string _consignee;//收货人
        private string _mobile;//手机
        private string _phone;//固话
        private string _email;//邮箱
        private string _zipcode;//邮件编码
        private string _address;//收货详细地址
        private DateTime _sendtime;//邮寄给商城时间
        private DateTime _receivetime;//商城收货时间
        private int _shipcoid2;//配送公司id
        private string _shipconame2;//配送公司名称
        private string _shipsn2;//配送单号
        private DateTime _backtime;//邮寄给客户时间

        /// <summary>
        /// 售后服务id
        /// </summary>
        public int ASId
        {
            get { return _asid; }
            set { _asid = value; }
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
        /// 用户id
        /// </summary>
        public int Uid
        {
            get { return _uid; }
            set { _uid = value; }
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
        /// 记录id
        /// </summary>
        public int RecordId
        {
            get { return _recordid; }
            set { _recordid = value; }
        }
        /// <summary>
        /// 商品id
        /// </summary>
        public int Pid
        {
            get { return _pid; }
            set { _pid = value; }
        }
        /// <summary>
        /// 分类id
        /// </summary>
        public int CateId
        {
            get { return _cateid; }
            set { _cateid = value; }
        }
        /// <summary>
        /// 品牌id
        /// </summary>
        public int BrandId
        {
            get { return _brandid; }
            set { _brandid = value; }
        }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string PName
        {
            get { return _pname; }
            set { _pname = value; }
        }
        /// <summary>
        /// 商品图片
        /// </summary>
        public string PShowImg
        {
            get { return _pshowimg; }
            set { _pshowimg = value; }
        }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }
        /// <summary>
        /// 金钱
        /// </summary>
        public decimal Money
        {
            get { return _money; }
            set { _money = value; }
        }
        /// <summary>
        /// 类型(0代表退货,1代表换货,2代表维修)
        /// </summary>
        public int Type
        {
            get { return _type; }
            set { _type = value; }
        }
        /// <summary>
        /// 申请原因
        /// </summary>
        public string ApplyReason
        {
            get { return _applyreason; }
            set { _applyreason = value; }
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
        /// 审核结果
        /// </summary>
        public string CheckResult
        {
            get { return _checkresult; }
            set { _checkresult = value; }
        }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime CheckTime
        {
            get { return _checktime; }
            set { _checktime = value; }
        }
        /// <summary>
        /// 配送公司id
        /// </summary>
        public int ShipCoId1
        {
            get { return _shipcoid1; }
            set { _shipcoid1 = value; }
        }
        /// <summary>
        /// 配送公司名称
        /// </summary>
        public string ShipCoName1
        {
            get { return _shipconame1; }
            set { _shipconame1 = value; }
        }
        /// <summary>
        /// 配送单号
        /// </summary>
        public string ShipSN1
        {
            get { return _shipsn1; }
            set { _shipsn1 = value; }
        }
        /// <summary>
        /// 收货区域id
        /// </summary>
        public int RegionId
        {
            get { return _regionid; }
            set { _regionid = value; }
        }
        /// <summary>
        /// 收货人
        /// </summary>
        public string Consignee
        {
            get { return _consignee; }
            set { _consignee = value; }
        }
        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile
        {
            get { return _mobile; }
            set { _mobile = value; }
        }
        /// <summary>
        /// 固话
        /// </summary>
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        /// <summary>
        /// 邮件编码
        /// </summary>
        public string ZipCode
        {
            get { return _zipcode; }
            set { _zipcode = value; }
        }
        /// <summary>
        /// 收货详细地址
        /// </summary>
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }
        /// <summary>
        /// 邮寄给商城时间
        /// </summary>
        public DateTime SendTime
        {
            get { return _sendtime; }
            set { _sendtime = value; }
        }
        /// <summary>
        /// 商城收货时间
        /// </summary>
        public DateTime ReceiveTime
        {
            get { return _receivetime; }
            set { _receivetime = value; }
        }
        /// <summary>
        /// 配送公司id
        /// </summary>
        public int ShipCoId2
        {
            get { return _shipcoid2; }
            set { _shipcoid2 = value; }
        }
        /// <summary>
        /// 配送公司名称
        /// </summary>
        public string ShipCoName2
        {
            get { return _shipconame2; }
            set { _shipconame2 = value; }
        }
        /// <summary>
        /// 配送单号
        /// </summary>
        public string ShipSN2
        {
            get { return _shipsn2; }
            set { _shipsn2 = value; }
        }
        /// <summary>
        /// 邮寄给客户时间
        /// </summary>
        public DateTime BackTime
        {
            get { return _backtime; }
            set { _backtime = value; }
        }
    }
}
