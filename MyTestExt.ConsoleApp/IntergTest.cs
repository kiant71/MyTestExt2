using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTestExt.ConsoleApp
{
    public class IntergTest
    {
        public void Do()
        {
            
            //int aa = 3147483648;

            //long a1 = 12580L;
            //int a2 = int.Parse(a1.ToString());
            //Console.Write(a2);

            decimal a5 = (decimal) 123117.890902;
            decimal a6 = (decimal) 4418.004500;
            decimal a7 = (decimal) 10009.000000;
            decimal a8 = (decimal)19;


            var str1 = ToForamt(a5);
            var str2 = ToForamt(a6);
            var str3 = ToForamt(a7);
            var str4 = ToForamt(a8);

            //var str1 = a5.ToString();
            //var str2 = a6.ToString();
            //var str3 = a7.ToString();

            //var a51 = Math.Ceiling(a5);
            //if (a5 == a51)
            //{
            //    var a511 = (long) a51;
            //}

            //var a61 = Math.Ceiling(a6);
            //if (a6 == a61)
            //{
            //    var a611 = (long)a61;
            //}

            //var a71 = Math.Ceiling(a7);
            //if (a7 == a71)
            //{
            //    var a711 = (long)a71;
            //}
        }

        public static string ToForamt(decimal num, short length = -1)
        {
            var len = 0;
            for (len = 0; len < 7; len++)
            {
                var val1 = num*(decimal) Math.Pow(10, len);
                var val2 = Math.Ceiling(num*(decimal) Math.Pow(10, len));
                if (val1 == val2)
                    break;    // 获取小数点后有效位数
            }

            // 输出格式
            var format = "#0";
            if (len > 0)
            {
                format = "#0.";
                for (var i = 0; i < len; i++)
                {
                    format += "#";
                }

                return num.ToString(format);
            }
            
            return num.ToString();
        }

    }
}
