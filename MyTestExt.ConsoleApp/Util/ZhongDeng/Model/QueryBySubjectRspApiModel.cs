using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyTestExt.ConsoleApp.Util.ZhongDeng.Model
{
    /// <summary>
    /// 查询反馈报文
    /// </summary>
    [XmlRoot("feedback")]
    public class QueryBySubjectRspApiModel
    {
        /// <summary>
        /// 【各个业务类型】查询的记录数的总和，ex.3
        /// </summary>
        [XmlElement]
        public string totalrows { get; set; }

        /// <summary>
        /// 查询反馈报文-结果
        /// </summary>
        [XmlElement]
        public QueryBySubjectDataRspApiModel data { get; set; }
    }

    /// <summary>
    /// 查询反馈报文-结果
    /// </summary>
    public class QueryBySubjectDataRspApiModel
    {
        /// <summary>
        /// 【各个业务类型】查询的记录数的总和
        /// </summary>
        /// <![CDATA[
        /// 多个 <results>...</results>   <results>...</results>   <results>...</results>
        /// ]]>
        //[XmlArrayItem("results")]
        [XmlElement]
        public List<QueryBySubjectBizTypeRspApiModel> results { get; set; }
    }

    /// <summary>
    /// 【业务类型】的查询结果
    /// </summary>
    public class QueryBySubjectBizTypeRspApiModel
    {
        /// <summary>
        /// 业务类型, ex.A00200
        /// </summary>
        [XmlElement]
        public string businesstype { get; set; }

        /// <summary>
        /// 该业务类型的查询记录数, ex.3
        /// </summary>
        [XmlElement]
        public string rows { get; set; }

        /// <summary>
        /// 此内容为代码，具体解释见资金融出方名称附件, ex.A00200
        /// </summary>
        [XmlElement]
        public string spnametitle { get; set; }

        /// <summary>
        /// 明细子项列表
        /// </summary>
        /// <![CDATA[
        /// 多个 <result>...</result>  <result>...</result>  <result>...</result>
        /// ]]>
        //[XmlArray("result")]
        [XmlElement]
        public List<QueryBySubjectBizTypeItemRspApiModel> result { get; set; }

    }

    /// <summary>
    /// 【业务类型】的查询结果 - 明细子项
    /// </summary>
    public class QueryBySubjectBizTypeItemRspApiModel
    {
        /// <summary>
        /// 登记号, ex.00030754000003660297
        /// </summary>
        [XmlElement]
        public string registerno { get; set; }

        /// <summary>
        /// 登记时间, ex.2019-12-20 11:22:39
        /// </summary>
        [XmlElement]
        public string registertime { get; set; }

        /// <summary>
        /// 过期日期，ex.2020-02-19
        /// </summary>
        [XmlElement]
        public string registerexpiredate { get; set; }

        /// <summary>
        /// 登记类型,ex.01
        /// </summary>
        [XmlElement]
        public string registertype { get; set; }

        /// <summary>
        /// 关键名（金融机构编码等，ex.jrjgbm）
        /// </summary>
        [XmlElement]
        public string spname { get; set; }

    }
}
