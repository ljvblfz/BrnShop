using System;
using System.Collections.Generic;

using BrnMall.Core;

namespace BrnMall.Services
{
    /// <summary>
    /// 店铺管理日志操作管理类
    /// </summary>
    public partial class StoreAdminLogs
    {
        /// <summary>
        /// 创建店铺管理日志
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="nickName">用户昵称</param>
        /// <param name="storeId">店铺id</param>
        /// <param name="storeName">店铺名称</param>
        /// <param name="ip">ip</param>
        /// <param name="operation">操作行为</param>
        /// <param name="description">操作描述</param>
        public static void CreateStoreAdminLog(int uid, string nickName, int storeId, string storeName, string ip, string operation, string description)
        {
            StoreAdminLogInfo storeAdminLogInfo = new StoreAdminLogInfo
            {
                Uid = uid,
                NickName = nickName,
                StoreId = storeId,
                StoreName = storeName,
                IP = ip,
                OperateTime = DateTime.Now,
                Operation = operation,
                Description = description
            };
            BrnMall.Data.StoreAdminLogs.CreateStoreAdminLog(storeAdminLogInfo);
        }

        /// <summary>
        /// 创建店铺管理日志
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="nickName">用户昵称</param>
        /// <param name="storeId">店铺id</param>
        /// <param name="storeName">店铺名称</param>
        /// <param name="ip">ip</param>
        /// <param name="operation">操作行为</param>
        /// <param name="description">操作描述</param>
        public static void CreateStoreAdminLog(int uid, string nickName, int storeId, string storeName, string ip, string operation)
        {
            CreateStoreAdminLog(uid, nickName, storeId, storeName, ip, operation, "");
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
            return BrnMall.Data.StoreAdminLogs.GetStoreAdminLogList(pageSize, pageNumber, condition);
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
            return BrnMall.Data.StoreAdminLogs.GetStoreAdminLogListCondition(storeId, operation, startTime, endTime);
        }

        /// <summary>
        /// 获得店铺管理日志数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static int GetStoreAdminLogCount(string condition)
        {
            return BrnMall.Data.StoreAdminLogs.GetStoreAdminLogCount(condition);
        }

        /// <summary>
        /// 删除店铺管理日志
        /// </summary>
        /// <param name="logIdList">日志id</param>
        public static void DeleteStoreAdminLogById(int[] logIdList)
        {
            if (logIdList != null && logIdList.Length > 0)
                BrnMall.Data.StoreAdminLogs.DeleteStoreAdminLogById(CommonHelper.IntArrayToString(logIdList));
        }
    }
}
