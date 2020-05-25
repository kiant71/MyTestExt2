using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyTestExt.ConsoleAppCore
{
    public class ZipArchiveCoreTest
    {
        public void Do()
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();


            //ZipActive_Entries();

            ZipArchive_Extract();

            
            sw.Stop();

        }


        public void ZipArchive_Entries()
        {
            var file = @"D:\0.Work\FtpTest-compress\D_sap360File8fc15228892c4c70b428022708118f94202051228b1397a8ad048198aca44c88039481e.zip";

            var fs = new FileStream(file, FileMode.Open, FileAccess.Read);
            var data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            using (var stream = new MemoryStream(data))
            {
                using (var archive = new ZipArchive(stream, ZipArchiveMode.Read, true, Encoding.UTF8))
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
                    var pdfName = "06198101000734947685.pdf";
                    var entryReg = archive.Entries.FirstOrDefault(c => c.Name.ToLower() == pdfName.ToLower());
                    if (entryReg == null)
                    {
                        // error.
                    } 

                    // 解析、上传(系统的)登记证明文件.pdf
                    var regData = ReadBytes(entryReg);

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

        #region MyRegion
        private static byte[] ReadBytes(ZipArchiveEntry entry)
        {
            byte[] bytes;
            using (var entryStream = entry.Open())
            {
                using (var reader = new BinaryReader(entryStream))
                {
                    bytes = reader.ReadBytes((int)entry.Length);
                }
            }

            return bytes;
        } 
        #endregion


        public void ZipArchive_Extract()
        {
            var file = @"D:\0.Work\FtpTest-compress\Unix_file.zip";
            var dir = @"D:\0.Work\FtpTest-compress\1\";

            // ex.1 https://stackoverflow.com/questions/42262013/how-can-i-get-the-valid-entries-of-a-ziparchive-when-one-them-contains-illegal-c
            
            // ex.2.ZipFile.ExtractToDirectory(file, @"D:\0.Work\FtpTest-compress\1\");
            // 文件名、目录名或卷标语法不正确。 :
            // 'D:\0.Work\FtpTest-compress\1\HZRHDX-20190705-002-000001||at||1.pdf'

            using var fs = new FileStream(file, FileMode.Open, FileAccess.Read);
            var bytes = new byte[fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            
            using var stream = new MemoryStream(bytes);
            
            // 这里只是示例 文件->流->字节缓冲区 的转换方式，实际上可以进行删减
            using var zip = new ZipArchive(stream, ZipArchiveMode.Read, true, Encoding.UTF8);
            foreach (var entry in zip.Entries.ToList())
            {
                var fullName = dir + ReplaceInvalidChars(entry.FullName);
                var path = Path.GetDirectoryName(fullName);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                entry.ExtractToFile(fullName, true);  // 或者可以直接读取流 Stream entryStream = entry.Open()
            }
        }



        /// <summary>
        /// 替换掉（文件名包含的）特殊字符
        /// </summary>
        public string ReplaceInvalidChars(string filename, string replaceStr = "_")
        {
            var invalidStr = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            foreach (var c in invalidStr)
            {
                filename = filename.Replace(c.ToString(), replaceStr); 
            }

            return filename;
        }

    }




}
