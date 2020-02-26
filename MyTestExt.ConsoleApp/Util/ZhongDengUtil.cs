using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Serializers;
using MyTestExt.ConsoleApp.Util.smcrypto;
using MyTestExt.ConsoleApp.Util.ZhongDeng;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Utilities.Encoders;

namespace MyTestExt.ConsoleApp.Util
{
    public class ZhongDengUtil
    {


//        public static string encrypt(string pubStr, string input)
//        {
//            /*
//public static String encryptByKeyStr(String data, String pubk)
//{ return Util.byteToHex(encrypt(Base64.decode(Base64.encode(Util.hexToByte(pubk)))
//                        , data.getBytes("UTF-8"))); }
// */

//            // pubKey字符串转字节（中登的转义方式）
//            var pubData = Base64.Decode(Base64.Encode(HexToByte(pubStr)));
//            var data = Encoding.UTF8.GetBytes(input);

//            var encryptData = encrypt(pubData, data);

//            var result = ByteToHex(encryptData);
//            return result;
//        }





        //public static byte[] encrypt(byte[] publicKey, byte[] data)
        //{
        //    if (null == publicKey || publicKey.Length == 0)
        //    {
        //        return null;
        //    }
        //    if (data == null || data.Length == 0)
        //    {
        //        return null;
        //    }

        //    byte[] source = new byte[data.Length];
        //    Array.Copy(data, 0, source, 0, data.Length);

        //    SM2Cipher cipher = new SM2Cipher();
        //    SM2 sm2 = SM2.Instance();
        //    ECPoint userKey = sm2.ecc_curve.DecodePoint(publicKey);

        //    ECPoint c1 = cipher.Init_enc(sm2, userKey);
        //    cipher.Encrypt(source);
        //    byte[] c3 = new byte[32];
        //    cipher.Dofinal(c3);

        //    // 自换
        //    //var c1Data = Hex.Encode(c1.GetEncoded());
        //    //var sourceData = Hex.Encode(source);
        //    //var c3Data = Hex.Encode(c3);
        //    //var buff = new byte[c1Data.Length + sourceData.Length + c3Data.Length];
        //    //Array.Copy(c1Data, buff, c1Data.Length);
        //    //Array.Copy(sourceData, 0, buff, c1Data.Length, sourceData.Length);
        //    //Array.Copy(c3Data, 0, buff, c1Data.Length + sourceData.Length, c3Data.Length);
        //    //return buff;

        //    //// 参考
        //    //String sc1 = Encoding.ASCII.GetString(Hex.Encode(c1.GetEncoded()));
        //    //String sc2 = Encoding.ASCII.GetString(Hex.Encode(source));
        //    //String sc3 = Encoding.ASCII.GetString(Hex.Encode(c3));
        //    //return Encoding.ASCII.GetBytes(sc1 + sc2 + sc3);

        //    // 原Java  ASN1Sequence
        //    DerInteger x = new DerInteger(c1.Normalize().XCoord.ToBigInteger());
        //    DerInteger y = new DerInteger(c1.Normalize().YCoord.ToBigInteger());
        //    DerOctetString derDig = new DerOctetString(c3);   
        //    DerOctetString derEnc = new DerOctetString(source);
        //    Asn1EncodableVector v = new Asn1EncodableVector();
        //    v.Add(x);
        //    v.Add(y);
        //    v.Add(derDig);
        //    v.Add(derEnc);
        //    DerSequence seq = new DerSequence(v);
        //    MemoryStream bos = new MemoryStream();
        //    DerOutputStream dos = new DerOutputStream(bos);
        //    dos.WriteObject(seq);
        //    return bos.ToArray();
        //}



        

        //public static byte[] decrypt(byte[] privateKey, byte[] encryptedData)
        //{
        //    if (privateKey == null || privateKey.Length == 0)
        //    {
        //        return null;
        //    }

        //    if (encryptedData == null || encryptedData.Length == 0)
        //    {
        //        return null;
        //    }

        //    byte[] enc = new byte[encryptedData.Length];
        //    Array.Copy(encryptedData, 0, enc, 0, encryptedData.Length);

        //    SM2 sm2 = SM2.Instance();
        //    BigInteger userD = new BigInteger(1, privateKey);

        //    // ASN1Sequence
        //    //MemoryStream bis = new MemoryStream(enc);
        //    Asn1InputStream dis = new Asn1InputStream(enc);
            
        //    Asn1Object derObj = dis.ReadObject();
        //    DerSequence asn1 = (DerSequence) derObj;
        //    DerInteger x = (DerInteger) asn1.GetObjectAt(0);
        //    DerInteger y = (DerInteger) asn1.GetObjectAt(1);
        //    ECPoint c1 = sm2.ecc_curve
        //        .CreatePoint(x.Value, y.Value, true);

        //    SM2Cipher cipher = new SM2Cipher();
        //    cipher.Init_dec(userD, c1);
        //    DerOctetString data = (DerOctetString) asn1.GetObjectAt(3);
        //    enc = data.GetOctets();
        //    cipher.Decrypt(enc);
        //    byte[] c3 = new byte[32];
        //    cipher.Dofinal(c3);
        //    return enc;
        //}



        //public static byte[] byteConvert32Bytes(BigInteger n)
        //{
        //    byte[] tmpd = (byte[])null;
        //    if (n == null)
        //    {
        //        return null;
        //    }

        //    if (n.ToByteArray().Length == 33)
        //    {
        //        tmpd = new byte[32];
        //        Array.Copy(n.ToByteArray(), 1, tmpd, 0, 32);
        //    }
        //    else if (n.ToByteArray().Length == 32)
        //    {
        //        tmpd = n.ToByteArray();
        //    }
        //    else
        //    {
        //        tmpd = new byte[32];
        //        for (int i = 0; i < 32 - n.ToByteArray().Length; i++)
        //        {
        //            tmpd[i] = 0;
        //        }
        //        Array.Copy(n.ToByteArray(), 0, tmpd,
        //            32 - n.ToByteArray().Length, n.ToByteArray().Length);
        //    }
        //    return tmpd;
        //}





  


    }
}