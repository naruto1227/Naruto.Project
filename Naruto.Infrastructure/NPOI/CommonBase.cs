
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
namespace Naruto.Infrastructure.NPOI
{
    /// <summary>
    /// Excel类库内部通用功能类
    /// 作者：ZhangHaiBo
    /// 日期：2016/1/15
    /// </summary>
    internal static class CommonBase
    {
        public static string DesktopDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        public static string TempDirectory = null;

        static CommonBase()
        {
            string temp = System.Environment.GetEnvironmentVariable("TEMP");
            DirectoryInfo info = new DirectoryInfo(temp);
            TempDirectory = info.FullName;
        }
        /// <summary>
        /// 判断是否为兼容模式
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool GetIsCompatible(string filePath)
        {
            string ext = Path.GetExtension(filePath);
            return new[] { ".xls", ".xlt" }.Count(e => e.Equals(ext, StringComparison.OrdinalIgnoreCase)) > 0;
        }



        /// <summary>
        /// 创建工作薄
        /// </summary>
        /// <param name="isCompatible"></param>
        /// <returns></returns>
        public static IWorkbook CreateWorkbook(bool isCompatible)
        {
            if (isCompatible)
            {
                return new HSSFWorkbook();
            }
            else
            {
                return new XSSFWorkbook();
            }
        }

        /// <summary>
        /// 创建工作薄(依据文件流)
        /// </summary>
        /// <param name="isCompatible"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static IWorkbook CreateWorkbook(bool isCompatible, dynamic stream)
        {
            if (isCompatible)
            {
                return new HSSFWorkbook(stream);
            }
            else
            {
                return new XSSFWorkbook(stream);
            }
        }

        /// <summary>
        /// 创建单元格样式
        /// </summary>
        /// <param name="workbook">workbook</param>
        /// <param name="isHeaderRow">是否获取头部样式</param>
        /// <param name="colorIndex">颜色的样式  列：NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index</param>
        /// <returns></returns>
        public static ICellStyle GetCellStyle(IWorkbook workbook, bool isHeaderRow = false,short colorIndex=22)
        {
            ICellStyle style = workbook.CreateCellStyle();
            if (isHeaderRow)
            {
                style.FillPattern = FillPattern.SolidForeground; ;
                style.FillForegroundColor = colorIndex;
            }
            style.RightBorderColor = HSSFColor.Black.Index;
            style.Alignment = HorizontalAlignment.Center;
            //设置垂直居中
            style.VerticalAlignment = VerticalAlignment.Center;
            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            return style;
        }


        /// <summary>
        /// 根据单元格内容重新设置列宽
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="cell"></param>
        public static void ReSizeColumnWidth(ISheet sheet, ICell cell)
        {
            int cellLength = (Encoding.Default.GetBytes(cell.ToString()).Length + 2) * 256;
            const int maxLength = 60 * 256; //255 * 256;
            if (cellLength > maxLength) //当单元格内容超过30个中文字符（英语60个字符）宽度，则强制换行
            {
                cellLength = maxLength;
                cell.CellStyle.WrapText = true;
            }
            int colWidth = sheet.GetColumnWidth(cell.ColumnIndex);
            if (colWidth < cellLength)
            {
                sheet.SetColumnWidth(cell.ColumnIndex, cellLength);
            }
        }

        /// <summary>
        /// 创建单元格样式并设置数据格式化规则
        /// </summary>
        /// <param name="workbook">workbook</param>
        /// <param name="format">格式化字符串</param>
        public static ICellStyle GetCellStyleWithDataFormat(IWorkbook workbook, string format)
        {
            var style = GetCellStyle(workbook);
            var dataFormat = workbook.CreateDataFormat();
            short formatId = -1;
            if (dataFormat is HSSFDataFormat)
            {
                formatId = HSSFDataFormat.GetBuiltinFormat(format);
            }
            if (formatId != -1)
            {
                style.DataFormat = formatId;
            }
            else
            {
                style.DataFormat = dataFormat.GetFormat(format);
            }
            return style;
        }

        /// <summary>
        /// 依据值类型为单元格设置值
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="value"></param>
        /// <param name="colType"></param>
        /// <param name="colStyles"></param>
        public static void SetCellValue(ICell cell, string value, Type colType, IDictionary<int, ICellStyle> colStyles)
        {
            string dataFormatStr = null;
            switch (colType.ToString())
            {
                case "System.String": //字符串类型
                    cell.SetCellType(CellType.String);
                    cell.SetCellValue(value);
                    break;
                case "System.DateTime": //日期类型

                    if (DateTime.TryParse(value, out var dateV))
                    {
                        cell.SetCellValue(dateV);
                    }
                    dataFormatStr = "yyyy/mm/dd hh:mm:ss";
                    break;
                case "System.Boolean": //布尔型
                    if (bool.TryParse(value, out var boolV))
                    {
                        cell.SetCellType(CellType.Boolean);
                        cell.SetCellValue(boolV);
                    }
                    break;
                case "System.Int16": //整型
                    cell.SetCellType(CellType.String);
                    cell.SetCellValue(value);
                    break;
                case "System.Int32":
                    cell.SetCellType(CellType.String);
                    cell.SetCellValue(value);
                    break;
                case "System.Int64":
                    cell.SetCellType(CellType.String);
                    cell.SetCellValue(value);
                    break;
                case "System.Byte":
                    if (int.TryParse(value, out var intV))
                    {
                        cell.SetCellType(CellType.Numeric);
                        cell.SetCellValue(intV);
                    }
                    dataFormatStr = "0";
                    break;
                case "System.Decimal": //浮点型

                    if (double.TryParse(value, out var dec))
                    {
                        cell.SetCellType(CellType.Numeric);
                        cell.SetCellValue(dec);
                    }
                    dataFormatStr = "0.0000";
                    break;
                case "System.Double":
                    if (double.TryParse(value, out var doubV))
                    {
                        cell.SetCellType(CellType.Numeric);
                        cell.SetCellValue(doubV);
                    }
                    dataFormatStr = "0.00";
                    break;
                case "System.DBNull": //空值处理
                    cell.SetCellType(CellType.Blank);
                    cell.SetCellValue("");
                    break;
                default:
                    cell.SetCellType(CellType.String);
                    cell.SetCellValue(value);
                    break;
            }

            if (!string.IsNullOrEmpty(dataFormatStr) && colStyles[cell.ColumnIndex].DataFormat <= 0) //没有设置，则采用默认类型格式
            {
                colStyles[cell.ColumnIndex] = GetCellStyleWithDataFormat(cell.Sheet.Workbook, dataFormatStr);
            }
            cell.CellStyle = colStyles[cell.ColumnIndex];
        }


        /// <summary>
        /// 从工作表中生成DataTable
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="headerRowIndex"></param>
        /// <returns></returns>
        public static DataTable GetDataTableFromSheet(ISheet sheet, int headerRowIndex)
        {
            DataTable table = new DataTable();

            if (sheet.LastRowNum <= 0) return table;

            IRow headerRow = sheet.GetRow(headerRowIndex);
            int cellCount = headerRow.LastCellNum;

            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                if (headerRow.GetCell(i) == null || headerRow.GetCell(i).StringCellValue.Trim() == "")
                {
                    // 如果遇到第一个空列，则不再继续向后读取
                    cellCount = i;
                    break;
                }
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }

            for (int i = (headerRowIndex + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                //如果遇到某行的第一个单元格的值为空，则不再继续向下读取
                if (row != null && row.GetCell(0) != null && !string.IsNullOrEmpty(row.GetCell(0).ToString()))
                {
                    DataRow dataRow = table.NewRow();

                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        ICell cell = row.GetCell(j);
                        dataRow[j] = cell == null ? "" : cell.ToString();
                    }
                    table.Rows.Add(dataRow);
                }
            }

            return table;
        }



        /// <summary>
        /// 从工作薄中获取指定范围的图形对象列表
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rangeMinRow"></param>
        /// <param name="rangeMaxRow"></param>
        /// <param name="rangeMinCol"></param>
        /// <param name="rangeMaxCol"></param>
        /// <returns></returns>
        public static List<object> GetShapesFromSheet(ISheet sheet, int? rangeMinRow = null, int? rangeMaxRow = null, int? rangeMinCol = null, int? rangeMaxCol = null)
        {
            List<object> shapeAllList = new List<object>();
            var shapeContainer = sheet.DrawingPatriarch;
            if (sheet is HSSFSheet)
            {
                var shapeContainerHSSF = sheet.DrawingPatriarch as HSSFShapeContainer;
                if (null != shapeContainer)
                {
                    var shapeList = shapeContainerHSSF.Children;
                    foreach (var shape in shapeList)
                    {
                        if (shape is HSSFShape && shape.Anchor is HSSFClientAnchor)
                        {
                            var anchor = shape.Anchor as HSSFClientAnchor;
                            if (IsInternalOrIntersect(rangeMinRow, rangeMaxRow, rangeMinCol, rangeMaxCol, anchor.Row1, anchor.Row2, anchor.Col1, anchor.Col2, true))
                            {
                                shapeAllList.Add(shape);
                            }
                        }
                    }
                }
            }
            else
            {
                var documentPartList = (sheet as XSSFSheet).GetRelations();
                foreach (var documentPart in documentPartList)
                {
                    if (documentPart is XSSFDrawing)
                    {
                        var drawing = (XSSFDrawing)documentPart;
                        var shapeList = drawing.GetShapes();
                        foreach (var shape in shapeList)
                        {
                            var anchorResult = shape.GetAnchor();
                            if (shape is XSSFShape && anchorResult is XSSFClientAnchor)
                            {
                                var anchor = anchorResult as XSSFClientAnchor;
                                if (IsInternalOrIntersect(rangeMinRow, rangeMaxRow, rangeMinCol, rangeMaxCol, anchor.Row1, anchor.Row2, anchor.Col1, anchor.Col2, true))
                                {
                                    shapeAllList.Add(shape);
                                }
                            }
                        }
                    }
                }
            }

            return shapeAllList;
        }


        /// <summary>
        /// 创建临时模板文件
        /// </summary>
        /// <param name="templatePath"></param>
        /// <param name="sheetName"></param>
        /// <param name="sheetCount"></param>
        /// <returns></returns>
        public static string CreateTempFileByTemplate(string templatePath, string sheetName, int sheetCount)
        {
            using (var fs = File.OpenRead(templatePath))
            {
                bool isCompatible = GetIsCompatible(templatePath);
                var workbook = CreateWorkbook(isCompatible, fs);
                var sheet = workbook.GetSheet(sheetName);

                CloneOneToManySheet(workbook, sheet, sheetCount);

                string tempTemplatePath = TempDirectory + string.Format("\\{0}-{1}-{2}", Guid.NewGuid().ToString("N"), sheetCount, Path.GetFileName(templatePath));

                using (var fs2 = File.Create(tempTemplatePath))
                {
                    workbook.Write(fs2);
                }

                return tempTemplatePath;
            }

        }

        /// <summary>
        /// 删除临时模板文件
        /// </summary>
        /// <param name="temp_templatePath"></param>
        public static void DeleteTempTemplateFile(string temp_templatePath)
        {
            if (!string.IsNullOrEmpty(temp_templatePath) && File.Exists(temp_templatePath))
            {
                File.Delete(temp_templatePath);
            }
            string temp_templateConfigFilePath = Path.ChangeExtension(temp_templatePath, ".xml");
            if (File.Exists(temp_templateConfigFilePath))
            {
                File.Delete(temp_templateConfigFilePath);
            }
        }


        /// <summary>
        /// 将一个工作薄克隆出多个工作薄
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="sheet"></param>
        /// <param name="sheetCount"></param>
        public static void CloneOneToManySheet(IWorkbook workbook, ISheet sheet, int sheetCount)
        {
            int sheetIndex = workbook.GetSheetIndex(sheet);
            for (int i = 1; i <= sheetCount - 1; i++)
            {
                workbook.CloneSheet(sheetIndex);
                workbook.SetSheetName(sheetIndex + i, sheet.SheetName + (i + 1).ToString());
            }
        }

        /// <summary>
        /// 判断是否在指定的区域范围内
        /// </summary>
        /// <param name="rangeMinRow"></param>
        /// <param name="rangeMaxRow"></param>
        /// <param name="rangeMinCol"></param>
        /// <param name="rangeMaxCol"></param>
        /// <param name="pictureMinRow"></param>
        /// <param name="pictureMaxRow"></param>
        /// <param name="pictureMinCol"></param>
        /// <param name="pictureMaxCol"></param>
        /// <param name="onlyInternal"></param>
        /// <returns></returns>

        private static bool IsInternalOrIntersect(int? rangeMinRow, int? rangeMaxRow, int? rangeMinCol, int? rangeMaxCol,
                                                int pictureMinRow, int pictureMaxRow, int pictureMinCol, int pictureMaxCol, bool onlyInternal)
        {
            int _rangeMinRow = rangeMinRow ?? pictureMinRow;
            int _rangeMaxRow = rangeMaxRow ?? pictureMaxRow;
            int _rangeMinCol = rangeMinCol ?? pictureMinCol;
            int _rangeMaxCol = rangeMaxCol ?? pictureMaxCol;

            if (onlyInternal)
            {
                return (_rangeMinRow <= pictureMinRow && _rangeMaxRow >= pictureMaxRow &&
                        _rangeMinCol <= pictureMinCol && _rangeMaxCol >= pictureMaxCol);
            }
            else
            {
                return ((Math.Abs(_rangeMaxRow - _rangeMinRow) + Math.Abs(pictureMaxRow - pictureMinRow) >= Math.Abs(_rangeMaxRow + _rangeMinRow - pictureMaxRow - pictureMinRow)) &&
                (Math.Abs(_rangeMaxCol - _rangeMinCol) + Math.Abs(pictureMaxCol - pictureMinCol) >= Math.Abs(_rangeMaxCol + _rangeMinCol - pictureMaxCol - pictureMinCol)));
            }
        }

    }
}
