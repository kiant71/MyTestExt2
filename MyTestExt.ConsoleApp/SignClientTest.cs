using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyTestExt.ConsoleApp
{
    public class SignClientTest
    {
        public static string PubKey = "BgIAAACkAABSU0ExAAIAAAEAAQCLllCfTO4/0MDu59lMb6uGaeLa3iBM0NPxCixc4xQN6Zt2NybbXH5X/1KrYsh24iM7KarPEpDDyFjv/XFEaIG9";

        public static void Do()
        {
            //InitKey(512);

            // 方式1：客户端通过公钥加密原始数据，然后提交到服务器端
            var baseData = "这是个原始数据信息12345！";
            var encBytes = Encrypt(baseData, PubKey);

            // 方式1：模拟服务器端收到数据，然后进行解密
            var decData = SignServerTest.Decrypt(encBytes, SignServerTest.PriKey);



            // 待处理问题：待加密的数据超长？ 原始内容进行hash，生成签名
        }




        public static byte[] Encrypt(string input, string key)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportCspBlob(Convert.FromBase64String(key));
                return rsa.Encrypt(Encoding.UTF8.GetBytes(input), false);
            }
        }


        public static bool VerifyData(string content, byte[] signature, string key)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportCspBlob(Convert.FromBase64String(key));
                return rsa.VerifyData(Encoding.UTF8.GetBytes(content), new SHA1CryptoServiceProvider(), signature);
            }
        }




        public static void InitKey(int length)
        {
            using (var rsa = new RSACryptoServiceProvider(length))
            {
                var pub = rsa.ExportCspBlob(false);
                var pubKey = Convert.ToBase64String(pub);
                
                var pri = rsa.ExportCspBlob(true);
                var priKey = Convert.ToBase64String(pri);
            }
        }

    }
}
