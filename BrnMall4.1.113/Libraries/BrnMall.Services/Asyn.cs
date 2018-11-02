using System;

using BrnMall.Core;

namespace BrnMall.Services
{
    /// <summary>
    /// 异步操作管理类
    /// </summary>
    public partial class Asyn
    {
        /// <summary>
        /// 更新在线用户
        /// </summary>
        public static void UpdateOnlineUser(int uid, string sid, string nickName, string ip, int regionId)
        {
            BMAAsyn.Instance.UpdateOnlineUser(new UpdateOnlineUserState(uid, sid, nickName, ip, regionId, DateTime.Now));
        }

        /// <summary>
        /// 更新浏览历史
        /// </summary>
        public static void UpdateBrowseHistory(int uid, int pid)
        {
            BMAAsyn.Instance.UpdateBrowseHistory(new UpdateBrowseHistoryState(uid, pid, DateTime.Now));
        }

        /// <summary>
        /// 更新搜索历史
        /// </summary>
        public static void UpdateSearchHistory(int uid, string word)
        {
            BMAAsyn.Instance.UpdateSearchHistory(new UpdateSearchHistoryState(uid, word, DateTime.Now));
        }

        /// <summary>
        /// 更新商品统计
        /// </summary>
        public static void UpdateProductStat(int pid, int regionId)
        {
            BMAAsyn.Instance.UpdateProductStat(new UpdateProductStatState(pid, regionId, DateTime.Now));
        }

        /// <summary>
        /// 更新PV统计
        /// </summary>
        public static void UpdatePVStat(int storeId, int uid, int regionId, string browser, string os)
        {
            //处理下浏览器类型
            if (!browser.StartsWith("ie"))
            {
                if (browser.StartsWith("chrome"))
                {
                    browser = "chrome";
                }
                else if (browser.StartsWith("safari"))
                {
                    browser = "safari";
                }
                else if (browser.StartsWith("firefox"))
                {
                    browser = "firefox";
                }
                else
                {
                    browser = "unknown";
                }
            }

            BMAAsyn.Instance.UpdatePVStat(new UpdatePVStatState(storeId, uid > 0, regionId, browser, os, DateTime.Now));
        }
    }
}
