using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication1.Util;
using MyTestExt.Utils.Json;

namespace MyTestExt.ConsoleApp
{
    public class InheriteTest
    {

        public static void Do()
        {
            // error
            //var a = new A {A1 = "1", A2 = 1, A3 = new List<long> {1, 2, 3}};
            //var aa = (AA) a;

            //AA a = new A { A1 = "1", A2 = 1, A3 = new List<long> { 1, 2, 3 } };

            A a = new AAA {A1 = "a1", A2 = 2, A3 = new List<long> {1, 2, 3},
                A10 = "A10", A100 = 100};
            var str = JsonNet.Serialize(a);
        }


        public class A
        {
            public string A1 { get; set; }

            public int A2 { get; set; }

            public List<long> A3 { get; set; }
        }

        public class AA : A
        {
            public string A10 { get; set; }
        }

        public class AAA : AA
        {
            public int A100 { get; set; }
        }
    }
}
