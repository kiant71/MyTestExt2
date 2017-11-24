using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyTestExt.ConsoleApp
{
    
    public class SignServerTest
    {
        public static string PriKey = "BwIAAACkAABSU0EyAAIAAAEAAQCLllCfTO4/0MDu59lMb6uGaeLa3iBM0NPxCixc4xQN6Zt2NybbXH5X/1KrYsh24iM7KarPEpDDyFjv/XFEaIG9V6b1Er6e73tH2hK6f6PpE/r0i6BSVSjQ3Wkv/tGUaertKFhOWQyU7ui8lv2hSeAlaZO6SP8RvISu/Dej/x31zs/eUDwbxe4j9Wl7FQBFFbieKm4WO3nx7+YyvB0bxT/TsWG+qzUGXb2IJuHtO/SO/tmfuTKFqf/5wpEpZUncrXmwfg3f4fZbpvkMNkQqlP9BaQRpVdXipaNLlLRR696gxMkt2d/41z9r5b36PFMwJWp9Ud7gPTtgjy7KzbIbeaKHvH/3MBQwyKYfReFJZp6AgbYIY2mk34V8Fgas+sJN8mE=";


        public static void Do()
        {
            // 方式三：引入第三方CA 数据证书，封装发送方的公钥进行传递

            // 方式三：1.模拟第三方生成 证书（将服务端的公钥加密到证书中）
            var ca_PriKey =
                "BwIAAACkAABSU0EyAAIAAAEAAQC/c0qBLYxtG9OfiDqSTcx7vDxWrF7JaLeVmnKp6D52Em7v8Sgmew0BjAl4AqfaAmg84UFrya690zAAVCdildDLa7+u+WXb3fUqzxwCvc6ggq/2G6G/ZWOvhXfF44fMbe79lQqwcrSHMwZj+alDJt0fqNdBOzZOcj/ZhLFuHMHV2uk3rjjMeAY7UqpgPv5fAU0IrzSRu2PyiZJ+w8AWFeORpcE/QbXoIfVyJ0OfWM6HXjtHQ8ziFotNGUslT6Vd0JRucnran/N2iDrtjba8OFO+zNHlWTvxndSHHuMep8fwtXGZf+g5GuHUvL4NVeJYFl7IlmjrdEZAHbJyql4HCWPh0V2ksCK5xtmco2vfgiM1aw/tHxg9R7AHSacpjYvSKrI=";
            var ca_CertByte = Encrypt(SignClientTest.PubKey, ca_PriKey);

            // 方式三：2.模拟第三方通过浏览器证书（解密获得服务器端的公钥）
            var ca_PubKey = "BgIAAACkAABSU0ExAAIAAAEAAQC/c0qBLYxtG9OfiDqSTcx7vDxWrF7JaLeVmnKp6D52Em7v8Sgmew0BjAl4AqfaAmg84UFrya690zAAVCdildDL";
            SignClientTest.PubKey = Decrypt(ca_CertByte, ca_PubKey);

            // 方式三：3.这样确保了 接收方（客户端）关于 发送方（服务器端）的 公钥是正确的！



            // 其下走方式二的校验。


            // 方式二：对内容进行HASH，生成签名，好处1.防止待加密内容过长 2.对方可校验发送方。3.对方可校验内容是否被篡改
            var content = "假设这是个很长很长很长的内容12345！";
            var signBytes = Sign(content, PriKey);

            // 方式二：模拟接收方，校验签名，对比内容HASH值
            var flag = SignClientTest.VerifyData(content, signBytes, SignClientTest.PubKey);
        }

        public static void Do2()
        {
            // 方式二：对内容进行HASH，生成签名，好处1.防止待加密内容过长 2.对方可校验发送方。3.对方可校验内容是否被篡改
            var content = "假设这是个很长很长很长的内容12345！";
            var signBytes = Sign(content, PriKey);

            // 方式二：模拟接收方，校验签名，对比内容HASH值
            var flag = SignClientTest.VerifyData(content, signBytes, SignClientTest.PubKey);

        }




        /// <summary>
        /// 加密
        /// </summary>
        public static byte[] Encrypt(string input, string key)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportCspBlob(Convert.FromBase64String(key));
                return rsa.Encrypt(Encoding.UTF8.GetBytes(input), false);
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        public static string Decrypt(byte[] data, string key)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportCspBlob(Convert.FromBase64String(key));
                var decBytes = rsa.Decrypt(data, false);
                return Encoding.UTF8.GetString(decBytes);
            }
        }

        public static byte[] Sign(string content, string key)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportCspBlob(Convert.FromBase64String(key));
                return rsa.SignData(Encoding.UTF8.GetBytes(content), new SHA1CryptoServiceProvider());
            }
        }


    }
}
