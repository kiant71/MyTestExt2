using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTestExt.ConsoleApp
{
    public class PrintEntityTest
    {
        public void Test()
        {
            var obj = new PrintEntityObj
            {
                AString = "adbadeg",
                AString_NULL = null,
                BLong = 239823489234,
                CInt = 343,
                DObject = new PrintEntityObj
                {
                    AString = "inner text1",
                    BLong = 999999
                },
                DObject_NULL = null
            };

            PrintProperty(obj, "BLong,");

            Console.Read();
        }

        private static void PrintProperty(PrintEntityObj obj, string strIgnoreProperty)
        {
            StringBuilder sb = new StringBuilder();

            //if (objValue == null) Console.WriteLine("objValue NULL");

            var listIgnore = strIgnoreProperty.Split(',').ToList<string>();
            foreach (System.Reflection.PropertyInfo p in obj.GetType().GetProperties())
            {
                if (listIgnore.Contains(p.Name)) continue;

                //Console.WriteLine("FieldName:{0} Value:{1}", p.FieldName, p.GetValue(objValue));
                sb.Append(string.Format("Name:{0} Value:{1}\n", p.Name, p.GetValue(obj)));

                //TODO.内置类的反射等下次做
                //if (p.PropertyType.IsValueType || p.PropertyType==typeof(string))
                //{
                //    Console.WriteLine("FieldName:{0} Value:{1}", p.FieldName, p.GetValue(objValue));
                //}

                //System.Reflection.Assembly assembly = new System.Reflection.Assembly();
                //var newObj = assembly.CreateInstance(p.GetValue(objValue))
            }

            sb.Append("\n");
            AppendStringToFile(string.Format(@"D:\work_project\log\{0}.txt", DateTime.Now.ToString("yyyyMMdd_hhmmss"))
                , sb.ToString());
        }


        public static void WriteStringToFile(string fileName, string contents)
        {
            StreamWriter sWriter = null;
            try
            {
                FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                sWriter = new StreamWriter(fileStream);
                sWriter.Write(contents);
            }
            finally
            {
                if (sWriter != null)
                {
                    sWriter.Close();
                }
            }
        }
        public static void AppendStringToFile(string fileName, string contents)
        {
            StreamWriter sWriter = null;
            try
            {
                FileStream fileStream = new FileStream(fileName, FileMode.Append, FileAccess.Write);
                sWriter = new StreamWriter(fileStream);
                sWriter.Write(contents);
            }
            finally
            {
                if (sWriter != null)
                {
                    sWriter.Close();
                }
            }
        }

    }


    public class PrintEntityObj
    {
        public string AString { get; set; }

        public string AString_NULL { get; set; }

        public long BLong { get; set; }

        public int CInt { get; set; }

        public int CInt_NULL { get; set; }

        public PrintEntityObj DObject { get; set; }

        public PrintEntityObj DObject_NULL { get; set; }

    }
}
