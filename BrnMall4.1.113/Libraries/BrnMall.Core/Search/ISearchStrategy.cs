using System;
using System.Collections.Generic;

namespace BrnMall.Core
{
    /// <summary>
    /// 搜索策略接口
    /// </summary>
    public partial interface ISearchStrategy
    {
        #region 分词相关

        /// <summary>
        /// 分词
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns></returns>
        List<string> Analyse(string text);

        #endregion

        #region 索引相关

        /// <summary>
        /// 创建商品关键词
        /// </summary>
        /// <param name="productKeywordInfo">商品关键词信息</param>
        void CreateProductKeyword(ProductKeywordInfo productKeywordInfo);

        /// <summary>
        /// 创建商品关键词
        /// </summary>
        /// <param name="pid">商品id</param>
        /// <param name="text">文本</param>
        /// <param name="relevancy">相关性</param>
        void CreateProductKeyword(int pid, string text, int relevancy);

        /// <summary>
        /// 更新商品关键词
        /// </summary>
        /// <param name="productKeywordInfo">商品关键词信息</param>
        void UpdateProductKeyword(ProductKeywordInfo productKeywordInfo);

        /// <summary>
        /// 更新商品关键词
        /// </summary>
        /// <param name="pid">商品id</param>
        /// <param name="text">文本</param>
        /// <param name="relevancy">相关性</param>
        void UpdateProductKeyword(int pid, string text, int relevancy);

        /// <summary>
        /// 删除商品关键词
        /// </summary>
        /// <param name="pid">商品id</param>
        void DeleteProductKeyword(int pid);

        /// <summary>
        /// 删除商品关键词
        /// </summary>
        /// <param name="pid">商品id</param>
        /// <param name="keyword">商品关键词</param>
        void DeleteProductKeyword(int pid, string keyword);

        /// <summary>
        /// 重置商品关键词
        /// </summary>
        /// <param name="pid">商品id</param>
        /// <param name="text">文本</param>
        /// <param name="relevancy">相关性</param>
        void ResetProductKeyword(int pid, string text, int relevancy);

        /// <summary>
        /// 获得商品关键词列表
        /// </summary>
        /// <param name="pid">商品id</param>
        /// <returns></returns>
        List<ProductKeywordInfo> GetProductKeywordList(int pid);

        /// <summary>
        /// 是否存在商品关键词
        /// </summary>
        /// <param name="pid">商品id</param>
        /// <param name="keyword">关键词</param>
        /// <returns></returns>
        bool IsExistProductKeyword(int pid, string keyword);

        #endregion

        #region 搜索相关

        /// <summary>
        /// 搜索商城商品
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="word">搜索词</param>
        /// <param name="cateId">分类id</param>
        /// <param name="brandId">品牌id</param>
        /// <param name="filterPrice">筛选价格</param>
        /// <param name="attrValueIdList">属性值id列表</param>
        /// <param name="onlyStock">是否只显示有货</param>
        /// <param name="sortColumn">排序列</param>
        /// <param name="sortDirection">排序方向</param>
        /// <param name="categoryInfo">分类信息</param>
        /// <param name="catePriceRangeList">分类价格范围列表</param>
        /// <param name="cateAAndVList">分类筛选属性及其值列表</param>
        /// <param name="categoryList">分类列表</param>
        /// <param name="brandInfo">品牌信息</param>
        /// <param name="brandList">品牌列表</param>
        /// <param name="totalCount">商品总数量</param>
        /// <param name="productList">商品列表</param>
        void SearchMallProducts(int pageSize, int pageNumber, string word, int cateId, int brandId, int filterPrice, List<int> attrValueIdList, int onlyStock, int sortColumn, int sortDirection, ref CategoryInfo categoryInfo, ref string[] catePriceRangeList, ref List<KeyValuePair<AttributeInfo, List<AttributeValueInfo>>> cateAAndVList, ref List<CategoryInfo> categoryList, ref BrandInfo brandInfo, ref List<BrandInfo> brandList, ref int totalCount, ref List<StoreProductInfo> productList);

        /// <summary>
        /// 搜索店铺商品
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="word">搜索词</param>
        /// <param name="storeId">店铺id</param>
        /// <param name="storeCid">店铺分类id</param>
        /// <param name="startPrice">开始价格</param>
        /// <param name="endPrice">结束价格</param>
        /// <param name="sortColumn">排序列</param>
        /// <param name="sortDirection">排序方向</param>
        /// <param name="totalCount">商品总数量</param>
        /// <param name="productList">商品列表</param>
        void SearchStoreProducts(int pageSize, int pageNumber, string word, int storeId, int storeCid, int startPrice, int endPrice, int sortColumn, int sortDirection, ref int totalCount, ref List<PartProductInfo> productList);

        #endregion
    }
}
