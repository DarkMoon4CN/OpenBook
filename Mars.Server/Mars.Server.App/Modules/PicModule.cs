using Mars.Server.BLL;
using Mars.Server.Entity;
using Mars.Server.Utils;
using Nancy;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.App.Modules
{
    public class PicModule : ModuleBase
    {
        public static int CurrentImageServerID = int.Parse(System.Configuration.ConfigurationManager.AppSettings["CurrentImageServerID"]);
        public static string CurrentImageServer;
        private static object locker = new object();
        static PicModule()
        {
            lock (locker)
            {
                CurrentImageServer = picobj.QueryPicServer(CurrentImageServerID).Domain;
            }
        }
        static BCtrl_PictureServer picobj = new BCtrl_PictureServer();
        static UploadImageHelper uploadBll = new UploadImageHelper();
        public PicModule()
            : base("/Pic")
        {

            Get["QueryStartPic"] = _ =>
            {
                try
                {
                   
                    DataTable dt = picobj.QueryStartPic();
                    
                    string defaultpic="";
                    string customepic="";
                    string url = "";
                    DateTime op=DateTime.MinValue;
                    DateTime ed=DateTime.MinValue;
                    string verson = base.Request.Headers["mars_version"].FirstOrDefault();
                    string appkey = base.Request.Headers["mars_appkey"].FirstOrDefault();
                    if (appkey.ToLower() == "mars.mobile" && (verson == "112" || verson == "1.1.2"))
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = "获取启动图片失败" }); 
                    }
                    if (dt.Rows.Count>0)
                    {
                        defaultpic = string.Format("{0}{1}", dt.Rows[0]["Domain"].ToString(), dt.Rows[0]["PicturePath"].ToString());
                        customepic = string.Format("{0}{1}", dt.Rows[0]["Domain"].ToString(), dt.Rows[0]["PicturePath"].ToString());
                        op = DateTime.Parse(dt.Rows[0]["StartTime"].ToString());
                        ed = DateTime.Parse(dt.Rows[0]["EndTime"].ToString());
                        url = dt.Rows[0]["Url"].ToString();
                    }
                    return JsonObj<dynamic>.ToJson(new { Status = 1, Msg = "获取成功", defaultUrl = defaultpic, customUrl = customepic, customStartTime = op, customEndTime = ed, url=url });
                }
                catch (Exception ex)
                {
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = "获取启动图片失败" });
                }
            };

            Post["/Upload"] = _ =>
            {
                string filename = Guid.NewGuid().ToString("N");
                string filepath = Path.Combine(AppPath.TempFolder, string.Format("{0}.jpg", filename));
                try
                {
                    //HttpFile

                    string path = string.Format("{0}/{1}.jpg", DateTime.Now.ToString("yyyyMMdd"), filename);
                    HttpFile file = Request.Files.ToList()[0];
                    byte[] buffer = new byte[2048];
                    using (FileStream fs = new FileStream(filepath, FileMode.Create))
                    {
                        int length = file.Value.Read(buffer, 0, buffer.Length);
                        while (length > 0)
                        {
                            fs.Write(buffer, 0, length);
                            length = file.Value.Read(buffer, 0, buffer.Length);
                        }
                    }

                    bool result = uploadBll.UpLoadImage(path, filepath);
                    if (result)
                    {
                        int picid = picobj.AddPicInfoToDB(CurrentImageServerID, string.Format("/{0}", path));
                        if (picid > 0)
                        {
                            return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 1, Msg = string.Format("{0}/{1}", CurrentImageServer, path), Tag = picid.ToString() });
                        }
                        else
                        {
                            return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = "图片信息写入服务器失败！" });
                        }
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = "图片上传至远程服务器失败！" });
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = ex.Message });
                }
                finally
                {
                    try
                    {
                        if (File.Exists(filepath))
                        {
                            File.Delete(filepath);
                        }
                    }
                    catch { }
                }
            };

            Get["QueryImages"] = _ =>
            {
                try
                {
                    List<int> pics = FecthQueryData<List<int>>();
                    List<PictureEntity> lists = picobj.QueryImages(pics);
                    if (lists != null)
                    {
                        return JsonObj<JsonMessageBase<List<PictureEntity>>>.ToJson(new JsonMessageBase<List<PictureEntity>>() { Status = 1, Msg = "获取成功", Value = lists });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase<List<PictureEntity>>>.ToJson(new JsonMessageBase<List<PictureEntity>>() { Status = 1, Msg = "获取失败", Value = new List<PictureEntity>() });

                    }
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase<List<PictureEntity>>>.ToJson(new JsonMessageBase<List<PictureEntity>>() { Status = 0, Msg = ex.Message, Value = new List<PictureEntity>() });
                }


            };
        }
    }
}
