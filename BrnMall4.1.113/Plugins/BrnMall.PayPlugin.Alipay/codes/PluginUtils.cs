using System;

using BrnMall.Core;

namespace BrnMall.PayPlugin.Alipay
{
    /// <summary>
    /// 插件工具类
    /// </summary>
    public class PluginUtils
    {
        private static object _locker = new object();//锁对象
        private static PluginSetInfo _pluginsetinfo = null;//插件设置信息
        private static string _dbfilepath = "/plugins/BrnMall.PayPlugin.Alipay/db.config";//数据文件路径

        /// <summary>
        ///获得插件设置
        /// </summary>
        /// <returns></returns>
        public static PluginSetInfo GetPluginSet()
        {
            if (_pluginsetinfo == null)
            {
                lock (_locker)
                {
                    if (_pluginsetinfo == null)
                    {
                        _pluginsetinfo = (PluginSetInfo)IOHelper.DeserializeFromXML(typeof(PluginSetInfo), IOHelper.GetMapPath(_dbfilepath));
                    }
                }
            }
            return _pluginsetinfo;
        }

        /// <summary>
        /// 保存插件设置
        /// </summary>
        public static void SavePluginSet(PluginSetInfo pluginSetInfo)
        {
            lock (_locker)
            {
                IOHelper.SerializeToXml(pluginSetInfo, IOHelper.GetMapPath(_dbfilepath));
                _pluginsetinfo = null;
                AlipayConfig.ReSet();
                Com.Alipay.Config.ReSet();
            }
        }
    }

    /// <summary>
    /// 插件设置信息类
    /// </summary>
    public class PluginSetInfo
    {
        private string _partner;//合作者身份ID
        private string _key;//交易安全检验码
        private string _seller;//收款支付宝帐户
        private string _privatekey;//私钥

        /// <summary>
        /// 合作者身份ID
        /// </summary>
        public string Partner
        {
            get { return _partner; }
            set { _partner = value; }
        }
        /// <summary>
        /// 交易安全检验码
        /// </summary>
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }
        /// <summary>
        /// 收款支付宝帐户
        /// </summary>
        public string Seller
        {
            get { return _seller; }
            set { _seller = value; }
        }
        /// <summary>
        /// 私钥
        /// </summary>
        public string PrivateKey
        {
            get { return _privatekey; }
            set { _privatekey = value; }
        }
    }
}
