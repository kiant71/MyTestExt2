using Org.BouncyCastle.Utilities;
using System;
using System.Text;
using MyTestExt.ConsoleApp.Util.smcrypto;
using Org.BouncyCastle.Utilities.Encoders;

using M = Org.BouncyCastle.Math;

namespace MyTestExt.ConsoleApp
{
    /// <summary>
    /// smcrypto
    /// </summary>
    /// <remarks>
    /// https://github.com/abcsxl/smcrypto  不再维护
    /// </remarks>
    [Obsolete("魔改方式，除了中登项目，不建议使用")]
    public class SM2BysmcryptoTest4
    {
        public void Do()
        {
            DoZhongDeng();
        }

        /// <summary>
        /// smcrypto + 中登公私钥（调用通过）
        /// </summary>
        public void DoZhongDeng()
        {
            var input = "加密测试字符串123ABC456 IOP";
            var inputData = Encoding.UTF8.GetBytes(input);   // data.getBytes("UTF-8")

            // 中登的公钥解析：Base64.decode(Base64.encode(Util.hexToByte(pubk)))
            var pubHex = "04906F32D4F76720851B6E047744EAA355E5EBBF6DC64DD36C2E6057E2F57579D2CD81EB59836F863CE18E40D53D4299F768221CB85489511129E08ED73B4EB656";
            var pubData = Base64.Decode(Base64.Encode(ZhongDengUtil.HexToByte(pubHex)));
            var encryptResStr = SM2Utils.Encrypt(pubData, inputData);
            var encryptResData = Encoding.ASCII.GetBytes(encryptResStr);  // note.内里返回的 ASCII编码

            // 中登的私钥解析：Base64.decode(new String(Base64.encode(Util.hexToByte(privateKey))).getBytes())
            var priHex = "66260AFC4F41A14EBC2CEB5D787A81922C18995D477E038CF25814F5D31BE3BB";
            var priData = Base64.Decode(Strings.FromByteArray(Base64.Encode(ZhongDengUtil.HexToByte(priHex))));
            var encryptResDecodeData = Hex.Decode(encryptResData);  // note.内里会 Hex.Encode(encryptedData)
            var decryptResData = SM2Utils.Decrypt(priData, encryptResDecodeData);

            var decryptResStr = Encoding.UTF8.GetString(decryptResData);
            if (input != decryptResStr)
            {
                throw new Exception("加解密失败！");
            }
        }


        /// <summary>
        /// smcrypto + BouncyCastle生成密钥（note.失败，BouncyCastle生成密钥 无法通过 smcrypto方式解析）
        /// </summary>
        public void DoBouncyCastleEncrypt()
        {
            //CreateKey();  // 创建密钥（写入文件）

            var input = "加密测试字符串123ABC456 IOP";
            var inputData = Encoding.UTF8.GetBytes(input);   // data.getBytes("UTF-8")

            // BouncyCastle公钥
            var pubData = SM2ByBouncyCastleTest.GetPubKey();
            var encryptResStr = SM2Utils.Encrypt(pubData, inputData);  // note.BouncyCastle生成密钥 无法通过 smcrypto方式解析
            var encryptResData = Encoding.ASCII.GetBytes(encryptResStr);  // note.内里返回的 ASCII编码

            // BouncyCastle私钥
            var priData = SM2ByBouncyCastleTest.GetPriKey();
            var decryptResData = SM2Utils.Decrypt(priData, encryptResData);

            var decryptResStr = Encoding.UTF8.GetString(decryptResData);
            if (input != decryptResStr)
            {
                throw new Exception("加解密失败！");
            }


        }



        //public void Do1()
        //{
        //    String sm2Pri = "66260AFC4F41A14EBC2CEB5D787A81922C18995D477E038CF25814F5D31BE3BB";
        //    String sm2Pub = "04906F32D4F76720851B6E047744EAA355E5EBBF6DC64DD36C2E6057E2F57579D2CD81EB59836F863CE18E40D53D4299F768221CB85489511129E08ED73B4EB656";
        //    String encryptStr = "加密测试字符串123ABC456 IOP";

        //    var pubData = Base64.Decode(Base64.Encode(ZhongDengUtil.HexToByte(sm2Pub)));
        //    var encryptStrData = Encoding.UTF8.GetBytes(encryptStr);
        //    var encryptResData = SM2Utils.Encrypt(pubData, encryptStrData);
        //    var encryptResStr = ZhongDengUtil.ByteToHex(encryptResData);


        //    var priStr = Strings.FromByteArray(Base64.Encode(ZhongDengUtil.HexToByte(sm2Pri)));
        //    var decryptResData = ZhongDengUtil.decrypt(Strings.ToByteArray(priStr), ZhongDengUtil.HexToByte(encryptResStr));
        //    var decryptResStr = Strings.FromByteArray(decryptResData);
        //    var decryptResStr2 = Encoding.UTF8.GetString(decryptResData);

        //    if (encryptStr != decryptResStr)
        //    {
        //        throw new Exception("加解密失败！");
        //    }
        //}










    }


    

}
