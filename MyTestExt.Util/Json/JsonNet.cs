using System;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyTestExt.Utils.Json
{
    /// <summary>
    /// Json解析
    /// </summary>
    public class JsonNet
    {
        private static JsonSerializerSettings jsonSerializerSettings = null;

        static JsonNet()
        {
            jsonSerializerSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Local,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented,
            };

            jsonSerializerSettings.Converters.Add(new BoolConverter());
            jsonSerializerSettings.Converters.Add(new DateTimeConverter());

        }

        public static JsonSerializerSettings JsonSetting
        {
            get { return jsonSerializerSettings; }
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
            return JsonConvert.SerializeObject(obj, jsonSerializerSettings);

        }




        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return default(T);
            return JsonConvert.DeserializeObject<T>(json, jsonSerializerSettings);
        }

        public static object DeserializeObject(string json, Type type)
        {
            return JsonConvert.DeserializeObject(json, type, jsonSerializerSettings);
        }
        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static object Deserialize(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            return JsonConvert.DeserializeObject(json);

        }

        public static T Deserialize<T>(string json, string table)
        {
            if (string.IsNullOrWhiteSpace(json)) return default(T);

            var jo = (JObject)JsonConvert.DeserializeObject(json);
            string json_1 = jo[table][0].ToString();
            return JsonConvert.DeserializeObject<T>(json_1, jsonSerializerSettings);
        }

        public static object Deserialize(string json, string table)
        {
            if (string.IsNullOrWhiteSpace(json)) return null;

            var jo = (JObject)JsonConvert.DeserializeObject(json);
            string json_1 = jo[table].ToString();
            return JsonConvert.DeserializeObject(json_1);
        }





    }
}
