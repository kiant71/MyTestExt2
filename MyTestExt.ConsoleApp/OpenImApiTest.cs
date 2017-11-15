using System;
using System.Collections.Generic;
using System.Data;
using ConsoleApplication1.Util;
using System.Linq;
using MyTestExt.ConsoleApp.Dapper;
using System.Xml.Linq;
using Top.Api.Response;
using Top.Api.Util;

namespace MyTestExt.ConsoleApp
{
    public class OpenImApiTest
    {
        public static void Test()
        {
            // 测试
            //OpenImApi.AppKey = "23331917";
            //OpenImApi.AppSecret = "92bd2396c4ca5979e2fdee13e6c4081e";

            // 生产
            OpenImApi.AppKey = "23369251";
            OpenImApi.AppSecret = "1f5e81da6ea7ad3926f68eecdb4b0b0d";


            //DoTribeUser();

            //DoUtilTest();

            // 新接口
            //var ret1 = OpenImApi.TribeGetalltribes("76696f5b23094e52ba0ac237b21fb297", "0,1");

            //var ret2 = OpenImApi.TribeSetmembernick("76696f5b23094e52ba0ac237b21fb297", 1900554245, "76696f5b23094e52ba0ac237b21fb297", "许志坚45");
            //var ret3 = OpenImApi.TribeSetmembernick("76696f5b23094e52ba0ac237b21fb297", 1900555265, "76696f5b23094e52ba0ac237b21fb297", "许志坚45");

            // 创建用户
            //var flag = OpenImApi.UserAdd("cb9e73966d5a42feae6fde495cff1d40", "428338", "曾丽娜");
            //OpenImApi.UserAdd("56e7fa4b80a74cd6ad83f8477cdce0ae", "675362", "刘涛");
            //OpenImApi.UserAdd("20dc88d4a71b45a9b4acea7a056c7613", "555255", "钟鹤");
            //OpenImApi.UserAdd("16966a411ddf4fa1a64c819b42775011", "417868", "龙慧", "http://doc.sap360.com.cn:36004/group1/M00/01/1D/wKhkHlh4VraEdpSwAAAAAGaGuOE142.png");
            //OpenImApi.UserAdd("104a93a00cda4c31a4550b9cce90ac47", "188678", "陈妍甄");
            //OpenImApi.UserAdd("c9f8f356df6c4eb29172d79cf001b847", "366486", "梁富萍");
            //OpenImApi.UserAdd("aaaf20c5d9eb4026a579cc58feecd7eb", "876837", "黄江");


            // 获取用户

            //var obj1_1 = OpenImApi.UsersGet("fcc9df8938fc4c359a3e223cdca1349c,cb9e73966d5a42feae6fde495cff1d40,56e7fa4b80a74cd6ad83f8477cdce0ae,20dc88d4a71b45a9b4acea7a056c7613,16966a411ddf4fa1a64c819b42775011,104a93a00cda4c31a4550b9cce90ac47,c9f8f356df6c4eb29172d79cf001b847,aaaf20c5d9eb4026a579cc58feecd7eb");

            //var obj = OpenImApi.UserGet("68ee2ab55ae04752ad8e460218840f4e");


            // 消息发送
            //var obj2 = ImmsgPush("598c82c86aa14bea83a6b18a8afa5813", "42bb6181e1244751b28674b57898e05a", 0, "testtest", "");


            // 设置群昵称
            //var flag = TribeSetmembernick("fd1774b2c74441a08b2b16964ed3c1df", 1900556634,
            //    "fd1774b2c74441a08b2b16964ed3c1df", "韩兴军123");

            var a = 1;

            // 获取群资料
            var obj3_0 = OpenImApi.TribeGetTribeInfo("systemuser", 1901145260);
            //var flag_0 = OpenImApi.TribeModifytribeinfo("d45f1b0f9b1e4d69a70b0f800fbbf072", 1900541604, "中鼎恒生•鑫荣团队核心圈",
            //    obj3_0.Notice);
            //var obj3_0_0 = OpenImApi.TribeGetTribeInfo("d45f1b0f9b1e4d69a70b0f800fbbf072", 1900541604);

            //获取讨论组成员
            var obj3 = OpenImApi.TribeGetMembers("systemuser", 1901139039);

            // 解散群
            //var obj4 = OpenImApi.TribeDismiss(1900573125);  //4582ce26b65f4c99a5a9b2f2e07cb8d8

            // 退群
            //var obj5 = TribeQuit("d45f1b0f9b1e4d69a70b0f800fbbf072", 1900541376);

            a = 2;

            // 单人聊天记录
            //var obj6 = ChatlogsGet("ccd4131d57e14a02822a9df8cad3da1f", "c0cc01da7226483ab523324aa9fa42bb"
            //    , DateTime.Now.Date, DateTime.Now, 100);

            // 获取群消息
            //var obj9 = OpenImApi.TribelogsGet("1900888017", DateTime.Now.AddMonths(-1).AddDays(3), DateTime.Now);

            // 客服发送消息  “alex”
            //var obj7 = OpenImApi.ImmsgPush("思普达目标达成:目标服务员2号", "a5fe61eff5524926b5900f6ffb428f38", 0,
            //    "这是客服给你发来的消息.by.server.16:23", "", 1);

            //// 发送自定义消息  杨雪
            //var toList8 = new List<string>();
            //toList8.Add("a5fe61eff5524926b5900f6ffb428f38");
            //var obj8 = OpenImApi.CustmsgPush("思普达目标达成:目标服务员2号", toList8, "这是客服给你发来的消息.by.server.16.24",
            //    "{\"D\":{\"T\":1002},\"T\":1002,\"M\":\"\",\"S\":\"\"}", "{\"alert\":\"ios apns push json\"}", "appparmas 123", 0, null, 1);



        }

        public static void DoTribeUser()
        {
            var dbName = "SqlServer";
            var sql = "select ImUserid, UserName from SAP360_Config.dbo.T_OUSI where CompanyID = 4594 ";
            IDbConnection cnn = DBOpr.GetConnection(dbName);
            var result = SqlMapper.Query<ImUser>(cnn, sql);
            if (result == null)
                return;

            var users = result.AsList();
            var dictUser = new Dictionary<string, string>();
            users.ForEach(c => { dictUser[c.ImUserid] = c.UserName; });

            var counter = users.Count/30;
            for (int i = 1; i <= counter; i++)
            {
                var memberIds = users.Skip((i - 1)*30).Take(30).Select(c => c.ImUserid);
                var inviteResult = OpenImApi.TribeInvite("ce74d6d8420f4575aec784319999852d", 1900647778, string.Join(",", memberIds), dictUser);
                if (inviteResult == null)
                {
                    
                }
                else if (inviteResult.FailUids.Any())
                {

                }
            }
            

        }


        public static void DoUtilTest()
        {
            string aa = "";
            var bb = JsonParse.Serialize("");

            var cc = JsonParse.Deserialize<ImUser>("");

            var kkkk = "123";
            var str = JsonParse.Serialize(new {kkkk});

            object kk2 = null;
            var str2 = JsonParse.Serialize(kk2);


            try
            {
                var dd = JsonParse.Serialize(null);
                var a1 = TopUtils.ParseResponse<OpenimUsersAddResponse>("");
                var a2 = TopUtils.ParseResponse<OpenimUsersAddResponse>(null);
            }
            catch (System.Exception ex)
            {
              //
            }


            
        }


        public class ImUser
        {
            public string ImUserid { get; set; }
            public string UserName { get; set; }
        }
    }
}
