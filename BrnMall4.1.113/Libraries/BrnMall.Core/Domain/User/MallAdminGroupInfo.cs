using System;

namespace BrnMall.Core
{
    /// <summary>
    /// 商城管理员组信息类
    /// </summary>
    public class MallAdminGroupInfo
    {
        private int _mallagid;//管理员组id
        private string _title;//管理员组标题
        private string _actionlist;//管理员组行为列表


        /// <summary>
        /// 管理员组id
        /// </summary>
        public int MallAGid
        {
            get { return _mallagid; }
            set { _mallagid = value; }
        }
        /// <summary>
        /// 管理员组标题
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value.TrimEnd(); }
        }
        /// <summary>
        /// 管理员组行为列表
        /// </summary>
        public string ActionList
        {
            get { return _actionlist; }
            set { _actionlist = value; }
        }
    }
}
