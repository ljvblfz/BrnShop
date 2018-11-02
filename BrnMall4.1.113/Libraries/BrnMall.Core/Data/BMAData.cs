using System;
using System.IO;

namespace BrnMall.Core
{
    /// <summary>
    /// BrnMall数据管理类
    /// </summary>
    public partial class BMAData
    {
        private static IRDBSStrategy _irdbsstrategy = null;//关系型数据库策略

        private static object _locker = new object();//锁对象
        private static bool _enablednosql = false;//是否启用非关系型数据库
        private static IUserNOSQLStrategy _iusernosqlstrategy = null;//用户非关系型数据库策略
        private static IProductNOSQLStrategy _iproductnosqlstrategy = null;//商品非关系型数据库策略
        private static IStoreNOSQLStrategy _istorenosqlstrategy = null;//店铺非关系型数据库策略
        private static IOrderNOSQLStrategy _iordernosqlstrategy = null;//订单非关系型数据库策略

        static BMAData()
        {
            try
            {
                string[] fileNameList = Directory.GetFiles(System.Web.HttpRuntime.BinDirectory, "BrnMall.RDBSStrategy.*.dll", SearchOption.TopDirectoryOnly);
                _irdbsstrategy = (IRDBSStrategy)Activator.CreateInstance(Type.GetType(string.Format("BrnMall.RDBSStrategy.{0}.RDBSStrategy, BrnMall.RDBSStrategy.{0}", fileNameList[0].Substring(fileNameList[0].LastIndexOf("RDBSStrategy.") + 13).Replace(".dll", "")),
                                                                                            false,
                                                                                            true));
            }
            catch
            {
                throw new BMAException("创建'关系数据库策略对象'失败,可能存在的原因:未将'关系数据库策略程序集'添加到bin目录中;'关系数据库策略程序集'文件名不符合'BrnMall.RDBSStrategy.{策略名称}.dll'格式");
            }
            _enablednosql = Directory.GetFiles(System.Web.HttpRuntime.BinDirectory, "BrnMall.NOSQLStrategy.*.dll", SearchOption.TopDirectoryOnly).Length > 0;
        }

        /// <summary>
        /// 关系型数据库
        /// </summary>
        public static IRDBSStrategy RDBS
        {
            get { return _irdbsstrategy; }
        }

        /// <summary>
        /// 用户非关系型数据库
        /// </summary>
        public static IUserNOSQLStrategy UserNOSQL
        {
            get
            {
                if (_enablednosql && BMAConfig.RedisNOSQLConfig.User.Enabled == 1)
                {
                    if (_iusernosqlstrategy == null)
                    {
                        lock (_locker)
                        {
                            if (_iusernosqlstrategy == null)
                            {
                                try
                                {
                                    string[] fileNameList = Directory.GetFiles(System.Web.HttpRuntime.BinDirectory, "BrnMall.NOSQLStrategy.*.dll", SearchOption.TopDirectoryOnly);
                                    _iusernosqlstrategy = (IUserNOSQLStrategy)Activator.CreateInstance(Type.GetType(string.Format("BrnMall.NOSQLStrategy.{0}.UserNOSQLStrategy, BrnMall.NOSQLStrategy.{0}", fileNameList[0].Substring(fileNameList[0].LastIndexOf("NOSQLStrategy.") + 14).Replace(".dll", "")),
                                                                                                                          false,
                                                                                                                          true));
                                }
                                catch
                                {
                                    throw new BMAException("创建'用户非关系数据库策略对象'失败,可能存在的原因:未将'用户非关系数据库策略程序集'添加到bin目录中;'用户非关系数据库策略程序集'文件名不符合'BrnMall.NOSQLStrategy.{策略名称}.dll'格式");
                                }
                            }
                        }
                    }
                }
                return _iusernosqlstrategy;
            }
        }

        /// <summary>
        /// 商品非关系型数据库
        /// </summary>
        public static IProductNOSQLStrategy ProductNOSQL
        {
            get
            {
                if (_enablednosql && BMAConfig.RedisNOSQLConfig.Product.Enabled == 1)
                {
                    if (_iproductnosqlstrategy == null)
                    {
                        lock (_locker)
                        {
                            if (_iproductnosqlstrategy == null)
                            {
                                try
                                {
                                    string[] fileNameList = Directory.GetFiles(System.Web.HttpRuntime.BinDirectory, "BrnMall.NOSQLStrategy.*.dll", SearchOption.TopDirectoryOnly);
                                    _iproductnosqlstrategy = (IProductNOSQLStrategy)Activator.CreateInstance(Type.GetType(string.Format("BrnMall.NOSQLStrategy.{0}.ProductNOSQLStrategy, BrnMall.NOSQLStrategy.{0}", fileNameList[0].Substring(fileNameList[0].LastIndexOf("NOSQLStrategy.") + 14).Replace(".dll", "")),
                                                                                                                                false,
                                                                                                                                true));
                                }
                                catch
                                {
                                    throw new BMAException("创建'商品非关系数据库策略对象'失败,可能存在的原因:未将'商品非关系数据库策略程序集'添加到bin目录中;'商品非关系数据库策略程序集'文件名不符合'BrnMall.NOSQLStrategy.{策略名称}.dll'格式");
                                }
                            }
                        }
                    }
                }
                return _iproductnosqlstrategy;
            }
        }

        /// <summary>
        /// 店铺非关系型数据库
        /// </summary>
        public static IStoreNOSQLStrategy StoreNOSQL
        {
            get
            {
                if (_enablednosql && BMAConfig.RedisNOSQLConfig.Store.Enabled == 1)
                {
                    if (_istorenosqlstrategy == null)
                    {
                        lock (_locker)
                        {
                            if (_istorenosqlstrategy == null)
                            {
                                try
                                {
                                    string[] fileNameList = Directory.GetFiles(System.Web.HttpRuntime.BinDirectory, "BrnMall.NOSQLStrategy.*.dll", SearchOption.TopDirectoryOnly);
                                    _istorenosqlstrategy = (IStoreNOSQLStrategy)Activator.CreateInstance(Type.GetType(string.Format("BrnMall.NOSQLStrategy.{0}.StoreNOSQLStrategy, BrnMall.NOSQLStrategy.{0}", fileNameList[0].Substring(fileNameList[0].LastIndexOf("NOSQLStrategy.") + 14).Replace(".dll", "")),
                                                                                                                            false,
                                                                                                                            true));
                                }
                                catch
                                {
                                    throw new BMAException("创建'店铺非关系数据库策略对象'失败,可能存在的原因:未将'店铺非关系数据库策略程序集'添加到bin目录中;'店铺非关系数据库策略程序集'文件名不符合'BrnMall.NOSQLStrategy.{策略名称}.dll'格式");
                                }
                            }
                        }
                    }
                }
                return _istorenosqlstrategy;
            }
        }

        /// <summary>
        /// 订单非关系型数据库
        /// </summary>
        public static IOrderNOSQLStrategy OrderNOSQL
        {
            get
            {
                if (_enablednosql && BMAConfig.RedisNOSQLConfig.Order.Enabled == 1)
                {
                    if (_iordernosqlstrategy == null)
                    {
                        lock (_locker)
                        {
                            if (_iordernosqlstrategy == null)
                            {
                                try
                                {
                                    string[] fileNameList = Directory.GetFiles(System.Web.HttpRuntime.BinDirectory, "BrnMall.NOSQLStrategy.*.dll", SearchOption.TopDirectoryOnly);
                                    _iordernosqlstrategy = (IOrderNOSQLStrategy)Activator.CreateInstance(Type.GetType(string.Format("BrnMall.NOSQLStrategy.{0}.OrderNOSQLStrategy, BrnMall.NOSQLStrategy.{0}", fileNameList[0].Substring(fileNameList[0].LastIndexOf("NOSQLStrategy.") + 14).Replace(".dll", "")),
                                                                                                                            false,
                                                                                                                            true));
                                }
                                catch
                                {
                                    throw new BMAException("创建'订单非关系数据库策略对象'失败,可能存在的原因:未将'订单非关系数据库策略程序集'添加到bin目录中;'订单非关系数据库策略程序集'文件名不符合'BrnMall.NOSQLStrategy.{策略名称}.dll'格式");
                                }
                            }
                        }
                    }
                }
                return _iordernosqlstrategy;
            }
        }
    }
}
