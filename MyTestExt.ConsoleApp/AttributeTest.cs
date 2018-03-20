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
            var model = new SFbspModel
            {
                orderid = "1231231313123",
                remark = "12345678901234567890111111",
                value1 = 3334,
            };

            var flag = SFbspTool.AttributeVerify(model);
        }

        
    }


    public class SFbspTool
    {
        public static bool AttributeVerify(SFbspModel model)
        {
            var t = model.GetType();
            var properties = t.GetProperties();
            foreach (var property in properties)
            {
                if (!property.IsDefined(typeof(SFbspAttribute), false))
                    continue;

                //var attrs = property.GetCustomAttributes(true);
                var attr = (SFbspAttribute) Attribute.GetCustomAttribute(property, typeof(SFbspAttribute))
                           ?? new SFbspAttribute();

                var strValue = property.GetValue(model) as string;
                var flagChange = false;

                // 如果字段值为空，
                if (string.IsNullOrWhiteSpace(strValue))
                {
                    // 则进行非空校验
                    if (attr.IsNotNull)
                        return false;

                    // 进行初始化赋值判断，否则跳过
                    if (!string.IsNullOrWhiteSpace(attr.Default))
                    {
                        strValue = attr.Default;
                        flagChange = true;
                    }
                    else
                        continue;
                }

                // 如果字段值超长，则进行截串
                if (attr.MaxLength > 0 && attr.MaxLength < strValue.Length)
                {
                    strValue = strValue.Substring(0, attr.MaxLength);
                    flagChange = true;
                }


                if (flagChange)
                    property.SetValue(model, Convert.ChangeType(strValue, property.PropertyType), null);
            }


            return true;
        }

    }




    public class SFbspAttribute : Attribute
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
        /// 指定默认值，数值型与字符型共用
        /// </summary>
        public string Default { get; set; } = "";

        
    }


    [XmlRoot("Request")]
    public class SFbspRequestModel<T>
    {
        [XmlAttribute]
        public string service { get; set; }

        [XmlAttribute]
        public string lang { get; set; } = "zh-CN";

        //[XmlElement]
        public string Head { get; set; }

        [XmlElement]
        public List<T> Body { get; set; }
    }


    public class SFbspBodyOrderModel
    {
        [XmlElement]
        public List<SFbspModel> Order { get; set; }
    }



    //[XmlRoot("Order")]
    //[XmlElement]
    public class SFbspModel
    {
        [XmlIgnore]
        public int DocEntry { get; set; }
        
        [SFbsp(IsNotNull =true, MaxLength = 8)]
        [XmlAttribute]
        public string orderid { get; set; }

        [SFbsp(MaxLength = 20)]
        [XmlAttribute]
        public string remark { get; set; }

        [SFbsp(MaxLength = 2, Default = "12")]
        [XmlAttribute]
        public int value1 { get; set; }

        [SFbsp(MaxLength = 20, Default = "12")]
        [XmlAttribute]
        public string value2 { get; set; }

        [XmlAttribute]
        public string NoVerify { get; set; }


        [XmlElement("OrderItem1")]
        public List<SFbspItem1Model> Item1 { get; set; }

        [XmlElement("OrderItem2")]
        public List<SFbspItem2Model> Item2 { get; set; }

    }


    //[XmlRoot("OrderItem1")]
    public class SFbspItem1Model
    {
        [XmlAttribute]
        public int id { get; set; }

        [XmlAttribute]
        public string name { get; set; }
    }

    //[XmlRoot("OrderItem2")]
    public class SFbspItem2Model
    {
        [XmlAttribute]
        public int id2 { get; set; }

        [XmlAttribute]
        public string name2 { get; set; }
    }



    [XmlRoot("Response")]
    public class SFbspResponseModel<T>
    {
        //[XmlElement]
        public string Head { get; set; }

        [XmlElement]
        public List<T> Body { get; set; }
    }

    public class SFbspBodyOrderResponseModel
    {
        [XmlElement("OrderResponse")]
        public List<SFbspOrderResponseModel> OrderResponses { get; set; }
    }

    /// <summary>
    /// 订单相应（返回）
    /// </summary>
    public class SFbspOrderResponseModel
    {
        /// <summary>
        /// 客户订单号
        /// 必填，String(64)
        /// </summary>
        [XmlAttribute]
        public string orderid { get; set; }

        /// <summary>
        /// 顺丰运单号，一个订单只能有一个母单号，如果是子母单的情况，以半角逗号分隔，主单号在第一个位置，如 “755123456789,001123456789,002123456789” ，可用于顺丰电子运单标签打印。
        /// String(4000) 
        /// </summary>
        [XmlAttribute]
        public string mailno { get; set; }

        /// <summary>
        /// 顺丰签回单服务运单号
        /// String(30)
        /// </summary>
        [XmlAttribute]
        public string return_tracking_no { get; set; }

        /// <summary>
        /// 原寄地区域代码，可用于顺丰电子运单标签打印。
        /// String(10)
        /// </summary>
        [XmlAttribute]
        public string origincode { get; set; }

        /// <summary>
        /// 目的地区域代码，可用于顺丰电子运单标签打印。
        /// String(10)
        /// </summary>
        [XmlAttribute]
        public string destcode { get; set; }

        /// <summary>
        /// 筛单结果： 1：人工确认 2：可收派 3：不可以收派
        /// Number(2)
        /// </summary>
        [XmlAttribute]
        public byte filter_result { get; set; }

        /// <summary>
        /// 如果 filter_result=3 时为必填，不可以收派的原因代码： 1：收方超范围 2：派方超范围 3-：其它原因
        /// String(100)
        /// </summary>
        [XmlAttribute]
        public string remark { get; set; }

        /// <summary>
        /// 代理单号
        /// String(40)
        /// </summary>
        [XmlAttribute]
        public string agentMailno { get; set; }

        /// <summary>
        /// 地址映射码
        /// String(30) 
        /// </summary>
        [XmlAttribute]
        public string mapping_mark { get; set; }

        /// <summary>
        /// 二维码 URL
        /// String(200)
        /// </summary>
        [XmlAttribute]
        public string url { get; set; }

    }


}
