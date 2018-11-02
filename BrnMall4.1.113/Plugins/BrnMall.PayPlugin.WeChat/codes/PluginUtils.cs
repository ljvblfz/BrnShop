using System;

using BrnMall.Core;

namespace BrnMall.PayPlugin.WeChat
{
    /// <summary>
    /// 插件工具类
    /// </summary>
    public class PluginUtils
    {
        private static object _locker = new object();//锁对象
        private static PluginSetInfo _pluginsetinfo = null;//插件设置信息
        private static string _dbfilepath = "/plugins/BrnMall.PayPlugin.WeChat/db.config";//数据文件路径

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
                PayConfig.ReSet();
                BrnMall.PayPlugin.OpenWeChat.PayConfig.ReSet();
            }
        }
    }

    /// <summary>
    /// 插件设置信息类
    /// </summary>
    public class PluginSetInfo
    {
        private string _wpmchid;
        private string _wpappid;
        private string _wpappsecret;
        private string _wpappkey;
        private string _openmchid;
        private string _openappid;
        private string _openappsecret;
        private string _openappkey;

        public string WPMchId
        {
            get { return _wpmchid; }
            set { _wpmchid = value; }
        }
        public string WPAppId
        {
            get { return _wpappid; }
            set { _wpappid = value; }
        }
        public string WPAppSecret
        {
            get { return _wpappsecret; }
            set { _wpappsecret = value; }
        }
        public string WPAppKey
        {
            get { return _wpappkey; }
            set { _wpappkey = value; }
        }
        public string OpenMchId
        {
            get { return _openmchid; }
            set { _openmchid = value; }
        }
        public string OpenAppId
        {
            get { return _openappid; }
            set { _openappid = value; }
        }
        public string OpenAppSecret
        {
            get { return _openappsecret; }
            set { _openappsecret = value; }
        }
        public string OpenAppKey
        {
            get { return _openappkey; }
            set { _openappkey = value; }
        }
    }
}
