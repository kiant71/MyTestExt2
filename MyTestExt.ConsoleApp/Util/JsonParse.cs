using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
    }

    public class BoolConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((bool)value) ? 1 : 0);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) return false;
            return reader.Value.ToString() == "1";
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(bool);
        }
    }

    public class DateTimeConverter : JsonConverter
    {




        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value != null && string.IsNullOrWhiteSpace(reader.Value.ToString())) return existingValue;
            return reader.Value;
        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }
    }
}
