using System;
using System.Data;

using BrnMall.Core;

namespace BrnMall.Data
{
    /// <summary>
    /// 商品收藏夹数据访问类
    /// </summary>
    public partial class FavoriteProducts
    {
        /// <summary>
        /// 将商品添加到收藏夹
        /// </summary>
        /// <returns></returns>
        public static bool AddProductToFavorite(int uid, int pid, int state, DateTime addTime)
        {
            return BrnMall.Core.BMAData.RDBS.AddProductToFavorite(uid, pid, state, addTime);
        }

        /// <summary>
        /// 删除收藏夹的商品
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="pid">商品id</param>
        /// <returns></returns>
        public static bool DeleteFavoriteProductByUidAndPid(int uid, int pid)
        {
            return BrnMall.Core.BMAData.RDBS.DeleteFavoriteProductByUidAndPid(uid, pid);
        }

        /// <summary>
        /// 商品是否已经收藏
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="pid">商品id</param>
        public static bool IsExistFavoriteProduct(int uid, int pid)
        {
            return BrnMall.Core.BMAData.RDBS.IsExistFavoriteProduct(uid, pid);
        }

        /// <summary>
        /// 获得收藏夹商品列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="uid">用户id</param>
        /// <param name="storeName">店铺名称</param>
        /// <param name="productName">商品名称</param>
        /// <returns></returns>
        public static DataTable GetFavoriteProductList(int pageSize, int pageNumber, int uid, string storeName, string productName)
        {
            return BrnMall.Core.BMAData.RDBS.GetFavoriteProductList(pageSize, pageNumber, uid, storeName, productName);
        }

        /// <summary>
        /// 获得收藏夹商品列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public static DataTable GetFavoriteProductList(int pageSize, int pageNumber, int uid)
        {
            return BrnMall.Core.BMAData.RDBS.GetFavoriteProductList(pageSize, pageNumber, uid);
        }

        /// <summary>
        /// 获得收藏夹商品数量
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="storeName">店铺名称</param>
        /// <param name="productName">商品名称</param>
        /// <returns></returns>
        public static int GetFavoriteProductCount(int uid, string storeName, string productName)
        {
            return BrnMall.Core.BMAData.RDBS.GetFavoriteProductCount(uid, storeName, productName);
        }

        /// <summary>
        /// 获得收藏夹商品数量
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public static int GetFavoriteProductCount(int uid)
        {
            return BrnMall.Core.BMAData.RDBS.GetFavoriteProductCount(uid);
        }

        /// <summary>
        /// 设置收藏夹商品状态
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="pid">商品id</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public static bool SetFavoriteProductState(int uid, int pid, int state)
        {
            return BrnMall.Core.BMAData.RDBS.SetFavoriteProductState(uid, pid, state);
        }
    }
}
