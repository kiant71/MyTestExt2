using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication1.Util;
using MyTestExt.Utils.Json;
//<<<<<<< HEAD
//using MyTestExt.Utils.Json;
//=======
using Newtonsoft.Json.Linq;
//>>>>>>> 24c0f261ed84310cc5bb4541765d3f317093f11e

namespace MyTestExt.ConsoleApp
{
    public class JsonTest
    {
        public static void Do()
        {
//<<<<<<< HEAD
//            //var obj = new JsonTestModel();
//            //var str = JsonParse.Serialize(obj);
//=======
            Do1();

            //var obj = new JsonTestModel();
            //var str = JsonNet.Serialize(obj);
//>>>>>>> 24c0f261ed84310cc5bb4541765d3f317093f11e

            //var str = "{\"mlimd@163.com\": \"2\",\"13067825582\": \"2\",\"13867410069\": \"2\"}";
            //var str1 = JsonParse.Deserialize<Dictionary<string, string>>(str);

            var str = "{\"errno\":0,\"cost\":57,\"data\":{\"a34963a0a786a1572467761\":\"1\"},\"errmsg\":\"\"}";
            var strO = JsonNet.Deserialize<ApiRspModel<Dictionary<string, string>>>(str);
            

            var aa = "非法调用";
            var aab = JsonNet.Deserialize<string>(aa);
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

            var str = JsonNet.Serialize(jObj);
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

    public class ApiRspModel<T>
    {
        /// <summary>
        /// Api 调用成功
        /// </summary>
        public bool ApiIsSucc => errno == "0";

        /// <summary>
        /// 错误码, 如果为 0 表示成功没有错误
        /// </summary>
        public string errno { get; set; }

        /// <summary>
        /// 错误描述，结合 errno 对照附录
        /// </summary>
        public string errmsg { get; set; }

        /// <summary>
        /// 输出数据
        /// </summary>
        public T data { get; set; }

        /// <summary>
        /// 接口处理花费的时间
        /// </summary>
        public string cost { get; set; }


    }

    public class JsonTestModel
    {
        public int ID { get; set; }

        public decimal Money { get; set; }

        public string Text { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
