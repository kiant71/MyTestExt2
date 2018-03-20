using System;
using System.Collections.Generic;

namespace MyTestExt.ConsoleApp
{
    public class ForEachTest
    {
        public static void Do()
        {
            var list1 = new List<ForEachEntityItem>()
            {
                new ForEachEntityItem {Name = "aaa", Code = 1},
                new ForEachEntityItem {Name = "bbb", Code = 2},
                new ForEachEntityItem {Name = "ccc", Code = 3},
                new ForEachEntityItem {Name = "ddd", Code = 4}
            };

            list1.ForEach(c =>
            {
                c.Code = new Random().Next(20);
                System.Threading.Thread.Sleep(1000);
            });

        }


        public class ForEachEntityItem
        {
            public string Name { get; set; }

            public int Code { get; set; }
        }
    }
}
