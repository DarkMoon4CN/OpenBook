using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using OfficeOpenXml;
using System.Text.RegularExpressions;
using OfficeOpenXml.Style;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
namespace Mars.Server.Utils
{
    public class ExcelHelper:IDisposable
    {
        private string fileName = null; //文件名
        private IWorkbook workbook = null;
        private FileStream fs = null;
        private bool disposed;

        public ExcelHelper(string fileName)
        {
            this.fileName = fileName;
            disposed = false;
        }

        public ExcelHelper()
        {
        }

        private static string GetValueNeedToAdd(DataRow dr, string keys, out bool isnumberic)
        {
            isnumberic = false;
            try
            {

                if (keys.Contains("+"))
                {
                    string[] fileds = keys.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
                    int sum = 0;
                    for (int i = 0; i < fileds.Length; i++)
                    {
                        sum += dr[fileds[i]].ToString().ToInt();
                    }
                    isnumberic = true;
                    return sum > 0 ? sum.ToString() : "";
                }
                else if (keys.Contains("-"))
                {
                    string[] fileds = keys.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                    if (fileds.Length == 2)
                    {
                        isnumberic = true;
                        return (double.Parse(dr[fileds[0]].ToString()) - double.Parse(dr[fileds[1]].ToString())).ToString("f4");
                    }
                    else
                        return "";
                }
                else
                {
                    string value = dr[keys].ToString();
                    isnumberic = R_Num.Match(value).Success;
                    return value;
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return "";
            }

        }
        //@"^[\d\.]+$"
        private static Regex R_Num = new Regex(@"^\d+\.?\d*$", RegexOptions.Compiled);


        private static string GetValue(DataRow dr, string key, out bool isnumberic)
        {
            isnumberic = false;
            try
            {
                if (key.StartsWith("P_"))
                {
                    string[] parts = key.Replace("P_", "").Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2)
                    {
                        //return StringUti.Divide(dr[parts[0]].ToString(), dr[parts[1]].ToString(), true);
                        string data = StringUti.Divide(GetValueNeedToAdd(dr, parts[0], out isnumberic), GetValueNeedToAdd(dr, parts[1], out isnumberic), true);
                        isnumberic = false;
                        return data;
                    }
                    else
                        return string.Empty;
                }
                else if (key.StartsWith("F1_"))
                {
                    string[] parts = key.Replace("F1_", "").Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2)
                    {
                        isnumberic = true;
                        //return StringUti.Divide(dr[parts[0]].ToString(), dr[parts[1]].ToString(), false);
                        return StringUti.Divide(GetValueNeedToAdd(dr, parts[0], out isnumberic), GetValueNeedToAdd(dr, parts[1], out isnumberic), false);
                    }
                    else
                        return string.Empty;
                }
                else
                {
                    string rate = GetValueNeedToAdd(dr, key, out isnumberic);
                    if (isnumberic && (key.ToLower().Contains("rate") || key.ToLower().Contains("contribute")) && !key.ToLower().Contains("onsale_stocksalerate"))
                    {
                        isnumberic = false;
                        return (double.Parse(rate) * 100).ToString("f2") + (key.Contains("-") ? "" : "%");
                    }
                    else
                    {
                        return isnumberic ? double.Parse(rate).ToString("f2") : rate;
                    }

                    //return GetValueNeedToAdd(dr, key, out isnumberic);
                }

            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return string.Empty;
            }


        }

        private static void WriteData(ref NPOI.SS.UserModel.ISheet sheet1, DataTable dt, Dictionary<string, string> columnInfo, string datadesc)
        {
            WriteData(ref sheet1, dt, columnInfo, datadesc, 0);
        }
        private static void WriteData(ref NPOI.SS.UserModel.ISheet sheet1, DataTable dt, Dictionary<string, string> columnInfo, string datadesc, int datastartrow)
        {
            //int datastartrow = 0;
            if (!string.IsNullOrEmpty(datadesc))
            {
                NPOI.SS.UserModel.IRow desc = sheet1.CreateRow(datastartrow);
                for (int i = 0; i < 10; i++)
                {
                    desc.CreateCell(i);
                }
                NPOI.SS.Util.CellRangeAddress address = new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 9);
                sheet1.AddMergedRegion(address);
                sheet1.GetRow(0).GetCell(0).SetCellValue(datadesc);
                datastartrow++;
            }

            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(datastartrow);
            int tick = 0;
            foreach (string value in columnInfo.Values)
            {
                row1.CreateCell(tick).SetCellValue(value);
                tick++;
            }

            //将数据逐步写入sheet1各个行
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + datastartrow + 1);
                int index = 0;
                foreach (string key in columnInfo.Keys)
                {
                    bool isnumberic = false;
                    if (key.ToLower() == "rowid")
                    {
                        rowtemp.CreateCell(index).SetCellValue(i + 1);
                    }
                    else
                    {
                        string data = GetValue(dt.Rows[i], key, out isnumberic);
                        if (isnumberic && data != "--")
                        {
                            rowtemp.CreateCell(index).SetCellValue(string.IsNullOrEmpty(data) ? 0 : double.Parse(data));
                        }
                        else
                        {
                            rowtemp.CreateCell(index).SetCellValue(data);
                        }
                    }
                    index++;
                }
            }
        }

        public static MemoryStream CreateExcel2003Stream(DataTable dt, Dictionary<string, string> columnInfo, string desc)
        {
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

            WriteData(ref sheet1, dt, columnInfo, desc);

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            return ms;
        }

        public static MemoryStream CreateExcel2003Stream(DataTable dt, Dictionary<string, string> columnInfo)
        {
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

            WriteData(ref sheet1, dt, columnInfo, string.Empty);

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            return ms;
        }
        public static MemoryStream CreateExcel2003StreamMulti(List<ExcelDataWapperEntity> dataentity)
        {
            return CreateExcel2003StreamMulti(dataentity, false);
            //NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //int tick = 1;
            //foreach (ExcelDataWapperEntity w in dataentity)
            //{
            //    NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet(w.DataSourceTitle ?? string.Format("Sheet{0}",tick));
            //    WriteData(ref sheet1, w.DataSource, w.ColumnInfo, w.DataSourceDesc);
            //}
            //System.IO.MemoryStream ms = new System.IO.MemoryStream();
            //book.Write(ms);
            //return ms;
        }
        public static MemoryStream CreateExcel2003StreamMulti(List<ExcelDataWapperEntity> dataentity, bool inOneSheet)
        {
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            int tick = 1;
            if (inOneSheet)
            {
                NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
                for (int i = 0; i < dataentity.Count; i++)
                {
                    int datastartrow = 0;
                    if (i > 0)
                    {
                        datastartrow = dataentity[i - 1].DataSource.Rows.Count + (string.IsNullOrEmpty(dataentity[i - 1].DataSourceDesc) ? 2 : 3);
                    }
                    WriteData(ref sheet1, dataentity[i].DataSource, dataentity[i].ColumnInfo, dataentity[i].DataSourceDesc, datastartrow);
                }
            }
            else
            {
                foreach (ExcelDataWapperEntity w in dataentity)
                {
                    NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet(w.DataSourceTitle ?? string.Format("Sheet{0}", tick));
                    WriteData(ref sheet1, w.DataSource, w.ColumnInfo, w.DataSourceDesc);
                }
            }
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            return ms;
        }
        public static byte[] CreateExcel2007Stream(DataTable dt, Dictionary<string, string> columnInfo)
        {
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet1");
            int tick = 1;
            foreach (string value in columnInfo.Values)
            {
                ws.SetValue(1, tick, value);
                tick++;
            }

            //将数据逐步写入sheet1各个行
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int index = 1;
                foreach (string key in columnInfo.Keys)
                {
                    bool isnumberic = false;
                    ws.SetValue(i + 2, index, (key.ToLower() == "rowid" ? (i + 1).ToString() : GetValue(dt.Rows[i], key, out isnumberic)));
                    index++;
                }
            }
            return pck.GetAsByteArray();
        }
        public static MemoryStream CreateZipCSVStream(DataTable dt, Dictionary<string, string> columnInfo, string message, string filename)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            using (TextWriter tw = new StreamWriter(ms, Encoding.GetEncoding("gb2312")))
            {
                using (CsvHelper.CsvWriter writer = new CsvHelper.CsvWriter(tw))
                {
                    if (!string.IsNullOrEmpty(message))
                    {
                        writer.WriteField(message);
                        writer.NextRecord();
                    }

                    foreach (string key in columnInfo.Values)
                    {
                        writer.WriteField(key);
                    }
                    writer.NextRecord();

                    int i = 0;
                    bool isnumberic = false;

                    foreach (DataRow dr in dt.Rows)
                    {
                        foreach (string key in columnInfo.Keys)
                        {
                            string data = key.ToLower() == "rowid" ? (i + 1).ToString() : GetValue(dr, key, out isnumberic);
                            Console.WriteLine(data);
                            writer.WriteField(data);
                        }
                        i++;
                        writer.NextRecord();
                    }
                    writer.NextRecord();
                    return CompressStream(ms, filename);
                }
            }
        }
        public static MemoryStream CompressStream(MemoryStream stream, string entryname)
        {
            MemoryStream ms = new MemoryStream();
            using (ICSharpCode.SharpZipLib.Zip.ZipOutputStream zipstream = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(ms))
            {
                ICSharpCode.SharpZipLib.Zip.ZipEntry entry = new ICSharpCode.SharpZipLib.Zip.ZipEntry(entryname);
                entry.DateTime = DateTime.Now;
                zipstream.PutNextEntry(entry);
                zipstream.Write(stream.ToArray(), 0, (int)stream.Length);
                zipstream.Flush();
                zipstream.Finish();
                zipstream.Close();
            }
            return ms;
        }

        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public DataTable ExcelToDataTable(string sheetName, bool isFirstRowColumn)
        {
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);

                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }

                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                //Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (fs != null)
                        fs.Close();
                }

                fs = null;
                disposed = true;
            }
        }
    }

    public class ExcelDataWapperEntity
    {
        private DataTable _DataSource;

        public DataTable DataSource
        {
            get { return _DataSource; }
            set { _DataSource = value; }
        }
        private Dictionary<string, string> _columnInfo;

        public Dictionary<string, string> ColumnInfo
        {
            get { return _columnInfo; }
            set { _columnInfo = value; }
        }
        private string _DataSourceDesc;

        public string DataSourceDesc
        {
            get { return _DataSourceDesc; }
            set { _DataSourceDesc = value; }
        }
        private string _DataSourceTitle;

        public string DataSourceTitle
        {
            get { return _DataSourceTitle; }
            set { _DataSourceTitle = value; }
        }
    }

    public class ExcelChartWapper : IDisposable
    {
        ExcelPackage package;
        ExcelWorksheet mainsheet;
        private int maxcols = 16;
        public ExcelChartWapper(string defaultsheetname = "Report")
        {
            package = new ExcelPackage();
            if (!string.IsNullOrEmpty(defaultsheetname))
            {
                mainsheet = package.Workbook.Worksheets.Add(defaultsheetname);
            }

        }

        public MemoryStream Render(int bottomrow)
        {
            if (mainsheet != null)
            {
                mainsheet.PrinterSettings.PrintArea = mainsheet.Cells[1, 1, bottomrow + 1, maxcols];
                mainsheet.PrinterSettings.FitToPage = true;
                mainsheet.View.ShowGridLines = false;
                mainsheet.View.PageBreakView = true;
            }

            MemoryStream ms = new MemoryStream();
            package.SaveAs(ms);
            return ms;
            // package.Save();
        }

        public void AddDataSheet(string sheetname, DataTable datasource, dynamic tag, Action<ExcelWorksheet, DataTable, dynamic> WriteDataFunc)
        {
            ExcelWorksheet datasheet = package.Workbook.Worksheets.Add(sheetname);
            WriteDataFunc(datasheet, datasource, tag);
            //holder.FillAndDrawData(holder.DataSource, datasheet, mainsheet, holder.FirstRow);
        }

        public void Dispose()
        {
            if (package != null)
            {
                package.Dispose();
            }
        }
    }
    public class ExcelDataHolder
    {
        public int FirstRow { get; set; }
        public List<dynamic> DataSource { get; set; }
        public DataSet DataSource2 { get; set; }
        public string TItle { get; set; }
        public string Content { get; set; }

        public Action<List<dynamic>, ExcelWorksheet, ExcelWorksheet, int> FillAndDrawData { get; set; }

        public Func<DataSet, ExcelWorksheet, ExcelWorksheet, int, int> FillAndDrawData2 { get; set; }
    }
}
