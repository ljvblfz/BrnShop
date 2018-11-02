using System;
using System.Data;
using System.Threading;
using System.Collections.Generic;

using BrnMall.Core;

namespace BrnMall.Services
{
    /// <summary>
    /// 后台优惠劵操作管理类
    /// </summary>
    public partial class AdminCoupons : Coupons
    {
        /// <summary>
        /// 创建优惠劵类型
        /// </summary>
        /// <param name="couponTypeInfo">优惠劵类型信息</param>
        public static void CreateCouponType(CouponTypeInfo couponTypeInfo)
        {
            BrnMall.Data.Coupons.CreateCouponType(couponTypeInfo);
        }

        /// <summary>
        /// 删除优惠劵类型
        /// </summary>
        /// <param name="couponTypeIdList">优惠劵类型id列表</param>
        public static void DeleteCouponTypeById(int[] couponTypeIdList)
        {
            if (couponTypeIdList != null && couponTypeIdList.Length > 0)
            {
                //删除优惠劵类型
                BrnMall.Data.Coupons.DeleteCouponTypeById(CommonHelper.IntArrayToString(couponTypeIdList));

                //删除优惠劵类型缓存
                foreach (int couponTypeId in couponTypeIdList)
                {
                    BrnMall.Core.BMACache.Remove(CacheKeys.MALL_COUPONTYPE_INFO + couponTypeId);
                }
            }
        }

        /// <summary>
        /// 后台获得优惠劵类型
        /// </summary>
        /// <param name="couponTypeId">优惠劵类型id</param>
        /// <returns></returns>
        public static CouponTypeInfo AdminGetCouponTypeById(int couponTypeId)
        {
            if (couponTypeId < 1) return null;
            return BrnMall.Data.Coupons.AdminGetCouponTypeById(couponTypeId);
        }

        /// <summary>
        /// 后台获得优惠劵类型列表条件
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="type">0代表正在发放，1代表正在使用，-1代表全部</param>
        /// <param name="couponTypeName">优惠劵类型名称</param>
        /// <returns></returns>
        public static string AdminGetCouponTypeListCondition(int storeId, int type, string couponTypeName)
        {
            return BrnMall.Data.Coupons.AdminGetCouponTypeListCondition(storeId, type, couponTypeName);
        }

        /// <summary>
        /// 后台获得优惠劵类型列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static DataTable AdminGetCouponTypeList(int pageSize, int pageNumber, string condition)
        {
            return BrnMall.Data.Coupons.AdminGetCouponTypeList(pageSize, pageNumber, condition);
        }

        /// <summary>
        /// 后台获得优惠劵类型数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static int AdminGetCouponTypeCount(string condition)
        {
            return BrnMall.Data.Coupons.AdminGetCouponTypeCount(condition);
        }

        /// <summary>
        /// 更新优惠劵类型状态
        /// </summary>
        /// <param name="couponTypeId">优惠劵类型id</param>
        /// <param name="state">状态(0代表关闭，1代表打开)</param>
        /// <returns></returns>
        public static bool ChangeCouponTypeState(int couponTypeId, int state)
        {
            bool result = BrnMall.Data.Coupons.ChangeCouponTypeState(couponTypeId, state);
            if (result)
            {
                BrnMall.Core.BMACache.Remove(CacheKeys.MALL_COUPONTYPE_INFO + couponTypeId);
            }
            return result;
        }

        /// <summary>
        /// 获得店铺id列表
        /// </summary>
        /// <param name="couponTypeIdList">优惠劵类型id列表</param>
        /// <returns></returns>
        public static DataTable GetStoreIdListByCouponTypeId(int[] couponTypeIdList)
        {
            if (couponTypeIdList != null && couponTypeIdList.Length > 0)
                return BrnMall.Core.BMAData.RDBS.GetStoreIdListByCouponTypeId(CommonHelper.IntArrayToString(couponTypeIdList));
            return new DataTable();
        }

        /// <summary>
        /// 判断优惠劵类型是否为同一指定店铺
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="couponTypeIdList">优惠劵类型id列表</param>
        /// <returns></returns>
        public static bool IsStoreCouponTypeByCouponTypeId(int storeId, int[] couponTypeIdList)
        {
            DataTable storeIdList = GetStoreIdListByCouponTypeId(couponTypeIdList);
            if (storeIdList.Rows.Count != 1 || storeId != TypeHelper.ObjectToInt(storeIdList.Rows[0][0]))
                return false;
            return true;
        }





        /// <summary>
        /// 后台获得优惠劵列表条件
        /// </summary>
        /// <param name="sn">编号</param>
        /// <param name="uid">用户id</param>
        /// <param name="coupontTypeId">优惠劵类型id</param>
        /// <returns></returns>
        public static string AdminGetCouponListCondition(string sn, int uid, int couponTypeId)
        {
            return BrnMall.Data.Coupons.AdminGetCouponListCondition(sn, uid, couponTypeId);
        }

        /// <summary>
        /// 后台获得优惠劵列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static DataTable AdminGetCouponList(int pageSize, int pageNumber, string condition)
        {
            return BrnMall.Data.Coupons.AdminGetCouponList(pageSize, pageNumber, condition);
        }

        /// <summary>
        /// 后台获得优惠劵列表数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static int AdminGetCouponCount(string condition)
        {
            return BrnMall.Data.Coupons.AdminGetCouponCount(condition);
        }

        /// <summary>
        /// 后台发放优惠劵给用户
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="couponTypeId">优惠劵类型id</param>
        /// <param name="storeId">店铺id</param>
        /// <param name="money">优惠劵金额</param>
        /// <param name="count">数量</param>
        /// <param name="sendUid">发放人id</param>
        /// <param name="sendTime">发放时间</param>
        /// <param name="sendIP">发放ip</param>
        public static void AdminSendCouponToUser(int uid, int couponTypeId, int storeId, int money, int count, int sendUid, DateTime sendTime, string sendIP)
        {
            for (int i = 0; i < count; i++)
            {
                CouponInfo couponInfo = new CouponInfo();

                couponInfo.CouponSN = GenerateCouponSN();
                couponInfo.Uid = uid;
                couponInfo.CouponTypeId = couponTypeId;
                couponInfo.StoreId = storeId;
                couponInfo.Oid = 0;
                couponInfo.UseTime = new DateTime(1900, 1, 1);
                couponInfo.UseIP = "";
                couponInfo.Money = money;
                couponInfo.ActivateTime = sendTime;
                couponInfo.ActivateIP = sendIP;
                couponInfo.CreateUid = sendUid;
                couponInfo.CreateOid = 0;
                couponInfo.CreateTime = sendTime;
                couponInfo.CreateIP = sendIP;

                CreateCoupon(couponInfo);
            }
        }

        /// <summary>
        /// 后台批量生成优惠劵
        /// </summary>
        /// <param name="couponTypeId">优惠劵类型id</param>
        /// <param name="storeId">店铺id</param>
        /// <param name="money">优惠劵金额</param>
        /// <param name="count">数量</param>
        /// <param name="sendUid">发放人id</param>
        /// <param name="sendTime">发放时间</param>
        /// <param name="sendIP">发放ip</param>
        public static void AdminBatchGenerateCoupon(int couponTypeId, int storeId, int money, int count, int sendUid, DateTime sendTime, string sendIP)
        {
            if (count > 100)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(AdminGenerateCoupon), new object[7] { couponTypeId, storeId, money, count, sendUid, sendTime, sendIP });
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    CouponInfo couponInfo = new CouponInfo();

                    couponInfo.CouponSN = Coupons.GenerateCouponSN();
                    couponInfo.Uid = 0;
                    couponInfo.CouponTypeId = couponTypeId;
                    couponInfo.StoreId = storeId;
                    couponInfo.Oid = 0;
                    couponInfo.UseTime = new DateTime(1900, 1, 1);
                    couponInfo.UseIP = "";
                    couponInfo.Money = money;
                    couponInfo.ActivateTime = new DateTime(1900, 1, 1);
                    couponInfo.ActivateIP = "";
                    couponInfo.CreateUid = sendUid;
                    couponInfo.CreateOid = 0;
                    couponInfo.CreateTime = sendTime;
                    couponInfo.CreateIP = sendIP;

                    CreateCoupon(couponInfo);
                }
            }
        }

        /// <summary>
        /// 后台生成优惠劵
        /// </summary>
        /// <param name="o">参数对象</param>
        private static void AdminGenerateCoupon(object o)
        {
            object[] parms = (object[])o;

            int couponTypeId = (int)parms[0];
            int storeId = (int)parms[1];
            int money = (int)parms[2];
            int count = (int)parms[3];
            int sendUid = (int)parms[4];
            DateTime sendTime = (DateTime)parms[5];
            string sendIP = (string)parms[6];

            for (int i = 0; i < count; i++)
            {
                CouponInfo couponInfo = new CouponInfo();

                couponInfo.CouponSN = Coupons.GenerateCouponSN();
                couponInfo.Uid = 0;
                couponInfo.CouponTypeId = couponTypeId;
                couponInfo.StoreId = storeId;
                couponInfo.Oid = 0;
                couponInfo.UseTime = new DateTime(1900, 1, 1);
                couponInfo.UseIP = "";
                couponInfo.Money = money;
                couponInfo.ActivateTime = new DateTime(1900, 1, 1);
                couponInfo.ActivateIP = "";
                couponInfo.CreateUid = sendUid;
                couponInfo.CreateOid = 0;
                couponInfo.CreateTime = sendTime;
                couponInfo.CreateIP = sendIP;

                AdminCoupons.CreateCoupon(couponInfo);
            }
        }

        /// <summary>
        /// 获得店铺id列表
        /// </summary>
        /// <param name="couponIdList">优惠劵id列表</param>
        /// <returns></returns>
        public static DataTable GetStoreIdListByCouponId(int[] couponIdList)
        {
            if (couponIdList != null && couponIdList.Length > 0)
                return BrnMall.Core.BMAData.RDBS.GetStoreIdListByCouponId(CommonHelper.IntArrayToString(couponIdList));
            return new DataTable();
        }

        /// <summary>
        /// 判断优惠劵是否为同一指定店铺
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="couponIdList">优惠劵id列表</param>
        /// <returns></returns>
        public static bool IsStoreCouponByCouponId(int storeId, int[] couponIdList)
        {
            DataTable storeIdList = GetStoreIdListByCouponId(couponIdList);
            if (storeIdList.Rows.Count != 1 || storeId != TypeHelper.ObjectToInt(storeIdList.Rows[0][0]))
                return false;
            return true;
        }






        /// <summary>
        /// 添加优惠劵商品
        /// </summary>
        /// <param name="couponTypeId">优惠劵类型id</param>
        /// <param name="pid">商品id</param>
        public static void AddCouponProduct(int couponTypeId, int pid)
        {
            if (couponTypeId > 0 && pid > 0)
                BrnMall.Data.Coupons.AddCouponProduct(couponTypeId, pid);
        }

        /// <summary>
        /// 删除优惠劵商品
        /// </summary>
        /// <param name="recordIdList">记录id列表</param>
        /// <returns></returns>
        public static bool DeleteCouponProductByRecordId(int[] recordIdList)
        {
            if (recordIdList != null && recordIdList.Length > 0)
                return BrnMall.Data.Coupons.DeleteCouponProductByRecordId(CommonHelper.IntArrayToString(recordIdList));
            return false;
        }

        /// <summary>
        /// 后台获得优惠劵商品列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="couponTypeId">优惠劵类型id</param>
        /// <returns></returns>
        public static DataTable AdminGetCouponProductList(int pageSize, int pageNumber, int couponTypeId)
        {
            return BrnMall.Data.Coupons.AdminGetCouponProductList(pageSize, pageNumber, couponTypeId);
        }

        /// <summary>
        /// 后台获得优惠劵商品数量
        /// </summary>
        /// <param name="couponTypeId">优惠劵类型id</param>
        /// <returns></returns>
        public static int AdminGetCouponProductCount(int couponTypeId)
        {
            return BrnMall.Data.Coupons.AdminGetCouponProductCount(couponTypeId);
        }

        /// <summary>
        /// 获得店铺id列表
        /// </summary>
        /// <param name="recordIdList">优惠劵商品记录id列表</param>
        /// <returns></returns>
        public static DataTable GetStoreIdListByCouponProductRecordId(int[] recordIdList)
        {
            if (recordIdList != null && recordIdList.Length > 0)
                return BrnMall.Core.BMAData.RDBS.GetStoreIdListByCouponProductRecordId(CommonHelper.IntArrayToString(recordIdList));
            return new DataTable();
        }

        /// <summary>
        /// 判断优惠劵商品是否为同一指定店铺
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="recordIdList">优惠劵商品id列表</param>
        /// <returns></returns>
        public static bool IsStoreCouponProductByCouponProductRecordId(int storeId, int[] recordIdList)
        {
            DataTable storeIdList = GetStoreIdListByCouponProductRecordId(recordIdList);
            if (storeIdList.Rows.Count != 1 || storeId != TypeHelper.ObjectToInt(storeIdList.Rows[0][0]))
                return false;
            return true;
        }
    }
}
