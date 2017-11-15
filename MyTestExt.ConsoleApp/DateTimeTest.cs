using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTestExt.ConsoleApp
{
    public class DateTimeTest
    {
        public static void Test()
        {
            var strFormat = "yyyy年MM月dd日 HH点mm分";
            var dt1 = new DateTime(2016, 1, 2, 3, 4, 5).ToString(strFormat);
            var dt2 = new DateTime(2016, 10, 11, 19, 10, 11).ToString(strFormat); ;


            //var date1 = DateTime.Now.AddDays(-1);
            //var date2 = date1;
            //var date3 = DateTime.Now.AddDays(1);

            //date1 = date3;

            //Console.WriteLine(date1.ToString("yyyy-MM-dd MM:HH:ss"));
            //Console.WriteLine(date2.ToString("yyyy-MM-dd MM:HH:ss"));
            //Console.WriteLine(date3.ToString("yyyy-MM-dd MM:HH:ss"));


            TestUtc();


            DateTime statBeginDate = new DateTime(2017,1,1);
            var aa = DateTime.Parse(statBeginDate.AddMonths(0 - ((statBeginDate.Month - 1) % 3)).ToString("yyyy-MM-01"));
            var bb = aa.AddMonths(3).AddSeconds(-1);

            //DateTime StatBeginDate = DateTime.Parse("2015-02-15")
            //        , StatEndDate = DateTime.Parse("2016-02-28");

            //Console.WriteLine();
            //Console.WriteLine("last year");
            //Console.WriteLine(DateTime.Parse(StatBeginDate.ToString("yyyy-01-01")).AddYears(-1));
            //Console.WriteLine(DateTime.Parse(StatBeginDate.ToString("yyyy-01-01")).AddSeconds(-1));

            //Console.WriteLine();
            //Console.WriteLine("last quor");
            //Console.WriteLine(DateTime.Parse(StatBeginDate.AddMonths(-3 - ((StatBeginDate.Month - 1) % 3)).ToString("yyyy-MM-01")));
            //Console.WriteLine(DateTime.Parse(StatBeginDate.AddMonths(0 - ((StatBeginDate.Month - 1) % 3)).ToString("yyyy-MM-01")).AddSeconds(-1));

            //Console.WriteLine();
            //Console.WriteLine("last month");
            //Console.WriteLine(DateTime.Parse(StatBeginDate.ToString("yyyy-MM-01")).AddMonths(-1));
            //Console.WriteLine(DateTime.Parse(StatBeginDate.ToString("yyyy-MM-01")).AddSeconds(-1));

            //Console.WriteLine();
            //Console.WriteLine("last week 周一为起始");
            //var dayOfWeed = (StatBeginDate.DayOfWeek== DayOfWeek.Sunday) ? 6 : Convert.ToInt16(StatBeginDate.DayOfWeek)-1;
            //Console.WriteLine(StatBeginDate.AddDays(-dayOfWeed).Date.AddDays(-7));
            //Console.WriteLine(StatBeginDate.AddDays(-dayOfWeed).Date.AddSeconds(-1));

            //Console.WriteLine();
            //Console.WriteLine("last day");
            //Console.WriteLine(StatBeginDate.Date.AddDays(-1));
            //Console.WriteLine(StatBeginDate.Date.AddSeconds(-1));              


            Console.Read();
        }

        public static void Test2()
        {
            var startDate = new DateTime(2016, 1, 17).Date;
            var endDate = new DateTime(2016, 1, 17).Date;


            // 工作周以周一为工作开始点，以周日为结束点（调整开始结束时间（开始时间调整为当周周一，结束时间调整为当周周日））
            // 比如范围选 13日(周日)--14日(周一)，则 13日一天为第一周，14日一天为第二周，共 2周 

            // 开始时间调整为当周周一
            if (startDate.DayOfWeek == DayOfWeek.Sunday) // DayOfWeek.Sunday ==0
                startDate = startDate.AddDays(-6);
            else
                startDate = startDate.AddDays(-((int) startDate.DayOfWeek - 1));
            // 结束时间调整为当周周日
            if (endDate.DayOfWeek != DayOfWeek.Sunday)
                endDate.AddDays((7 - (int) endDate.DayOfWeek));

            var days = (endDate - startDate).TotalDays + 1;

            var DateNum = (int) Math.Ceiling(days/7);


            Console.Read();
        }

        public static void Test3()
        {
            var startDate = new DateTime(2016, 1, 6, 3, 4, 5);
            var endDate = new DateTime(2017, 8, 5, 1, 4, 5);


            var days = (int) ((endDate.Date - startDate.Date).TotalDays + 1);
            var weeks = days/7;
            if (days%7 > 0)
                weeks += 1;

            var months = (endDate.Year - startDate.Year)*12 + (endDate.Month - startDate.Month);
            if (endDate.Day >= startDate.Day)
                months += 1; // 如果天数大于是等于，则月份增加1
            var jidus = months/3;
            if (months%3 > 0)
                jidus += 1;
            var years = months/12;
            if (months%12 > 0)
                years += 1;




            Console.Read();
        }

        public static void Test4()
        {

            var arr = "17:05".Split(':'); //TODO.验证格式有效性
            var dt = new DateTime(2015, 1, 1, int.Parse(arr[0]), int.Parse(arr[1]), 0);

            var str = DateTime.Now.ToString("yyyyMMddHHmmss");
            var date = new DateTime(
                int.Parse(str.Substring(0, 4)),
                int.Parse(str.Substring(4, 2)),
                int.Parse(str.Substring(6, 2)),
                int.Parse(str.Substring(8, 2)),
                int.Parse(str.Substring(10, 2)),
                int.Parse(str.Substring(12, 2))
                );

            Console.Read();
        }


        public static void TestUtc()
        {
            var localTime = DateTime.Now;
            var gmtTime = localTime.ToUniversalTime();
            
            var a0 = DateTimeToUtcMs(localTime);
            var b0 = UtcMsToDateTime(a0);

            var a1 = ToUtc(localTime);
            var a2 = UtcToLocalTime(a1);

            var b1 = ToUtc(gmtTime);
            var b2 = UtcToUniversalTime(b1);


            Console.Write("本地时间是： +8北京时间：");
            Console.WriteLine(localTime);
            Console.Write("转换后UTC时间");
            Console.WriteLine(a1);
            Console.Write("再次转换回本地时间： +8北京时间：");
            Console.WriteLine(a2);
        }


        public static long ToUtc(DateTime dt)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            if (dt.Kind == DateTimeKind.Utc)
                return Convert.ToInt64((dt - start).TotalSeconds);
            else
                return Convert.ToInt64((dt.ToUniversalTime() - start).TotalSeconds);
        }

        public static long DateTimeToUtcMs(DateTime dateTime)
        {
            if (dateTime.CompareTo(DateTime.MinValue) <= 0)
                return 0;

            var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            if (dateTime.Kind == DateTimeKind.Utc)
                return Convert.ToInt64((dateTime - start).TotalMilliseconds);
            else
                return Convert.ToInt64((dateTime.ToUniversalTime() - start).TotalMilliseconds);
        }

        public static DateTime UtcMsToDateTime(long timestamp)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            if (timestamp == 0) return start.ToLocalTime();
            return start.AddMilliseconds(timestamp).ToLocalTime();
        }


        public static DateTime UtcToLocalTime(long utc)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return start.AddSeconds(utc).ToLocalTime();
        }

        public static DateTime UtcToUniversalTime(long utc)
        {
            var localTime = UtcToLocalTime(utc);
            var localTimeZone = System.TimeZone.CurrentTimeZone;
            var timeSpan = localTimeZone.GetUtcOffset(localTime);
            var greenwishTime = localTime - timeSpan;
            return greenwishTime;
        }


        public static DateTime LocalTime2GreenwishTime(DateTime localTime)
        {
            TimeZone localTimeZone = System.TimeZone.CurrentTimeZone;
            TimeSpan timeSpan = localTimeZone.GetUtcOffset(localTime);
            DateTime greenwishTime = localTime - timeSpan;
            return greenwishTime;
        }

        public static DateTime GreenwishTime2LocalTime(DateTime greenwishTime)
        {
            TimeZone localTimeZone = System.TimeZone.CurrentTimeZone;
            TimeSpan timeSpan = localTimeZone.GetUtcOffset(greenwishTime);
            DateTime lacalTime = greenwishTime + timeSpan;
            return lacalTime;
        }

    }
}