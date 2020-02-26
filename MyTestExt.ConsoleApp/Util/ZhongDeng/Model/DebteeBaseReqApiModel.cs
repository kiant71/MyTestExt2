using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyTestExt.ConsoleApp.Util.ZhongDeng.Model
{
    /// <summary>
    /// 质权人（占有财产，并在债务人不履行债务时以该财产折价或者以拍卖、变卖该财产的价款优先受偿的人）
    /// </summary>
    [XmlInclude(typeof(DebteeEnptReqApiModel))]
    [XmlInclude(typeof(DebteeFinInstReqApiModel))]
    [XmlInclude(typeof(DebteeIndiReqApiModel))]
    [XmlInclude(typeof(DebteeInstReqApiModel))]
    [XmlInclude(typeof(DebteeOtherReqApiModel))]
    [XmlInclude(typeof(DebteePersReqApiModel))]
    public class DebteeBaseReqApiModel
    {
        /// <summary>
        /// id
        /// </summary>
        [XmlAttribute]
        public string id { get; set; }

        /// <summary>
        /// 质权人（受让人）类型
        /// 01	金融机构；02	企业；03	事业单位；06	个体工商户；04	个人；09	其他
        /// </summary>
        [XmlElement]
        public string debteetype { get; set; }
    }
}
