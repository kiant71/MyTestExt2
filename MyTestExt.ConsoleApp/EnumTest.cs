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

        BBB = 7,
        AAA = 5,

        CCC = 4,
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

            
            Array arrValues = Enum.GetValues(typeof(ExecuteType));
            Console.WriteLine(arrValues);            // arrValue.GetValue(0):"Query", "MultiQuery", "Modify"

            var ulist = new List<int>();
            var arrNames = Enum.GetNames(typeof(ExecuteType));
            foreach (var names in arrNames)
            {
                int u = Convert.ToInt32(Enum.Parse(typeof(ExecuteType), names));
                ulist.Add(u);
            }




            foreach (var names in arrNames)
            {
                switch ((ExecuteType)Enum.Parse(typeof(ExecuteType), names))
                {
                    case ExecuteType.AAA:
                        break;
                }
            }




            // 根据值，取名称
            int i = 3;
            string name = Enum.Parse(typeof(ExecuteType), i.ToString()).ToString();
            Console.WriteLine(name);                // "Modify"

            // 根据名称，取值
            string name2 = "Modify";
            int j = Convert.ToInt32(Enum.Parse(typeof(ExecuteType), name2));
            Console.WriteLine(j);                   // 3



            // 按枚举值排序（GetNames出来是值对象的有序队列）
            var sortNames = Enum.GetNames(typeof(LookICItemSourceType2));
            foreach (var name1 in sortNames)
            {
                var sourceType = (LookICItemSourceType2)Enum.Parse(typeof(LookICItemSourceType2), name1);
                var sourceValue = (int)sourceType;
                Console.WriteLine(sourceValue.ToString() + name1);
            }

        }
    }



    public enum LookICItemSourceType2
    {
        None = 0,


        #region 我在当前公司0、我在别的公司0

        /// <summary>
        /// 【我在当前公司0】                                                        ；直接显示公司名
        /// </summary>
        MyInCurrComp = 11,

        /// <summary>
        /// 【我在别的公司0】                                                        ；直接显示公司名 
        /// </summary>
        MyInElseComp = 12,

        #endregion


        #region 当前公司0的同事、别的公司0的同事 - 同事其他的公司1

        /// <summary>
        /// 【当前公司0的同事】-【同事其他的公司1】                                    ；同事
        /// </summary>
        InCurrCompCo_CoInOtComp = 21,

        /// <summary>
        /// 【别的公司0的同事】-【同事其他的公司1】                                    ；别的公司-同事
        /// </summary>
        InElseCompCo_CoInOtComp = 22,


        #endregion


        #region 我在当前公司0、我在别的公司0 - 我关注的公司1

        /// <summary>
        /// 【我在当前公司0】-【我关注的公司1】                                        ；关注公司-关注成员
        /// </summary>
        MyInCurrComp_MyCernComp = 31,

        /// <summary>
        /// 【我在别的公司0】-【我关注的公司1】                                        ；关注公司-关注成员
        /// </summary>
        MyInElseComp_MyCernComp = 32,


        #endregion


        #region 当前公司0的同事、别的公司0的同事 - 同事关注的公司1

        /// <summary>
        /// 【当前公司0的同事】-【同事关注的公司1】                                    ；同事
        /// </summary>
        InCurrCompCo_CoCernComp = 41,

        /// <summary>
        /// 【别的公司0的同事】-【同事关注的公司1】                                    ；别的公司-同事
        /// </summary>
        InElseCompCo_CoCernComp = 42,


        #endregion


        #region 我在当前公司0、我在别的公司0 - 我关注的公司1 - 其所有员工其他的公司2、其所有员工关注的公司2

        /// <summary>
        /// 【我在当前公司0】-【我关注的公司1】-【其所有员工其他的公司2】               ；(我)关注公司-关注成员
        /// </summary>
        MyInCurrComp_MyCernComp_AllUserInOtComp = 51,

        /// <summary>
        /// 【我在当前公司0】-【我关注的公司1】-【其所有员工关注的公司2】                ；(我)关注公司-关注成员
        /// </summary>
        MyInCurrComp_MyCernComp_AllUserCernComp = 52,

        /// <summary>
        /// 【我在别的公司0】-【我关注的公司1】-【其所有员工其他的公司2】                ；(我)关注公司-关注成员
        /// </summary>
        MyInElseComp_MyCernComp_AllUserInOtComp = 53,

        /// <summary>
        /// 【我在别的公司0】-【我关注的公司1】-【其所有员工关注的公司2】                ；(我)关注公司-关注成员
        /// </summary>
        MyInElseComp_MyCernComp_AllUserCernComp = 54,

        #endregion


        #region 当前公司0的同事、别的公司0的同事 - 其他的公司1、关注的公司1 - 其所有员工其他的公司2、其所有员工关注的公司2

        /// <summary>
        /// 【当前公司0的同事】-【同事其他的公司1】-【其所有员工其他的公司2】             ；同事
        /// </summary>
        InCurrCompCo_CoInOtComp_AllUserInOtComp = 61,

        /// <summary>
        /// 【当前公司0的同事】-【同事其他的公司1】-【其所有员工关注的公司2】             ；同事
        /// </summary>
        InCurrCompCo_CoInOtComp_AllUserCernComp = 62,

        /// <summary>
        /// 【当前公司0的同事】-【同事关注的公司1】-【其所有员工其他的公司2】             ；同事
        /// </summary>
        InCurrCompCo_CoCernComp_AllUserInOtComp = 63,

        /// <summary>
        /// 【当前公司0的同事】-【同事关注的公司1】-【其所有员工关注的公司2】             ；同事
        /// </summary>
        InCurrCompCo_CoCernComp_AllUserCernComp = 64,



        /// <summary>
        /// 【别的公司0的同事】-【同事其他的公司1】-【其所有员工其他的公司2】             ；别的公司-同事
        /// </summary>
        InElseCompCo_CoInOtComp_AllUserInOtComp = 65,

        /// <summary>
        /// 【别的公司0的同事】-【同事其他的公司1】-【其所有员工关注的公司】              ；别的公司-同事
        /// </summary>
        InElseCompCo_CoInOtComp_AllUserCernComp = 66,

        /// <summary>
        /// 【别的公司0的同事】-【同事关注的公司1】-【其所有员工其他的公司2】             ；别的公司-同事
        /// </summary>
        InElseCompCo_CoCernComp_AllUserInOtComp = 67,

        /// <summary>
        /// 【别的公司0的同事】-【同事关注的公司1】-【其所有员工关注的公司2】             ；别的公司-同事
        /// </summary>
        InElseCompCo_CoCernComp_AllUserCernComp = 68,


        #endregion

    }

}
