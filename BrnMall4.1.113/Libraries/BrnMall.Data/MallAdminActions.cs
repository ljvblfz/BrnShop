using System;
using System.Data;
using System.Collections.Generic;

using BrnMall.Core;

namespace BrnMall.Data
{
    /// <summary>
    /// 商城后台操作数据访问类
    /// </summary>
    public partial class MallAdminActions
    {
        /// <summary>
        /// 获得商城后台操作列表
        /// </summary>
        /// <returns></returns>
        public static List<MallAdminActionInfo> GetMallAdminActionList()
        {
            List<MallAdminActionInfo> mallAdminActionList = new List<MallAdminActionInfo>();
            IDataReader reader = BrnMall.Core.BMAData.RDBS.GetMallAdminActionList();
            while (reader.Read())
            {
                MallAdminActionInfo mallAdminActionInfo = new MallAdminActionInfo();
                mallAdminActionInfo.Aid = TypeHelper.ObjectToInt(reader["aid"]);
                mallAdminActionInfo.Title = reader["title"].ToString();
                mallAdminActionInfo.Action = reader["action"].ToString();
                mallAdminActionInfo.ParentId = TypeHelper.ObjectToInt(reader["parentid"]);
                mallAdminActionInfo.DisplayOrder = TypeHelper.ObjectToInt(reader["displayorder"]);
                mallAdminActionList.Add(mallAdminActionInfo);
            }
            reader.Close();
            return mallAdminActionList;
        }
    }
}
