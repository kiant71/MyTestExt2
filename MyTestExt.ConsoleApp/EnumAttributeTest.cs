using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;

namespace MyTestExt.ConsoleApp
{
    public class EnumAttributeTest
    {
        public static void Do()
        {
            var aa = GetResources(typeof(FormatSearchBindControlType));
        }

        public static List<EnumResourceKeyValModel> GetResources(Type enumType)
        {
            if (enumType == (Type)null)
                return null;

            var key = ((ResourceKeyAttribute) Attribute.GetCustomAttribute(
                    enumType, typeof(ResourceKeyAttribute)))?.Key;
            if (string.IsNullOrEmpty(key))
                return null;

            var rets = new List<EnumResourceKeyValModel>();
            var values = Enum.GetValues(enumType); // 也可以通过反射获得enumType.GetFields();
            for (var i = 0; i < values.Length; i++)
            {
                var enumVal = (int)Enum.Parse(enumType, values.GetValue(i).ToString());
                rets.Add(new EnumResourceKeyValModel
                {
                    EnumVal = enumVal,
                    ResourceKey = string.Format("{0}_{1}", key, enumVal),
                });
            }

            return rets;
        }
    }

    #region Model
    
    /// <summary>
    /// 枚举对应资源关键字
    /// </summary>
    public class EnumResourceKeyValModel
    {
        /// <summary>
        /// （枚举）原值
        /// </summary>
        public int EnumVal { get; set; }

        /// <summary>
        /// （资源文件）关键字名称
        /// </summary>
        public string ResourceKey { get; set; }
    } 

    #endregion


    #region Attribute

    /// <summary>
    /// 资源关键字 属性
    /// </summary>
    public class ResourceKeyAttribute : Attribute
    {
        public ResourceKeyAttribute()
        {
        }

        public ResourceKeyAttribute(string key)
        {
            Key = key;
        }

        /// <summary>
        /// 关键字名称
        /// </summary>
        public string Key { get; set; }
    }

    #endregion

    #region Enum

    /// <summary>
    /// 格式化搜索绑定控件类型
    /// </summary>
    [ResourceKey("Enum_FormatSearchBindControlType")]
    [EnumAsInt]
    public enum FormatSearchBindControlType
    {
        /// <summary>
        /// 无 
        /// </summary>
        NONE = 99,
        /// <summary>
        /// 网格控件
        /// </summary>
        GridControl = 1,

        /// <summary>
        /// 透视表
        /// </summary>
        PivotControl = 2,

        /// <summary>
        /// 树型控件
        /// </summary>
        TreeListControl = 3,
    }

    #endregion

}
