using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Collections.Generic;

using BrnMall.Core;
using BrnMall.Services;

using PanGu;
using PanGu.Match;

namespace BrnMall.SearchStrategy.SqlServer
{
    /// <summary>
    /// 基于SqlServer的搜索策略
    /// </summary>
    public partial class SearchStrategy : ISearchStrategy
    {
        private readonly string _configfilepath = "/App_Data/pangu/pangu.config";//盘古分词配置信息文件路径

        private Segment _segment = null;

        public SearchStrategy()
        {
            Segment.Init(IOHelper.GetMapPath(_configfilepath));
            _segment = new Segment();
        }

        #region 分词相关

        /// <summary>
        /// 分词
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns></returns>
        public List<string> Analyse(string text)
        {
            List<string> keywordList = new List<string>();
            foreach (WordInfo wordInfo in _segment.DoSegment(text))
                keywordList.Add(wordInfo.Word);
            return keywordList;
        }

        #endregion

        #region 索引相关

        /// <summary>
        /// 创建商品关键词
        /// </summary>
        /// <param name="productKeywordInfo">商品关键词信息</param>
        public void CreateProductKeyword(ProductKeywordInfo productKeywordInfo)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@keyword", SqlDbType.NChar,40,productKeywordInfo.Keyword),
                                        GenerateInParam("@pid", SqlDbType.Int,4,productKeywordInfo.Pid),
                                        GenerateInParam("@relevancy", SqlDbType.Int,4,productKeywordInfo.Relevancy)
                                    };
            string commandText = string.Format("INSERT INTO [{0}productkeywords]([keyword],[pid],[relevancy]) VALUES(@keyword,@pid,@relevancy)",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 创建商品关键词
        /// </summary>
        /// <param name="pid">商品id</param>
        /// <param name="text">文本</param>
        /// <param name="relevancy">相关性</param>
        public void CreateProductKeyword(int pid, string text, int relevancy)
        {
            List<string> keywordList = Analyse(text);
            foreach (string keyword in keywordList)
            {
                ProductKeywordInfo productKeywordInfo = new ProductKeywordInfo()
                {
                    Keyword = keyword,
                    Pid = pid,
                    Relevancy = relevancy
                };
                CreateProductKeyword(productKeywordInfo);
            }
        }

        /// <summary>
        /// 更新商品关键词
        /// </summary>
        /// <param name="productKeywordInfo">商品关键词信息</param>
        public void UpdateProductKeyword(ProductKeywordInfo productKeywordInfo)
        {
            DeleteProductKeyword(productKeywordInfo.Pid, productKeywordInfo.Keyword);
            CreateProductKeyword(productKeywordInfo);
        }

        /// <summary>
        /// 更新商品关键词
        /// </summary>
        /// <param name="pid">商品id</param>
        /// <param name="text">文本</param>
        /// <param name="relevancy">相关性</param>
        public void UpdateProductKeyword(int pid, string text, int relevancy)
        {
            ResetProductKeyword(pid, text, relevancy);
        }

        /// <summary>
        /// 删除商品关键词
        /// </summary>
        /// <param name="pid">商品id</param>
        public void DeleteProductKeyword(int pid)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@pid", SqlDbType.Int, 4, pid)
                                   };
            string commandText = string.Format("DELETE FROM [{0}productkeywords] WHERE [pid]=@pid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 删除商品关键词
        /// </summary>
        /// <param name="pid">商品id</param>
        /// <param name="keyword">商品关键词</param>
        public void DeleteProductKeyword(int pid, string keyword)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@keyword", SqlDbType.NChar, 40, keyword),
                                    GenerateInParam("@pid", SqlDbType.Int, 4, pid)
                                   };
            string commandText = string.Format("DELETE FROM [{0}productkeywords] WHERE [keyword]=@keyword AND [pid]=@pid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 重置商品关键词
        /// </summary>
        /// <param name="pid">商品id</param>
        /// <param name="text">文本</param>
        /// <param name="relevancy">相关性</param>
        public void ResetProductKeyword(int pid, string text, int relevancy)
        {
            DeleteProductKeyword(pid);
            CreateProductKeyword(pid, text, relevancy);
        }

        /// <summary>
        /// 获得商品关键词列表
        /// </summary>
        /// <param name="pid">商品id</param>
        /// <returns></returns>
        public List<ProductKeywordInfo> GetProductKeywordList(int pid)
        {
            List<ProductKeywordInfo> productKeywordList = new List<ProductKeywordInfo>();

            DbParameter[] parms = {
                                    GenerateInParam("@pid", SqlDbType.Int, 4, pid)
                                   };
            string commandText = string.Format("SELECT [keyword],[pid],[relevancy] FROM [{0}productkeywords] WHERE [pid]=@pid",
                                                RDBSHelper.RDBSTablePre);
            IDataReader reader = RDBSHelper.ExecuteReader(CommandType.Text, commandText, parms);
            while (reader.Read())
            {
                ProductKeywordInfo productKeywordInfo = BuildProductKeyWordFromReader(reader);
                productKeywordList.Add(productKeywordInfo);
            }
            reader.Close();

            return productKeywordList;
        }

        /// <summary>
        /// 是否存在商品关键词
        /// </summary>
        /// <param name="pid">商品id</param>
        /// <param name="keyword">关键词</param>
        /// <returns></returns>
        public bool IsExistProductKeyword(int pid, string keyword)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@keyword", SqlDbType.NChar, 40, keyword),
                                    GenerateInParam("@pid", SqlDbType.Int, 4, pid)
                                   };
            string commandText = string.Format("SELECT [pid] FROM [{0}productkeywords] WHERE [keyword]=@keyword AND [pid]=@pid ",
                                                RDBSHelper.RDBSTablePre);
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms)) > 0;
        }

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
        public void SearchMallProducts(int pageSize, int pageNumber, string word, int cateId, int brandId, int filterPrice, List<int> attrValueIdList, int onlyStock, int sortColumn, int sortDirection, ref CategoryInfo categoryInfo, ref string[] catePriceRangeList, ref List<KeyValuePair<AttributeInfo, List<AttributeValueInfo>>> cateAAndVList, ref List<CategoryInfo> categoryList, ref BrandInfo brandInfo, ref List<BrandInfo> brandList, ref int totalCount, ref List<StoreProductInfo> productList)
        {
            //针对用户搜索词进行分词
            List<string> keywordList = Analyse(word);

            //构建关键词查询sql
            string keywordSql = BuildKeywordSql(keywordList);

            #region 设置分类列表

            categoryList = new List<CategoryInfo>();

            StringBuilder commandText3 = new StringBuilder();
            commandText3.AppendFormat("SELECT [cateid],[displayorder],[name],[pricerange],[parentid],[layer],[haschild],[path] FROM [{0}categories] WHERE [cateid] IN (SELECT DISTINCT [cateid] FROM [{0}products] WHERE [state]=0 AND [pid] IN (SELECT [pid] FROM [{0}productkeywords] WHERE [keyword] IN ({1})))",
                                       RDBSHelper.RDBSTablePre,
                                       keywordSql);
            IDataReader reader2 = RDBSHelper.ExecuteReader(CommandType.Text, commandText3.ToString());
            while (reader2.Read())
            {
                CategoryInfo info = new CategoryInfo();

                info.CateId = TypeHelper.ObjectToInt(reader2["cateid"]);
                info.DisplayOrder = TypeHelper.ObjectToInt(reader2["displayorder"]);
                info.Name = reader2["name"].ToString();
                info.PriceRange = reader2["pricerange"].ToString();
                info.ParentId = TypeHelper.ObjectToInt(reader2["parentid"]);
                info.Layer = TypeHelper.ObjectToInt(reader2["layer"]);
                info.HasChild = TypeHelper.ObjectToInt(reader2["haschild"]);
                info.Path = reader2["path"].ToString();

                categoryList.Add(info);
            }
            reader2.Close();

            #endregion

            //当关联分类列表中一个分类也不存在时，认为商品也不存在，直接返回
            if (categoryList.Count < 1)
                return;

            if (cateId > 0)
            {
                foreach (CategoryInfo info in categoryList)
                {
                    if (info.CateId == cateId)
                    {
                        categoryInfo = info;
                        break;
                    }
                }

                //当筛选了分类，但是分类不在分类列表中，认为商品也不存在，直接返回
                if (categoryInfo == null)
                    return;
            }

            if (categoryInfo != null)
            {
                cateAAndVList = Categories.GetCategoryFilterAAndVList(categoryInfo.CateId);
                catePriceRangeList = StringHelper.SplitString(categoryInfo.PriceRange, "\r\n");
            }

            #region 设置品牌列表

            brandList = new List<BrandInfo>();

            StringBuilder commandText4 = new StringBuilder();
            commandText4.AppendFormat("SELECT [brandid],[displayorder],[name],[logo] FROM [{0}brands] WHERE [brandid] IN (SELECT DISTINCT [brandid] FROM [{0}products] WHERE [state]=0 {1} AND [pid] IN (SELECT [pid] FROM [{0}productkeywords] WHERE [keyword] IN ({2})))",
                                       RDBSHelper.RDBSTablePre,
                                       categoryInfo == null ? "" : "AND [cateid]=" + categoryInfo.CateId,
                                       keywordSql);
            IDataReader reader3 = RDBSHelper.ExecuteReader(CommandType.Text, commandText4.ToString());
            while (reader3.Read())
            {
                BrandInfo info = new BrandInfo();

                info.BrandId = TypeHelper.ObjectToInt(reader3["brandid"]);
                info.DisplayOrder = TypeHelper.ObjectToInt(reader3["displayorder"]);
                info.Name = reader3["name"].ToString();
                info.Logo = reader3["logo"].ToString();

                brandList.Add(info);
            }
            reader3.Close();

            #endregion

            //当品牌列表中一个品牌也不存在时，认为商品也不存在，直接返回
            if (brandList.Count < 1)
                return;

            //当筛选了品牌，但是品牌不在品牌列表中，认为商品也不存在，直接返回
            if (brandId > 0)
            {
                bool flag = true;
                foreach (BrandInfo info in brandList)
                {
                    if (info.BrandId == brandId)
                    {
                        brandInfo = info;
                        flag = false;
                        break;
                    }
                }
                if (flag)
                    return;
            }

            #region 获取商品列表

            StringBuilder commandText1 = new StringBuilder();
            if (pageNumber == 1)
            {
                commandText1.AppendFormat("SELECT TOP {1} [p].[pid],[p].[psn],[p].[cateid],[p].[brandid],[p].[storeid],[p].[storecid],[p].[storestid],[p].[skugid],[p].[name],[p].[shopprice],[p].[marketprice],[p].[costprice],[p].[state],[p].[isbest],[p].[ishot],[p].[isnew],[p].[displayorder],[p].[weight],[p].[showimg],[p].[salecount],[p].[visitcount],[p].[reviewcount],[p].[star1],[p].[star2],[p].[star3],[p].[star4],[p].[star5],[p].[addtime],[s].[name] AS [storename] FROM [{0}products] AS [p]", RDBSHelper.RDBSTablePre, pageSize);

                if (onlyStock == 1)
                    commandText1.AppendFormat(" LEFT JOIN [{0}productstocks] AS [ps] ON [p].[pid]=[ps].[pid]", RDBSHelper.RDBSTablePre);

                commandText1.AppendFormat(" LEFT JOIN [{0}productkeywords] AS [pk] ON [p].[pid]=[pk].[pid]", RDBSHelper.RDBSTablePre);

                commandText1.AppendFormat(" LEFT JOIN [{0}stores] AS [s] ON [p].[storeid]=[s].[storeid]", RDBSHelper.RDBSTablePre);

                commandText1.Append(" WHERE [p].[state]=0");

                if (categoryInfo != null)
                    commandText1.AppendFormat(" AND [p].[cateid]={0}", categoryInfo.CateId);

                if (brandId > 0)
                    commandText1.AppendFormat(" AND [p].[brandid]={0}", brandId);

                if (categoryInfo != null && filterPrice > 0 && filterPrice <= catePriceRangeList.Length)
                {
                    string[] priceRange = StringHelper.SplitString(catePriceRangeList[filterPrice - 1], "-");
                    if (priceRange.Length == 1)
                        commandText1.AppendFormat(" AND [p].[shopprice]>='{0}'", priceRange[0]);
                    else if (priceRange.Length == 2)
                        commandText1.AppendFormat(" AND [p].[shopprice]>='{0}' AND [p].[shopprice]<'{1}'", priceRange[0], priceRange[1]);
                }

                if (categoryInfo != null && attrValueIdList.Count > 0 && attrValueIdList.Count <= cateAAndVList.Count)
                {
                    commandText1.Append(" AND [p].[pid] IN (SELECT [pa1].[pid] FROM");
                    for (int i = 0; i < attrValueIdList.Count; i++)
                    {
                        if (i == 0)
                            commandText1.AppendFormat(" (SELECT [pid] FROM [{0}productattributes] WHERE [attrvalueid]={1}) AS [pa1]", RDBSHelper.RDBSTablePre, attrValueIdList[i]);
                        else
                            commandText1.AppendFormat(" INNER JOIN (SELECT [pid] FROM [{0}productattributes] WHERE [attrvalueid]={1}) AS [pa{2}] ON [pa{2}].[pid]=[pa{3}].[pid]", RDBSHelper.RDBSTablePre, attrValueIdList[i], i + 1, i);
                    }
                    commandText1.Append(")");
                }

                if (onlyStock == 1)
                    commandText1.Append(" AND [ps].[number]>0");

                commandText1.AppendFormat(" AND [pk].[keyword] IN ({0})", keywordSql);

                commandText1.Append(" AND [s].[state]=0");

                commandText1.Append(" ORDER BY ");
                switch (sortColumn)
                {
                    case 0:
                        commandText1.Append("[pk].[relevancy]");
                        break;
                    case 1:
                        commandText1.Append("[p].[salecount]");
                        break;
                    case 2:
                        commandText1.Append("[p].[shopprice]");
                        break;
                    case 3:
                        commandText1.Append("[p].[reviewcount]");
                        break;
                    case 4:
                        commandText1.Append("[p].[addtime]");
                        break;
                    case 5:
                        commandText1.Append("[p].[visitcount]");
                        break;
                    default:
                        commandText1.Append("[pk].[relevancy]");
                        break;
                }
                switch (sortDirection)
                {
                    case 0:
                        commandText1.Append(" DESC");
                        break;
                    case 1:
                        commandText1.Append(" ASC");
                        break;
                    default:
                        commandText1.Append(" DESC");
                        break;
                }
                commandText1.Append(",[p].[pid] DESC");
            }
            else
            {
                commandText1.Append("SELECT [pid],[psn],[cateid],[brandid],[storeid],[storecid],[storestid],[skugid],[name],[shopprice],[marketprice],[costprice],[state],[isbest],[ishot],[isnew],[displayorder],[weight],[showimg],[salecount],[visitcount],[reviewcount],[star1],[star2],[star3],[star4],[star5],[addtime],[storename] FROM");
                commandText1.Append(" (SELECT ROW_NUMBER() OVER (ORDER BY ");
                switch (sortColumn)
                {
                    case 0:
                        commandText1.Append("[pk].[relevancy]");
                        break;
                    case 1:
                        commandText1.Append("[p].[salecount]");
                        break;
                    case 2:
                        commandText1.Append("[p].[shopprice]");
                        break;
                    case 3:
                        commandText1.Append("[p].[reviewcount]");
                        break;
                    case 4:
                        commandText1.Append("[p].[addtime]");
                        break;
                    case 5:
                        commandText1.Append("[p].[visitcount]");
                        break;
                    default:
                        commandText1.Append("[pk].[relevancy]");
                        break;
                }
                switch (sortDirection)
                {
                    case 0:
                        commandText1.Append(" DESC");
                        break;
                    case 1:
                        commandText1.Append(" ASC");
                        break;
                    default:
                        commandText1.Append(" DESC");
                        break;
                }
                commandText1.Append(",[p].[pid] DESC");
                commandText1.AppendFormat(") AS [rowid],[p].[pid],[p].[psn],[p].[cateid],[p].[brandid],[p].[storeid],[p].[storecid],[p].[storestid],[p].[skugid],[p].[name],[p].[shopprice],[p].[marketprice],[p].[costprice],[p].[state],[p].[isbest],[p].[ishot],[p].[isnew],[p].[displayorder],[p].[weight],[p].[showimg],[p].[salecount],[p].[visitcount],[p].[reviewcount],[p].[star1],[p].[star2],[p].[star3],[p].[star4],[p].[star5],[p].[addtime],[s].[name] AS [storename] FROM [{0}products] AS [p]", RDBSHelper.RDBSTablePre);

                if (onlyStock == 1)
                    commandText1.AppendFormat(" LEFT JOIN [{0}productstocks] AS [ps] ON [p].[pid]=[ps].[pid]", RDBSHelper.RDBSTablePre);

                commandText1.AppendFormat(" LEFT JOIN [{0}productkeywords] AS [pk] ON [p].[pid]=[pk].[pid]", RDBSHelper.RDBSTablePre);

                commandText1.AppendFormat(" LEFT JOIN [{0}stores] AS [s] ON [p].[storeid]=[s].[storeid]", RDBSHelper.RDBSTablePre);

                commandText1.Append(" WHERE [p].[state]=0");

                if (categoryInfo != null)
                    commandText1.AppendFormat(" AND [p].[cateid]={0}", categoryInfo.CateId);

                if (brandId > 0)
                    commandText1.AppendFormat(" AND [p].[brandid]={0}", brandId);

                if (categoryInfo != null && filterPrice > 0 && filterPrice <= catePriceRangeList.Length)
                {
                    string[] priceRange = StringHelper.SplitString(catePriceRangeList[filterPrice - 1], "-");
                    if (priceRange.Length == 1)
                        commandText1.AppendFormat(" AND [p].[shopprice]>='{0}'", priceRange[0]);
                    else if (priceRange.Length == 2)
                        commandText1.AppendFormat(" AND [p].[shopprice]>='{0}' AND [p].[shopprice]<'{1}'", priceRange[0], priceRange[1]);
                }

                if (categoryInfo != null && attrValueIdList.Count > 0 && attrValueIdList.Count == cateAAndVList.Count)
                {
                    commandText1.Append(" AND [p].[pid] IN (SELECT [pa1].[pid] FROM");
                    for (int i = 0; i < attrValueIdList.Count; i++)
                    {
                        if (i == 0)
                            commandText1.AppendFormat(" (SELECT [pid] FROM [{0}productattributes] WHERE [attrvalueid]={1}) AS [pa1]", RDBSHelper.RDBSTablePre, attrValueIdList[i]);
                        else
                            commandText1.AppendFormat(" INNER JOIN (SELECT [pid] FROM [{0}productattributes] WHERE [attrvalueid]={1}) AS [pa{2}] ON [pa{2}].[pid]=[pa{3}].[pid]", RDBSHelper.RDBSTablePre, attrValueIdList[i], i + 1, i);
                    }
                    commandText1.Append(")");
                }

                if (onlyStock == 1)
                    commandText1.Append(" AND [ps].[number]>0");

                commandText1.AppendFormat(" AND [pk].[keyword] IN ({0}) ", keywordSql);

                commandText1.Append(" AND [s].[state]=0");

                commandText1.Append(") AS [temp]");
                commandText1.AppendFormat(" WHERE [rowid] BETWEEN {0} AND {1}", pageSize * (pageNumber - 1) + 1, pageSize * pageNumber);

            }

            productList = new List<StoreProductInfo>();
            IDataReader reader1 = RDBSHelper.ExecuteReader(CommandType.Text, commandText1.ToString());
            while (reader1.Read())
            {
                StoreProductInfo storeProductInfo = new StoreProductInfo();

                storeProductInfo.Pid = TypeHelper.ObjectToInt(reader1["pid"]);
                storeProductInfo.PSN = reader1["psn"].ToString();
                storeProductInfo.CateId = TypeHelper.ObjectToInt(reader1["cateid"]);
                storeProductInfo.BrandId = TypeHelper.ObjectToInt(reader1["brandid"]);
                storeProductInfo.StoreId = TypeHelper.ObjectToInt(reader1["storeid"]);
                storeProductInfo.StoreCid = TypeHelper.ObjectToInt(reader1["storecid"]);
                storeProductInfo.StoreSTid = TypeHelper.ObjectToInt(reader1["storestid"]);
                storeProductInfo.SKUGid = TypeHelper.ObjectToInt(reader1["skugid"]);
                storeProductInfo.Name = reader1["name"].ToString();
                storeProductInfo.ShopPrice = TypeHelper.ObjectToDecimal(reader1["shopprice"]);
                storeProductInfo.MarketPrice = TypeHelper.ObjectToDecimal(reader1["marketprice"]);
                storeProductInfo.CostPrice = TypeHelper.ObjectToDecimal(reader1["costprice"]);
                storeProductInfo.State = TypeHelper.ObjectToInt(reader1["state"]);
                storeProductInfo.IsBest = TypeHelper.ObjectToInt(reader1["isbest"]);
                storeProductInfo.IsHot = TypeHelper.ObjectToInt(reader1["ishot"]);
                storeProductInfo.IsNew = TypeHelper.ObjectToInt(reader1["isnew"]);
                storeProductInfo.DisplayOrder = TypeHelper.ObjectToInt(reader1["displayorder"]);
                storeProductInfo.Weight = TypeHelper.ObjectToInt(reader1["weight"]);
                storeProductInfo.ShowImg = reader1["showimg"].ToString();
                storeProductInfo.SaleCount = TypeHelper.ObjectToInt(reader1["salecount"]);
                storeProductInfo.VisitCount = TypeHelper.ObjectToInt(reader1["visitcount"]);
                storeProductInfo.ReviewCount = TypeHelper.ObjectToInt(reader1["reviewcount"]);
                storeProductInfo.Star1 = TypeHelper.ObjectToInt(reader1["star1"]);
                storeProductInfo.Star2 = TypeHelper.ObjectToInt(reader1["star2"]);
                storeProductInfo.Star3 = TypeHelper.ObjectToInt(reader1["star3"]);
                storeProductInfo.Star4 = TypeHelper.ObjectToInt(reader1["star4"]);
                storeProductInfo.Star5 = TypeHelper.ObjectToInt(reader1["star5"]);
                storeProductInfo.AddTime = TypeHelper.ObjectToDateTime(reader1["addtime"]);
                storeProductInfo.StoreName = reader1["storename"].ToString();

                productList.Add(storeProductInfo);
            }
            reader1.Close();

            #endregion

            #region 设置商品总数量

            StringBuilder commandText2 = new StringBuilder();

            commandText2.AppendFormat("SELECT COUNT([p].[pid]) FROM [{0}products] AS [p]", RDBSHelper.RDBSTablePre);

            if (onlyStock == 1)
                commandText2.AppendFormat(" LEFT JOIN [{0}productstocks] AS [ps] ON [p].[pid]=[ps].[pid]", RDBSHelper.RDBSTablePre);

            commandText2.AppendFormat(" LEFT JOIN [{0}productkeywords] AS [pk] ON [p].[pid]=[pk].[pid]", RDBSHelper.RDBSTablePre);

            commandText2.AppendFormat(" LEFT JOIN [{0}stores] AS [s] ON [p].[storeid]=[s].[storeid]", RDBSHelper.RDBSTablePre);

            commandText2.Append(" WHERE [p].[state]=0");

            if (categoryInfo != null)
                commandText2.AppendFormat(" AND [p].[cateid]={0}", categoryInfo.CateId);

            if (brandId > 0)
                commandText2.AppendFormat(" AND [p].[brandid]={0}", brandId);

            if (categoryInfo != null && filterPrice > 0 && filterPrice <= catePriceRangeList.Length)
            {
                string[] priceRange = StringHelper.SplitString(catePriceRangeList[filterPrice - 1], "-");
                if (priceRange.Length == 1)
                    commandText2.AppendFormat(" AND [p].[shopprice]>='{0}'", priceRange[0]);
                else if (priceRange.Length == 2)
                    commandText2.AppendFormat(" AND [p].[shopprice]>='{0}' AND [p].[shopprice]<'{1}'", priceRange[0], priceRange[1]);
            }

            if (categoryInfo != null && attrValueIdList.Count > 0 && attrValueIdList.Count <= cateAAndVList.Count)
            {
                commandText2.Append(" AND [p].[pid] IN (SELECT [pa1].[pid] FROM");
                for (int i = 0; i < attrValueIdList.Count; i++)
                {
                    if (i == 0)
                        commandText2.AppendFormat(" (SELECT [pid] FROM [{0}productattributes] WHERE [attrvalueid]={1}) AS [pa1]", RDBSHelper.RDBSTablePre, attrValueIdList[i]);
                    else
                        commandText2.AppendFormat(" INNER JOIN (SELECT [pid] FROM [{0}productattributes] WHERE [attrvalueid]={1}) AS [pa{2}] ON [pa{2}].[pid]=[pa{3}].[pid]", RDBSHelper.RDBSTablePre, attrValueIdList[i], i + 1, i);
                }
                commandText2.Append(")");
            }

            if (onlyStock == 1)
                commandText2.Append(" AND [ps].[number]>0");

            commandText2.AppendFormat(" AND [pk].[keyword] IN ({0}) ", keywordSql);

            commandText2.Append(" AND [s].[state]=0");

            totalCount = TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText2.ToString()));

            #endregion
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
        public void SearchStoreProducts(int pageSize, int pageNumber, string word, int storeId, int storeCid, int startPrice, int endPrice, int sortColumn, int sortDirection, ref int totalCount, ref List<PartProductInfo> productList)
        {
            //针对用户搜索词进行分词
            List<string> keywordList = Analyse(word);

            //构建关键词查询sql
            string keywordSql = BuildKeywordSql(keywordList);

            #region 获取商品列表

            StringBuilder commandText1 = new StringBuilder();

            if (pageNumber == 1)
            {
                commandText1.AppendFormat("SELECT TOP {1} [p].[pid],[p].[psn],[p].[cateid],[p].[brandid],[p].[storeid],[p].[storecid],[p].[storestid],[p].[skugid],[p].[name],[p].[shopprice],[p].[marketprice],[p].[costprice],[p].[state],[p].[isbest],[p].[ishot],[p].[isnew],[p].[displayorder],[p].[weight],[p].[showimg],[p].[salecount],[p].[visitcount],[p].[reviewcount],[p].[star1],[p].[star2],[p].[star3],[p].[star4],[p].[star5],[p].[addtime] FROM [{0}products] AS [p]", RDBSHelper.RDBSTablePre, pageSize);

                commandText1.AppendFormat(" LEFT JOIN [{0}productkeywords] AS [pk] ON [p].[pid]=[pk].[pid]", RDBSHelper.RDBSTablePre);

                commandText1.AppendFormat(" WHERE [p].[storeid]={0}", storeId);

                if (storeCid > 0)
                    commandText1.AppendFormat(" AND [p].[storecid]={0}", storeCid);

                if (startPrice > 0)
                    commandText1.AppendFormat(" AND [p].[shopprice]>={0}", startPrice);
                if (endPrice > 0)
                    commandText1.AppendFormat(" AND [p].[shopprice]<={0}", endPrice);

                commandText1.Append(" AND [p].[state]=0");

                commandText1.AppendFormat(" AND [pk].[keyword] IN ({0}) ", keywordSql);

                commandText1.Append(" ORDER BY ");
                switch (sortColumn)
                {
                    case 0:
                        commandText1.Append("[pk].[relevancy]");
                        break;
                    case 1:
                        commandText1.Append("[p].[salecount]");
                        break;
                    case 2:
                        commandText1.Append("[p].[shopprice]");
                        break;
                    case 3:
                        commandText1.Append("[p].[reviewcount]");
                        break;
                    case 4:
                        commandText1.Append("[p].[addtime]");
                        break;
                    case 5:
                        commandText1.Append("[p].[visitcount]");
                        break;
                    default:
                        commandText1.Append("[pk].[relevancy]");
                        break;
                }
                switch (sortDirection)
                {
                    case 0:
                        commandText1.Append(" DESC");
                        break;
                    case 1:
                        commandText1.Append(" ASC");
                        break;
                    default:
                        commandText1.Append(" DESC");
                        break;
                }
                commandText1.Append(",[p].[pid] DESC");
            }
            else
            {
                commandText1.Append("SELECT [pid],[psn],[cateid],[brandid],[storeid],[storecid],[storestid],[skugid],[name],[shopprice],[marketprice],[costprice],[state],[isbest],[ishot],[isnew],[displayorder],[weight],[showimg],[salecount],[visitcount],[reviewcount],[star1],[star2],[star3],[star4],[star5],[addtime] FROM");
                commandText1.Append(" (SELECT ROW_NUMBER() OVER (ORDER BY ");
                switch (sortColumn)
                {
                    case 0:
                        commandText1.Append("[pk].[relevancy]");
                        break;
                    case 1:
                        commandText1.Append("[p].[salecount]");
                        break;
                    case 2:
                        commandText1.Append("[p].[shopprice]");
                        break;
                    case 3:
                        commandText1.Append("[p].[reviewcount]");
                        break;
                    case 4:
                        commandText1.Append("[p].[addtime]");
                        break;
                    case 5:
                        commandText1.Append("[p].[visitcount]");
                        break;
                    default:
                        commandText1.Append("[pk].[relevancy]");
                        break;
                }
                switch (sortDirection)
                {
                    case 0:
                        commandText1.Append(" DESC");
                        break;
                    case 1:
                        commandText1.Append(" ASC");
                        break;
                    default:
                        commandText1.Append(" DESC");
                        break;
                }
                commandText1.Append(",[p].[pid] DESC");
                commandText1.AppendFormat(") AS [rowid],[p].[pid],[p].[psn],[p].[cateid],[p].[brandid],[p].[storeid],[p].[storecid],[p].[storestid],[p].[skugid],[p].[name],[p].[shopprice],[p].[marketprice],[p].[costprice],[p].[state],[p].[isbest],[p].[ishot],[p].[isnew],[p].[displayorder],[p].[weight],[p].[showimg],[p].[salecount],[p].[visitcount],[p].[reviewcount],[p].[star1],[p].[star2],[p].[star3],[p].[star4],[p].[star5],[p].[addtime] FROM [{0}products] AS [p]", RDBSHelper.RDBSTablePre);

                commandText1.AppendFormat(" LEFT JOIN [{0}productkeywords] AS [pk] ON [p].[pid]=[pk].[pid]", RDBSHelper.RDBSTablePre);

                commandText1.AppendFormat(" WHERE [p].[storeid]={0}", storeId);

                if (storeCid > 0)
                    commandText1.AppendFormat(" AND [p].[storecid]={0}", storeCid);

                if (startPrice > 0)
                    commandText1.AppendFormat(" AND [p].[shopprice]>={0}", startPrice);
                if (endPrice > 0)
                    commandText1.AppendFormat(" AND [p].[shopprice]<={0}", endPrice);

                commandText1.Append(" AND [p].[state]=0");

                commandText1.AppendFormat(" AND [pk].[keyword] IN ({0}) ", keywordSql);

                commandText1.Append(") AS [temp]");
                commandText1.AppendFormat(" WHERE [rowid] BETWEEN {0} AND {1}", pageSize * (pageNumber - 1) + 1, pageSize * pageNumber);
            }

            productList = new List<PartProductInfo>();
            IDataReader reader1 = RDBSHelper.ExecuteReader(CommandType.Text, commandText1.ToString());
            while (reader1.Read())
            {
                PartProductInfo partProductInfo = new PartProductInfo();

                partProductInfo.Pid = TypeHelper.ObjectToInt(reader1["pid"]);
                partProductInfo.PSN = reader1["psn"].ToString();
                partProductInfo.CateId = TypeHelper.ObjectToInt(reader1["cateid"]);
                partProductInfo.BrandId = TypeHelper.ObjectToInt(reader1["brandid"]);
                partProductInfo.StoreId = TypeHelper.ObjectToInt(reader1["storeid"]);
                partProductInfo.StoreCid = TypeHelper.ObjectToInt(reader1["storecid"]);
                partProductInfo.StoreSTid = TypeHelper.ObjectToInt(reader1["storestid"]);
                partProductInfo.SKUGid = TypeHelper.ObjectToInt(reader1["skugid"]);
                partProductInfo.Name = reader1["name"].ToString();
                partProductInfo.ShopPrice = TypeHelper.ObjectToDecimal(reader1["shopprice"]);
                partProductInfo.MarketPrice = TypeHelper.ObjectToDecimal(reader1["marketprice"]);
                partProductInfo.CostPrice = TypeHelper.ObjectToDecimal(reader1["costprice"]);
                partProductInfo.State = TypeHelper.ObjectToInt(reader1["state"]);
                partProductInfo.IsBest = TypeHelper.ObjectToInt(reader1["isbest"]);
                partProductInfo.IsHot = TypeHelper.ObjectToInt(reader1["ishot"]);
                partProductInfo.IsNew = TypeHelper.ObjectToInt(reader1["isnew"]);
                partProductInfo.DisplayOrder = TypeHelper.ObjectToInt(reader1["displayorder"]);
                partProductInfo.Weight = TypeHelper.ObjectToInt(reader1["weight"]);
                partProductInfo.ShowImg = reader1["showimg"].ToString();
                partProductInfo.SaleCount = TypeHelper.ObjectToInt(reader1["salecount"]);
                partProductInfo.VisitCount = TypeHelper.ObjectToInt(reader1["visitcount"]);
                partProductInfo.ReviewCount = TypeHelper.ObjectToInt(reader1["reviewcount"]);
                partProductInfo.Star1 = TypeHelper.ObjectToInt(reader1["star1"]);
                partProductInfo.Star2 = TypeHelper.ObjectToInt(reader1["star2"]);
                partProductInfo.Star3 = TypeHelper.ObjectToInt(reader1["star3"]);
                partProductInfo.Star4 = TypeHelper.ObjectToInt(reader1["star4"]);
                partProductInfo.Star5 = TypeHelper.ObjectToInt(reader1["star5"]);
                partProductInfo.AddTime = TypeHelper.ObjectToDateTime(reader1["addtime"]);

                productList.Add(partProductInfo);
            }
            reader1.Close();

            #endregion

            #region 设置商品总数量

            StringBuilder commandText2 = new StringBuilder();

            commandText2.AppendFormat("SELECT COUNT([p].[pid]) FROM [{0}products] AS [p]", RDBSHelper.RDBSTablePre);

            commandText2.AppendFormat(" LEFT JOIN [{0}productkeywords] AS [pk] ON [p].[pid]=[pk].[pid]", RDBSHelper.RDBSTablePre);

            commandText2.AppendFormat(" WHERE [p].[storeid]={0}", storeId);

            if (storeCid > 0)
                commandText2.AppendFormat(" AND [p].[storecid]={0}", storeCid);

            if (startPrice > 0)
                commandText2.AppendFormat(" AND [p].[shopprice]>={0}", startPrice);
            if (endPrice > 0)
                commandText2.AppendFormat(" AND [p].[shopprice]<={0}", endPrice);

            commandText2.Append(" AND [p].[state]=0");

            commandText2.AppendFormat(" AND [pk].[keyword] IN ({0}) ", keywordSql);

            totalCount = TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText2.ToString()));

            #endregion
        }

        #endregion

        #region  辅助方法

        /// <summary>
        /// 生成输入参数
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="sqlDbType">参数类型</param>
        /// <param name="size">类型大小</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        private DbParameter GenerateInParam(string paramName, SqlDbType sqlDbType, int size, object value)
        {
            SqlParameter param = new SqlParameter(paramName, sqlDbType, size);
            param.Direction = ParameterDirection.Input;
            if (value != null)
                param.Value = value;
            return param;
        }

        /// <summary>
        /// 从IDataReader创建ProductKeywordInfo
        /// </summary>
        private ProductKeywordInfo BuildProductKeyWordFromReader(IDataReader reader)
        {
            ProductKeywordInfo productKeywordInfo = new ProductKeywordInfo();

            productKeywordInfo.Keyword = reader["keyword"].ToString();
            productKeywordInfo.Pid = TypeHelper.ObjectToInt(reader["pid"]);
            productKeywordInfo.Relevancy = TypeHelper.ObjectToInt(reader["relevancy"]);

            return productKeywordInfo;
        }

        /// <summary>
        /// 构建关键词查询sql
        /// </summary>
        /// <param name="keywordList">分词后的关键词列表</param>
        /// <returns></returns>
        private string BuildKeywordSql(List<string> keywordList)
        {
            StringBuilder sql = new StringBuilder();
            foreach (string keyword in keywordList)
            {
                if (SecureHelper.IsSafeSqlString(keyword))
                    sql.AppendFormat("'{0}',", keyword);
            }
            if (sql.Length > 0)
                sql.Remove(sql.Length - 1, 1);
            return sql.ToString();
        }

        #endregion
    }
}
