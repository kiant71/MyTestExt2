using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication1.Util;

namespace MyTestExt.ConsoleApp
{

    public class PhoneAlertTest
    {
        public static void DoTest()
        {
            var obj = new { SmsType = 1, Msg = "测试文本", MobilePhone = new string[] { "18565779874" } };
            var strJson = JsonParse.Serialize(obj);
            var strAES = AesTest.Encrypt(strJson);

            var uri = @"http://cloud2.sap360.com.cn:36010/api/Register/SMSNotify";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
            HttpContent content = new StringContent(JsonParse.Serialize(strAES));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Content = content;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.SendAsync(request).Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
            }
        }
    }
}
