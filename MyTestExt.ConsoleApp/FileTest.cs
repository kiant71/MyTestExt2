using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LumiSoft.Net.Log;

namespace MyTestExt.ConsoleApp
{
    public class FileTest
    {
        public static void Do()
        {
            MoveTest();
        }


        public static void MoveTest()
        {
            var fileStr = @"D:\1.Desktop\result\QueryBySubject_OK.zip";
            var fi = new FileInfo(fileStr);

            var _ftpFolder = @"D:\1.Desktop";

            var relativePath = fi.DirectoryName.Length > _ftpFolder.Length
                ? fi.DirectoryName.Replace(_ftpFolder + @"\", "")
                : ""; // ex: bbb\ddd

            var _custFolder = @"D:\0.Work";
            var backFolder = string.Format(@"{0}-backup\{1}-{2}\{3}\{4}"
                , _custFolder, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, relativePath);
            var targetFile = backFolder + @"\" + fi.Name;


            var relativePath2 = @"F:\FTP\S0027\易捷测报\2019.12".Length > @"F:\Ftp".Length
                ? @"F:\Ftp\S0027\易捷测报\2019.12".Replace(@"F:\Ftp" + @"\", "")
                : ""; // ex: bbb\ddd
        }


        public static void Do1()
        {
            var downPath = @"C:\!ssd_data\Project\relate2_down";
            var instPath = @"C:\!ssd_data\Project\relate1";

            var backRoot = string.Format(@"{0}\_UpdateBackup", instPath);
            var backPath = string.Format(@"{0}\{1}", backRoot, DateTime.Now.ToString("yyyyMMddHHmmss"));
            Directory.CreateDirectory(backPath);

            foreach (var downFile in Directory.GetFiles(downPath, "*", SearchOption.AllDirectories))
            {
                var relativeFile = downFile.Replace(downPath, "");  //文件相对路径
                //if (relativeFile.Equals(@"\" + serName + ".zip", StringComparison.OrdinalIgnoreCase))
                //    continue;   //安装包文件不进行拷贝

                var instFile = instPath + relativeFile;       //文件安装路径
                var backFile = backPath + relativeFile;       //文件备份路径

                try
                {
                    if (File.Exists(instFile))
                    {//备份
                        //Util.Util.CreateFilePath(backFile);
                        var tmpPath1 = backFile.Substring(0, backFile.LastIndexOf(@"\"));
                        if (!System.IO.Directory.Exists(tmpPath1))
                            System.IO.Directory.CreateDirectory(tmpPath1);

                        File.Move(instFile, backFile);
                    }

                    //更新
                    //Util.Util.CreateFilePath(installFile);
                    var tmpPath2 = instFile.Substring(0, instFile.LastIndexOf(@"\"));
                    if (!System.IO.Directory.Exists(tmpPath2))
                        System.IO.Directory.CreateDirectory(tmpPath2);

                    File.Move(downFile, instFile);
                }
                catch (Exception ex)
                {
                    //Logger.Log(downFile, ex);
                    Directory.Move(backRoot, instPath);  //将之前备份的文件返回
                    throw ex;//抛异常，中断循环
                }
            }

        }


    }
}
