using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


            var aa = new List<int> { 2, 3, 1 };
            var bb = aa.ToList();

            bb.Add(4);

            var aaCnt = aa.Count;
            var bbCnt = bb.Count;

            aa.Sort((x, y) => y - x);

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
