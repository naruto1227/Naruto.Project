
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Naruto.Infrastructure.NPOI
{
    /// <summary>
    /// EXCEL导出功能集合类
    /// 作者：ZhangHaiBo
    /// 日期：2016/1/15
    /// </summary>
    public static class ExportHelper
    {
        /// <summary>
        /// 由DataSet导出Excel(多个工作谱)
        /// </summary>
        /// <param name="sourceDs">要导出数据的DataSet</param>
        /// <param name="filePath">导出路径</param>
        /// <param name="colorIndex">表头颜色的样式  列：NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index</param>
        /// <param name="colorTrue">是否显示表头的颜色  默认显示</param>
        /// <returns></returns>
        public static string ToExcel(this DataSet sourceDs, string filePath, bool colorTrue = true, short colorIndex = 22)
        {

            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("导出地址不能为空");
            }

            if (string.IsNullOrEmpty(filePath)) return null;

            bool isCompatible = CommonBase.GetIsCompatible(filePath);

            IWorkbook workbook = CommonBase.CreateWorkbook(isCompatible);
            ICellStyle headerCellStyle = CommonBase.GetCellStyle(workbook, colorTrue, colorIndex);
            //ICellStyle cellStyle = Common.GetCellStyle(workbook);

            for (int i = 0; i < sourceDs.Tables.Count; i++)
            {
                DataTable table = sourceDs.Tables[i];
                string sheetName = string.IsNullOrEmpty(table.TableName) ? "result" + i.ToString() : table.TableName;
                ISheet sheet = workbook.CreateSheet(sheetName);
                IRow headerRow = sheet.CreateRow(0);
                Dictionary<int, ICellStyle> colStyles = new Dictionary<int, ICellStyle>();
                // handling header.
                foreach (DataColumn column in table.Columns)
                {
                    ICell headerCell = headerRow.CreateCell(column.Ordinal);
                    headerCell.SetCellValue(column.ColumnName);
                    headerCell.CellStyle = headerCellStyle;
                    sheet.AutoSizeColumn(headerCell.ColumnIndex);
                    colStyles[headerCell.ColumnIndex] = CommonBase.GetCellStyle(workbook);
                }

                // handling value.
                int rowIndex = 1;

                foreach (DataRow row in table.Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);

                    foreach (DataColumn column in table.Columns)
                    {
                        ICell cell = dataRow.CreateCell(column.Ordinal);
                        //cell.SetCellValue((row[column] ?? "").ToString());
                        //cell.CellStyle = cellStyle;
                        CommonBase.SetCellValue(cell, (row[column] ?? "").ToString(), column.DataType, colStyles);
                        CommonBase.ReSizeColumnWidth(sheet, cell);
                    }

                    rowIndex++;
                }
                sheet.ForceFormulaRecalculation = true;
            }

            FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            workbook.Write(fs);
            fs.Dispose();
            workbook = null;

            return filePath;

        }
        /// <summary>
        /// 由DataSet导出Excel (一个工作谱) sheet的名字为 table的tableName内容
        /// </summary>
        /// <param name="sourceDs">要导出数据的DataSet</param>
        /// <param name="filePath">导出路径</param>
        /// <param name="growthIndex">多个表之间间隔的行数 ，可选</param>
        /// <param name="colorIndex">表头颜色的样式  列：NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index</param>
        /// <param name="colorTrue">是否显示表头颜色 默认 是</param>
        /// <returns></returns>
        public static string ToExcelOne(this DataSet sourceDs, string filePath, string sheetTableName = "result", int growthIndex = 3, bool colorTrue = true, short colorIndex = 22)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("导出地址不能为空");
            }

            if (string.IsNullOrEmpty(filePath)) return null;

            bool isCompatible = CommonBase.GetIsCompatible(filePath);

            IWorkbook workbook = CommonBase.CreateWorkbook(isCompatible);
            ICellStyle headerCellStyle = CommonBase.GetCellStyle(workbook, colorTrue, colorIndex);
            //ICellStyle cellStyle = Common.GetCellStyle(workbook);
            string sheetName = sheetTableName;
            ISheet sheet = workbook.CreateSheet(sheetName);
            int index = 0; //行的索引
            int rowIndex = 1;
            for (int i = 0; i < sourceDs.Tables.Count; i++)
            {
                DataTable table = sourceDs.Tables[i];
                if (index != 0)
                {
                    index = index + growthIndex;
                }
                IRow headerRow = sheet.CreateRow(index);
                Dictionary<int, ICellStyle> colStyles = new Dictionary<int, ICellStyle>();
                // 处理表头
                foreach (DataColumn column in table.Columns)
                {
                    ICell headerCell = headerRow.CreateCell(column.Ordinal);
                    headerCell.SetCellValue(column.ColumnName);
                    headerCell.CellStyle = headerCellStyle;
                    sheet.AutoSizeColumn(headerCell.ColumnIndex);
                    colStyles[headerCell.ColumnIndex] = CommonBase.GetCellStyle(workbook);
                }

                // 处理数据
                if (rowIndex != 1)
                {
                    rowIndex = rowIndex + growthIndex;
                }
                foreach (DataRow row in table.Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in table.Columns)
                    {
                        ICell cell = dataRow.CreateCell(column.Ordinal);
                        CommonBase.SetCellValue(cell, (row[column] ?? "").ToString(), column.DataType, colStyles);
                        CommonBase.ReSizeColumnWidth(sheet, cell);
                    }
                    rowIndex++;
                }
                sheet.ForceFormulaRecalculation = true;
                index++;
            }
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                workbook.Write(fs);
            }
            workbook = null;
            return filePath;
        }
        /// <summary>
        /// 由DataTable导出Excel sheet的名字为 table的tableName内容
        /// </summary>
        /// <param name="sourceTable">要导出数据的DataTable</param>
        /// <param name="colAliasNames">导出的列名重命名数组</param>
        /// <param name="sheetName">工作薄名称，可选</param>
        /// <param name="filePath">导出路径，可选</param>
        /// <param name="colDataFormats">列格式化集合，可选</param>
        /// <param name="colorIndex">表头颜色的样式  列：NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index</param>
        /// <param name="colorTrue">是否显示表头颜色 默认 是</param>
        /// <returns></returns>
        public static string ToExcel(this DataTable sourceTable, string[] colAliasNames, string sheetName = "result", string filePath = null, IDictionary<string, string> colDataFormats = null, bool colorTrue = true, short colorIndex = 22)
        {
            if (sourceTable.Rows.Count <= 0) return null;

            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("导出地址不能为空");
            }

            if (string.IsNullOrEmpty(filePath)) return null;

            if (colAliasNames == null || sourceTable.Columns.Count != colAliasNames.Length)
            {
                throw new ArgumentException("列名重命名数组与DataTable列集合不匹配。", "colAliasNames");
            }
            //判断是否为兼容模式
            bool isCompatible = CommonBase.GetIsCompatible(filePath);
            //创建工作铺
            IWorkbook workbook = CommonBase.CreateWorkbook(isCompatible);
            ICellStyle headerCellStyle = CommonBase.GetCellStyle(workbook, colorTrue, colorIndex);
            Dictionary<int, ICellStyle> colStyles = new Dictionary<int, ICellStyle>();
            ISheet sheet = workbook.CreateSheet(sheetName);
            IRow headerRow = sheet.CreateRow(0);
            // 处理表头
            foreach (DataColumn column in sourceTable.Columns)
            {
                ICell headerCell = headerRow.CreateCell(column.Ordinal);
                headerCell.SetCellValue(colAliasNames[column.Ordinal]);
                headerCell.CellStyle = headerCellStyle;
                sheet.AutoSizeColumn(headerCell.ColumnIndex);
                if (colDataFormats != null && colDataFormats.ContainsKey(column.ColumnName))
                {
                    colStyles[headerCell.ColumnIndex] = CommonBase.GetCellStyleWithDataFormat(workbook, colDataFormats[column.ColumnName]);
                }
                else
                {
                    colStyles[headerCell.ColumnIndex] = CommonBase.GetCellStyle(workbook);
                }
            }

            // 初始的行数
            int rowIndex = 1;
            //填充数据
            foreach (DataRow row in sourceTable.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);

                foreach (DataColumn column in sourceTable.Columns)
                {
                    ICell cell = dataRow.CreateCell(column.Ordinal);
                    CommonBase.SetCellValue(cell, (row[column] ?? "").ToString(), column.DataType, colStyles);
                    CommonBase.ReSizeColumnWidth(sheet, cell);
                }

                rowIndex++;
            }
            sheet.ForceFormulaRecalculation = true;
            //写入文件
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                workbook.Write(fs);
                sheet = null;
                headerRow = null;
                workbook = null;
            }
            return filePath;
        }

        /// <summary>
        /// 由DataTable导出Excel
        /// </summary>
        /// <param name="sourceTable">要导出数据的DataTable</param>
        /// <param name="sheetName">工作薄名称，可选</param>
        /// <param name="filePath">导出路径</param>
        /// <param name="colNames">需要导出的列名,可选</param>
        /// <param name="colAliasNames">导出的列名重命名，可选</param>
        /// <param name="colDataFormats">列格式化集合，可选</param>
        /// <param name="sheetSize">指定每个工作薄显示的记录数，可选（不指定或指定小于0，则表示只生成一个工作薄）</param>
        /// <param name="colorIndex">表头颜色的样式  列：NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index</param>
        /// <param name="colorTrue">是否显示表头颜色 默认 是</param>
        /// <returns></returns>
        public static string ToExcel(this DataTable sourceTable, string sheetName = "result", string filePath = null, string[] colNames = null, IDictionary<string, string> colAliasNames = null, IDictionary<string, string> colDataFormats = null, int sheetSize = 0, bool colorTrue = true, short colorIndex = 22)
        {
            if (sourceTable.Rows.Count <= 0) return null;

            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("导出地址不能为空");
            }

            if (string.IsNullOrEmpty(filePath)) return null;

            bool isCompatible = CommonBase.GetIsCompatible(filePath);

            IWorkbook workbook = CommonBase.CreateWorkbook(isCompatible);
            ICellStyle headerCellStyle = CommonBase.GetCellStyle(workbook, colorTrue, colorIndex);
            //需要导出的列头
            if (colNames == null || colNames.Length <= 0)
            {
                colNames = sourceTable.Columns.Cast<DataColumn>().OrderBy(c => c.Ordinal).Select(c => c.ColumnName).ToArray();
            }
            IEnumerable<DataRow> batchDataRows, dataRows = sourceTable.Rows.Cast<DataRow>();
            int sheetCount = 0;
            if (sheetSize <= 0)
            {
                sheetSize = sourceTable.Rows.Count;
            }
            while ((batchDataRows = dataRows.Take(sheetSize)).Count() > 0)
            {

                Dictionary<int, ICellStyle> colStyles = new Dictionary<int, ICellStyle>();

                ISheet sheet = workbook.CreateSheet(sheetName + (++sheetCount).ToString());
                IRow headerRow = sheet.CreateRow(0);

                // 表头
                for (int i = 0; i < colNames.Length; i++)
                {
                    ICell headerCell = headerRow.CreateCell(i);
                    if (colAliasNames != null && colAliasNames.ContainsKey(colNames[i]))
                    {
                        headerCell.SetCellValue(colAliasNames[colNames[i]]);
                    }
                    else
                    {
                        headerCell.SetCellValue(colNames[i]);
                    }
                    headerCell.CellStyle = headerCellStyle;
                    sheet.AutoSizeColumn(headerCell.ColumnIndex);
                    if (colDataFormats != null && colDataFormats.ContainsKey(colNames[i]))
                    {
                        colStyles[headerCell.ColumnIndex] = CommonBase.GetCellStyleWithDataFormat(workbook, colDataFormats[colNames[i]]);
                    }
                    else
                    {
                        colStyles[headerCell.ColumnIndex] = CommonBase.GetCellStyle(workbook);
                    }
                }
                // 起始的行数
                int rowIndex = 1;
                //处理数据
                foreach (DataRow row in batchDataRows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);
                    for (int i = 0; i < colNames.Length; i++)
                    {
                        ICell cell = dataRow.CreateCell(i);
                        CommonBase.SetCellValue(cell, (row[colNames[i]] ?? "").ToString(), sourceTable.Columns[colNames[i]].DataType, colStyles);
                        CommonBase.ReSizeColumnWidth(sheet, cell);
                    }
                    rowIndex++;
                }
                sheet.ForceFormulaRecalculation = true;
                dataRows = dataRows.Skip(sheetSize);
            }
            //写入文件
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                workbook.Write(fs);
            }
            workbook = null;
            return filePath;
        }
    }
}
