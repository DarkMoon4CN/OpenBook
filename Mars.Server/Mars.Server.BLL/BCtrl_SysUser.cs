using Mars.Server.DAO;
using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Mars.Server.BLL
{
    public class BCtrl_SysUser
    {
        private SysUserDAO sysuserobj = new SysUserDAO();
        public AdminSessionEntity QuerySysUserInfo(string uid)
        {
            DataTable dt = sysuserobj.QuerySysUserInfo(uid);
            if (dt != null && dt.Rows.Count > 0)
            {
                AdminSessionEntity ase = new AdminSessionEntity();
                ase.Sys_DisplayName = dt.Rows[0]["User_Name"].ToString();
                ase.Sys_RoleID = int.Parse(dt.Rows[0]["User_RoleID"].ToString());
                ase.Sys_RoleName = dt.Rows[0]["Role_Name"].ToString();
                return ase;
            }
            else
                return null;
        }

        #region by perry
        /// <summary>
        /// 根据角色查询所有用户
        /// </summary>
        /// <param name="rid"></param>
        /// <returns></returns>
        public DataTable GetUsersByRoleID(int rid)
        {
            DataSet ds = sysuserobj.GetUsersByRoleID(rid);
            if (ds.Tables.Count != 0)
                return ds.Tables[0];
            else
                return new DataTable();
        }
        /// <summary>
        /// 帐户维护时获得所有已分配权限的用户信息
        /// </summary>
        /// <remarks>
        /// author: zp
        /// createtime: 2010.07.13
        /// </remarks> 
        /// <param name="PageSize">每页大小</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="Records">返回总页数</param>
        /// <returns></returns>
        public DataTable GetUserWithHaveFunForPage(String username, int deptid, int PageSize, int PageIndex, out int Records)
        {
            StringBuilder sb = new StringBuilder("Role_ID<>100");
            if (username != "")
            {
                sb.Append(" and User_Name like '%");
                sb.Append(StringUti.CleanString(username));
                sb.Append("%'");
            }
            if (deptid != -1)
            {
                sb.Append(" and Dept_ID=");
                sb.Append(deptid);
            }
            return sysuserobj.GetUserWithHaveFunForPage(sb.ToString(), PageSize, PageIndex, out Records).Tables[0];
        }

        public DataTable GetUserWithHaveFun()
        {
            DataTable table = null;
            DataSet ds = sysuserobj.GetUserWithHaveFun();
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        table = ds.Tables[0];
                    }
                }
            }

            return table;
        }
        /// <summary>
        /// 帐户维护页默认加载所有部门
        /// </summary>
        /// <remarks>
        /// author: zp
        /// createtime: 2010.07.14
        /// </remarks> 
        /// <returns></returns>
        public DataTable GetAllDept()
        {
            DataSet ds = sysuserobj.GetAllDept();
            if (ds.Tables.Count != 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return new DataTable();
            }
        }
        /// <summary>
        /// 查询用户角色编号
        /// </summary>
        /// <remarks>
        /// author: zp
        /// createtime: 2010.07.13
        /// </remarks> 
        /// <param name="userid">用户编号</param>
        /// <returns></returns>
        public int GetRoleByUserID(String userid)
        {
            return sysuserobj.GetRoleByUserID(userid);
        }
        /// <summary>
        /// 设置角色时查询所有角色信息
        /// </summary>
        /// <remarks>
        /// author: zp
        /// createtime: 2010.05.11
        /// </remarks> 
        /// <returns></returns>
        public DataTable GetAllRole()
        {
            DataSet ds = sysuserobj.GetAllRole();
            if (ds.Tables.Count != 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return new DataTable();
            }
        }
        /// <summary>
        /// 修改用户角色
        /// </summary>
        /// <remarks>
        /// author: zp
        /// createtime: 2010.07.13
        /// </remarks> 
        /// <param name="userid">用户编号</param>
        /// <param name="roleid">修改后的角色</param>
        /// <returns></returns>
        public bool UpdateUserRole(String userid, int roleid)
        {
            return sysuserobj.UpdateUserRole(userid, roleid);
        }
        /// <summary>
        /// 设置用户权限
        /// </summary>
        /// <remarks>
        /// author: zp
        /// createtime: 2010.07.13
        /// </remarks> 
        /// <param name="userid"></param>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public Boolean SetUserFun(String userid, int roleid)
        {
            Boolean a = sysuserobj.DelUserFunctionRel(userid);
            if (a)
            {
                Boolean b = sysuserobj.SetUserFunctionRel(userid, roleid);
                if (b)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 角色设置时查询用户编号是否已存在
        /// </summary>
        /// <remarks>
        /// author: zp
        /// createtime: 2010.07.13
        /// </remarks> 
        /// <param name="userid">用户编号</param>
        /// <returns></returns>
        public bool GetUserIsHaveByUserID(String userid)
        {
            return sysuserobj.GetUserIsHaveByUserID(userid);
        }
        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <remarks>
        /// author: zp
        /// createtime: 2010.07.13
        /// </remarks> 
        /// <param name="userid">用户编号</param>
        /// <param name="roleid">角色编号</param>
        /// <returns></returns>
        public bool SetUserRole(String userid, int roleid)
        {
            return sysuserobj.SetUserRole(userid, roleid);
        }

        /// <summary>
        /// 登录判断
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pwd"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool TryLogin(string username, string pwd, out string userid)
        {
            return sysuserobj.TryLogin(username, pwd, out userid);
        }
        #endregion by perry

        public DataTable QueryAdminTable(AdminSearchEntity entity, out int totalcnt)
        {
            return sysuserobj.QueryAdminTable(entity, out totalcnt);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Insert(AdminEntity entity)
        {
            return sysuserobj.Insert(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(AdminEntity entity)
        {
            return sysuserobj.Update(entity);
        }

        /// <summary>
        /// 删除系统管理员
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool Delete(int userID)
        {
            return sysuserobj.Delete(userID);
        }

        /// <summary>
        /// 判断邮箱是否可用
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool IsUseableByEmail(string email, int userID)
        {
            return sysuserobj.IsUseableByEmail(email, userID);
        }

        /// <summary>
        /// 判断用户是否可以注册
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool IsUseableByUsername(string username)
        {
            return sysuserobj.IsUseableByUsername(username);
        }

        /// <summary>
        /// 获取管理员实体
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public AdminEntity QuerySysUserEntity(int userID)
        {
            return sysuserobj.QuerySysUserEntity(userID);
        }

        /// <summary>
        /// 修改管理员密码
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool UpdatePassword(int userID, string password)
        {
            return sysuserobj.UpdatePassword(userID, password);
        }

        /// <summary>
        /// 初始化系统数据
        /// </summary>
        /// <returns></returns>
        public bool InitDataBase()
        {
            return sysuserobj.InitDataBase();
        }
    }

    public class ClearCacheOrSession
    {
        #region 清理管理员及权限相关Session和Cache
        /// <summary>
        /// 当增加或修改菜单时，删除相关缓存和Session
        /// </summary>
        /// <returns></returns>
        public static bool ClearFunctionsCacheByCRUD()
        {
            bool isSuccess = false;

            try
            {
                if (HttpContext.Current.Session[WebKeys.AdminSessionKey] != null)
                {
                    AdminSessionEntity adminEntity = HttpContext.Current.Session[WebKeys.AdminSessionKey] as AdminSessionEntity;

                    string key = string.Format(WebKeys.CK_SYS_LEFTMENU, adminEntity.Sys_UserID);
                    HttpContext.Current.Session[key] = null;

                    adminEntity.Functions = new BCtrl_Function().GetFunction(adminEntity.Sys_UserID);
                    HttpContext.Current.Session[WebKeys.AdminSessionKey] = adminEntity;
                }
                isSuccess = true;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
            return isSuccess;
        }

        /// <summary>
        /// 修改管理员 清除Cache和Session
        /// </summary>
        /// <param name="adminID"></param>
        /// <returns></returns>
        public static bool ClearAdminCacheByCRUD(int? userID = null)
        {
            bool isSuccess = false;

            try
            {
                AdminSessionEntity adminEntity = HttpContext.Current.Session[WebKeys.AdminSessionKey] as AdminSessionEntity;
                if (userID.HasValue)
                {
                    if (adminEntity.Sys_UserID == userID.Value.ToString())
                    {
                        HttpContext.Current.Session[WebKeys.AdminSessionKey] = null;
                    }
                }
                else
                {
                    if (HttpContext.Current.Session[WebKeys.AdminSessionKey] != null)
                    {                      
                        HttpContext.Current.Session[WebKeys.AdminSessionKey] = null;
                    }
                }
              
                isSuccess = true;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
            return isSuccess;
        }


        /// <summary>
        /// 当操作Role时，删除相关缓存和Session
        /// </summary>
        /// <returns></returns>
        public static bool ClearRoleCacheByCRUD()
        {
            bool isSuccess = false;

            try
            {
                if (HttpContext.Current.Session[WebKeys.AdminSessionKey] != null)
                {
                    AdminSessionEntity adminEntity = HttpContext.Current.Session[WebKeys.AdminSessionKey] as AdminSessionEntity;

                    string key = string.Format(WebKeys.CK_SYS_LEFTMENU, adminEntity.Sys_UserID);
                    HttpContext.Current.Session[key] = null;

                    adminEntity.Functions = new BCtrl_Function().GetFunction(adminEntity.Sys_UserID);
                    HttpContext.Current.Session[WebKeys.AdminSessionKey] = adminEntity;
                }
                isSuccess = true;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
            return isSuccess;
        }
        #endregion
    }
}
