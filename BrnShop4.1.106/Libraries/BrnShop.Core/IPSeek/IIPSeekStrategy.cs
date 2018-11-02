using System;

namespace BrnShop.Core
{
    /// <summary>
    /// IP查找策略接口
    /// </summary>
    public partial interface IIPSeekStrategy
    {
        /// <summary>
        /// 根据ip地址确定所在区域
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <returns></returns>
        RegionInfo Seek(string ip);
    }
}
