using System;
using System.IO;

namespace BrnMall.Core
{
    /// <summary>
    /// BrnMall随机性管理类
    /// </summary>
    public class BMARandom
    {
        private static IRandomStrategy _irandomstrategy = null;//随机性策略

        static BMARandom()
        {
            try
            {
                string[] fileNameList = Directory.GetFiles(System.Web.HttpRuntime.BinDirectory, "BrnMall.RandomStrategy.*.dll", SearchOption.TopDirectoryOnly);
                _irandomstrategy = (IRandomStrategy)Activator.CreateInstance(Type.GetType(string.Format("BrnMall.RandomStrategy.{0}.RandomStrategy, BrnMall.RandomStrategy.{0}", fileNameList[0].Substring(fileNameList[0].IndexOf("RandomStrategy.") + 15).Replace(".dll", "")),
                                                                                         false,
                                                                                         true));
            }
            catch
            {
                throw new BMAException("创建'随机性策略对象'失败,可能存在的原因:未将'随机性策略程序集'添加到bin目录中;'随机性策略程序集'文件名不符合'BrnMall.RandomStrategy.{策略名称}.dll'格式");
            }
        }

        /// <summary>
        /// 随机性策略实例
        /// </summary>
        public static IRandomStrategy Instance
        {
            get { return _irandomstrategy; }
        }
    }
}
