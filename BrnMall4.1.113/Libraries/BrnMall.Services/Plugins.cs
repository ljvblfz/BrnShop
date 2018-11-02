using System;
using System.Collections.Generic;

using BrnMall.Core;

namespace BrnMall.Services
{
    /// <summary>
    /// 插件操作管理类
    /// </summary>
    public partial class Plugins
    {
        /// <summary>
        /// 获得默认开放授权插件
        /// </summary>
        /// <returns></returns>
        public static PluginInfo GetDefaultOAuthPlugin()
        {
            List<PluginInfo> oAuthPluginList = GetOAuthPluginList();

            if (oAuthPluginList.Count == 0)
                return null;

            foreach (PluginInfo pluginInfo in oAuthPluginList)
            {
                if (pluginInfo.IsDefault == 1)
                    return pluginInfo;
            }

            return oAuthPluginList[0];
        }

        /// <summary>
        /// 获得默认支付插件
        /// </summary>
        /// <returns></returns>
        public static PluginInfo GetDefaultPayPlugin()
        {
            List<PluginInfo> payPluginList = GetPayPluginList();

            if (payPluginList.Count == 0)
                return null;

            foreach (PluginInfo pluginInfo in payPluginList)
            {
                if (pluginInfo.IsDefault == 1)
                    return pluginInfo;
            }

            return payPluginList[0];
        }

        /// <summary>
        /// 获得开放授权插件列表
        /// </summary>
        /// <returns></returns>
        public static List<PluginInfo> GetOAuthPluginList()
        {
            return BMAPlugin.OAuthPluginList;
        }

        /// <summary>
        /// 获得支付插件列表
        /// </summary>
        /// <returns></returns>
        public static List<PluginInfo> GetPayPluginList()
        {
            return BMAPlugin.PayPluginList;
        }

        /// <summary>
        /// 获得开放授权插件
        /// </summary>
        /// <param name="systemName">插件系统名称</param>
        /// <returns></returns>
        public static PluginInfo GetOAuthPluginBySystemName(string systemName)
        {
            if (!string.IsNullOrWhiteSpace(systemName))
            {
                foreach (PluginInfo info in GetOAuthPluginList())
                {
                    if (info.SystemName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase))
                        return info;
                }
            }

            return null;
        }

        /// <summary>
        /// 获得支付插件
        /// </summary>
        /// <param name="systemName">插件系统名称</param>
        /// <returns></returns>
        public static PluginInfo GetPayPluginBySystemName(string systemName)
        {
            if (!string.IsNullOrWhiteSpace(systemName))
            {
                foreach (PluginInfo info in GetPayPluginList())
                {
                    if (info.SystemName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase))
                        return info;
                }
            }

            return null;
        }
    }
}
