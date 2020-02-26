using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyTestExt.ConsoleApp.Util.ZhongDeng.Model
{
    /// <summary>
    /// 财产标签 - 应收账款转让
    /// </summary>
    public class PledgeReceivableReqApiModel : PledgeBaseReqApiModel
    {
        /// <summary>
        /// 转让财产币种，【可空】
        /// </summary>
        /// <![CDATA[
        /// CNY	人民币；USD	美元；EUR	欧元；GBP	英镑；JPY	日元；HKY	港币；OTH	其他币种
        /// ]]>
        [XmlElement]
        public string pledgecontractcurrency { get; set; }

        /// <summary>
        /// 转让财产金额，【可空】【数字、ASCII字符】【最大长度20】
        /// </summary>
        /// <![CDATA[
        /// 长度不超18位的非负整数或小数点前不超过18位且小数点后不超过2位的小数
        /// ]]>
        [XmlElement]
        public string pledgecontractsum { get; set; }

    }
}
