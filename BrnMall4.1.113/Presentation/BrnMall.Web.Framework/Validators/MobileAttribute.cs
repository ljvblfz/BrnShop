using System;
using System.ComponentModel.DataAnnotations;

namespace BrnMall.Web.Framework
{
    /// <summary>
    /// 手机号验证属性
    /// </summary>
    public class MobileAttribute : ValidationAttribute
    {
        public MobileAttribute()
        {
            ErrorMessage = "不是有效的手机号";
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;
            else return BrnMall.Core.ValidateHelper.IsMobile(value.ToString());

        }
    }
}
