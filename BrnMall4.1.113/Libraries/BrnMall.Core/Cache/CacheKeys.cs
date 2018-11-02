using System;

namespace BrnMall.Core
{
    /// <summary>
    /// 缓存键
    /// </summary>
    public partial class CacheKeys
    {
        /// <summary>
        /// 在线游客数量缓存键
        /// </summary>
        public const string MALL_ONLINEUSER_GUESTCOUNT = "/Mall/OnlineGuestCount";
        /// <summary>
        /// 全部在线人数缓存键
        /// </summary>
        public const string MALL_ONLINEUSER_ALLUSERCOUNT = "/Mall/OnlineAllUserCount";

        /// <summary>
        /// 用户等级列表缓存键
        /// </summary>
        public const string MALL_USERRANK_LIST = "/Mall/UserRankList";

        /// <summary>
        /// 商城后台操作HashSet缓存键
        /// </summary>
        public const string MALL_MALLADMINACTION_HASHSET = "/Mall/MallAdminActionHashSet";
        /// <summary>
        /// 商城管理员组列表缓存键
        /// </summary>
        public const string MALL_MALLADMINGROUP_LIST = "/Mall/MallAdminGroupList";
        /// <summary>
        /// 商城管理员组操作HashSet缓存键
        /// </summary>
        public const string MALL_MALLADMINGROUP_ACTIONHASHSET = "/Mall/MallAdminGroupActionHashSet/";

        /// <summary>
        /// 被禁止的IPHashSet缓存键
        /// </summary>
        public const string MALL_BANNEDIP_HASHSET = "/Mall/BannedIPHashSet";

        /// <summary>
        /// 筛选词正则列表缓存键
        /// </summary>
        public const string MALL_FILTERWORD_REGEXLIST = "/Mall/FilterWordRegexList";

        /// <summary>
        /// 品牌信息缓存键
        /// </summary>
        public const string MALL_BRAND_INFO = "/Mall/BrandInfo/";

        /// <summary>
        /// 分类列表缓存键
        /// </summary>
        public const string MALL_CATEGORY_LIST = "/Mall/CategoryList";
        /// <summary>
        /// 分类筛选属性及其值列表缓存键
        /// </summary>
        public const string MALL_CATEGORY_FILTERAANDVLIST = "/Mall/CategoryFilterAANDVList/";
        /// <summary>
        /// 分类属性及其值列表JSON缓存键
        /// </summary>
        public const string MALL_CATEGORY_AANDVLISTJSONCACHE = "/Mall/CategoryAANDVListJSONCache/";
        /// <summary>
        /// 分类品牌列表缓存键
        /// </summary>
        public const string MALL_CATEGORY_BRANDLIST = "/Mall/CategoryBrandList/";

        /// <summary>
        /// 商品咨询类型列表缓存键
        /// </summary>
        public const string MALL_PRODUCTCONSULTTYPE_LIST = "/Mall/ProductConsultTypeList";

        /// <summary>
        /// 店铺特征商品列表缓存键
        /// </summary>
        public const string MALL_PRODUCT_STORETRAITLIST = "/Mall/StoreTraitProductList/{0}_{1}_{2}_{3}";

        /// <summary>
        /// 店铺销量商品列表缓存键
        /// </summary>
        public const string MALL_PRODUCT_STORESALELIST = "/Mall/StoreSaleProductList/{0}_{1}_{2}";

        /// <summary>
        /// 活动专题信息缓存键
        /// </summary>
        public const string MALL_TOPIC_INFO = "/Mall/TopicInfo/";

        /// <summary>
        /// 发放中的优惠劵类型列表缓存键
        /// </summary>
        public const string MALL_COUPONTYPE_SENDINGLIST = "/Mall/SendingCouponTypeList";
        /// <summary>
        /// 使用中的优惠劵类型列表缓存键
        /// </summary>
        public const string MALL_COUPONTYPE_USINGLIST = "/Mall/UsingCouponTypeList";
        /// <summary>
        /// 优惠劵类型信息缓存键
        /// </summary>
        public const string MALL_COUPONTYPE_INFO = "/Mall/CouponTypeInfo/";

        /// <summary>
        /// 新闻类型列表缓存键
        /// </summary>
        public const string MALL_NEWSTYPE_LIST = "/Mall/NewsTypeList";
        /// <summary>
        /// 置首新闻列表缓存键
        /// </summary>
        public const string MALL_NEWS_HOMELIST = "/Mall/HomeNewsList/";

        /// <summary>
        /// 帮助列表缓存键
        /// </summary>
        public const string MALL_HELP_LIST = "/Mall/HelpList";

        /// <summary>
        /// 友情链接列表缓存键
        /// </summary>
        public const string MALL_FRIENDLINK_LIST = "/Mall/FriendLinkList";

        /// <summary>
        /// 导航列表缓存键
        /// </summary>
        public const string MALL_NAV_LIST = "/Mall/NavList";
        /// <summary>
        /// 主导航列表缓存键
        /// </summary>
        public const string MALL_NAV_MAINLIST = "/Mall/MainNavList";

        /// <summary>
        /// 子区域列表缓存键
        /// </summary>
        public const string MALL_REGION_CHILDLIST = "/Mall/ChildRegionList/";
        /// <summary>
        /// 区域缓存键
        /// </summary>
        public const string MALL_REGION_INFOBYID = "/Mall/RegionInfo/";
        /// <summary>
        /// 区域缓存键
        /// </summary>
        public const string MALL_REGION_INFOBYNAMEANDLAYER = "/Mall/RegionInfo/{0}_{1}";

        /// <summary>
        /// 广告列表缓存键
        /// </summary>
        public const string MALL_ADVERT_LIST = "/Mall/AdvertList/";

        /// <summary>
        /// 店铺行业列表缓存键
        /// </summary>
        public const string MALL_STORE_INDUSTRYLIST = "/Mall/StoreIndustryList";
        /// <summary>
        /// 店铺等级列表缓存键
        /// </summary>
        public const string MALL_STORE_RANKLIST = "/Mall/StoreRankList";
        /// <summary>
        /// 店铺分类列表缓存键
        /// </summary>
        public const string MALL_STORE_CLASSLIST = "/Mall/StoreClassList/";
        /// <summary>
        /// 店铺配送模板信息缓存键
        /// </summary>
        public const string MALL_STORE_SHIPTEMPLATEINFO = "/Mall/StoreShipTemplateInfo/";

        /// <summary>
        /// 配送公司列表缓存键
        /// </summary>
        public const string MALL_SHIPCOMPANY_LIST = "/Mall/ShipCompanyList";
    }
}
