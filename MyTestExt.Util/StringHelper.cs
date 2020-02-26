using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyTestExt.Utils
{
    public class StringHelper
    {
        /// <summary>
        /// 转换为base64码 并将特殊字符 /  +  = 替换成_a _b _c  
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncodeBase64(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return string.Empty;
            string base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
            return base64String.Replace("/", "_a").Replace("+", "_b").Replace("=", "_c");
        }

        /// <summary>
        /// 解码base64 并将特殊字符 _a _b _c  替换成/  +  = 
        /// </summary>
        /// <param name="base64Str"></param>
        /// <returns></returns>
        public static string DecodeBase64(string base64Str)
        {
            if (string.IsNullOrWhiteSpace(base64Str)) return string.Empty;
            string temp = base64Str.Replace("_a", "/").Replace("_b", "+").Replace("_c", "=");
            try
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(temp));
            }
            catch
            {
                return base64Str;
            }
        }


        public static string HtmlEncode(string str)
        {
            return string.IsNullOrWhiteSpace(str) ? "" : HttpUtility.HtmlEncode(str);
        }

        public static string HtmlDecode(string str)
        {
            return string.IsNullOrWhiteSpace(str) ? "" : HttpUtility.HtmlDecode(str);
        }


        public static string UrlEncode(string str)
        {
            return string.IsNullOrWhiteSpace(str) ? "" : HttpUtility.UrlEncode(str);
        }

        public static string UrlDecode(string str)
        {
            return string.IsNullOrWhiteSpace(str) ? "" : HttpUtility.UrlDecode(str);
        }

        /// <summary>
        /// 调整为 decimal 可识别的格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TryDecimal(string str)
        {
            if (decimal.TryParse(str, out var ret))
                return ret.ToString(CultureInfo.InvariantCulture);

            return default(decimal).ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 字符串替换，不区分大小写
        /// </summary>
        /// <param name="input">原始字符串</param>
        /// <param name="pattern">需要被替换的字符串</param>
        /// <param name="replacement">替换成的字符串</param>
        /// <returns></returns>
        /// <![CDATA[
        /// ex. var input = "中地abc位， 阿aBC斯顿撒Abc多拉Ac代理ABC费开始的开始都放声大哭";
        /// var pattern = "abc";
        /// var target = "---";
        /// res : 中地---位， 阿---斯顿撒---多拉Ac代理---费开始的开始都放声大哭
        /// ]]>
        public static string ReplaceIgnoreCase(string input, string pattern, string replacement)
        {
            return System.Text.RegularExpressions.Regex.Replace(
                input, pattern, replacement, 
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }
    }
}
