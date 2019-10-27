//using MySql.Data.MySqlClient;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Fate.Common.Infrastructure;
//using Microsoft.Extensions.Configuration;
//using System.IO;
//namespace Fate.Common.Extensions
//{
//    public static class MySqlBulkExtensions
//    {
//        /// <summary>
//        /// 批量导入
//        /// </summary>
//        /// <param name="_mySqlConnection"></param>
//        /// <param name="dt"></param>
//        /// <returns></returns>
//        public static async Task<int> BulkLoadAsync(this DataTable table, string connectionString)
//        {
//            var res = 0;
//            var path = "";
//            if (table != null && table.Rows.Count > 0)
//            {
//                //获取连接地址
//                if (string.IsNullOrWhiteSpace(connectionString))
//                    throw new ArgumentNullException("mysql连接地址不能为空");
//                using (MySqlConnection _mySqlConnection = new MySqlConnection(connectionString))
//                {
//                    path = await table.ToCsvAsync();
//                    MySqlTransaction mySqlTransaction = null;
//                    try
//                    {
//                        _mySqlConnection.Open();
//                        //开启事务
//                        mySqlTransaction = _mySqlConnection.BeginTransaction();
//                        //获取需要导入的列
//                        var columns = table.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToList();
//                        //初始化参数
//                        MySqlBulkLoader bulk = new MySqlBulkLoader(_mySqlConnection)
//                        {
//                            FieldTerminator = ",",
//                            FieldQuotationCharacter = '"',
//                            EscapeCharacter = '"',
//                            LineTerminator = "\r\n",
//                            FileName = path,//文件名
//                            NumberOfLinesToSkip = 0,
//                            TableName = table.TableName,//数据表名
//                            CharacterSet = "utf8"
//                        };
//                        bulk.Columns.AddRange(columns);
//                        //通过文件的方式 添加数据到数据库
//                        res = await bulk.LoadAsync().ConfigureAwait(false);
//                        //提交事务
//                        mySqlTransaction.Commit();
//                    }
//                    catch (Exception ex)
//                    {
//                        if (mySqlTransaction != null)
//                        {
//                            mySqlTransaction.Rollback();
//                        }
//                        throw ex;
//                    }
//                    finally
//                    {
//                        if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
//                        {
//                            File.Delete(path);
//                        }
//                    }
//                }
//            }
//            return await Task.FromResult(res).ConfigureAwait(false);
//        }
//    }
//}
