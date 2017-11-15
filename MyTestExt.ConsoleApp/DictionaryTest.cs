using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace MyTestExt.ConsoleApp
{
    public class DictionaryTest
    {
        public static Dictionary<IPEndPoint, MyObj2> trackerPools = new Dictionary<IPEndPoint, MyObj2>();

        public static void Test()
        {
            TestForech();
        }

        private static void Do()
        {
            var a1 = new IPEndPoint(IPAddress.Parse("192.168.1.14"), 23000);
            var a2 = new IPEndPoint(IPAddress.Parse("192.168.1.14"), 23000);
            if (Equals(a1, a2))
                Console.WriteLine(" a1 equals a2 ");

            trackerPools.Add(a1, new MyObj2(){ K1 = "11"});
            if (trackerPools.ContainsKey(a2))
                Console.WriteLine(" list(a1) containskey a2 ");


            var a3 = new IPEndPoint(IPAddress.Parse("192.168.1.23"), 23000);
            trackerPools.Add(a1, new MyObj2() {K1 = "23"});

            var a4 = new IPEndPoint(IPAddress.Parse("192.168.1.24"), 23000);
            trackerPools.Add(a1, new MyObj2() { K1 = "24" });


            var list2 = trackerPools.Values.ToArray();


        }

        private static void TestForech()
        {
            var lists = new List<string>();
            lists.Add("ccccc");
            lists.Add("fffff");
            lists.Add("eeeee");
            lists.Add("aaaaa");
            foreach (var item in lists)
            {
                Console.WriteLine(item);   // 按输入顺序输出
            }


            var dicts = new Dictionary<string, object>();
            dicts.Add("ccccc", "ccccc");
            dicts.Add("fffff", "fffff");
            dicts.Add("eeeee", "eeeee");
            dicts.Add("aaaaa", "aaaaa");
            foreach (var item in dicts)
            {
                Console.WriteLine(item.Key);   // 按输入顺序输出
            }


            var sortDicts = new SortedDictionary<string, object>();
            sortDicts.Add("ccccc", "ccccc");
            sortDicts.Add("fffff", "fffff");
            sortDicts.Add("eeeee", "eeeee");
            sortDicts.Add("aaaaa", "aaaaa");
            foreach (var item in sortDicts)
            {
                Console.WriteLine(item.Key);   // 按【主键】顺序输出
            }

        }
    }

    public class MyObj2
    {
        public string K1;

        public string K2;
    }
}
