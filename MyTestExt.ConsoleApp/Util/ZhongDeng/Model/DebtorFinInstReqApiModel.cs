using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyTestExt.ConsoleApp.Util.ZhongDeng.Model
{
    /// <summary>
    /// 出质人 - 当类型为金融机构时（debtortype=01）
    /// </summary>
    public class DebtorFinInstReqApiModel : DebtorBaseReqApiModel
    {
        /// <summary>
        /// 名称，【非空】【任意字符（汉字）】【最大字符100】
        /// </summary>
        [XmlElement]
        public string debtorname { get; set; }

        /// <summary>
        /// 金融机构编码，【非空】【数字、ASCII字符】【最大字符18】
        /// </summary>
        [XmlElement]
        public string fininstcode { get; set; }

        /// <summary>
        /// 工商注册号/统一社会信用代码，【非空】【任意字符（汉字）】【最大字符30】
        /// </summary>
        [XmlElement]
        public string industryregistrationcode { get; set; }

        /// <summary>
        /// 全球法人机构识别编码，【可空】【数字、ASCII字符】【最大字符20】
        /// </summary>
        [XmlElement]
        public string lei { get; set; }

        /// <summary>
        /// 法定代表人，【非空】【任意字符（汉字）】【最大字符40】
        /// </summary>
        [XmlElement]
        public string corporationname { get; set; }

        /// <summary>
        /// 住所标签
        /// </summary>
        //[XmlElement("address")]
        public AddressReqApiModel address { get; set; }

    }
}
