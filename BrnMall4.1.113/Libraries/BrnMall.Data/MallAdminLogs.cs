using System;
using System.Data;
using System.Collections.Generic;

using BrnMall.Core;

namespace BrnMall.Data
{
    /// <summary>
    /// 商城管理日志数据访问类
    /// </summary>
    public partial class MallAdminLogs
    {
        /// <summary>
        /// 创建商城管理日志
        /// </summary>
        public static void CreateMallAdminLog(MallAdminLogInfo mallAdminLogInfo)
        {
            BrnMall.Core.BMAData.RDBS.CreateMallAdminLog(mallAdminLogInfo);
        }

        /// <summary>
        /// 获得商城管理日志列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static List<MallAdminLogInfo> GetMallAdminLogList(int pageSize, int pageNumber, string condition)
        {
            List<MallAdminLogInfo> mallAdminLogList = new List<MallAdminLogInfo>();
            IDataReader reader = BrnMall.Core.BMAData.RDBS.GetMallAdminLogList(pageSize, pageNumber, condition);
            while (reader.Read())
            {
                MallAdminLogInfo mallAdminLogInfo = new MallAdminLogInfo();
                mallAdminLogInfo.LogId = TypeHelper.ObjectToInt(reader["logid"]);
                mallAdminLogInfo.Uid = TypeHelper.ObjectToInt(reader["uid"]);
                mallAdminLogInfo.NickName = reader["nickname"].ToString();
                mallAdminLogInfo.MallAGid = TypeHelper.ObjectToInt(reader["mallagid"]);
                mallAdminLogInfo.MallAGTitle = reader["mallagtitle"].ToString();
                mallAdminLogInfo.Operation = reader["operation"].ToString();
                mallAdminLogInfo.Description = reader["description"].ToString();
                mallAdminLogInfo.IP = reader["ip"].ToString();
                mallAdminLogInfo.OperateTime = TypeHelper.ObjectToDateTime(reader["operatetime"]);
                mallAdminLogList.Add(mallAdminLogInfo);
            }
            reader.Close();
            return mallAdminLogList;
        }

        /// <summary>
        /// 获得商城管理日志列表条件
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="operation">操作行为</param>
        /// <param name="startTime">操作开始时间</param>
        /// <param name="endTime">操作结束时间</param>
        /// <returns></returns>
        public static string GetMallAdminLogListCondition(int uid, string operation, string startTime, string endTime)
        {
            return BrnMall.Core.BMAData.RDBS.GetMallAdminLogListCondition(uid, operation, startTime, endTime);
        }

        /// <summary>
        /// 获得商城管理日志数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static int GetMallAdminLogCount(string condition)
        {
            return BrnMall.Core.BMAData.RDBS.GetMallAdminLogCount(condition);
        }

        /// <summary>
        /// 删除商城管理日志
        /// </summary>
        /// <param name="logIdList">日志id</param>
        public static void DeleteMallAdminLogById(string logIdList)
        {
            BrnMall.Core.BMAData.RDBS.DeleteMallAdminLogById(logIdList);
        }

    }
}
