using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication1.Util;
using Newtonsoft.Json.Linq;

namespace MyTestExt.ConsoleApp
{
    public class JsonTest
    {
        public static void Do()
        {
            Do1();

            var obj = new JsonTestModel();
            var str = JsonParse.Serialize(obj);


            var aa = "非法调用";
            var aab = JsonParse.Deserialize<string>(aa);
        }

        public static void Do1()
        {
            var jObj = new JObject();

            jObj.Add("vk1", new JObject
            {
                {"key", "key1"},
                {"val", "val1"}
            });

            jObj.Add("vk2", new JObject
            {
                {"key", "key2"},
                {"val", "val2"}
            });

            var str = JsonParse.Serialize(jObj);
            /*
             {
  "vk1": {
    "key": "key1",
    "val": "val1"
  },
  "vk2": {
    "key": "key2",
    "val": "val2"
  }
}
             
             
             */
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
