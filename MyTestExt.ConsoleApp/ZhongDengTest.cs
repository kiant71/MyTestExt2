//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.IO.Compression;
//using System.Linq;
//using System.Text;
//using System.Xml;
//using System.Xml.Serialization;
//using java.io;
//using MyTestExt.ConsoleApp.Util;
//using MyTestExt.ConsoleApp.Util.smcrypto;
//using MyTestExt.ConsoleApp.Util.ZhongDeng.Model;
//using org.apache.pdfbox.pdmodel;
//using org.apache.pdfbox.util;
//using Org.BouncyCastle.Utilities;
//using Org.BouncyCastle.Utilities.Encoders;

//namespace MyTestExt.ConsoleApp
//{
//    public class ZhongDengTest
//    {
//        static string pubKey =
//            "";
//        static string userName = ""; //
//        static string userPassword = "";
//        static string platformAuthCode = "1d4-";

//        static string platformAuthCodeEncrypt = "";
//        static string loginTokenEncrypt = "";


//        public static void Do()
//        {
//            //Login();
//            //InitRegister();

//            QueryLogin();
//            QueryBySubject();
//            //QueryDownLoadFileByNum();

//            //QueryBySubjectRead();
//            //QueryDownLoadFileByNumRead();
//        }

//        public static void Login()
//        {
//            var userNameEncrypt = ZhongDengUtil.Encrypt(userName, pubKey);
//            var userPasswordEncrypt = ZhongDengUtil.Encrypt(userPassword, pubKey);
//            platformAuthCodeEncrypt = ZhongDengUtil.Encrypt(platformAuthCode, pubKey);


//            // ToByteArray(hex) 里面的数据都是0-ABCDEF，所以编码转义有效
//            var login = new cn.org.zhongdengwang.ws.LoginService.WSLoginServiceService();
//            var result = login.login(
//                Strings.ToByteArray(userNameEncrypt)
//                , Strings.ToByteArray(userPasswordEncrypt)
//                , Strings.ToByteArray(platformAuthCodeEncrypt));

//            var loginToken = Encoding.UTF8.GetString(result);
//            loginTokenEncrypt = ZhongDengUtil.Encrypt(loginToken, pubKey);
//        }

//        public static void InitRegister()
//        {
//            #region MyRegion

//            var attData = ReadAttach();

//            var post = new ReceivableReqApiModel
//            {
//                businesstype = "A00200",
//                timelimit = "2",
//                title = "12.23.001（< > & ' \"）" + DateTime.Now.ToString("yyyyMMdd HHmmss"),

//                #region debtors

//                debtors = new List<DebtorBaseReqApiModel>
//                {
//                    new DebtorFinInstReqApiModel
//                    {
//                        id = "1",
//                        debtortype = "01",
//                        debtorname = "深圳市出质公司供应商A",
//                        fininstcode = "jrjgbm",
//                        industryregistrationcode = "91442000MA4WTD3494",
//                        lei = "32523",
//                        corporationname = "代表人",
//                        address = new AddressReqApiModel
//                        {
//                            nationality = "CHN",
//                            province = "110000",
//                            city = "11XX00",
//                            detailaddress = "详细地址详细地址详细"
//                        }
//                    },
//                },

//                #endregion

//                #region debtees

//                debtees = new List<DebteeBaseReqApiModel>
//                {
//                    new DebteeFinInstReqApiModel
//                    {
//                        id = "1",
//                        debteetype = "01",
//                        debteename = "深圳市质权公司保理公司B",
//                        fininstcode = "jrjgbm22",
//                        industryregistrationcode = "91442000MA4WTD3494",
//                        lei = "32523",
//                        corporationname = "代表人",
//                        address = new AddressReqApiModel
//                        {
//                            nationality = "CHN",
//                            province = "110000",
//                            city = "11XX00",
//                            detailaddress = "详细地址详细地址详细"
//                        }
//                    },
//                },


//                #endregion pledgeinfo

//                #region pledgeinfo
//                pledgeinfo = new PledgeReceivableReqApiModel
//                {
//                    maincontractnumber = null,
//                    maincontractcurrency = null,
//                    maincontractsum = null,
//                    collateraldescribe = "财产描述信息（需要换行处，加入\r\n），【非空】",
//                    //attachments = new List<PledgeAttachmentReqApiModel>
//                    //{
//                    //    new PledgeAttachmentReqApiModel
//                    //    {
//                    //        attachmentname = "测试JPG2.jpg"
//                    //    },
//                    //    new PledgeAttachmentReqApiModel
//                    //    {
//                    //        attachmentname = "测试Pdf1.pdf"
//                    //    }
//                    //},

//                    pledgecontractcurrency = null,
//                    pledgecontractsum = null
//                }, 
//                #endregion
//            };

//            #endregion

//            //var xmlStr = XmlExtends.ToXmlUtf8(post);
//            //var init = new cn.org.zhongdengwang.ws.InitRegisterService.WSInitRegisterServiceService();
//            //var result = init.initRegister(
//            //    Strings.ToByteArray("AT")
//            //    , Strings.ToByteArray(platformAuthCodeEncrypt)
//            //    , Strings.ToByteArray(loginTokenEncrypt)
//            //    , Strings.ToByteArray(string.Format("{0}.xml", DateTime.Now.ToString("yyyyMMdd-HHmmss-fff")))
//            //    , Strings.ToByteArray(ZhongDengUtil.Encrypt(xmlStr, pubKey))
//            //    , attData);

//            //var resultStr = Encoding.UTF8.GetString(result);
//            //var xmlRes = XmlExtends.FromXml<FeedbackRspApiModel>(resultStr);
//        }


//        public static byte[] ReadAttach()
//        {
//            using (var fs = new FileStream(@"D:/1.Desktop/attach1.zip", FileMode.Open, FileAccess.Read))
//            {
//                byte[] pubBytes = new byte[fs.Length];
//                fs.Read(pubBytes, 0, pubBytes.Length);
//                fs.Close();

//                return pubBytes; 
//            }
//        }

//        public static byte[] ReadAttach(string file)
//        {
//            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
//            {
//                byte[] pubBytes = new byte[fs.Length];
//                fs.Read(pubBytes, 0, pubBytes.Length);
                
//                return pubBytes;
//            }
//        }

//        //public static byte[] ToByteArray(
//        //    string s)
//        //{
//        //    byte[] bs = new byte[s.Length];
//        //    for (int i = 0; i < bs.Length; ++i)
//        //    {
//        //        bs[i] = Convert.ToByte(s[i]);
//        //    }
//        //    return bs;
//        //}





//        public static void QueryLogin()
//        {
//            var userNameEncrypt = ZhongDengUtil.Encrypt(userName, pubKey);
//            var userPasswordEncrypt = ZhongDengUtil.Encrypt(userPassword, pubKey);
//            platformAuthCodeEncrypt = ZhongDengUtil.Encrypt(platformAuthCode, pubKey);


//            // ToByteArray(hex) 里面的数据都是0-ABCDEF，所以编码转义有效
//            var login = new cn.org.zhongdengwang.wsquery.Login.WSLoginServiceService();
//            var result = login.login(
//                Strings.ToByteArray(userNameEncrypt)
//                , Strings.ToByteArray(userPasswordEncrypt)
//                , Strings.ToByteArray(platformAuthCodeEncrypt));

//            var loginToken = Encoding.UTF8.GetString(result);
//            loginTokenEncrypt = ZhongDengUtil.Encrypt(loginToken, pubKey);
//        }


//        public static void QueryBySubject()
//        {
//            var init = new cn.org.zhongdengwang.wsquery.QueryBySubject.WSQueryBySubjectServiceService();
//            var result = init.queryBySubject(
//                Strings.ToByteArray(platformAuthCodeEncrypt)
//                , Strings.ToByteArray(loginTokenEncrypt)
//                , Strings.ToByteArray(ZhongDengUtil.Encrypt("1000", pubKey))
//                , Strings.ToByteArray(ZhongDengUtil.Encrypt("深圳市出质公司供应商A", pubKey))
//                , Strings.ToByteArray(ZhongDengUtil.Encrypt("false", pubKey)));

//            var fileName = string.Format("D:/1.Desktop/result/{0}.zip", DateTime.Now.ToString("yyyyMMdd-HHmmss-fff"));
//            using (var fs = new FileStream(fileName, FileMode.CreateNew))
//            {
//                fs.Write(result, 0, result.Length);
//            }
//        }

//        public static void QueryDownLoadFileByNum()
//        {
//            var init = new cn.org.zhongdengwang.wsquery.QueryDownLoadFileByNum.WSDownLoadFileByNumServiceService();
//            var result = init.queryDownLoadFileByNum(
//                Strings.ToByteArray(platformAuthCodeEncrypt)
//                , Strings.ToByteArray(loginTokenEncrypt)
//                , Strings.ToByteArray(ZhongDengUtil.Encrypt("00031470000003746685", pubKey))
//                , Strings.ToByteArray(ZhongDengUtil.Encrypt("1000", pubKey))
//                , Strings.ToByteArray(ZhongDengUtil.Encrypt("深圳市出质公司供应商A", pubKey))
//                );

//            var fileName = string.Format("D:/1.Desktop/result/{0}.zip", DateTime.Now.ToString("yyyyMMdd-HHmmss-fff"));
//            using (var fs = new FileStream(fileName, FileMode.CreateNew))
//            {
//                fs.Write(result, 0, result.Length);
//            }
//        }

       


//        public static void QueryBySubjectRead()
//        {
//            var data = ReadAttach(@"D:/1.Desktop/result/QueryBySubject_OK.zip");

//            using (var stream = new MemoryStream(data))
//            {
//                using (var zipArchive = new ZipArchive(stream))
//                {
//                    var entry = zipArchive.Entries.FirstOrDefault(c => 
//                        c.Name.EndsWith(".xml", StringComparison.OrdinalIgnoreCase));
//                    if (entry != null)
//                    {
//                        using (var entryStream = entry.Open())
//                        {
//                            using (var reader = new BinaryReader(entryStream))
//                            {
//                                var bytes = reader.ReadBytes((int)entry.Length);
//                                var str = Encoding.UTF8.GetString(bytes);
//                                //var obj = XmlExtends.FromXml<QueryBySubjectRspApiModel>(str); 
//                            }
//                        }


//                        #region remark. DeflateStream 不能通过 fs.Length获取
//                        //var fs = entry.Open();
//                        //var bytes = new byte[fs.Length];
//                        //fs.Seek(0, SeekOrigin.Begin);
//                        //stream.Read(bytes, 0, bytes.Length);
//                        //var str = Encoding.UTF8.GetString(bytes);
//                        //var obj = XmlExtends.FromXml<QueryBySubjectRspApiModel>(str); 
//                        #endregion

//                        #region 可以通过 XmlDocument.Load读取
//                        //var document = new XmlDocument();
//                        //document.Load(entry.Open());
//                        //var obj = XmlExtends.FromXml<QueryBySubjectRspApiModel>(document.ToXml()); 
//                        #endregion
//                    }
//                }
//            }
            

//        }

//        public static void QueryDownLoadFileByNumRead()
//        {
//            //var fileName = @"D:/1.Desktop/result/QueryDownLoadFileByNum_OK.zip";
//            //var fileName = @"D:/1.Desktop/result/QueryDownLoadFileByNum_OK.zip";

//            var pdfName = "00031470000003746685.pdf";

//            var data = ReadAttach(@"D:/1.Desktop/result/QueryDownLoadFileByNum_OK.zip");

//            using (var stream = new MemoryStream(data))
//            {
//                using (var zipArchive = new ZipArchive(stream, ZipArchiveMode.Read, true, Encoding.GetEncoding("GBK")))
//                {
//                    // 如果存在 xml文件，则是发生错误
//                    var entryXml = zipArchive.Entries.FirstOrDefault(c =>
//                        c.Name.EndsWith(".xml", StringComparison.OrdinalIgnoreCase));
//                    if (entryXml != null)
//                    {
//                        var bytes = ReadBytes(entryXml);
//                        var str = Encoding.UTF8.GetString(bytes);
//                        //var obj = XmlExtends.FromXml<QueryBySubjectRspApiModel>(str);
//                    }

//                    // 如果不存在登记证明文件，则返回错误， ex.00031470000003746685.pdf
//                    var entryReg = zipArchive.Entries.FirstOrDefault(c => 
//                        c.Name.ToLower() == pdfName.ToLower());
//                    if (entryReg == null)
//                    {
                        
//                    }
                    
//                    // 解析登记证明文件
//                    var regData = ReadBytes(entryReg);
//                    using (var inputStream = new ByteArrayInputStream(regData))
//                    {
//                        using (var doc = PDDocument.load(inputStream))
//                        {
//                            var pdfStripper = new PDFTextStripper();
//                            var text = pdfStripper.getText(doc);
//                        }
//                    }

//                    // 解析登记附件列表
//                    foreach (var entryAttach in zipArchive.Entries.Where(c =>
//                                                    c.Name.ToLower() != pdfName.ToLower()))
//                    {
//                        var bytes = ReadBytes(entryAttach);
//                        using (var fs = new MemoryStream(bytes))
//                        {
//                            var upload = new UploadPrivateHelper("http://baoli.sap360.com.cn:36002/DocManage", "4a6d58262d1141efa8dc119e936000b5");
//                            var model = new UploadPrivateModel
//                            {
//                                FileName = entryAttach.Name,
//                                Size = entryAttach.Length,
//                            };
//                            model.FullUrlName = upload.Upload(model, fs);
//                        }
//                    }
//                } // end.of. using (var zipArchive = new ZipArchive(stream))
//            } // end.of. using  using (var stream = new MemoryStream(data))



//        }

//        private static byte[] ReadBytes(ZipArchiveEntry entry)
//        {
//            byte[] bytes;
//            using (var entryStream = entry.Open())
//            {
//                using (var reader = new BinaryReader(entryStream))
//                {
//                    bytes = reader.ReadBytes((int) entry.Length);
//                }
//            }

//            return bytes;
//        }
//    }


//    #region ZhongDengUtil

//    public class ZhongDengUtil
//    {
//        public static string Encrypt(string input, string pubHex)
//        {
//            var encryptStrData = Encoding.UTF8.GetBytes(input);
//            var pubData = Base64.Decode(Base64.Encode(HexToByte(pubHex)));

//            var encryptResData = SM2Utils.EncryptZhongDeng(pubData, encryptStrData);

//            var res = ByteToHex(encryptResData);
//            return res;
//        }

//        public static string ByteToHex(byte[] b)
//        {
//            var hs = "";
//            var stmp = "";
//            for (var n = 0; n < b.Length; n++)
//            {
//                //stmp = Integer.toHexString(b[n] & 0xFF);
//                stmp = (b[n] & 0xFF).ToString("x");
//                if (stmp.Length == 1)
//                {
//                    hs = hs + "0" + stmp;
//                }
//                else
//                {
//                    hs = hs + stmp;
//                }
//            }

//            return hs.ToUpper();
//        }

//        public static byte[] HexToByte(string hex)
//        {
//            var arr = hex.ToCharArray();
//            var b = new byte[hex.Length / 2];
//            //for (int i = 0, j = 0, l = hex.Length; i < l; i++, j++)
//            //{
//            //    string swap = arr[i++] + arr[i];
//            //    int byteint = Integer.parseInt(swap, 16) & 0xFF;
//            //    b[j] = (new Integer(byteint)).byteValue();
//            //}
//            for (var i = 0; i < b.Length; i++)
//            {
//                var pos = i * 2;
//                b[i] = (byte)(CharToByte(arr[pos]) << 4 | CharToByte(arr[pos + 1]));
//            }

//            return b;
//        }

//        private static byte CharToByte(char c)
//        {
//            return (byte)"0123456789ABCDEF".IndexOf(c);
//        }
//    }

//    #endregion




//}
