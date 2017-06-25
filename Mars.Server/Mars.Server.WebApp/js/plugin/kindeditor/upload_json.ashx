<%@ WebHandler Language="C#" Class="Upload" %>

/**
 * KindEditor ASP.NET
 *
 * 本ASP.NET程序是演示程序，建议不要直接在实际项目中使用。
 * 如果您确定直接使用本程序，使用之前请仔细确认相关安全设置。
 *
 */

using System;
using System.Collections;
using System.Web;
using System.IO;
using System.Globalization;
using LitJson;
using Mars.Server.BLL;
using Mars.Server.Entity;
using Mars.Server.Utils;

public class Upload : IHttpHandler
{
    private HttpContext context;

    public void ProcessRequest(HttpContext context)
    {
        //String aspxUrl = context.Request.Path.Substring(0, context.Request.Path.LastIndexOf("/") + 1);
        String aspxUrl = Mars.Server.Utils.WebMaster.webroot;

        //文件保存目录路径
        String savePath = "~/UploadImages/KindeditorImages/";

        //文件保存目录URL
        String saveUrl = aspxUrl + "UploadImages/KindeditorImages/";

        //定义允许上传的文件扩展名
        Hashtable extTable = new Hashtable();
        extTable.Add("image", "gif,jpg,jpeg,png,bmp");
        extTable.Add("flash", "swf,flv");
        extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
        extTable.Add("file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2");

        //最大文件大小
        int maxSize = 10000000;
        this.context = context;

        HttpPostedFile imgFile = context.Request.Files["imgFile"];
        if (imgFile == null)
        {
            showError("请选择文件。");
        }

        String dirPath = context.Server.MapPath(savePath);
        if (!Directory.Exists(dirPath))
        {
            showError("上传目录不存在。");
        }

        String dirName = context.Request.QueryString["dir"];
        if (String.IsNullOrEmpty(dirName))
        {
            showError("目录不存在。");
        }
        if (!extTable.ContainsKey(dirName))
        {
            showError("目录名不正确。");
        }

        String fileName = imgFile.FileName;
        String fileExt = Path.GetExtension(fileName).ToLower();

        if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
        {
            showError("上传文件大小超过限制。");
        }

        if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
        {
            showError("上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dirName]) + "格式。");
        }

        String newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
        String filePath = dirPath + newFileName;

        imgFile.SaveAs(filePath);

        UploadImageHelper uploadImgHelper = new UploadImageHelper();

        string qiniuImgName;
        bool isUploaSuccess = uploadImgHelper.UpLoadImage(filePath, out qiniuImgName, true);

        if (isUploaSuccess)
        {
            BCtrl_PictureServer bllPicServer = new BCtrl_PictureServer();
            PictureServerEntity picserverEntity = bllPicServer.QueryPicServer(WebMaster.ImageServerID);
            if (picserverEntity != null && picserverEntity.PictureServerID > 0)
            {
                PictureEntity picEntity = new PictureEntity();
                picEntity.PictureServerID = picserverEntity.PictureServerID.ToString();
                picEntity.PicturePath = qiniuImgName;
                picEntity.PictureState = 0;

                int picId = bllPicServer.InsertPicture(picEntity);
                if (picId>0)
                {
                    string fileUrl = uploadImgHelper.QiniuDomain + qiniuImgName;
                    Hashtable hash = new Hashtable();
                    hash["error"] = 0;
                    hash["url"] = fileUrl;
                    hash["imgname"] = qiniuImgName;
                    hash["picid"] = picId;
                    context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
                    context.Response.Write(JsonMapper.ToJson(hash));
                    context.Response.End();
                }
                else
                {
                    showError("上传图片保存失败！");
                }
            }
            else
            {
                uploadImgHelper.DeleteImage(qiniuImgName);
                showError("图片服务配置错误！");
            }
        }
        else
        {
            showError("上传图片失败！");
        }
    }

    private void showError(string message)
    {
        Hashtable hash = new Hashtable();
        hash["error"] = 1;
        hash["message"] = message;
        context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
        context.Response.Write(JsonMapper.ToJson(hash));
        context.Response.End();
    }

    public bool IsReusable
    {
        get
        {
            return true;
        }
    }
}
