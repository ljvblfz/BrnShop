using System;
using System.Collections.Generic;

using BrnMall.Core;

namespace BrnMall.Services
{
    /// <summary>
    /// 店铺等级操作管理类
    /// </summary>
    public partial class StoreRanks
    {
        /// <summary>
        /// 获得店铺等级列表
        /// </summary>
        /// <returns></returns>
        public static List<StoreRankInfo> GetStoreRankList()
        {
            List<StoreRankInfo> storeRankList = BrnMall.Core.BMACache.Get(CacheKeys.MALL_STORE_RANKLIST) as List<StoreRankInfo>;
            if (storeRankList == null)
            {
                storeRankList = BrnMall.Data.StoreRanks.GetStoreRankList();
                BrnMall.Core.BMACache.Insert(CacheKeys.MALL_STORE_RANKLIST, storeRankList);
            }
            return storeRankList;
        }

        /// <summary>
        /// 获得店铺等级
        /// </summary>
        /// <param name="storeRid">店铺等级id</param>
        /// <returns></returns>
        public static StoreRankInfo GetStoreRankById(int storeRid)
        {
            foreach (StoreRankInfo storeRankInfo in GetStoreRankList())
            {
                if (storeRid == storeRankInfo.StoreRid)
                    return storeRankInfo;
            }
            return null;
        }

        /// <summary>
        /// 获得店铺等级id
        /// </summary>
        /// <param name="title">店铺等级标题</param>
        /// <returns></returns>
        public static int GetStoreRidByTitle(string title)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                foreach (StoreRankInfo storeRankInfo in GetStoreRankList())
                {
                    if (storeRankInfo.Title == title)
                        return storeRankInfo.StoreRid;
                }
            }
            return -1;
        }

        /// <summary>
        /// 获得诚信对应的店铺等级
        /// </summary>
        /// <param name="honesties">诚信</param>
        /// <returns></returns>
        public static StoreRankInfo GetStoreRankByhonesties(int honesties)
        {
            foreach (StoreRankInfo storeRankInfo in GetStoreRankList())
            {
                if (storeRankInfo.HonestiesLower <= honesties && (storeRankInfo.HonestiesUpper > honesties || storeRankInfo.HonestiesUpper == -1))
                    return storeRankInfo;
            }
            return null;
        }

        /// <summary>
        /// 获得最低店铺等级
        /// </summary>
        /// <returns></returns>
        public static StoreRankInfo GetLowestStoreRank()
        {
            foreach (StoreRankInfo storeRankInfo in GetStoreRankList())
            {
                if (storeRankInfo.HonestiesLower == 0)
                    return storeRankInfo;
            }
            return null;
        }
    }
}
