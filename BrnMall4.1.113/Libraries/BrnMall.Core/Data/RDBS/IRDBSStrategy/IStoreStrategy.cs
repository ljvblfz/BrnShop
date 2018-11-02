using System;
using System.Data;

namespace BrnMall.Core
{
    /// <summary>
    /// BrnMall关系数据库策略之店铺分部接口
    /// </summary>
    public partial interface IRDBSStrategy
    {
        #region 店铺行业

        /// <summary>
        /// 获得店铺行业列表
        /// </summary>
        IDataReader GetStoreIndustryList();

        /// <summary>
        /// 创建店铺行业
        /// </summary>
        void CreateStoreIndustry(StoreIndustryInfo storeIndustryInfo);

        /// <summary>
        /// 更新店铺行业
        /// </summary>
        void UpdateStoreIndustry(StoreIndustryInfo storeIndustryInfo);

        /// <summary>
        /// 删除店铺行业
        /// </summary>
        /// <param name="storeIid">店铺行业id</param>
        void DeleteStoreIndustryById(int storeIid);

        #endregion

        #region 店铺等级

        /// <summary>
        /// 获得店铺等级列表
        /// </summary>
        /// <returns></returns>
        IDataReader GetStoreRankList();

        /// <summary>
        /// 创建店铺等级
        /// </summary>
        void CreateStoreRank(StoreRankInfo storeRankInfo);

        /// <summary>
        /// 删除店铺等级
        /// </summary>
        /// <param name="storeRid">店铺等级id</param>
        void DeleteStoreRankById(int storeRid);

        /// <summary>
        /// 更新店铺等级
        /// </summary>
        void UpdateStoreRank(StoreRankInfo storeRankInfo);

        #endregion

        #region 店铺评价

        /// <summary>
        /// 创建店铺评价
        /// </summary>
        /// <param name="storeReviewInfo">店铺评价信息</param>
        void CreateStoreReview(StoreReviewInfo storeReviewInfo);

        /// <summary>
        /// 获得店铺评价
        /// </summary>
        /// <param name="oid">订单id</param>
        IDataReader GetStoreReviewByOid(int oid);

        /// <summary>
        /// 汇总店铺评价
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        DataTable SumStoreReview(int storeId, DateTime startTime, DateTime endTime);

        #endregion

        #region 店铺

        /// <summary>
        /// 创建店铺
        /// </summary>
        /// <param name="storeInfo">店铺信息</param>
        /// <returns>店铺id</returns>
        int CreateStore(StoreInfo storeInfo);

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
        IDataReader GetStoreById(int storeId);

        /// <summary>
        /// 后台获得店铺列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        DataTable AdminGetStoreList(int pageSize, int pageNumber, string condition);

        /// <summary>
        /// 后台获得店铺选择列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        DataTable AdminGetStoreSelectList(int pageSize, int pageNumber, string condition);

        /// <summary>
        /// 后台获得店铺列表条件
        /// </summary>
        /// <param name="storeName">店铺名称</param>
        /// <param name="storeRid">店铺等级id</param>
        /// <param name="storeIid">店铺行业id</param>
        /// <param name="state">店铺状态</param>
        /// <returns></returns>
        string AdminGetStoreListCondition(string storeName, int storeRid, int storeIid, int state);

        /// <summary>
        /// 后台获得店铺数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        int AdminGetStoreCount(string condition);

        /// <summary>
        /// 根据店铺名称得到店铺id
        /// </summary>
        /// <param name="storeName">店铺名称</param>
        /// <returns></returns>
        int GetStoreIdByName(string storeName);

        /// <summary>
        /// 后台根据店铺名称得到店铺id
        /// </summary>
        /// <param name="storeName">店铺名称</param>
        /// <returns></returns>
        int AdminGetStoreIdByName(string storeName);

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

        /// <summary>
        /// 获得店铺数量通过店铺行业id
        /// </summary>
        /// <param name="storeIid">店铺行业id</param>
        /// <returns></returns>
        int GetStoreCountByStoreIid(int storeIid);

        /// <summary>
        /// 获得店铺数量通过店铺等级id
        /// </summary>
        /// <param name="storeRid">店铺等级id</param>
        /// <returns></returns>
        int GetStoreCountByStoreRid(int storeRid);

        /// <summary>
        /// 获得店铺id列表
        /// </summary>
        /// <returns></returns>
        DataTable GetStoreIdList();

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
        IDataReader GetStoreKeeperById(int storeId);

        #endregion

        #region 店铺分类

        /// <summary>
        /// 获得店铺分类列表
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <returns></returns>
        IDataReader GetStoreClassList(int storeId);

        /// <summary>
        /// 创建店铺分类
        /// </summary>
        int CreateStoreClass(StoreClassInfo storeClassInfo);

        /// <summary>
        /// 更新店铺分类
        /// </summary>
        void UpdateStoreClass(StoreClassInfo storeClassInfo);

        /// <summary>
        /// 删除店铺分类
        /// </summary>
        /// <param name="storeCid">店铺分类id</param>
        void DeleteStoreClassById(int storeCid);

        #endregion

        #region 店铺配送模板

        /// <summary>
        /// 创建店铺配送模板
        /// </summary>
        /// <param name="storeShipTemplateInfo">店铺配送模板信息</param>
        int CreateStoreShipTemplate(StoreShipTemplateInfo storeShipTemplateInfo);

        /// <summary>
        /// 更新店铺配送模板
        /// </summary>
        /// <param name="storeShipTemplateInfo">店铺配送模板信息</param>
        void UpdateStoreShipTemplate(StoreShipTemplateInfo storeShipTemplateInfo);

        /// <summary>
        /// 删除店铺配送模板
        /// </summary>
        /// <param name="storeSTid">店铺配送模板id</param>
        void DeleteStoreShipTemplateById(int storeSTid);

        /// <summary>
        /// 获得店铺配送模板
        /// </summary>
        /// <param name="storeSTid">店铺配送模板id</param>
        /// <returns></returns>
        IDataReader GetStoreShipTemplateById(int storeSTid);

        /// <summary>
        /// 获得店铺配送模板列表
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <returns></returns>
        IDataReader GetStoreShipTemplateList(int storeId);

        #endregion

        #region 店铺配送费用

        /// <summary>
        /// 创建店铺配送费用
        /// </summary>
        /// <param name="storeShipFeeInfo">店铺配送费用信息</param>
        void CreateStoreShipFee(StoreShipFeeInfo storeShipFeeInfo);

        /// <summary>
        /// 更新店铺配送费用
        /// </summary>
        /// <param name="storeShipFeeInfo">店铺配送费用信息</param>
        void UpdateStoreShipFee(StoreShipFeeInfo storeShipFeeInfo);

        /// <summary>
        /// 删除店铺配送费用
        /// </summary>
        /// <param name="recordId">记录id</param>
        void DeleteStoreShipFeeById(int recordId);

        /// <summary>
        /// 获得店铺配送费用
        /// </summary>
        /// <param name="recordId">记录id</param>
        /// <returns></returns>
        IDataReader GetStoreShipFeeById(int recordId);

        /// <summary>
        /// 后台获得店铺配送费用列表
        /// </summary>
        /// <param name="storeSTid">店铺配送模板id</param>
        /// <returns></returns>
        DataTable AdminGetStoreShipFeeList(int storeSTid);

        /// <summary>
        /// 获得店铺配送费用
        /// </summary>
        /// <param name="storeSTid">店铺模板id</param>
        /// <param name="provinceId">省id</param>
        /// <param name="cityId">市id</param>
        /// <returns></returns>
        IDataReader GetStoreShipFeeByStoreSTidAndRegion(int storeSTid, int provinceId, int cityId);

        #endregion
    }
}
