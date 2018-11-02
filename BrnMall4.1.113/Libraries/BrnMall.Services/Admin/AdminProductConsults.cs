using System;
using System.Data;
using System.Collections.Generic;

using BrnMall.Core;

namespace BrnMall.Services
{
    /// <summary>
    /// 后台商品咨询操作管理类
    /// </summary>
    public partial class AdminProductConsults : ProductConsults
    {
        /// <summary>
        /// 创建商品咨询类型
        /// </summary>
        public static void CreateProductConsultType(ProductConsultTypeInfo productConsultTypeInfo)
        {
            BrnMall.Data.ProductConsults.CreateProductConsultType(productConsultTypeInfo);
            BrnMall.Core.BMACache.Remove(CacheKeys.MALL_PRODUCTCONSULTTYPE_LIST);
        }

        /// <summary>
        /// 更新商品咨询类型
        /// </summary>
        public static void UpdateProductConsultType(ProductConsultTypeInfo productConsultTypeInfo)
        {
            BrnMall.Data.ProductConsults.UpdateProductConsultType(productConsultTypeInfo);
            BrnMall.Core.BMACache.Remove(CacheKeys.MALL_PRODUCTCONSULTTYPE_LIST);
        }

        /// <summary>
        /// 删除商品咨询类型
        /// </summary>
        /// <param name="consultTypeId">商品咨询类型id</param>
        /// <returns></returns>
        public static int DeleteProductConsultTypeById(int consultTypeId)
        {
            string condition = AdminGetProductConsultListCondition(consultTypeId, 0, 0, 0, "", "", "");
            int count = AdminGetProductConsultCount(condition);
            if (count > 0)
                return 0;

            BrnMall.Data.ProductConsults.DeleteProductConsultTypeById(consultTypeId);
            BrnMall.Core.BMACache.Remove(CacheKeys.MALL_PRODUCTCONSULTTYPE_LIST);
            return 1;
        }





        /// <summary>
        /// 后台获得商品咨询
        /// </summary>
        /// <param name="consultId">咨询id</param>
        /// <returns></returns>
        public static ProductConsultInfo AdminGetProductConsultById(int consultId)
        {
            if (consultId > 0)
                return BrnMall.Data.ProductConsults.AdminGetProductConsultById(consultId);
            return null;
        }

        /// <summary>
        /// 后台获得商品咨询列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static DataTable AdminGetProductConsultList(int pageSize, int pageNumber, string condition)
        {
            return BrnMall.Data.ProductConsults.AdminGetProductConsultList(pageSize, pageNumber, condition);
        }

        /// <summary>
        /// 后台获得商品咨询列表条件
        /// </summary>
        /// <param name="consultTypeId">商品咨询类型id</param>
        /// <param name="storeId">店铺id</param>
        /// <param name="pid">商品id</param>
        /// <param name="uid">用户id</param>
        /// <param name="consultMessage">咨询内容</param>
        /// <param name="consultStartTime">咨询开始时间</param>
        /// <param name="consultEndTime">咨询结束时间</param>
        /// <returns></returns>
        public static string AdminGetProductConsultListCondition(int consultTypeId, int storeId, int pid, int uid, string consultMessage, string consultStartTime, string consultEndTime)
        {
            return BrnMall.Data.ProductConsults.AdminGetProductConsultListCondition(consultTypeId, storeId, pid, uid, consultMessage, consultStartTime, consultEndTime);
        }

        /// <summary>
        /// 后台获得商品咨询数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static int AdminGetProductConsultCount(string condition)
        {
            return BrnMall.Data.ProductConsults.AdminGetProductConsultCount(condition);
        }

        /// <summary>
        /// 获得店铺id列表
        /// </summary>
        /// <param name="consultIdList">商品咨询id列表</param>
        /// <returns></returns>
        public static DataTable GetStoreIdListByConsultId(int[] consultIdList)
        {
            if (consultIdList != null && consultIdList.Length > 0)
                return BrnMall.Data.ProductConsults.GetStoreIdListByConsultId(CommonHelper.IntArrayToString(consultIdList));
            return new DataTable();
        }

        /// <summary>
        /// 判断商品咨询是否为同一指定店铺
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="consultIdList">商品咨询id列表</param>
        /// <returns></returns>
        public static bool IsStoreConsultByConsultId(int storeId, int[] consultIdList)
        {
            DataTable storeIdList = GetStoreIdListByConsultId(consultIdList);
            if (storeIdList.Rows.Count != 1)
                return false;
            if (storeId != TypeHelper.ObjectToInt(storeIdList.Rows[0][0]))
                return false;
            return true;
        }
    }
}
