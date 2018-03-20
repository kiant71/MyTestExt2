using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;

namespace MyTestExt.ConsoleApp
{
    public class IonicZipTest
    {
        public static void DoTest()
        {
            string path = @"f:\email";
            using (ZipFile zip = new ZipFile(System.Text.Encoding.Default))
            {
                zip.AddDirectory(path, "");
                zip.Save(@"f:\email.zip");
            }
            Console.Read();

            using (ZipFile zip = new ZipFile(@"f:\email.zip", System.Text.Encoding.Default))
            {
                foreach (ZipEntry entry in zip)
                {
                    entry.Extract(@"f:\email2", ExtractExistingFileAction.OverwriteSilently);
                }
            }
            Console.Read();

        }

    }
}
