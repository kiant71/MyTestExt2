using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyTestExt.ConsoleApp
{
    public class WebClientTest
    {
        public static void Do()
        {
            var url = "";
            var destFile = @"D:\0.Work\TestZip\111.zip";
            try
            {
                using (var webClient = new WebClient())
                {
                    webClient.DownloadFile(url, destFile);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }


        #region HttpWebRequest异步GET

        public static void AsyncGetWithWebRequest(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.BeginGetResponse(new AsyncCallback(ReadCallback), request);
        }

        private static void ReadCallback(IAsyncResult asynchronousResult)
        {
            var request = (HttpWebRequest)asynchronousResult.AsyncState;
            var response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var resultString = streamReader.ReadToEnd();
                Console.WriteLine(resultString);
            }
        }

        #endregion


        #region WebClient异步GET

        public static void AsyncGetWithWebClient(string url)
        {
            var webClient = new WebClient();

            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            webClient.DownloadStringAsync(new Uri(url));
        }

        private static void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            //Console.WriteLine(e.Cancelled);
            Console.WriteLine(e.Error != null ? "WebClient异步GET发生错误！" : e.Result);
        }

        #endregion


        public static async Task<string> AsyncGetWithWebClient2(string url)
        {
            Console.WriteLine("Code Index 1: in first async");

            var webClient = new HttpClient();

            //Task<string> queue = DownloadStringAsync(webClient, url);

            Task<string> result = webClient.GetStringAsync(url);

            DoOtherThing();

            string aa = await result;

            //string resultAsync = await queue;
            Console.WriteLine("Code Index 5: await result");

            return aa;
        }

        private static void DoOtherThing()
        {
            Console.WriteLine("Code Index 4: do otherThings");
        }

        private static async Task<string> DownloadStringAsync(HttpClient webClient, string url)
        {
            Console.WriteLine("Code Index 2: in first async DownloadString");

            //var queue = webClient.DownloadString(url);
            Task<string> result = webClient.GetStringAsync(url);

            Console.WriteLine("Code Index 3: after async DownloadString");

            return result.Result;

            //string objValue = await queue;
            //return objValue;
        }



        #region WebClient的OpenReadAsync测试

        public static void TestGetWebResponseAsync(string url)
        {
            var webClient = new WebClient();
            webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(webClient_OpenReadCompleted);
            webClient.OpenReadAsync(new Uri(url));
        }

        private static void webClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                var streamReader = new StreamReader(e.Result);
                var result = streamReader.ReadToEnd();
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine("执行WebClient的OpenReadAsync出错：" + e.Error);
            }
        }

        #endregion

        public void Main2()
        {
            //AsyncGetWithWebRequest("http://baidu.com");
            //Console.WriteLine("hello");

            //AsyncGetWithWebClient("http://baidu.com");
            //Console.WriteLine("world");

            Task<string> aa = AsyncGetWithWebClient2("http://baidu.com");
            Console.WriteLine("Code Index 6: end AsyncGetWithWebClient2");
            Console.WriteLine("Code Index 7: result output" + aa.Result);

            TestGetWebResponseAsync("http://baidu.com");
            Console.WriteLine("jxqlovejava");

            Console.Read();
        }

    }
}
