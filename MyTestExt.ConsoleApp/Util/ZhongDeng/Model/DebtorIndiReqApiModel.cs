using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyTestExt.ConsoleApp.Util.ZhongDeng.Model
{
    /// <summary>
    /// 出质人 - 当类型为个体工商户时（debtortype=06）
    /// </summary>
    public class DebtorIndiReqApiModel : DebtorBaseReqApiModel
    {
        /// <summary>
        /// 字号名称，【可空】【任意字符（汉字）】【最大字符100】
        /// </summary>
        [XmlElement]
        public string tradename { get; set; }

        /// <summary>
        /// 证件类型，【非空】
        /// </summary>
        [XmlElement]
        public string certificatetype { get; set; }

        /// <summary>
        /// 护照颁发国家，，【非空】【任意字符（汉字）】【最大字符3】
        /// note.证件类型必须是护照
        /// note.具体编码参见附件-国家代码部分
        /// </summary>
        [XmlElement]
        public string distributecountry { get; set; }

        /// <summary>
        /// 证件号码，【非空】【数字、ASCII字符】【最大字符18 或者 30】
        /// </summary>
        /// <![CDATA[
        /// 证件类型为身份证时，长度为15位或18位。
        ///     15位时，全为数字，
        ///     18位时，前17位全为数字，最后一位是数字或X；
        /// 证件类型为港澳居民来往内地通行证、台湾居民来往内地通行证、护照时，长度不超过30位
        /// ]]>
        [XmlElement]
        public string certificatecode { get; set; }

        /// <summary>
        /// 护照到期日，"YYYY-MM-DD"
        /// note.护照有值，同时护照到期日必需晚于当前日期
        /// </summary>
        [XmlElement]
        public string certificateexpirationdate { get; set; }

        /// <summary>
        /// 住所标签
        /// </summary>
        //[XmlElement("address")]
        public AddressReqApiModel address { get; set; }
    }
}
