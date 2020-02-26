using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyTestExt.ConsoleApp.Util.ZhongDeng.Model
{
    /// <summary>
    /// 地址标签
    /// </summary>
    public class AddressReqApiModel
    {
        /// <summary>
        /// 国家，
        /// note.只能是“CHN”(中国)、“OTH”(其他国家或地区)中的一个
        /// <![CDATA[
        /// 1、类型为“金融机构”、“企业”、“事业单位”、“其他”时必需此项且必填【非空】
        /// 2、类型为“个人”、“个体工商户”，且证件类型是身份证、临时身份证必填【非空】
        ///                              ，且证件类型是护照【不显示】
        ///                              ，且证件类型为其他类型【可空】
        /// ]]>
        /// </summary>
        [XmlElement]
        public string nationality { get; set; }

        /// <summary>
        /// 省，所属国家为中中国时，其所属省份
        /// note.具体编码见附件-省市代码部分
        /// </summary>
        /// <![CDATA[
        /// 1、类型为“金融机构”、“企业”、“事业单位”、“其他”时必需此项且必填【非空】
        /// 2、类型为“个人”、“个体工商户”，且证件类型是身份证、临时身份证必填【非空】
        ///                              ，且证件类型是护照【不显示】
        ///                              ，且证件类型为其他类型【可空】
        /// ]]>
        [XmlElement]
        public string province { get; set; }

        /// <summary>
        /// 市，所属国家为中国时，其所属省份的下辖城市
        /// note.具体编码见附件-省市代码部分
        /// </summary>
        /// <![CDATA[
        /// 1、类型为“金融机构”、“企业”、“事业单位”、“其他”时必需此项且必填【非空】
        /// 2、类型为“个人”、“个体工商户”，且证件类型是身份证、临时身份证必填【非空】
        ///                              ，且证件类型是护照【不显示】
        ///                              ，且证件类型为其他类型【可空】
        /// ]]>
        [XmlElement]
        public string city { get; set; }

        /// <summary>
        /// 详细地址，【任意字符（汉字）】【最大100字符】
        /// </summary>
        [XmlElement]
        public string detailaddress { get; set; }
    }
}
