using Mars.Server.BLL.Comments;
using Mars.Server.Entity;
using Mars.Server.Entity.Comments;
using Mars.Server.Sevice.BaseHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Mars.Server.Sevice.Controller
{
    [AjaxController]
    public class SensitiveWordController : BaseController
    {
        [AjaxHandlerAction]
        public string ChangeStateType(HttpContext context)
        {
            int id = 0;
            int st = 0;
            if (int.TryParse(context.Request.Form["_st"], out st) && int.TryParse(context.Request.Form["_id"], out id))
            {
                BCtrl_SensitiveWordSearch bll = new BCtrl_SensitiveWordSearch();
                if (bll.ChangeStateType(id, st))
                {
                    return string.Format(Entity.G.JSON_OK_STATE_STRING, "修改状态成功");
                }
                else
                {
                    return string.Format(Entity.G.JSON_ERROR_STATE_STRING, "修改状态失败");
                }
            }
            else
            {
                return string.Format(Entity.G.JSON_PMSERROR_STATE_STRING, "获取参数失败");
            }
        }

        [AjaxHandlerAction]
        public string SensitiveWordEdit(HttpContext context)
        {
            int id = 0;
            string sw = "";
            bool st = false;

            if (!string.IsNullOrEmpty(context.Request.Form["sw"]))
            {
                int.TryParse(context.Request.Form["id"], out id);
                bool.TryParse(context.Request.Form["st"], out st);
                sw = De(context.Request.Form["sw"]);

                BCtrl_SensitiveWordSearch bll = new BCtrl_SensitiveWordSearch();
                
                if (id > 0)
                {
                    SensitiveWordEntity item = new SensitiveWordEntity()
                    {
                        SWID = id,
                        SensitiveWords = sw,
                        IsNeedRecheck = st,
                        StateTypeID = 1,
                        CreateTime = DateTime.Now,
                        CreateUserID = int.Parse(this.CurrentAdmin.Sys_UserID)
                    };

                    if (bll.UpdateSensitiveWord(item))
                    {
                        return string.Format(G.JSON_OK_STATE_STRING, "修改成功");
                    }
                    else
                    {
                        return string.Format(G.JSON_ERROR_STATE_STRING, "修改失败");
                    }
                }
                else
                {
                    string[] sws = sw.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (sws != null)
                    {
                        bool success = true;
                        foreach (string _sw in sws)
                        {
                            SensitiveWordEntity item = new SensitiveWordEntity()
                            {
                                SWID = id,
                                SensitiveWords = _sw,
                                IsNeedRecheck = st,
                                StateTypeID = 1,
                                CreateTime = DateTime.Now,
                                CreateUserID = int.Parse(this.CurrentAdmin.Sys_UserID)
                            };
                            if (bll.AddSensitiveWord(item) == 0)
                            {
                                success = false;
                                break;
                            }
                        }

                        if (success)
                        {
                            return string.Format(G.JSON_OK_STATE_STRING, "添加成功");
                        }
                        else
                        {
                            return string.Format(G.JSON_ERROR_STATE_STRING, "添加失败");
                        }
                    }
                    else
                    {
                        return string.Format(G.JSON_ERROR_STATE_STRING, "添加失败");
                    }
                }
            }
            else
            {
                return string.Format(G.JSON_PMSERROR_STATE_STRING, "参数获取失败");
            }
        }
    }
}
