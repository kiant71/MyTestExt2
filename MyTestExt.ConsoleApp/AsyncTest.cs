using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyTestExt.ConsoleApp
{
    public class AsyncTest
    {
        public void Test()
        {
            Console.WriteLine("1.1111");

#pragma warning disable 4014
            TestAsycn1();
#pragma warning restore 4014

            Console.WriteLine("5.2222");
        }

        public async System.Threading.Tasks.Task TestAsycn1()
        {
            await System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                Console.WriteLine("2.in startNew");

                Thread.Sleep(1000);

                Console.WriteLine("3.out startNew");
            });

            Console.WriteLine("4.out AsyncMethod code");
        }





        public void Do()
        {
            Console.WriteLine("-------主线程启动-------");
            Task<string> task = GetLengthAsync();
            Console.WriteLine("Main方法做其他事情");
            Console.WriteLine("Task返回的值" + task.Result);
            Console.WriteLine("-------主线程结束-------");
        }

        static async Task<string> GetLengthAsync()
        {
            Console.WriteLine("GetLengthAsync Start");
            CustmorEntity task = await GetStringAsync();
            Console.WriteLine("GetLengthAsync End");
            return task.Str;
        }

        static Task<CustmorEntity> GetStringAsync()
        {
            return Task<string>.Run(() =>
            {
                Thread.Sleep(2000);
                return new CustmorEntity { Str = "finished" };
            });
        }
    }


    public class CustmorEntity
    {
        public string Str { get; set; }

        public long Long { get; set; }

    }
}
