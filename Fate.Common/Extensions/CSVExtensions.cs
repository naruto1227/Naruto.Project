
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Common.Extensions
{
    public static class CSVEx
    {
        /// <summary>
        ///将DataTable转换为标准的CSV
        /// </summary>
        /// <param name="table">数据表</param>
        /// <returns>返回标准的CSV</returns>
        public static async Task<string> ToCsvAsync (this DataTable table)
        {
            //以半角逗号（即,）作分隔符，列为空也要表达其存在。
            //列内容如存在半角逗号（即,）则用半角引号（即""）将该字段值包含起来。
            //列内容如存在半角引号（即"）则应替换成半角双引号（""）转义，并用半角引号（即""）将该字段值包含起来。
            StringBuilder sb = new StringBuilder();
            DataColumn colum;
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    colum = table.Columns[i];
                    if (i != 0) sb.Append(",");
                    if (colum.DataType == typeof(string) && row[colum].ToString().Contains(","))
                    {
                        sb.Append("\"" + row[colum].ToString().Replace("\"", "\"\"") + "\"");
                    }
                    else sb.Append(row[colum].ToString());
                }
                sb.AppendLine();
            }
            //拼接路劲 地址位mysql默认允许的文件存放的地址
            var path = Path.Combine(Config.MySqlBulkConfig.MySqlLoadFilePath, table.TableName + DateTime.Now.Ticks + ".csv");
            await Task.Run(() =>
            {
                File.WriteAllText(path, sb.ToString());
            });
            return path;
        }
    }
}
