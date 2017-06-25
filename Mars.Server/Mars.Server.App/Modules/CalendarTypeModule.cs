using Mars.Server.BLL;
using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.App.Modules
{
    public class CalendarTypeModule : ModuleBase
    {
        BCtrl_CalendarType ctypeobj = new BCtrl_CalendarType();
        public CalendarTypeModule()
            : base("/CalendarType")
        {
            Get["/GetPublicCalendarTypes"] = _ =>
            {
                try
                {
                    dynamic data = FecthQueryData();
                    int parentid = (data.parentcalendartypeid == null ? 0 : data.parentcalendartypeid);
                    int pagesize = data.pagesize == null ? 20 : data.pagesize;
                    int pageno = data.pageno == null ? 1 : data.pageno;
                    if (pagesize > 50)
                    {
                        pagesize = 50;
                    }
                    List<CalendarTypeEntity> lists = ctypeobj.QueryCalendarTypes(pageno, pagesize, parentid, 2, CurrentUser == null ? 0 : CurrentUser.UserID);
                    if (lists != null)
                    {
                        return JsonObj<JsonMessageBase<List<CalendarTypeEntity>>>.ToJson(new JsonMessageBase<List<CalendarTypeEntity>>() { Value = lists, Status = 1, Msg = "获取成功" });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase<List<CalendarTypeEntity>>>.ToJson(new JsonMessageBase<List<CalendarTypeEntity>>() { Value = new List<CalendarTypeEntity>(), Status = 0, Msg = "读取数据过程中发生错误" });
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase<List<CalendarTypeEntity>>>.ToJson(new JsonMessageBase<List<CalendarTypeEntity>>() { Value = new List<CalendarTypeEntity>(), Status = 0, Msg = ex.Message });
                }


            };

            Post["/AddFavor"] = _ =>
            {
                try
                {
                    dynamic data = FetchFormData();
                    int calendartypeid = data.CalendarTypeID;
                    if (ctypeobj.AddFavor(calendartypeid, CurrentUser.UserID))
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 1, Msg = "收藏成功" });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = "收藏失败" });
                    }
                }
                catch (Exception ex)
                {
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = ex.Message });
                }
            };

            Post["/RemoveFavor"] = _ =>
            {
                try
                {
                    dynamic data = FetchFormData();
                    int calendartypeid = data.CalendarTypeID;
                    if (ctypeobj.RemoveFavor(calendartypeid, CurrentUser.UserID))
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 1, Msg = "操作成功" });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = "操作失败" });
                    }
                }
                catch (Exception ex)
                {
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = ex.Message });
                }
            };
            Get["/GetMyFavorList"] = _ =>
            {
                try
                {
                    //dynamic data = FecthQueryData();
                    //int pagesize = data.pagesize == null ? 20 : data.pagesize;
                    //int pageno = data.pageno == null ? 1 : data.pageno;

                    //if (pagesize > 50)
                    //{
                    //    pagesize = 50;
                    //}

                    List<CalendarTypeEntity> lists = ctypeobj.QueryFavorCalendarTypes(1, 1000, CurrentUser.UserID);
                    if (lists != null)
                    {
                        return JsonObj<JsonMessageBase<List<CalendarTypeEntity>>>.ToJson(new JsonMessageBase<List<CalendarTypeEntity>>() { Status = 1, Msg = "获取成功", Value = lists });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase<List<CalendarTypeEntity>>>.ToJson(new JsonMessageBase<List<CalendarTypeEntity>>() { Status = 0, Msg = "读取数据过程中发生错误", Value = new List<CalendarTypeEntity>() });
                    }

                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase<List<CalendarTypeEntity>>>.ToJson(new JsonMessageBase<List<CalendarTypeEntity>>() { Status = 0, Msg = ex.Message, Value = new List<CalendarTypeEntity>() });
                }
            };
        }
    }
}
