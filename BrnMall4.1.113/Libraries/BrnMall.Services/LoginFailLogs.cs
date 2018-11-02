using System;

using BrnMall.Core;

namespace BrnMall.Services
{
    /// <summary>
    /// 登录失败日志操作管理类
    /// </summary>
    public partial class LoginFailLogs
    {
        /// <summary>
        /// 获得登录失败次数
        /// </summary>
        /// <param name="loginIP">登录IP</param>
        /// <returns></returns>
        public static int GetLoginFailTimesByIp(string loginIP)
        {
            LoginFailLogInfo loginFailLogInfo = BrnMall.Data.LoginFailLogs.GetLoginFailLogByIP(CommonHelper.ConvertIPToLong(loginIP));
            if (loginFailLogInfo == null)
                return 0;
            if (loginFailLogInfo.LastLoginTime.AddMinutes(15) < DateTime.Now)
                return 0;

            return loginFailLogInfo.FailTimes;
        }

        /// <summary>
        /// 增加登录失败次数
        /// </summary>
        /// <param name="loginIP">登录IP</param>
        /// <param name="loginTime">登录时间</param>
        public static void AddLoginFailTimes(string loginIP, DateTime loginTime)
        {
            BrnMall.Data.LoginFailLogs.AddLoginFailTimes(CommonHelper.ConvertIPToLong(loginIP), loginTime);
        }

        /// <summary>
        /// 删除登录失败日志
        /// </summary>
        /// <param name="loginIP">登录IP</param>
        public static void DeleteLoginFailLogByIP(string loginIP)
        {
            BrnMall.Data.LoginFailLogs.DeleteLoginFailLogByIP(CommonHelper.ConvertIPToLong(loginIP));
        }
    }
}
