using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using java.io;
using MyTestExt.ConsoleApp.Util;
using MyTestExt.Utils;
using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;

namespace MyTestExt.ConsoleApp
{
    public class ZipArchiveTest
    {
        string zipedFile = @"D:\0.Work\TestZip\TestFolder.zip";
        string baseFolder = @"D:\0.Work\TestZip\TestFolder";

        public void Do()
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            //Add();
            //DownAndCreate("测试压缩123");


            //G2Test_ZipFolder();

            //ZipActive_Create();
            ZipActive_Entries();

            //await CreateInMemory();

            //ZipFilesView();

            sw.Stop();

        }



        public void G2Test_ZipFolder()
        {
            var folder = @"D:\0.Work\TestZip\TestFolder\2GTest";
            var file = @"D:\0.Work\TestZip\2GTest_1.zip";

            var sw = new Stopwatch();
            sw.Start();

            if (System.IO.File.Exists(file))
                System.IO.File.Delete(file);

            // 2g 35(s) - NoCompression
            System.IO.Compression.ZipFile.CreateFromDirectory(folder, file, CompressionLevel.NoCompression, false);

            sw.Stop();
        }
        

        public void ZipActive_Create()
        {
            var folder = @"D:\0.Work\TestZip\TestFolder\2GTest";
            var file = @"D:\0.Work\TestZip\2GTest_2.zip";

            var sw = new Stopwatch();
            sw.Start();

            if (System.IO.File.Exists(file))
                System.IO.File.Delete(file);

            var dict2 = new Dictionary<string, string>();
            var allDirs = Directory.GetDirectories(folder, "*", SearchOption.AllDirectories);
            allDirs.ToList().ForEach(f =>
            {
                dict2[f.Replace(folder + Path.DirectorySeparatorChar, "")] = "";
            });
            var allFiles = Directory.GetFiles(folder, "*", SearchOption.AllDirectories);
            allFiles.ToList().ForEach(f =>
            {
                dict2[f.Replace(folder + Path.DirectorySeparatorChar, "")] = f;
            });
            

            // 2g 16(s) - NoCompression
            using (var archive = ZipFile.Open(file, ZipArchiveMode.Create))
            {
                foreach (var key in dict2.Keys)
                {
                    if (string.IsNullOrWhiteSpace(dict2[key]))
                        archive.CreateEntry(key + Path.DirectorySeparatorChar);  // 创建目录结构 +"/"
                    //else
                    //    archive.CreateEntryFromFile(dict2[key], key, CompressionLevel.NoCompression);

                }
            }


            //var dict = new Dictionary<string, List<string>>();

            //dict[""] = Directory.GetFiles(folder).ToList();

            //var subTrees = Directory.GetDirectories(folder, "*", SearchOption.AllDirectories);
            //subTrees.ToList().ForEach(dir =>
            //{
            //    var key = dir.Replace(folder + Path.DirectorySeparatorChar, "");
            //    dict[key] = Directory.GetFiles(dir).ToList();
            //});

            sw.Stop();
        }

        public void ZipActive_Entries()
        {
            var file = @"D:\0.Work\FtpTest-compress\D_sap360File8fc15228892c4c70b428022708118f94202051228b1397a8ad048198aca44c88039481e.zip";

            var fs = new FileStream(file, FileMode.Open, FileAccess.Read);
            var data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            using (var stream = new MemoryStream(data))
            {
                using (var archive = new ZipArchive(stream, ZipArchiveMode.Read, true, Encoding.GetEncoding("GBK")))
                {
                    // https://stackoverflow.com/questions/42262013/how-can-i-get-the-valid-entries-of-a-ziparchive-when-one-them-contains-illegal-c
                    
                    // 如果存在 xml文件，则是发生错误
                    var entryErrorXml = archive.Entries.FirstOrDefault(c =>
                        c.Name.EndsWith(".xml", StringComparison.OrdinalIgnoreCase));
                    if (entryErrorXml != null)
                    {
                        // error.
                    }

                    // 如果不存在登记证明文件，则返回错误， ex.00031470000003746685.pdf 登记编号序列
                    var pdfName = "test" + ".pdf";
                    var entryReg = archive.Entries.FirstOrDefault(c => c.Name.ToLower() == pdfName.ToLower());
                    if (entryReg == null)
                    {
                        // error.
                    } 

                    // 解析、上传(系统的)登记证明文件.pdf
                    var regData = ReadBytes(entryReg);
                    #region MyRegion
                    using (var inputStream = new ByteArrayInputStream(regData))
                    {
                        using (var doc = PDDocument.load(inputStream))
                        {
                            var pdfStripper = new PDFTextStripper();
                            //ret.RegisterText = pdfStripper.getText(doc);
                        }

                        //ret.RegisterFile = Upload(entryReg);
                    } 
                    #endregion

                    // 上传登记资料的附件列表
                    var atts = new List<ZipArchiveEntry>();
                    foreach (var entryAttach in archive.Entries.Where(c =>
                                                    c.Name.ToLower() != pdfName.ToLower()))
                    {
                        atts.Add(entryAttach);
                    }

                } // end.of. using (var zipArchive = new ZipArchive(stream))
            } // end.of. using  using (var stream = new MemoryStream(data))
        }
        

        public void ZipFolder()
        {
            System.IO.Compression.ZipFile.CreateFromDirectory(baseFolder, zipedFile);
        }
        

        public void Add()
        {
            var addFile = @"D:\0.Work\TestZip\TestFolder\Change\A1\新建文本文档.txt";

            using (var archive = ZipFile.Open(zipedFile, ZipArchiveMode.Update))
            {
                var zipRelatName = addFile.Replace(baseFolder, "");
                while (zipRelatName.Substring(0, 1) == @"\")
                    zipRelatName = zipRelatName.Substring(1);

                var entry = archive.GetEntry(zipRelatName);
                entry?.Delete();

                //archive.CreateEntryFromFile(addFile, zipRelatName);
            }

            //using (var fs = new FileStream(zipedFile, FileMode.Open))
            //{
            //    using (var zip = new ZipArchive(fs, ZipArchiveMode.Update))
            //    {
            //        var zipRelatName = addFile.Replace(baseFolder, "");
            //        if (zipRelatName.Substring(0, 1) == "\\")
            //            zipRelatName = zipRelatName.Substring(1);

            //        var entry = zip.GetEntry(zipRelatName);
            //        entry?.Delete();

            //        //zip.CreateEntryFromFile(addFile, zipRelatName);
            //    }
            //}
        }


        public void DownAndCreate(string zipName, int companyID=1)
        {
            var tmpEx = Path.GetExtension(
                "http://in.sap360.com.cn:559/group1/M00/00/82/wKgB0VztBHmEbaEOAAAAAAHd2qY7660.cs");


            var files = new Dictionary<string, string>();
            files[@"_7zip.cs"] = "http://in.sap360.com.cn:559/group1/M00/00/82/wKgB0VztBHmEbaEOAAAAAAHd2qY7660.cs";
            files[@"piao.jpg"] = "http://in.sap360.com.cn:559/group1/M00/00/83/wKgBuFztBO-EPc_dAAAAAG8yk98307.jpg";
            files[@"piao3.jpg"] = "http://in.sap360.com.cn:559/group1/M00/00/82/wKgB0VztBSmENMRvAAAAAPrulhM298.jpg";
            
            files[@"1\1.1.txt"] = "http://in.sap360.com.cn:559/group1/M00/00/83/wKgBuFztBZOEDBnNAAAAACmN-r8421.txt";
            files[@"1\1.2.jpg"] = "http://in.sap360.com.cn:559/group1/M00/00/82/wKgB0VztBUuEBwWjAAAAAK_nr34555.jpg";
            files[@"1\11\11.1.txt"] = "http://in.sap360.com.cn:559/group1/M00/00/83/wKgBuFztBZOEDBnNAAAAACmN-r8421.txt";
            files[@"1\11\11.2.jpg"] = "http://in.sap360.com.cn:559/group1/M00/00/82/wKgB0VztBUuEBwWjAAAAAK_nr34555.jpg";
            files[@"1\12\12.1.txt"] = "http://in.sap360.com.cn:559/group1/M00/00/83/wKgBuFztBZOEDBnNAAAAACmN-r8421.txt";
            files[@"1\12\12.2.jpg"] = "http://in.sap360.com.cn:559/group1/M00/00/82/wKgB0VztBUuEBwWjAAAAAK_nr34555.jpg";
            
            files[@"2\2.1.jpg"] = "http://in.sap360.com.cn:559/group1/M00/00/82/wKgB0VztBUuEBwWjAAAAAK_nr34555.jpg";
            files[@"2\2.2.jpg"] = "http://in.sap360.com.cn:559/group1/M00/00/82/wKgB0VztBUuEBwWjAAAAAK_nr34555.jpg";
            files[@"2\21\21.1.jpg"] = "http://in.sap360.com.cn:559/group1/M00/00/82/wKgB0VztBUuEBwWjAAAAAK_nr34555.jpg";
            files[@"2\21\211\211.1.xml"] = "http://in.sap360.com.cn:559/group1/M00/00/83/wKgBuFztBlKEcy6mAAAAAJ4JYRY426.xml";
            
            files[@"3\3.1.jpg"] = "http://in.sap360.com.cn:559/group1/M00/00/83/wKgBuFztBTyET87DAAAAACnsJuc074.jpg";
            files[@"3\3.2.jpg"] = "http://in.sap360.com.cn:559/group1/M00/00/83/wKgBuFztBTyET87DAAAAACnsJuc074.jpg";
            files[@"3\31\31.1.txt"] = "http://in.sap360.com.cn:559/group1/M00/00/83/wKgBuFztBZOEDBnNAAAAACmN-r8421.txt";
            files[@"3\31\31.2.jpg"] = "http://in.sap360.com.cn:559/group1/M00/00/82/wKgB0VztBUuEBwWjAAAAAK_nr34555.jpg";
            files[@"3\32\32.1.txt"] = "http://in.sap360.com.cn:559/group1/M00/00/83/wKgBuFztBZOEDBnNAAAAACmN-r8421.txt";
            files[@"3\32\32.2.jpg"] = "http://in.sap360.com.cn:559/group1/M00/00/82/wKgB0VztBUuEBwWjAAAAAK_nr34555.jpg";
            files[@"3\32\321\321.1.jpg"] = "http://in.sap360.com.cn:559/group1/M00/00/83/wKgBuFztBTyET87DAAAAACnsJuc074.jpg";
            files[@"3\32\321\321.2.jpg"] = "http://in.sap360.com.cn:559/group1/M00/00/83/wKgBuFztBTyET87DAAAAACnsJuc074.jpg";

            //files[@"111\211\新建文本文档.txt"] = "http://in.sap360.com.cn:559/group1/M00/00/83/wKgBuFztBZOEDBnNAAAAACmN-r8421.txt";
            //files[@"111\Result.xml"] = "http://in.sap360.com.cn:559/group1/M00/00/83/wKgBuFztBlKEcy6mAAAAAJ4JYRY426.xml";
            //files[@"txt\SAP360 服务端改进方案.docx"] = "http://in.sap360.com.cn:559/group1/M00/00/82/wKgB0VztBmOEezNlAAAAALzsU2M46.docx";
            //files[@"txt\移动端API 迁移.xlsx"] = "http://in.sap360.com.cn:559/group1/M00/00/83/wKgBuFztBn-ENZokAAAAAGrgJjs86.xlsx";

            // 20M
            //files[@"GitHub入门与实践.pdf"] = "http://in.sap360.com.cn:559/group1/M00/00/82/wKgB0VzvjZWEICbWAAAAAF5PKhM877.pdf";
            //files[@"SAP培训教材和帮助文档.rar"] = "http://in.sap360.com.cn:559/group1/M00/00/83/wKgBuFztBVyEU33wAAAAADPl4j8969.rar";
            //files[@"新保理资产包打包上传功能开发方案.docx"] = "http://in.sap360.com.cn:559/group1/M00/00/82/wKgB0VztBXiERBKIAAAAAL5QcqE09.docx";
            //files[@"Soft\SW_DVD5_Office_Professional_Plus_2013_64Bit_ChnSimp_MLF_X18-55285.ISO"] = "http://in.sap360.com.cn:559/group1/M00/00/82/wKgB0VztCFWECoDDAAAAAPh_DBA881.ISO";


            // 重置初始化目录，防止之前未清除的文件冗余。note.标准目录结构  【 !!TempZip\cid 】 后缀没有 "\"
            var baseFolder = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName +
                             Path.DirectorySeparatorChar + "!!TempZip"
                             + Path.DirectorySeparatorChar + companyID;
            if (Directory.Exists(baseFolder))
                Directory.Delete(baseFolder, true);
            Directory.CreateDirectory(baseFolder);

            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            // 文件下载目录： 【!!TempZip\cid\xxx 】
            var downRootFolder = baseFolder + Path.DirectorySeparatorChar + zipName;
            var tasks = new List<Task>();
            foreach (var kv in files)
            {
                var descFile = downRootFolder + Path.DirectorySeparatorChar + kv.Key;
                var descFolder = Path.GetDirectoryName(descFile);
                if (!Directory.Exists(descFolder) && !string.IsNullOrWhiteSpace(descFolder))
                    Directory.CreateDirectory(descFolder);


                tasks.Add(Task.Factory.StartNew(() => { DownLoadFile(kv.Value, descFile); }));

                //using (var webClient = new WebClient())
                //{
                //    tasks.Add(webClient.DownloadFileTaskAsync(kv.Value, descFile));
                //}
            }
            Task.WaitAll(tasks.ToArray());

            sw.Stop();





            // zip压缩后目录（逐目录生成）：   【!!TempZip\cid\xxx-zip 】
            var zipRootFolder = baseFolder + Path.DirectorySeparatorChar + zipName + "-zip";
            var downFolders = Directory.GetDirectories(downRootFolder, "*", SearchOption.AllDirectories).ToList();
            downFolders.Insert(0, downRootFolder);
            foreach (var downFolder in downFolders)
            {
                // 无具体文件则退出
                if (Directory.GetFiles(downFolder, "*", SearchOption.AllDirectories).Length == 0)
                    continue;

                // 参照下载目录，构建同类的压缩目录【!!TempZip\cid\xxx 】  ==>  【!!TempZip\cid\xxx-zip 】
                var zipFolder = downFolder.Replace(downRootFolder, zipRootFolder);
                if (!Directory.Exists(zipFolder) && !string.IsNullOrWhiteSpace(zipFolder))
                    Directory.CreateDirectory(zipFolder);

                sw.Restart();

                if (downFolder == downRootFolder)
                {
                    var zipFile = zipFolder + Path.DirectorySeparatorChar + zipName + ".zip";
                    System.IO.Compression.ZipFile.CreateFromDirectory(downFolder, zipFile);
                }
                else
                {
                    var parentFolder = Directory.GetParent(zipFolder).FullName;
                    var dirShortName = zipFolder.Replace(parentFolder, "")
                        .Replace(Path.DirectorySeparatorChar.ToString(), "");

                    var zipFile = parentFolder + Path.DirectorySeparatorChar + dirShortName + ".zip";
                    System.IO.Compression.ZipFile.CreateFromDirectory(downFolder, zipFile);
                }

                

                sw.Stop();

            }





        }

        private void DownLoadFile(string url, string descFile)
        {
            var replayCnt = 5;
            try
            {
                using (var webClient = new WebClient())
                {
                    webClient.DownloadFile(url, descFile);
                }
            }
            catch (Exception e)
            {
                replayCnt--;
                if (replayCnt > 0)
                    DownLoadFile(url, descFile);
                else
                    throw e;
            }




        }



        public async Task MemoryCreate()
        {
            var attachments = new List<AttachmentReqApiModel>
            {
                #region MyRegion

                new AttachmentReqApiModel
                {
                    attachmentname = "中登清单.pdf",
                    AttachFullUrl =
                        "http://139.159.192.213:36002/DocManage/DownLoadFile/8fc15228892c4c70b428022708118f94_2019_7_2_9eae6129beb843e1a184c9688462413d.pdf",
                },
                new AttachmentReqApiModel
                {
                    attachmentname = "0138上海泾东-保理协议.pdf",
                    AttachFullUrl =
                        "http://baoli.sap360.com.cn:36002/DocManage/DownLoadFile/8fc15228892c4c70b428022708118f94_2019_12_20_3050e09e507f43a1bacef7b496b912da.pdf"
                }

                #endregion
            };

            // 下载
            var dictFile = new Dictionary<string, byte[]>();
            foreach (var attach in attachments)
            {
                dictFile[attach.attachmentname] = await DownLoadFile2(attach.AttachFullUrl);
            }

            // 将字节文件列表，压缩
            using (var stream = new MemoryStream())
            {
                // 以 Encoding.GetEncoding("GBK")方式 回写中文名，模拟window操作
                using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, true, Encoding.GetEncoding("GBK")))
                {
                    foreach (var key in dictFile.Keys)
                    {
                        // 单个文件处理
                        var entry = archive.CreateEntry(key, CompressionLevel.NoCompression);
                        using (var entryStream = entry.Open())
                        {
                            using (var compressStream = new MemoryStream(dictFile[key]))
                            {
                                compressStream.CopyTo(entryStream);
                            }
                        }
                    }
                }

                // 流写入文件
                // stream.ToArray();
                using (var fileStream = new FileStream(@"D:/1.Desktop/result/attach_out.zip", FileMode.Create))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(fileStream);
                }
            }

        }

        public async Task MemoryRead(byte[] result)
        {
            using (var stream = new MemoryStream(result))
            {
                // Encoding.GetEncoding("GBK") 假定文件是 win桌面压缩
                using (var archive = new ZipArchive(stream, ZipArchiveMode.Read, true, Encoding.GetEncoding("GBK")))
                {
                    // 如果存在 xml文件，则是发生错误
                    var entryErrorXml = archive.Entries.FirstOrDefault(c =>
                        c.Name.EndsWith(".xml", StringComparison.OrdinalIgnoreCase));
                    #region if (entryXml != null)

                    if (entryErrorXml != null)
                    {
                        var bytes = ReadBytes(entryErrorXml);
                        var str = Encoding.UTF8.GetString(bytes);
                        //var obj = XmlExtends.FromXml<QueryBySubjectRspApiModel>(str);
                    }

                    #endregion

                    // 如果不存在登记证明文件，则返回错误， ex.00031470000003746685.pdf 登记编号序列
                    var pdfName = "xxx" + ".pdf";
                    var entryReg = archive.Entries.FirstOrDefault(c => c.Name.ToLower() == pdfName.ToLower());
                    #region if (entryReg == null)
                    if (entryReg == null)
                    {
                    }
                    #endregion

                    // 解析、上传登记证明文件
                    var regData = ReadBytes(entryReg);  // toBytes
                    #region pdfText
                    using (var inputStream = new java.io.ByteArrayInputStream(regData))
                    {
                        using (var doc = org.apache.pdfbox.pdmodel.PDDocument.load(inputStream))
                        {
                            var pdfStripper = new org.apache.pdfbox.util.PDFTextStripper();
                            var pdfText = pdfStripper.getText(doc);
                        }
                    } 
                    #endregion

                } // end.of. using (var zipArchive = new ZipArchive(stream))
            } // end.of. using  using (var stream = new MemoryStream(data))
        }

        private static byte[] ReadBytes(ZipArchiveEntry entry)
        {
            byte[] bytes;
            using (Stream entryStream = entry.Open())
            {
                using (var reader = new BinaryReader(entryStream))
                {
                    bytes = reader.ReadBytes((int)entry.Length);
                }
            }

            return bytes;
        }



        private async Task<byte[]> DownLoadFile2(string url)
        {
            var replayCnt = 5;
            try
            {
                using (var webClient = new WebClient())
                {
                    return await webClient.DownloadDataTaskAsync(url);
                }
            }
            catch (Exception e)
            {
                replayCnt--;
                if (replayCnt > 0)
                    return await DownLoadFile2(url);
                else
                    throw e;
            }
        }


        private void ZipFilesView()
        {
            // 在操作系统里面打包的文件，其文件名在C# 读取时乱码   0138�Ϻ�����-����Э��.pdf
            var aa = @"D:/1.Desktop/result/attach_out_OK.zip";
            var aaFilse = new List<string>();
            using (ZipArchive archive = ZipFile.OpenRead(aa))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    aaFilse.Add(entry.FullName);
                }
            }

            // 在C#打包的文件，其文件名在C# 读取时正常    0138上海泾东-保理协议.pdf
            var bb = @"D:/1.Desktop/result/attach_out_err.zip";    
            var bbFilse = new List<string>();
            using (ZipArchive archive = ZipFile.OpenRead(bb))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    bbFilse.Add(entry.FullName);
                }
            }
        }


    }



    public class AttachmentReqApiModel
    {
        /// <summary>
        /// 名称，【非空】【任意字符（汉字）】【最大长度30】
        /// note.附件只能是(jpg、pdf)格式，附件大小合计不超过20M,100个
        /// </summary>
        public string attachmentname { get; set; }

        /// <summary>
        /// 附件路径
        /// </summary>
        public string AttachFullUrl { get; set; }
    }
}
