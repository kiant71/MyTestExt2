using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;


namespace MyTestExt.ConsoleApp
{
    [Obsolete("建议作废？看新的？")]
    public class RSATest 
    {
        public static void Do()
        {
            var key = KeyPair.CreateNew(512);
            Console.WriteLine(key.Public);
            Console.WriteLine(key.Private);
            var raw = Encoding.ASCII.GetBytes("Hello Wu");
            Console.WriteLine(raw);

            // 公钥 - 加密
            var r = KeyPair.Encrypt(raw, key);
            Console.WriteLine(r);

            // 生成签名
            var signature = KeyPair.Sign(key, r);
            Console.WriteLine(signature);


            var success = KeyPair.VerifyData(key, r, signature);
            Console.WriteLine(success);



            // 私钥 - 解密
            var u = Encoding.ASCII.GetString(KeyPair.Decrypt(r, key));
            Console.WriteLine(u);

            Console.ReadKey();
        }

    }



    public class KeyPair
    {
        public string Public { get; set; }
        public string Private { get; set; }

        public KeyPair(string pu, string pr)
        {
            Public = pu;
            Private = pr;
        }

        public static byte[] Encrypt(byte[] data, KeyPair kp)
        {
            byte[] raw;

            using (var rsa = new RSACryptoServiceProvider())
            {
                //导入表示 RSA 密钥信息的 blob
                rsa.ImportCspBlob(Decompress(Base32.FromBase32String(kp.Public)));
                //使用 RSA 算法加密数据
                //true 若要直接执行 RSA 使用 OAEP 填充 （仅可在运行 Windows XP 的计算机上或更高版本） 的加密; 否则为 false 使用 PKCS #1 v1.5 填充。
                raw = rsa.Encrypt(data, true);
            }

            return raw;
        }

        public static byte[] Decrypt(byte[] data, KeyPair kp)
        {
            byte[] raw;

            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportCspBlob(Decompress(Base32.FromBase32String(kp.Private)));

                raw = rsa.Decrypt(data, true);
            }

            return raw;
        }

        public static byte[] Sign(KeyPair kp, byte[] data)
        {
            byte[] signature;
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportCspBlob(Decompress(Base32.FromBase32String(kp.Private)));
                //使用指定的哈希算法计算指定字节数组的哈希值，并对生成的哈希值进行签名。
                signature = rsa.SignData(data, new SHA1CryptoServiceProvider());
            }

            return signature;
        }

        public static bool VerifyData(KeyPair kp, byte[] data, byte[] signature)
        {
            bool b;
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportCspBlob(Decompress(Base32.FromBase32String(kp.Public)));
                //通过使用提供的公钥确定签名中的哈希值并将其与所提供数据的哈希值进行比较验证数字签名是否有效。
                b = rsa.VerifyData(data, new SHA1CryptoServiceProvider(), signature);
            }
            return b;
        }

        public static KeyPair CreateNew(int length)
        {
            KeyPair ret;
            //密钥的大小
            using (var rsa = new RSACryptoServiceProvider(length))
            {
                try
                {
                    //参数true 生成私钥,false生成公钥
                    var pub = rsa.ExportCspBlob(false);
                    var priv = rsa.ExportCspBlob(true);

                    ret = new KeyPair(Base32.ToBase32String(Compress(pub)), Base32.ToBase32String(Compress(priv)));
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }

            return ret;
        }

        public byte[] ToArray()
        {
            return Decompress(Base32.FromBase32String(Private));
        }

        public static KeyPair Import(byte[] raw, int length)
        {
            KeyPair ret;
            using (var rsa = new RSACryptoServiceProvider(length))
            {
                rsa.ImportCspBlob(raw);

                try
                {
                    var pub = rsa.ExportCspBlob(false);
                    var priv = rsa.ExportCspBlob(true);

                    ret = new KeyPair(Base32.ToBase32String(Compress(pub)), Base32.ToBase32String(Compress(priv)));
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }

            return ret;
        }

        private static byte[] Compress(byte[] raw)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(memory, CompressionMode.Compress, true))
                {
                    gzip.Write(raw, 0, raw.Length);
                }
                return memory.ToArray();
            }
        }
        private static byte[] Decompress(byte[] gzip)
        {
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip),
                                  CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }
    }



    #region Base32

    internal sealed class Base32
    {
        //Base32和Base64相比只有一个区别就是，用32个字符表示256个ASC字符，也就是说5个ASC字符一组可以生成8个Base字符，反之亦然。
        /// <summary>
        /// 常规字节的大小（以位为单位）
        /// </summary>
        private const int InByteSize = 8;

        /// <summary>
        /// 转换字节的大小（位）
        /// </summary>
        private const int OutByteSize = 5;

        /// <summary>
        ///字母表
        /// </summary>
        private const string Base32Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        /// <summary>
        ///将字节数组转换为Base32格式
        /// </summary>
        /// <param name="bytes">要转换为Base32格式的字节数组</param>
        /// <returns>返回表示字节数组的字符串</returns>
        internal static string ToBase32String(byte[] bytes)
        {
            // 检查字节数组是否为NULL
            if (bytes == null)
            {
                return null;
            }
            // 检查是否为空
            else if (bytes.Length == 0)
            {
                return string.Empty;
            }

            //准备容器为了最终大小值
            StringBuilder builder = new StringBuilder(bytes.Length * InByteSize / OutByteSize);

            //在输入缓冲器中的位置
            int bytesPosition = 0;

            // <bytePosition>指向的单个字节内的偏移量（从左到右）
            //0 - 最高位，7 - 最低位
            int bytesSubPosition = 0;

            // 字节在字典中查找
            byte outputBase32Byte = 0;

            //当前输出字节中填充的位数
            int outputBase32BytePosition = 0;

            //迭代通过输入缓冲区，直到我们到达它的结束
            while (bytesPosition < bytes.Length)
            {
                // 计算可以从当前输入字节中提取的位数，以填充输出字节中的缺少位
                int bitsAvailableInByte = Math.Min(InByteSize - bytesSubPosition, OutByteSize - outputBase32BytePosition);

                // 在输出字节中留出空格
                outputBase32Byte <<= bitsAvailableInByte;

                // 提取输入字节的一部分并将其移动到输出字节
                outputBase32Byte |= (byte)(bytes[bytesPosition] >> (InByteSize - (bytesSubPosition + bitsAvailableInByte)));

                // 更新当前子字节位置
                bytesSubPosition += bitsAvailableInByte;

                // 检查溢出
                if (bytesSubPosition >= InByteSize)
                {
                    // 移动到下一个字节
                    bytesPosition++;
                    bytesSubPosition = 0;
                }

                // 更新当前base32字节完成
                outputBase32BytePosition += bitsAvailableInByte;

                // 检查输入数组的溢出或结束
                if (outputBase32BytePosition >= OutByteSize)
                {
                    //丢弃溢出位
                    outputBase32Byte &= 0x1F;  // 0x1F = 00011111 in binary

                    // 添加当前Base32字节并将其转换为字符
                    builder.Append(Base32Alphabet[outputBase32Byte]);

                    //移动到下一个字节
                    outputBase32BytePosition = 0;
                }
            }

            // 检查我们是否有剩余
            if (outputBase32BytePosition > 0)
            {
                //移动到右边的位
                outputBase32Byte <<= (OutByteSize - outputBase32BytePosition);

                //丢弃溢出位
                outputBase32Byte &= 0x1F;  // 0x1F = 00011111 in binary

                //添加当前Base32字节并将其转换为字符
                builder.Append(Base32Alphabet[outputBase32Byte]);
            }

            return builder.ToString();
        }

        /// <summary>
        ///将base32字符串转换为字节数组
        /// </summary>
        /// <param name="base32String">Base32 string to convert</param>
        /// <returns>Returns a byte array converted from the string</returns>
        internal static byte[] FromBase32String(string base32String)
        {

            if (base32String == null)
            {
                return null;
            }
            else if (base32String == string.Empty)
            {
                return new byte[0];
            }
            // 转换为大写
            string base32StringUpperCase = base32String.ToUpperInvariant();

            //准备输出字节数组
            byte[] outputBytes = new byte[base32StringUpperCase.Length * OutByteSize / InByteSize];

            if (outputBytes.Length == 0)
            {
                throw new ArgumentException("Specified string is not valid Base32 format because it doesn't have enough data to construct a complete byte array");
            }

            // 在字符串中的位置
            int base32Position = 0;

            // 字符串中的字符偏移
            int base32SubPosition = 0;

            // 输出字节数组中的位置
            int outputBytePosition = 0;

            //当前输出字节中填充的位数
            int outputByteSubPosition = 0;

            //通常我们将对输入数组进行迭代，但在这种情况下，我们实际上是对输出数组进行迭代
            //我们这样做，因为输出数组没有溢出位，而输入却会导致输出数组溢出，如果我们不能及时停止
            while (outputBytePosition < outputBytes.Length)
            {
                //在字典中查找当前字符将int转换为字节
                int currentBase32Byte = Base32Alphabet.IndexOf(base32StringUpperCase[base32Position]);

                if (currentBase32Byte < 0)
                {
                    throw new ArgumentException(string.Format("Specified string is not valid Base32 format because character \"{0}\" does not exist in Base32 alphabet", base32String[base32Position]));
                }

                //计算从当前输入字符中提取的位数，以填充输出字节中的缺失位
                int bitsAvailableInByte = Math.Min(OutByteSize - base32SubPosition, InByteSize - outputByteSubPosition);

                // 在输出字节中留出空格
                outputBytes[outputBytePosition] <<= bitsAvailableInByte;

                //提取输入字符的一部分并将其移动到输出字节
                outputBytes[outputBytePosition] |= (byte)(currentBase32Byte >> (OutByteSize - (base32SubPosition + bitsAvailableInByte)));

                //更新当前子字节位置
                outputByteSubPosition += bitsAvailableInByte;

                // 检查溢出
                if (outputByteSubPosition >= InByteSize)
                {
                    outputBytePosition++;
                    outputByteSubPosition = 0;
                }

                //更新当前base32字节完成
                base32SubPosition += bitsAvailableInByte;

                // 检查溢出或输入数组的结尾
                if (base32SubPosition >= OutByteSize)
                {
                    // 移动到下一个字符
                    base32Position++;
                    base32SubPosition = 0;
                }
            }

            return outputBytes;
        }
    }

    #endregion


}