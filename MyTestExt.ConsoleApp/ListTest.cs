using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
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
