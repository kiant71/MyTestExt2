using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication1.Util;
using MyTestExt.Utils.Json;

namespace MyTestExt.ConsoleApp
{
    public class DynamicTest
    {



        public static void Do()
        {

            DynamicModel a0 = null;
            if (a0?.AAA == "11")
            {

            }
            else
            {
            }

            var a1 = new DynamicModel {AAA = "aaa", BBB = 10, CCc = 8283, DDD = DateTime.Now};
            var a2 = new DynamicModel { AAA = "aa2", BBB = 20, CCc = 28283, DDD = DateTime.Now.AddDays(1) };
            var a3 = new DynamicModel { AAA = "aa3", BBB = 30, CCc = 38283, DDD = DateTime.Now.AddDays(2) };


            var list = new List<dynamic>();

            list.Add(new 
            {
                a1.AAA,
                a1.BBB,
                a2.CCc,
                a3.DDD,
            });
            list.Add(new
            {
                a3.AAA,
                a3.BBB,
                a1.CCc,
                a2.DDD,
            });
            list.Add(new
            {
                a2.AAA,
                a1.BBB,
                a3.CCc,
                a3.DDD,
            });


            var str = JsonNet.Serialize(list);

        }


        

    }


    public class DynamicModel
    {
        public string AAA = "";

        public int BBB;

        public long CCc;

        public DateTime DDD;
    }
}
