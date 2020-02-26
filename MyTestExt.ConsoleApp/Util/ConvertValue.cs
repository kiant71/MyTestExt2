using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyTestExt.ConsoleApp.Util
{
    public class ConvertValue
    {





        private static long startTimeTick = 0;
        /// <summary>
        /// 转换长整形为时间,
        /// </summary>
        /// <param name="tick">是1970.1.1号之后的毫秒数</param>
        /// <returns></returns>
        public static DateTime ConvertLongToDateTime(long tick)
        {
            if (tick <= 0) return DateTime.MinValue;
            if (startTimeTick == 0) startTimeTick = (new DateTime(1970, 1, 1)).Ticks;
            return new DateTime(tick - startTimeTick);

        }


        public static string Substring(string str, int length)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > length)
                return str.Substring(length);

            return str;
        }

        public static string HtmlEncode(string str)
        {
            return string.IsNullOrWhiteSpace(str) ? "" : HttpUtility.HtmlEncode(str);
        }

        public static string HtmlDecode(string str)
        {
            return string.IsNullOrWhiteSpace(str) ? "" : HttpUtility.HtmlDecode(str);
        }


        public static string UrlEncode(string str)
        {
            return string.IsNullOrWhiteSpace(str) ? "" : HttpUtility.UrlEncode(str);
        }

        public static string UrlDecode(string str)
        {
            return string.IsNullOrWhiteSpace(str) ? "" : HttpUtility.UrlDecode(str);
        }


        /// <summary>
        /// 转换为base64码 并将特殊字符 /  +  = 替换成_a _b _c  
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncodeBase64(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return string.Empty;
            string base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
            return base64String.Replace("/", "_a").Replace("+", "_b").Replace("=", "_c");
        }

        /// <summary>
        /// 解码base64 并将特殊字符 _a _b _c  替换成/  +  = 
        /// </summary>
        /// <param name="base64Str"></param>
        /// <returns></returns>
        public static string DecodeBase64(string base64Str)
        {
            if (string.IsNullOrWhiteSpace(base64Str)) return string.Empty;
            string temp = base64Str.Replace("_a", "/").Replace("_b", "+").Replace("_c", "=");
            try
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(temp));
            }
            catch
            {
                return base64Str;
            }
        }

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <typeparam name="T">要获取的类型</typeparam>
        /// <param name="val">转换值对象</param>
        /// <returns>成功后返回转换后的值得,失败后返回该类型的默认值</returns>
        public static T ConvertType<T>(object val)
        {
            return ConvertType<T>(val, default(T));
        }

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <typeparam name="T">要获取的类型</typeparam>
        /// <param name="val">转换值对象</param>
        /// <param name="dftValue">默认值</param>
        /// <returns>成功后返回转换后的值得,失败后返回默认值</returns>
        public static T ConvertType<T>(object val, T dftValue)
        {
            T tempDftValue = dftValue == null ? default(T) : dftValue;
            if (val == null || val == System.DBNull.Value) return tempDftValue;
            Type tp = typeof(T);
            Type valueType = val.GetType();
            if (valueType == tp) return (T)val;
            //泛型Nullable判断，取其中的类型
            if (tp.IsGenericType) tp = tp.GetGenericArguments()[0];
            if (tp == typeof(string)) return (T)val;
            if (valueType != typeof(string))
            {
                try
                {
                    return (T)Convert.ChangeType(val, tp);
                }
                catch
                {
                    return tempDftValue;
                }
            }
            var TryParse = tp.GetMethod("TryParse", BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder,
                                            new Type[] { typeof(string), tp.MakeByRefType() },
                                            new ParameterModifier[] { new ParameterModifier(2) });
            var parameters = new object[] { val.ToString(), Activator.CreateInstance(tp) };
            bool success = (bool)TryParse.Invoke(null, parameters);
            if (success) return (T)parameters[1];
            return tempDftValue;
        }


        public static byte[] ShortToBytes(short i)
        {
            byte[] result = BitConverter.GetBytes(i);
            if (BitConverter.IsLittleEndian)
                return ReverseBytes(result);
            else
                return result;
        }

        public static byte[] IntToByte(int i)
        {
            byte[] result = BitConverter.GetBytes(i);
            if (BitConverter.IsLittleEndian)
                return ReverseBytes(result);
            else
                return result;

        }


        public static short ByteToShort(byte[] bytes, int startIndex)
        {
            if (bytes == null || bytes.Length == 0) return 0;
            if (BitConverter.IsLittleEndian)
                return BitConverter.ToInt16(ReverseBytes(bytes), startIndex);
            else
                return BitConverter.ToInt16(bytes, startIndex);
        }


        public static int ByteToInt(byte[] bytes, int startIndex)
        {
            if (bytes == null || bytes.Length == 0) return 0;
            if (BitConverter.IsLittleEndian)
                return BitConverter.ToInt32(ReverseBytes(bytes), startIndex);
            else
                return BitConverter.ToInt32(bytes, startIndex);
        }



        public static string GetMd5(string str)
        {
            var md5 = new MD5CryptoServiceProvider();
            var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            md5.Clear();

            var sb = new StringBuilder();
            for (var i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("x2").ToLower());
            }

            return sb.ToString();
        }



        public static byte[] ReverseBytes(byte[] inArray)
        {
            byte temp;
            int highCtr = inArray.Length - 1;

            for (int ctr = 0; ctr < inArray.Length / 2; ctr++)
            {
                temp = inArray[ctr];
                inArray[ctr] = inArray[highCtr];
                inArray[highCtr] = temp;
                highCtr -= 1;
            }
            return inArray;
        }

        /// <summary>
        /// 将数据集转换为强类型的
        /// </summary>
        /// <param name="noTypeDataSet">弱类型dataset</param>
        /// <param name="ds">强类型dataset</param>
        /// <param name="deleteField">是否删除不存在的数据表和字段</param>
        /// <returns></returns>
        public static DataSet ConvertTypeDataSet(DataSet noTypeDataSet, DataSet ds, bool deleteField)
        {
            if (noTypeDataSet == null || ds == null) return null;
            ds.Relations.Clear();

            Dictionary<string, List<string>> oDictDateField = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> oDictIntField = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> oDictDecimalField = new Dictionary<string, List<string>>();

            foreach (DataTable dt in ds.Tables)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.DataType == typeof(DateTime))
                    {
                        if (oDictDateField.ContainsKey(dt.TableName))
                            oDictDateField[dt.TableName].Add(dc.ColumnName);
                        else
                        {
                            List<string> list = new List<string>();
                            list.Add(dc.ColumnName);
                            oDictDateField.Add(dt.TableName, list);
                        }
                    }
                    else if (dc.DataType == typeof(int) || dc.DataType == typeof(long) || dc.DataType == typeof(short))
                    {
                        if (oDictIntField.ContainsKey(dt.TableName))
                            oDictIntField[dt.TableName].Add(dc.ColumnName);
                        else
                        {
                            List<string> list = new List<string>();
                            list.Add(dc.ColumnName);
                            oDictIntField.Add(dt.TableName, list);
                        }
                    }
                    else if (dc.DataType == typeof(float) || dc.DataType == typeof(decimal) || dc.DataType == typeof(double))
                    {
                        if (oDictDecimalField.ContainsKey(dt.TableName))
                            oDictDecimalField[dt.TableName].Add(dc.ColumnName);
                        else
                        {
                            List<string> list = new List<string>();
                            list.Add(dc.ColumnName);
                            oDictDecimalField.Add(dt.TableName, list);
                        }

                    }
                }
            }

            //处理空的日期字段
            foreach (var item in oDictDateField)
            {
                DataTable dt = noTypeDataSet.Tables[item.Key];
                if (dt == null) continue;
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (var columnName in item.Value)
                    {
                        if (!dt.Columns.Contains(columnName)) continue;
                        if (string.IsNullOrWhiteSpace(dr[columnName].ToString()))
                            dr[columnName] = System.DBNull.Value;
                    }

                }
            }

            //处理数字类型字段
            foreach (var item in oDictIntField)
            {
                DataTable dt = noTypeDataSet.Tables[item.Key];
                if (dt == null) continue;
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (var columnName in item.Value)
                    {
                        if (!dt.Columns.Contains(columnName)) continue;
                        if (string.IsNullOrWhiteSpace(dr[columnName].ToString()))
                            dr[columnName] = System.DBNull.Value;
                    }

                }
            }

            //处理小数点类型字段

            foreach (var item in oDictDecimalField)
            {
                DataTable dt = noTypeDataSet.Tables[item.Key];
                if (dt == null) continue;
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (var columnName in item.Value)
                    {
                        if (!dt.Columns.Contains(columnName)) continue;
                        if (string.IsNullOrWhiteSpace(dr[columnName].ToString()))
                            dr[columnName] = System.DBNull.Value;
                    }

                }
            }

            Dictionary<string, string> oDictTable = new Dictionary<string, string>();
            foreach (DataTable dt in noTypeDataSet.Tables)
            {
                oDictTable.Add(dt.TableName, "");
                DataTable dtType = ds.Tables[dt.TableName];
                if (dtType == null) continue;

                foreach (DataRow dr in dt.Rows)
                    ds.Tables[dt.TableName].ImportRow(dr);
                if (deleteField)
                {
                    Dictionary<string, string> oDictField = new Dictionary<string, string>();
                    foreach (DataColumn dc in dt.Columns)
                        oDictField.Add(dc.ColumnName, "");
                    List<string> listDelCol = new List<string>();
                    foreach (DataColumn dc in dtType.Columns)
                    {
                        if (oDictField.ContainsKey(dc.ColumnName)) continue;
                        listDelCol.Add(dc.ColumnName);
                    }
                    //删除不存在的字段
                    foreach (var item in listDelCol)
                        dtType.Columns.Remove(item);
                }
            }

            if (deleteField)
            {
                //删除不存在的数据表
                List<string> list = new List<string>();
                foreach (DataTable dt in ds.Tables)
                {
                    if (dt.Columns.Count == 0)
                    {
                        list.Add(dt.TableName);
                        continue;
                    }
                    if (oDictTable.ContainsKey(dt.TableName)) continue;
                    list.Add(dt.TableName);


                }
                foreach (var item in list)
                    ds.Tables.Remove(item);
            }
            return ds;
        }


        /// <summary>
        /// 转换数据表
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columns"></param>
        /// <param name="isAddRowsStatus"></param>
        /// <returns></returns>
        private static string CreateTableSQL(DataTable dt, string[] columns, bool isAddRowsStatus, bool isTempTable)
        {
            if (dt == null || columns == null || columns.Length == 0) return string.Empty;
            StringBuilder sbSql = new StringBuilder();
            if (isTempTable)
                sbSql.Append(string.Format("CREATE TABLE #{0}(", dt.TableName));
            else
                sbSql.Append(string.Format("DECLARE @{0} TABLE(", dt.TableName)).AppendLine();
            for (int i = 0; i < columns.Length; i++)
            {
                string columnName = columns[i].Trim();
                DataColumn dc = dt.Columns[columnName];
                Type columnType = dc.DataType;
                if (i > 0) sbSql.Append(",");
                string dataType = string.Empty;
                if (columnType == typeof(string))
                {
                    if (dc.MaxLength == -1)
                        dataType = " NVARCHAR(MAX) COLLATE DATABASE_DEFAULT";
                    else
                    {
                        if (dc.MaxLength > 7999)
                            dataType = " NVARCHAR(MAX) COLLATE DATABASE_DEFAULT";
                        else if (dc.MaxLength == 1)
                            dataType = " CHAR(1)";
                        else
                            dataType = String.Format(" NVARCHAR({0}) COLLATE DATABASE_DEFAULT", dc.MaxLength);
                    }
                    sbSql.Append(columnName).Append(dataType);
                }
                else if (columnType == typeof(DateTime))
                {
                    dataType = " DATETIME";
                    sbSql.Append(columnName).Append(dataType);
                }
                else if (columnType == typeof(int) || columnType == typeof(Int16) || columnType == typeof(Int32))
                {
                    dataType = " INT";
                    sbSql.Append(columnName).Append(dataType);
                }

                else if (columnType == typeof(Int64) || columnType == typeof(long))
                {
                    dataType = " BIGINT";
                    sbSql.Append(columnName).Append(dataType);
                }
                else
                {
                    dataType = " DECIMAL(19,6)";
                    sbSql.Append(columnName).Append(dataType);
                }
            }

            if (isAddRowsStatus)
                sbSql.Append(", RowState NVARCHAR(10)  COLLATE DATABASE_DEFAULT");

            sbSql.Append(")").Append(System.Environment.NewLine);

            return sbSql.ToString();
        }

        private static string CreateInsertSql(DataTable dt, string[] columns, bool isAddRowsStatus, bool isTempTable)
        {
            if (dt == null || columns == null || columns.Length == 0) return string.Empty;
            StringBuilder sbSql = new StringBuilder();
            if (isTempTable)
                sbSql.Append(string.Format("INSERT INTO #{0} (", dt.TableName));
            else
                sbSql.Append(string.Format("INSERT INTO @{0} (", dt.TableName));
            for (int i = 0; i < columns.Length; i++)
            {
                if (i > 0) sbSql.Append(",");
                sbSql.Append(columns[i]);
            }
            if (isAddRowsStatus)
                sbSql.Append(",RowState");
            sbSql.Append(") VALUES (");

            //创建值
            for (int i = 0; i < columns.Length; i++)
            {
                if (i > 0) sbSql.Append(",");
                sbSql.Append(string.Format("{{{0}}}", i));
            }
            if (isAddRowsStatus)
                sbSql.Append(string.Format(",N'{{{0}}}'", columns.Length));
            sbSql.Append(")");
            return sbSql.ToString();
        }

        /// <summary>
        ///  取得datatable在sql中创建数据表插入数据的sql 
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <param name="columns">列集合，如果为空则为dataTable的所有列创建</param>
        /// <param name="isAddRowsStatus">是否增加行状态字段</param>
        /// <returns></returns>
        public static string CreateDataTableSQLSyntax(DataTable dt, string[] columns, bool isAddRowsStatus)
        {
            return CreateTableSQLSyntax(dt, columns, isAddRowsStatus, false, false, false);
        }

        public static string CreateTempTableSQLSyntax(DataTable dt, string[] columns, bool isAddRowsStatus)
        {
            return CreateTableSQLSyntax(dt, columns, isAddRowsStatus, false, false, true);
        }


        /// <summary>
        ///  取得datatable在sql中创建数据表插入数据的sql 
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <param name="columns">列集合，如果为空则为dataTable的所有列创建</param>
        /// <param name="isAddRowsStatus">是否增加行状态字段</param>
        /// <param name="ignoreNotChangeRow">忽略数据没有变化的行</param>
        /// <param name="isGetSourceValue">是否取原始值</param>
        /// <returns></returns>
        public static string CreateDataTableSQLSyntax(DataTable dt, string[] columns, bool isAddRowsStatus, bool ignoreNotChangeRow, bool isGetSourceValue)
        {
            return CreateTableSQLSyntax(dt, columns, isAddRowsStatus, ignoreNotChangeRow, isGetSourceValue, false);
        }

        public static string CreateTempTableSQLSyntax(DataTable dt, string[] columns, bool isAddRowsStatus, bool ignoreNotChangeRow, bool isGetSourceValue)
        {
            return CreateTableSQLSyntax(dt, columns, isAddRowsStatus, ignoreNotChangeRow, isGetSourceValue, true);
        }

        private static string CreateTableSQLSyntax(DataTable dt, string[] columns, bool isAddRowsStatus, bool ignoreNotChangeRow, bool isGetSourceValue, bool isTempTable)
        {
            if (dt == null || dt.Columns.Count == 0) return string.Empty;
            Dictionary<string, string> oDictDate = new Dictionary<string, string>();
            Dictionary<string, string> oDictString = new Dictionary<string, string>();
            Dictionary<string, string> oDictChar = new Dictionary<string, string>();

            if (columns == null)
            {
                List<string> listCol = new List<string>();
                columns = new string[dt.Columns.Count];
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.DataType == typeof(byte[])) continue;
                    listCol.Add(dc.ColumnName);
                }
                columns = listCol.ToArray();
            }
            else if (columns.Length == 0)
                return string.Empty;
            else
            {
                for (int i = 0; i < columns.Length; i++)
                    columns[i] = columns[i].Trim();
            }
            List<string> listExist = new List<string>();
            //记录日期类型字段
            foreach (string columnName in columns)
            {
                DataColumn dc = dt.Columns[columnName];
                if (dc == null) continue;
                listExist.Add(columnName);
                string colNameLower = columnName.ToLower();
                if (dc.DataType == typeof(DateTime)) oDictDate.Add(colNameLower, "");
                else if (dc.DataType == typeof(string)) oDictString.Add(colNameLower, "");
                else if (dc.DataType == typeof(char)) oDictChar.Add(colNameLower, "");
            }
            columns = listExist.ToArray();
            StringBuilder sbSql = new StringBuilder();
            string createTableSql = CreateTableSQL(dt, columns, isAddRowsStatus, isTempTable);
            if (string.IsNullOrWhiteSpace(createTableSql)) return string.Empty;
            string insertSql = CreateInsertSql(dt, columns, isAddRowsStatus, isTempTable);
            if (string.IsNullOrWhiteSpace(insertSql)) return string.Empty;
            sbSql.AppendLine(createTableSql);
            int length = columns.Length;
            if (isAddRowsStatus) length++;
            string[] valueArray = new string[length];
            foreach (DataRow dr in dt.Rows)
            {
                if (dr.RowState == DataRowState.Detached) continue;
                if (ignoreNotChangeRow && dr.RowState == DataRowState.Unchanged) continue;
                for (int i = 0; i < columns.Length; i++)
                {
                    string columnName = columns[i];
                    string columnValue = string.Empty;
                    string colNameLower = columnName.ToLower();
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        if (dr[columnName, DataRowVersion.Original] == System.DBNull.Value)
                            columnValue = "NULL";
                        else
                        {
                            if (oDictDate.ContainsKey(colNameLower))
                                columnValue = string.Format("CONVERT(DATETIME,'{0}',120)", ((DateTime)dr[columnName, DataRowVersion.Original]).ToString("yyyy.MM.dd HH:mm:ss"));
                            else if (oDictString.ContainsKey(colNameLower))
                                columnValue = string.Format("N'{0}'", dr[columnName, DataRowVersion.Original].ToString().Replace("'", "''"));
                            else if (oDictChar.ContainsKey(colNameLower))
                                columnValue = string.Format("'{0}'", dr[columnName, DataRowVersion.Original].ToString().Replace("'", "''"));
                            else
                                columnValue = dr[columnName, DataRowVersion.Original].ToString();
                        }
                    }
                    else if (dr.RowState == DataRowState.Modified)
                    {
                        if (isGetSourceValue)
                        {
                            if (dr[columnName, DataRowVersion.Original] == System.DBNull.Value)
                                columnValue = "NULL";
                            else
                            {
                                if (oDictDate.ContainsKey(colNameLower))
                                    columnValue = string.Format("CONVERT(DATETIME,'{0}',120)", ((DateTime)dr[columnName, DataRowVersion.Original]).ToString("yyyy.MM.dd HH:mm:ss"));
                                else if (oDictString.ContainsKey(colNameLower))
                                    columnValue = string.Format("N'{0}'", dr[columnName, DataRowVersion.Original].ToString().Replace("'", "''"));
                                else if (oDictChar.ContainsKey(colNameLower))
                                    columnValue = string.Format("'{0}'", dr[columnName, DataRowVersion.Original].ToString().Replace("'", "''"));
                                else
                                    columnValue = dr[columnName, DataRowVersion.Original].ToString();
                            }
                        }
                        else
                        {
                            if (dr[columnName] == System.DBNull.Value)
                                columnValue = "NULL";
                            else
                            {
                                if (oDictDate.ContainsKey(colNameLower))
                                    columnValue = string.Format("CONVERT(DATETIME,'{0}',120)", ((DateTime)dr[columnName]).ToString("yyyy.MM.dd HH:mm:ss"));
                                else if (oDictString.ContainsKey(colNameLower))
                                    columnValue = string.Format("N'{0}'", dr[columnName].ToString().Replace("'", "''"));
                                else if (oDictChar.ContainsKey(colNameLower))
                                    columnValue = string.Format("'{0}'", dr[columnName].ToString().Replace("'", "''"));
                                else
                                    columnValue = dr[columnName].ToString();
                            }
                        }
                    }
                    else
                    {
                        if (dr[columnName] == System.DBNull.Value)
                            columnValue = "NULL";
                        else
                        {
                            if (oDictDate.ContainsKey(colNameLower))
                                columnValue = string.Format("CONVERT(DATETIME,'{0}',120)", ((DateTime)dr[columnName]).ToString("yyyy.MM.dd HH:mm:ss"));
                            else if (oDictString.ContainsKey(colNameLower))
                                columnValue = string.Format("N'{0}'", dr[columnName].ToString().Replace("'", "''"));
                            else if (oDictChar.ContainsKey(colNameLower))
                                columnValue = string.Format("'{0}'", dr[columnName].ToString().Replace("'", "''"));
                            else
                                columnValue = dr[columnName].ToString();
                        }
                    }
                    valueArray[i] = columnValue;
                }

                if (isAddRowsStatus)
                    valueArray[length - 1] = dr.RowState.ToString();

                sbSql.AppendLine(string.Format(insertSql, valueArray));
            }
            return sbSql.ToString();
        }


    }
}
