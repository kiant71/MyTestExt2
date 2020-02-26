using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication1.Util;
using MyTestExt.ConsoleApp.Util;
using MyTestExt.Utils.Json;
using ServiceStack.Text;

namespace MyTestExt.ConsoleApp
{
    public class HttpClientTest
    {
        public static async Task Do()
        {
            await HttpGet<dynamic>();
        }



        private static async Task<ApiRspModel<T>> HttpGet<T>()
        {
            // 接口级别， 标准接口是 v2，高精接口是 vip
            var queryVersion = "v2";
            // 领域 code
            var domain = @"sifa"; 
            var authCode = "226ho5fvBsPAH7v7E4vk";  // 授权码
            var rt = DateTime.Now.ToUnixTimeMs();
            var sign = ConvertValue.GetMd5(string.Format("{0}{1}", authCode, rt));

            var args = new Dictionary<string, object>();
            args["dataType"] = "cpws,zxgg,shixin";
            args["keyword"] = "小米";
            args["pageno"] = 1;
            args["range"] = 20;
            var argStr = JsonNet.Serialize(args);  //ConvertValue.HtmlEncode(JsonParse.Serialize(args));

            var fullUrl = string.Format(@"https://api.fahaicc.com/v2/query/sifa?authCode={0}&rt={1}&sign={2}&args={3}"
                , authCode, rt, sign, argStr);

            var resStr = "";
            ApiRspModel<T> resObj = null;
            try
            {
                var handle = new HttpClientHandler();
                var client = new HttpClient(handle);
                // GET requests can have "Accept" headers
                //client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");
                client.DefaultRequestHeaders.Add("Accept-Encoding","gzip,deflate,sdch");
                client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.8");
                client.DefaultRequestHeaders.Add("Accept-Charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.3");
                //client.DefaultRequestHeaders.Add("Accept-Encoding","gzip,deflate,sdch");

                var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);
                // Content-Type HTTP header should be set only for PUT and POST requests.
                //request.Content = new StringContent(postJson, Encoding.UTF8,
                //    "application/json");

                var response = client.SendAsync(request).Result;
                resStr = await response.Content.ReadAsStringAsync();

                

                //resObj = JsonNet.Deserialize<ApiRspModel<T>>(resStr);
                
            }
            catch (Exception e)
            {
                //LogHelper.Error(e, string.Format("BestSignApi.HttpGet Error! url:{0}, result:{1}"
                //    , fullUrl, resStr));
            }

            return resObj;
        }
    }
}
