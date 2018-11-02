using System;

using BrnShop.Core;

namespace BrnShop.Services
{
    /// <summary>
    /// IP查找操作管理类
    /// </summary>
    public partial class IPSeek
    {
        private static IIPSeekStrategy _iipseekstrategy = BSPIPSeek.Instance;//IP查找策略

        /// <summary>
        /// 根据ip地址确定所在区域
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <returns></returns>
        public static RegionInfo Seek(string ip)
        {
            return _iipseekstrategy.Seek(ip);
        }
    }
}
