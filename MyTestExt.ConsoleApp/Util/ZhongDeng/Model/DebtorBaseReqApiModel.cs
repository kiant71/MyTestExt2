using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyTestExt.ConsoleApp.Util.ZhongDeng.Model
{
    /// <summary>
    /// 出质人（提供财产方）
    /// </summary>
    [XmlInclude(typeof(DebtorEnptReqApiModel))]
    [XmlInclude(typeof(DebtorFinInstReqApiModel))]
    [XmlInclude(typeof(DebtorIndiReqApiModel))]
    [XmlInclude(typeof(DebtorInstReqApiModel))]
    [XmlInclude(typeof(DebtorOtherReqApiModel))]
    [XmlInclude(typeof(DebtorPersReqApiModel))]
    public class DebtorBaseReqApiModel
    {
        /// <summary>
        /// id
        /// </summary>
        [XmlAttribute]
        public string id { get; set; }

        /// <summary>
        /// 出质人类型
        /// 01	金融机构；02	企业；03	事业单位；06	个体工商户；04	个人；09	其他
        /// </summary>
        [XmlElement]
        public string debtortype { get; set; }

    }
}
