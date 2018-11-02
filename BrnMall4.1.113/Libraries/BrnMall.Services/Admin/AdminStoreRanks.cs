using System;

using BrnMall.Core;

namespace BrnMall.Services
{
    /// <summary>
    /// 后台店铺等级操作管理类
    /// </summary>
    public partial class AdminStoreRanks : StoreRanks
    {
        /// <summary>
        /// 创建店铺等级
        /// </summary>
        public static void CreateStoreRank(StoreRankInfo storeRankInfo)
        {
            BrnMall.Data.StoreRanks.CreateStoreRank(storeRankInfo);
            BrnMall.Core.BMACache.Remove(CacheKeys.MALL_STORE_RANKLIST);
        }

        /// <summary>
        /// 删除店铺等级
        /// </summary>
        /// <param name="storeRid">店铺等级id</param>
        /// <returns>-1代表此等级下还有店铺未删除，0代表此店铺等级不存在，1代表删除成功</returns>
        public static int DeleteStoreRankById(int storeRid)
        {
            StoreRankInfo storeRankInfo = GetStoreRankById(storeRid);
            if (storeRankInfo != null)
            {
                if (AdminStores.GetStoreCountByStoreRid(storeRid) > 0)
                    return -1;

                BrnMall.Data.StoreRanks.DeleteStoreRankById(storeRid);
                BrnMall.Core.BMACache.Remove(CacheKeys.MALL_STORE_RANKLIST);
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// 更新店铺等级
        /// </summary>
        public static void UpdateStoreRank(StoreRankInfo storeRankInfo)
        {
            BrnMall.Data.StoreRanks.UpdateStoreRank(storeRankInfo);
            BrnMall.Core.BMACache.Remove(CacheKeys.MALL_STORE_RANKLIST);
        }
    }
}
