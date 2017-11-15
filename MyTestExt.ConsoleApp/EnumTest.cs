using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTestExt.ConsoleApp
{
    public enum ExecuteType
    {
        Query = 1,
        MultiQuery = 2,
        Modify = 3,
    }

    public class EnumTest
    {
        public static void DoTest()
        {
            // 传入默认0，或者其他不再范围的值
            var a1 = (ExecuteType) 0;
            var a2 = (ExecuteType)99;


            // Name相关
            ExecuteType eType = ExecuteType.Modify;
            Console.WriteLine(eType.ToString());    // "Modify"

            string objStr = Enum.GetName(typeof(ExecuteType), eType);
            Console.WriteLine(objStr);              // "Modify"

            string[] arrStr = Enum.GetNames(typeof(ExecuteType));
            Console.WriteLine(arrStr);              // "Query", "MultiQuery", "Modify"


            //对象转换
            ExecuteType atype2 = (ExecuteType)1;    // ExecuteType.Query
            int cc = (short)ExecuteType.Modify;      // 1


            // Value相关
            Console.WriteLine(eType);

            Array arrValue = Enum.GetValues(typeof(ExecuteType));
            Console.WriteLine(arrValue);            // arrValue.GetValue(0):"Query", "MultiQuery", "Modify"


            // 根据值，取名称
            int i = 3;
            string name = Enum.Parse(typeof(ExecuteType), i.ToString()).ToString();
            Console.WriteLine(name);                // "Modify"

            // 根据名称，取值
            string name2 = "Modify";
            int j = Convert.ToInt32(Enum.Parse(typeof(ExecuteType), name2));
            Console.WriteLine(j);                   // 3

        }
    }
}
