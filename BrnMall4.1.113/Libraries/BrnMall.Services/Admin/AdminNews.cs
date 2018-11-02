using System;
using System.Data;

using BrnMall.Core;

namespace BrnMall.Services
{
    /// <summary>
    /// 后台新闻操作管理类
    /// </summary>
    public partial class AdminNews : News
    {
        /// <summary>
        /// 创建新闻类型
        /// </summary>
        public static void CreateNewsType(NewsTypeInfo newsTypeInfo)
        {
            BrnMall.Data.News.CreateNewsType(newsTypeInfo);
            BrnMall.Core.BMACache.Remove(CacheKeys.MALL_NEWSTYPE_LIST);
            BrnMall.Core.BMACache.Remove(CacheKeys.MALL_NEWS_HOMELIST + "\\d+");
        }

        /// <summary>
        /// 删除新闻类型
        /// </summary>
        /// <param name="newsTypeId">新闻类型id</param>
        public static void DeleteNewsTypeById(int newsTypeId)
        {
            BrnMall.Data.News.DeleteNewsTypeById(newsTypeId);
            BrnMall.Core.BMACache.Remove(CacheKeys.MALL_NEWSTYPE_LIST);
            BrnMall.Core.BMACache.Remove(CacheKeys.MALL_NEWS_HOMELIST + "\\d+");
        }

        /// <summary>
        /// 更新新闻类型
        /// </summary>
        public static void UpdateNewsType(NewsTypeInfo newsTypeInfo)
        {
            BrnMall.Data.News.UpdateNewsType(newsTypeInfo);
            BrnMall.Core.BMACache.Remove(CacheKeys.MALL_NEWSTYPE_LIST);
            BrnMall.Core.BMACache.Remove(CacheKeys.MALL_NEWS_HOMELIST + "\\d+");
        }



        /// <summary>
        /// 创建新闻
        /// </summary>
        public static void CreateNews(NewsInfo newsInfo)
        {
            BrnMall.Data.News.CreateNews(newsInfo);
            BrnMall.Core.BMACache.Remove(CacheKeys.MALL_NEWS_HOMELIST + newsInfo.NewsTypeId);
            BrnMall.Core.BMACache.Remove(CacheKeys.MALL_NEWS_HOMELIST + "\\d+");
        }

        /// <summary>
        /// 删除新闻
        /// </summary>
        /// <param name="newsIdList">新闻id列表</param>
        public static void DeleteNewsById(int[] newsIdList)
        {
            if (newsIdList != null && newsIdList.Length > 0)
            {
                BrnMall.Data.News.DeleteNewsById(CommonHelper.IntArrayToString(newsIdList));
                BrnMall.Core.BMACache.Remove(CacheKeys.MALL_NEWS_HOMELIST + "\\d+");
            }
        }

        /// <summary>
        /// 更新新闻
        /// </summary>
        public static void UpdateNews(NewsInfo newsInfo)
        {
            BrnMall.Data.News.UpdateNews(newsInfo);
            BrnMall.Core.BMACache.Remove(CacheKeys.MALL_NEWS_HOMELIST + newsInfo.NewsTypeId);
            BrnMall.Core.BMACache.Remove(CacheKeys.MALL_NEWS_HOMELIST + "\\d+");
        }

        /// <summary>
        /// 后台获得新闻
        /// </summary>
        /// <param name="newsId">新闻id</param>
        /// <returns></returns>
        public static NewsInfo AdminGetNewsById(int newsId)
        {
            if (newsId > 0)
                return BrnMall.Data.News.AdminGetNewsById(newsId);
            return null;
        }

        /// <summary>
        /// 后台获得新闻列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static DataTable AdminGetNewsList(int pageSize, int pageNumber, string condition)
        {
            return BrnMall.Data.News.AdminGetNewsList(pageSize, pageNumber, condition);
        }

        /// <summary>
        /// 后台获得新闻列表搜索条件
        /// </summary>
        /// <param name="newsTypeId">新闻类型id</param>
        /// <param name="title">新闻标题</param>
        /// <returns></returns>
        public static string AdminGetNewsListCondition(int newsTypeId, string title)
        {
            return BrnMall.Data.News.AdminGetNewsListCondition(newsTypeId, title);
        }

        /// <summary>
        /// 后台获得新闻数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static int AdminGetNewsCount(string condition)
        {
            return BrnMall.Data.News.AdminGetNewsCount(condition);
        }

        /// <summary>
        /// 后台根据新闻标题得到新闻id
        /// </summary>
        /// <param name="title">新闻标题</param>
        /// <returns></returns>
        public static int AdminGetNewsIdByTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return 0;
            return BrnMall.Data.News.AdminGetNewsIdByTitle(title);
        }
    }
}
