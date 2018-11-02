using System;
using System.Data;
using System.Collections.Generic;

using BrnMall.Core;

namespace BrnMall.Data
{
    /// <summary>
    /// 店铺等级数据访问类
    /// </summary>
    public partial class StoreRanks
    {
        /// <summary>
        /// 获得店铺等级列表
        /// </summary>
        /// <returns></returns>
        public static List<StoreRankInfo> GetStoreRankList()
        {
            List<StoreRankInfo> storeRankList = new List<StoreRankInfo>();
            IDataReader reader = BrnMall.Core.BMAData.RDBS.GetStoreRankList();
            while (reader.Read())
            {
                StoreRankInfo storeRankInfo = new StoreRankInfo();
                storeRankInfo.StoreRid = TypeHelper.ObjectToInt(reader["storerid"]);
                storeRankInfo.Title = reader["title"].ToString();
                storeRankInfo.Avatar = reader["avatar"].ToString();
                storeRankInfo.HonestiesLower = TypeHelper.ObjectToInt(reader["honestieslower"]);
                storeRankInfo.HonestiesUpper = TypeHelper.ObjectToInt(reader["honestiesupper"]);
                storeRankInfo.ProductCount = TypeHelper.ObjectToInt(reader["productcount"]);
                storeRankList.Add(storeRankInfo);
            }
            reader.Close();
            return storeRankList;
        }

        /// <summary>
        /// 创建店铺等级
        /// </summary>
        public static void CreateStoreRank(StoreRankInfo storeRankInfo)
        {
            BrnMall.Core.BMAData.RDBS.CreateStoreRank(storeRankInfo);
        }

        /// <summary>
        /// 删除店铺等级
        /// </summary>
        /// <param name="storeRid">店铺等级id</param>
        public static void DeleteStoreRankById(int storeRid)
        {
            BrnMall.Core.BMAData.RDBS.DeleteStoreRankById(storeRid);
        }

        /// <summary>
        /// 更新店铺等级
        /// </summary>
        public static void UpdateStoreRank(StoreRankInfo storeRankInfo)
        {
            BrnMall.Core.BMAData.RDBS.UpdateStoreRank(storeRankInfo);
        }
    }
}
