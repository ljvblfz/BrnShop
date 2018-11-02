using System;
using System.Data;
using System.Collections.Generic;

using BrnMall.Core;

namespace BrnMall.Data
{
    /// <summary>
    /// 店铺管理日志数据访问类
    /// </summary>
    public partial class StoreAdminLogs
    {
        /// <summary>
        /// 创建店铺管理日志
        /// </summary>
        public static void CreateStoreAdminLog(StoreAdminLogInfo storeAdminLogInfo)
        {
            BrnMall.Core.BMAData.RDBS.CreateStoreAdminLog(storeAdminLogInfo);
        }

        /// <summary>
        /// 获得店铺管理日志列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static List<StoreAdminLogInfo> GetStoreAdminLogList(int pageSize, int pageNumber, string condition)
        {
            List<StoreAdminLogInfo> storeAdminLogList = new List<StoreAdminLogInfo>();
            IDataReader reader = BrnMall.Core.BMAData.RDBS.GetStoreAdminLogList(pageSize, pageNumber, condition);
            while (reader.Read())
            {
                StoreAdminLogInfo storeAdminLogInfo = new StoreAdminLogInfo();
                storeAdminLogInfo.LogId = TypeHelper.ObjectToInt(reader["logid"]);
                storeAdminLogInfo.Uid = TypeHelper.ObjectToInt(reader["uid"]);
                storeAdminLogInfo.NickName = reader["nickname"].ToString();
                storeAdminLogInfo.StoreId = TypeHelper.ObjectToInt(reader["storeid"]);
                storeAdminLogInfo.StoreName = reader["storename"].ToString();
                storeAdminLogInfo.Operation = reader["operation"].ToString();
                storeAdminLogInfo.Description = reader["description"].ToString();
                storeAdminLogInfo.IP = reader["ip"].ToString();
                storeAdminLogInfo.OperateTime = TypeHelper.ObjectToDateTime(reader["operatetime"]);
                storeAdminLogList.Add(storeAdminLogInfo);
            }
            reader.Close();
            return storeAdminLogList;
        }

        /// <summary>
        /// 获得店铺管理日志列表条件
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="operation">操作行为</param>
        /// <param name="startTime">操作开始时间</param>
        /// <param name="endTime">操作结束时间</param>
        /// <returns></returns>
        public static string GetStoreAdminLogListCondition(int storeId, string operation, string startTime, string endTime)
        {
            return BrnMall.Core.BMAData.RDBS.GetStoreAdminLogListCondition(storeId, operation, startTime, endTime);
        }

        /// <summary>
        /// 获得店铺管理日志数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static int GetStoreAdminLogCount(string condition)
        {
            return BrnMall.Core.BMAData.RDBS.GetStoreAdminLogCount(condition);
        }

        /// <summary>
        /// 删除店铺管理日志
        /// </summary>
        /// <param name="logIdList">日志id</param>
        public static void DeleteStoreAdminLogById(string logIdList)
        {
            BrnMall.Core.BMAData.RDBS.DeleteStoreAdminLogById(logIdList);
        }
    }
}
