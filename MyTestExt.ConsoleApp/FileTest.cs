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
