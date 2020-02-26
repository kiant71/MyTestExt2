using System;
using System.Security.Cryptography;
using System.Text;

namespace MyTestExt.Utils.Crypto
{
    /// <summary>
    /// RSA 加密/解密类
    /// </summary>
    public class RsaCrypto
    {
        private static Encoding Encoder = Encoding.UTF8;

        public const int KeySize = 512;

        /// <summary>
        /// 创建公钥 和 私钥
        /// </summary>
        /// <returns>Item1:PubKey，Item2:PriKey</returns>
        public static Tuple<string, string> CreateKey(int length = KeySize)
        {
            using (var rsa = new RSACryptoServiceProvider(length))
            {
                var pub = rsa.ExportCspBlob(false);
                var pubKey = Convert.ToBase64String(pub);

                var pri = rsa.ExportCspBlob(true);
                var priKey = Convert.ToBase64String(pri);

                return new Tuple<string, string>(pubKey, priKey);
            }
        }



        /// <summary>
        /// 加密
        /// </summary>
        public static byte[] Encrypt(string text, string key, int length = KeySize)
        {
            var data = Encoder.GetBytes(text);

            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportCspBlob(Convert.FromBase64String(key));

                // 待加密的字节数不能超过密钥的长度值除以 8 再减去 11
                var maxBlockSize = length / 8 - 11;
                if (text.Length <= maxBlockSize)
                    return rsa.Encrypt(data, false);

                // 分段加密，先将内容构建成输入流，分段读取、加密、缓存到加密流中
                using (var encrStream = new System.IO.MemoryStream())
                {
                    using (var dataStream = new System.IO.MemoryStream(data))
                    {
                        var buff = new byte[maxBlockSize];

                        var buffRead = dataStream.Read(buff, 0, maxBlockSize);
                        while (buffRead > 0)
                        {
                            var tmpEncr = rsa.Encrypt(buff, false);
                            encrStream.Write(tmpEncr, 0, tmpEncr.Length);

                            //
                            buff = new byte[maxBlockSize];
                            buffRead = dataStream.Read(buff, 0, maxBlockSize);
                        }
                    }

                    return encrStream.ToArray();
                }
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        public static string Decrypt(byte[] data, string key, int length = KeySize)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportCspBlob(Convert.FromBase64String(key));

                // 待解密的密文的字节数，正好是密钥的长度值除以 8
                var maxBlockSize = length / 8;
                if (data.Length <= maxBlockSize)
                    return Encoder.GetString(rsa.Decrypt(data, false));

                // 分段加密，分段读取流数据、解密、缓存，最后统一转成字符
                using (var decrStream = new System.IO.MemoryStream())
                {
                    using (var dataStream = new System.IO.MemoryStream(data))
                    {
                        var buff = new byte[maxBlockSize];

                        var buffRead = dataStream.Read(buff, 0, maxBlockSize);
                        while (buffRead > 0)
                        {
                            var decrBlock = new byte[buff.Length];
                            Array.Copy(buff, decrBlock, buff.Length);

                            var tmpDecr = rsa.Decrypt(decrBlock, false);
                            decrStream.Write(tmpDecr, 0, tmpDecr.Length);

                            //
                            buff = new byte[maxBlockSize];
                            buffRead = dataStream.Read(buff, 0, maxBlockSize);
                        }
                    }

                    return Encoder.GetString(decrStream.ToArray());
                }

            }
        }



        /// <summary>
        /// 数字签名
        /// </summary>
        public static byte[] Sign(string content, string key)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportCspBlob(Convert.FromBase64String(key));
                return rsa.SignData(Encoding.UTF8.GetBytes(content), new SHA1CryptoServiceProvider());
            }
        }

        /// <summary>
        /// 校验 数字签名
        /// </summary>
        public static bool VerifySign(string content, byte[] signature, string key)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportCspBlob(Convert.FromBase64String(key));
                return rsa.VerifyData(Encoding.UTF8.GetBytes(content), new SHA1CryptoServiceProvider(), signature);
            }
        }

    }
}
