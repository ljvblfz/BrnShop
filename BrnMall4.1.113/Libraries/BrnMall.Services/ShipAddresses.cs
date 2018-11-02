using System;
using System.Collections.Generic;

using BrnMall.Core;

namespace BrnMall.Services
{
    /// <summary>
    /// 用户配送地址操作管理类
    /// </summary>
    public partial class ShipAddresses
    {
        /// <summary>
        /// 创建用户配送地址
        /// </summary>
        public static int CreateShipAddress(ShipAddressInfo shipAddressInfo)
        {
            return BrnMall.Data.ShipAddresses.CreateShipAddress(shipAddressInfo);
        }

        /// <summary>
        /// 更新用户配送地址
        /// </summary>
        public static void UpdateShipAddress(ShipAddressInfo shipAddressInfo)
        {
            BrnMall.Data.ShipAddresses.UpdateShipAddress(shipAddressInfo);
        }

        /// <summary>
        /// 获得完整用户配送地址列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public static List<FullShipAddressInfo> GetFullShipAddressList(int uid)
        {
            return BrnMall.Data.ShipAddresses.GetFullShipAddressList(uid);
        }

        /// <summary>
        /// 获得用户配送地址数量
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public static int GetShipAddressCount(int uid)
        {
            return BrnMall.Data.ShipAddresses.GetShipAddressCount(uid);
        }

        /// <summary>
        /// 获得默认完整用户配送地址
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public static FullShipAddressInfo GetDefaultFullShipAddress(int uid)
        {
            return BrnMall.Data.ShipAddresses.GetDefaultFullShipAddress(uid);
        }

        /// <summary>
        /// 获得完整用户配送地址
        /// </summary>
        /// <param name="saId">配送地址id</param>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public static FullShipAddressInfo GetFullShipAddressBySAId(int saId, int uid)
        {
            if (saId < 1)
                return null;

            FullShipAddressInfo fullShipAddressInfo = BrnMall.Data.ShipAddresses.GetFullShipAddressBySAId(uid, saId);
            if (fullShipAddressInfo == null || fullShipAddressInfo.Uid != uid)
                return null;
            else
                return fullShipAddressInfo;
        }

        /// <summary>
        /// 获得用户配送地址
        /// </summary>
        /// <param name="saId">配送地址id</param>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public static ShipAddressInfo GetShipAddressBySAId(int saId, int uid)
        {
            if (saId < 1)
                return null;

            ShipAddressInfo shipAddressInfo = BrnMall.Data.ShipAddresses.GetShipAddressBySAId(uid, saId);
            if (shipAddressInfo == null || shipAddressInfo.Uid != uid)
                return null;
            else
                return shipAddressInfo;
        }

        /// <summary>
        /// 删除用户配送地址
        /// </summary>
        /// <param name="saId">配送地址id</param>
        /// <param name="uid">用户id</param>
        public static bool DeleteShipAddress(int saId, int uid)
        {
            if (saId < 1)
                return false;
            return BrnMall.Data.ShipAddresses.DeleteShipAddress(saId, uid);
        }

        /// <summary>
        /// 更新用户配送地址的默认状态
        /// </summary>
        /// <param name="saId">配送地址id</param>
        /// <param name="uid">用户id</param>
        /// <param name="isDefault">状态</param>
        /// <returns></returns>
        public static bool UpdateShipAddressIsDefault(int saId, int uid, int isDefault)
        {
            if (saId < 1)
                return false;

            if (isDefault != 0) isDefault = 1;
            return BrnMall.Data.ShipAddresses.UpdateShipAddressIsDefault(saId, uid, isDefault);
        }
    }
}
