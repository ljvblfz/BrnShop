using System;
using System.Collections.Generic;

using BrnMall.Core;

namespace BrnMall.Services
{
    /// <summary>
    /// 搜索操作管理类
    /// </summary>
    public partial class Searches
    {
        private static ISearchStrategy _isearchstrategy = BMASearch.Instance;//搜索策略

        /// <summary>
        /// 分词
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns></returns>
        public static List<string> Analyse(string text)
        {
            return _isearchstrategy.Analyse(text);
        }

        /// <summary>
        /// 创建商品关键词
        /// </summary>
        /// <param name="productKeywordInfo">商品关键词信息</param>
        public static void CreateProductKeyword(ProductKeywordInfo productKeywordInfo)
        {
            _isearchstrategy.CreateProductKeyword(productKeywordInfo);
        }

        /// <summary>
        /// 创建商品关键词
        /// </summary>
        /// <param name="pid">商品id</param>
        /// <param name="text">文本</param>
        /// <param name="relevancy">相关性</param>
        public static void CreateProductKeyword(int pid, string text, int relevancy)
        {
            _isearchstrategy.CreateProductKeyword(pid, text, relevancy);
        }

        /// <summary>
        /// 更新商品关键词
        /// </summary>
        /// <param name="productKeywordInfo">商品关键词信息</param>
        public static void UpdateProductKeyword(ProductKeywordInfo productKeywordInfo)
        {
            _isearchstrategy.UpdateProductKeyword(productKeywordInfo);
        }

        /// <summary>
        /// 更新商品关键词
        /// </summary>
        /// <param name="pid">商品id</param>
        /// <param name="text">文本</param>
        /// <param name="relevancy">相关性</param>
        public static void UpdateProductKeyword(int pid, string text, int relevancy)
        {
            _isearchstrategy.UpdateProductKeyword(pid, text, relevancy);
        }

        /// <summary>
        /// 删除商品关键词
        /// </summary>
        /// <param name="pid">商品id</param>
        public static void DeleteProductKeyword(int pid)
        {
            _isearchstrategy.DeleteProductKeyword(pid);
        }

        /// <summary>
        /// 删除商品关键词
        /// </summary>
        /// <param name="pid">商品id</param>
        /// <param name="keyword">商品关键词</param>
        public static void DeleteProductKeyword(int pid, string keyword)
        {
            _isearchstrategy.DeleteProductKeyword(pid, keyword);
        }

        /// <summary>
        /// 重置商品关键词
        /// </summary>
        /// <param name="pid">商品id</param>
        /// <param name="text">文本</param>
        /// <param name="relevancy">相关性</param>
        public static void ResetProductKeyword(int pid, string text, int relevancy)
        {
            _isearchstrategy.ResetProductKeyword(pid, text, relevancy);
        }

        /// <summary>
        /// 获得商品关键词列表
        /// </summary>
        /// <param name="pid">商品id</param>
        /// <returns></returns>
        public static List<ProductKeywordInfo> GetProductKeywordList(int pid)
        {
            return _isearchstrategy.GetProductKeywordList(pid);
        }

        /// <summary>
        /// 是否存在商品关键词
        /// </summary>
        /// <param name="pid">商品id</param>
        /// <param name="keyword">关键词</param>
        /// <returns></returns>
        public static bool IsExistProductKeyword(int pid, string keyword)
        {
            return _isearchstrategy.IsExistProductKeyword(pid, keyword);
        }

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
        public static void SearchMallProducts(int pageSize, int pageNumber, string word, int cateId, int brandId, int filterPrice, List<int> attrValueIdList, int onlyStock, int sortColumn, int sortDirection, ref CategoryInfo categoryInfo, ref string[] catePriceRangeList, ref List<KeyValuePair<AttributeInfo, List<AttributeValueInfo>>> cateAAndVList, ref List<CategoryInfo> categoryList, ref BrandInfo brandInfo, ref List<BrandInfo> brandList, ref int totalCount, ref List<StoreProductInfo> productList)
        {
            _isearchstrategy.SearchMallProducts(pageSize, pageNumber, word, cateId, brandId, filterPrice, attrValueIdList, onlyStock, sortColumn, sortDirection, ref  categoryInfo, ref  catePriceRangeList, ref  cateAAndVList, ref  categoryList, ref  brandInfo, ref brandList, ref  totalCount, ref  productList);
        }

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
        public static void SearchStoreProducts(int pageSize, int pageNumber, string word, int storeId, int storeCid, int startPrice, int endPrice, int sortColumn, int sortDirection, ref int totalCount, ref List<PartProductInfo> productList)
        {
            _isearchstrategy.SearchStoreProducts(pageSize, pageNumber, word, storeId, storeCid, startPrice, endPrice, sortColumn, sortDirection, ref totalCount, ref productList);
        }
    }
}
