using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Util
{
    /// <summary>
    /// Json解析
    /// </summary>
    public class JsonParse
    {
        private static Newtonsoft.Json.JsonSerializerSettings jsonSerializerSettings = null;

        static JsonParse()
        {
            jsonSerializerSettings = new Newtonsoft.Json.JsonSerializerSettings
            {
                DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local
            };


        }
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(Object obj)
        {
            if (obj == null) return string.Empty;
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj, jsonSerializerSettings);
        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string json) where T : class
        {
            if (string.IsNullOrEmpty(json)) return default(T);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json, jsonSerializerSettings);

        }
    }
}
