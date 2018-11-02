using System;

namespace BrnMall.Core
{
    /// <summary>
    /// 店铺分类信息类
    /// </summary>
	public class StoreClassInfo
	{
        private int _storecid;//店铺分类id
        private int _storeid;//店铺id
        private int _displayorder = 0;//排序
        private string _name = "";//名称
        private int _parentid = 0;//父id
        private int _layer = 0;//层级
        private int _haschild = 0;//是否有子节点
        private string _path;//分类路径

		/// <summary>
        /// 店铺分类id
		/// </summary>
        public int StoreCid
		{
            set { _storecid = value; }
            get { return _storecid; }
		}
        /// <summary>
        /// 店铺id
        /// </summary>
        public int StoreId
        {
            set { _storeid = value; }
            get { return _storeid; }
        }
        /// <summary>
        /// 排序
        /// </summary>
        public int DisplayOrder
        {
            set { _displayorder = value; }
            get { return _displayorder; }
        }
		/// <summary>
        /// 名称
		/// </summary>
		public string Name
		{
            set { _name = value.TrimEnd(); }
            get { return _name; }
		}
        /// <summary>
        /// 父id
        /// </summary>
        public int ParentId
        {
            set { _parentid = value; }
            get { return _parentid; }
        }
        /// <summary>
        /// 层级
        /// </summary>
        public int Layer
        {
            set { _layer = value; }
            get { return _layer; }
        }
        /// <summary>
        /// 是否有子节点
        /// </summary>
        public int HasChild
        {
            set { _haschild = value; }
            get { return _haschild; }
        }
        /// <summary>
        /// 分类路径
        /// </summary>
        public string Path
        {
            set { _path = value.TrimEnd(); }
            get { return _path; }
        }
	}
}

