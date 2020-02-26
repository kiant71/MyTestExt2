using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using java.io;
using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;
using File = System.IO.File;

namespace MyTestExt.ConsoleApp
{
    public class PdfTest
    {
        public void Do()
        {
            Read();
        }

        public void Read()
        {
            var fileName = @"D:\1.Desktop\result\00031470000003746685.pdf";

            if (!File.Exists(fileName))
                return ;

            try
            {
                //using (var doc = PDDocument.load(fileName))
                //{
                //    var pdfStripper = new PDFTextStripper();
                //    var text = pdfStripper.getText(doc);
                //}
                
                //using (var fs = new FileInputStream(fileName))
                //{
                //    using (var doc = PDDocument.load(fs))
                //    {
                //        var pdfStripper = new PDFTextStripper();
                //        var text = pdfStripper.getText(doc);
                //    }
                //}

                byte[] bytes = null;
                using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    bytes = new byte[fs.Length];
                    fs.Seek(0, SeekOrigin.Begin);
                    fs.Read(bytes, 0, bytes.Length);
                }

                using (var inputStream = new ByteArrayInputStream(bytes))
                {
                    using (var doc = PDDocument.load(inputStream))
                    {
                        var pdfStripper = new PDFTextStripper();
                        var text = pdfStripper.getText(doc);
                    }
                }
                
            }
            catch (Exception e)
            {

            }

        }
    }
}
