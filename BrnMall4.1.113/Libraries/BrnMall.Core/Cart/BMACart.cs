using System;
using System.IO;

namespace BrnMall.Core
{
    /// <summary>
    /// BrnMall购物车管理类
    /// </summary>
    public class BMACart
    {
        private static ICartStrategy _icartstrategy = null;//购物车策略

        static BMACart()
        {
            try
            {
                string[] fileNameList = Directory.GetFiles(System.Web.HttpRuntime.BinDirectory, "BrnMall.CartStrategy.*.dll", SearchOption.TopDirectoryOnly);
                _icartstrategy = (ICartStrategy)Activator.CreateInstance(Type.GetType(string.Format("BrnMall.CartStrategy.{0}.CartStrategy, BrnMall.CartStrategy.{0}", fileNameList[0].Substring(fileNameList[0].IndexOf("CartStrategy.") + 13).Replace(".dll", "")),
                                                                                      false,
                                                                                      true));
            }
            catch
            {
                throw new BMAException("创建'购物车策略对象'失败,可能存在的原因:未将'购物车策略程序集'添加到bin目录中;'购物车策略程序集'文件名不符合'BrnMall.CartStrategy.{策略名称}.dll'格式");
            }
        }

        /// <summary>
        /// 购物车策略实例
        /// </summary>
        public static ICartStrategy Instance
        {
            get { return _icartstrategy; }
        }
    }
}
