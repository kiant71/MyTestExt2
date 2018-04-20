using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MyTestExt.ConsoleApp
{
    public class XmlTest
    {
        public static void Do ()
        {
            var xmlModel = new XmlModelA<XmlModelB>
            {
                Head = "cHead1",
                Body = new List<XmlModelB>
                {
                    new XmlModelB
                    {
                        Orders = new List<XmlModeC>
                        {
                            new XmlModeC
                            {
                                DocEntry = 1,
                                orderid = "11",
                                Cargoes = new List<XmlModelD>
                                {
                                    new XmlModelD {LineNum = 1, name = "name1"},
                                    new XmlModelD {LineNum = 2, name = "name2"},
                                }
                            }
                        }

                    },
                }
            };

            var xmlStr = XmlUtil.ToXml(xmlModel);
            var xmlStr2 = @"
<?xml version=""1.0"" encoding=""utf-8""?>
<Request xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
    lang=""zh-CN"">
  <Head>cHead1</Head>
  <Body>
    <Order orderid=""11"">
      <Cargo name=""name1"" />
      <Cargo name=""name2"" />
    </Order>
  </Body>
</Request>";

            var xmlModelA = XmlUtil.FromXml<XmlModelA<XmlModelB>>(xmlStr);
            var xmlModelB = XmlUtil.FromXml<XmlModelA<XmlModelB>>(xmlStr);
            var nameA = xmlModelA.Body.First().Orders.First().Cargoes.First().name;
            var nameB = xmlModelB.Body.First().Orders.First().Cargoes.First().name;
        }

    }



    [XmlRoot("Request")]
    public class XmlModelA<T>
    {
        [XmlAttribute]
        public string lang { get; set; } = "zh-CN";

        //[XmlElement]
        public string Head { get; set; }

        [XmlElement]
        public List<T> Body { get; set; }
    }
    
    public class XmlModelB
    {
        [XmlElement("Order")]
        public List<XmlModeC> Orders { get; set; }
    }

    public class XmlModeC
    {
        // xml忽略
        [XmlIgnore]
        public int DocEntry { get; set; }

        // 作为属性输出，string 格式值为 null时不输出，""或者有值时正常输出
        [XmlAttribute]
        public string orderid { get; set; }

        // 利用虚构字段，做为数值型，值为0 时不进行输出
        [XmlIgnore]
        public byte is_gen_bill_no { get; set; }

        [XmlAttribute("is_gen_bill_no")]
        public string is_gen_bill_no_XML
        {
            get
            {
                if (is_gen_bill_no != 0)
                    return is_gen_bill_no.ToString();
                return null;
            }
            set { } // 这段是必须写的，否则无法创建 xml属性
        }

        // 利用虚构字段，做为日期型，无值 时不进行输出
        [XmlIgnore]
        public DateTime? sendstarttime { get; set; }


        [XmlAttribute("sendstarttime")]
        public string sendstarttime_XML
        {
            get
            {
                if (sendstarttime.HasValue)
                    return sendstarttime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                return null;
            }
            set { }
        }


        [XmlElement("Cargo")]
        public List<XmlModelD> Cargoes { get; set; }
    }
    

    public class XmlModelD
    {
        [XmlIgnore]
        public int LineNum { get; set; }


        [XmlAttribute]
        public string name { get; set; }
    }





    public static class XmlUtil
    {
        /// <summary>
        /// 写 Xml字符串
        /// </summary>
        public static string ToXml<T>(T obj) where T : new()
        {
            using (var writer = new StringWriterUTF8())
            {
                new XmlSerializer(obj.GetType()).Serialize(writer, obj);
                return writer.ToString();
            }
        }

        /// <summary>
        /// 从 Xml字符串读取对象
        /// </summary>
        public static T FromXml<T>(string strXml) where T : class
        {
            var reader = new XmlTextReader(new StringReader(strXml));
            var xs = new XmlSerializer(typeof(T));
            var result = xs.Deserialize(reader);

            return result as T;
        }


        /// <summary>
        /// utf8 格式编码，C# 默认是utf16
        /// </summary>
        public class StringWriterUTF8 : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }



}
