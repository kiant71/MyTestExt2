using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication1.Util;
using MyTestExt.ConsoleApp.Dapper;

namespace MyTestExt.ConsoleApp
{
    public class DataAccessTest
    {
        public static void Test()
        {
            /// 采用继承和重写的方式，适应不同的数据库，不同的语句
            /// DataSet 中 DataRelation 的问题，配置中如何指定这种关系。
            /// 关系名称，父对象列数组，子对象列数组，是否约束

            //var aa = new RequestJson { BizId = "queryBizA", Params = new RequestJsonParam { type = "IT", type_desc = "SYSTEM_TABLE" } };
            //string bb = JsonParse.Serialize(aa);

            //####  SqlServer
            string input = @"{'BizId':'queryBizA','Params':{'user_id':10000}}";

            //MySql
            //string input = @"{'BizId':'queryBizA','Params':{'parentno':0,'loc':'%cd%', 'dept_id': 0}}";

            //Oracle
            //string input = @"{'BizId':'queryBizA','Params':{'MyUSER':'ROOT1', 'user_id':1001, 'user_id2':1002}}";


            //TODO.从公网上传输过来的数据，最好定义一个对象封装（规范参数）
            dynamic inputObj = JsonParse.Deserialize<dynamic>(input);


            //####### 实际上根据 BizId 解析出来的dbname, sql 
            string dbName = "SqlServer";
            //string sql = "select user_id, username, password, tel1, role_id, dept_id, dept_name from s_tb_user where dept_id = @dept_id ";
            string sql = "select top(10) * from T_IMRE ";

            //string dbName = "mysql";
            //string sql = "select user_id, username, password, tel1, role_id, dept_id, dept_name from m_tb_user where dept_id = @dept_id ";
            //MySql 允许一次查询返回多个结果集

            //string dbName = "Oracle";
            //string sql = "select user_id, username, password, tel1, role_id, dept_id, dept_name from o_tb_user where dept_id = :dept_id and  username =:username ";
            //Oracle pl/sql 块内 select 必须和 into搭配


            //验证脚本
            //DynamicParameters param2 = new DynamicParameters();
            //Regex regex = new Regex(@"[@:](\w+)");
            //MatchCollection matches = regex.Matches(sql);
            //foreach (Match match in matches)
            //{
            //    param2.Add(match.Groups[1].Value);
            //}
            //DBOpr.Query2(dbName, sql, param2);

            Newtonsoft.Json.Linq.JObject inputParam = inputObj["Params"];
            DynamicParameters param = new DynamicParameters();
            if (inputParam != null)
            {
                foreach (var item in inputParam)
                {
                    param.Add(item.Key, item.Value.ToString());
                }
            }

            try
            {
                DataTable dt = new DataTable();
                using (IDbConnection cnn = DBOpr.GetConnection(dbName))
                {
                    using (IDataReader dr = SqlMapper.ExecuteReader(cnn, sql, param))
                    {
                        dt.Load(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //
        }

    }


    public class DBOpr
    {
        public static Queue<List<dynamic>> Query(string dbName, string sql, object param = null)
        {
            if (string.IsNullOrWhiteSpace(dbName) || string.IsNullOrWhiteSpace(sql))
                return null;

            using (IDbConnection cnn = GetConnection(dbName))
            {
                try
                {
                    var grid = SqlMapper.QueryMultiple(cnn, sql, param);
                    Queue<List<dynamic>> res = new Queue<List<dynamic>>();
                    while (!grid.IsConsumed)
                    {
                        res.Enqueue(grid.Read(false).ToList());
                    }
                    return res;
                }
                catch (Exception ex)
                {
                    //log
                    throw ex;
                }
            }
        }


        //针对可能的数据库，定义数据库工厂集合
        static Dictionary<string, DbProviderFactory> providerFactoies = new Dictionary<string, DbProviderFactory>();

        //读取配置文件，获得对应的数据库连接语句
        //static string SqlServerDbString = "Data Source=SQLSERVER03;Initial Catalog=test_data;User ID=sa;Password=sap360code;Persist Security Info=True;Pooling=true";
        static string SqlServerDbString = "Application Name=SAP360;Password=spd_SAP360E206;Persist Security Info=True;Pooling=true;User ID=sa;Initial Catalog=SAP360_Common;Data Source=192.168.1.22;Connect Timeout=10;ApplicationIntent=ReadOnly";
        static string MySqlDbString = "Server=192.168.1.153;database=fastdfs;User ID=root;Password=pass1;charset=utf8";
        static string OracleDbString = "Data Source=192.168.1.153:1521/orcl;User ID=root1;Password=pass1;Pooling=true";

        /// <summary>
        /// 获取数据库工厂
        /// </summary>
        /// <param configName="dbName"></param>
        /// <returns></returns>
        private static DbProviderFactory GetFactory(string dbName)
        {
            if (!providerFactoies.ContainsKey(dbName))
            {
                string providerName = "";
                switch (dbName.ToLower())
                {
                    case "sqlserver": providerName = "System.Data.SqlClient";
                        break;
                    case "mysql": providerName = "MySql.Data.MySqlClient";
                        break;
                    case "oracle": providerName = "Oracle.ManagedDataAccess.Client";
                        break;
                    default: providerName = "System.Data.SqlClient";
                        break;
                }

                try
                {
                    providerFactoies.Add(dbName, DbProviderFactories.GetFactory(providerName));

                }
                catch (Exception ex)
                {
                    //log....
                    throw ex;
                }
            }

            return providerFactoies[dbName];
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <returns></returns>
        public static IDbConnection GetConnection(string dbName)
        {
            IDbConnection conn = null;
            try
            {
                conn = GetFactory(dbName).CreateConnection();

                string strConnection = "";
                switch (dbName.ToLower())
                {
                    case "sqlserver": strConnection = SqlServerDbString;
                        break;
                    case "mysql": strConnection = MySqlDbString;
                        break;
                    case "oracle": strConnection = OracleDbString;
                        break;
                    default: strConnection = SqlServerDbString;
                        break;
                }

                conn.ConnectionString = strConnection;
                if (conn.State == ConnectionState.Closed) conn.Open();
            }
            catch (Exception ex)
            {
                //log
                throw ex;
            }
            return conn;
        }




        public static void Query2(string dbName, string sql, object param = null)
        {
            if (string.IsNullOrWhiteSpace(dbName) || string.IsNullOrWhiteSpace(sql))
                return;

            using (IDbConnection cnn = GetConnection(dbName))
            {
                try
                {
                    var dr = SqlMapper.ExecuteReader(cnn, sql, param);

                }
                catch (Exception ex)
                {
                    //log
                    throw ex;
                }
            }
        }


    }
}
