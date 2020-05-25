using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using SevenZip;

namespace MyTestExt.ConsoleApp
{
    public class Zip_JavacFileTest
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

            SevenZipSharp_Extract();

            
            sw.Stop();

        }


        public void SevenZipSharp_Entries()
        {
            var file = @"D:\0.Work\FtpTest-compress\D_sap360File8fc15228892c4c70b428022708118f94202051228b1397a8ad048198aca44c88039481e.zip";

            using (var szipExt = new SevenZipExtractor(file))
            {
                var names = szipExt.ArchiveFileNames;

                for (int i = 0; i < szipExt.ArchiveFileData.Count; i++)
                {
                    var aa = szipExt.ArchiveFileData[i];
                    //tmp.ExtractFiles(@"d:\max\", tmp.ArchiveFileData[i].Index); //解压文件路径
                }
            }
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


        public void SevenZipSharp_Extract()
        {
            var file = @"D:\0.Work\FtpTest-compress\D_sap360File8fc15228892c4c70b428022708118f94202051228b1397a8ad048198aca44c88039481e.zip";
            var dir = @"D:\0.Work\FtpTest-compress\1\";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            //var fs0 = new FileStream(file, FileMode.Open, FileAccess.Read);
            //var data = new byte[fs0.Length];
            //fs0.Read(data, 0, data.Length);

            //using (var fs0 = new FileStream(file, FileMode.Open, FileAccess.Read))
            //{
                //fs0.Position = 0;
                using (var zip = new SevenZipExtractor(file))
                {
                    foreach (var entry in zip.ArchiveFileData)
                    {
                        var fullName = dir + ReplaceInvalidChars(entry.FileName);
                        var newPath = Path.GetDirectoryName(fullName);
                        if (!Directory.Exists(newPath)) Directory.CreateDirectory(newPath);

                        using (var fs = new FileStream(fullName, FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            zip.ExtractFile(entry.FileName, fs);
                            var aa = fs.Length;
                        }
                    }
                }
            //}

            
        }




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
