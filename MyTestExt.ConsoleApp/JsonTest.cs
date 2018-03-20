using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication1.Util;

namespace MyTestExt.ConsoleApp
{
    public class JsonTest
    {
        public static void Do()
        {
            var obj = new JsonTestModel();
            var str = JsonParse.Serialize(obj);


            var aa = "非法调用";
            var aab = JsonParse.Deserialize<string>(aa);
        }

    }



    public class JsonTestModel
    {
        public int ID { get; set; }

        public decimal Money { get; set; }

        public string Text { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
