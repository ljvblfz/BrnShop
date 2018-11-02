using System;
using System.Data;
using System.Collections.Generic;

using BrnShop.Core;

namespace BrnShop.Data
{
    /// <summary>
    /// 配送公司数据访问类
    /// </summary>
    public partial class ShipCompanies
    {
        #region 辅助方法

        /// <summary>
        /// 通过IDataReader创建ShipCompanyInfo
        /// </summary>
        public static ShipCompanyInfo BuildShipCompanyFromReader(IDataReader reader)
        {
            ShipCompanyInfo shipCompanyInfo = new ShipCompanyInfo();

            shipCompanyInfo.ShipCoId = TypeHelper.ObjectToInt(reader["shipcoid"]);
            shipCompanyInfo.Name = reader["name"].ToString();
            shipCompanyInfo.DisplayOrder = TypeHelper.ObjectToInt(reader["displayorder"]);

            return shipCompanyInfo;
        }

        #endregion

        /// <summary>
        /// 获得配送公司列表
        /// </summary>
        /// <returns></returns>
        public static List<ShipCompanyInfo> GetShipCompanyList()
        {
            List<ShipCompanyInfo> shipCompanyList = new List<ShipCompanyInfo>();
            IDataReader reader = BrnShop.Core.BSPData.RDBS.GetShipCompanyList();
            while (reader.Read())
            {
                ShipCompanyInfo shipCompanyInfo = BuildShipCompanyFromReader(reader);
                shipCompanyList.Add(shipCompanyInfo);
            }

            reader.Close();
            return shipCompanyList;
        }

        /// <summary>
        /// 创建配送公司
        /// </summary>
        public static void CreateShipCompany(ShipCompanyInfo shipCompanyInfo)
        {
            BrnShop.Core.BSPData.RDBS.CreateShipCompany(shipCompanyInfo);
        }

        /// <summary>
        /// 更新配送公司
        /// </summary>
        public static void UpdateShipCompany(ShipCompanyInfo shipCompanyInfo)
        {
            BrnShop.Core.BSPData.RDBS.UpdateShipCompany(shipCompanyInfo);
        }

        /// <summary>
        /// 删除配送公司
        /// </summary>
        /// <param name="shipCoId">配送公司id</param>
        public static void DeleteShipCompanyById(int shipCoId)
        {
            BrnShop.Core.BSPData.RDBS.DeleteShipCompanyById(shipCoId);
        }
    }
}
