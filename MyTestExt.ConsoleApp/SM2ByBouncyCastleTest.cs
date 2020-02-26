using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;

using M = Org.BouncyCastle.Math;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;

namespace MyTestExt.ConsoleApp
{
    /// <summary>
    /// BouncyCastle 示例
    /// </summary>
    /// <remarks>
    /// github: https://github.com/bcgit/bc-csharp
    /// Bouncy Castle C# Distribution (Mirror) https://www.bouncycastle.org/csharp
    /// </remarks>
    public class SM2ByBouncyCastleTest
    {
        public void Do()
        {
            

            //DoByKey();

            //DoByKey2();

            //DoEngineTestFp();

            //DoEngineTestF2m();

            //DoEngineTestF2c();
        }

        #region CreateKey
        public void CreateKey()
        {
            BigInteger SM2_ECC_A = new BigInteger("00", 16);
            BigInteger SM2_ECC_B = new BigInteger("E78BCD09746C202378A7E72B12BCE00266B9627ECB0B5A25367AD1AD4CC6242B", 16);
            BigInteger SM2_ECC_N = new BigInteger("7FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFBC972CF7E6B6F900945B3C6A0CF6161D", 16);
            BigInteger SM2_ECC_H = BigInteger.ValueOf(4);
            BigInteger SM2_ECC_GX = new BigInteger("00CDB9CA7F1E6B0441F658343F4B10297C0EF9B6491082400A62E7A7485735FADD", 16);
            BigInteger SM2_ECC_GY = new BigInteger("013DE74DA65951C4D76DC89220D5F7777A611B1C38BAE260B175951DC8060C2B3E", 16);

            ECCurve curve = new F2mCurve(257, 12, SM2_ECC_A, SM2_ECC_B, SM2_ECC_N, SM2_ECC_H);

            ECPoint g = curve.CreatePoint(SM2_ECC_GX, SM2_ECC_GY);
            ECDomainParameters domainParams = new ECDomainParameters(curve, g, SM2_ECC_N, SM2_ECC_H);

            ECKeyPairGenerator keyPairGenerator = new ECKeyPairGenerator();

            ECKeyGenerationParameters aKeyGenParams = new ECKeyGenerationParameters(domainParams, new TestRandomBigInteger("56A270D17377AA9A367CFA82E46FA5267713A9B91101D0777B07FCE018C757EB", 16));

            keyPairGenerator.Init(aKeyGenParams);

            AsymmetricCipherKeyPair aKp = keyPairGenerator.GenerateKeyPair();

            ECPublicKeyParameters aPub = (ECPublicKeyParameters)aKp.Public;
            ECPrivateKeyParameters aPriv = (ECPrivateKeyParameters)aKp.Private;

            // 公、私密钥保存
            savetheKey(aPub, aPriv);
        }

        private void savetheKey(AsymmetricKeyParameter publicKey, AsymmetricKeyParameter privateKey)
        {
            //保存公钥到文件
            SubjectPublicKeyInfo publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKey);
            Asn1Object aobject = publicKeyInfo.ToAsn1Object();
            byte[] pubInfoByte = aobject.GetEncoded();
            FileStream fs = new FileStream(@"D:/1.Desktop/a.pub", FileMode.Create, FileAccess.Write);
            fs.Write(pubInfoByte, 0, pubInfoByte.Length);
            fs.Close();

            var pubBase64Str = Convert.ToBase64String(pubInfoByte);

            //保存私钥到文件
            /*
            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKey);
            aobject = privateKeyInfo.ToAsn1Object();
            byte[] priInfoByte = aobject.GetEncoded();
            fs = new FileStream(@"E:/Desktop/a.pri", FileMode.Create, FileAccess.Write);
            fs.Write(priInfoByte, 0, priInfoByte.Length);
            fs.Close();
            */
            string alg = "1.2.840.113549.1.12.1.3"; // 3 key triple DES with SHA-1
            byte[] salt = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            int count = 1000;
            char[] password = "123456".ToCharArray();
            EncryptedPrivateKeyInfo enPrivateKeyInfo = EncryptedPrivateKeyInfoFactory.CreateEncryptedPrivateKeyInfo(
                alg,
                password,
                salt,
                count,
                privateKey);
            byte[] priInfoByte = enPrivateKeyInfo.ToAsn1Object().GetEncoded();
            fs = new FileStream(@"D:/1.Desktop/a.pri", FileMode.Create, FileAccess.Write);
            fs.Write(priInfoByte, 0, priInfoByte.Length);
            fs.Close();
            //还原
            //PrivateKeyInfo priInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(password, enPrivateKeyInfo);
            //AsymmetricKeyParameter privateKey = PrivateKeyFactory.CreateKey(priInfoByte);

        }
        
        public static byte[] GetPubKey()
        {
            // 读取本地的公钥文件，尝试进行Hex字符化
            var fs = new FileStream(@"D:/1.Desktop/a.pub", FileMode.Open, FileAccess.Read);
            byte[] pubBytes = new byte[fs.Length];
            fs.Read(pubBytes, 0, pubBytes.Length);
            fs.Close();

            return pubBytes;
        }

        public static byte[] GetPriKey()
        {
            var aobj = Asn1Object.FromStream(new FileStream(@"D:/1.Desktop/a.pri", FileMode.Open, FileAccess.Read));
            var enpri = EncryptedPrivateKeyInfo.GetInstance(aobj);
            var password = "123456".ToCharArray();
            var priKey = PrivateKeyInfoFactory.CreatePrivateKeyInfo(password, enpri);
            var tmp = priKey.GetEncoded();
            return tmp;
        }

        #endregion

        /// <summary>
        /// 连 密钥都是自己成功
        /// </summary>
        public void DoEncrypt()
        {
            //CreateKey();  // 创建密钥（写入文件）

            var pubData = GetPubKey();
            var priData = GetPriKey();
            var input = "popozh AAA test";
            var data = Encoding.UTF8.GetBytes(input);

            var sm2Engine = new SM2Engine();

            // 加密
            var pubObj = (ECPublicKeyParameters)PublicKeyFactory.CreateKey(pubData);
            sm2Engine.Init(true, new ParametersWithRandom(pubObj
                , new TestRandomBigInteger("6D3B497153E3E92524E5C122682DBDC8705062E20B917A5F8FCDB8EE4C66663D", 16)));
            var enc = sm2Engine.ProcessBlock(data, 0, data.Length);


            // 解密
            var priObj = (ECPrivateKeyParameters)PrivateKeyFactory.CreateKey(priData);
            sm2Engine.Init(false, priObj);
            var dec = sm2Engine.ProcessBlock(enc, 0, enc.Length);

            var decStr = Strings.FromByteArray(dec);
            if (input != decStr)
            {
                throw new Exception("加密解密失败！");
            }

        }





        //private void DoEngineTestFp()
        //{
        //    BigInteger SM2_ECC_P = new BigInteger("8542D69E4C044F18E8B92435BF6FF7DE457283915C45517D722EDB8B08F1DFC3", 16);
        //    BigInteger SM2_ECC_A = new BigInteger("787968B4FA32C3FD2417842E73BBFEFF2F3C848B6831D7E0EC65228B3937E498", 16);
        //    BigInteger SM2_ECC_B = new BigInteger("63E4C6D3B23B0C849CF84241484BFE48F61D59A5B16BA06E6E12D1DA27C5249A", 16);
        //    BigInteger SM2_ECC_N = new BigInteger("8542D69E4C044F18E8B92435BF6FF7DD297720630485628D5AE74EE7C32E79B7", 16);
        //    BigInteger SM2_ECC_H = BigInteger.One;
        //    BigInteger SM2_ECC_GX = new BigInteger("421DEBD61B62EAB6746434EBC3CC315E32220B3BADD50BDC4C4E6C147FEDD43D", 16);
        //    BigInteger SM2_ECC_GY = new BigInteger("0680512BCBB42C07D47349D2153B70C4E5D7FDFCBFA36EA1A85841B9E46E09A2", 16);

        //    ECCurve curve = new FpCurve(SM2_ECC_P, SM2_ECC_A, SM2_ECC_B, SM2_ECC_N, SM2_ECC_H);

        //    ECPoint g = curve.CreatePoint(SM2_ECC_GX, SM2_ECC_GY);
        //    ECDomainParameters domainParams = new ECDomainParameters(curve, g, SM2_ECC_N);

        //    ECKeyPairGenerator keyPairGenerator = new ECKeyPairGenerator();

        //    ECKeyGenerationParameters aKeyGenParams = new ECKeyGenerationParameters(domainParams, new TestRandomBigInteger("1649AB77A00637BD5E2EFE283FBF353534AA7F7CB89463F208DDBC2920BB0DA0", 16));

        //    keyPairGenerator.Init(aKeyGenParams);

        //    AsymmetricCipherKeyPair aKp = keyPairGenerator.GenerateKeyPair();

        //    ECPublicKeyParameters aPub = (ECPublicKeyParameters)aKp.Public;
        //    ECPrivateKeyParameters aPriv = (ECPrivateKeyParameters)aKp.Private;

        //    SM2Engine sm2Engine = new SM2Engine();

        //    byte[] m = Strings.ToByteArray("encryption standard");

        //    // 加密
        //    sm2Engine.Init(true, new ParametersWithRandom(aPub, new TestRandomBigInteger("4C62EEFD6ECFC2B95B92FD6C3D9575148AFA17425546D49018E5388D49DD7B4F", 16)));
        //    byte[] enc = sm2Engine.ProcessBlock(m, 0, m.Length);
            

        //    // 解密
        //    sm2Engine.Init(false, aPriv);
        //    byte[] dec = sm2Engine.ProcessBlock(enc, 0, enc.Length);

        //    //IsTrue("dec wrong", Arrays.AreEqual(m, dec));

        //    enc[80] = (byte)(enc[80] + 1);

        //    try
        //    {
        //        sm2Engine.ProcessBlock(enc, 0, enc.Length);
        //        //Fail("no exception");
        //    }
        //    catch (InvalidCipherTextException e)
        //    {
        //        //IsTrue("wrong exception", "invalid cipher text".Equals(e.Message));
        //    }

        //    // long message
        //    sm2Engine = new SM2Engine();

        //    m = new byte[4097];
        //    for (int i = 0; i != m.Length; i++)
        //    {
        //        m[i] = (byte)i;
        //    }

        //    sm2Engine.Init(true, new ParametersWithRandom(aPub, new TestRandomBigInteger("4C62EEFD6ECFC2B95B92FD6C3D9575148AFA17425546D49018E5388D49DD7B4F", 16)));

        //    enc = sm2Engine.ProcessBlock(m, 0, m.Length);

        //    sm2Engine.Init(false, aPriv);

        //    dec = sm2Engine.ProcessBlock(enc, 0, enc.Length);

        //    //IsTrue("dec wrong", Arrays.AreEqual(m, dec));
        //}

        //private void DoEngineTestF2m()
        //{
        //    BigInteger SM2_ECC_A = new BigInteger("00", 16);
        //    BigInteger SM2_ECC_B = new BigInteger("E78BCD09746C202378A7E72B12BCE00266B9627ECB0B5A25367AD1AD4CC6242B", 16);
        //    BigInteger SM2_ECC_N = new BigInteger("7FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFBC972CF7E6B6F900945B3C6A0CF6161D", 16);
        //    BigInteger SM2_ECC_H = BigInteger.ValueOf(4);
        //    BigInteger SM2_ECC_GX = new BigInteger("00CDB9CA7F1E6B0441F658343F4B10297C0EF9B6491082400A62E7A7485735FADD", 16);
        //    BigInteger SM2_ECC_GY = new BigInteger("013DE74DA65951C4D76DC89220D5F7777A611B1C38BAE260B175951DC8060C2B3E", 16);

        //    ECCurve curve = new F2mCurve(257, 12, SM2_ECC_A, SM2_ECC_B, SM2_ECC_N, SM2_ECC_H);

        //    ECPoint g = curve.CreatePoint(SM2_ECC_GX, SM2_ECC_GY);
        //    ECDomainParameters domainParams = new ECDomainParameters(curve, g, SM2_ECC_N, SM2_ECC_H);

        //    ECKeyPairGenerator keyPairGenerator = new ECKeyPairGenerator();

        //    ECKeyGenerationParameters aKeyGenParams = new ECKeyGenerationParameters(domainParams, new TestRandomBigInteger("56A270D17377AA9A367CFA82E46FA5267713A9B91101D0777B07FCE018C757EB", 16));

        //    keyPairGenerator.Init(aKeyGenParams);

        //    AsymmetricCipherKeyPair aKp = keyPairGenerator.GenerateKeyPair();

        //    ECPublicKeyParameters aPub = (ECPublicKeyParameters)aKp.Public;
        //    ECPrivateKeyParameters aPriv = (ECPrivateKeyParameters)aKp.Private;

           


        //    SM2Engine sm2Engine = new SM2Engine();

        //    byte[] data = Strings.ToByteArray("encryption standard");


        //    // 指定key
        //    var clientKey = "04593D186F5AB68871891C235A35ED90E6D5E8DC33414C89E05C29EBF5D19C3919F0D1956568619BA138AC724F4E5DC730266C8186F610D8CC85BFC0748F899695";
        //    var keyArr = Strings.ToByteArray(clientKey);
        //    var keyObj = (object) Asn1Object.FromByteArray(keyArr);
        //    var keyInfo = SubjectPublicKeyInfo.GetInstance(keyObj);
        //    AsymmetricKeyParameter pubKey = PublicKeyFactory.CreateKey(keyInfo);
        //    //ECPublicKeyParameters pubKey = (ECPublicKeyParameters)PublicKeyFactory.CreateKey(keyArr);
        //    aPub = (ECPublicKeyParameters) pubKey;

        //    // 加密
        //    var pubArr = aPub.Q.GetEncoded();
        //    var pubStr = Encoding.UTF8.GetString(pubArr);
        //    sm2Engine.Init(true, new ParametersWithRandom(aPub
        //        , new TestRandomBigInteger("6D3B497153E3E92524E5C122682DBDC8705062E20B917A5F8FCDB8EE4C66663D", 16)));
        //    byte[] enc = sm2Engine.ProcessBlock(data, 0, data.Length);
        //    var encStr = Strings.FromByteArray(enc);
            
        //    // 解密
        //    sm2Engine.Init(false, aPriv);
        //    byte[] dec = sm2Engine.ProcessBlock(enc, 0, enc.Length);
        //    var decStr = Strings.FromByteArray(dec);
        //}

        



        //private void DoByKey()
        //{
        //    string input = "popozh AAA test";
        //    byte[] testData = Encoding.UTF8.GetBytes(input);


        //    // 读取公钥加密
        //    SM2Engine sm2Engine = new SM2Engine();
        //    ECPublicKeyParameters aPub = (ECPublicKeyParameters)PublicKeyFactory.CreateKey(
        //        new FileStream(@"D:/1.Desktop/a.pub", FileMode.Open, FileAccess.Read));
        //    sm2Engine.Init(true, new ParametersWithRandom(aPub
        //        , new TestRandomBigInteger("6D3B497153E3E92524E5C122682DBDC8705062E20B917A5F8FCDB8EE4C66663D", 16)));
        //    byte[] enc = sm2Engine.ProcessBlock(testData, 0, testData.Length);


        //    // 读取私钥解密
        //    Asn1Object aobj = Asn1Object.FromStream(new FileStream(@"D:/1.Desktop/a.pri", FileMode.Open, FileAccess.Read));   //a.pvk??
        //    EncryptedPrivateKeyInfo enpri = EncryptedPrivateKeyInfo.GetInstance(aobj);
        //    char[] password = "123456".ToCharArray();
        //    PrivateKeyInfo priKey = PrivateKeyInfoFactory.CreatePrivateKeyInfo(password, enpri);    //解密
        //    ECPrivateKeyParameters aPriv = (ECPrivateKeyParameters)PrivateKeyFactory.CreateKey(priKey);
        //    sm2Engine.Init(false, aPriv);
        //    byte[] dec = sm2Engine.ProcessBlock(enc, 0, enc.Length);
        //    var decStr = Strings.FromByteArray(dec);

        //    if (input != decStr)
        //    {

        //    }
        //}


        //private void DoEngineTestF2c()
        //{
        //    BigInteger SM2_ECC_A = new BigInteger("00", 16);
        //    BigInteger SM2_ECC_B = new BigInteger("E78BCD09746C202378A7E72B12BCE00266B9627ECB0B5A25367AD1AD4CC6242B", 16);
        //    BigInteger SM2_ECC_N = new BigInteger("7FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFBC972CF7E6B6F900945B3C6A0CF6161D", 16);
        //    BigInteger SM2_ECC_H = BigInteger.ValueOf(4);
        //    BigInteger SM2_ECC_GX = new BigInteger("00CDB9CA7F1E6B0441F658343F4B10297C0EF9B6491082400A62E7A7485735FADD", 16);
        //    BigInteger SM2_ECC_GY = new BigInteger("013DE74DA65951C4D76DC89220D5F7777A611B1C38BAE260B175951DC8060C2B3E", 16);

        //    ECCurve curve = new F2mCurve(257, 12, SM2_ECC_A, SM2_ECC_B, SM2_ECC_N, SM2_ECC_H);

        //    ECPoint g = curve.CreatePoint(SM2_ECC_GX, SM2_ECC_GY);
        //    ECDomainParameters domainParams = new ECDomainParameters(curve, g, SM2_ECC_N, SM2_ECC_H);

        //    ECKeyPairGenerator keyPairGenerator = new ECKeyPairGenerator();

        //    ECKeyGenerationParameters aKeyGenParams = new ECKeyGenerationParameters(domainParams, new TestRandomBigInteger("56A270D17377AA9A367CFA82E46FA5267713A9B91101D0777B07FCE018C757EB", 16));

        //    keyPairGenerator.Init(aKeyGenParams);

        //    AsymmetricCipherKeyPair aKp = keyPairGenerator.GenerateKeyPair();

        //    ECPublicKeyParameters aPub = (ECPublicKeyParameters)aKp.Public;
        //    ECPrivateKeyParameters aPriv = (ECPrivateKeyParameters)aKp.Private;

        //    var pubKey = Strings.FromByteArray(Hex.Encode(aPub.Q
        //                .GetEncoded())); //Encoding.UTF8.GetString(Hex.Encode(aPub.Q.GetEncoded())).ToUpper();
        //    byte[] pubKeys = aPub.Q.GetEncoded();
        //    byte[] m = Strings.ToByteArray("encryption standard");

        //    //var encStr = Encrypt(pubKeys, m);
        //}


        ////public static String Encrypt(byte[] publicKey, byte[] data)
        ////{
        ////    if (null == publicKey || publicKey.Length == 0)
        ////    {
        ////        return null;
        ////    }
        ////    if (data == null || data.Length == 0)
        ////    {
        ////        return null;
        ////    }

        ////    byte[] source = new byte[data.Length];
        ////    Array.Copy(data, 0, source, 0, data.Length);

        ////    Cipher cipher = new Cipher();
        ////    SM2 sm2 = SM2.Instance;

        ////    ECPoint userKey = sm2.ecc_curve.DecodePoint(publicKey);

        ////    ECPoint c1 = cipher.Init_enc(sm2, userKey);
        ////    cipher.Encrypt(source);

        ////    byte[] c3 = new byte[32];
        ////    cipher.Dofinal(c3);

        ////    String sc1 = Encoding.ASCII.GetString(Hex.Encode(c1.GetEncoded()));
        ////    String sc2 = Encoding.ASCII.GetString(Hex.Encode(source));
        ////    String sc3 = Encoding.ASCII.GetString(Hex.Encode(c3));

        ////    return (sc1 + sc2 + sc3).ToUpper();
        ////}



        ////private void DoByKey2()
        ////{
        ////    var input = "popozh AAA test";
        ////    var data = Encoding.UTF8.GetBytes(input);

        ////    var sm2Engine = new SM2Engine();

        ////    // 读取公钥加密
        ////    var pubStr =
        ////        "04593D186F5AB68871891C235A35ED90E6D5E8DC33414C89E05C29EBF5D19C3919F0D1956568619BA138AC724F4E5DC730266C8186F610D8CC85BFC0748F899695";
        ////    var pubData = Convert.FromBase64String(Convert.ToBase64String(HexStringToBytes(pubStr)));
        ////    Cipher cipher = new Cipher();
        ////    SM2 sm2 = SM2.Instance;
        ////    ECPoint userKey = sm2.ecc_curve.DecodePoint(pubData);

        ////    ECPoint c1 = cipher.Init_enc(sm2, userKey);
        ////    cipher.Encrypt(data);

        ////    byte[] c3 = new byte[32];
        ////    cipher.Dofinal(c3);

        ////    //ASN1Integer x = new ASInteger(c1.XCoord.ToBigInteger());
        ////    //ASN1Integer y = new ASN1Integer(c1.getY().toBigInteger());
        ////    //DEROctetString derDig = new DEROctetString(c3);
        ////    //DEROctetString derEnc = new DEROctetString(source);
        ////    //ASN1EncodableVector v = new ASN1EncodableVector();
        ////    //v.add(x);
        ////    //v.add(y);
        ////    //v.add(derDig);
        ////    //v.add(derEnc);
        ////    //DERSequence seq = new DERSequence(v);
        ////    //ByteArrayOutputStream bos = new ByteArrayOutputStream();
        ////    //DEROutputStream dos = new DEROutputStream(bos);
        ////    //dos.writeObject(seq);


        ////    var pubKey = (ECPublicKeyParameters)PublicKeyFactory.CreateKey(pubData);
        ////    //var fs = new FileStream(@"D:/1.Desktop/a.pub", FileMode.Open, FileAccess.Read);
        ////    //var len = fs.Length;

        ////    sm2Engine.Init(true, new ParametersWithRandom(pubKey
        ////        , new TestRandomBigInteger("6D3B497153E3E92524E5C122682DBDC8705062E20B917A5F8FCDB8EE4C66663D", 16)));
        ////    var enc = sm2Engine.ProcessBlock(data, 0, data.Length);


        ////    // 读取私钥解密
        ////    Asn1Object aobj = Asn1Object.FromStream(new FileStream(@"D:/1.Desktop/a.pri", FileMode.Open, FileAccess.Read));   //a.pvk??
        ////    EncryptedPrivateKeyInfo enpri = EncryptedPrivateKeyInfo.GetInstance(aobj);
        ////    char[] password = "123456".ToCharArray();
        ////    PrivateKeyInfo priKey = PrivateKeyInfoFactory.CreatePrivateKeyInfo(password, enpri);    //解密
        ////    ECPrivateKeyParameters aPriv = (ECPrivateKeyParameters)PrivateKeyFactory.CreateKey(priKey);
        ////    sm2Engine.Init(false, aPriv);
        ////    byte[] dec = sm2Engine.ProcessBlock(enc, 0, enc.Length);
        ////    var decStr = Strings.FromByteArray(dec);

        ////    if (input != decStr)
        ////    {

        ////    }
        ////}



        //public string BytesToHexString(byte[] data)
        //{
        //    var sb = new StringBuilder(data.Length * 3);
        //    foreach (var b in data)
        //    {
        //        sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
        //    }

        //    return sb.ToString().ToUpper();
        //}


        //public byte[] HexStringToBytes(string hex)
        //{
        //    var buffer = new byte[hex.Length / 2];
        //    for (var i = 0; i < hex.Length; i += 2)
        //    {
        //        buffer[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        //    }

        //    return buffer;
        //}



    }


    #region test

    //public class Cipher
    //{
    //    private int ct;
    //    private ECPoint p2;
    //    private SM3Digest sm3keybase;
    //    private SM3Digest sm3c3;
    //    private byte[] key;
    //    private byte keyOff;

    //    public Cipher()
    //    {
    //        this.ct = 1;
    //        this.key = new byte[32];
    //        this.keyOff = 0;
    //    }

    //    public static byte[] byteConvert32Bytes(BigInteger n)
    //    {
    //        byte[] tmpd = null;
    //        if (n == null)
    //        {
    //            return null;
    //        }

    //        if (n.ToByteArray().Length == 33)
    //        {
    //            tmpd = new byte[32];
    //            Array.Copy(n.ToByteArray(), 1, tmpd, 0, 32);
    //        }
    //        else if (n.ToByteArray().Length == 32)
    //        {
    //            tmpd = n.ToByteArray();
    //        }
    //        else
    //        {
    //            tmpd = new byte[32];
    //            for (int i = 0; i < 32 - n.ToByteArray().Length; i++)
    //            {
    //                tmpd[i] = 0;
    //            }
    //            Array.Copy(n.ToByteArray(), 0, tmpd, 32 - n.ToByteArray().Length, n.ToByteArray().Length);
    //        }
    //        return tmpd;
    //    }

    //    private void Reset()
    //    {
    //        this.sm3keybase = new SM3Digest();
    //        this.sm3c3 = new SM3Digest();

    //        byte[] p = byteConvert32Bytes(p2.Normalize().XCoord.ToBigInteger());
    //        this.sm3keybase.BlockUpdate(p, 0, p.Length);
    //        this.sm3c3.BlockUpdate(p, 0, p.Length);

    //        p = byteConvert32Bytes(p2.Normalize().YCoord.ToBigInteger());
    //        this.sm3keybase.BlockUpdate(p, 0, p.Length);
    //        this.ct = 1;
    //        NextKey();
    //    }

    //    private void NextKey()
    //    {
    //        SM3Digest sm3keycur = new SM3Digest(this.sm3keybase);
    //        sm3keycur.Update((byte)(ct >> 24 & 0xff));
    //        sm3keycur.Update((byte)(ct >> 16 & 0xff));
    //        sm3keycur.Update((byte)(ct >> 8 & 0xff));
    //        sm3keycur.Update((byte)(ct & 0xff));
    //        sm3keycur.DoFinal(key, 0);
    //        this.keyOff = 0;
    //        this.ct++;
    //    }

    //    public ECPoint Init_enc(SM2 sm2, ECPoint userKey)
    //    {
    //        AsymmetricCipherKeyPair key = sm2.ecc_key_pair_generator.GenerateKeyPair();
    //        ECPrivateKeyParameters ecpriv = (ECPrivateKeyParameters)key.Private;
    //        ECPublicKeyParameters ecpub = (ECPublicKeyParameters)key.Public;
    //        BigInteger k = ecpriv.D;
    //        ECPoint c1 = ecpub.Q;
    //        this.p2 = userKey.Multiply(k);
    //        Reset();
    //        return c1;
    //    }

    //    public void Encrypt(byte[] data)
    //    {
    //        this.sm3c3.BlockUpdate(data, 0, data.Length);
    //        for (int i = 0; i < data.Length; i++)
    //        {
    //            if (keyOff == key.Length)
    //            {
    //                NextKey();
    //            }
    //            data[i] ^= key[keyOff++];
    //        }
    //    }

    //    public void Init_dec(BigInteger userD, ECPoint c1)
    //    {
    //        this.p2 = c1.Multiply(userD);
    //        Reset();
    //    }

    //    public void Decrypt(byte[] data)
    //    {
    //        for (int i = 0; i < data.Length; i++)
    //        {
    //            if (keyOff == key.Length)
    //            {
    //                NextKey();
    //            }
    //            data[i] ^= key[keyOff++];
    //        }

    //        this.sm3c3.BlockUpdate(data, 0, data.Length);
    //    }

    //    public void Dofinal(byte[] c3)
    //    {
    //        byte[] p = byteConvert32Bytes(p2.Normalize().YCoord.ToBigInteger());
    //        this.sm3c3.BlockUpdate(p, 0, p.Length);
    //        this.sm3c3.DoFinal(c3, 0);
    //        Reset();
    //    }
    //}

    ////public class SM3Digest : GeneralDigest
    ////{
    ////    public override string AlgorithmName
    ////    {
    ////        get
    ////        {
    ////            return "SM3";
    ////        }

    ////    }
    ////    public override int GetDigestSize()
    ////    {
    ////        return DIGEST_LENGTH;
    ////    }

    ////    private const int DIGEST_LENGTH = 32;

    ////    private static readonly int[] v0 = new int[] { 0x7380166f, 0x4914b2b9, 0x172442d7, unchecked((int)0xda8a0600), unchecked((int)0xa96f30bc), 0x163138aa, unchecked((int)0xe38dee4d), unchecked((int)0xb0fb0e4e) };

    ////    private int[] v = new int[8];
    ////    private int[] v_ = new int[8];

    ////    private static readonly int[] X0 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    ////    private int[] X = new int[68];
    ////    private int xOff;

    ////    private int T_00_15 = 0x79cc4519;
    ////    private int T_16_63 = 0x7a879d8a;

    ////    public SM3Digest()
    ////    {
    ////        Reset();
    ////    }

    ////    public SM3Digest(SM3Digest t) : base(t)
    ////    {

    ////        Array.Copy(t.X, 0, X, 0, t.X.Length);
    ////        xOff = t.xOff;

    ////        Array.Copy(t.v, 0, v, 0, t.v.Length);
    ////    }

    ////    public override void Reset()
    ////    {
    ////        base.Reset();

    ////        Array.Copy(v0, 0, v, 0, v0.Length);

    ////        xOff = 0;
    ////        Array.Copy(X0, 0, X, 0, X0.Length);
    ////    }

    ////    internal override void ProcessBlock()
    ////    {
    ////        int i;

    ////        int[] ww = X;
    ////        int[] ww_ = new int[64];

    ////        for (i = 16; i < 68; i++)
    ////        {
    ////            ww[i] = P1(ww[i - 16] ^ ww[i - 9] ^ (ROTATE(ww[i - 3], 15))) ^ (ROTATE(ww[i - 13], 7)) ^ ww[i - 6];
    ////        }

    ////        for (i = 0; i < 64; i++)
    ////        {
    ////            ww_[i] = ww[i] ^ ww[i + 4];
    ////        }

    ////        int[] vv = v;
    ////        int[] vv_ = v_;

    ////        Array.Copy(vv, 0, vv_, 0, v0.Length);

    ////        int SS1, SS2, TT1, TT2, aaa;
    ////        for (i = 0; i < 16; i++)
    ////        {
    ////            aaa = ROTATE(vv_[0], 12);
    ////            SS1 = aaa + vv_[4] + ROTATE(T_00_15, i);
    ////            SS1 = ROTATE(SS1, 7);
    ////            SS2 = SS1 ^ aaa;

    ////            TT1 = FF_00_15(vv_[0], vv_[1], vv_[2]) + vv_[3] + SS2 + ww_[i];
    ////            TT2 = GG_00_15(vv_[4], vv_[5], vv_[6]) + vv_[7] + SS1 + ww[i];
    ////            vv_[3] = vv_[2];
    ////            vv_[2] = ROTATE(vv_[1], 9);
    ////            vv_[1] = vv_[0];
    ////            vv_[0] = TT1;
    ////            vv_[7] = vv_[6];
    ////            vv_[6] = ROTATE(vv_[5], 19);
    ////            vv_[5] = vv_[4];
    ////            vv_[4] = P0(TT2);
    ////        }
    ////        for (i = 16; i < 64; i++)
    ////        {
    ////            aaa = ROTATE(vv_[0], 12);
    ////            SS1 = aaa + vv_[4] + ROTATE(T_16_63, i);
    ////            SS1 = ROTATE(SS1, 7);
    ////            SS2 = SS1 ^ aaa;

    ////            TT1 = FF_16_63(vv_[0], vv_[1], vv_[2]) + vv_[3] + SS2 + ww_[i];
    ////            TT2 = GG_16_63(vv_[4], vv_[5], vv_[6]) + vv_[7] + SS1 + ww[i];
    ////            vv_[3] = vv_[2];
    ////            vv_[2] = ROTATE(vv_[1], 9);
    ////            vv_[1] = vv_[0];
    ////            vv_[0] = TT1;
    ////            vv_[7] = vv_[6];
    ////            vv_[6] = ROTATE(vv_[5], 19);
    ////            vv_[5] = vv_[4];
    ////            vv_[4] = P0(TT2);
    ////        }
    ////        for (i = 0; i < 8; i++)
    ////        {
    ////            vv[i] ^= vv_[i];
    ////        }

    ////        // Reset
    ////        xOff = 0;
    ////        Array.Copy(X0, 0, X, 0, X0.Length);
    ////    }

    ////    internal override void ProcessWord(byte[] in_Renamed, int inOff)
    ////    {
    ////        int n = in_Renamed[inOff] << 24;
    ////        n |= (in_Renamed[++inOff] & 0xff) << 16;
    ////        n |= (in_Renamed[++inOff] & 0xff) << 8;
    ////        n |= (in_Renamed[++inOff] & 0xff);
    ////        X[xOff] = n;

    ////        if (++xOff == 16)
    ////        {
    ////            ProcessBlock();
    ////        }
    ////    }

    ////    internal override void ProcessLength(long bitLength)
    ////    {
    ////        if (xOff > 14)
    ////        {
    ////            ProcessBlock();
    ////        }

    ////        X[14] = (int)(SupportClass.URShift(bitLength, 32));
    ////        X[15] = (int)(bitLength & unchecked((int)0xffffffff));
    ////    }

    ////    public static void IntToBigEndian(int n, byte[] bs, int off)
    ////    {
    ////        bs[off] = (byte)(SupportClass.URShift(n, 24));
    ////        bs[++off] = (byte)(SupportClass.URShift(n, 16));
    ////        bs[++off] = (byte)(SupportClass.URShift(n, 8));
    ////        bs[++off] = (byte)(n);
    ////    }

    ////    public override int DoFinal(byte[] out_Renamed, int outOff)
    ////    {
    ////        Finish();

    ////        for (int i = 0; i < 8; i++)
    ////        {
    ////            IntToBigEndian(v[i], out_Renamed, outOff + i * 4);
    ////        }

    ////        Reset();

    ////        return DIGEST_LENGTH;
    ////    }

    ////    private int ROTATE(int x, int n)
    ////    {
    ////        return (x << n) | (SupportClass.URShift(x, (32 - n)));
    ////    }

    ////    private int P0(int X)
    ////    {
    ////        return ((X) ^ ROTATE((X), 9) ^ ROTATE((X), 17));
    ////    }

    ////    private int P1(int X)
    ////    {
    ////        return ((X) ^ ROTATE((X), 15) ^ ROTATE((X), 23));
    ////    }

    ////    private int FF_00_15(int X, int Y, int Z)
    ////    {
    ////        return (X ^ Y ^ Z);
    ////    }

    ////    private int FF_16_63(int X, int Y, int Z)
    ////    {
    ////        return ((X & Y) | (X & Z) | (Y & Z));
    ////    }

    ////    private int GG_00_15(int X, int Y, int Z)
    ////    {
    ////        return (X ^ Y ^ Z);
    ////    }

    ////    private int GG_16_63(int X, int Y, int Z)
    ////    {
    ////        return ((X & Y) | (~X & Z));
    ////    }

    ////    //[STAThread]
    ////    //public static void  Main()
    ////    //{
    ////    //    byte[] md = new byte[32];
    ////    //    byte[] msg1 = Encoding.Default.GetBytes("ererfeiisgod");
    ////    //    SM3Digest sm3 = new SM3Digest();
    ////    //    sm3.BlockUpdate(msg1, 0, msg1.Length);
    ////    //    sm3.DoFinal(md, 0);
    ////    //    System.String s = new UTF8Encoding().GetString(Hex.Encode(md));
    ////    //    System.Console.Out.WriteLine(s.ToUpper());

    ////    //    Console.ReadLine();
    ////    //}
    ////}

    ////public class SM2
    ////{
    ////    public static SM2 Instance
    ////    {
    ////        get
    ////        {
    ////            return new SM2();
    ////        }

    ////    }
    ////    public static SM2 InstanceTest
    ////    {
    ////        get
    ////        {
    ////            return new SM2();
    ////        }

    ////    }

    ////    public static readonly string[] sm2_param = {
    ////        "FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FFFFFFFFFFFFFFFF",// p,0
    ////        "FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FFFFFFFFFFFFFFFC",// a,1
    ////        "28E9FA9E9D9F5E344D5A9E4BCF6509A7F39789F515AB8F92DDBCBD414D940E93",// b,2
    ////        "FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFF7203DF6B21C6052B53BBF40939D54123",// n,3
    ////        "32C4AE2C1F1981195F9904466A39C9948FE30BBFF2660BE1715A4589334C74C7",// gx,4
    ////        "BC3736A2F4F6779C59BDCEE36B692153D0A9877CC62A474002DF32E52139F0A0" // gy,5
    ////    };

    ////    public string[] ecc_param = sm2_param;

    ////    public readonly BigInteger ecc_p;
    ////    public readonly BigInteger ecc_a;
    ////    public readonly BigInteger ecc_b;
    ////    public readonly BigInteger ecc_n;
    ////    public readonly BigInteger ecc_gx;
    ////    public readonly BigInteger ecc_gy;

    ////    public readonly ECCurve ecc_curve;
    ////    public readonly ECPoint ecc_point_g;

    ////    public readonly ECDomainParameters ecc_bc_spec;

    ////    public readonly ECKeyPairGenerator ecc_key_pair_generator;

    ////    private SM2()
    ////    {
    ////        ecc_param = sm2_param;

    ////        ecc_p = new BigInteger(ecc_param[0], 16);
    ////        ecc_a = new BigInteger(ecc_param[1], 16);
    ////        ecc_b = new BigInteger(ecc_param[2], 16);
    ////        ecc_n = new BigInteger(ecc_param[3], 16);
    ////        ecc_gx = new BigInteger(ecc_param[4], 16);
    ////        ecc_gy = new BigInteger(ecc_param[5], 16);

    ////        ecc_curve = new FpCurve(ecc_p, ecc_a, ecc_b, null, null);
    ////        ecc_point_g = ecc_curve.CreatePoint(ecc_gx, ecc_gy);

    ////        ecc_bc_spec = new ECDomainParameters(ecc_curve, ecc_point_g, ecc_n);

    ////        ECKeyGenerationParameters ecc_ecgenparam;
    ////        ecc_ecgenparam = new ECKeyGenerationParameters(ecc_bc_spec, new SecureRandom());

    ////        ecc_key_pair_generator = new ECKeyPairGenerator();
    ////        ecc_key_pair_generator.Init(ecc_ecgenparam);
    ////    }

    ////    public virtual byte[] Sm2GetZ(byte[] userId, ECPoint userKey)
    ////    {
    ////        SM3Digest sm3 = new SM3Digest();
    ////        byte[] p;
    ////        // userId length
    ////        int len = userId.Length * 8;
    ////        sm3.Update((byte)(len >> 8 & 0x00ff));
    ////        sm3.Update((byte)(len & 0x00ff));

    ////        // userId
    ////        sm3.BlockUpdate(userId, 0, userId.Length);

    ////        // a,b
    ////        p = ecc_a.ToByteArray();
    ////        sm3.BlockUpdate(p, 0, p.Length);
    ////        p = ecc_b.ToByteArray();
    ////        sm3.BlockUpdate(p, 0, p.Length);
    ////        // gx,gy
    ////        p = ecc_gx.ToByteArray();
    ////        sm3.BlockUpdate(p, 0, p.Length);
    ////        p = ecc_gy.ToByteArray();
    ////        sm3.BlockUpdate(p, 0, p.Length);

    ////        // x,y
    ////        p = userKey.AffineXCoord.ToBigInteger().ToByteArray();
    ////        sm3.BlockUpdate(p, 0, p.Length);
    ////        p = userKey.AffineYCoord.ToBigInteger().ToByteArray();
    ////        sm3.BlockUpdate(p, 0, p.Length);

    ////        // Z
    ////        byte[] md = new byte[sm3.GetDigestSize()];
    ////        sm3.DoFinal(md, 0);

    ////        return md;
    ////    }
    ////}


    //public class SupportClass
    //{
    //    /// <summary>
    //    /// Performs an unsigned bitwise right shift with the specified number
    //    /// </summary>
    //    ///<param name="number">Number to operate on
    //    ///<param name="bits">Ammount of bits to shift
    //    /// <returns>The resulting number from the shift operation</returns>
    //    public static int URShift(int number, int bits)
    //    {
    //        if (number >= 0)
    //            return number >> bits;
    //        else
    //            return (number >> bits) + (2 << ~bits);
    //    }

    //    /// <summary>
    //            /// Performs an unsigned bitwise right shift with the specified number
    //            /// </summary>
    //            ///<param name="number">Number to operate on
    //            ///<param name="bits">Ammount of bits to shift
    //            /// <returns>The resulting number from the shift operation</returns>
    //    public static int URShift(int number, long bits)
    //    {
    //        return URShift(number, (int)bits);
    //    }

    //    /// <summary>
    //            /// Performs an unsigned bitwise right shift with the specified number
    //            /// </summary>
    //            ///<param name="number">Number to operate on
    //            ///<param name="bits">Ammount of bits to shift
    //            /// <returns>The resulting number from the shift operation</returns>
    //    public static long URShift(long number, int bits)
    //    {
    //        if (number >= 0)
    //            return number >> bits;
    //        else
    //            return (number >> bits) + (2L << ~bits);
    //    }

    //    /// <summary>
    //            /// Performs an unsigned bitwise right shift with the specified number
    //            /// </summary>
    //            ///<param name="number">Number to operate on
    //            ///<param name="bits">Ammount of bits to shift
    //            /// <returns>The resulting number from the shift operation</returns>
    //    public static long URShift(long number, long bits)
    //    {
    //        return URShift(number, (int)bits);
    //    }


    //}


    //public abstract class GeneralDigest : IDigest
    //{
    //    private const int BYTE_LENGTH = 64;

    //    private byte[] xBuf;
    //    private int xBufOff;

    //    private long byteCount;

    //    internal GeneralDigest()
    //    {
    //        xBuf = new byte[4];
    //    }

    //    internal GeneralDigest(GeneralDigest t)
    //    {
    //        xBuf = new byte[t.xBuf.Length];
    //        Array.Copy(t.xBuf, 0, xBuf, 0, t.xBuf.Length);

    //        xBufOff = t.xBufOff;
    //        byteCount = t.byteCount;
    //    }

    //    public void Update(byte input)
    //    {
    //        xBuf[xBufOff++] = input;

    //        if (xBufOff == xBuf.Length)
    //        {
    //            ProcessWord(xBuf, 0);
    //            xBufOff = 0;
    //        }

    //        byteCount++;
    //    }

    //    public void BlockUpdate(
    //    byte[] input,
    //    int inOff,
    //    int length)
    //    {
    //        //
    //        // fill the current word
    //        //
    //        while ((xBufOff != 0) && (length > 0))
    //        {
    //            Update(input[inOff]);
    //            inOff++;
    //            length--;
    //        }

    //        //
    //        // process whole words.
    //        //
    //        while (length > xBuf.Length)
    //        {
    //            ProcessWord(input, inOff);

    //            inOff += xBuf.Length;
    //            length -= xBuf.Length;
    //            byteCount += xBuf.Length;
    //        }

    //        //
    //        // load in the remainder.
    //        //
    //        while (length > 0)
    //        {
    //            Update(input[inOff]);

    //            inOff++;
    //            length--;
    //        }
    //    }

    //    public void Finish()
    //    {
    //        long bitLength = (byteCount << 3);

    //        //
    //        // add the pad bytes.
    //        //
    //        Update(unchecked((byte)128));

    //        while (xBufOff != 0) Update(unchecked((byte)0));
    //        ProcessLength(bitLength);
    //        ProcessBlock();
    //    }

    //    public virtual void Reset()
    //    {
    //        byteCount = 0;
    //        xBufOff = 0;
    //        Array.Clear(xBuf, 0, xBuf.Length);
    //    }

    //    public int GetByteLength()
    //    {
    //        return BYTE_LENGTH;
    //    }

    //    internal abstract void ProcessWord(byte[] input, int inOff);
    //    internal abstract void ProcessLength(long bitLength);
    //    internal abstract void ProcessBlock();
    //    public abstract string AlgorithmName { get; }
    //    public abstract int GetDigestSize();
    //    public abstract int DoFinal(byte[] output, int outOff);
    //}


    #endregion


    #region bouncycastle

    public class TestRandomBigInteger
    : FixedSecureRandom
    {
        /**
         * Constructor from a base 10 represention of a BigInteger.
         *
         * @param encoding a base 10 represention of a BigInteger.
         */
        public TestRandomBigInteger(string encoding)
            : this(encoding, 10)
        {
        }

        /**
         * Constructor from a base radix represention of a BigInteger.
         *
         * @param encoding a String BigInteger of base radix.
         * @param radix the radix to use.
         */
        public TestRandomBigInteger(string encoding, int radix)
            : base(new FixedSecureRandom.Source[] { new FixedSecureRandom.BigInteger(BigIntegers.AsUnsignedByteArray(new M.BigInteger(encoding, radix))) })
        {
        }

        /**
         * Constructor based on a byte array.
         *
         * @param encoding a 2's complement representation of the BigInteger.
         */
        public TestRandomBigInteger(byte[] encoding)
            : base(new FixedSecureRandom.Source[] { new FixedSecureRandom.BigInteger(encoding) })
        {
        }

        /**
         * Constructor which ensures encoding will produce a BigInteger from a request from the passed in bitLength.
         *
         * @param bitLength bit length for the BigInteger data request.
         * @param encoding bytes making up the encoding.
         */
        public TestRandomBigInteger(int bitLength, byte[] encoding)
            : base(new FixedSecureRandom.Source[] { new FixedSecureRandom.BigInteger(bitLength, encoding) })
        {
        }
    }

    public class FixedSecureRandom
        : SecureRandom
    {
        private static readonly M.BigInteger REGULAR = new M.BigInteger("01020304ffffffff0506070811111111", 16);
        private static readonly M.BigInteger ANDROID = new M.BigInteger("1111111105060708ffffffff01020304", 16);
        private static readonly M.BigInteger CLASSPATH = new M.BigInteger("3020104ffffffff05060708111111", 16);

        private static readonly bool isAndroidStyle;
        private static readonly bool isClasspathStyle;
        private static readonly bool isRegularStyle;

        static FixedSecureRandom()
        {
            M.BigInteger check1 = new M.BigInteger(128, new RandomChecker());
            M.BigInteger check2 = new M.BigInteger(120, new RandomChecker());

            isAndroidStyle = check1.Equals(ANDROID);
            isRegularStyle = check1.Equals(REGULAR);
            isClasspathStyle = check2.Equals(CLASSPATH);
        }

        private byte[] _data;
        private int _index;

        /**
         * Base class for sources of fixed "Randomness"
         */
        public class Source
        {
            internal byte[] data;

            internal Source(byte[] data)
            {
                this.data = data;
            }
        }

        /**
         * Data Source - in this case we just expect requests for byte arrays.
         */
        public class Data
            : Source
        {
            public Data(byte[] data)
                : base(data)
            {
            }
        }

        /**
         * BigInteger Source - in this case we expect requests for data that will be used
         * for BigIntegers. The FixedSecureRandom will attempt to compensate for platform differences here.
         */
        public class BigInteger
            : Source
        {
            public BigInteger(byte[] data)
                : base(data)
            {
            }

            public BigInteger(int bitLength, byte[] data)
                : base(ExpandToBitLength(bitLength, data))
            {
            }

            public BigInteger(string hexData)
                : this(Hex.Decode(hexData))
            {
            }

            public BigInteger(int bitLength, string hexData)
                : base(ExpandToBitLength(bitLength, Hex.Decode(hexData)))
            {
            }
        }

        protected FixedSecureRandom(
            byte[] data)
        {
            _data = data;
        }

        public static FixedSecureRandom From(
            params byte[][] values)
        {
            MemoryStream bOut = new MemoryStream();

            for (int i = 0; i != values.Length; i++)
            {
                try
                {
                    byte[] v = values[i];
                    bOut.Write(v, 0, v.Length);
                }
                catch (IOException)
                {
                    throw new ArgumentException("can't save value array.");
                }
            }

            return new FixedSecureRandom(bOut.ToArray());
        }

        public FixedSecureRandom(
            Source[] sources)
        {
            MemoryStream bOut = new MemoryStream();

            if (isRegularStyle)
            {
                if (isClasspathStyle)
                {
                    for (int i = 0; i != sources.Length; i++)
                    {
                        try
                        {
                            if (sources[i] is BigInteger)
                            {
                                byte[] data = sources[i].data;
                                int len = data.Length - (data.Length % 4);
                                for (int w = data.Length - len - 1; w >= 0; w--)
                                {
                                    bOut.WriteByte(data[w]);
                                }
                                for (int w = data.Length - len; w < data.Length; w += 4)
                                {
                                    bOut.Write(data, w, 4);
                                }
                            }
                            else
                            {
                                bOut.Write(sources[i].data, 0, sources[i].data.Length);
                            }
                        }
                        catch (IOException)
                        {
                            throw new ArgumentException("can't save value source.");
                        }
                    }
                }
                else
                {
                    for (int i = 0; i != sources.Length; i++)
                    {
                        try
                        {
                            bOut.Write(sources[i].data, 0, sources[i].data.Length);
                        }
                        catch (IOException)
                        {
                            throw new ArgumentException("can't save value source.");
                        }
                    }
                }
            }
            else if (isAndroidStyle)
            {
                for (int i = 0; i != sources.Length; i++)
                {
                    try
                    {
                        if (sources[i] is BigInteger)
                        {
                            byte[] data = sources[i].data;
                            int len = data.Length - (data.Length % 4);
                            for (int w = 0; w < len; w += 4)
                            {
                                bOut.Write(data, data.Length - (w + 4), 4);
                            }
                            if (data.Length - len != 0)
                            {
                                for (int w = 0; w != 4 - (data.Length - len); w++)
                                {
                                    bOut.WriteByte(0);
                                }
                            }
                            for (int w = 0; w != data.Length - len; w++)
                            {
                                bOut.WriteByte(data[len + w]);
                            }
                        }
                        else
                        {
                            bOut.Write(sources[i].data, 0, sources[i].data.Length);
                        }
                    }
                    catch (IOException)
                    {
                        throw new ArgumentException("can't save value source.");
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("Unrecognized BigInteger implementation");
            }

            _data = bOut.ToArray();
        }

        public override byte[] GenerateSeed(int numBytes)
        {
            return SecureRandom.GetNextBytes(this, numBytes);
        }

        public override void NextBytes(
            byte[] buf)
        {
            Array.Copy(_data, _index, buf, 0, buf.Length);

            _index += buf.Length;
        }

        public override void NextBytes(
            byte[] buf,
            int off,
            int len)
        {
            Array.Copy(_data, _index, buf, off, len);

            _index += len;
        }

        public bool IsExhausted
        {
            get { return _index == _data.Length; }
        }

        private class RandomChecker
            : SecureRandom
        {
            byte[] data = Hex.Decode("01020304ffffffff0506070811111111");
            int index = 0;

            public override void NextBytes(byte[] bytes)
            {
                Array.Copy(data, index, bytes, 0, bytes.Length);

                index += bytes.Length;
            }
        }

        private static byte[] ExpandToBitLength(int bitLength, byte[] v)
        {
            if ((bitLength + 7) / 8 > v.Length)
            {
                byte[] tmp = new byte[(bitLength + 7) / 8];

                Array.Copy(v, 0, tmp, tmp.Length - v.Length, v.Length);
                if (isAndroidStyle)
                {
                    if (bitLength % 8 != 0)
                    {
                        uint i = BE_To_UInt32(tmp, 0);
                        UInt32_To_BE(i << (8 - (bitLength % 8)), tmp, 0);
                    }
                }

                return tmp;
            }
            else
            {
                if (isAndroidStyle && bitLength < (v.Length * 8))
                {
                    if (bitLength % 8 != 0)
                    {
                        uint i = BE_To_UInt32(v, 0);
                        UInt32_To_BE(i << (8 - (bitLength % 8)), v, 0);
                    }
                }
            }

            return v;
        }

        internal static uint BE_To_UInt32(byte[] bs, int off)
        {
            return (uint)bs[off] << 24
                | (uint)bs[off + 1] << 16
                | (uint)bs[off + 2] << 8
                | (uint)bs[off + 3];
        }

        internal static void UInt32_To_BE(uint n, byte[] bs, int off)
        {
            bs[off] = (byte)(n >> 24);
            bs[off + 1] = (byte)(n >> 16);
            bs[off + 2] = (byte)(n >> 8);
            bs[off + 3] = (byte)(n);
        }
    }

    #endregion

}
