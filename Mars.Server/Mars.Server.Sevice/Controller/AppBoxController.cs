using Mars.Server.BLL;
using Mars.Server.Entity;
using Mars.Server.Sevice.BaseHandler;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Mars.Server.Sevice.Controller
{
    [AjaxController]
    public class AppBoxController:BaseController
    {
        [AjaxHandlerAction]
        public string Import(HttpContext context)//导入数据
        {
            UploadImageHelper uploadBll = new UploadImageHelper();
            HttpPostedFile file = context.Request.Files["Filedata"];
            string fileName = file.FileName;
            //string tempPath = AppDomain.CurrentDomain.BaseDirectory + "/UploadFile/" + "AppBox/"; 

            string tempPath = HttpContext.Current.Server.MapPath("~/UploadFile/AppBox/");
            //创建目录  
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }
            fileName = System.IO.Path.GetFileName(fileName); //获取文件名（不带路径）
            string currFileExtension = System.IO.Path.GetExtension(fileName);//获取文件的扩展名
            string currFilePath = tempPath + fileName;       //获取上传后的文件路径 记录到前面声明的全局变量
            file.SaveAs(currFilePath);
            try
            {
                //七牛
                string strName = Guid.NewGuid().ToString("N") + ".png";
                bool state = uploadBll.UpLoadImage(currFilePath, out  strName, true);
                string  imageUrl= uploadBll.QiniuDomain + strName;
                return imageUrl;
            }
            catch 
            {
                return "false";
            }
        }


        [AjaxHandlerAction]
        public string GetArticleUrl(HttpContext context)//文章的URL
        {
            string aid = context.Request.Form["aid"];
            int eventItemID = 0;
            int.TryParse(aid, out eventItemID);
            BCtrl_EventItem bll = new BCtrl_EventItem();
            EventItemEntity entity= bll.QueryEntity(eventItemID);
            return entity.Url;
        }

        [AjaxHandlerAction]
        public string GetMaxTime(HttpContext context) // 获取当前最大的数据ID
        {
            string shortDate = DateTime.Now.ToString("yyyy-MM-dd");
            string startTime = shortDate + " 00:00:00";
            string endTime = shortDate + " 23:59:59";
            string strWhere = " AND StartTime   BETWEEN  '" + startTime + "' ";
            strWhere += " AND  '" + endTime + "'   ORDER BY  StartTime  DESC ";
            OperationResult<IList<DialogMessagesEntity>> result
                          =BCtrl_DialogMessages.Instance.DialogMessages_GetWhere(strWhere);
            if (result.ResultType == OperationResultType.Success)
            {
                IList<DialogMessagesEntity> entitys = result.AppendData;
                if (entitys != null && entitys.Count != 0)
                {
                    DateTime? sTime = entitys[0].StartTime;
                    sTime = sTime.Value.AddSeconds(1);
                    if (sTime.Value.ToShortDateString() == DateTime.Now.ToShortDateString())
                    {
                        return sTime.Value.ToString("yyyy-MM-dd  HH:mm:ss");
                    }
                    else
                    {
                        return "N_" + sTime.Value.ToString("yyyy-MM-dd  HH:mm:ss");
                    }
                }
                else 
                {
                    return startTime;
                }
            }
            return "null";
        }

        [AjaxHandlerAction]
        public string Add(HttpContext context) //添加appBox
        {     
            string imageLink=context.Request.Form["ilink"];
            string buttonText = context.Request.Form["btnText"];
            string articleLink = context.Request.Form["alink"];
            string contents = context.Request.Form["contents"];
            string startTime = context.Request.Form["stime"];
            string stype = context.Request.Form["startType"];
            if (string.IsNullOrEmpty(imageLink)==true || imageLink=="") 
            {
                return "false_图片没有连接！";
            }
            else if (startTime == null)
            {
                return "false_预约时间不可为空！";
            }
            else if (string.IsNullOrEmpty(stype) == true || stype == "" || stype == "-1")
            {
                return "false_弹框类型没有设置！";
            }
            int startType = 0;
            int.TryParse(stype, out startType);
            DialogMessagesEntity entity = new DialogMessagesEntity();
            entity.ImageLink = imageLink;
            entity.ButtonText = buttonText;
            entity.ArticleLink = articleLink;
            entity.Contents = contents;
            entity.StartTime = DateTime.Parse(startTime);
            entity.EndTime = entity.StartTime;
            entity.MoblieType = 0;
            entity.StartType = startType;
            entity.StartCount = 1;
            OperationResult<bool>  result=  BCtrl_DialogMessages.Instance.DialogMessages_Insert(entity);
            if (result.ResultType == OperationResultType.Success && result.AppendData == true)
            {
                return "true";
            }
            else
            {
                return "false_"+result.Message;
            }
        }

        [AjaxHandlerAction]
        public string Update(HttpContext context)//编辑appBox
        {
            string imageLink = context.Request.Form["ilink"];
            string buttonText = context.Request.Form["btnText"];
            string articleLink = context.Request.Form["alink"];
            string contents = context.Request.Form["contents"];
            string startTime = context.Request.Form["stime"];
            string stype = context.Request.Form["startType"];
            string mid = context.Request.Form["mid"];

            if (string.IsNullOrEmpty(imageLink) == true || imageLink == "")
            {
                return "false_图片没有连接！";
            }
            else if (startTime == null)
            {
                return "false_预约时间不可为空！";
            }
            else if (string.IsNullOrEmpty(stype) == true || stype == "" || stype == "-1")
            {
                return "false_弹框类型没有设置！";
            }

            if(string.IsNullOrEmpty(mid) || mid=="")
            {
                return "false_弹框信息无效！";
            }
            int messageID=0;
            int.TryParse(mid,out messageID);
            int startType = 0;
            int.TryParse(stype, out startType);
            DialogMessagesEntity entity = new DialogMessagesEntity();
            entity.ImageLink = imageLink;
            entity.ButtonText = buttonText;
            entity.ArticleLink = articleLink;
            entity.Contents = contents;
            entity.StartTime = DateTime.Parse(startTime);
            entity.EndTime = entity.StartTime;
            entity.MoblieType = 0;
            entity.StartType = startType;
            entity.StartCount = 1;
            entity.MessageID = messageID;
            OperationResult<bool> result = BCtrl_DialogMessages.Instance.DialogMessages_Update(entity);
            if (result.ResultType == OperationResultType.Success && result.AppendData == true)
            {
                return "true";
            }
            else
            {
                return "false_" + result.Message;
            }
        }

        [AjaxHandlerAction]
        public string Delete(HttpContext context) //删除AppBox
        {
            string mid = context.Request.Form["mid"];
            if (string.IsNullOrEmpty(mid) || mid == "")
            {
                return "false_弹框信息无效！";
            }
            int messageID = 0;
            int.TryParse(mid, out messageID);

            string strWhere = string.Empty;
            strWhere += " AND  MessageID={0} ";
            strWhere = string.Format(strWhere,mid);
            OperationResult<IList<DialogMessagesEntity>> result =
                  BCtrl_DialogMessages.Instance.DialogMessages_GetWhere(strWhere);
            if (result.AppendData.Count == 0)
            {
                return "false_所需删除的数据不存在！";            
            }
            OperationResult<bool> delResult=
                           BCtrl_DialogMessages.Instance.DialogMessages_Delete(messageID);
            if (delResult.ResultType == OperationResultType.Success && delResult.AppendData == true)
            {
                return "true";
            }
            else
            {
                return "false_"+delResult.Message;
            }
           
        }
    }
}
