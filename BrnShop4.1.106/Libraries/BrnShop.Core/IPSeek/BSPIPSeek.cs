using System;
using System.IO;

namespace BrnShop.Core
{
    /// <summary>
    /// BrnShopIP查找管理类
    /// </summary>
    public class BSPIPSeek
    {
        private static IIPSeekStrategy _iipseekstrategy = null;//ip查找策略

        static BSPIPSeek()
        {
            try
            {
                string[] fileNameList = Directory.GetFiles(System.Web.HttpRuntime.BinDirectory, "BrnShop.IPSeekStrategy.*.dll", SearchOption.TopDirectoryOnly);
                _iipseekstrategy = (IIPSeekStrategy)Activator.CreateInstance(Type.GetType(string.Format("BrnShop.IPSeekStrategy.{0}.IPSeekStrategy, BrnShop.IPSeekStrategy.{0}", fileNameList[0].Substring(fileNameList[0].IndexOf("IPSeekStrategy.") + 15).Replace(".dll", "")),
                                                                                          false,
                                                                                          true));
            }
            catch
            {
                throw new BSPException("创建'IP查找策略对象'失败,可能存在的原因:未将'IP查找策略程序集'添加到bin目录中;'IP查找策略程序集'文件名不符合'BrnShop.IPSeekStrategy.{策略名称}.dll'格式");
            }
        }

        /// <summary>
        /// IP查找策略实例
        /// </summary>
        public static IIPSeekStrategy Instance
        {
            get { return _iipseekstrategy; }
        }
    }
}
