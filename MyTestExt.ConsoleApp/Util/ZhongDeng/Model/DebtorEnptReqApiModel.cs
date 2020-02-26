using System.Xml.Serialization;

namespace MyTestExt.ConsoleApp.Util.ZhongDeng.Model
{
    /// <summary>
    /// 出质人 - 当类型为企业时（debtortype=02）
    /// </summary>
    public class DebtorEnptReqApiModel : DebtorBaseReqApiModel
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
        /// 企业所属行业
        /// </summary>
        /// <![CDATA[
        /// 9999	其他；A	农、林、牧、渔业；B	采矿业；C	制造业；D	电力、热力、燃气及水生产和供应业；E	建筑业；F	批发和零售业；G	交通运输、仓储和邮政业；H	住宿和餐饮业；I	信息传输、软件和信息技术服务业；J	金融业；K	房地产业；L	租赁和商务服务业；M	科学研究和技术服务业；N	水利、环境和公共设施管理业；O	居民服务、修理和其他服务业；P	教育；Q	卫生和社会工作；R	文化、体育和娱乐业；S	公共管理、社会保障和社会组织；T	国际组织
        /// ]]>
        [XmlElement]
        public string industrycode { get; set; }

        /// <summary>
        /// 企业规模
        /// </summary>
        /// <![CDATA[
        /// 10	大型企业；20	中型企业；30	小型企业；40	微型企业
        /// ]]>
        [XmlElement]
        public string corporationscale { get; set; }

        /// <summary>
        /// 住所标签
        /// </summary>
        [XmlElement]
        public AddressReqApiModel address { get; set; }
    }
}
