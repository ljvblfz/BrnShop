using System;
using System.Data;

using BrnMall.Core;

namespace BrnMall.Data
{
    /// <summary>
    /// 商城管理员组数据访问类
    /// </summary>
    public partial class MallAdminGroups
    {
        /// <summary>
        /// 获得商城管理员组列表
        /// </summary>
        /// <returns></returns>
        public static MallAdminGroupInfo[] GetMallAdminGroupList()
        {
            DataTable dt = BrnMall.Core.BMAData.RDBS.GetMallAdminGroupList();
            MallAdminGroupInfo[] mallAdminGroupList = new MallAdminGroupInfo[dt.Rows.Count];
            int index = 0;
            foreach (DataRow dr in dt.Rows)
            {
                MallAdminGroupInfo mallAdminGroupInfo = new MallAdminGroupInfo();
                mallAdminGroupInfo.MallAGid = TypeHelper.ObjectToInt(dr["mallagid"]);
                mallAdminGroupInfo.Title = dr["title"].ToString();
                mallAdminGroupInfo.ActionList = dr["actionlist"].ToString();
                mallAdminGroupList[index] = mallAdminGroupInfo;
                index++;
            }
            return mallAdminGroupList;
        }

        /// <summary>
        /// 创建商城管理员组
        /// </summary>
        /// <param name="mallAdminGroupInfo">商城管理员组信息</param>
        /// <returns></returns>
        public static int CreateMallAdminGroup(MallAdminGroupInfo mallAdminGroupInfo)
        {
            return BrnMall.Core.BMAData.RDBS.CreateMallAdminGroup(mallAdminGroupInfo);
        }

        /// <summary>
        /// 删除商城管理员组
        /// </summary>
        /// <param name="mallAGid">商城管理员组id</param>
        public static void DeleteMallAdminGroupById(int mallAGid)
        {
            BrnMall.Core.BMAData.RDBS.DeleteMallAdminGroupById(mallAGid);
        }

        /// <summary>
        /// 更新商城管理员组
        /// </summary>
        public static void UpdateMallAdminGroup(MallAdminGroupInfo mallAdminGroupInfo)
        {
            BrnMall.Core.BMAData.RDBS.UpdateMallAdminGroup(mallAdminGroupInfo);
        }
    }
}
