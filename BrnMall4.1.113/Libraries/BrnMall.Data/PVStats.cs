using System;
using System.Data;
using System.Collections.Generic;

using BrnMall.Core;

namespace BrnMall.Data
{
    /// <summary>
    /// PV统计数据访问类
    /// </summary>
    public partial class PVStats
    {
        #region 辅助方法

        /// <summary>
        /// 从IDataReader创建PVStatInfo
        /// </summary>
        public static PVStatInfo BuildPVStatFromReader(IDataReader reader)
        {
            PVStatInfo pvStatInfo = new PVStatInfo();

            pvStatInfo.RecordId = TypeHelper.ObjectToInt(reader["recordid"]);
            pvStatInfo.StoreId = TypeHelper.ObjectToInt(reader["storeid"]);
            pvStatInfo.Category = reader["category"].ToString();
            pvStatInfo.Value = reader["value"].ToString();
            pvStatInfo.Count = TypeHelper.ObjectToInt(reader["count"]);

            return pvStatInfo;
        }

        #endregion

        /// <summary>
        /// 更新PV统计
        /// </summary>
        /// <param name="updatePVStatState">更新PV统计状态</param>
        public static void UpdatePVStat(UpdatePVStatState updatePVStatState)
        {
            BrnMall.Core.BMAData.RDBS.UpdatePVStat(updatePVStatState);
        }

        /// <summary>
        /// 获得省级区域统计
        /// </summary>
        /// <param name="storeId">店铺id(0代表整个系统)</param>
        /// <returns></returns>
        public static DataTable GetProvinceRegionStat(int storeId)
        {
            return BrnMall.Core.BMAData.RDBS.GetProvinceRegionStat(storeId);
        }

        /// <summary>
        /// 获得市级区域统计
        /// </summary>
        /// <param name="storeId">店铺id(0代表整个系统)</param>
        /// <param name="provinceId">省id</param>
        /// <returns></returns>
        public static DataTable GetCityRegionStat(int storeId, int provinceId)
        {
            return BrnMall.Core.BMAData.RDBS.GetCityRegionStat(storeId, provinceId);
        }

        /// <summary>
        /// 获得区/县级区域统计
        /// </summary>
        /// <param name="storeId">店铺id(0代表整个系统)</param>
        /// <param name="cityId">市id</param>
        /// <returns></returns>
        public static DataTable GetCountyRegionStat(int storeId, int cityId)
        {
            return BrnMall.Core.BMAData.RDBS.GetCountyRegionStat(storeId, cityId);
        }

        /// <summary>
        /// 获得PV统计列表
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static List<PVStatInfo> GetPVStatList(string condition)
        {
            List<PVStatInfo> pvStatList = new List<PVStatInfo>();
            IDataReader reader = BrnMall.Core.BMAData.RDBS.GetPVStatList(condition);
            while (reader.Read())
            {
                PVStatInfo pvStatInfo = BuildPVStatFromReader(reader);
                pvStatList.Add(pvStatInfo);
            }

            reader.Close();
            return pvStatList;
        }

        /// <summary>
        /// 获得PV统计
        /// </summary>
        /// <param name="category">分类</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static PVStatInfo GetPVStatByCategoryAndValue(string category, string value)
        {
            PVStatInfo pvStatInfo = null;
            IDataReader reader = BrnMall.Core.BMAData.RDBS.GetPVStatByCategoryAndValue(category, value);
            if (reader.Read())
            {
                pvStatInfo = BuildPVStatFromReader(reader);
            }

            reader.Close();
            return pvStatInfo;
        }
    }
}
