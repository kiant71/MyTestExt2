using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MyTestExt.ConsoleApp
{
    public class RequestTest
    {
        public static void Test()
        {

            //for (int i = 0; i < 10000; i++)
            //{
            //    var request = new HttpRequestMessage(HttpMethod.Get, "http://in.sap360.com.cn:559/group1/M00/00/0E/wKgBuFkK8iSEaMObAAAAACyvK6I050_thu100x100_40.png");
            //    var httpClient = new HttpClient();
            //    var response = httpClient.SendAsync(request).Result;
            //    byte[] result;
            //    if (response.StatusCode == HttpStatusCode.OK)
            //    {
            //        result = response.Content.ReadAsByteArrayAsync().Result;
            //        Console.WriteLine(i);
            //    }
            //    else
            //    {
            //        var str = response.Content.ReadAsStringAsync().Result;
            //        Console.WriteLine(str);
            //    }
            //}






            Get();


            //Post();
        }

        private static void Get()
        {
            //var request = new HttpRequestMessage(HttpMethod.Get, "http://audio.xmcdn.com/group27/M08/03/FC/wKgJR1j43xGQ238fARJi3vXRenA342.m4a");
            //var request = new HttpRequestMessage(HttpMethod.Get, "http://192.168.1.184:8080/group1/M00/03/FC/wKgJL1hwQa3AXqIVA7uXOCCf3Sw370.m4a");
            var request = new HttpRequestMessage(HttpMethod.Get,
                "http://in.sap360.com.cn:564");
            //request.Headers.Add("Range", "bytes=0-1048575");
            //request.Headers.Add("Range", "bytes=57500559-62625591");
            //request.Headers.Add("Nonce", "4tgggergigwow323t23t");
            //request.Headers.Add("CurTime", "1443592222");
            //request.Headers.Add("CheckSum", "9e9db3b6c9abb2e1962cf3e6f7316fcc55583f86");
            //var content = new StringContent("accid=zhangsan&name=zhangsan");
            //content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            //request.Content = content;

            var httpClient = new HttpClient();
            var response = httpClient.SendAsync(request).Result;
            string result;
            if (response.StatusCode == HttpStatusCode.OK)
                result = response.Content.ReadAsStringAsync().Result;
            else
                result = response.Content.ReadAsStringAsync().Result;

            if (result.IndexOf("Welcome to ASP.NET Web API!") > 0)
            {

            }

        }

        private static void Post()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "http://api.netease.im/nimserver/user/create.action");
            request.Headers.Add("AppKey", "go9dnk49bkd9jd9vmel1kglw0803mgq3");
            request.Headers.Add("Nonce", "4tgggergigwow323t23t");
            request.Headers.Add("CurTime", "1443592222");
            request.Headers.Add("CheckSum", "9e9db3b6c9abb2e1962cf3e6f7316fcc55583f86");
            var content = new StringContent("accid=zhangsan&name=zhangsan");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            request.Content = content;

            var httpClient = new HttpClient();
            var response = httpClient.SendAsync(request).Result;
            string result;
            if (response.StatusCode == HttpStatusCode.OK)
                result = response.Content.ReadAsStringAsync().Result;
            else
                result = response.Content.ReadAsStringAsync().Result;
        }
    }
}
