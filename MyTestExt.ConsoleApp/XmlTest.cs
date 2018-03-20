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
        public void Do()
        {
            Do1();
            Do3();
        }

        private static void Do1()
        {
            var model = new SFbspModel
            {
                DocEntry = 123,
                orderid = "1231231313123",
                remark = "12345678901234567890111111",
                value1 = 3334,

                Item1 = new List<SFbspItem1Model>
                {
                    new SFbspItem1Model {id = 1, name = "aaa"},
                    new SFbspItem1Model {id = 2, name = "bbb"}
                },

                Item2 = new List<SFbspItem2Model>
                {
                    new SFbspItem2Model {id2 = 3, name2 = "ccc"},
                    new SFbspItem2Model {id2 = 4, name2 = "ddd"}
                }
            };

            var req = new SFbspRequestModel<SFbspBodyOrderModel>
            {
                service = "OrderService",
                lang = "zh-CN",
                Head = "BSPdevelop",
                Body = new List<SFbspBodyOrderModel> {new SFbspBodyOrderModel {Order = new List<SFbspModel> {model}}},
            };

            var aa = model.ToXml();
            var bb = XmlExtends.ToXml2(req);
        }
        
        public static string Do2()
        {
            var model = new SFbspModel
            {
                DocEntry = 123,
                orderid = "1231231313123",
                remark = "12345678901234567890111111",
                value1 = 3334,

                Item1 = new List<SFbspItem1Model>
                {
                    new SFbspItem1Model {id = 1, name = "aaa"},
                    new SFbspItem1Model {id = 2, name = "bbb"}
                },

                Item2 = new List<SFbspItem2Model>
                {
                    new SFbspItem2Model {id2 = 3, name2 = "ccc"},
                    new SFbspItem2Model {id2 = 4, name2 = "ddd"}
                }
            };

            var req = new SFbspRequestModel<SFbspBodyOrderModel>
            {
                service = "OrderService",
                lang = "zh-CN",
                Head = "BSPdevelop",
                Body = new List<SFbspBodyOrderModel> { new SFbspBodyOrderModel { Order = new List<SFbspModel> { model } } },
            };


            var bb = XmlExtends.ToXml2(req);
            return bb;
        }

        public static void Do3()
        {
            var strXml =
                "<?xml version='1.0' encoding='UTF-8'?><Response service=\"OrderService\"><Head>ERR</Head><ERROR code=\"8001\">IP未授权</ERROR></Response>";

            strXml = "<?xml version='1.0' encoding='UTF-8'?><Response service=\"OrderService\"><Head>OK</Head>    <Body>    <OrderResponse orderid=\"TE20150104\" mailno=\"444003078089\" origincode=\"755\" destcode=\"010\" filter_result=\"2\"    url=\"http://ucmp-wx.sit.sfexpress.com/wxaccess/weixin/activity/cx_open_order?p1=033234448562\"/>    </Body></Response>";

            var obj = XmlExtends.FromXml<SFbspResponseModel<SFbspBodyOrderResponseModel>>(strXml);

        }
    }

    public static class XmlExtends
    {

        /// <summary>
        /// 对象序列化为Xml格式的字符串
        /// </summary>
        public static string ToXml<T>(this T o) where T : new()
        {
            string ret;
            using (var ms = new MemoryStream())
            {
                var xs = new XmlSerializer(typeof(T));
                xs.Serialize(ms, o);
                ms.Flush();
                ms.Position = 0;
                var sr = new StreamReader(ms);
                ret = sr.ReadToEnd();
            }
            return ret;
        }


        /// <summary>
        /// 对象序列化为Xml格式的字符串
        /// </summary>
        public static string ToXml2<T>(T o) where T : new()
        {
            //string ret;
            //using (var ms = new MemoryStream())
            //{
            //    var xs = new XmlSerializer(typeof(T));
            //    xs.Serialize(ms, o);
            //    ms.Flush();
            //    ms.Position = 0;
            //    var sr = new StreamReader(ms, Encoding.UTF8);
            //    ret = sr.ReadToEnd();
            //}
            //return ret;

            // 这个是根据系统编码的？ encoding="utf-16"? 
            using (var writer = new StringWriterUTF8())
            {
                new XmlSerializer(o.GetType()).Serialize(writer, o);
                return writer.ToString();
            }
        }

        public static T FromXml<T>(string strXml) where T : class
        {
            var reader = new XmlTextReader(new StringReader(strXml));
            var xs = new XmlSerializer(typeof(T));
            var result = xs.Deserialize(reader);

            return result as T;
        }
    }


    public class StringWriterUTF8 : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}
