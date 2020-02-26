using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Readers;

namespace MyTestExt.ConsoleApp
{
    public class SharpCompressTest
    {
        public static void Do()
        {
            var baseDir = @"D:\0.Work\";
            var unCompressDir = @"D:\0.Work\UnCompress\";
            var names = new List<string>
            {
                @"DoWork.7z",
                @"DoWork.zip",
                @"DoWork.rar"
            };

            foreach (var name in names)
            {
                var sourceFile = baseDir + name;
                var targetFolder = unCompressDir + name;

                var sw = new Stopwatch();
                sw.Start();

                FolderDeleteAll(targetFolder);

                Directory.CreateDirectory(targetFolder);
                UnCompress(sourceFile, targetFolder);

                sw.Stop();
                Console.WriteLine("sw: " + sw.ElapsedMilliseconds / 1000);
            }
            
        }

        public static void FolderDeleteAll(string targetFolder)
        {
            if (!Directory.Exists(targetFolder))
                return;

            var files = Directory.GetFiles(targetFolder, "*", SearchOption.AllDirectories);
            if (files.Any())
            {
                foreach (var file in files)
                {
                    // 只读文件先修改属性，然后再处理
                    var fi = new FileInfo(file);
                    fi.IsReadOnly = false;
                    //if (fi.Attributes.ToString().IndexOf("ReadOnly", StringComparison.Ordinal) != -1)
                    //    fi.Attributes = FileAttributes.Normal;

                    File.Delete(file);
                }
            }

            Directory.Delete(targetFolder, true);
        }

        public static void UnCompress(string sourceFile, string targetFolder)
        {
            if (string.IsNullOrWhiteSpace(sourceFile) || string.IsNullOrWhiteSpace(targetFolder))
                throw new Exception("参数为空！");

            if (Path.GetExtension(sourceFile).ToLower() == ".7z")
            {
                using (var archive = ArchiveFactory.Open(sourceFile))
                {
                    foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                    {
                        entry.WriteToDirectory(targetFolder,
                            new ExtractionOptions
                            {
                                ExtractFullPath = true,
                                Overwrite = true,
                                PreserveAttributes = true,
                                PreserveFileTime = true
                            });
                    }
                }
            }
            else
            {
                using (var stream = File.OpenRead(sourceFile))
                {
                    var reader = ReaderFactory.Open(stream);
                    while (reader.MoveToNextEntry())
                    {
                        if (!Directory.Exists(targetFolder))
                            Directory.CreateDirectory(targetFolder);

                        if (!reader.Entry.IsDirectory)
                            reader.WriteEntryToDirectory(targetFolder, new ExtractionOptions
                            {
                                ExtractFullPath = true,
                                Overwrite = true,
                            });
                    }
                }
            }

        }

       



    }
}
