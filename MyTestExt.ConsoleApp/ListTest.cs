using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication1.Util;

namespace MyTestExt.ConsoleApp
{
    public class ListTest
    {

        public void Do()
        {
            //var aa = new List<int> {3, 5, 4};

            //aa = aa ?? new List<int>();

            //aa.Sort((x, y) => x - y);

            //var bb = OtherCall();
            //bb = bb ?? new List<DateTime>();

            //bb.Sort((x, y) => x.CompareTo(y));


            //var aa = new List<int> { 2, 3, 1 };
            //var bb = aa.ToList();

            //bb.Add(4);

            //var a1 = bb.Take(-1).ToList();
            //var aa2 = bb.Take(0).ToList();

            //var aaCnt = aa.Count;
            //var bbCnt = bb.Count;

            //aa.Sort((x, y) => y - x);


            SortListTest();
        }


        public void SortListTest()
        {
            var sl = new SortedList(new Dictionary<long, string>());

            sl.Add(3, "ccccc");
            sl.Add(1, "aaa");
            sl.Add(2, "bbb");

            var a1 = JsonParse.Serialize(sl);
            var sl2 = JsonParse.Deserialize<SortedList>(a1);



        }


        public List<DateTime> OtherCall()
        {
            var ret = new List<DateTime>();

            ret.Add(DateTime.Now.AddDays(-10));

            ret.Add(DateTime.Now.AddDays(-5));

            ret.Add(DateTime.Now.AddDays(-11));

            return ret;
        }

    }
}
