using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace MyTestExt.ConsoleApp
{
    public class AesTest
    {
        // 密匙
        //public const string DefaultKey = "2B7E151628AED2A6ABF7158809CF4F3C";  //KeySize = 256

        //// 向量（CBC 模式用）
        //public const string DefaultIV = "0001020304050607"; //16个字节，默认BlockSize=128

        public const string DefaultKey = "P)(%&#*~<>diuej8ApU!Wm,#@:3TuoiQ";
        public const string DefaultIV = "0001020304050607";

        #region

        /// <summary>
        /// 加密（CBC模式）
        /// </summary>
        /// <param configName="strEncrypt">待加密内容</param>
        /// <param configName="strKey">密匙</param>
        /// <param configName="strIV">向量</param>
        /// <returns></returns>
        public static string Encrypt(string strEncrypt, string strKey = DefaultKey, string strIV = DefaultIV)
        {
            strKey = strKey ?? DefaultKey;
            if (strKey.Length != 32 && strKey.Length != 24 && strKey.Length != 16)
                throw new ArgumentOutOfRangeException("密匙长度异常，非128/192/256位！");
            strIV = strIV ?? DefaultIV;
            if (strIV.Length != 16)
                throw new ArgumentOutOfRangeException("向量长度异常，非128位");

            using (RijndaelManaged rDel = new RijndaelManaged())
            {
                rDel.Key = UTF8Encoding.UTF8.GetBytes(strKey);
                rDel.IV = UTF8Encoding.UTF8.GetBytes(strIV);
                rDel.Padding = PaddingMode.PKCS7;
                rDel.Mode = CipherMode.CBC;

                byte[] arrEncrypt = UTF8Encoding.UTF8.GetBytes(strEncrypt);
                ICryptoTransform cTransform = rDel.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(arrEncrypt, 0, arrEncrypt.Length);

                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
        }


        /// <summary>
        /// 解密
        /// </summary>
        /// <param configName="strDecrypt">待解密的内容，强制Base64String 字符串</param>
        /// <param configName="strKey">密匙</param>
        /// <param configName="strIV">向量（CBC 模式用）</param>
        /// <returns></returns>
        public static string Decrypt(string strDecrypt, string strKey = DefaultKey, string strIV = DefaultIV)
        {
            strKey = strKey ?? DefaultKey;
            if (strKey.Length != 32 && strKey.Length != 24 && strKey.Length != 16)
                throw new ArgumentOutOfRangeException("密匙长度异常，非128/192/256位！");
            strIV = strIV ?? DefaultIV;
            if (strIV.Length != 16)
                throw new ArgumentOutOfRangeException("向量长度异常，非128位");

            using (RijndaelManaged rDel = new RijndaelManaged())
            {
                rDel.Key = UTF8Encoding.UTF8.GetBytes(strKey);
                rDel.IV = UTF8Encoding.UTF8.GetBytes(strIV);
                rDel.Padding = PaddingMode.PKCS7;
                rDel.Mode = CipherMode.CBC;

                byte[] arrDecrypt = Convert.FromBase64String(strDecrypt);   //TODO. strDecrypt 非64位格式，转换失效的异常【是否这里需要强制验证】
                ICryptoTransform cTransform = rDel.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(arrDecrypt, 0, arrDecrypt.Length);

                return UTF8Encoding.UTF8.GetString(resultArray);
            }
        }

        #endregion
    }
}
