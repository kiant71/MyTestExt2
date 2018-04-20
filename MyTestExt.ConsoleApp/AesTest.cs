using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using ConsoleApplication1.Util;

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


    public class SecurityKey
    {
        private const string DBKey = "12,./#$%*&^)$d@k|pz';86,#(!qabv.mo;b,euod";

        /// <summary>
        /// 解密数据库密码
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string DecryptDbKey(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return password;
            AESEncryptor aes = new AESEncryptor(DBKey, AESBits.BITS256);
            return aes.Decrypt(password);
        }

        /// <summary>
        /// 加密数据库密码
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string EncryptionDbKey(string password)
        {
            AESEncryptor aes = new AESEncryptor(DBKey, AESBits.BITS256);
            return aes.Encrypt(password);
        }

    }




    public enum AESBits
    {
        BITS128,
        BITS192,
        BITS256
    };

    /// <summary>
    /// AES加密类
    /// </summary>
    public class AESEncryptor
    {
        private string fPassword;
        private AESBits fEncryptionBits;
        private byte[] fSalt = new byte[] { 0x00, 0x01, 0x02, 0x1C, 0x1D, 0x1E, 0x03, 0x04, 0x05, 0x0F, 0x20, 0x21, 0xAD, 0xAF, 0xA4 };

        /// <summary>
        /// Initialize new AESEncryptor.
        /// </summary>
        /// <param name="password">The password to use for encryption/decryption.</param>
        /// <param name="encryptionBits">Encryption bits (128,192,256).</param>
        public AESEncryptor(string password, AESBits encryptionBits)
        {
            fPassword = password;
            fEncryptionBits = encryptionBits;
        }

        /// <summary>
        /// Initialize new AESEncryptor.
        /// </summary>
        /// <param name="password">The password to use for encryption/decryption.</param>
        /// <param name="encryptionBits">Encryption bits (128,192,256).</param>
        /// <param name="salt">Salt bytes. Bytes length must be 15.</param>
        public AESEncryptor(string password, AESBits encryptionBits, byte[] salt)
        {
            fPassword = password;
            fEncryptionBits = encryptionBits;
            fSalt = salt;
        }

        private byte[] iEncrypt(byte[] data, byte[] key, byte[] iV)
        {
            MemoryStream ms = new MemoryStream();

            Rijndael alg = Rijndael.Create();
            alg.Key = key;
            alg.IV = iV;
            CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);
            alg.Dispose();
            cs.Write(data, 0, data.Length);
            cs.Close();
            byte[] encryptedData = ms.ToArray();
            return encryptedData;
        }

        /// <summary>
        /// Encrypt string with AES algorith.
        /// </summary>
        /// <param name="data">String to encrypt.</param>
        public string Encrypt(string data)
        {
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(data);

            PasswordDeriveBytes pdb = new PasswordDeriveBytes(fPassword, fSalt);

            switch (fEncryptionBits)
            {
                case AESBits.BITS128:
                    return Convert.ToBase64String(iEncrypt(clearBytes, pdb.GetBytes(16), pdb.GetBytes(16)));
                case AESBits.BITS192:
                    return Convert.ToBase64String(iEncrypt(clearBytes, pdb.GetBytes(24), pdb.GetBytes(16)));
                case AESBits.BITS256:
                    return Convert.ToBase64String(iEncrypt(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16)));
            }
            return null;
        }

        /// <summary>
        /// Encrypt byte array with AES algorithm.
        /// </summary>
        /// <param name="data">Bytes to encrypt.</param>
        public byte[] Encrypt(byte[] data)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(fPassword, fSalt);
            switch (fEncryptionBits)
            {
                case AESBits.BITS128:
                    return iEncrypt(data, pdb.GetBytes(16), pdb.GetBytes(16));
                case AESBits.BITS192:
                    return iEncrypt(data, pdb.GetBytes(24), pdb.GetBytes(16));
                case AESBits.BITS256:
                    return iEncrypt(data, pdb.GetBytes(32), pdb.GetBytes(16));
            }
            return null;
        }

        private byte[] iDecrypt(byte[] data, byte[] key, byte[] iv)
        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();
            alg.Key = key;
            alg.IV = iv;
            CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(data, 0, data.Length);
            cs.Close();
            alg.Dispose();
            byte[] decryptedData = ms.ToArray();
            return decryptedData;
        }


        /// <summary>
        /// Decrypt string with AES algorithm.
        /// </summary>
        /// <param name="data">Encrypted string.</param>
        public string Decrypt(string data)
        {
            byte[] dataToDecrypt = Convert.FromBase64String(data);

            PasswordDeriveBytes pdb = new PasswordDeriveBytes(fPassword, fSalt);

            switch (fEncryptionBits)
            {
                case AESBits.BITS128:
                    return System.Text.Encoding.Unicode.GetString(iDecrypt(dataToDecrypt, pdb.GetBytes(16), pdb.GetBytes(16)));
                case AESBits.BITS192:
                    return System.Text.Encoding.Unicode.GetString(iDecrypt(dataToDecrypt, pdb.GetBytes(24), pdb.GetBytes(16)));
                case AESBits.BITS256:
                    return System.Text.Encoding.Unicode.GetString(iDecrypt(dataToDecrypt, pdb.GetBytes(32), pdb.GetBytes(16)));
            }
            return null;
        }

        /// <summary>
        /// Decrypt byte array with AES algorithm.
        /// </summary>
        /// <param name="data">Encrypted byte array.</param>
        public byte[] Decrypt(byte[] data)
        {
            using (PasswordDeriveBytes pdb = new PasswordDeriveBytes(fPassword, fSalt))
            {
                switch (fEncryptionBits)
                {
                    case AESBits.BITS128:
                        return iDecrypt(data, pdb.GetBytes(16), pdb.GetBytes(16));
                    case AESBits.BITS192:
                        return iDecrypt(data, pdb.GetBytes(24), pdb.GetBytes(16));
                    case AESBits.BITS256:
                        return iDecrypt(data, pdb.GetBytes(32), pdb.GetBytes(16));
                }
                return null;
            }
        }

        /// <summary>
        /// Encryption/Decryption password.
        /// </summary>
        public string Password
        {
            get { return fPassword; }
            set { fPassword = value; }
        }

        /// <summary>
        /// Encryption/Decryption bits.
        /// </summary>
        public AESBits EncryptionBits
        {
            get { return fEncryptionBits; }
            set { fEncryptionBits = value; }
        }

        /// <summary>
        /// Salt bytes (bytes length must be 15).
        /// </summary>
        public byte[] Salt
        {
            get { return fSalt; }
            set { fSalt = value; }
        }




        // 密匙
        private const string DefaultKey = "P)(%&#*~<>diuej8ApU!Wm,#@:3TuoiQ";  //KeySize = 256

        // 向量（CBC 模式用）
        private const string DefaultIV = "0001020304050607"; //16个字节，默认BlockSize=128


        public static string PlatformEncrypt(object entityObj, string strKey = DefaultKey, string strIV = DefaultIV)
        {
            if (entityObj == null) return string.Empty;
            string s = JsonNet.Serialize(entityObj);
            return PlatformEncrypt(s, strKey, strIV);
        }

        public static T PlatformDecrypt<T>(string strDecrypt, string strKey = DefaultKey, string strIV = DefaultIV)
        {
            if (string.IsNullOrWhiteSpace(strDecrypt)) return default(T);
            string s = PlatformDecrypt(strDecrypt, strKey, strIV);
            return JsonNet.Deserialize<T>(s);
        }


        /// <summary>
        /// 跨平台加密（CBC模式）
        /// </summary>
        /// <param name="strEncrypt">待加密内容</param>
        /// <param name="strKey">密匙</param>
        /// <param name="strIV">向量</param>
        /// <returns></returns>
        public static string PlatformEncrypt(string strEncrypt, string strKey = DefaultKey, string strIV = DefaultIV)
        {
            if (string.IsNullOrWhiteSpace(strEncrypt))
                throw new ArgumentOutOfRangeException("无效的字符");
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
        /// 跨平台解密
        /// </summary>
        /// <param name="strDecrypt">待解密的内容，强制Base64String 字符串</param>
        /// <param name="strKey">密匙</param>
        /// <param name="strIV">向量（CBC 模式用）</param>
        /// <returns></returns>
        public static string PlatformDecrypt(string strDecrypt, string strKey = DefaultKey, string strIV = DefaultIV)
        {
            if (string.IsNullOrWhiteSpace(strDecrypt)) return "";
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

    }

}
