using System;
using System.Data;

namespace BrnMall.Services
{
    public partial class AdminSearchHistories : SearchHistories
    {
        /// <summary>
        /// 获得搜索词统计列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="word">搜索词</param>
        /// <returns></returns>
        public static DataTable GetSearchWordStatList(int pageSize, int pageNumber, string word)
        {
            return BrnMall.Data.SearchHistories.GetSearchWordStatList(pageSize, pageNumber, word);
        }

        /// <summary>
        /// 获得搜索词统计数量
        /// </summary>
        /// <param name="word">搜索词</param>
        /// <returns></returns>
        public static int GetSearchWordStatCount(string word)
        {
            return BrnMall.Data.SearchHistories.GetSearchWordStatCount(word);
        }
    }
}
