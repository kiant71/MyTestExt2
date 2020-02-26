using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Dapper;


namespace MyTestExt.ConsoleApp
{
    public partial class OrmLiteTest
    {
        private static string ConnString =
            // "Initial Catalog=king_comm; Data Source=192.168.1.192;Password=spd_SAP360E206;Application Name=SAP360;Persist Security Info=True;Pooling=true;User ID=sa;";
            "Application Name=SPD;Password=spd_SAP360E206;Persist Security Info=True;Pooling=true;User ID=sa;Initial Catalog=SAP360_Management;Data Source=192.168.1.192";

        public static void Do()
        {
            var dbFactory = new OrmLiteConnectionFactory(ConnString, SqlServer2014Dialect.Provider);
            using (IDbConnection db = dbFactory.Open())
            {
                db.DropAndCreateTable<OrmLiteTestModel>();
                var todo = new OrmLiteTestModel
                {
                    TestID = 1,
                     CompanyID = 1,
                     ImageIndex = 3,
                     Remark = "123",
                     TestName = "Name1",
                     TestType = 3,
                };
                db.Save(todo);

                //var savedTodo = db.SingleById<Todo>(todo.Id);
                //savedTodo.Content = "Updated";
                //db.Save(savedTodo);

                //"Updated Todo:".Print();
                //db.Select<Todo>(q => q.Content == "Updated").PrintDump();

                //db.DeleteById<Todo>(savedTodo.Id);

                //"No more Todos:".Print();
                //db.Select<Todo>().PrintDump();
            }
        }


        public static void Do2()
        {
            var dbFactory = new OrmLiteConnectionFactory(ConnString, SqlServer2014Dialect.Provider);
            using (IDbConnection db = dbFactory.Open())
            {
                var sqlBase = @"
/* 相关类型用户已经在 群组成员列表中维护成具体人员UserSign */
DECLARE @dt TABLE(            GroupID BIGINT, UnReadMsgNum int,  UserColor int, IsNotify BIT,        DocEntry BIGINT, ApproveStatus tinyint, WorkFlowStatus tinyint, IsConvert BIT, TargetDoc BIGINT, DraftID BIGINT, WtmCode INT,  WtmName nvarchar(50)
        , CurrWstName nvarchar(50))
INSERT INTO @dt
SELECT TOP(20) t.GroupID,     t1.UnReadMsgNum,   t1.UserColor,  t1.IsNotify,         oa.DocEntry,     oa.ApproveStatus,      oa.WorkFlowStatus,      oa.IsConvert,  oa.TargetDoc,     oa.DraftID,      oa.WtmCode,  oa.WtmName
        , (SELECT TOP(1) StageName FROM T_OAWF3 oa3 WHERE oa3.DocEntry=oa.DocEntry AND oa3.StageID=oa.CurrWst) CurrWstName
    FROM         T_OMGC t
    INNER JOIN   T_OMGC1 t1  ON t.GroupID=t1.GroupID   AND t1.UserSign=@UserSign  AND t1.IsDel=0  
    INNER JOIN   T_OAWF  oa  ON oa.DocEntry=t.BaseEntry
    WHERE 1=1  AND t.GroupType=3
    /**sqlCondi**/
    /**orderBy**/ ;

/* result1: list，做多一次查询，暂时不在临时表中缓存，因为涉及表结构变动的问题 */
SELECT b.*, t.Subject, t.GroupType, t.Status, t.CompanyID, t.UserSign, t.MainUser, t.UserCount, t.CreateDate, t.CreateMS, 
                t.UpdateTime, t.UpdateMS, t.BaseCompanyID, t.BaseEntry, t.BaseType, t.BaseName, t.FormID, t.TemplateID, t.ApproveFlowID, 
                t.ImageFullUrl, t.ImageIndex, t.LastMsgUserSign, t.LastMsgText, t.LastMsgTime, t.LastMsgMS, t.ExtField, t.Title, t.LastMsgID, 
                t.LastMsgType, t.OtherUserSign, t.IsMigration, t.IsDel, t.IconUsers, t.TitleBaseType, t.TitleImageIndex, t.GroupTypeSec 
FROM          @dt b
INNER JOIN  T_OMGC t ON t.GroupID=b.GroupID

/* result2: listUsers */
SELECT   oa3.DocEntry, oa3.UserType, oa3.UserSign, oa3.ApproveID
FROM       @dt    oa
INNER JOIN T_OAWF3 oa3 ON oa3.DocEntry=oa.DocEntry AND oa3.CurrApprover=1

";
                //var result = db.SqlList<dynamic>(sqlBase, new { UserSign = 100035});

                var multi = db.QueryMultiple(sqlBase, new { UserSign = 100035 });
                var result1 = multi.Read<dynamic>().ToList();
                var result2 = multi.Read<dynamic>().ToList();

                
            }
        }


        public static void Do3()
        {
            var dbFactory = new OrmLiteConnectionFactory(ConnString, SqlServer2014Dialect.Provider);
            using (IDbConnection db = dbFactory.Open())
            {
                var q = db.From<OrmLiteTestModel>()
                    .Where(c => c.TestID == 1);

                var sql = q.ToSelectStatement();
                // SELECT "TestID", "CompanyID", "TestName", "TestType", "Remark", "ImageIndex" 
                //FROM "T_OLTE"
                //WHERE("TestID" = @0)

                //var res1 = db.QueryFirstOrDefault<OrmLiteTestModel>(sql);

                var res2 = db.Select<OrmLiteTestModel>(q);
            }

            using (IDbConnection db = dbFactory.Open())
            {
                Expression<Func<OrmLiteTestModel, bool>> where = (c => c.TestID == 1);
                var q = db.From<OrmLiteTestModel>()
                    .Where(where);
                
                var res2 = db.Select<OrmLiteTestModel>(q);
            }
        }
    }
}
