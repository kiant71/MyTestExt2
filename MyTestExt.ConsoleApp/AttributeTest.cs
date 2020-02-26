using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyTestExt.ConsoleApp
{
    public class AttributeTest
    {
        public void Do()
        {
            var model = new AttributeTestModel
            {
                orderid = "1231231313123",
                remark = "12345678901234567890111111",
                value1 = 3334,
            };

            var flag = AttributeUtil.AttributeVerify(model);
        }
    }



    public class AttributeUtil
    {
        /// <summary>
        /// 对象属性的特性校验
        /// </summary>
        public static bool AttributeVerify(object obj)
        {
            var t = obj.GetType();
            var properties = t.GetProperties();
            foreach (var property in properties)
            {
                if (!property.IsDefined(typeof(SimpleAttribute), false))
                    continue;

                var attr = (SimpleAttribute) Attribute.GetCustomAttribute(property, typeof(SimpleAttribute))
                           ?? new SimpleAttribute();

                var strValue = property.GetValue(obj) as string;
                var flagChange = false;

                // IsNotNull：字段值空 判断
                if (string.IsNullOrWhiteSpace(strValue))
                {
                    // 则进行非空校验
                    if (attr.IsNotNull)
                        return false;

                    // Default：字段值处初始化判断
                    if (!string.IsNullOrWhiteSpace(attr.Default))
                    {
                        strValue = attr.Default;
                        flagChange = true;
                    }
                    else
                        continue;
                }

                // MaxLength：字段值超长验证，截串
                if (attr.IsIncludeCN && attr.MaxLength > 0)
                    attr.MaxLength = attr.MaxLength / 3;
                if (attr.MaxLength > 0 && attr.MaxLength < strValue.Length)
                {
                    strValue = strValue.Substring(0, attr.MaxLength);
                    flagChange = true;
                }

                if (flagChange)
                    property.SetValue(obj, Convert.ChangeType(strValue, property.PropertyType), null);
            }

            return true;
        }

    }

    public class SimpleAttribute : Attribute
    {
        /// <summary>
        /// 是否不允许空，默认为否-允许为空
        /// </summary>
        public bool IsNotNull { get; set; }

        /// <summary>
        /// 最大长度，默认为0-不进行长度校验
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// 是否包含中文字符。
        /// </summary>
        public bool IsIncludeCN { get; set; }

        /// <summary>
        /// 指定默认值，数值型与字符型共用
        /// </summary>
        public string Default { get; set; } = "";
    }


    public class AttributeTestModel
    {
        [Simple(IsNotNull = true, MaxLength = 8)]
        public string orderid { get; set; }

        [Simple(MaxLength = 20)]
        public string remark { get; set; }

        [Simple(MaxLength = 2, Default = "12")]
        public int value1 { get; set; }

        [Simple(MaxLength = 20, Default = "12")]
        public string value2 { get; set; }

    }





}
