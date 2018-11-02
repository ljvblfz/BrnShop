using System;
using System.Data;

namespace BrnShop.Data
{
    /// <summary>
    /// 搜索历史数据访问类
    /// </summary>
    public partial class SearchHistories
    {
        /// <summary>
        /// 更新搜索历史
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="word">搜索词</param>
        /// <param name="updateTime">更新时间</param>
        public static void UpdateSearchHistory(int uid, string word, DateTime updateTime)
        {
            BrnShop.Core.BSPData.RDBS.UpdateSearchHistory(uid, word, updateTime);
        }

        /// <summary>
        /// 清空过期搜索历史
        /// </summary>
        public static void ClearExpiredSearchHistory()
        {
            BrnShop.Core.BSPData.RDBS.ClearExpiredSearchHistory();
        }

        /// <summary>
        /// 获得搜索词统计列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="word">搜索词</param>
        /// <returns></returns>
        public static DataTable GetSearchWordStatList(int pageSize, int pageNumber, string word)
        {
            return BrnShop.Core.BSPData.RDBS.GetSearchWordStatList(pageSize, pageNumber, word);
        }

        /// <summary>
        /// 获得搜索词统计数量
        /// </summary>
        /// <param name="word">搜索词</param>
        /// <returns></returns>
        public static int GetSearchWordStatCount(string word)
        {
            return BrnShop.Core.BSPData.RDBS.GetSearchWordStatCount(word);
        }
    }
}
