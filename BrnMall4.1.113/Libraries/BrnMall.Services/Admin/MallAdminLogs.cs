using System;
using System.Collections.Generic;

using BrnMall.Core;

namespace BrnMall.Services
{
    /// <summary>
    /// 商城管理日志操作管理类
    /// </summary>
    public partial class MallAdminLogs
    {
        /// <summary>
        /// 创建商城管理日志
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="nickName">用户昵称</param>
        /// <param name="mallAGid">商城管理员组id</param>
        /// <param name="mallAGTitle">商城管理员组标题</param>
        /// <param name="ip">ip</param>
        /// <param name="operation">操作行为</param>
        /// <param name="description">操作描述</param>
        public static void CreateMallAdminLog(int uid, string nickName, int mallAGid, string mallAGTitle, string ip, string operation, string description)
        {
            MallAdminLogInfo adminOperateLogInfo = new MallAdminLogInfo
            {
                Uid = uid,
                NickName = nickName,
                MallAGid = mallAGid,
                MallAGTitle = mallAGTitle,
                IP = ip,
                OperateTime = DateTime.Now,
                Operation = operation,
                Description = description
            };
            BrnMall.Data.MallAdminLogs.CreateMallAdminLog(adminOperateLogInfo);
        }

        /// <summary>
        /// 创建商城管理日志
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="nickName">用户昵称</param>
        /// <param name="mallAGid">商城管理员组id</param>
        /// <param name="mallAGTitle">商城管理员组标题</param>
        /// <param name="ip">ip</param>
        /// <param name="operation">操作行为</param>
        public static void CreateMallAdminLog(int uid, string nickName, int mallAGid, string mallAGTitle, string ip, string operation)
        {
            CreateMallAdminLog(uid, nickName, mallAGid, mallAGTitle, ip, operation, "");
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
            return BrnMall.Data.MallAdminLogs.GetMallAdminLogList(pageSize, pageNumber, condition);
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
            return BrnMall.Data.MallAdminLogs.GetMallAdminLogListCondition(uid, operation, startTime, endTime);
        }

        /// <summary>
        /// 获得商城管理日志数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static int GetMallAdminLogCount(string condition)
        {
            return BrnMall.Data.MallAdminLogs.GetMallAdminLogCount(condition);
        }

        /// <summary>
        /// 删除商城管理日志
        /// </summary>
        /// <param name="logIdList">日志id</param>
        public static void DeleteMallAdminLogById(int[] logIdList)
        {
            if (logIdList != null && logIdList.Length > 0)
                BrnMall.Data.MallAdminLogs.DeleteMallAdminLogById(CommonHelper.IntArrayToString(logIdList));
        }
    }
}
