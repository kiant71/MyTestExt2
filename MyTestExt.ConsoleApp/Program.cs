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

            new BspTest().Do();

            //DataAccessTest.Test();

            // 时间
            //DateTimeTest.Test();

            //DictionaryTest.Test();
            //DownTest.Do();


            //EnumTest.DoTest();
            //DynamicTest.Do();
            //EnCodingTest.Test();

            //FileTest.Do();
            //ForEachTest.Do();


            //new IntergTest().Do();

            //new ListTest().Do();


            //OpenImApiTest.Test();

            //RSATest.Do();

            //SignClientTest.Do();

            //SignServerTest.Do1();

            //TestLinq.Test();

            //RequestTest.Test();

            //new RegularTest().Test();

            //ReadErrorCode();

            //StringTest.Do();


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
