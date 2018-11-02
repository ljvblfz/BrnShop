using System;
using System.Collections.Generic;

using BrnMall.Core;

namespace BrnMall.Services
{
    public partial class MallAdminActions
    {
        /// <summary>
        /// 获得商城后台操作列表
        /// </summary>
        /// <returns></returns>
        public static List<MallAdminActionInfo> GetMallAdminActionList()
        {
            return BrnMall.Data.MallAdminActions.GetMallAdminActionList();
        }

        /// <summary>
        /// 获得商城后台操作树
        /// </summary>
        /// <returns></returns>
        public static List<MallAdminActionInfo> GetMallAdminActionTree()
        {
            List<MallAdminActionInfo> mallAdminActionTree = new List<MallAdminActionInfo>();
            List<MallAdminActionInfo> mallAdminActionList = GetMallAdminActionList();
            CreateMallAdminActionTree(mallAdminActionList, mallAdminActionTree, 0);
            return mallAdminActionTree;
        }

        /// <summary>
        /// 递归创建商城后台操作树
        /// </summary>
        private static void CreateMallAdminActionTree(List<MallAdminActionInfo> mallAdminActionList, List<MallAdminActionInfo> mallAdminActionTree, int parentId)
        {
            foreach (MallAdminActionInfo mallAdminActionInfo in mallAdminActionList)
            {
                if (mallAdminActionInfo.ParentId == parentId)
                {
                    mallAdminActionTree.Add(mallAdminActionInfo);
                    CreateMallAdminActionTree(mallAdminActionList, mallAdminActionTree, mallAdminActionInfo.Aid);
                }
            }
        }

        /// <summary>
        /// 获得商城后台操作HashSet
        /// </summary>
        /// <returns></returns>
        public static HashSet<string> GetMallAdminActionHashSet()
        {
            HashSet<string> actionHashSet = BrnMall.Core.BMACache.Get(CacheKeys.MALL_MALLADMINACTION_HASHSET) as HashSet<string>;
            if (actionHashSet == null)
            {
                actionHashSet = new HashSet<string>();
                List<MallAdminActionInfo> mallAdminActionList = GetMallAdminActionList();
                foreach (MallAdminActionInfo mallAdminActionInfo in mallAdminActionList)
                {
                    if (mallAdminActionInfo.ParentId != 0 && mallAdminActionInfo.Action != string.Empty)
                        actionHashSet.Add(mallAdminActionInfo.Action);
                }
                BrnMall.Core.BMACache.Insert(CacheKeys.MALL_MALLADMINACTION_HASHSET, actionHashSet);
            }
            return actionHashSet;
        }
    }
}
