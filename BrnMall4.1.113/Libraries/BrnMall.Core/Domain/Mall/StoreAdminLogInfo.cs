using System;

namespace BrnMall.Core
{
    /// <summary>
    /// 店铺管理日志信息类
    /// </summary>
    public class StoreAdminLogInfo
    {
        private int _logid;//日志id
        private int _uid;//用户id
        private string _nickname;//用户昵称
        private int _storeid;//店铺id
        private string _storename;//店铺名称
        private string _operation;//操作动作
        private string _description;//操作描述
        private string _ip;//ip
        private DateTime operatetime;//操作时间

        /// <summary>
        /// 日志id
        /// </summary>
        public int LogId
        {
            get { return _logid; }
            set { _logid = value; }
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
        /// 用户昵称
        /// </summary>
        public string NickName
        {
            get { return _nickname; }
            set { _nickname = value; }
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
        /// 操作动作
        /// </summary>
        public string Operation
        {
            get { return _operation; }
            set { _operation = value; }
        }
        /// <summary>
        /// 操作描述
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        /// <summary>
        /// ip
        /// </summary>
        public string IP
        {
            get { return _ip; }
            set { _ip = value; }
        }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateTime
        {
            get { return operatetime; }
            set { operatetime = value; }
        }
    }
}
