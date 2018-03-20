using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTestExt.ConsoleApp
{
    public class SortTest
    {
        /// <summary>
        /// 冒泡排序
        /// </summary>
        public void Maopao()
        {
            int[] arr = { 1, 5, 2, 9, 0, 8, 7, 4 };

            bool flag = true;
            do
            {
                flag = false;

                for (int i = 0; i < arr.Length - 1; i++)
                {
                    if (arr[i] > arr[i + 1])
                    {
                        var temp = arr[i];
                        arr[i] = arr[i + 1];
                        arr[i + 1] = temp;

                        flag = true;
                    }
                }
            } while (flag);

            return;

        }


        //选择排序
        public void Xuanzhe()
        {
            int[] arr = { 1, 5, 2, 9, 0, 8, 7, 4 };

            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = i + 1; j < arr.Length; j++)
                {
                    if (arr[i] > arr[j])
                    {
                        var temp = arr[i];
                        arr[i] = arr[j];
                        arr[j] = temp;
                    }
                }
            }


            return;

        }



        public int QuickSortIn(int[] arr, int i, int j)
        {
            int pivot = arr[i]; //将转入数组的第一个值作为描点
            while (i < j)
            {
                while (i < j && arr[j] >= pivot) j--;   //从右开始，寻找小于描点的数，直到找到为止
                arr[i] = arr[j];                        //右边小于描点的数，并与左边的第一个数（描点）进行互换

                while (i < j && arr[i] > pivot) i++;    //从左边开始，寻找大于描点的数，直到找到为止
                arr[j] = arr[i];                        //左边大于描点的数，与右边之前找到的互换
            }//当 i==j时，退出循环

            arr[i] = pivot;     //将原来描点值返回 i==j 的定位轴
            return i;           //返回定位轴
        }

        public void QuickSort(int[] arr, int i, int j)
        {
            if (i >= j) return;

            int pivotIndex = QuickSortIn(arr, i, j);
            QuickSort(arr, i, pivotIndex - 1);    //对描点左边进行快速排序
            QuickSort(arr, pivotIndex + 1, j);    //对描点右边进行快速排序
        }


        public void QuickSort2()
        {
            int[] arr = { 1, 5, 2, 9, 0, 8, 7, 4 };

            QuickSort(arr, 0, arr.Length - 1);
        }


        public void Sort<T>(IList<T> arr, Func<T, T, bool> comparison)
        {
            var flag = true;
            do
            {
                flag = false;
                for (int i = 0; i < arr.Count - 1; i++)
                {
                    if (comparison(arr[i], arr[i + 1]))
                    {
                        T temp = arr[i];
                        arr[i] = arr[i + 1];
                        arr[i + 1] = temp;
                        flag = true;
                    }
                }
            } while (flag);
        }


        public void Test2()
        {
            string mid = ", middle part, ";
            Func<string, string> lambda = param =>
            {
                param += mid;
                param += " and this was added to the string.";
                return param;
            };

            Console.WriteLine(lambda("Start of string"));
            //queue: Start of string, middle part,  and this was added to the string.

            Func<double, double, double> twoParams = (x, y) => x * y;
            Console.WriteLine(twoParams(4, 2));
        }


        public int Fibo(int m)
        {
            if (m < 1)
                return 0;
            else if (m == 1)
                return 1;
            else
                return Fibo(m - 1) + Fibo(m - 2);
        }
    }

    class Employee
    {
        //定义了 FieldName、Salary 属性

        //类内自定义了比较方法
        //public bool CompareSalary(Employee e1, Employee e2)
        //{
        //    return e1.Salary > e2.Salary;
        //}
    }


    public class Singleton
    {
        private static Singleton instance = null;

        public static Singleton GetInstance()
        {
            if (instance == null)
                instance = new Singleton();

            return instance;
        }
    }
}
