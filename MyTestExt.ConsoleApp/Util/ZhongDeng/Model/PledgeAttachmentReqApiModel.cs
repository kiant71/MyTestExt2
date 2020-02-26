using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyTestExt.ConsoleApp.Util.ZhongDeng.Model
{
    /// <summary>
    /// 财产附件
    /// </summary>
    public class PledgeAttachmentReqApiModel
    {
        /// <summary>
        /// 名称，【非空】【任意字符（汉字）】【最大长度30】
        /// note.附件只能是(jpg、pdf)格式，附件大小合计不超过20M,100个
        /// </summary>
        [XmlElement]
        public string attachmentname { get; set; }
    }
}
