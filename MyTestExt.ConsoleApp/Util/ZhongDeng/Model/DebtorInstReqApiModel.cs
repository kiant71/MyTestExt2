using System.Xml.Serialization;

namespace MyTestExt.ConsoleApp.Util.ZhongDeng.Model
{
    /// <summary>
    /// 出质人 - 当类型为机关事业单位时（debtortype=03）
    /// </summary>
    public class DebtorInstReqApiModel : DebtorBaseReqApiModel
    {
        /// <summary>
        /// 名称，【非空】【任意字符（汉字）】【最大字符100】
        /// </summary>
        [XmlElement]
        public string debtorname { get; set; }

        /// <summary>
        /// 组织机构代码/统一社会信用代码，note.【非空】【任意字符（汉字）】【最大字符18】
        /// </summary>
        [XmlElement]
        public string organizationcode { get; set; }

        /// <summary>
        /// 事业单位法人证书号，【可空】【任意字符（汉字）】【最大字符30】
        /// </summary>
        [XmlElement]
        public string instcorpcertno { get; set; }

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
        [XmlElement]
        public AddressReqApiModel address { get; set; }
    }
}
