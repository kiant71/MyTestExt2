using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyTestExt.ConsoleApp.Util.ZhongDeng.Model
{
    /// <summary>
    /// 财产标签（转让、质押等基础类）
    /// </summary>
    [XmlInclude(typeof(PledgeReceivableReqApiModel))]
    public class PledgeBaseReqApiModel
    {
        /// <summary>
        /// 主合同号，【可空】【任意字符（汉字）】【最大字符100】
        /// </summary>
        [XmlElement]
        public string maincontractnumber { get; set; }

        /// <summary>
        /// 主合同币种，【可空】
        /// </summary>
        /// <![CDATA[
        /// CNY	人民币；USD	美元；EUR	欧元；GBP	英镑；JPY	日元；HKY	港币；OTH	其他币种
        /// ]]>
        [XmlElement]
        public string maincontractcurrency { get; set; }

        /// <summary>
        /// 主合同金额，【可空】【数字、ASCII字符】【最大长度20】
        /// </summary>
        /// <![CDATA[
        /// 长度不超18位的非负整数或小数点前不超过18位且小数点后不超过2位的小数
        /// ]]>
        [XmlElement]
        public string maincontractsum { get; set; }

        /// <summary>
        /// 财产描述信息（需要换行处，加入\r\n），【非空】【任意字符（汉字）】【最大长度4000】
        /// </summary>
        [XmlElement]
        public string collateraldescribe { get; set; }

        /// <summary>
        /// 财产附件，可空,（有值的情况下，需要确保附件数据已经提交）
        /// </summary>
        [XmlArray("attachments")]
        [XmlArrayItem("attachment")]
        public List<PledgeAttachmentReqApiModel> attachments { get; set; }

    }
}
