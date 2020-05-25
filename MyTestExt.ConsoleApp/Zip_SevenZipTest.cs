using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using java.io;
using MyTestExt.ConsoleApp.Util;
using MyTestExt.Utils;
using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;
using SevenZip;
using File = System.IO.File;

namespace MyTestExt.ConsoleApp
{
    public class Zip_SevenZipTest
    {
      public void Do()
        {
            // 如果是服务，则BasePath
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            SevenZipBase.SetLibraryPath(Environment.Is64BitProcess 
                ? basePath + @"\Libs\7z64.dll" 
                : basePath + @"\Libs\7z.dll");

            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();


            //SevenZipSharp_Entries();

            //SevenZipSharp_Extract();
            //SevenZipSharp_ExtractStream();
            SevenZipSharp_ZhongDengTest();
            
            sw.Stop();

        }



        #region MyRegion

        #endregion



        public void SevenZip_ExtractStream0()
        {
            var file = @"D:\0.Work\FtpTest-compress\Unix_File.zip";
            var dir = @"D:\0.Work\FtpTest-compress\1\";
            var nameKey = "06198101000734947685";

            var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            var data = new byte[fileStream.Length];
            fileStream.Read(data, 0, data.Length);

            // 直接读取 Stream流格式会报错，所以暂时做一个转义（先临时保存）
            // Invalid archive: open/read error! Is it encrypted and a wrong password was provided?
            // If your archive is an exotic one, it is possible that SevenZipSharp has no signature for its format and thus decided it is TAR by mistake.
            var tmpZipFullName = CreateTmpFile(data, nameKey);
            using (var zip = new SevenZipExtractor(tmpZipFullName))
            {
                // 解压所有文件到指定目录
                // zip.ExtractArchive(targetDirectory);  

                // 解压单个文件到指定目录
                // zip.ExtractFiles(targetDirectory, 包内的文件索引或者文件名);

                // 包内单个文件读取到字节缓冲区
                //foreach (var entry in zip.ArchiveFileData)
                //{
                //    using (var memoryStream = new MemoryStream())
                //    {
                //        zip.ExtractFile(entry.FileName, memoryStream);

                //        var entryBuffer = new byte[memoryStream.Length];
                //        memoryStream.Position = 0;
                //        memoryStream.Read(entryBuffer, 0, entryBuffer.Length);
                //    }
                //}

                // 解压单个文件到指定目录（允许改文件名，以流方式写入）
                foreach (var entry in zip.ArchiveFileData)
                {
                    var newFullName = dir + ReplaceInvalidChars(entry.FileName);
                    var newPath = Path.GetDirectoryName(newFullName);
                    if (!string.IsNullOrWhiteSpace(newPath) && !Directory.Exists(newPath))
                        Directory.CreateDirectory(newPath);
                    using (var fs = new FileStream(newFullName, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        zip.ExtractFile(entry.FileName, fs); // 将包内的文件以流的方式写入
                    }
                }

            } // enf.of using (var zip = new SevenZipExtractor(tmpZipFullName))
        }
        
        public void SevenZipSharp_ZhongDengTest()
        {
            var file = @"D:\0.Work\FtpTest-compress\Unix_File.zip";
            var postNo = "06198101000734947685";

            var fs0 = new FileStream(file, FileMode.Open, FileAccess.Read);
            var result = new byte[fs0.Length];
            fs0.Read(result, 0, result.Length);
            var tmpZipFullName = CreateTmpFile(result, postNo);
            try
            {
                using (var zip = new SevenZipExtractor(tmpZipFullName))
                {
                    // 如果存在 xml文件，则是发生错误
                    var entryErrorXmlName = zip.ArchiveFileNames.FirstOrDefault(c =>
                        c.EndsWith(".xml", StringComparison.OrdinalIgnoreCase));
                    #region if (entryXml != null)

                    if (!string.IsNullOrWhiteSpace(entryErrorXmlName))
                    {
                        var bytes = ReadBytes(zip, entryErrorXmlName);
                        var str = Encoding.UTF8.GetString(bytes);

                        //var obj = XmlExtends.FromXml<QueryBySubjectRspApiModel>(str);

                        var msg = "queryDownByNum 查询结果错误!" + " no: " + postNo;
                        throw new Exception(msg);
                    }

                    #endregion

                    // 如果不存在登记证明文件，则返回错误， ex.00031470000003746685.pdf 登记编号序列
                    var pdfName =  postNo + ".pdf";


                    var entryRegName = zip.ArchiveFileNames.FirstOrDefault(c => c.ToLower() == pdfName.ToLower());
                    #region if (entryReg == null)
                    if (string.IsNullOrWhiteSpace(entryRegName))
                    {
                        var msg = "queryDownByNum 不存在登记证明（pdf）文件!" + "no: " + postNo;
                        throw new Exception(msg);
                    } 
                    #endregion

                    // 解析、上传(系统的)登记证明文件.pdf
                    var regData = ReadBytes(zip, entryRegName);
                    #region MyRegion
                    using (var inputStream = new ByteArrayInputStream(regData))
                    {
                        using (var doc = PDDocument.load(inputStream))
                        {
                            var pdfStripper = new PDFTextStripper();
                            var RegisterText = pdfStripper.getText(doc);
                        }

                        var newFileName = ReplaceInvalidChars(entryRegName);
                        var RegisterFile = Upload(regData, newFileName);
                        LogHelper.Debug(string.Format("查询-按编号下载 上传证明文件！ rsp:{0}", "111"));
                    } 
                    #endregion

                    // 上传登记资料的附件列表
                    var attachments = new List<object>();
                    foreach (var entryAttachName in zip.ArchiveFileNames.Where(c =>
                                                    c.ToLower() != pdfName.ToLower()))
                    {
                        var attachData = ReadBytes(zip, entryAttachName);
                        var newFileName = ReplaceInvalidChars(entryAttachName);
                        var attach = Upload(attachData, newFileName);
                        attachments.Add(attach);
                        LogHelper.Debug(string.Format("查询-按编号下载 上传登记附件！ rsp:{0}", "1111"));
                    }

                } // end.of. using (var zipArchive = new ZipArchive(stream))
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(tmpZipFullName) && System.IO.File.Exists(tmpZipFullName)) 
                    System.IO.File.Delete(tmpZipFullName);
            }

            
        }


                #region MyRegion

        private static string CreateTmpFile(byte[] data, string nameKey)
        {
            // 直接读取 Stream流格式会报错，所以暂时做一个转义（先临时保存）
            // Invalid archive: open/read error! Is it encrypted and a wrong password was provided?
            // If your archive is an exotic one, it is possible that SevenZipSharp has no signature for its format and thus decided it is TAR by mistake.
            
            var dir = @"D:\0.Work\FtpTest-compress\1";
            var baseFolder = dir + Path.DirectorySeparatorChar + "TmpZip" + Path.DirectorySeparatorChar;
            if (!System.IO.Directory.Exists(baseFolder)) Directory.CreateDirectory(baseFolder);

            var tmpZipFullName = string.Format("{0}{1}_{2}.zip"
                , baseFolder, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff"), nameKey);
            if (System.IO.File.Exists(tmpZipFullName)) System.IO.File.Delete(tmpZipFullName);

            using (var fs1 = new FileStream(tmpZipFullName, FileMode.Create, FileAccess.Write))
            {
                fs1.Write(data, 0, data.Length);
            }

            return tmpZipFullName;
        }


        private static object Upload(byte [] bytes, string fileName)
        {
            using (var fs = new MemoryStream(bytes))
            {


                return null;
            }
        }

       

        private static byte[] ReadBytes(SevenZipExtractor zip, string entryFileName)
        {
            using (var stream = new MemoryStream())
            {
                zip.ExtractFile(entryFileName, stream);
                
                var bytes = new byte[stream.Length];
                stream.Position = 0;
                stream.Read(bytes, 0, bytes.Length);
                return bytes;
            }

            //byte[] bytes;
            //using (var entryStream = entry..Open())
            //{
            //    using (var reader = new BinaryReader(entryStream))
            //    {
            //        bytes = reader.ReadBytes((int)entry.Length);
            //    }
            //}

            //return bytes;
        } 
        #endregion




        public string ReplaceInvalidChars(string filename, string replaceStr = "_")
        {
            var invalidStr = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            foreach (char c in invalidStr)
            {
                filename = filename.Replace(c.ToString(), replaceStr); 
            }

            return filename;
        }

    }




}
