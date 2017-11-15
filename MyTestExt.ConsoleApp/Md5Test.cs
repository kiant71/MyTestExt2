using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyTestExt.ConsoleApp
{
    public class Md5Test
    {
        public static void Do()
        {
            var str = "L0FQSS9XZWJHdWVzdC9WMS9JQ1NoYXJlR2V0fDE4OA_c_c";

            var str0 = DecodeBase64(str);

        }




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


        public static string GetMd5(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(fs);
            fs.Close();

            byte[] bytes = md5.Hash;
            md5.Clear();

            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("X2"));
            }



            return sb.ToString();
        }




    }
}
