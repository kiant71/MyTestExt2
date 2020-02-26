using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyTestExt.ConsoleApp.Util.ZhongDeng.Model
{
    [XmlRoot("feedback")]
    public class FeedbackRspApiModel
    {
        [XmlElement]
        public string registertype { get; set; }

        [XmlElement]
        public string registerresult { get; set; }

        /// <summary>
        /// 初始登记编号
        /// </summary>
        [XmlElement]
        public string registernumber { get; set; }

        /// <summary>
        /// 修改码
        /// </summary>
        [XmlElement]
        public string authorizationcode { get; set; }



        /// <summary>
        /// 错误提示：当有值则为发生错误
        /// </summary>
        [XmlElement]
        public List<FeedbackErrorApiModel> errors { get; set; }
    }

    public class FeedbackErrorApiModel
    {
        [XmlElement]
        public string error { get; set; }
    }
}
