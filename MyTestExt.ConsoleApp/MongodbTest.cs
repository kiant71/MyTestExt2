using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication1.Util;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MyTestExt.Utils.Json;
using ServiceStack.OrmLite;

namespace MyTestExt.ConsoleApp
{
    public  class MongodbTest
    {
        //private static string serUrls = ""; //"mongodb://192.168.1.209:30000,192.168.1.184:30000,192.168.1.183:30000" , "ApplicationDatabase");
 
        public static void Do()
        {
            var client = new MongoClient("mongodb://192.168.1.209:30000,192.168.1.184:30000,192.168.1.183:30000");
            var db = client.GetDatabase("db1");
            var coll = db.GetCollection<MongodbTestModel>("test0425");

            //var model = new MongodbTestModel
            //{
            //    ID = 1, Name = "1.1",
            //    DT1 = DateTime.Now,
            //    DT2 = DateTime.UtcNow,
            //    DTS1 = DateTimeOffset.Now,
            //    DTS2 = DateTimeOffset.UtcNow,
            //};
            //coll.InsertOne(model);

            var res = coll.Find(c => c.ID == 1).FirstOrDefault();
            var resJson = JsonNet.Serialize(res);


            var coll2 = db.GetCollection<MongodbTest2Model>("test0425");
            var res2 = coll2.Find(c => c.ID == 1).FirstOrDefault();
            var res2Json = JsonNet.Serialize(res2);
        }

    }



    [BsonIgnoreExtraElements]

    public class MongodbTestModel
    {
        [BsonId]
        public int ID { get; set; }

        public string Name { get; set; } = "";

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DT1 { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? DT2 { get; set;}

        public DateTimeOffset DTS1 { get; set; }

        public DateTimeOffset? DTS2 { get; set; }
    }

    [BsonIgnoreExtraElements]

    public class MongodbTest2Model
    {
        [BsonId]
        public int ID { get; set; }

        public string Name { get; set; } = "";

        //[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        //public DateTime DTS1 { get; set; }

        //[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        //public DateTime? DTS2 { get; set; }

        public DateTimeOffset DT1 { get; set; }

        public DateTimeOffset? DT2 { get; set; }
    }

}
