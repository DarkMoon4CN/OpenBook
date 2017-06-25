
using Mars.Server.BLL;
using Mars.Server.Entity;
using Mars.Server.Sevice.BaseHandler;
using Mars.Server.Utils;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Mars.Server.Sevice.Controller
{
    [AjaxController]
    public class SMSController : BaseController
    {
        [AjaxHandlerAction]
        public string Import(HttpContext context)//导入数据
        {
            try
            {
                context.Response.ContentType = "text/plain";
                HttpPostedFile file = context.Request.Files["Filedata"];
                string suID = context.Request.Form["sUserID"];
                string customer = string.Empty;//用于输出第一个客户的姓名
                int sysUserID = 0;
                int.TryParse(suID, out sysUserID);
                string fileName = file.FileName;
                //string tempPath = AppDomain.CurrentDomain.BaseDirectory + "/UploadFile/" + "SMS/";  //设置短消息临时文件夹
                string tempPath = HttpContext.Current.Server.MapPath("~/UploadFile/SMS/");
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
                    if (row["手机"].ToString().Trim() == "")
                    {
                        row.Delete();
                    }
                }
                dt.AcceptChanges();

                if (dt.Rows.Count > 0)
                {
                   customer= dt.Rows[0]["姓名"].ToString();
                }

                //清理上次操作未发送的数据
                BCtrl_SMS.Instance.SMS_Delete(sysUserID, 0, true);
                
                //入库
                string strWhere = "AND  IsSend=0 AND SendTime IS NULL AND  Content IS NULL  AND SysUserID={0} AND Moblie='{1}' ";
                foreach (DataRow item in dt.Rows)
                {
                    SMSEntity entity = new SMSEntity();
                    entity.Customer = item["姓名"].ToString();
                    entity.Moblie = item["手机"].ToString();
                    entity.IsSend = 0;
                    entity.SendTime =null;
                    entity.SysUserID = sysUserID;

                    //设定此用户还在发送状态，不写入库
                    IList<SMSEntity>  entityList=BCtrl_SMS.Instance.SMS_GetALL(string.Format(strWhere, sysUserID, entity.Moblie));
                    if (entityList == null || entityList.Count == 0)
                    {
                        BCtrl_SMS.Instance.SMS_Insert(entity);
                    }
                }
                File.Delete(currFilePath);
                return "true_"+customer;
            }
            catch 
            {
                return "false";
            }
        }
        [AjaxHandlerAction]
        public string Add(HttpContext context) //发送短消息 send
        {
            try
            {
                string isExcel = context.Request.Form["isExcel"];
                string txtPhone = context.Request.Form["txtPhone"];
                string txtCustomer = context.Request.Form["txtCustomer"];
                string txtContent = context.Request.Form["txtContent"];
                string sysUserID= context.Request.Form["sUserID"];
                string modelKey = context.Request.Form["modelKey"];
                string sendContent = string.Empty;
                int sysID = 0;
                int.TryParse(sysUserID,out sysID);
                if (string.IsNullOrEmpty(txtContent))
                {
                    return "false";
                }

                //获取和设置消息模板
                OperationResult<IList<SMSModeEntity>> result =
                              BCtrl_SignSMSModel.Instance.SignSMSModel_GetWhere(" AND ModelKey='" + modelKey + "' ");
                if (result.ResultType == OperationResultType.Success)
                {
                    IList<SMSModeEntity> entitys = result.AppendData;
                    if (entitys != null && entitys.Count != 0)
                    {
                        sendContent = entitys[0].ModelContent;
                    }
                    else 
                    {
                        return "false";
                    }
                }
               
                if (isExcel == "0") //如果是单条信息
                {
                    SMSEntity entity = new SMSEntity();
                    entity.Customer = txtCustomer;
                    entity.Moblie = txtPhone;
                    entity.IsSend = 1;
                    entity.SysUserID = sysID;
                    entity.Content = txtContent.Trim();
                    entity.SendTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    sendContent = sendContent.Replace("{1}", entity.Customer);
                    sendContent = sendContent.Replace("{2}", entity.Content);
                    bool isSend = SmsMananger.SendContent(entity.Moblie, modelKey, entity.Customer, entity.Content);
                    entity.Content = sendContent;
                    if (isSend)
                    {
                        BCtrl_SMS.Instance.SMS_Insert(entity);
                    }
                }
                else
                {
                    //查询出所有需要发送的数据
                    string strWhere = string.Empty;
                    strWhere = " AND  IsSend=0 AND SendTime IS NULL AND  Content IS NULL  AND SysUserID= " + sysUserID + " ";
                    IList<SMSEntity> entityList = BCtrl_SMS.Instance.SMS_GetALL(strWhere);
                    for (int i = 0; i < entityList.Count; i++)
                    {
                        //调用短信接口
                        string saveSend = sendContent.Replace("{1}", entityList[i].Customer);
                        saveSend = saveSend.Replace("{2}", txtContent.Trim());
                        bool isSend = SmsMananger.SendContent(entityList[i].Moblie, modelKey, entityList[i].Customer, txtContent.Trim());
                        entityList[i].Content = saveSend;
                        if (isSend)
                        {
                            //改变据库发送状态
                            BCtrl_SMS.Instance.SMS_Update(entityList[i].Content, 1, modelKey, entityList[i].SmsID);
                        }
                        else
                        {
                            BCtrl_SMS.Instance.SMS_Update(entityList[i].Content, 0, modelKey, entityList[i].SmsID);
                        }
                    }
                }
                return "true";
            }
            catch
            {
                return "false";
            }
        }

        [AjaxHandlerAction]
        public string Delete(HttpContext context) //更新用户状态
        {
            string sid = context.Request.Form["smsID"];
            int smsID = 0;
            int.TryParse(sid, out smsID);
            IList<SMSEntity> entity = BCtrl_SMS.Instance.SMS_GetALL("AND SmsID= " + smsID + " ");
            if (entity == null || entity.Count == 0)
            {
                return "false_没有找到需要删除数据";
            }
            bool state = BCtrl_SMS.Instance.SMS_Delete(smsID);
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
        public string GetModelContent(HttpContext context)//获取短信模板内容
        {
            string modelKey = context.Request.Form["modelKey"];
            OperationResult<IList<SMSModeEntity>> result = 
                             BCtrl_SignSMSModel.Instance.SignSMSModel_GetWhere(" AND ModelKey='"+modelKey+"' ");
            if (result.ResultType == OperationResultType.Success)
            {
                IList<SMSModeEntity> entitys = result.AppendData;
                if (entitys != null && entitys.Count != 0)
                {
                    return entitys[0].ModelContent;
                }
                return "null";
            }
            else 
            {
                return "null";
            }
           
        }
        /// <summary>
        ///  已存在的excel 转成 table
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="currFilePath">文件路径</param>
        /// <returns></returns>
        private DataTable ExcelToDataTable(string fileName, string currFilePath)
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
                            DataColumn column = new DataColumn(cellValue);
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

    }
}
