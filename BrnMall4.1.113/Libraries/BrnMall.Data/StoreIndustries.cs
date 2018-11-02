using System;
using System.Data;
using System.Collections.Generic;

using BrnMall.Core;

namespace BrnMall.Data
{
    /// <summary>
    /// 店铺行业数据访问类
    /// </summary>
    public partial class StoreIndustries
    {
        /// <summary>
        /// 获得店铺行业列表
        /// </summary>
        public static List<StoreIndustryInfo> GetStoreIndustryList()
        {
            List<StoreIndustryInfo> storeIndustrylist = new List<StoreIndustryInfo>();

            IDataReader reader = BrnMall.Core.BMAData.RDBS.GetStoreIndustryList();
            while (reader.Read())
            {
                StoreIndustryInfo storeIndustryInfo = new StoreIndustryInfo();
                storeIndustryInfo.StoreIid = TypeHelper.ObjectToInt(reader["storeiid"]);
                storeIndustryInfo.Title = reader["title"].ToString();
                storeIndustryInfo.DisplayOrder = TypeHelper.ObjectToInt(reader["displayorder"]);
                storeIndustrylist.Add(storeIndustryInfo);
            }
            reader.Close();

            return storeIndustrylist;
        }

        /// <summary>
        /// 创建店铺行业
        /// </summary>
        public static void CreateStoreIndustry(StoreIndustryInfo storeIndustryInfo)
        {
            BrnMall.Core.BMAData.RDBS.CreateStoreIndustry(storeIndustryInfo);
        }

        /// <summary>
        /// 更新店铺行业
        /// </summary>
        public static void UpdateStoreIndustry(StoreIndustryInfo storeIndustryInfo)
        {
            BrnMall.Core.BMAData.RDBS.UpdateStoreIndustry(storeIndustryInfo);
        }

        /// <summary>
        /// 删除店铺行业
        /// </summary>
        /// <param name="storeIid">店铺行业id</param>
        public static void DeleteStoreIndustryById(int storeIid)
        {
            BrnMall.Core.BMAData.RDBS.DeleteStoreIndustryById(storeIid);
        }
    }
}
