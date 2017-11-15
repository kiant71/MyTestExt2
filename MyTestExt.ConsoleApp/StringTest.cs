﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTestExt.ConsoleApp
{
    public class StringTest
    {
        public static void Do()
        {
            //BulidSeq();

            var tmp0 = new List<int> {4, 6, 7, 888, 99};
            var str0 = string.Join(",", tmp0);



            string aabb = "aabb";
            aabb = aabb ?? "aacc";

            var ac = new ABC{ AB = "aabb", ABs = new List<string>{"a1", "a2", "a3"} };
            ac.ABCs = new List<ABItem>
            {
                new ABItem {Item1 = "1", Item2 = "1.1"},
                new ABItem {Item1 = "2", Item2 = "2.1"}
            };
            ac.ABCDs = ac.ABCs.ToList();
            ac.AB = ac.AB ?? "aacc";
            ac.ABs = ac.ABs ?? new List<string>();
            ac.ABCs = ac.ABCs ?? new List<ABItem>();
            ac.ABCDs = ac.ABCDs ?? new List<ABItem>();  // ??本身 赋值给本身后，导致数据异常？ 因为对象 set属性中有 Clear()，会导致值重置

            decimal a = 70000;
            decimal b = 6;
            var c = a / b;

            var a0 = c.ToString();
            var a1 = c.ToString("#.#####");
            var a2 = c.ToString("#0.#####");


            var str = "李芳-rose、Elaine、Sunny、范敏123456789";
            var str2 = GetStringByByteLength(str, 30);

            var str3 = "李芳-rose、Elaine、Sunny、范敏123456789";
            var str4 = GetStringByByteLength2(str3, 30);

            var str5 = "李芳-rose、Elaine、Sunny、范敏123456789";
            var str6 = GetStringByByteLength3(str5, 30);

            var vt = (decimal)Math.Pow(10, 5);
            var vx = c * vt;
            var temp = 0.5M;
            vx += temp;
            var abc = Math.Floor(vx) / vt;


            var bb = Math.Round(c, 5);
            var bb1 = Math.Round((decimal) 2.00000000001, 5);
            var bb2 = Math.Round((decimal)2.00000000000, 5);

            vt = (decimal)Math.Pow(10, 5);
            vx = (decimal)2.00000000001 * vt;
            temp = 0.5M;
            vx += temp;
            var bb3 = Math.Floor(vx) / vt;
        }

        // 字符串自增
        public static void BulidSeq()
        {
            var list = new List<string>();
            list.Add("12");
            list.Add("AB11");
            list.Add("ab13");
            list.Add("ab-13");
            list.Add("ab-13.21");
            list.Add("ab-18sdfsd21");
            list.Add("17");

            for (int i = 0; i < 200; i++)
            {
                var maxValue = 1L;
                foreach (var item in list)
                {
                    var regex = new System.Text.RegularExpressions.Regex(@"^\d+$");
                    var match = regex.Match(item);
                    if (match.Success)
                    {
                        var tmpValue = long.Parse(match.Value);
                        if (tmpValue >= maxValue)
                            maxValue = tmpValue + 1;
                    }
                }
                list.Add(maxValue.ToString());
            }

            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
            
        }



        public static string GetStringByByteLength(string str, int byteLength)
        {
            if (string.IsNullOrEmpty(str) || byteLength == 0)
                return str;

            var strBytes = System.Text.Encoding.Default.GetBytes(str);
            while (strBytes.Length > byteLength)
            {
                str = str.Substring(0, str.Length - 1);
                strBytes = System.Text.Encoding.Default.GetBytes(str);
            }

            return str;
        }

        public static string GetStringByByteLength2(string str, int byteLength)
        {
            if (string.IsNullOrEmpty(str) || byteLength == 0)
                return str;


            var strBytes = System.Text.Encoding.Unicode.GetBytes(str);
            while (strBytes.Length > byteLength)
            {
                str = str.Substring(0, str.Length - 1);
                strBytes = System.Text.Encoding.Unicode.GetBytes(str);
            }

            return str;
        }

        public static string GetStringByByteLength3(string str, int byteLength)
        {
            if (string.IsNullOrEmpty(str) || byteLength == 0)
                return str;

            var strBytes = System.Text.Encoding.UTF8.GetBytes(str);
            while (strBytes.Length > byteLength)
            {
                str = str.Substring(0, str.Length - 1);
                strBytes = System.Text.Encoding.UTF8.GetBytes(str);
            }

            return str;
        }

    }


    public class ABC
    {
        public string AB { get; set; }

        public List<string> ABs { get; set; }

        public List<ABItem> ABCs { get; set; }

        private List<ABItem> _ABCDs = new List<ABItem>();

        public List<ABItem> ABCDs
        {
            get { return _ABCDs; }
            set
            {
                _ABCDs.Clear();
                if (value != null)
                    _ABCDs = value;
            }
        }

    }

    public class ABItem
    {
        public string Item1 { get; set; }

        public string Item2 { get; set; }
    }


    public class AuotProPearty
    {
        public string Name { get; set; } = "自动属性";

    }
}
