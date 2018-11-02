using System;

namespace BrnMall.Core
{
    /// <summary>
    /// BrnMall非关系型数据库策略之店铺接口
    /// </summary>
    public partial interface IStoreNOSQLStrategy
    {
        #region 店铺

        /// <summary>
        /// 创建店铺
        /// </summary>
        /// <param name="storeInfo">店铺信息</param>
        void CreateStore(StoreInfo storeInfo);

        /// <summary>
        /// 更新店铺
        /// </summary>
        /// <param name="storeInfo">店铺信息</param>
        void UpdateStore(StoreInfo storeInfo);

        /// <summary>
        /// 获得店铺
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <returns></returns>
        StoreInfo GetStoreById(int storeId);

        /// <summary>
        /// 更新店铺状态
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="state">状态</param>
        /// <param name="stateEndTime">状态结束时间</param>
        void UpdateStoreState(int storeId, StoreState state, DateTime stateEndTime);

        /// <summary>
        /// 更新店铺积分
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="dePoint">商品描述评分</param>
        /// <param name="sePoint">商家服务评分</param>
        /// <param name="shPoint">商家配送评分</param>
        void UpdateStorePoint(int storeId, decimal dePoint, decimal sePoint, decimal shPoint);

        #endregion

        #region 店长

        /// <summary>
        /// 创建店长
        /// </summary>
        /// <param name="storeKeeperInfo">店长信息</param>
        void CreateStoreKeeper(StoreKeeperInfo storeKeeperInfo);

        /// <summary>
        /// 更新店长
        /// </summary>
        /// <param name="storeKeeperInfo">店长信息</param>
        void UpdateStoreKeeper(StoreKeeperInfo storeKeeperInfo);

        /// <summary>
        /// 获得店长
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <returns></returns>
        StoreKeeperInfo GetStoreKeeperById(int storeId);

        #endregion
    }
}
