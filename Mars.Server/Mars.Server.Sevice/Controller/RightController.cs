using Mars.Server.BLL;
using Mars.Server.Entity;
using Mars.Server.Sevice.BaseHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Mars.Server.Sevice.Controller
{
    /// <summary>
    /// 权限控制器 
    /// 操作 角色 功能菜单相关功能
    /// </summary>
    [AjaxController]
    public class RightController : BaseController
    {
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string DeleteRole(HttpContext context)
        {
            string status = "{\"status\":-1}";
            int roleID = 0;

            if (int.TryParse(context.Request.Form["pid"], out roleID) && roleID > 0)
            {
                BCtrl_SysRole bllRole = new BCtrl_SysRole();

                if (bllRole.IsCanDelRole(roleID))
                {
                    if (bllRole.Delete(roleID))
                    {
                        ClearCacheOrSession.ClearRoleCacheByCRUD();
                        status = "{\"status\":1}";
                    }
                    else
                    {
                        status = "{\"status\":0}";
                    }
                }
                else
                {
                    status = "{\"status\":2}";
                }
            }

            return status;
        }


        /// <summary>
        /// 判断角色名是否可用 
        /// </summary>       
        /// <returns></returns>
        [AjaxHandlerAction]
        public string IsUseableRoleName(HttpContext context)
        {
            int roleID = 0;
            string roleName = context.Request.Form["rname"];

            if (!string.IsNullOrEmpty(roleName) && int.TryParse(context.Request.Form["pid"], out roleID))
            {
                BCtrl_SysRole bllRole = new BCtrl_SysRole();
                bool isUseable = bllRole.IsUseableRoleName(roleID, roleName);

                if (isUseable)
                {
                    return "true";
                }
                else
                {
                    return "false";
                }
            }
            else
            {
                return "false";
            }
        }

        /// <summary>
        /// 保存角色及相应权限
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string SaveRoleFunRel(HttpContext context)
        {
            string status = "{\"status\":0;}";

            int roleID = 0;
            string roleName = context.Request.Form["rname"];
            string functionsid = context.Request.Form["functionsid"];

            if (!int.TryParse(context.Request.Form["pid"], out roleID) || string.IsNullOrEmpty(functionsid) || string.IsNullOrEmpty(roleName))
            {
                status = "{\"status\":-1}";  //传递参数不完整
            }
            else
            {
                BCtrl_SysRole bllRole = new BCtrl_SysRole();
                List<SysRoleFunctionRelEntity> roleFunlist = new List<SysRoleFunctionRelEntity>();
                SysRoleEntity roleEntity = null;

                String[] funid = functionsid.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (String var in funid)
                {
                    SysRoleFunctionRelEntity fe = new SysRoleFunctionRelEntity();
                    fe.RFRel_FunctionID = Convert.ToInt32(var);
                    fe.RFRel_RoleID = roleID;
                    roleFunlist.Add(fe);
                }

                if (roleID > 0)
                {
                    //修改
                    roleEntity = bllRole.QueryEntity(roleID);
                    roleEntity.Role_Name = roleName;

                    if (roleEntity != null)
                    {
                        if (bllRole.Update(roleEntity, roleFunlist))
                        {
                            status = "{\"status\":1}";
                        }
                    }
                }
                else
                {
                    //添加
                    roleEntity = new SysRoleEntity();
                    roleEntity.Role_Name = roleName;
                    roleEntity.CreateTime = DateTime.Now;

                    if (bllRole.Insert(roleEntity, roleFunlist))
                    {
                        status = "{\"status\":1}";
                    }
                }
            }

            return status;
        }

        /// <summary>
        /// 判断菜单名称是否可用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string IsUseableFunctionName(HttpContext context)
        {
            int functionID = 0;
            string funcionName = context.Request.Form["fname"];

            if (!string.IsNullOrEmpty(funcionName) && int.TryParse(context.Request.Form["pid"], out functionID))
            {
                BCtrl_Function bll = new BCtrl_Function();
                bool isUseable = bll.IsUseableFunctionName(functionID, funcionName);

                if (isUseable)
                {
                    return "true";
                }
                else
                {
                    return "false";
                }
            }
            else
            {
                return "false";
            }
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string DeleteFunction(HttpContext context)
        {
            string status = "{\"status\":-1}";
            int functionID = 0;

            if (int.TryParse(context.Request.Form["pid"], out functionID) && functionID > 0)
            {
                BCtrl_Function bll = new BCtrl_Function();

                if (bll.Delete(functionID))
                {
                    ClearCacheOrSession.ClearFunctionsCacheByCRUD();
                    status = "{\"status\":1}";
                }
                else
                {
                    status = "{\"status\":0}";
                }
            }

            return status;
        }
    }
}
