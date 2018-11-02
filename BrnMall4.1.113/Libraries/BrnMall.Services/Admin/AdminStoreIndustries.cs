using System;

using BrnMall.Core;

namespace BrnMall.Services
{
    /// <summary>
    /// 后台店铺行业操作管理类
    /// </summary>
    public partial class AdminStoreIndustries : StoreIndustries
    {
        /// <summary>
        /// 创建店铺行业
        /// </summary>
        public static void CreateStoreIndustry(StoreIndustryInfo storeIndustryInfo)
        {
            BrnMall.Data.StoreIndustries.CreateStoreIndustry(storeIndustryInfo);
            BrnMall.Core.BMACache.Remove(CacheKeys.MALL_STORE_INDUSTRYLIST);
        }

        /// <summary>
        /// 更新店铺行业
        /// </summary>
        public static void UpdateStoreIndustry(StoreIndustryInfo storeIndustryInfo)
        {
            BrnMall.Data.StoreIndustries.UpdateStoreIndustry(storeIndustryInfo);
            BrnMall.Core.BMACache.Remove(CacheKeys.MALL_STORE_INDUSTRYLIST);
        }

        /// <summary>
        /// 删除店铺行业
        /// </summary>
        /// <param name="storeIid">店铺行业id</param>
        /// <returns>-1代表此店铺行业下还有店铺未删除，0代表此店铺行业不存在，1代表删除成功</returns>
        public static int DeleteStoreIndustryById(int storeIid)
        {
            StoreIndustryInfo storeIndustryInfo = GetStoreIndustryById(storeIid);
            if (storeIndustryInfo != null)
            {
                if (AdminStores.GetStoreCountByStoreIid(storeIid) > 0)
                    return -1;

                BrnMall.Data.StoreIndustries.DeleteStoreIndustryById(storeIid);
                BrnMall.Core.BMACache.Remove(CacheKeys.MALL_STORE_INDUSTRYLIST);
                return 1;
            }
            return 0;
        }
    }
}
