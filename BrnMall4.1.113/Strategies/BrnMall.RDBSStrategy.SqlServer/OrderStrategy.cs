using System;
using System.Data;
using System.Text;
using System.Data.Common;

using BrnMall.Core;

namespace BrnMall.RDBSStrategy.SqlServer
{
    /// <summary>
    /// SqlServer策略之订单分部类
    /// </summary>
    public partial class RDBSStrategy : IRDBSStrategy
    {
        #region 订单

        /// <summary>
        /// 获得订单信息
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <returns>订单信息</returns>
        public IDataReader GetOrderByOid(int oid)
        {
            DbParameter[] parms = {
	                                 GenerateInParam("@oid", SqlDbType.Int,4,oid)
                                    };
            return RDBSHelper.ExecuteReader(CommandType.StoredProcedure,
                                            string.Format("{0}getorderbyoid", RDBSHelper.RDBSTablePre),
                                            parms);
        }

        /// <summary>
        /// 获得订单信息
        /// </summary>
        /// <param name="osn">订单编号</param>
        /// <returns>订单信息</returns>
        public IDataReader GetOrderByOSN(string osn)
        {
            DbParameter[] parms = {
	                                 GenerateInParam("@osn", SqlDbType.Char,30,osn)
                                    };
            return RDBSHelper.ExecuteReader(CommandType.StoredProcedure,
                                            string.Format("{0}getorderbyosn", RDBSHelper.RDBSTablePre),
                                            parms);
        }

        /// <summary>
        /// 获得订单数量
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="orderState">订单状态</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public int GetOrderCountByCondition(int storeId, int orderState, string startTime, string endTime)
        {
            StringBuilder condition = new StringBuilder();

            if (storeId > 0)
                condition.AppendFormat(" AND [storeid] = {0}", storeId);
            if (orderState > 0)
                condition.AppendFormat(" AND [orderstate] = {0}", orderState);
            if (!string.IsNullOrEmpty(startTime))
                condition.AppendFormat(" AND [addtime] >= '{0}'", TypeHelper.StringToDateTime(startTime).ToString("yyyy-MM-dd HH:mm:ss"));
            if (!string.IsNullOrEmpty(endTime))
                condition.AppendFormat(" AND [addtime] <= '{0}'", TypeHelper.StringToDateTime(endTime).ToString("yyyy-MM-dd HH:mm:ss"));

            string commandText;
            if (condition.Length > 0)
            {
                commandText = string.Format("SELECT COUNT([oid]) FROM [{0}orders] WHERE {1}",
                                             RDBSHelper.RDBSTablePre,
                                             condition.Remove(0, 4).ToString());
            }
            else
            {
                commandText = string.Format("SELECT COUNT([oid]) FROM [{0}orders]",
                                            RDBSHelper.RDBSTablePre);
            }

            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText));
        }

        /// <summary>
        /// 获得订单列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public DataTable GetOrderList(int pageSize, int pageNumber, string condition)
        {
            bool noCondition = string.IsNullOrWhiteSpace(condition);
            string commandText;
            if (pageNumber == 1)
            {
                if (noCondition)
                    commandText = string.Format("SELECT [temp1].[oid],[temp1].[osn],[temp1].[uid],[temp1].[orderstate],[temp1].[orderamount],[temp1].[surplusmoney],[temp1].[isreview],[temp1].[addtime],[temp1].[storeid],[temp1].[storename],[temp1].[paymode],[temp1].[regionid],[temp1].[consignee],[temp1].[mobile],[temp1].[phone],[temp1].[email],[temp1].[zipcode],[temp1].[address],[temp1].[besttime],[temp2].[username],[temp3].[provincename],[temp3].[cityname],[temp3].[name] AS [countyname] FROM (SELECT TOP {0} [oid],[osn],[uid],[orderstate],[orderamount],[surplusmoney],[isreview],[addtime],[storeid],[storename],[paymode],[regionid],[consignee],[mobile],[phone],[email],[zipcode],[address],[besttime] FROM [{1}orders] ORDER BY [oid] DESC) AS [temp1] LEFT JOIN [{1}users] AS [temp2] ON [temp1].[uid]=[temp2].[uid] LEFT JOIN [{1}regions] AS [temp3] ON [temp1].[regionid]=[temp3].[regionid]",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre);

                else
                    commandText = string.Format("SELECT [temp1].[oid],[temp1].[osn],[temp1].[uid],[temp1].[orderstate],[temp1].[orderamount],[temp1].[surplusmoney],[temp1].[isreview],[temp1].[addtime],[temp1].[storeid],[temp1].[storename],[temp1].[paymode],[temp1].[regionid],[temp1].[consignee],[temp1].[mobile],[temp1].[phone],[temp1].[email],[temp1].[zipcode],[temp1].[address],[temp1].[besttime],[temp2].[username],[temp3].[provincename],[temp3].[cityname],[temp3].[name] AS [countyname] FROM (SELECT TOP {0} [oid],[osn],[uid],[orderstate],[orderamount],[surplusmoney],[isreview],[addtime],[storeid],[storename],[paymode],[regionid],[consignee],[mobile],[phone],[email],[zipcode],[address],[besttime] FROM [{1}orders] WHERE {2} ORDER BY [oid] DESC) AS [temp1] LEFT JOIN [{1}users] AS [temp2] ON [temp1].[uid]=[temp2].[uid] LEFT JOIN [{1}regions] AS [temp3] ON [temp1].[regionid]=[temp3].[regionid]",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                condition);
            }
            else
            {
                if (noCondition)
                    commandText = string.Format("SELECT [temp1].[oid],[temp1].[osn],[temp1].[uid],[temp1].[orderstate],[temp1].[orderamount],[temp1].[surplusmoney],[temp1].[isreview],[temp1].[addtime],[temp1].[storeid],[temp1].[storename],[temp1].[paymode],[temp1].[regionid],[temp1].[consignee],[temp1].[mobile],[temp1].[phone],[temp1].[email],[temp1].[zipcode],[temp1].[address],[temp1].[besttime],[temp2].[username],[temp3].[provincename],[temp3].[cityname],[temp3].[name] AS [countyname] FROM (SELECT [oid],[osn],[uid],[orderstate],[orderamount],[surplusmoney],[isreview],[addtime],[storeid],[storename],[paymode],[regionid],[consignee],[mobile],[phone],[email],[zipcode],[address],[besttime] FROM (SELECT TOP {0} ROW_NUMBER() OVER (ORDER BY [oid] DESC) AS [rowid],[oid],[osn],[uid],[orderstate],[orderamount],[surplusmoney],[isreview],[addtime],[storeid],[storename],[paymode],[regionid],[consignee],[mobile],[phone],[email],[zipcode],[address],[besttime] FROM [{1}orders]) AS [temp] WHERE [temp].[rowid] BETWEEN {2} AND {3}) AS [temp1] LEFT JOIN [{1}users] AS [temp2] ON [temp1].[uid]=[temp2].[uid] LEFT JOIN [{1}regions] AS [temp3] ON [temp1].[regionid]=[temp3].[regionid]",
                                                pageSize * pageNumber,
                                                RDBSHelper.RDBSTablePre,
                                                pageSize * (pageNumber - 1) + 1,
                                                pageSize * pageNumber);

                else
                    commandText = string.Format("SELECT [temp1].[oid],[temp1].[osn],[temp1].[uid],[temp1].[orderstate],[temp1].[orderamount],[temp1].[surplusmoney],[temp1].[isreview],[temp1].[addtime],[temp1].[storeid],[temp1].[storename],[temp1].[paymode],[temp1].[regionid],[temp1].[consignee],[temp1].[mobile],[temp1].[phone],[temp1].[email],[temp1].[zipcode],[temp1].[address],[temp1].[besttime],[temp2].[username],[temp3].[provincename],[temp3].[cityname],[temp3].[name] AS [countyname] FROM (SELECT [oid],[osn],[uid],[orderstate],[orderamount],[surplusmoney],[isreview],[addtime],[storeid],[storename],[paymode],[regionid],[consignee],[mobile],[phone],[email],[zipcode],[address],[besttime] FROM (SELECT TOP {0} ROW_NUMBER() OVER (ORDER BY [oid] DESC) AS [rowid],[oid],[osn],[uid],[orderstate],[orderamount],[surplusmoney],[isreview],[addtime],[storeid],[storename],[paymode],[regionid],[consignee],[mobile],[phone],[email],[zipcode],[address],[besttime] FROM [{1}orders] WHERE {4}) AS [temp] WHERE [temp].[rowid] BETWEEN {2} AND {3}) AS [temp1] LEFT JOIN [{1}users] AS [temp2] ON [temp1].[uid]=[temp2].[uid] LEFT JOIN [{1}regions] AS [temp3] ON [temp1].[regionid]=[temp3].[regionid]",
                                                pageSize * pageNumber,
                                                RDBSHelper.RDBSTablePre,
                                                pageSize * (pageNumber - 1) + 1,
                                                pageSize * pageNumber,
                                                condition);
            }

            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 获得订单列表搜索条件
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="osn">订单编号</param>
        /// <param name="uid">用户id</param>
        /// <param name="consignee">收货人</param>
        /// <param name="orderState">订单状态</param>
        /// <returns></returns>
        public string GetOrderListCondition(int storeId, string osn, int uid, string consignee, int orderState)
        {
            StringBuilder condition = new StringBuilder();

            if (storeId > 0)
                condition.AppendFormat(" AND [storeid] = {0} ", storeId);
            if (!string.IsNullOrWhiteSpace(osn) && SecureHelper.IsSafeSqlString(osn))
                condition.AppendFormat(" AND [osn] like '{0}%' ", osn);
            if (uid > 0)
                condition.AppendFormat(" AND [uid] = {0} ", uid);
            if (!string.IsNullOrWhiteSpace(consignee) && SecureHelper.IsSafeSqlString(consignee))
                condition.AppendFormat(" AND [consignee] like '{0}%' ", consignee);
            if (orderState > 0)
                condition.AppendFormat(" AND [orderstate] = {0} ", orderState);

            return condition.Length > 0 ? condition.Remove(0, 4).ToString() : "";
        }

        /// <summary>
        /// 获得订单数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public int GetOrderCount(string condition)
        {
            string commandText;
            if (string.IsNullOrWhiteSpace(condition))
                commandText = string.Format("SELECT COUNT(oid) FROM [{0}orders]", RDBSHelper.RDBSTablePre);
            else
                commandText = string.Format("SELECT COUNT(oid) FROM [{0}orders] WHERE {1}", RDBSHelper.RDBSTablePre, condition);

            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText), 0);
        }

        /// <summary>
        /// 获得订单商品列表
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <returns></returns>
        public IDataReader GetOrderProductList(int oid)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@oid", SqlDbType.Int, 4, oid)    
                                   };
            return RDBSHelper.ExecuteReader(CommandType.StoredProcedure,
                                            string.Format("{0}getorderproductlistbyoid", RDBSHelper.RDBSTablePre),
                                            parms);
        }

        /// <summary>
        /// 获得订单商品列表
        /// </summary>
        /// <param name="oidList">订单id列表</param>
        /// <returns></returns>
        public IDataReader GetOrderProductList(string oidList)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@oidlist", SqlDbType.NVarChar, 1000, oidList)    
                                   };
            return RDBSHelper.ExecuteReader(CommandType.StoredProcedure,
                                            string.Format("{0}getorderproductlistbyoidlist", RDBSHelper.RDBSTablePre),
                                            parms);
        }

        /// <summary>
        /// 更新订单折扣
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <param name="discount">折扣</param>
        /// <param name="surplusMoney">剩余金额</param>
        public void UpdateOrderDiscount(int oid, decimal discount, decimal surplusMoney)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@oid", SqlDbType.Int, 4, oid), 
                                    GenerateInParam("@discount", SqlDbType.Decimal, 8, discount), 
                                    GenerateInParam("@surplusmoney", SqlDbType.Decimal, 8, surplusMoney)
                                   };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}updateorderdiscount", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        /// <summary>
        /// 更新订单配送费用
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <param name="shipFee">配送费用</param>
        /// <param name="orderAmount">订单合计</param>
        /// <param name="surplusMoney">剩余金额</param>
        public void UpdateOrderShipFee(int oid, decimal shipFee, decimal orderAmount, decimal surplusMoney)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@oid", SqlDbType.Int, 4, oid), 
                                    GenerateInParam("@shipfee", SqlDbType.Decimal, 8, shipFee), 
                                    GenerateInParam("@orderamount", SqlDbType.Decimal, 8, orderAmount),
                                    GenerateInParam("@surplusmoney", SqlDbType.Decimal, 8, surplusMoney)
                                   };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}updateordershipfee", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <param name="orderState">订单状态</param>
        public void UpdateOrderState(int oid, OrderState orderState)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@oid", SqlDbType.Int, 4, oid), 
                                    GenerateInParam("@orderstate", SqlDbType.TinyInt, 1, (int)orderState)
                                   };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}updateorderstate", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <param name="orderState">订单状态</param>
        /// <param name="shipSN">配送单号</param>
        /// <param name="shipCoId">配送公司id</param>
        /// <param name="shipCoName">配送公司名称</param>
        /// <param name="shipTime">配送时间</param>
        public void SendOrderProduct(int oid, OrderState orderState, string shipSN, int shipCoId, string shipCoName, DateTime shipTime)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@oid", SqlDbType.Int, 4, oid), 
                                    GenerateInParam("@orderstate", SqlDbType.TinyInt, 1, (int)orderState),
                                    GenerateInParam("@shipsn", SqlDbType.Char, 30, shipSN),
                                    GenerateInParam("@shipcoid", SqlDbType.SmallInt, 2, shipCoId), 
                                    GenerateInParam("@shipconame", SqlDbType.NChar, 30, shipCoName), 
                                    GenerateInParam("@shiptime", SqlDbType.DateTime, 8, shipTime)
                                   };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}sendorderproduct", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        /// <summary>
        /// 付款
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <param name="orderState">订单状态</param>
        /// <param name="paySN">支付单号</param>
        /// <param name="paySystemName">支付系统名称</param>
        /// <param name="payFriendName">支付友好名称</param>
        /// <param name="payTime">支付时间</param>
        public void PayOrder(int oid, OrderState orderState, string paySN, string paySystemName, string payFriendName, DateTime payTime)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@oid", SqlDbType.Int, 4, oid), 
                                    GenerateInParam("@orderstate", SqlDbType.TinyInt, 1, (int)orderState),
                                    GenerateInParam("@paysn", SqlDbType.Char, 30, paySN),
                                    GenerateInParam("@paysystemname", SqlDbType.Char, 20, paySystemName),
                                    GenerateInParam("@payfriendname", SqlDbType.NChar, 30, payFriendName),
                                    GenerateInParam("@paytime", SqlDbType.DateTime, 8, payTime)
                                   };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}payorder", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        /// <summary>
        /// 更新订单的评价
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <param name="isReview">是否评价</param>
        public void UpdateOrderIsReview(int oid, int isReview)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@oid", SqlDbType.Int, 4, oid), 
                                    GenerateInParam("@isreview", SqlDbType.TinyInt, 1, isReview)
                                   };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}updateorderisreview", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        /// <summary>
        /// 清空过期的在线支付订单
        /// </summary>
        /// <param name="expireTime">过期时间</param>
        public void ClearExpiredOnlinePayOrder(DateTime expireTime)
        {
            DbParameter[] parms = {
	                                 GenerateInParam("@expiretime", SqlDbType.DateTime,8,expireTime)
                                   };
            string commandText = string.Format("DELETE FROM [{0}orderproducts] WHERE [oid] IN (SELECT [oid] FROM [{0}orders] WHERE [paymode]=1 AND [paysn]='' AND [addtime]<@expiretime);DELETE FROM [{0}orders] WHERE [paymode]=1 AND [paysn]='' AND [addtime]<@expiretime",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 自动收货
        /// </summary>
        /// <param name="expireTime">过期时间</param>
        public void AutoReceiveOrder(DateTime expireTime)
        {
            DbParameter[] parms = {
	                                 GenerateInParam("@expiretime", SqlDbType.DateTime,8,expireTime)
                                   };
            string commandText = string.Format("UPDATE [{0}orders] SET [orderstate]={1} WHERE [orderstate]={2} AND [shiptime]<@expiretime",
                                                RDBSHelper.RDBSTablePre,
                                                (int)OrderState.Received,
                                                (int)OrderState.Sended);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 获得用户订单列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="startAddTime">添加开始时间</param>
        /// <param name="endAddTime">添加结束时间</param>
        /// <param name="orderState">订单状态(0代表全部状态)</param>
        /// <returns></returns>
        public DataTable GetUserOrderList(int uid, int pageSize, int pageNumber, string startAddTime, string endAddTime, int orderState)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@uid", SqlDbType.Int, 4, uid), 
                                    GenerateInParam("@pagesize", SqlDbType.Int, 4, pageSize), 
                                    GenerateInParam("@pagenumber", SqlDbType.Int, 4, pageNumber), 
                                    GenerateInParam("@startaddtime", SqlDbType.VarChar, 30, startAddTime.Length > 0? TypeHelper.StringToDateTime(startAddTime).ToString("yyyy-MM-dd HH:mm:ss") : ""),
                                    GenerateInParam("@endaddtime", SqlDbType.VarChar, 30, endAddTime.Length > 0? TypeHelper.StringToDateTime(endAddTime).ToString("yyyy-MM-dd HH:mm:ss") : ""),
                                    GenerateInParam("@orderstate", SqlDbType.TinyInt, 1, orderState)
                                   };
            return RDBSHelper.ExecuteDataset(CommandType.StoredProcedure,
                                             string.Format("{0}getuserorderlist", RDBSHelper.RDBSTablePre),
                                             parms).Tables[0];
        }

        /// <summary>
        /// 获得用户订单列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="startAddTime">添加开始时间</param>
        /// <param name="endAddTime">添加结束时间</param>
        /// <param name="orderState">订单状态(0代表全部状态)</param>
        /// <returns></returns>
        public int GetUserOrderCount(int uid, string startAddTime, string endAddTime, int orderState)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@uid", SqlDbType.Int, 4, uid), 
                                    GenerateInParam("@startaddtime", SqlDbType.VarChar, 30, startAddTime.Length > 0? TypeHelper.StringToDateTime(startAddTime).ToString("yyyy-MM-dd HH:mm:ss") : ""),
                                    GenerateInParam("@endaddtime", SqlDbType.VarChar, 30, endAddTime.Length > 0? TypeHelper.StringToDateTime(endAddTime).ToString("yyyy-MM-dd HH:mm:ss") : ""),
                                    GenerateInParam("@orderstate", SqlDbType.TinyInt, 1, orderState)
                                   };
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                   string.Format("{0}getuserordercount", RDBSHelper.RDBSTablePre),
                                                                   parms));
        }

        /// <summary>
        /// 获得用户未评价订单列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="startAddTime">添加开始时间</param>
        /// <param name="endAddTime">添加结束时间</param>
        /// <returns></returns>
        public DataTable GetUserUnReviewOrderList(int uid, int pageSize, int pageNumber, string startAddTime, string endAddTime)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@uid", SqlDbType.Int, 4, uid), 
                                    GenerateInParam("@pagesize", SqlDbType.Int, 4, pageSize), 
                                    GenerateInParam("@pagenumber", SqlDbType.Int, 4, pageNumber), 
                                    GenerateInParam("@startaddtime", SqlDbType.VarChar, 30, startAddTime.Length > 0? TypeHelper.StringToDateTime(startAddTime).ToString("yyyy-MM-dd HH:mm:ss") : ""),
                                    GenerateInParam("@endaddtime", SqlDbType.VarChar, 30, endAddTime.Length > 0? TypeHelper.StringToDateTime(endAddTime).ToString("yyyy-MM-dd HH:mm:ss") : "")
                                   };
            return RDBSHelper.ExecuteDataset(CommandType.StoredProcedure,
                                             string.Format("{0}getuserunrevieworderlist", RDBSHelper.RDBSTablePre),
                                             parms).Tables[0];
        }

        /// <summary>
        /// 获得用户未评价订单列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="startAddTime">添加开始时间</param>
        /// <param name="endAddTime">添加结束时间</param>
        /// <returns></returns>
        public int GetUserUnReviewOrderCount(int uid, string startAddTime, string endAddTime)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@uid", SqlDbType.Int, 4, uid), 
                                    GenerateInParam("@startaddtime", SqlDbType.VarChar, 30, startAddTime.Length > 0? TypeHelper.StringToDateTime(startAddTime).ToString("yyyy-MM-dd HH:mm:ss") : ""),
                                    GenerateInParam("@endaddtime", SqlDbType.VarChar, 30, endAddTime.Length > 0? TypeHelper.StringToDateTime(endAddTime).ToString("yyyy-MM-dd HH:mm:ss") : "")
                                   };
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                   string.Format("{0}getuserunreviewordercount", RDBSHelper.RDBSTablePre),
                                                                   parms));
        }

        /// <summary>
        /// 获得销售商品列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="storeId">店铺id</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="orderState">订单状态</param>
        /// <returns></returns>
        public DataTable GetSaleProductList(int pageSize, int pageNumber, int storeId, string startTime, string endTime, int orderState)
        {
            string condition = GetSaleProductListCondition(storeId, startTime, endTime, orderState);
            string commandText;
            if (pageNumber == 1)
            {
                commandText = string.Format("SELECT TOP {2} [temp2].[psn],[temp2].[name],[temp2].[realcount],[temp2].[shopprice],[temp1].[osn],[temp1].[addtime] FROM (SELECT [oid],[osn],[addtime] FROM [{0}orders] {1}) AS [temp1] LEFT JOIN [{0}orderproducts] AS [temp2] ON [temp1].[oid]=[temp2].[oid] ORDER BY [recordid] DESC",
                                             RDBSHelper.RDBSTablePre,
                                             condition,
                                             pageSize);
            }
            else
            {
                commandText = string.Format("SELECT [psn],[name],[realcount],[shopprice],[osn],[addtime] FROM (SELECT TOP {1} ROW_NUMBER() OVER (ORDER BY [recordid] DESC) AS [rowid],[temp2].[psn],[temp2].[name],[temp2].[realcount],[temp2].[shopprice],[temp1].[osn],[temp1].[addtime] FROM (SELECT [oid],[osn],[addtime] FROM [{0}orders] {3}) AS [temp1] LEFT JOIN [{0}orderproducts] AS [temp2] ON [temp1].[oid]=[temp2].[oid]) AS [temp] WHERE [temp].[rowid] BETWEEN {2} AND {1}",
                                             RDBSHelper.RDBSTablePre,
                                             pageNumber * pageSize,
                                             (pageNumber - 1) * pageSize + 1,
                                             condition);
            }

            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 获得销售商品数量
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="orderState">订单状态</param>
        /// <returns></returns>
        public int GetSaleProductCount(int storeId, string startTime, string endTime, int orderState)
        {
            string condition = GetSaleProductListCondition(storeId, startTime, endTime, orderState);
            string commandText = string.Format("SELECT COUNT([temp2].[recordid]) FROM (SELECT [oid],[osn],[addtime] FROM [{0}orders] {1}) AS [temp1] LEFT JOIN [{0}orderproducts] AS [temp2] ON [temp1].[oid]=[temp2].[oid]",
                                                RDBSHelper.RDBSTablePre,
                                                condition);
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText));
        }

        /// <summary>
        /// 获得销售商品列表条件
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="orderState">订单状态</param>
        /// <returns></returns>
        private string GetSaleProductListCondition(int storeId, string startTime, string endTime, int orderState)
        {
            StringBuilder condition = new StringBuilder();

            if (orderState > 0)
                condition.AppendFormat(" AND [orderstate]={0} ", orderState);
            if (storeId > 0)
                condition.AppendFormat(" AND [storeid]={0} ", storeId);
            if (!string.IsNullOrEmpty(startTime))
                condition.AppendFormat(" AND [addtime]>='{0}' ", TypeHelper.StringToDateTime(startTime).ToString("yyyy-MM-dd HH:mm:ss"));
            if (!string.IsNullOrEmpty(endTime))
                condition.AppendFormat(" AND [addtime]<='{0}' ", TypeHelper.StringToDateTime(endTime).ToString("yyyy-MM-dd HH:mm:ss"));

            return condition.Length > 0 ? condition.Remove(0, 4).Insert(0, " WHERE").ToString() : "";
        }

        /// <summary>
        /// 获得订单统计
        /// </summary>
        /// <param name="statType">统计类型(0代表订单数，1代表订单合计)</param>
        /// <param name="storeId">店铺id</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public DataTable GetOrderStat(int statType, int storeId, string startTime, string endTime)
        {
            StringBuilder condition = new StringBuilder();

            if (storeId > 0)
                condition.AppendFormat(" AND [storeid]={0} ", storeId);
            if (!string.IsNullOrEmpty(startTime))
                condition.AppendFormat(" AND [addtime]>='{0}' ", TypeHelper.StringToDateTime(startTime).ToString("yyyy-MM-dd HH:mm:ss"));
            if (!string.IsNullOrEmpty(endTime))
                condition.AppendFormat(" AND [addtime]<'{0}' ", TypeHelper.StringToDateTime(endTime).ToString("yyyy-MM-dd HH:mm:ss"));

            if (condition.Length > 0)
                condition = condition.Remove(0, 4).Insert(0, " WHERE");

            string commandText = "";
            if (statType == 0)
            {
                commandText = string.Format("SELECT COUNT([oid]) AS [value],[orderstate] FROM (SELECT [oid],[orderstate] FROM [{0}orders] {1}) AS [temp] GROUP BY [temp].[orderstate]",
                                             RDBSHelper.RDBSTablePre,
                                             condition.ToString());
            }
            else
            {
                commandText = string.Format("SELECT SUM([orderamount]) AS [value],[orderstate] FROM (SELECT [oid],[orderstate],[orderamount] FROM [{0}orders] {1}) AS [temp] GROUP BY [temp].[orderstate]",
                                             RDBSHelper.RDBSTablePre,
                                             condition.ToString());
            }
            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        #endregion

        #region 订单处理

        /// <summary>
        /// 创建订单处理
        /// </summary>
        /// <param name="orderActionInfo">订单处理信息</param>
        public void CreateOrderAction(OrderActionInfo orderActionInfo)
        {
            DbParameter[] parms = {
	                                    GenerateInParam("@oid", SqlDbType.Int,4,orderActionInfo.Oid),
	                                    GenerateInParam("@uid", SqlDbType.Int,4 ,orderActionInfo.Uid),
	                                    GenerateInParam("@realname", SqlDbType.NVarChar,10,orderActionInfo.RealName),
	                                    GenerateInParam("@actiontype", SqlDbType.TinyInt,1 ,orderActionInfo.ActionType),
                                        GenerateInParam("@actiontime", SqlDbType.DateTime, 8,orderActionInfo.ActionTime),
                                        GenerateInParam("@actiondes", SqlDbType.NVarChar, 250,orderActionInfo.ActionDes)
                                    };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}createorderaction", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        /// <summary>
        /// 获得订单处理列表
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <returns></returns>
        public IDataReader GetOrderActionList(int oid)
        {
            DbParameter[] parms = {
	                                    GenerateInParam("@oid", SqlDbType.Int,4,oid)
                                    };
            return RDBSHelper.ExecuteReader(CommandType.StoredProcedure,
                                            string.Format("{0}getorderactionlist", RDBSHelper.RDBSTablePre),
                                            parms);
        }

        /// <summary>
        /// 获得订单id列表
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="orderActionType">订单操作类型</param>
        /// <returns></returns>
        public DataTable GetOrderIdList(DateTime startTime, DateTime endTime, int orderActionType)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@starttime", SqlDbType.DateTime, 8, startTime),
                                    GenerateInParam("@endtime", SqlDbType.DateTime, 8, endTime),
                                    GenerateInParam("@orderactiontype", SqlDbType.TinyInt, 1, orderActionType)
                                   };
            string commandText = string.Format("SELECT [oid] FROM [{0}orderactions] WHERE [actiontype]=@orderactiontype AND [actiontime]>=@starttime AND [actiontime]<@endtime",
                                                RDBSHelper.RDBSTablePre);
            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
        }

        #endregion

        #region 订单售后服务

        /// <summary>
        /// 申请订单售后服务
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="uid">用户id</param>
        /// <param name="oid">订单id</param>
        /// <param name="recordId">记录id</param>
        /// <param name="pid">商品id</param>
        /// <param name="cateId">分类id</param>
        /// <param name="brandId">品牌id</param>
        /// <param name="storeId">店铺id</param>
        /// <param name="pname">商品名称</param>
        /// <param name="pshowImg">商品图片</param>
        /// <param name="count">数量</param>
        /// <param name="money">金钱</param>
        /// <param name="type">类型(0代表退货,1代表换货,2代表维修)</param>
        /// <param name="applyReason">申请原因</param>
        /// <param name="applyTime">申请时间</param>
        public void ApplyOrderAfterService(OrderAfterServiceState state, int uid, int oid, int recordId, int pid, int cateId, int brandId, int storeId, string pname, string pshowImg, int count, decimal money, int type, string applyReason, DateTime applyTime)
        {
            DbParameter[] parms = {
	                                    GenerateInParam("@state", SqlDbType.TinyInt, 1, (int)state),
	                                    GenerateInParam("@uid", SqlDbType.Int, 4 , uid),
	                                    GenerateInParam("@oid", SqlDbType.Int, 4, oid),
	                                    GenerateInParam("@recordid", SqlDbType.Int, 4, recordId),
                                        GenerateInParam("@pid", SqlDbType.Int, 4, pid),
                                        GenerateInParam("@cateid", SqlDbType.SmallInt, 2, cateId),
                                        GenerateInParam("@brandid", SqlDbType.Int, 4, brandId),
                                        GenerateInParam("@storeid", SqlDbType.Int, 4, storeId),
                                        GenerateInParam("@pname", SqlDbType.NVarChar, 200, pname),
                                        GenerateInParam("@pshowimg", SqlDbType.NVarChar, 100, pshowImg),
                                        GenerateInParam("@count", SqlDbType.Int, 4, count),
                                        GenerateInParam("@money", SqlDbType.Decimal, 8, money),
                                        GenerateInParam("@type", SqlDbType.TinyInt, 1, type),
                                        GenerateInParam("@applyreason", SqlDbType.NVarChar, 300, applyReason),
                                        GenerateInParam("@applytime", SqlDbType.DateTime, 8, applyTime)
                                    };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}applyorderafterservice", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        /// <summary>
        /// 审核订单售后服务
        /// </summary>
        /// <param name="asId">售后服务id</param>
        /// <param name="state">状态</param>
        /// <param name="checkResult">审核结果</param>
        /// <param name="checkTime">审核时间</param>
        public void CheckOrderAfterService(int asId, OrderAfterServiceState state, string checkResult, DateTime checkTime)
        {
            DbParameter[] parms = {
	                                    GenerateInParam("@asid", SqlDbType.Int, 4, asId),
	                                    GenerateInParam("@state", SqlDbType.TinyInt, 1, (int)state),
                                        GenerateInParam("@checkresult", SqlDbType.NVarChar, 300, checkResult),
                                        GenerateInParam("@checktime", SqlDbType.DateTime, 8, checkTime)
                                    };
            string commandText = string.Format("UPDATE [{0}orderafterservices] SET [state]=@state,[checkresult]=@checkresult,[checktime]=@checktime WHERE [asid]=@asid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 邮寄给商城
        /// </summary>
        /// <param name="asId">售后服务id</param>
        /// <param name="state">状态</param>
        /// <param name="shipCoId">配送公司id</param>
        /// <param name="shipCoName">配送公司名称</param>
        /// <param name="shipSN">配送单号</param>
        /// <param name="regionId">收货区域id</param>
        /// <param name="consignee">收货人</param>
        /// <param name="mobile">手机</param>
        /// <param name="phone">固话</param>
        /// <param name="email">邮箱</param>
        /// <param name="zipCode">邮件编码</param>
        /// <param name="address">收货详情地址</param>
        /// <param name="sendTime">邮寄给商城时间</param>
        public void SendOrderAfterService(int asId, OrderAfterServiceState state, int shipCoId, string shipCoName, string shipSN, int regionId, string consignee, string mobile, string phone, string email, string zipCode, string address, DateTime sendTime)
        {
            DbParameter[] parms = {
	                                    GenerateInParam("@asid", SqlDbType.Int, 4, asId),
	                                    GenerateInParam("@state", SqlDbType.TinyInt, 1, (int)state),
	                                    GenerateInParam("@shipcoid", SqlDbType.SmallInt, 2, shipCoId),
	                                    GenerateInParam("@shipconame", SqlDbType.NVarChar, 30, shipCoName),
	                                    GenerateInParam("@shipsn", SqlDbType.VarChar, 30, shipSN),
                                        GenerateInParam("@regionid", SqlDbType.SmallInt, 2, regionId),
                                        GenerateInParam("@consignee", SqlDbType.NVarChar, 20, consignee),
                                        GenerateInParam("@mobile", SqlDbType.VarChar, 15, mobile),
                                        GenerateInParam("@phone", SqlDbType.VarChar, 12, phone),
                                        GenerateInParam("@email", SqlDbType.VarChar, 50, email),
                                        GenerateInParam("@zipcode", SqlDbType.Char, 6, zipCode),
	                                    GenerateInParam("@address", SqlDbType.NVarChar, 150, address),
                                        GenerateInParam("@sendtime", SqlDbType.DateTime, 8, sendTime)
                                    };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}sendorderafterservice", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        /// <summary>
        /// 商城收到客户邮寄的商品
        /// </summary>
        /// <param name="asId">售后服务id</param>
        /// <param name="state">状态</param>
        /// <param name="receiveTime">收货时间</param>
        public void ReceiveOrderAfterService(int asId, OrderAfterServiceState state, DateTime receiveTime)
        {
            DbParameter[] parms = {
	                                    GenerateInParam("@asid", SqlDbType.Int, 4, asId),
	                                    GenerateInParam("@state", SqlDbType.TinyInt, 1, (int)state),
                                        GenerateInParam("@receivetime", SqlDbType.DateTime, 8, receiveTime)
                                    };
            string commandText = string.Format("UPDATE [{0}orderafterservices] SET [state]=@state,[receivetime]=@receivetime WHERE [asid]=@asid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 邮寄给客户
        /// </summary>
        /// <param name="asId">售后服务id</param>
        /// <param name="state">状态</param>
        /// <param name="shipCoId">配送公司id</param>
        /// <param name="shipCoName">配送公司名称</param>
        /// <param name="shipSN">配送单号</param>
        /// <param name="backTime">邮寄给客户时间</param>
        public void BackOrderAfterService(int asId, OrderAfterServiceState state, int shipCoId, string shipCoName, string shipSN, DateTime backTime)
        {
            DbParameter[] parms = {
	                                    GenerateInParam("@asid", SqlDbType.Int, 4, asId),
	                                    GenerateInParam("@state", SqlDbType.TinyInt, 1, (int)state),
	                                    GenerateInParam("@shipcoid", SqlDbType.SmallInt, 2, shipCoId),
	                                    GenerateInParam("@shipconame", SqlDbType.NVarChar, 30, shipCoName),
	                                    GenerateInParam("@shipsn", SqlDbType.VarChar, 30, shipSN),
                                        GenerateInParam("@backtime", SqlDbType.DateTime, 8, backTime)
                                    };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}backorderafterservice", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        /// <summary>
        /// 订单售后服务完成
        /// </summary>
        /// <param name="asId">售后服务id</param>
        /// <param name="state">状态</param>
        public void CompleteOrderAfterService(int asId, OrderAfterServiceState state)
        {
            DbParameter[] parms = {
	                                    GenerateInParam("@asid", SqlDbType.Int, 4, asId),
	                                    GenerateInParam("@state", SqlDbType.TinyInt, 1, (int)state)
                                    };
            string commandText = string.Format("UPDATE [{0}orderafterservices] SET [state]=@state WHERE [asid]=@asid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 获得订单售后服务列表
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <returns></returns>
        public IDataReader GetOrderAfterServiceList(int oid)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@oid", SqlDbType.Int, 4, oid)
                                    };
            return RDBSHelper.ExecuteReader(CommandType.StoredProcedure,
                                            string.Format("{0}getorderafterservicelist", RDBSHelper.RDBSTablePre),
                                            parms);
        }

        /// <summary>
        /// 获得订单商品售后服务列表
        /// </summary>
        /// <param name="recordId">记录id</param>
        /// <returns></returns>
        public IDataReader GetOrderProductAfterServiceList(int recordId)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@recordid", SqlDbType.Int, 4, recordId)
                                    };
            return RDBSHelper.ExecuteReader(CommandType.StoredProcedure,
                                            string.Format("{0}getorderproductafterservicelist", RDBSHelper.RDBSTablePre),
                                            parms);
        }

        /// <summary>
        /// 获得订单商品售后服务列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public DataTable GetOrderProductAfterServiceList(int pageSize, int pageNumber, string condition)
        {
            bool noCondition = string.IsNullOrWhiteSpace(condition);
            string commandText;
            if (pageNumber == 1)
            {
                if (noCondition)
                    commandText = string.Format("SELECT TOP {0} [asid],[state],[uid],[oid],[recordid],[pid],[storeid],[pname],[pshowimg],[count],[money],[type],[applytime],[checkresult],[checktime],[regionid],[sendtime],[receivetime],[backtime] FROM [{1}orderafterservices] ORDER BY [asid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre);

                else
                    commandText = string.Format("SELECT TOP {0} [asid],[state],[uid],[oid],[recordid],[pid],[storeid],[pname],[pshowimg],[count],[money],[type],[applytime],[checkresult],[checktime],[regionid],[sendtime],[receivetime],[backtime] FROM [{1}orderafterservices] WHERE {2} ORDER BY [asid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                condition);
            }
            else
            {
                if (noCondition)
                    commandText = string.Format("SELECT TOP {0} [asid],[state],[uid],[oid],[recordid],[pid],[storeid],[pname],[pshowimg],[count],[money],[type],[applytime],[checkresult],[checktime],[regionid],[sendtime],[receivetime],[backtime] FROM [{1}orderafterservices] WHERE [asid] < (SELECT MIN([asid]) FROM (SELECT TOP {2} [asid] FROM [{1}orderafterservices] ORDER BY [asid] DESC) AS [temp1]) ORDER BY [asid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                (pageNumber - 1) * pageSize);
                else
                    commandText = string.Format("SELECT TOP {0} [asid],[state],[uid],[oid],[recordid],[pid],[storeid],[pname],[pshowimg],[count],[money],[type],[applytime],[checkresult],[checktime],[regionid],[sendtime],[receivetime],[backtime] FROM [{1}orderafterservices] WHERE {3} AND [asid] < (SELECT MIN([asid]) FROM (SELECT TOP {2} [asid] FROM [{1}orderafterservices] WHERE {3} ORDER BY [asid] DESC) AS [temp1]) ORDER BY [asid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                (pageNumber - 1) * pageSize,
                                                condition);
            }

            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 获得订单商品售后服务数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public int GetOrderProductAfterServiceCount(string condition)
        {
            string commandText;
            if (string.IsNullOrWhiteSpace(condition))
                commandText = string.Format("SELECT COUNT([asid]) FROM [{0}orderafterservices]", RDBSHelper.RDBSTablePre);
            else
                commandText = string.Format("SELECT COUNT([asid]) FROM [{0}orderafterservices] WHERE {1}", RDBSHelper.RDBSTablePre, condition);

            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText));
        }

        /// <summary>
        /// 获得订单商品售后服务列表搜索条件
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="uid">用户id</param>
        /// <param name="oid">订单id</param>
        /// <param name="state">状态</param>
        /// <param name="type">类型</param>
        /// <param name="applyStartTime">申请开始时间</param>
        /// <param name="applyEndTime">申请结束时间</param>
        /// <returns></returns>
        public string GetOrderProductAfterServiceListCondition(int storeId, int uid, int oid, int state, int type, string applyStartTime, string applyEndTime)
        {
            StringBuilder condition = new StringBuilder();

            if (storeId > 0)
                condition.AppendFormat(" AND [storeid]={0} ", storeId);
            if (uid > 0)
                condition.AppendFormat(" AND [uid]={0} ", uid);
            if (oid > 0)
                condition.AppendFormat(" AND [oid]={0} ", oid);
            if (state > -1)
                condition.AppendFormat(" AND [state]={0} ", state);
            if (type > -1)
                condition.AppendFormat(" AND [type]={0} ", type);
            if (!string.IsNullOrEmpty(applyStartTime))
                condition.AppendFormat(" AND [starttime] >= '{0}' ", TypeHelper.StringToDateTime(applyStartTime).ToString("yyyy-MM-dd HH:mm:ss"));
            if (!string.IsNullOrEmpty(applyEndTime))
                condition.AppendFormat(" AND [starttime] <= '{0}' ", TypeHelper.StringToDateTime(applyEndTime).ToString("yyyy-MM-dd HH:mm:ss"));

            return condition.Length > 0 ? condition.Remove(0, 4).ToString() : "";
        }

        /// <summary>
        /// 获得订单售后服务
        /// </summary>
        /// <param name="asId">售后服务id</param>
        /// <returns></returns>
        public IDataReader GetOrderAfterServiceByASId(int asId)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@asid", SqlDbType.Int, 4, asId)
                                    };
            return RDBSHelper.ExecuteReader(CommandType.StoredProcedure,
                                            string.Format("{0}getorderafterservicebyasid", RDBSHelper.RDBSTablePre),
                                            parms);
        }

        #endregion

        #region 订单退款

        /// <summary>
        /// 申请退款
        /// </summary>
        /// <param name="orderRefundInfo">订单退款信息</param>
        public void ApplyRefund(OrderRefundInfo orderRefundInfo)
        {
            DbParameter[] parms = {
	                                GenerateInParam("@state", SqlDbType.TinyInt,1 ,orderRefundInfo.State),
	                                GenerateInParam("@storeid", SqlDbType.Int, 4, orderRefundInfo.StoreId),
	                                GenerateInParam("@storename", SqlDbType.NVarChar,60,orderRefundInfo.StoreName),
	                                GenerateInParam("@oid", SqlDbType.Int, 4, orderRefundInfo.Oid),
	                                GenerateInParam("@osn", SqlDbType.VarChar,30,orderRefundInfo.OSN),
	                                GenerateInParam("@uid", SqlDbType.Int,4 ,orderRefundInfo.Uid),
	                                GenerateInParam("@asid", SqlDbType.Int,4 ,orderRefundInfo.ASId),
                                    GenerateInParam("@paysystemname", SqlDbType.VarChar,20 ,orderRefundInfo.PaySystemName),
                                    GenerateInParam("@payfriendname", SqlDbType.NVarChar,30 ,orderRefundInfo.PayFriendName),
                                    GenerateInParam("@paysn", SqlDbType.VarChar,30 ,orderRefundInfo.PaySN),
	                                GenerateInParam("@paymoney", SqlDbType.Decimal,8,orderRefundInfo.PayMoney),
	                                GenerateInParam("@refundmoney", SqlDbType.Decimal,8,orderRefundInfo.RefundMoney),
	                                GenerateInParam("@applytime", SqlDbType.DateTime,8,orderRefundInfo.ApplyTime),
                                    GenerateInParam("@refundsn", SqlDbType.VarChar,30 ,orderRefundInfo.RefundSN),
                                    GenerateInParam("@refundtime", SqlDbType.DateTime,8 ,orderRefundInfo.RefundTime)
                                   };
            string commandText = string.Format("INSERT INTO [{0}orderrefunds]([state],[storeid],[storename],[oid],[osn],[uid],[asid],[paysystemname],[payfriendname],[paysn],[paymoney],[refundmoney],[applytime],[refundsn],[refundtime]) VALUES(@state,@storeid,@storename,@oid,@osn,@uid,@asid,@paysystemname,@payfriendname,@paysn,@paymoney,@refundmoney,@applytime,@refundsn,@refundtime)",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="refundId">退款id</param>
        /// <param name="state">状态</param>
        /// <param name="refundSN">退款单号</param>
        /// <param name="refundTime">退款时间</param>
        public void RefundOrder(int refundId, OrderRefundState state, string refundSN, DateTime refundTime)
        {
            DbParameter[] parms = {
	                                GenerateInParam("@refundid", SqlDbType.Int, 4, refundId),
	                                GenerateInParam("@state", SqlDbType.TinyInt, 1, (int)state),
                                    GenerateInParam("@refundsn", SqlDbType.VarChar,30 ,refundSN),
                                    GenerateInParam("@refundtime", SqlDbType.DateTime,8 ,refundTime)
                                   };
            string commandText = string.Format("UPDATE [{0}orderrefunds] SET [state]=@state,[refundsn]=@refundsn,[refundtime]=@refundtime WHERE [refundid]=@refundid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 获得订单退款列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public IDataReader GetOrderRefundList(int pageSize, int pageNumber, string condition)
        {
            bool noCondition = string.IsNullOrWhiteSpace(condition);
            string commandText;
            if (pageNumber == 1)
            {
                if (noCondition)
                    commandText = string.Format("SELECT TOP {0} {2} FROM [{1}orderrefunds] ORDER BY [refundid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                TableFields.ORDER_REFUNDS);

                else
                    commandText = string.Format("SELECT TOP {0} {3} FROM [{1}orderrefunds] WHERE {2} ORDER BY [refundid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                condition,
                                                TableFields.ORDER_REFUNDS);
            }
            else
            {
                if (noCondition)
                    commandText = string.Format("SELECT TOP {0} {3} FROM [{1}orderrefunds] WHERE [refundid] < (SELECT MIN([refundid]) FROM (SELECT TOP {2} [refundid] FROM [{1}orderrefunds] ORDER BY [refundid] DESC) AS [temp]) ORDER BY [refundid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                (pageNumber - 1) * pageSize,
                                                TableFields.ORDER_REFUNDS);
                else
                    commandText = string.Format("SELECT TOP {0} {4} FROM [{1}orderrefunds] WHERE [refundid] < (SELECT MIN([refundid]) FROM (SELECT TOP {2} [refundid] FROM [{1}orderrefunds] WHERE {3} ORDER BY [refundid] DESC) AS [temp]) AND {3} ORDER BY [refundid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                (pageNumber - 1) * pageSize,
                                                condition,
                                                TableFields.ORDER_REFUNDS);
            }

            return RDBSHelper.ExecuteReader(CommandType.Text, commandText);
        }

        /// <summary>
        /// 获得订单退款列表条件
        /// </summary>
        /// <param name="storeId">店铺id</param>
        /// <param name="osn">订单编号</param>
        /// <returns></returns>
        public string GetOrderRefundListCondition(int storeId, string osn)
        {
            StringBuilder condition = new StringBuilder();

            if (storeId > 0)
                condition.AppendFormat(" AND [storeid] = {0} ", storeId);
            if (!string.IsNullOrWhiteSpace(osn) && SecureHelper.IsSafeSqlString(osn))
                condition.AppendFormat(" AND [osn] like '{0}%' ", osn);

            return condition.Length > 0 ? condition.Remove(0, 4).ToString() : "";
        }

        /// <summary>
        /// 获得订单退款数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public int GetOrderRefundCount(string condition)
        {
            string commandText;
            if (string.IsNullOrWhiteSpace(condition))
                commandText = string.Format("SELECT COUNT([refundid]) FROM [{0}orderrefunds]", RDBSHelper.RDBSTablePre);
            else
                commandText = string.Format("SELECT COUNT([refundid]) FROM [{0}orderrefunds] WHERE {1}", RDBSHelper.RDBSTablePre, condition);

            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText), 0);
        }

        #endregion
    }
}
