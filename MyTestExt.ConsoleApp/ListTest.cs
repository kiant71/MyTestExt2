using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication1.Util;
using MyTestExt.Utils.Json;

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


            //SortListTest();

            ToListTest();
        }


        public void SortListTest()
        {
            var sl = new SortedList(new Dictionary<long, string>());

            sl.Add(3, "ccccc");
            sl.Add(1, "aaa");
            sl.Add(2, "bbb");

            var a1 = JsonNet.Serialize(sl);
            var sl2 = JsonNet.Deserialize<SortedList>(a1);



        }


        public List<DateTime> OtherCall()
        {
            var ret = new List<DateTime>();

            ret.Add(DateTime.Now.AddDays(-10));

            ret.Add(DateTime.Now.AddDays(-5));

            ret.Add(DateTime.Now.AddDays(-11));

            return ret;
        }

        public void ToListTest()
        {
            var list1 = new List<ABModel>
            {
                new ABModel{A = "1A", B = "1B"},
                new ABModel{A = "2A", B = "2B"},
                new ABModel{A = "3A", B = "3B"},
            };

            var list2 = list1.ToList();

            var flag1 = list1 == list2;
            var flag10 = list1.Equals(list2);

            var flag2 = list1[0] == list2[0];
            var flag20 = list1[0].Equals(list2[0]);

            list1[0].A = "1a";

            var aa = list1[0].A;  // "1a"
            var bb = list2[0].A;  // "1a"
        }


        #region MyRegion

        public class ABModel
        {
            public string A { get; set; }

            public string B { get; set; }
        }

        #endregion
    }

    public static class DataTableExtends
    {
        /// <summary>
        /// List<Entity>转化为DataTable
        /// </summary>
        public static DataTable ToDataTable<T>(this IEnumerable<T> varlist)
        {
            var ret = new DataTable();
            if (varlist == null) return ret;

            PropertyInfo[] arrProps = null;
            foreach (T rec in varlist)
            {
                if (arrProps == null)
                {
                    // 在第一次时，使用反射获取列表对象的字段变量名，用于创建表的列名
                    arrProps = ((Type)rec.GetType()).GetProperties();
                    foreach (var pi in arrProps)
                    {
                        var colType = pi.PropertyType;
                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }
                        ret.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                var dr = ret.NewRow();
                foreach (var pi in arrProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                        (rec, null);
                }
                ret.Rows.Add(dr);
            }
            return ret;
        }
    }


}
