using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MyTestExt.ConsoleApp.Util;
using MyTestExt.Utils.Crypto;

namespace MyTestExt.ConsoleApp
{

    // 参考 “数字签名是什么？”  作者： 阮一峰  http://www.ruanyifeng.com/blog/2011/08/what_is_a_digital_signature.html

    public class RSATest2
    {
        public static string PubKey = "BgIAAACkAABSU0ExAAIAAAEAAQD79nsWXDci0XU8TNFrXiEdpIYRSl1T98U858CThi0qFLyQkJfxWlQ8w/f39psSNv+peFPPLLApBS4t10eYfLXC";

        public static string PriKey = "BwIAAACkAABSU0EyAAIAAAEAAQD79nsWXDci0XU8TNFrXiEdpIYRSl1T98U858CThi0qFLyQkJfxWlQ8w/f39psSNv+peFPPLLApBS4t10eYfLXCn+HWmadhZ9qJ9ZaRXvk/CaHOQ5xitpA8+uUDUH9OM/glxXI6zl60uY//TSuJ7wnmwo1vNn0Sf16MDYl7rNvTyDmyu3SemK9E6s1NP3Y5dQ5FS3eYCo3ywpwqAxIncSm+fTrBNyL3vovhfLbyL1R9QZ0fMJg3o89lxY6G+A+GTYQmmxurOvP926eJrBFA0SsKl1dOjcN+JxbPKDHqzMERTCHBFXBLDIfAtAs0ja4GVxs7VgmAnFujgdwd6gAZgbj2E9ESU6kfe0a/a7qCcot9754OY3l1Y0H/UcmGLdBM0LQ=";

        public static void Do1()
        {
            var a1 = RsaCrypto.CreateKey();

            // 方式1：【客户端】-【公钥】加密原始数据，然后提交到服务器端
            var baseStr = "这是个原始数据信息12345！";
            var encBytes = RsaCrypto.Encrypt(baseStr, PubKey);
            var encStr = Encoding.UTF8.GetString(encBytes);

            // 方式1：【服务器】-【私钥】解密收到的数据
            var decStr = RsaCrypto.Decrypt(encBytes, PriKey);
            if (baseStr != decStr)
            {
                var flag = false;
            }


            // 长内容加密
            baseStr = "500是应用程序错误。如果仅仅是cpu忙不过来，或者带宽不够，是不会返回这个状态的。你应该先并发测试好你的服务程序。在一个测试环境（例如一个console程序）使用10个线程，每一个线程连续调用10遍所有的服务，不500是应用程序错误。如果仅仅是cpu忙不过来，或者带宽不够，是不会返回这个状态的。你应该先并发测试好你的服务程序。在一个测试环境（例如一个console程序）使用10个线程，每一个线程连续调用10遍所有的服务，不500是应用程序错误。如果仅仅是cpu忙不过来，或者带宽不够，是不会返回这个状态的。你应该先并发测试好你的服务程序。在一个测试环境（例如一个console程序）使用10个线程，每一个线程连续调用10遍所有的服务，不500是应用程序错误。如果仅仅是cpu忙不过来，或者带宽不够，是不会返回这个状态的。你应该先并发测试好你的服务程序。在一个测试环境（例如一个console程序）使用10个线程，每一个线程连续调用10遍所有的服务，不";
            encBytes = RsaCrypto.Encrypt(baseStr, PubKey);   // 数据过长的情况下，原始内容是否需要压缩
            decStr = RsaCrypto.Decrypt(encBytes, PriKey);
            if (string.Compare(baseStr, decStr, StringComparison.OrdinalIgnoreCase ) > 0)
            {
                var flag = false;
            }


            // 待处理问题：待加密的数据超长？ 原始内容进行hash，生成签名
        }

        public static void Do2()
        {
            // 方式三：引入第三方CA 数据证书，封装发送方的公钥进行传递

            // 方式三：1.模拟第三方生成 证书（将服务端的公钥加密到证书中）
            var ca_PriKey =
                "BwIAAACkAABSU0EyAAIAAAEAAQC/c0qBLYxtG9OfiDqSTcx7vDxWrF7JaLeVmnKp6D52Em7v8Sgmew0BjAl4AqfaAmg84UFrya690zAAVCdildDLa7+u+WXb3fUqzxwCvc6ggq/2G6G/ZWOvhXfF44fMbe79lQqwcrSHMwZj+alDJt0fqNdBOzZOcj/ZhLFuHMHV2uk3rjjMeAY7UqpgPv5fAU0IrzSRu2PyiZJ+w8AWFeORpcE/QbXoIfVyJ0OfWM6HXjtHQ8ziFotNGUslT6Vd0JRucnran/N2iDrtjba8OFO+zNHlWTvxndSHHuMep8fwtXGZf+g5GuHUvL4NVeJYFl7IlmjrdEZAHbJyql4HCWPh0V2ksCK5xtmco2vfgiM1aw/tHxg9R7AHSacpjYvSKrI=";
            var ca_CertByte = RsaCrypto.Encrypt(PubKey, ca_PriKey);

            // 方式三：2.模拟第三方通过浏览器证书（解密获得服务器端的公钥）
            var ca_PubKey = "BgIAAACkAABSU0ExAAIAAAEAAQC/c0qBLYxtG9OfiDqSTcx7vDxWrF7JaLeVmnKp6D52Em7v8Sgmew0BjAl4AqfaAmg84UFrya690zAAVCdildDL";
            PubKey = RsaCrypto.Decrypt(ca_CertByte, ca_PubKey);

            // 方式三：3.这样确保了 接收方（客户端）关于 发送方（服务器端）的 公钥是正确的！




            // 然后走方式二的【签名】校验（防篡改）


            // 方式二：对内容进行HASH，生成签名，好处1.防止待加密内容过长 2.对方可校验发送方。3.对方可校验内容是否被篡改
            var content = "假设这是个很长很长很长的内容12345！";
            var signBytes = RsaCrypto.Sign(content, PriKey);

            // 方式二：模拟接收方，校验签名，对比内容HASH值
            var flag = RsaCrypto.VerifySign(content, signBytes, PubKey);
        }

        public static void Do3()
        {
            // 方式二：对内容进行HASH，生成签名，好处1.防止待加密内容过长 2.对方可校验发送方。3.对方可校验内容是否被篡改
            var content = "假设这是个很长很长很长的内容12345！";
            var signBytes = RsaCrypto.Sign(content, PriKey);

            // 方式二：模拟接收方，校验签名，对比内容HASH值
            var flag = RsaCrypto.VerifySign(content, signBytes, PubKey);

        }


        /// <summary>
        /// 众签设置
        /// </summary>
        public static void Do4()
        {
            var a1 = new Tuple<string, string>("BgIAAACkAABSU0ExAAIAAAEAAQDZ6JMQ17GTH2xkOFpAhKqf8QLHcILohHClxazPfeKTjmCH8pQHvwD9HXQBwXKsE1Q+/uXAgfQjoKoi8T8AP//G"
                , "BwIAAACkAABSU0EyAAIAAAEAAQDZ6JMQ17GTH2xkOFpAhKqf8QLHcILohHClxazPfeKTjmCH8pQHvwD9HXQBwXKsE1Q+/uXAgfQjoKoi8T8AP//G/2zH4Qt30Nl7cGf0+pEsa3havVGfGQhjxl87uVbZoPYnspdJq42Xj5zWfwATuLN6caDibmtkfZ5K2JhXugyPzlN5kWB3+VR4fd7ln4DRfZX9KN1nfhccVZlNQh5dH4QIC4VWAghbFQ2okVTnUPRVCOwqA2gUntiNb06p5azDdzTKX4rUKjKM6fIKpdIUd7ij0Xbzwh1fFMJ/F05PlsJrIxm0vbxCcDUhjVpjG+0ej6XvqM7hGkBlZ56syHexVWQw0C0JJe5U7LKPNWLqrZPbwM6OVbM34/Y71nzMYIjMQyM=");
            //var a1 = RSACrypto.CreateKey();
            var _webPubKey = a1.Item1;
            var _apiPriKey = a1.Item2;


            { // 对比
                var k = "EEkmSkywFuI17FdILoG1UHomXkMMtexRdHTKL%2bndiVr2ef7Vfpu1sTrBwUYqCGi3pibLDxMb1eOp7e6NKf9ZMg%3d%3d";
                var arr1 = Convert.FromBase64String(ConvertValue.UrlDecode(k));
                var va = RsaCrypto.Decrypt(arr1, _apiPriKey);
            }



            // 方式1：【客户端】-【公钥】加密原始数据，然后提交到服务器端
            var baseStr = 26.ToString();
            var encBytes = RsaCrypto.Encrypt(baseStr, _webPubKey);
            var key = Convert.ToBase64String(encBytes);
            var keyUrl = ConvertValue.UrlEncode(key);

          
            // 方式1：【服务器】-【私钥】解密收到的数据
            var key3 = ConvertValue.UrlDecode(keyUrl);
            var arrVal = Convert.FromBase64String(key3);
            var decStr = RsaCrypto.Decrypt(arrVal, _apiPriKey);


            if (baseStr != decStr)
            {
                var flag = false;
            }
        }

    }





}
