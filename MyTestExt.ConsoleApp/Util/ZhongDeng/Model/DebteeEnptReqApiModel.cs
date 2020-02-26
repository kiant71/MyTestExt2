using System.Xml.Serialization;

namespace MyTestExt.ConsoleApp.Util.ZhongDeng.Model
{
    /// <summary>
    /// 质权人 - 当类型为企业时（debtortype=02）
    /// </summary>
    public class DebteeEnptReqApiModel : DebteeBaseReqApiModel
    {
        /// <summary>
        /// 名称，【非空】【任意字符（汉字）】【最大字符100】
        /// </summary>
        [XmlElement]
        public string debteename { get; set; }

        /// <summary>
        /// 组织机构代码/统一社会信用代码，note.【非空】【任意字符（汉字）】【最大字符18】
        /// </summary>
        [XmlElement]
        public string organizationcode { get; set; }

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

        ///// <summary>
        ///// 企业所属行业
        ///// </summary>
        //public string industrycode { get; set; }

        ///// <summary>
        ///// 企业规模
        ///// </summary>
        //public string corporationscale { get; set; }

        /// <summary>
        /// 住所标签
        /// </summary>
        [XmlElement]
        public AddressReqApiModel address { get; set; }

        


    }
}
