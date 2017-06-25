using Mars.Server.BLL;
using Mars.Server.Entity;
using Mars.Server.Sevice.BaseHandler;
using Mars.Server.Utils;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using ThoughtWorks.QRCode.Codec;
namespace Mars.Server.Sevice.Controller
{
    [AjaxController]
    public class SignBookController : BaseController
    {
        [AjaxHandlerAction]
        public string Import(HttpContext context)//导入数据
        {
            try
            {   
                UploadImageHelper uploadBll = new UploadImageHelper();
                context.Response.ContentType = "text/plain";
                HttpPostedFile file = context.Request.Files["Filedata"];
                string fileName = file.FileName;
                //string tempPath = AppDomain.CurrentDomain.BaseDirectory + "/UploadFile/" + "SignBook/";  //设置二维码临时位置
                string tempPath = HttpContext.Current.Server.MapPath("~/UploadFile/SignBook/");
                //创建目录  
                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }

                fileName = System.IO.Path.GetFileName(fileName); //获取文件名（不带路径）
                string currFileExtension = System.IO.Path.GetExtension(fileName);//获取文件的扩展名
                string currFilePath = tempPath + fileName;       //获取上传后的文件路径 记录到前面声明的全局变量
                file.SaveAs(currFilePath);                       //保存临时文件
                DataTable dt = ExcelToDataTable(fileName, currFilePath);
                for (int r = dt.Rows.Count - 1; r >= 0; r--)
                {
                     DataRow row = dt.Rows[r];
                    if (row["姓名"].ToString().Trim() == "")
                    {
                        row.Delete();
                    }
                }
                dt.AcceptChanges();

                //查询数据库中电话是否存在
                string existMoblie = string.Empty;
                IList<SignBookEntity> entityList = BCtrl_SignBook.Instance.SignBook_GetALL();

                foreach (var item in entityList)
                {
                    string moblie = item.Moblie;
                    foreach (DataRow item2 in dt.Rows)
                    {
                        string moblie2 = item2["手机"].ToString();

                        if (moblie.Trim() == moblie2.Trim())
                        {
                            existMoblie += moblie + ",";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(existMoblie))
                {
                    if (existMoblie.EndsWith(","))
                    {
                        existMoblie = existMoblie.Substring(0, existMoblie.Length - 1);
                        return "false_" + existMoblie;
                    }
                }
                //入库
                foreach (DataRow item in dt.Rows)
                {
                    SignBookEntity entity = new SignBookEntity();
                    entity.Customer = item["姓名"].ToString().Trim();
                    entity.Moblie = item["手机"].ToString().Trim().Replace(Environment.NewLine,"");
                    entity.Moblie = entity.Moblie.Replace("\n", "").Replace("\r","");
                    entity.Company = item["单位"].ToString();
                    entity.Department = item["部门"].ToString();
                    entity.Position = item["职位"].ToString();
                    entity.Email = item["邮箱"].ToString();
                    entity.SalesName = item["业务对接人"].ToString();
                    entity.SignURL = item["部门"].ToString();
                    entity.CustomerKey = MD5.Fun_MD5(entity.Moblie);
                    entity.LuckyNumber = GetLuckyNumber();
                    entity.CreateTime = DateTime.Now;
                    entity.IsSign = 0;

                    //二维码
                    string strName = Guid.NewGuid().ToString("N") + ".png";
                    Bitmap image = Create_ImgCode(entity.CustomerKey, 15);
                    SaveImg(strName, tempPath, image);

                    //七牛
                    uploadBll.UpLoadImage(tempPath + "/" + strName, out  strName, true);
                    entity.SignURL = uploadBll.QiniuDomain + strName;
                    BCtrl_SignBook.Instance.SignBook_Insert(entity);
                }
                File.Delete(currFilePath);
                return "true";
            }
            catch
            {
                return "false";
            }
           
        }

        [AjaxHandlerAction]
        public string UpdateState(HttpContext context) //更新用户状态
        {
            string sid = context.Request.Form["signID"];
            int signID = 0;
            int.TryParse(sid, out signID);
            string msg = string.Empty;
            IList<SignBookEntity> entity = BCtrl_SignBook.Instance.SignBook_Get(signID);
            if (entity == null || entity.Count ==0)
            {
                return "false_没有找到签到数据";
            }
            if (entity[0].IsSign == 0)
            {
                entity[0].IsSign = 1;
                msg = "_已将客户"+entity[0].Customer +"修改至已签到！";

            }
            else
            {
                entity[0].IsSign = 0;
                msg = "_已将客户" + entity[0].Customer + "修改至未签到！";
            }
            bool state= BCtrl_SignBook.Instance.SignBook_Update(entity[0].CustomerKey, entity[0].IsSign);

            if (state)
            {
                return "true"+msg;
            }
            else 
            {
                return "false";
            }
        }

        [AjaxHandlerAction]
        public string Delete(HttpContext context) //更新用户状态
        {
            string sid = context.Request.Form["signID"];
            int signID = 0;
            int.TryParse(sid, out signID);
            IList<SignBookEntity> entity = BCtrl_SignBook.Instance.SignBook_Get(signID);
            if (entity == null || entity.Count == 0)
            {
                return "false_没有找到签到数据";
            }
            bool state = BCtrl_SignBook.Instance.SignBook_Delete(signID);
            if (state)
            {
                return "true";
            }
            else 
            {
                return "false";
            }
        }

        [AjaxHandlerAction]
        public string Export(HttpContext context) //导出数据
        {
           string strType = context.Request.Form["selType"];
           string strValue = context.Request.Form["selValue"];
            //string tempPath = AppDomain.CurrentDomain.BaseDirectory + "/UploadFile/" + "SignBook";
           string tempPath = HttpContext.Current.Server.MapPath("~/UploadFile/SignBook/");
           string fileName = DateTime.Now.ToString("yyyyMMddHHmmss")+".xls";
           DataTable dt = new DataTable();
           string strWhere = string.Empty;
          
           //当前页查询所有名单
           switch (Convert.ToInt32(strType))
           {
                case 1: strWhere = " AND Customer like '%" + strValue + "%'  "; break;
                case 2: strWhere = " AND Company like '%" + strValue + "%'  "; break;
                case 3: strWhere = " AND Moblie like '" + strValue + "%'"; break;
                case 4: strWhere = " AND SalesName like '" + strValue + "%'   "; break;
                case 5: strWhere = " AND IsSign =1   "; break;
                case 6: strWhere = " AND IsSign =0   "; break;
           }  
           IList<SignBookEntity> entity = BCtrl_SignBook.Instance.SignBook_GetALL(strWhere);
           if (entity == null || entity.Count == 0)
           {
               return "false";
           }
           string[] parms = { "SignID", "Customer", "Moblie", "Company", "Department", "Position", "Email", "SalesName", "LuckyNumber", "IsSign", "IsRegister" };

           dt = ToDataTable<SignBookEntity>(entity, parms);
           try
           {
               using (MemoryStream ms = DataTableToExcel(dt, tempPath))
               {
                   lock (ms)
                   {
                       using (FileStream fs = new FileStream(tempPath + "/" + fileName, FileMode.Create, FileAccess.Write))
                       {
                           byte[] data = ms.ToArray();
                           fs.Write(data, 0, data.Length);
                           fs.Flush();
                       }
                   }
               }
           }
           catch (Exception ex)
           {
               LogUtil.WriteLog(ex);
           }
           return "true_" + fileName;
        }

        [AjaxHandlerAction]
        public string GetInfo(HttpContext context) //返回客户信息
        {
            string sid=context.Request.Form["sid"];
            int signID=0;
            int.TryParse(sid,out signID);
            IList<SignBookEntity> entity=BCtrl_SignBook.Instance.SignBook_Get(signID);
            if (entity == null || entity.Count == 0)
            {
                return "false";
            }
            string json = JsonObj<SignBookEntity>.ToJson(entity[0]);
            return json;
        }

        [AjaxHandlerAction]
        public string GetCount(HttpContext context) //返回总数据及总签到数
        {
            int allCount= BCtrl_SignBook.Instance.SignBook_Count();
            int signCount = BCtrl_SignBook.Instance.SignBook_Count(" AND IsSign=1 ");
            return "报名总数: " + allCount + " 位 、" +"已签到客户：" +signCount +" 位" ;
        }

        [AjaxHandlerAction]
        public string Update(HttpContext context)  //更新用户数据
        {
            string sid = context.Request.Form["sid"];
            string customer = context.Request.Form["customer"];
            string moblie = context.Request.Form["moblie"];
            string commany = context.Request.Form["commany"];
            string departement = context.Request.Form["departement"];
            string position = context.Request.Form["position"];
            string email = context.Request.Form["email"];
            int signID = 0;
            int.TryParse(sid, out signID);
            string msg = string.Empty;
            IList<SignBookEntity> entity = BCtrl_SignBook.Instance.SignBook_Get(signID);
            if (entity == null || entity.Count == 0)
            {
                return "false_没有找到签到数据！";
            }
            else 
            {
                entity[0].Department = departement;
                entity[0].Position = position;
                entity[0].Email = email;
                entity[0].Company = commany;
                bool state= BCtrl_SignBook.Instance.SignBook_Update(entity[0]);
                if (state)
                {
                    return "true_更新成功！";
                }
                else 
                {
                    return "false_更新失败！";
                }
            }
        }

        /// <summary>
        ///  已存在的excel 转成 table
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="currFilePath">文件路径</param>
        /// <returns></returns>
        private DataTable ExcelToDataTable(string fileName,string currFilePath)
        {
            DataTable dt = new DataTable();
            IWorkbook wk = null;
            FileStream fs = File.OpenRead(currFilePath);
            if (fileName.IndexOf(".xlsx") != -1)
            {
                //把xlsx文件中的数据写入wk中
                wk = new XSSFWorkbook(fs);
            }
            else
            {
                //把xls文件中的数据写入wk中
                wk = new HSSFWorkbook(fs);
            }
            fs.Close();

            //读取当前表数据
            ISheet sheet = wk.GetSheetAt(0);
            if (sheet != null)
            {
                IRow firstRow = sheet.GetRow(0);
                int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                {
                    ICell cell = firstRow.GetCell(i);
                    if (cell != null)
                    {
                        string cellValue = cell.StringCellValue;
                        if (cellValue != null)
                        {
                            DataColumn column = new DataColumn(cellValue.Trim());
                            dt.Columns.Add(column);
                        }
                    }
                }
                int rowCount = sheet.LastRowNum;
                int startRow = sheet.FirstRowNum + 1;
                for (int i = startRow; i <= rowCount; ++i)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue; //没有数据的行默认是null　　　　　　　

                    DataRow dataRow = dt.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; ++j)
                    {
                        if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                            dataRow[j] = row.GetCell(j).ToString();
                    }
                    dt.Rows.Add(dataRow);
                }
            }
            return dt;
        }

        /// <summary>
        /// 用户随机数
        /// </summary>
        /// <returns>001-999</returns>
        private string GetLuckyNumber() 
        {
            string numStr = string.Empty;
            while (true) 
            {
                Random rd = new Random();
                int num= rd.Next(1, 999);

                numStr = num.ToString();
                switch (numStr.Length) 
                {
                    case 1: numStr = "00" + numStr; break;
                    case 2: numStr = "0" + numStr; break;
                }
                bool isExist = BCtrl_SignBook.Instance.IsExistLuckyNumber(numStr);
                if (!isExist)
                {
                    break;
                }
            }
            return numStr;
        }

        /// <summary>  
        /// 生成二维码图片  
        /// </summary>  
        /// <param name="codeNumber">要生成二维码的字符串</param>       
        /// <param name="size">大小尺寸</param>  
        /// <returns>二维码图片</returns>  
        private Bitmap Create_ImgCode(string codeNumber, int size)
        {
            //创建二维码生成类  
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            //设置编码模式  
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            //设置编码测量度  
            qrCodeEncoder.QRCodeScale = size;
            //设置编码版本  
            qrCodeEncoder.QRCodeVersion = 3;
            //设置编码错误纠正  
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            //生成二维码图片  
            System.Drawing.Bitmap image = qrCodeEncoder.Encode(codeNumber, System.Text.Encoding.UTF8);
            return image;
        }

        /// <summary>  
        /// 保存图片  
        /// </summary>  
        /// <param name="strName">保存名</param>  
        /// <param name="strPath">保存路径</param>  
        /// <param name="img">图片</param>  
        private void SaveImg(string strName, string strPath, Bitmap img)
        {
            //保存图片到目录  
            if (Directory.Exists(strPath))
            {
                img.Save(strPath + "/" + strName, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        /// <summary>
        /// 导出客户签到数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileName"></param>
        private MemoryStream DataTableToExcel(DataTable dt, string fileName) 
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("客户签到表");

            //填充表头
            IRow dataRow = sheet.CreateRow(0);
            foreach (DataColumn column in dt.Columns)
            {
                string title = string.Empty;
                switch (column.ColumnName) 
                {
                    case "SignID": title = "报名序号"; break;
                    case "Customer": title = "姓名"; break;
                    case "Moblie": title = "手机"; break;
                    case "Company": title = "单位"; break;
                    case "Department": title = "部门"; break;
                    case "Position": title = "职位"; break;
                    case "Email": title = "邮箱"; break;
                    case "SalesName": title = "业务对接人"; break;
                    case "LuckyNumber": title = "参会编号"; break;
                    case "IsSign": title = "签到状态"; break;
                    case "IsRegister": title = "是否注册"; break;
                }
                if (string.IsNullOrEmpty(title))
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                }
                else 
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(title);
                }
               
            }
            //填充内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dataRow = sheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {

                    if (dt.Columns[j].ColumnName == "IsSign")
                    {
                        string state= dt.Rows[i]["IsSign"].ToString();
                        if (state == "1")
                        {
                            dataRow.CreateCell(j).SetCellValue("已签");
                        }
                        else  if(state == "0")
                        {
                            dataRow.CreateCell(j).SetCellValue("未签到");
                        }
                    }
                    else if (dt.Columns[j].ColumnName == "IsRegister")
                    {
                        string state = dt.Rows[i]["IsRegister"].ToString();
                        
                        if (state == "0")
                        {
                            dataRow.CreateCell(j).SetCellValue("未注册");
                        }else
                        {
                             dataRow.CreateCell(j).SetCellValue("已注册");
                        }
                    }
                    else
                    {
                        dataRow.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                    }
                }
            }
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                return ms;
            } 
        }

        /// <summary>
        /// 将泛型集合类转换成DataTable
        /// </summary>
        /// <typeparam name="T">集合项类型</typeparam>
        /// <param name="list">集合</param>
        /// <param name="propertyName">需要返回的列的列名</param>
        /// <returns>数据集(表)</returns>
        public static DataTable ToDataTable<T>(IList<T> list, params string[] propertyName)
        {
            List<string> propertyNameList = new List<string>();
            if (propertyName != null)
                propertyNameList.AddRange(propertyName);

            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    if (propertyNameList.Count == 0)
                    {
                        result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                    else
                    {
                        if (propertyNameList.Contains(pi.Name))
                            result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (propertyNameList.Count == 0)
                        {
                            object obj = pi.GetValue(list[i], null);
                            tempList.Add(obj);
                        }
                        else
                        {
                            if (propertyNameList.Contains(pi.Name))
                            {
                                object obj = pi.GetValue(list[i], null);
                                tempList.Add(obj);
                            }
                        }
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

        /// <summary>
        /// Json 字符串 转换为 DataTable数据集合
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public  DataTable JsonToDataTable(string json)
        {
            DataTable dataTable = new DataTable();  //实例化
            DataTable result;
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count<string>() == 0)
                        {
                            result = dataTable;
                            return result;
                        }
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                dataTable.Columns.Add(current, dictionary[current].GetType());
                            }
                        }
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            dataRow[current] = dictionary[current];
                        }

                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
                    }
                }
            }
            catch
            {
            }
            result = dataTable;
            return result;
        }
    }
}
    