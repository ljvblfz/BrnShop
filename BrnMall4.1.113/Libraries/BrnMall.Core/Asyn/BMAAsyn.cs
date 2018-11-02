using System;
using System.IO;

namespace BrnMall.Core
{
    /// <summary>
    /// BrnMall异步管理类
    /// </summary>
    public class BMAAsyn
    {
        private static IAsynStrategy _iasynstrategy = null;//异步策略

        static BMAAsyn()
        {
            try
            {
                string[] fileNameList = Directory.GetFiles(System.Web.HttpRuntime.BinDirectory, "BrnMall.AsynStrategy.*.dll", SearchOption.TopDirectoryOnly);
                _iasynstrategy = (IAsynStrategy)Activator.CreateInstance(Type.GetType(string.Format("BrnMall.AsynStrategy.{0}.AsynStrategy, BrnMall.AsynStrategy.{0}", fileNameList[0].Substring(fileNameList[0].LastIndexOf("AsynStrategy.") + 13).Replace(".dll", "")),
                                                                                      false,
                                                                                      true));
            }
            catch
            {
                throw new BMAException("创建'异步策略对象'失败,可能存在的原因:未将'异步策略程序集'添加到bin目录中;'异步策略程序集'文件名不符合'BrnMall.AsynStrategy.{策略名称}.dll'格式");
            }
        }

        /// <summary>
        /// 异步策略实例
        /// </summary>
        public static IAsynStrategy Instance
        {
            get { return _iasynstrategy; }
        }
    }
}
