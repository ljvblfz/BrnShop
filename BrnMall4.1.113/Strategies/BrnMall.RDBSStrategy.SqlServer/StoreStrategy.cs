using System;
using System.Text;
using System.Data;
using System.Data.Common;

using BrnMall.Core;

namespace BrnMall.RDBSStrategy.SqlServer
{
    /// <summary>
    /// SqlServer策略之店铺分部类
    /// </summary>
    public partial class RDBSStrategy : IRDBSStrategy
    {
        #region 店铺行业

        /// <summary>
        /// 获得店铺行业列表
        /// </summary>
        public IDataReader GetStoreIndustryList()
        {
            string commandText = string.Format("SELECT {1} FROM [{0}storeindustries] ORDER BY [displayorder] DESC,[storeiid] DESC",
                                                RDBSHelper.RDBSTablePre,
                                                TableFields.STORE_INDUSTRIES);
            return RDBSHelper.ExecuteReader(CommandType.Text, commandText);
        }

        /// <summary>
        /// 创建店铺行业
        /// </summary>
        public void CreateStoreIndustry(StoreIndustryInfo storeIndustryInfo)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@title",SqlDbType.NChar, 150, storeIndustryInfo.Title),
                                    GenerateInParam("@displayorder",SqlDbType.Int,4,storeIndustryInfo.DisplayOrder)
                                   };
            string commandText = String.Format("INSERT INTO [{0}storeindustries] ([title],[displayorder]) VALUES(@title,@displayorder)",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 更新店铺行业
        /// </summary>
        public void UpdateStoreIndustry(StoreIndustryInfo storeIndustryInfo)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@title",SqlDbType.NChar, 150, storeIndustryInfo.Title),
                                    GenerateInParam("@displayorder",SqlDbType.Int,4,storeIndustryInfo.DisplayOrder),
                                    GenerateInParam("@storeiid", SqlDbType.SmallInt, 2, storeIndustryInfo.StoreIid)
                                   };
            string commandText = String.Format("UPDATE [{0}storeindustries] SET [title]=@title,[displayorder]=@displayorder WHERE [storeiid]=@storeiid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 删除店铺行业
        /// </summary>
        /// <param name="storeIid">店铺行业id</param>
        public void DeleteStoreIndustryById(int storeIid)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@storeiid", SqlDbType.SmallInt, 2, storeIid)
                                   };
            string commandText = String.Format("DELETE FROM [{0}storeindustries] WHERE [storeiid]=@storeiid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        #endregion

        #region 店铺等级

        /// <summary>
        /// 获得店铺等级列表
        /// </summary>
        /// <returns></returns>
        public IDataReader GetStoreRankList()
        {
            string commandText = string.Format("SELECT {1} FROM [{0}storeranks]",
                                                RDBSHelper.RDBSTablePre,
                                                TableFields.STORE_RANKS);
            return RDBSHelper.ExecuteReader(CommandType.Text, commandText);
        }

        /// <summary>
        /// 创建店铺等级
        /// </summary>
        public void CreateStoreRank(StoreRankInfo storeRankInfo)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@title", SqlDbType.NChar,50,storeRankInfo.Title),
                                        GenerateInParam("@avatar", SqlDbType.Char,50,storeRankInfo.Avatar),
                                        GenerateInParam("@honestieslower", SqlDbType.Int, 4, storeRankInfo.HonestiesLower),
                                        GenerateInParam("@honestiesupper", SqlDbType.Int,4,storeRankInfo.HonestiesUpper),
                                        GenerateInParam("@productcount", SqlDbType.Int,4,storeRankInfo.ProductCount)
                                    };
            string commandText = string.Format("INSERT INTO [{0}storeranks]([title],[avatar],[honestieslower],[honestiesupper],[productcount]) VALUES(@title,@avatar,@honestieslower,@honestiesupper,@productcount)",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 删除店铺等级
        /// </summary>
        /// <param name="storeRid">店铺等级id</param>
        public void DeleteStoreRankById(int storeRid)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@storerid", SqlDbType.SmallInt, 2, storeRid)    
                                    };
            string commandText = string.Format("DELETE FROM [{0}storeranks] WHERE [storerid]=@storerid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 更新店铺等级
        /// </summary>
        public void UpdateStoreRank(StoreRankInfo storeRankInfo)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@title", SqlDbType.NChar,50,storeRankInfo.Title),
                                        GenerateInParam("@avatar", SqlDbType.Char,50,storeRankInfo.Avatar),
                                        GenerateInParam("@honestieslower", SqlDbType.Int, 4, storeRankInfo.HonestiesLower),
                                        GenerateInParam("@honestiesupper", SqlDbType.Int,4,storeRankInfo.HonestiesUpper),
                                        GenerateInParam("@productcount", SqlDbType.Int,4,storeRankInfo.ProductCount),
                                        GenerateInParam("@storerid", SqlDbType.SmallInt,2,storeRankInfo.StoreRid)
                                    };
            string commandText = string.Format("UPDATE [{0}storeranks] SET [title]=@title,[avatar]=@avatar,[honestieslower]=@honestieslower,[honestiesupper]=@honestiesupper,[productcount]=@productcount WHERE [storerid]=@storerid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        #endregion

        #region 店铺评价

        /// <summary>
        /// 创建店铺评价
        /// </summary>
        /// <param name="storeReviewInfo">店铺评价信息</param>
        public void CreateStoreReview(StoreReviewInfo storeReviewInfo)
        {
            DbParameter[] parms = {
	                                 GenerateInParam("@oid", SqlDbType.Int, 4, storeReviewInfo.Oid),
	                                 GenerateInParam("@storeid", SqlDbType.Int, 4, storeReviewInfo.StoreId),
	                                 GenerateInParam("@descriptionstar", SqlDbType.TinyInt, 1, storeReviewInfo.DescriptionStar),
	                                 GenerateInParam("@servicestar", SqlDbType.TinyInt, 1, storeReviewInfo.ServiceStar),
	                                 GenerateInParam("@shipstar", SqlDbType.TinyInt, 1, storeReviewInfo.ShipStar),
	                                 GenerateInParam("@uid", SqlDbType.Int, 4, storeReviewInfo.Uid),
	                                 GenerateInParam("@reviewtime", SqlDbType.DateTime, 8, storeReviewInfo.ReviewTime),
	                                 GenerateInParam("@ip", SqlDbType.VarChar, 15, storeReviewInfo.IP)
                                    };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}createstorereview", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        /// <summary>
        /// 获得店铺评价
        /// </summary>
        /// <param name="oid">订单id</param>
        public IDataReader GetStoreReviewByOid(int oid)
        {
            DbParameter[] parms = {
	                                 GenerateInParam("@oid", SqlDbType.Int, 4, oid)
                                    };
            return RDBSHelper.ExecuteReader(CommandType.StoredProcedure,
                                            string.Format("{0}getstorereviewbyoid", RDBSHelper.RDBSTablePre),
                                            parms);
        }

        /// <summary>
        /// 汇总店铺评价
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public DataTable SumStoreReview(int storeId, DateTime startTime, DateTime endTime)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@storeid", SqlDbType.Int, 4, storeId),
                                        GenerateInParam("@starttime", SqlDbType.DateTime, 8, startTime),   
                                        GenerateInParam("@endtime", SqlDbType.DateTime, 8, endTime)    
                                    };
            string commandText = string.Format(@"SELECT [descriptionstar] AS [star],SUM([descriptionstar]) AS [count] FROM [{0}storereviews] WHERE [storeid]=@storeid AND [reviewtime]>=@starttime AND [reviewtime]<=@endtime GROUP BY [descriptionstar] UNION ALL
                                                 SELECT [star]=0,[count]=0 UNION ALL
                                                 SELECT [servicestar] AS [star],SUM([servicestar]) AS [count] FROM [{0}storereviews] WHERE [storeid]=@storeid AND [reviewtime]>=@starttime AND [reviewtime]<=@endtime GROUP BY [servicestar] UNION ALL
                                                 SELECT [star]=0,[count]=0 UNION ALL
                                                 SELECT [shipstar] AS [star],SUM([shipstar]) AS [count] FROM [{0}storereviews] WHERE [storeid]=@storeid AND [reviewtime]>=@starttime AND [reviewtime]<=@endtime GROUP BY [shipstar]",
                                                RDBSHelper.RDBSTablePre);
            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
        }

        #endregion

        #region 店铺

        /// <summary>
        /// 创建店铺
        /// </summary>
        /// <param name="storeInfo">店铺信息</param>
        /// <returns>店铺id</returns>
        public int CreateStore(StoreInfo storeInfo)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@state", SqlDbType.TinyInt,1,storeInfo.State),
                                    GenerateInParam("@name", SqlDbType.NChar,60,storeInfo.Name),
                                    GenerateInParam("@regionid", SqlDbType.SmallInt,2,storeInfo.RegionId),
                                    GenerateInParam("@storerid", SqlDbType.SmallInt,2,storeInfo.StoreRid),
                                    GenerateInParam("@storeiid", SqlDbType.SmallInt,2,storeInfo.StoreIid),
                                    GenerateInParam("@logo", SqlDbType.NChar,100,storeInfo.Logo),
                                    GenerateInParam("@createtime", SqlDbType.DateTime,8,storeInfo.CreateTime),
                                    GenerateInParam("@mobile", SqlDbType.Char,15,storeInfo.Mobile),
                                    GenerateInParam("@phone", SqlDbType.Char,12,storeInfo.Phone),
                                    GenerateInParam("@qq", SqlDbType.Char,11,storeInfo.QQ),
                                    GenerateInParam("@ww", SqlDbType.Char,50,storeInfo.WW),
                                    GenerateInParam("@depoint", SqlDbType.Decimal,4,storeInfo.DePoint),
                                    GenerateInParam("@sepoint", SqlDbType.Decimal,4,storeInfo.SePoint),
                                    GenerateInParam("@shpoint", SqlDbType.Decimal,4,storeInfo.ShPoint),
                                    GenerateInParam("@honesties", SqlDbType.Int,4,storeInfo.Honesties),
                                    GenerateInParam("@stateendtime", SqlDbType.DateTime,8,storeInfo.StateEndTime),
                                    GenerateInParam("@theme", SqlDbType.Char,20,storeInfo.Theme),
                                    GenerateInParam("@banner", SqlDbType.NChar,100,storeInfo.Banner),
                                    GenerateInParam("@announcement", SqlDbType.NChar,200,storeInfo.Announcement),
                                    GenerateInParam("@description", SqlDbType.NChar,300,storeInfo.Description)
                                   };
            string commandText = string.Format("INSERT INTO [{0}stores]([state],[name],[regionid],[storerid],[storeiid],[logo],[createtime],[mobile],[phone],[qq],[ww],[depoint],[sepoint],[shpoint],[honesties],[stateendtime],[theme],[banner],[announcement],[description]) VALUES(@state,@name,@regionid,@storerid,@storeiid,@logo,@createtime,@mobile,@phone,@qq,@ww,@depoint,@sepoint,@shpoint,@honesties,@stateendtime,@theme,@banner,@announcement,@description);SELECT SCOPE_IDENTITY();",
                                                RDBSHelper.RDBSTablePre);
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms), -1);
        }

        /// <summary>
        /// 更新店铺
        /// </summary>
        /// <param name="storeInfo">店铺信息</param>
        public void UpdateStore(StoreInfo storeInfo)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@state", SqlDbType.TinyInt,1,storeInfo.State),
                                    GenerateInParam("@name", SqlDbType.NChar,60,storeInfo.Name),
                                    GenerateInParam("@regionid", SqlDbType.SmallInt,2,storeInfo.RegionId),
                                    GenerateInParam("@storerid", SqlDbType.SmallInt,2,storeInfo.StoreRid),
                                    GenerateInParam("@storeiid", SqlDbType.SmallInt,2,storeInfo.StoreIid),
                                    GenerateInParam("@logo", SqlDbType.NChar,100,storeInfo.Logo),
                                    GenerateInParam("@createtime", SqlDbType.DateTime,8,storeInfo.CreateTime),
                                    GenerateInParam("@mobile", SqlDbType.Char,15,storeInfo.Mobile),
                                    GenerateInParam("@phone", SqlDbType.Char,12,storeInfo.Phone),
                                    GenerateInParam("@qq", SqlDbType.Char,11,storeInfo.QQ),
                                    GenerateInParam("@ww", SqlDbType.Char,50,storeInfo.WW),
                                    GenerateInParam("@depoint", SqlDbType.Decimal,4,storeInfo.DePoint),
                                    GenerateInParam("@sepoint", SqlDbType.Decimal,4,storeInfo.SePoint),
                                    GenerateInParam("@shpoint", SqlDbType.Decimal,4,storeInfo.ShPoint),
                                    GenerateInParam("@honesties", SqlDbType.Int,4,storeInfo.Honesties),
                                    GenerateInParam("@stateendtime", SqlDbType.DateTime,8,storeInfo.StateEndTime),
                                    GenerateInParam("@theme", SqlDbType.Char,20,storeInfo.Theme),
                                    GenerateInParam("@banner", SqlDbType.NChar,100,storeInfo.Banner),
                                    GenerateInParam("@announcement", SqlDbType.NChar,200,storeInfo.Announcement),
                                    GenerateInParam("@description", SqlDbType.NChar,300,storeInfo.Description),
                                    GenerateInParam("@storeid", SqlDbType.Int,4,storeInfo.StoreId)
                                   };
            string commandText = string.Format("UPDATE [{0}stores] SET [state]=@state,[name]=@name,[regionid]=@regionid,[storerid]=@storerid,[storeiid]=@storeiid,[logo]=@logo,[createtime]=@createtime,[mobile]=@mobile,[phone]=@phone,[qq]=@qq,[ww]=@ww,[depoint]=@depoint,[sepoint]=@sepoint,[shpoint]=@shpoint,[honesties]=@honesties,[stateendtime]=@stateendtime,[theme]=@theme,[banner]=@banner,[announcement]=@announcement,[description]=@description WHERE [storeid]=@storeid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 获得店铺
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <returns></returns>
        public IDataReader GetStoreById(int storeId)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@storeid", SqlDbType.Int, 4, storeId)    
                                    };
            return RDBSHelper.ExecuteReader(CommandType.StoredProcedure,
                                            string.Format("{0}getstorebyid", RDBSHelper.RDBSTablePre),
                                            parms);
        }

        /// <summary>
        /// 后台获得店铺列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public DataTable AdminGetStoreList(int pageSize, int pageNumber, string condition)
        {
            bool noCondition = string.IsNullOrWhiteSpace(condition);
            string commandText;
            if (pageNumber == 1)
            {
                if (noCondition)
                    commandText = string.Format("SELECT [temp1].[storeid],[temp1].[state],[temp1].[name],[temp1].[storerid],[temp1].[createtime],[temp2].[title],[temp3].[uid] FROM (SELECT TOP {0} [storeid],[state],[name],[storerid],[createtime] FROM [{1}stores] ORDER BY [storeid] DESC) AS [temp1] LEFT JOIN [{1}storeranks] AS [temp2] ON [temp1].[storerid]=[temp2].[storerid] LEFT JOIN [{1}users] AS [temp3] ON [temp1].[storeid]=[temp3].[storeid]",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre);

                else
                    commandText = string.Format("SELECT [temp1].[storeid],[temp1].[state],[temp1].[name],[temp1].[storerid],[temp1].[createtime],[temp2].[title],[temp3].[uid] FROM (SELECT TOP {0} [storeid],[state],[name],[storerid],[createtime] FROM [{1}stores] WHERE {2} ORDER BY [storeid] DESC) AS [temp1] LEFT JOIN [{1}storeranks] AS [temp2] ON [temp1].[storerid]=[temp2].[storerid] LEFT JOIN [{1}users] AS [temp3] ON [temp1].[storeid]=[temp3].[storeid]",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                condition);
            }
            else
            {
                if (noCondition)
                    commandText = string.Format("SELECT [temp1].[storeid],[temp1].[state],[temp1].[name],[temp1].[storerid],[temp1].[createtime],[temp2].[title],[temp3].[uid] FROM (SELECT TOP {0} [storeid],[state],[name],[storerid],[createtime] FROM [{1}stores] WHERE [storeid] NOT IN (SELECT TOP {2} [storeid] FROM [{1}stores] ORDER BY [storeid] DESC) ORDER BY [storeid] DESC) AS [temp1] LEFT JOIN [{1}storeranks] AS [temp2] ON [temp1].[storerid]=[temp2].[storerid] LEFT JOIN [{1}users] AS [temp3] ON [temp1].[storeid]=[temp3].[storeid]",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                (pageNumber - 1) * pageSize);
                else
                    commandText = string.Format("SELECT [temp1].[storeid],[temp1].[state],[temp1].[name],[temp1].[storerid],[temp1].[createtime],[temp2].[title],[temp3].[uid] FROM (SELECT TOP {0} [storeid],[state],[name],[storerid],[createtime] FROM [{1}stores] WHERE [storeid] NOT IN (SELECT TOP {2} [storeid] FROM [{1}stores] WHERE {3} ORDER BY [storeid] DESC) AND {3} ORDER BY [storeid] DESC) AS [temp1] LEFT JOIN [{1}storeranks] AS [temp2] ON [temp1].[storerid]=[temp2].[storerid] LEFT JOIN [{1}users] AS [temp3] ON [temp1].[storeid]=[temp3].[storeid]",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                (pageNumber - 1) * pageSize,
                                                condition);
            }

            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 后台获得店铺选择列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public DataTable AdminGetStoreSelectList(int pageSize, int pageNumber, string condition)
        {
            bool noCondition = string.IsNullOrWhiteSpace(condition);
            string commandText;
            if (pageNumber == 1)
            {
                if (noCondition)
                    commandText = string.Format("SELECT TOP {0} [storeid],[name] FROM [{1}stores] ORDER BY [storeid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre);

                else
                    commandText = string.Format("SELECT TOP {0} [storeid],[name] FROM [{1}stores] WHERE {2} ORDER BY [storeid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                condition);
            }
            else
            {
                if (noCondition)
                    commandText = string.Format("SELECT TOP {0} [storeid],[name] FROM [{1}stores] WHERE [storeid] < (SELECT MIN([storeid]) FROM (SELECT TOP {2} [storeid] FROM [{1}stores] ORDER BY [storeid] DESC) AS [temp1]) ORDER BY [storeid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                (pageNumber - 1) * pageSize);
                else
                    commandText = string.Format("SELECT TOP {0} [storeid],[name] FROM [{1}stores] WHERE {3} AND [storeid] < (SELECT MIN([storeid]) FROM (SELECT TOP {2} [storeid] FROM [{1}stores] WHERE {3} ORDER BY [storeid] DESC) AS [temp1]) ORDER BY [storeid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                (pageNumber - 1) * pageSize,
                                                condition);
            }

            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 后台获得店铺列表条件
        /// </summary>
        /// <param name="storeName">店铺名称</param>
        /// <param name="storeRid">店铺等级id</param>
        /// <param name="storeIid">店铺行业id</param>
        /// <param name="state">店铺状态</param>
        /// <returns></returns>
        public string AdminGetStoreListCondition(string storeName, int storeRid, int storeIid, int state)
        {
            StringBuilder condition = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(storeName) && SecureHelper.IsSafeSqlString(storeName))
                condition.AppendFormat(" AND [name] like '{0}%' ", storeName);

            if (storeRid > 0)
                condition.AppendFormat(" AND [storerid] = {0} ", storeRid);

            if (storeIid > 0)
                condition.AppendFormat(" AND [storeiid] = {0} ", storeIid);

            if (state > -1)
                condition.AppendFormat(" AND [state]={0} ", (int)state);

            return condition.Length > 0 ? condition.Remove(0, 4).ToString() : "";
        }

        /// <summary>
        /// 后台获得店铺数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public int AdminGetStoreCount(string condition)
        {
            string commandText;
            if (string.IsNullOrWhiteSpace(condition))
                commandText = string.Format("SELECT COUNT(storeid) FROM [{0}stores]", RDBSHelper.RDBSTablePre);
            else
                commandText = string.Format("SELECT COUNT(storeid) FROM [{0}stores] WHERE {1}", RDBSHelper.RDBSTablePre, condition);

            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText));
        }

        /// <summary>
        /// 根据店铺名称得到店铺id
        /// </summary>
        /// <param name="storeName">店铺名称</param>
        /// <returns></returns>
        public int GetStoreIdByName(string storeName)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@name", SqlDbType.NChar, 60, storeName)
                                    };
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                    string.Format("{0}getstoreidbyname", RDBSHelper.RDBSTablePre),
                                                                    parms));
        }

        /// <summary>
        /// 后台根据店铺名称得到店铺id
        /// </summary>
        /// <param name="storeName">店铺名称</param>
        /// <returns></returns>
        public int AdminGetStoreIdByName(string storeName)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@name", SqlDbType.NChar, 60, storeName)
                                    };
            string commandText = string.Format("SELECT [storeid] FROM [{0}stores] WHERE [name]=@name",
                                                RDBSHelper.RDBSTablePre);
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms));
        }

        /// <summary>
        /// 更新店铺状态
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="state">状态</param>
        /// <param name="stateEndTime">状态结束时间</param>
        public void UpdateStoreState(int storeId, StoreState state, DateTime stateEndTime)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@state", SqlDbType.TinyInt,1,(int)state),
                                    GenerateInParam("@stateendtime", SqlDbType.DateTime,8,stateEndTime),
                                    GenerateInParam("@storeid", SqlDbType.Int,4,storeId)
                                   };
            string commandText = string.Format("UPDATE [{0}stores] SET [state]=@state,[stateendtime]=@stateendtime WHERE [storeid]=@storeid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 更新店铺积分
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="dePoint">商品描述评分</param>
        /// <param name="sePoint">商家服务评分</param>
        /// <param name="shPoint">商家配送评分</param>
        public void UpdateStorePoint(int storeId, decimal dePoint, decimal sePoint, decimal shPoint)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@depoint", SqlDbType.Decimal,4,dePoint),
                                    GenerateInParam("@sepoint", SqlDbType.Decimal,4,sePoint),
                                    GenerateInParam("@shpoint", SqlDbType.Decimal,4,shPoint),
                                    GenerateInParam("@storeid", SqlDbType.Int,4,storeId)
                                   };
            string commandText = string.Format("UPDATE [{0}stores] SET [depoint]=@depoint,[sepoint]=@sepoint,[shpoint]=@shpoint WHERE [storeid]=@storeid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 获得店铺数量通过店铺行业id
        /// </summary>
        /// <param name="storeIid">店铺行业id</param>
        /// <returns></returns>
        public int GetStoreCountByStoreIid(int storeIid)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@storeiid", SqlDbType.SmallInt, 2, storeIid)    
                                    };
            string commandText = string.Format("SELECT COUNT([storeid]) FROM [{0}stores] WHERE [storeiid]=@storeiid",
                                                RDBSHelper.RDBSTablePre);
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms));
        }

        /// <summary>
        /// 获得店铺数量通过店铺等级id
        /// </summary>
        /// <param name="storeRid">店铺等级id</param>
        /// <returns></returns>
        public int GetStoreCountByStoreRid(int storeRid)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@storerid", SqlDbType.SmallInt, 2, storeRid)    
                                    };
            string commandText = string.Format("SELECT COUNT([storeid]) FROM [{0}stores] WHERE [storerid]=@storerid",
                                                RDBSHelper.RDBSTablePre);
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms));
        }

        /// <summary>
        /// 获得店铺id列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetStoreIdList()
        {
            string commandText = string.Format("SELECT [storeid] FROM [{0}stores]",
                                                RDBSHelper.RDBSTablePre);
            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        #endregion

        #region 店长

        /// <summary>
        /// 创建店长
        /// </summary>
        /// <param name="storeKeeperInfo">店长信息</param>
        public void CreateStoreKeeper(StoreKeeperInfo storeKeeperInfo)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@storeid", SqlDbType.Int,4,storeKeeperInfo.StoreId),
                                        GenerateInParam("@type", SqlDbType.TinyInt,1,storeKeeperInfo.Type),
                                        GenerateInParam("@name", SqlDbType.NVarChar, 100, storeKeeperInfo.Name),
                                        GenerateInParam("@idcard", SqlDbType.NVarChar,50,storeKeeperInfo.IdCard),
                                        GenerateInParam("@address", SqlDbType.NVarChar,300,storeKeeperInfo.Address)
                                    };
            string commandText = string.Format("INSERT INTO [{0}storekeepers]([storeid],[type],[name],[idcard],[address]) VALUES(@storeid,@type,@name,@idcard,@address)",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 更新店长
        /// </summary>
        /// <param name="storeKeeperInfo">店长信息</param>
        public void UpdateStoreKeeper(StoreKeeperInfo storeKeeperInfo)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@storeid", SqlDbType.Int,4,storeKeeperInfo.StoreId),
                                        GenerateInParam("@type", SqlDbType.TinyInt,1,storeKeeperInfo.Type),
                                        GenerateInParam("@name", SqlDbType.NVarChar, 100, storeKeeperInfo.Name),
                                        GenerateInParam("@idcard", SqlDbType.NVarChar,50,storeKeeperInfo.IdCard),
                                        GenerateInParam("@address", SqlDbType.NVarChar,300,storeKeeperInfo.Address)
                                    };
            string commandText = string.Format("UPDATE [{0}storekeepers] SET [type]=@type,[name]=@name,[idcard]=@idcard,[address]=@address WHERE [storeid]=@storeid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 获得店长
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <returns></returns>
        public IDataReader GetStoreKeeperById(int storeId)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@storeid", SqlDbType.Int, 4, storeId)
                                   };
            string commandText = string.Format("SELECT {1} FROM [{0}storekeepers] WHERE [storeid]=@storeid",
                                                RDBSHelper.RDBSTablePre,
                                                TableFields.STORE_KEEPERS);
            return RDBSHelper.ExecuteReader(CommandType.Text, commandText, parms);
        }

        #endregion

        #region 店铺分类

        /// <summary>
        /// 获得店铺分类列表
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <returns></returns>
        public IDataReader GetStoreClassList(int storeId)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@storeid", SqlDbType.Int, 4, storeId)
                                   };
            return RDBSHelper.ExecuteReader(CommandType.StoredProcedure,
                                            string.Format("{0}getstoreclasslist", RDBSHelper.RDBSTablePre),
                                            parms);
        }

        /// <summary>
        /// 创建店铺分类
        /// </summary>
        public int CreateStoreClass(StoreClassInfo storeClassInfo)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@storeid", SqlDbType.Int,4, storeClassInfo.StoreId),
                                        GenerateInParam("@displayorder", SqlDbType.Int,4, storeClassInfo.DisplayOrder),
                                        GenerateInParam("@name", SqlDbType.NChar,60, storeClassInfo.Name),
                                        GenerateInParam("@parentid", SqlDbType.Int, 4, storeClassInfo.ParentId),
                                        GenerateInParam("@layer", SqlDbType.TinyInt,1,storeClassInfo.Layer),
                                        GenerateInParam("@haschild", SqlDbType.TinyInt,1,storeClassInfo.HasChild),
                                        GenerateInParam("@path", SqlDbType.Char,100, storeClassInfo.Path)
                                    };
            string commandText = string.Format("INSERT INTO [{0}storeclasses]([storeid],[displayorder],[name],[parentid],[layer],[haschild],[path]) VALUES(@storeid,@displayorder,@name,@parentid,@layer,@haschild,@path);SELECT SCOPE_IDENTITY()",
                                                RDBSHelper.RDBSTablePre);
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms), -1);
        }

        /// <summary>
        /// 更新店铺分类
        /// </summary>
        public void UpdateStoreClass(StoreClassInfo storeClassInfo)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@storeid", SqlDbType.Int,4, storeClassInfo.StoreId),
                                        GenerateInParam("@displayorder", SqlDbType.Int,4, storeClassInfo.DisplayOrder),
                                        GenerateInParam("@name", SqlDbType.NChar,60, storeClassInfo.Name),
                                        GenerateInParam("@parentid", SqlDbType.Int, 4, storeClassInfo.ParentId),
                                        GenerateInParam("@layer", SqlDbType.TinyInt,1,storeClassInfo.Layer),
                                        GenerateInParam("@haschild", SqlDbType.TinyInt,1,storeClassInfo.HasChild),
                                        GenerateInParam("@path", SqlDbType.Char,100, storeClassInfo.Path),
                                        GenerateInParam("@storecid", SqlDbType.Int,4, storeClassInfo.StoreCid)
                                    };
            string commandText = string.Format("UPDATE [{0}storeclasses] SET [storeid]=@storeid,[displayorder]=@displayorder,[name]=@name,[parentId]=@parentId,[layer]=@layer,[haschild]=@haschild,[path]=@path WHERE [storecid]=@storecid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 删除店铺分类
        /// </summary>
        /// <param name="storeCid">店铺分类id</param>
        public void DeleteStoreClassById(int storeCid)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@storecid", SqlDbType.Int, 4, storeCid)    
                                    };
            string commandText = string.Format("DELETE FROM [{0}storeclasses] WHERE [storecid]=@storecid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        #endregion

        #region 店铺配送模板

        /// <summary>
        /// 创建店铺配送模板
        /// </summary>
        /// <param name="storeShipTemplateInfo">店铺配送模板信息</param>
        public int CreateStoreShipTemplate(StoreShipTemplateInfo storeShipTemplateInfo)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@storeid", SqlDbType.Int,4, storeShipTemplateInfo.StoreId),
                                        GenerateInParam("@free", SqlDbType.TinyInt,1, storeShipTemplateInfo.Free),
                                        GenerateInParam("@type", SqlDbType.TinyInt,1, storeShipTemplateInfo.Type),
                                        GenerateInParam("@title", SqlDbType.NChar, 100, storeShipTemplateInfo.Title)
                                    };
            string commandText = string.Format("INSERT INTO [{0}storeshiptemplates]([storeid],[free],[type],[title]) VALUES(@storeid,@free,@type,@title);SELECT SCOPE_IDENTITY();",
                                                RDBSHelper.RDBSTablePre);
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms));
        }

        /// <summary>
        /// 更新店铺配送模板
        /// </summary>
        /// <param name="storeShipTemplateInfo">店铺配送模板信息</param>
        public void UpdateStoreShipTemplate(StoreShipTemplateInfo storeShipTemplateInfo)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@storeid", SqlDbType.Int,4, storeShipTemplateInfo.StoreId),
                                        GenerateInParam("@free", SqlDbType.TinyInt,1, storeShipTemplateInfo.Free),
                                        GenerateInParam("@type", SqlDbType.TinyInt,1, storeShipTemplateInfo.Type),
                                        GenerateInParam("@title", SqlDbType.NVarChar, 100, storeShipTemplateInfo.Title),
                                        GenerateInParam("@storestid", SqlDbType.Int,4, storeShipTemplateInfo.StoreSTid)
                                    };
            string commandText = string.Format("UPDATE [{0}storeshiptemplates] SET [storeid]=@storeid,[free]=@free,[type]=@type,[title]=@title WHERE [storestid]=@storestid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 删除店铺配送模板
        /// </summary>
        /// <param name="storeSTid">店铺配送模板id</param>
        public void DeleteStoreShipTemplateById(int storeSTid)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@storestid", SqlDbType.Int, 4, storeSTid)    
                                    };
            string commandText = string.Format("DELETE FROM [{0}storeshipfees] WHERE [storestid]=@storestid;DELETE FROM [{0}storeshiptemplates] WHERE [storestid]=@storestid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 获得店铺配送模板
        /// </summary>
        /// <param name="storeSTid">店铺配送模板id</param>
        /// <returns></returns>
        public IDataReader GetStoreShipTemplateById(int storeSTid)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@storestid", SqlDbType.Int, 4, storeSTid)
                                   };
            return RDBSHelper.ExecuteReader(CommandType.StoredProcedure,
                                            string.Format("{0}getstoreshiptemplatebyid", RDBSHelper.RDBSTablePre),
                                            parms);
        }

        /// <summary>
        /// 获得店铺配送模板列表
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <returns></returns>
        public IDataReader GetStoreShipTemplateList(int storeId)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@storeid", SqlDbType.Int, 4, storeId)
                                   };
            string commandText = string.Format("SELECT {1} FROM [{0}storeshiptemplates] WHERE [storeid]=@storeid",
                                                RDBSHelper.RDBSTablePre,
                                                TableFields.STORE_SHIPTEMPLATES);
            return RDBSHelper.ExecuteReader(CommandType.Text, commandText, parms);
        }

        #endregion

        #region 店铺配送费用

        /// <summary>
        /// 创建店铺配送费用
        /// </summary>
        /// <param name="storeShipFeeInfo">店铺配送费用信息</param>
        public void CreateStoreShipFee(StoreShipFeeInfo storeShipFeeInfo)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@storestid", SqlDbType.Int,4, storeShipFeeInfo.StoreSTid),
                                        GenerateInParam("@regionid", SqlDbType.Int,4, storeShipFeeInfo.RegionId),
                                        GenerateInParam("@startvalue", SqlDbType.Int,4, storeShipFeeInfo.StartValue),
                                        GenerateInParam("@startfee", SqlDbType.Int,4, storeShipFeeInfo.StartFee),
                                        GenerateInParam("@addvalue", SqlDbType.Int,4, storeShipFeeInfo.AddValue),
                                        GenerateInParam("@addfee", SqlDbType.Int,4, storeShipFeeInfo.AddFee)
                                    };
            string commandText = string.Format("INSERT INTO [{0}storeshipfees]([storestid],[regionid],[startvalue],[startfee],[addvalue],[addfee]) VALUES(@storestid,@regionid,@startvalue,@startfee,@addvalue,@addfee)",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 更新店铺配送费用
        /// </summary>
        /// <param name="storeShipFeeInfo">店铺配送费用信息</param>
        public void UpdateStoreShipFee(StoreShipFeeInfo storeShipFeeInfo)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@storestid", SqlDbType.Int,4, storeShipFeeInfo.StoreSTid),
                                        GenerateInParam("@regionid", SqlDbType.Int,4, storeShipFeeInfo.RegionId),
                                        GenerateInParam("@startvalue", SqlDbType.Int,4, storeShipFeeInfo.StartValue),
                                        GenerateInParam("@startfee", SqlDbType.Int,4, storeShipFeeInfo.StartFee),
                                        GenerateInParam("@addvalue", SqlDbType.Int,4, storeShipFeeInfo.AddValue),
                                        GenerateInParam("@addfee", SqlDbType.Int,4, storeShipFeeInfo.AddFee),
                                        GenerateInParam("@recordid", SqlDbType.Int,4, storeShipFeeInfo.RecordId)
                                    };
            string commandText = string.Format("UPDATE [{0}storeshipfees] SET [storestid]=@storestid,[regionid]=@regionid,[startvalue]=@startvalue,[startfee]=@startfee,[addvalue]=@addvalue,[addfee]=@addfee WHERE [recordid]=@recordid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 删除店铺配送费用
        /// </summary>
        /// <param name="recordId">记录id</param>
        public void DeleteStoreShipFeeById(int recordId)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@recordid", SqlDbType.Int, 4, recordId)    
                                    };
            string commandText = string.Format("DELETE FROM [{0}storeshipfees] WHERE [recordid]=@recordId",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 获得店铺配送费用
        /// </summary>
        /// <param name="recordId">记录id</param>
        /// <returns></returns>
        public IDataReader GetStoreShipFeeById(int recordId)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@recordid", SqlDbType.Int, 4, recordId)
                                   };
            string commandText = string.Format("SELECT {1} FROM [{0}storeshipfees] WHERE [recordid]=@recordId",
                                                RDBSHelper.RDBSTablePre,
                                                TableFields.STORE_SHIPFEES);
            return RDBSHelper.ExecuteReader(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 后台获得店铺配送费用列表
        /// </summary>
        /// <param name="storeSTid">店铺配送模板id</param>
        /// <returns></returns>
        public DataTable AdminGetStoreShipFeeList(int storeSTid)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@storestid", SqlDbType.Int, 4, storeSTid)
                                   };
            string commandText = string.Format("SELECT [temp1].[recordid],[temp1].[storestid],[temp1].[regionid],[temp1].[startvalue],[temp1].[startfee],[temp1].[addvalue],[temp1].[addfee],[temp2].[provincename],[temp2].[cityname],[temp2].[name] FROM (SELECT {1} FROM [{0}storeshipfees] WHERE [storestid]=@storestid) AS [temp1] LEFT JOIN [{0}regions] AS [temp2] ON [temp1].[regionid]=[temp2].[regionid]",
                                                RDBSHelper.RDBSTablePre,
                                                TableFields.STORE_SHIPFEES);
            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
        }

        /// <summary>
        /// 获得店铺配送费用
        /// </summary>
        /// <param name="storeSTid">店铺模板id</param>
        /// <param name="provinceId">省id</param>
        /// <param name="cityId">市id</param>
        /// <returns></returns>
        public IDataReader GetStoreShipFeeByStoreSTidAndRegion(int storeSTid, int provinceId, int cityId)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@storestid", SqlDbType.Int, 4, storeSTid),
                                    GenerateInParam("@provinceid", SqlDbType.SmallInt, 2, provinceId),
                                    GenerateInParam("@cityid", SqlDbType.SmallInt, 2, cityId)
                                   };
            return RDBSHelper.ExecuteReader(CommandType.StoredProcedure,
                                            string.Format("{0}getstoreshipfeebystorestidandregion", RDBSHelper.RDBSTablePre),
                                            parms);
        }

        #endregion
    }
}
