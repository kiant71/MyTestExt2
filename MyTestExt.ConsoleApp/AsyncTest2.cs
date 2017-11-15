using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyTestExt.ConsoleApp
{
    class AsyncTest2
    {
        public void TestMain()
        {
            Console.WriteLine("主线程测试开始：");

            /*第一种方式，开新线程【前台】，这样这是外部调用
             new TestAsync().TestMain();
             Console.WriteLine("主线程继续===========");
             * 也会等子线程完成后才关闭
             * */
            //Thread th = new Thread(ThMethod0);
            //th.Start();

            /*第二种方式，Task.Run 启动线程【后台】
             * 这种方式下在主线程中【不会等待】后台线程执行完毕
             */
            //ThMethod2(1);
            //ThMethod2(2);

            //第三种方式，内置异步，以事件注册方式【后台】
            //这种方式下在主线程中【不会等待】后台线程执行完毕
            ThMethod3("http://www.163.com");
            ThMethod3("http://www.facebook.com");
            ThMethod3("http://www.sina.com.cn");
            ThMethod3("http://www.google.com");


            //Thread.Sleep(1000);
            //Console.WriteLine("主线程测试结束。");
            //Console.Read();
        }

        void ThMethod0()
        {
            ThMethod(1);
        }

        void ThMethod(int x = 1)
        {
            Console.WriteLine("----异步执行开始..." + x);
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("----异步执行 {1} -- {0,-3}...", i, x);
                Thread.Sleep(1000);
            }
            Console.WriteLine("----异步执行完成。" + x);
        }

        void ThMethod2(int y)
        {
            Task task = Task.Run(() =>
            {
                ThMethod(1 * y);
            });

            Task task2 = Task.Run(() =>
            {
                ThMethod(2 * y);
            });
        }

        void ThMethod3(string uri)
        {
            Console.WriteLine("----异步下载：" + uri);
            System.Net.WebClient wc = new System.Net.WebClient();
            //wc.DownloadDataCompleted += wc_DownloadDataCompleted;
            wc.DownloadDataCompleted += (sender, e) => { wc_DownloadDataCompleted(sender, e); };
            wc.DownloadDataAsync(new Uri(uri));
        }

        private void wc_DownloadDataCompleted(object sender, System.Net.DownloadDataCompletedEventArgs e)
        {
            Console.WriteLine("----异步下载完成： " + e.Result.Length);
            Thread.Sleep(1000);
        }
    }
}
