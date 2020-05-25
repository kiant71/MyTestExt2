using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using MyTestExt.ConsoleApp;

namespace ConsoleApplication1
{
    class Program
    {
        public static void Main(string[] args)
        {
            //var aa = new List<string> {"aa", "bb", "cc"};
            //var bb = aa.Take(20);

            //new AsyncTest().Test();
            //new AttributeTest().Do();

            //new BspTest().Do();

            //DataAccessTest.Test();

            // 时间
            //DateTimeTest.Test_StringParse();

            //DictionaryTest.Test();
            //DownTest.Do();


            //EnumTest.DoTest();
            //EnumAttributeTest.Do();

            //DynamicTest.Do();
            //EnCodingTest.Test();

            //FileTest.Do();
            //ForEachTest.Do();

            //<<<<<<< HEAD
            //            //HttpClientTest.Do();

            //=======
            //>>>>>>> 24c0f261ed84310cc5bb4541765d3f317093f11e
            //JsonTest.Do();

            //new IntergTest().Do();

            //InheriteTest.Do();

            //ITextSharpTest.Do();

            //new ListTest().Do();

            //MongodbTest.Do();

            //OpenImApiTest.Test();

            //OrmLiteTest.Do3();

            //new PdfTest().Do();

            //RSATest2.Do4();

            //ReadErrorCode();

            new RegularTest().Do();

            //SharpCompressTest.Do();
            //SignClientTest.Do();
            //SignServerTest.Do1();
            //new SM2Test().Do();
            //new SM2Test2().Do();
            //new SM2Test3().Do();
            //new SM2Test4().Do();
            //new StringTest().Do();

            //TestLinq.Test();


            //WebClientTest.Do();





            //XmlTest.Do();

            //ZhongDengTest.Do();

            //new ZipTest().Do();

            while (true)
                System.Threading.Thread.Sleep(1000);
        }

        
        public static void ReadErrorCode()
        {
            try
            {
                var resource = "ConsoleApplication1.Resource.ErrorCode.xml";
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
                {
                    if (stream == null) return;
                    XDocument doc = XDocument.Load(stream);
                    var q = (from e in doc.Element("Language").Elements()
                             select new
                             {
                                 Key = e.Attribute("key").Value,
                                 Value = e.Attribute("value").Value,
                             }).GroupBy(c => c.Key)
                             .Select(c => c.First())
                             .ToDictionary(c => c.Key, c => c.Value);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
