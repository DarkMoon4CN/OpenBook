using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mars.Server.Sevice.BaseHandler;
using System.Web;
using Mars.Server.Entity;
using Mars.Server.BLL;
using Mars.Server.Utils;
using System.Data;

namespace Mars.Server.Sevice.Controller
{
    /// <summary>
    /// WebApp管理员AJAX管理类
    /// </summary>
    [AjaxController]
    public class AdminController : BaseController
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string AdminLogins(HttpContext context)
        {
            string loginname = context.Request.Form["un"];
            string password = context.Request.Form["pw"];
            string valid = context.Request.Form["va"];
            string returnUrl = context.Request.Form["url"];

            if (string.IsNullOrEmpty(loginname) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(valid))
            {
                return "{\"state\":-9}";  //传递参数不完整
            }
            else
            {
                if (valid.Equals(DateTime.Now.Day.ToString()))
                {
                    BCtrl_SysUser sysUserBll = new BCtrl_SysUser();

                    #region 管理员登录验证
                    string userid = "";
                    password = MD5.Encode(WebKeys.AdminPwdRandom, password);
                    //尝试登录
                    //if (new PassportServiceProxy().TryLogin(loginname, password, "NewBookSystem", out  userid))
                    if (sysUserBll.TryLogin(loginname, password, out userid))
                    {
                        if (!string.IsNullOrEmpty(userid))
                        {
                            //查询用户所属角色能访问的频道
                            List<FunctionEntity> list = new BCtrl_Function().GetFunction(userid);
                            if (list.Count != 0)
                            {
                                AdminSessionEntity ue = new BCtrl_SysUser().QuerySysUserInfo(userid);
                                if (ue != null)
                                {
                                    ue.Sys_LoginName = loginname;
                                    ue.Sys_UserID = userid;

                                    ue.Functions = list;
                                    context.Session[WebKeys.AdminSessionKey] = ue;

                                    return "{\"state\":1}";  //登录成功
                                }
                                else
                                {
                                    return "{\"state\":-7}";  //登录成功但在系统中为找到授权
                                }
                            }
                            else
                            {
                                return "{\"state\":-6}";  //登录成功但无使用功能权限
                            }
                        }
                        else
                        {
                            return "{\"state\":-1}";  //登录失败 用户名密码错误
                        }
                    }
                    else
                    {
                        return "{\"state\":-1}";  //登录失败 用户名密码错误
                    }
                    #endregion

                    #region 搭建时测试
                    //if (loginname == "zl" && password == "123456")
                    //{
                    //    AdminSessionEntity ue = new AdminSessionEntity();

                    //    ue.Sys_LoginName = loginname;
                    //    ue.Sys_UserID = "1";

                    //    context.Session[WebKeys.AdminSessionKey] = ue;
                    //    string result = "{\"state\":1, \"url\":\"" + returnUrl + "\"}";
                    //    return result;  //登录成功
                    //}
                    //else
                    //{
                    //    return "{\"state\":-1}";  //登录失败 用户名密码错误
                    //}
                    #endregion
                }
                else
                {
                    //验证码不正确
                    return "{\"state\":-8}";  //验证码不正确
                }
            }
        }

        /// <summary>
        /// 退出系统
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string Logout(HttpContext context)
        {
            string status = "{\"status\":0}";
            try
            {
                context.Session.RemoveAll();
                status = "{\"status\":1}";
            }
            catch (Exception ex)
            {
                string message = "系统管理员" + base.CurrentAdmin.Sys_LoginName + "退出系统失败！" + ex.Message;
                LogUtil.WriteLog(message);
            }
            return status;
        }

        /// <summary>
        /// 判断邮箱是可有使用 true可以 false不可以已存在
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string IsUseableByEmail(HttpContext context)
        {
            bool isUseable = false;
            string email = context.Request.Form["email"];
            int pid = 0;
            int.TryParse(context.Request["pid"], out pid);
            if (!string.IsNullOrEmpty(email))
            {
                BCtrl_SysUser sysUserBll = new BCtrl_SysUser();
                isUseable = sysUserBll.IsUseableByEmail(email.Trim(), pid);
            }

            if (isUseable)
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }

        /// <summary>
        /// 判断用户是否可以注册
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string IsUseableByUsername(HttpContext context)
        {
            bool isUseable = false;
            string username = context.Request.Form["username"];
            if (!string.IsNullOrEmpty(username))
            {
                BCtrl_SysUser sysUserBll = new BCtrl_SysUser();
                isUseable = sysUserBll.IsUseableByUsername(username.Trim());
            }

            if (isUseable)
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }

        /// <summary>
        /// 删除系统管理员
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string DeleteAdmin(HttpContext context)
        {
            int userID = 0;

            if (int.TryParse(context.Request.Form["pid"], out userID) && userID > 0)
            {
                BCtrl_SysUser bllSysuser = new BCtrl_SysUser();
                bool isSuccess = bllSysuser.Delete(userID);

                if (isSuccess)
                {
                    ClearCacheOrSession.ClearAdminCacheByCRUD(userID);
                    return "{\"status\":1}";
                }
                else
                {
                    return "{\"status\":0}";
                }
            }
            else
            {
                return "{\"status\":-1}";
            }
        }

        /// <summary>
        /// 修改管理员密码 
        /// </summary>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string ChangeAdminPwd(HttpContext context)
        {
            int userID = 0;
            string pwd = context.Request.Form["pwd"];

            if (!string.IsNullOrEmpty(pwd) && int.TryParse(context.Request.Form["pid"], out userID) && userID > 0)
            {
                BCtrl_SysUser bll = new BCtrl_SysUser();

                pwd = MD5.Encode(WebKeys.AdminPwdRandom, pwd.Trim());
                bool isSuccess = bll.UpdatePassword(userID, pwd);


                if (isSuccess)
                {
                    ClearCacheOrSession.ClearAdminCacheByCRUD(userID);
                    return "{\"status\":1}";
                }
                else
                {
                    return "{\"status\":0}";
                }
            }
            else
            {
                return "{\"status\":-1}";
            }
        }

        /// <summary>
        /// 设置角色
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string SetRole(HttpContext context)
        {
            string returnValue = "{\"state\":0}";
            string userid = context.Request.Form["userid"];
            int roleid = 0;
            int.TryParse(context.Request.Form["roleid"], out roleid);

            if (string.IsNullOrEmpty(userid) || roleid == 0)
            {
                returnValue = "{\"state\":-1}";  //传递参数不完整
            }
            else
            {
                if (new BCtrl_SysUser().SetUserRole(userid, roleid))
                {
                    //如果用户被设置为超级管理员应该获得所有分类权限
                    if (roleid == 100)
                    {
                        if (GetFunToAdmin(userid))
                        {
                            ClearCacheOrSession.ClearRoleCacheByCRUD();
                            returnValue = "{\"state\":1}";                          
                        }
                    }
                    else
                    {
                        if (new BCtrl_SysUser().SetUserFun(userid, roleid))
                        {
                            ClearCacheOrSession.ClearRoleCacheByCRUD();
                            returnValue = "{\"state\":1}";                          
                        }
                    }
                }
            }

            return returnValue;
        }

        /// <summary>
        /// 设置权限
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string SetFunctions(HttpContext context)
        {
            string returnValue = "{\"state\":0}";

            string userid = context.Request.Form["userid"];
            string functionsid = context.Request.Form["functionsid"];

            if (string.IsNullOrEmpty(userid) || string.IsNullOrEmpty(functionsid))
            {
                returnValue = "{\"state\":-1}";  //传递参数不完整
            }
            else
            {
                List<FunctionEntity> list = new List<FunctionEntity>();

                String[] funid = functionsid.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (String var in funid)
                {
                    FunctionEntity fe = new FunctionEntity();
                    fe.Function_ID = Convert.ToInt32(var);
                    list.Add(fe);
                }
                if (new BCtrl_Function().EditUserFunRel(userid, list))
                {
                    ClearCacheOrSession.ClearFunctionsCacheByCRUD();
                    returnValue = "{\"state\":1}";
                }
            }

            return returnValue;
        }

        /// <summary>
        /// 初始化系统数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string InitData(HttpContext context)
        {
            string status = "{\"status\":-1}";
            if (base.CurrentAdmin.Sys_RoleID == 100)
            {
                BCtrl_SysUser bll = new BCtrl_SysUser();
                if (bll.InitDataBase())
                {
                    ClearCacheOrSession.ClearAdminCacheByCRUD();
                    status = "{\"status\":1}";
                }
                else
                {
                    status = "{\"status\":0}";
                }
            }
            else
            {
                status = "{\"status\":2}"; //没有权限执行操作
            }

            return status;
        }

        #region 补充选项
        /// <summary>
        /// 为超级管理员分配所有权限
        /// </summary>
        /// <returns></returns>
        private bool GetFunToAdmin(string userid)
        {
            bool b = true;

            List<FunctionEntity> list = new List<FunctionEntity>();
            //先查询所有权限
            DataTable dt = new BCtrl_Function().GetAllFunction();
            foreach (DataRow var in dt.Rows)
            {
                FunctionEntity fe = new FunctionEntity();
                fe.Function_ID = Convert.ToInt32(var["Function_ID"].ToString());
                list.Add(fe);
            }

            //再分配所有分类权限给用户
            b = new BCtrl_Function().EditUserFunRel(userid, list);

            return b;
        }
        #endregion
    }
}
