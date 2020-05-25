using System;
using System.Linq;
using System.Text;
using ConsoleApplication1.Util;
using MyTestExt.Utils.Json;

namespace MyTestExt.ConsoleApp
{
    public class EnCodingTest
    {
        public static void Test()
        {
            //var str = "一二三四五六七八九十abcdefghij0123456789一二三四五六七八九十abcdefghij0123456789一二三四五六七八九十abcdefghij0123456789一二三四五六七八九十abcdefghij0123456789一二三四五六七八九十abcdefghij0123456789一二三四五六七八九十abcdefghij0123456789一二三四五六七八九十一二三四五六七八九十";
            //var str2 = str.ByteCeilling(80);

            //var str3 = "许志坚36创建分享:一二三四五六七八九十abcdefghij0123456789一二三四五六七八九十abcdefghij0123456789一二三四五";
            //var str3L = System.Text.Encoding.Default.GetBytes(str3).Length;
            //var str4 = str.ByteCeilling(160);

            Test1();
        }


        public static void Test1()
        {
            string msgText = "徐晓敏创建签到:广东省深圳市南山区西丽街道源兴科技大厦奇建贸易有限公司南山分公司南山城市展厅";
            int msgLen = System.Text.Encoding.UTF8.GetBytes(msgText).Length;

            string ptmString = "{\"S\":0,\"B\":1539,\"F\":0,\"C\":\"mycom\",\"T\":15,\"A\":\"签到\"}";
            int lenOther = System.Text.Encoding.UTF8.GetBytes(ptmString + "{\"alert\":\"\",\"sound\":\"sound.caf\"}").Length;
            int paramLen = 200 - lenOther;
            while (msgLen > paramLen)
            {
                msgText = msgText.Substring(0, msgText.Length - 1);
                msgLen = System.Text.Encoding.UTF8.GetBytes(msgText).Length;
            }
            var iosAlert = new { alert = msgText, sound = "sound.caf" };

            var aa = JsonNet.Serialize(iosAlert);
            //OpenImApi.CustmsgPush("systemuser", new string[] { "3fa4f83c0807485a89267e010081c71f" }.ToList(), msgText,
            //                                "{\"M\":\"徐晓敏创建签到:广东省深圳市南山区西丽街道源兴科技大厦奇建贸易有限公司南山分公司南山城市展厅\",\"S\":\"签到\",\"T\":15,\"D\":{\"S\":0,\"B\":1539,\"F\":0,\"C\":\"mycom\",\"T\":15,\"A\":\"签到\"},\"C\":\"2016-06-30T14:22:07.7441331+08:00\",\"ID\":445}",
            //                                aa,ptmString);
        }

        public static void Test2()
        {
            //根据操作系统决定 Encoding.Default=DBCSCodePageEncoding        BodyName="gb2312"
            Console.WriteLine(Encoding.Default.BodyName);
            Console.WriteLine(Encoding.GetEncoding("gb2312").BodyName); //返回指定编码    BodyName="gb2312"

            Encoding gb2312 = Encoding.GetEncoding("gb2312");
            Encoding utf8 = Encoding.UTF8;

            string str = "C# 里面文字是UTF8 码";

            //同一种字符串，在不同的编码方式下有不同的长度
            Console.WriteLine(str.Length);              //Length=14
            byte[] gb2312Bytes = gb2312.GetBytes(str);    //Length=20
            byte[] utf8Bytes = utf8.GetBytes(str);        //Length=26

            //同一种编码方式下转换（无损） 【string-> byte-> string】
            Console.WriteLine(gb2312.GetString(gb2312Bytes)); //"C# 里面文字是UTF8 码"  Length=14
            Console.WriteLine(utf8.GetString(utf8Bytes));     //"C# 里面文字是UTF8 码"  Length=14

            //不同编码方式下，由string 进行中转的话，会出错【gb2312-> utf8(string)-> gb2312】
            string utf8StringFromGb2312Bytes = utf8.GetString(gb2312Bytes);
            Console.WriteLine(utf8StringFromGb2312Bytes);             //"C# ����������UTF8 ��"  Length=20
            Console.WriteLine(gb2312.GetString(utf8.GetBytes(utf8StringFromGb2312Bytes)));
            //"C# 锟斤拷锟斤拷锟斤拷锟斤拷锟斤拷UTF8 锟斤拷"  Length=26

            //不同编码方式下，由byte 进行中转的话，字符串不变【gb2312-> utf8(byte)-> gb2312】
            byte[] byteGB2312ToUtf8 = Encoding.Convert(gb2312, utf8, gb2312Bytes);    //Length: 20 -> 26
            Console.WriteLine(gb2312.GetString(Encoding.Convert(utf8, gb2312, byteGB2312ToUtf8)));
            //"C# 里面文字是UTF8 码"  Length=14


            Console.WriteLine(utf8Bytes.Length);      //26

            //转换为gb2312
            //byte[] arrGB2312 =  
            Console.WriteLine(Encoding.GetEncoding("gb2312").GetString(utf8Bytes));   //C# 閲岄潰鏂囧瓧鏄疷TF8 鐮?     Length=17


            //Encoding.Convert()


            Test2();

        }

        public static void Test3()
        {
            Console.WriteLine(Encoding.Default.BodyName);
            Console.WriteLine(Encoding.GetEncoding("gb2312").BodyName); //返回指定编码    BodyName="gb2312"

            Encoding gb2312 = Encoding.GetEncoding("gb2312");
            Encoding utf8 = Encoding.UTF8;

            string str = "-ERR 登录失败，用户不存在，login failed ";

            //同一种字符串，在不同的编码方式下有不同的长度
            Console.WriteLine(str.Length);              //Length=14
            byte[] gb2312Bytes = gb2312.GetBytes(str);    //Length=20
            byte[] utf8Bytes = utf8.GetBytes(str);        //Length=26

            //同一种编码方式下转换（无损） 【string-> byte-> string】
            //Console.WriteLine(gb2312.GetString(gb2312Bytes)); //"C# 里面文字是UTF8 码"  Length=14
            //Console.WriteLine(utf8.GetString(utf8Bytes));     //"C# 里面文字是UTF8 码"  Length=14

            //不同编码方式下，由string 进行中转的话，会出错【gb2312-> utf8(string)-> gb2312】
            string utf8StringFromGb2312 = utf8.GetString(gb2312Bytes);
            Console.WriteLine(utf8StringFromGb2312);             //"-ERR ��¼ʧ�ܣ��û������ڣ�login failed "
            Console.WriteLine(gb2312.GetString(utf8.GetBytes(utf8StringFromGb2312)));
            //"-ERR 锟斤拷录失锟杰ｏ拷锟矫伙拷锟斤拷锟斤拷锟节ｏ拷login failed "  



            Console.WriteLine(gb2312.GetString(Encoding.Convert(utf8, gb2312, (utf8.GetBytes(utf8StringFromGb2312)))));
            //-ERR ?????????????????login failed 

            byte[] utf8BytesFromUnknow = utf8.GetBytes(utf8StringFromGb2312);  //"-ERR ��¼ʧ�ܣ��û������ڣ�login failed "
            byte[] gb2312BytesFromUnknowConvert = Encoding.Convert(utf8, gb2312, utf8BytesFromUnknow);
            char[] gb2312Chars = new char[utf8.GetCharCount(gb2312BytesFromUnknowConvert, 0, gb2312BytesFromUnknowConvert.Length)];
            utf8.GetChars(gb2312BytesFromUnknowConvert, 0, gb2312BytesFromUnknowConvert.Length, gb2312Chars, 0);
            string newString = string.Empty;
            newString = new string(gb2312Chars);
            //-ERR ?????????????????login failed 




            //不同编码方式下，由byte 进行中转的话，字符串不变【gb2312-> utf8(byte)-> gb2312】
            byte[] byteGB2312ToUtf8 = Encoding.Convert(gb2312, utf8, gb2312Bytes);    //Length: 20 -> 26
            Console.WriteLine(gb2312.GetString(Encoding.Convert(utf8, gb2312, byteGB2312ToUtf8)));
            //"C# 里面文字是UTF8 码"  Length=14


            Console.WriteLine(utf8Bytes.Length);      //26

            //转换为gb2312
            //byte[] arrGB2312 =  
            Console.WriteLine(Encoding.GetEncoding("gb2312").GetString(utf8Bytes));   //C# 閲岄潰鏂囧瓧鏄疷TF8 鐮?     Length=17


            //Encoding.Convert()
        }
    }


    public static class StringExtend
    {
        public static string ByteCeilling(this string str, int length)
        {
            while (System.Text.Encoding.Default.GetBytes(str).Length > length && !string.IsNullOrEmpty(str))
            {
                str = str.Substring(0, str.Length - 1);
            }
            return str;
        }
    }
}
