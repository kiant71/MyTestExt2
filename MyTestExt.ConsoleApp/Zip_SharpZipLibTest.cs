
using System.IO;

using ICSharpCode.SharpZipLib.Zip;


namespace MyTestExt.ConsoleApp
{
    public class Zip_SharpZipLibTest
    {
        public void Do()
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            
            //SharpZipLib_Entries();
            SharpZipLib_Extract();

            sw.Stop();
        }



        public void SharpZipLib_Entries()
        {
            var file = @"xxx.zip";

            using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                using (var archive = new ZipFile(stream))
                {

                }
            }
        }

        
        public void SharpZipLib_Extract()
        {
            var file = @"xxx.zip";

            using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                new FastZip().ExtractZip(file, @"D:\0.Work\FtpTest-compress\1\", "");
            }
        }


    }



}
