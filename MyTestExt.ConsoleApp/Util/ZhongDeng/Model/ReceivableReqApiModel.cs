using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyTestExt.ConsoleApp.Util.ZhongDeng.Model
{
    /// <summary>
    /// 注册，暂时只做应收账款
    /// </summary>
    [XmlRoot("register")]
    public class ReceivableReqApiModel
    {
        /// <summary>
        /// 登记业务类型,【非空】,【数字、ASCII字符】
        /// </summary>
        /// <![CDATA[
        /// B00000	融资租赁；A00200	应收账款转让；A00100	应收账款质押；N10000	保证金质押；N10010	存货/仓单质押；N01700	所有权保留；N01600	动产留置权；N10020	动产信托；N10030	农业设施抵押；N00000	其他动产融资业务；P00100	生产设备、原材料、半成品、产品抵押登记
        /// ]]>
        [XmlElement]
        public string businesstype { get; set; }

        /// <summary>
        /// 登记期限, 单位月	1-360间的正整数（包含1,360）
        /// 【非空】,【数字】
        /// </summary>
        [XmlElement]
        public string timelimit { get; set; }

        /// <summary>
        /// 填表人归档号，可空，【任意字符（汉字）】【最大40字符】
        /// </summary>
        [XmlElement]
        public string title { get; set; }

        /// <summary>
        /// 出质人（提供财产方）,【非空】
        /// </summary>
        [XmlArray("debtors")]
        [XmlArrayItem("debtor")]
        public List<DebtorBaseReqApiModel> debtors { get; set; }

        /// <summary>
        /// 质权人（占有财产，并在债务人不履行债务时以该财产折价或者以拍卖、变卖该财产的价款优先受偿的人）
        /// ,【非空】
        /// </summary>
        [XmlArray("debtees")]
        [XmlArrayItem("debtee")]
        public List<DebteeBaseReqApiModel> debtees { get; set; }

        /// <summary>
        /// 财产标签（转让、质押等）,【非空】
        /// </summary>
        [XmlElement]
        public PledgeBaseReqApiModel pledgeinfo { get; set; }

    }
}
