using System;
using System.Web;
using System.Collections.Generic;

using BrnMall.Core;

namespace BrnMall.Services
{
    /// <summary>
    /// 区域操作管理类
    /// </summary>
    public partial class Regions
    {
        /// <summary>
        /// 获得全部区域
        /// </summary>
        /// <returns></returns>
        public static List<RegionInfo> GetAllRegion()
        {
            return BrnMall.Data.Regions.GetAllRegion();
        }

        /// <summary>
        /// 获得省列表
        /// </summary>
        /// <returns></returns>
        public static List<RegionInfo> GetProvinceList()
        {
            List<RegionInfo> provinceList = BrnMall.Core.BMACache.Get(CacheKeys.MALL_REGION_CHILDLIST + 0) as List<RegionInfo>;
            if (provinceList == null)
            {
                provinceList = GetRegionList(0);
                BrnMall.Core.BMACache.Insert(CacheKeys.MALL_REGION_CHILDLIST + 0, provinceList);
            }
            return provinceList;
        }

        /// <summary>
        /// 获得市列表
        /// </summary>
        /// <param name="provinceId">省id</param>
        /// <returns></returns>
        public static List<RegionInfo> GetCityList(int provinceId)
        {
            List<RegionInfo> cityList = BrnMall.Core.BMACache.Get(CacheKeys.MALL_REGION_CHILDLIST + provinceId) as List<RegionInfo>;
            if (cityList == null)
            {
                cityList = GetRegionList(provinceId);
                BrnMall.Core.BMACache.Insert(CacheKeys.MALL_REGION_CHILDLIST + provinceId, cityList);
            }
            return cityList;
        }

        /// <summary>
        /// 获得县或区列表
        /// </summary>
        /// <param name="cityId">市id</param>
        /// <returns></returns>
        public static List<RegionInfo> GetCountyList(int cityId)
        {
            List<RegionInfo> countyList = BrnMall.Core.BMACache.Get(CacheKeys.MALL_REGION_CHILDLIST + cityId) as List<RegionInfo>;
            if (countyList == null)
            {
                countyList = GetRegionList(cityId);
                BrnMall.Core.BMACache.Insert(CacheKeys.MALL_REGION_CHILDLIST + cityId, countyList);
            }
            return countyList;
        }

        /// <summary>
        /// 获得区域列表
        /// </summary>
        /// <param name="parentId">父id</param>
        /// <returns></returns>
        public static List<RegionInfo> GetRegionList(int parentId)
        {
            return BrnMall.Data.Regions.GetRegionList(parentId);
        }

        /// <summary>
        /// 获得区域
        /// </summary>
        /// <param name="regionId">区域id</param>
        /// <returns></returns>
        public static RegionInfo GetRegionById(int regionId)
        {
            if (regionId > 0)
            {
                RegionInfo regionInfo = BrnMall.Core.BMACache.Get(CacheKeys.MALL_REGION_INFOBYID + regionId) as RegionInfo;
                if (regionInfo == null)
                {
                    regionInfo = BrnMall.Data.Regions.GetRegionById(regionId);
                    BrnMall.Core.BMACache.Insert(CacheKeys.MALL_REGION_INFOBYID + regionId, regionInfo);
                }
                return regionInfo;
            }
            return null;
        }

        /// <summary>
        /// 获得区域
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="layer">级别</param>
        /// <returns></returns>
        public static RegionInfo GetRegionByNameAndLayer(string name, int layer)
        {
            RegionInfo regionInfo = BrnMall.Core.BMACache.Get(string.Format(CacheKeys.MALL_REGION_INFOBYNAMEANDLAYER, name, layer)) as RegionInfo;
            if (regionInfo == null)
            {
                regionInfo = BrnMall.Data.Regions.GetRegionByNameAndLayer(name, layer);
                BrnMall.Core.BMACache.Insert(string.Format(CacheKeys.MALL_REGION_INFOBYNAMEANDLAYER, name, layer), regionInfo);
            }
            return regionInfo;
        }

        /// <summary>
        /// 获取IP对应区域
        /// </summary>
        /// <param name="ip">ip</param>
        /// <returns></returns>
        public static RegionInfo GetRegionByIP(string ip)
        {
            RegionInfo regionInfo = null;
            HttpCookie cookie = HttpContext.Current.Request.Cookies["loc"];
            if (cookie != null)
            {
                if (cookie["ip"] == ip)
                {
                    regionInfo = GetRegionById(TypeHelper.StringToInt(cookie["regionid"]));
                }
                else
                {
                    cookie.Values["ip"] = ip;
                    regionInfo = IPSeek.Seek(ip);
                    if (regionInfo != null)
                        cookie.Values["regionid"] = regionInfo.RegionId.ToString();
                    else
                        cookie.Values["regionid"] = "-1";
                    cookie.Expires = DateTime.Now.AddYears(1);

                    HttpContext.Current.Response.AppendCookie(cookie);
                }

            }
            else
            {
                cookie = new HttpCookie("loc");
                cookie.Values["ip"] = ip;
                regionInfo = IPSeek.Seek(ip);
                if (regionInfo != null)
                    cookie.Values["regionid"] = regionInfo.RegionId.ToString();
                else
                    cookie.Values["regionid"] = "-1";
                cookie.Expires = DateTime.Now.AddYears(1);

                HttpContext.Current.Response.AppendCookie(cookie);
            }

            if (regionInfo != null)
                return regionInfo;
            else
                return new RegionInfo() { RegionId = -1, Name = "未知区域" };
        }
    }
}
