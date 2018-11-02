using System;
using System.Data;
using System.Collections.Generic;

using BrnMall.Core;
using BrnMall.Web.Framework;

namespace BrnMall.Web.MallAdmin.Models
{
    /// <summary>
    /// 商城管理日志列表模型类
    /// </summary>
    public class MallAdminLogListModel
    {
        /// <summary>
        /// 分页对象
        /// </summary>
        public PageModel PageModel { get; set; }
        /// <summary>
        /// 日志列表
        /// </summary>
        public List<MallAdminLogInfo> MallAdminLogList { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 操作动作
        /// </summary>
        public string Operation { get; set; }
        /// <summary>
        /// 操作开始时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 操作结束时间
        /// </summary>
        public string EndTime { get; set; }
    }

    /// <summary>
    /// 店铺管理日志列表模型类
    /// </summary>
    public class StoreAdminLogListModel
    {
        /// <summary>
        /// 分页对象
        /// </summary>
        public PageModel PageModel { get; set; }
        /// <summary>
        /// 日志列表
        /// </summary>
        public List<StoreAdminLogInfo> StoreAdminLogList { get; set; }
        /// <summary>
        /// 店铺id
        /// </summary>
        public int StoreId { get; set; }
        /// <summary>
        /// 店铺名称
        /// </summary>
        public string StoreName { get; set; }
        /// <summary>
        /// 操作动作
        /// </summary>
        public string Operation { get; set; }
        /// <summary>
        /// 操作开始时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 操作结束时间
        /// </summary>
        public string EndTime { get; set; }
    }

    /// <summary>
    /// 积分日志列表模型类
    /// </summary>
    public class CreditLogListModel
    {
        /// <summary>
        /// 分页对象
        /// </summary>
        public PageModel PageModel { get; set; }
        /// <summary>
        /// 积分日志列表
        /// </summary>
        public DataTable CreditLogList { get; set; }
        /// <summary>
        /// 账户名
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }
    }
}
