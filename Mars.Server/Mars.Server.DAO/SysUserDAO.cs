using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Mars.Server.DAO
{
    public class SysUserDAO
    {
        public DataTable QuerySysUserInfo(string uid)
        {
            SqlParameter[] prms = { 
                                    new SqlParameter("@Uid",uid)
                                };
            string sql = "select User_RoleID,c.User_Name, Role_Name from M_System_User_Role_Rel u inner join M_System_Role  r on u.User_RoleID=r.Role_ID inner join M_System_User c on c.User_ID=u.User_ID where u.User_ID=@Uid";
            try
            {
                return SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.Text, sql, prms).Tables[0];
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        #region by perry
        /// <summary>
        /// 根据角色查询所有用户
        /// </summary>
        /// <param name="rid"></param>
        /// <returns></returns>
        public DataSet GetUsersByRoleID(int rid)
        {
            SqlParameter[] prms = { 
               new SqlParameter("@roleid",rid)                      
            };
            try
            {
                if (rid == 100)
                    return SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.Text, "select a.User_ID,b.User_Name from M_System_User_Role_Rel as a inner join M_System_User as b on a.User_ID=b.User_ID where (a.User_RoleID=115 or a.User_RoleID=125)", prms);
                else
                    return SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.Text, "select a.User_ID,b.User_Name from M_System_User_Role_Rel as a inner join M_System_User as b on a.User_ID=b.User_ID where a.User_RoleID=@roleid", prms);
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return new DataSet();
            }
        }
        /// <summary>
        /// 分页查询所有已分配角色用户信息
        /// </summary>
        /// <remarks>
        /// author: zp
        /// createtime: 2010.07.13
        /// </remarks> 
        /// <param name="PageSize">页面大小</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="Records">返回总页数</param>
        /// <returns></returns>
        public DataSet GetUserWithHaveFunForPage(String sqlwhere, int PageSize, int PageIndex, out int Records)
        {
            SqlParameter[] prms ={
                new SqlParameter("@TableName","M_V_System_User"),
                new SqlParameter("@Fields","*"),
                new SqlParameter("@OrderField","Role_ID"),
                new SqlParameter("@sqlWhere",sqlwhere),
                new SqlParameter("@pageSize",PageSize),
                new SqlParameter("@pageIndex",PageIndex),
                new SqlParameter("@Records",0)
            };
            try
            {
                prms[6].Direction = ParameterDirection.Output;
                DataSet ds = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.StoredProcedure, "dbo.sp_pager06", prms);
                Records = Convert.ToInt32(prms[6].Value.ToString());
                return ds;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                Records = 0;
                return new DataSet();
            }
        }
        public DataSet GetUserWithHaveFun()
        {
            DataSet ds = null;
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT * FROM M_V_System_User
                                                    WHERE Role_ID <> 100
                                                    ORDER BY Role_ID");
            try
            {
                ds = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.Text, strSql.ToString(), null);
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
            }
            return ds;
        }
        /// <summary>
        /// 查询所有部门信息
        /// </summary>
        /// <remarks>
        /// author: zp
        /// createtime: 2010.07.14
        /// </remarks> 
        /// <returns></returns>
        public DataSet GetAllDept()
        {
            try
            {
                return SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.Text, "select * from Copy_DepartInfo");
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return new DataSet();
            }
        }
        /// <summary>
        /// 根据用户编号查询角色编号
        /// </summary>
        /// <remarks>
        /// author: zp
        /// createtime: 2010.07.13
        /// </remarks> 
        /// <param name="userid">用户编号</param>
        /// <returns></returns>
        public int GetRoleByUserID(String userid)
        {
            SqlParameter[] prms ={ 
               new SqlParameter("@userid",userid)
            };
            try
            {
                Object o = SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, "select User_RoleID from M_System_User_Role_Rel where User_ID=@userid", prms);
                return Convert.ToInt32(o.ToString());
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return -1;
            }
        }
        /// <summary>
        /// 查询所有角色
        /// </summary>
        /// <remarks>
        /// author: zp
        /// createtime: 2010.07.15
        /// </remarks> 
        /// <returns></returns>
        public DataSet GetAllRole()
        {
            try
            {
                return SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.Text, "select * from M_System_Role");
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return new DataSet();
            }
        }
        /// <summary>
        /// 根据用户编号修改角色
        /// </summary>
        /// <remarks>
        /// author: zp
        /// createtime: 2010.07.13
        /// </remarks> 
        /// <param name="userid">用户编号</param>
        /// <param name="roleid">修改后的角色编号</param>
        /// <returns></returns>
        public bool UpdateUserRole(String userid, int roleid)
        {
            SqlParameter[] prms ={ 
                new SqlParameter("@userid",userid),
                new SqlParameter("@roleid",roleid)
            };
            try
            {
                int i = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, "update M_System_User_Role_Rel set User_RoleID=@roleid where User_ID=@userid", prms);
                if (i != 1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
        }
        /// <summary>
        /// 删除用户已分配的权限
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public Boolean DelUserFunctionRel(String userid)
        {
            SqlParameter[] prms ={ 
               new SqlParameter("@userid",userid)
            };
            try
            {
                int i = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, "delete from M_System_User_Fun_Rel where UFRel_UserID=@userid", prms);
                if (i < 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
        }
        /// <summary>
        /// 为用户分配权限
        /// <remarks>
        /// create by zp
        /// create time 2010-07-15
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        public Boolean SetUserFunctionRel(String userid, int roleid)
        {
            SqlParameter[] prms ={
               new SqlParameter("@userid",userid),
               new SqlParameter("@roleid",roleid)
            };
            try
            {
                SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, "insert into dbo.M_System_User_Fun_Rel select RFRel_FunctionID,@userid from  dbo.M_System_Role_Fun_Rel where RFRel_RoleID=@roleid", prms);
                return true;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
        }
        /// <summary>
        /// 查询用户编号是否已存在
        /// </summary>
        /// <remarks>
        /// author: zp
        /// createtime: 2010.07.13
        /// </remarks> 
        /// <param name="userid">用户编号</param>
        /// <returns></returns>
        public bool GetUserIsHaveByUserID(String userid)
        {
            SqlParameter[] prms ={
                new SqlParameter("@userid",userid)
            };
            try
            {
                Object o = SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, "select count(*) from M_System_User_Role_Rel where User_ID=@userid", prms);
                if (o.ToString() == "0")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"IF EXISTS(SELECT * FROM M_System_User_Role_Rel WHERE User_ID=@userid)
                                        UPDATE M_System_User_Role_Rel SET User_RoleID = @roleid WHERE USER_ID =@userid
                                        ELSE
                                        INSERT INTO M_System_User_Role_Rel(User_ID,User_RoleID,User_ShowName) VALUES(@userid,@roleid,'')");

            SqlParameter[] prms ={ 
              new SqlParameter("@userid",userid),
              new SqlParameter("@roleid",roleid)
            };
            try
            {
                int i = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, strSql.ToString(), prms);
                if (i == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
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
            userid = "0";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT User_ID FROM M_System_User WHERE User_Name=@User_Name AND User_Pwd=@User_Pwd");

            SqlParameter[] prms = { 
                                  new SqlParameter("@User_Name", username),
                                  new SqlParameter("@User_Pwd",pwd)
                                  };
            try
            {
                DataTable dt = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.Text, sb.ToString(), prms).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    userid = dt.Rows[0]["User_ID"].ToString();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
        }
        #endregion by perry

        /// <summary>
        /// 查询分页/不分页两种
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="totalcnt"></param>
        /// <returns></returns>
        public DataTable QueryAdminTable(AdminSearchEntity entity, out int totalcnt)
        {
            DataTable table = null;
            totalcnt = 0;

            try
            {
                SqlParameter[] prms = ParseToSqlParameters(entity).ToArray();

                if (entity.UseDBPagination)
                {
                    table = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.StoredProcedure, "sp_pager06", prms).Tables[0];
                    totalcnt = int.Parse(prms[prms.Length - 1].Value.ToString());
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            catch (Exception ex)
            {
                totalcnt = -1;
                LogUtil.WriteLog(ex);
            }

            return table;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Insert(AdminEntity entity)
        {
            bool isSuccess = false;

            try
            {
                SqlParameter[] prms = {
                                          new SqlParameter("@TrueName",SqlDbType.NVarChar,20),
                                          new SqlParameter("@User_Name",SqlDbType.NVarChar,20),
                                          new SqlParameter("@User_Pwd",SqlDbType.NVarChar,100),
                                          new SqlParameter("@User_Sex",SqlDbType.Int),
                                          new SqlParameter("@User_Tel",SqlDbType.NVarChar,15),
                                          new SqlParameter("@User_Tel_Private",SqlDbType.NVarChar,15),
                                          new SqlParameter("@User_Mobile",SqlDbType.NVarChar,15),
                                          new SqlParameter("@User_Mail",SqlDbType.NVarChar,50),
                                          new SqlParameter("@User_PhotoPath",SqlDbType.NVarChar,200),
                                          new SqlParameter("@User_DeptID",SqlDbType.Int),
                                          new SqlParameter("@RegisterDate",SqlDbType.DateTime),
                                          new SqlParameter("@IsValid",SqlDbType.Bit),
                                          new SqlParameter("@User_PositionID",SqlDbType.Int),
                                       };
                prms[0].Value = entity.TrueName;
                prms[1].Value = entity.User_Name;
                prms[2].Value = entity.User_Pwd;
                prms[3].Value = entity.User_Sex;
                prms[4].Value = entity.User_Tel;
                prms[5].Value = entity.User_Tel_Private;
                prms[6].Value = entity.User_Mobile;
                prms[7].Value = entity.User_Mail;
                prms[8].Value = entity.User_PhotoPath;
                prms[9].Value = entity.User_DeptID;
                prms[10].Value = entity.RegisterDate;
                prms[11].Value = entity.IsValid;
                prms[12].Value = entity.User_PositionID;

                StringBuilder sbSql = new StringBuilder();
                sbSql.Append(@" INSERT INTO M_System_User(TrueName,User_Name,User_Pwd,User_Sex,User_Tel, User_Tel_Private,User_Mobile,User_Mail,
                                            User_PhotoPath,User_DeptID,RegisterDate,IsValid,User_PositionID)");
                sbSql.Append("VALUES(");
                sbSql.Append(@"@TrueName,@User_Name,@User_Pwd,@User_Sex,@User_Tel, @User_Tel_Private,@User_Mobile,@User_Mail,
                                @User_PhotoPath,@User_DeptID,@RegisterDate,@IsValid,@User_PositionID");
                sbSql.Append(" )");

                isSuccess = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, sbSql.ToString(), prms) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }

            return isSuccess;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(AdminEntity entity)
        {
            bool isSuccess = false;

            try
            {
                SqlParameter[] prms = {
                                          new SqlParameter("@TrueName",SqlDbType.NVarChar,20),
                                          new SqlParameter("@User_Name",SqlDbType.NVarChar,20),
                                          new SqlParameter("@User_Pwd",SqlDbType.NVarChar,100),
                                          new SqlParameter("@User_Sex",SqlDbType.Int),
                                          new SqlParameter("@User_Tel",SqlDbType.NVarChar,15),
                                          new SqlParameter("@User_Tel_Private",SqlDbType.NVarChar,15),
                                          new SqlParameter("@User_Mobile",SqlDbType.NVarChar,15),
                                          new SqlParameter("@User_Mail",SqlDbType.NVarChar,50),
                                          new SqlParameter("@User_PhotoPath",SqlDbType.NVarChar,200),
                                          new SqlParameter("@User_DeptID",SqlDbType.Int),
                                          new SqlParameter("@RegisterDate",SqlDbType.DateTime),
                                          new SqlParameter("@IsValid",SqlDbType.Bit),
                                          new SqlParameter("@User_PositionID",SqlDbType.Int),
                                          new SqlParameter("@User_ID",SqlDbType.Int)
                                       };
                prms[0].Value = entity.TrueName;
                prms[1].Value = entity.User_Name;
                prms[2].Value = entity.User_Pwd;
                prms[3].Value = entity.User_Sex;
                prms[4].Value = entity.User_Tel;
                prms[5].Value = entity.User_Tel_Private;
                prms[6].Value = entity.User_Mobile;
                prms[7].Value = entity.User_Mail;
                prms[8].Value = entity.User_PhotoPath;
                prms[9].Value = entity.User_DeptID;
                prms[10].Value = entity.RegisterDate;
                prms[11].Value = entity.IsValid;
                prms[12].Value = entity.User_PositionID;
                prms[13].Value = entity.User_ID;

                StringBuilder sbSql = new StringBuilder();
                sbSql.Append(@" UPDATE M_System_User SET");
                sbSql.Append(" TrueName=@TrueName,");
                sbSql.Append(" User_Name=@User_Name,");
                sbSql.Append(" User_Pwd=@User_Pwd,");
                sbSql.Append(" User_Sex=@User_Sex,");
                sbSql.Append(" User_Tel=@User_Tel,");
                sbSql.Append(" User_Tel_Private=@User_Tel_Private,");
                sbSql.Append(" User_Mobile=@User_Mobile,");
                sbSql.Append(" User_Mail=@User_Mail,");
                sbSql.Append(" User_PhotoPath=@User_PhotoPath,");
                sbSql.Append(" User_DeptID=@User_DeptID,");
                sbSql.Append(" RegisterDate=@RegisterDate,");
                sbSql.Append(" IsValid=@IsValid,");
                sbSql.Append(" User_PositionID=@User_PositionID");
                sbSql.Append(" WHERE User_ID=@User_ID");

                isSuccess = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, sbSql.ToString(), prms) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }

            return isSuccess;
        }

        /// <summary>
        /// 删除系统管理员
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool Delete(int userID)
        {
            bool isSuccess = false;
            SqlTransaction trans = null;

            using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
            {
                try
                {
                    conn.Open();
                    trans = conn.BeginTransaction();

                    SqlParameter[] prms = {
                                               new SqlParameter("@User_ID", SqlDbType.Int)
                                           };
                    prms[0].Value = userID;

                    string delUserSql = " DELETE FROM M_System_User WHERE [User_ID]=@User_ID";
                    string delUser_Fun_RelSql = "DELETE FROM M_System_User_Fun_Rel WHERE UFRel_UserID=@User_ID";
                    string delUser_Role_RelSql = "DELETE FROM M_System_User_Role_Rel WHERE [User_ID]=@User_ID";

                    SQlHelper.ExecuteNonQuery(trans, CommandType.Text, delUserSql, prms);
                    SQlHelper.ExecuteNonQuery(trans, CommandType.Text, delUser_Fun_RelSql, prms);
                    SQlHelper.ExecuteNonQuery(trans, CommandType.Text, delUser_Role_RelSql, prms);

                    trans.Commit();
                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    if (trans != null)
                    {
                        trans.Rollback();
                    }
                    LogUtil.WriteLog(ex);
                    return false;
                }
            }

            return isSuccess;
        }

        /// <summary>
        /// 判断邮箱是否可用
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool IsUseableByEmail(string email, int userID)
        {
            bool isUseable = false; //true 可用 false已存在

            try
            {
                string sql = null;
                object result = null;
                if (userID > 0)
                {
                    SqlParameter[] prms =
                                          {
                                              new SqlParameter("@User_Mail", SqlDbType.NVarChar, 50),
                                              new SqlParameter("@User_ID", SqlDbType.Int)
                                          };
                    prms[0].Value = email;
                    prms[1].Value = userID;

                    sql = "SELECT COUNT(User_Mail) FROM M_System_User WHERE [User_ID]!=@User_ID AND  User_Mail=@User_Mail";
                    result = SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, sql, prms);
                }
                else
                {
                    SqlParameter[] prms =
                                          {
                                              new SqlParameter("@User_Mail", SqlDbType.NVarChar, 50)
                                          };
                    prms[0].Value = email;
                    sql = "SELECT COUNT(User_Mail) FROM M_System_User WHERE User_Mail=@User_Mail";
                    result = SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, sql, prms);
                }


                if (result != null && Convert.ToInt32(result) == 0)
                {
                    isUseable = true;
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }

            return isUseable;
        }

        /// <summary>
        /// 判断用户是否可以注册
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool IsUseableByUsername(string username)
        {
            bool isUseable = false; //true 可用 false已存在

            try
            {
                SqlParameter[] prms = {
                                          new SqlParameter("@User_Name", SqlDbType.NVarChar, 20)
                                      };
                prms[0].Value = username;
                string sql = "SELECT COUNT(User_Name) FROM M_System_User WHERE User_Name=@User_Name";

                object result = SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, sql, prms);

                if (result != null && Convert.ToInt32(result) == 0)
                {
                    isUseable = true;
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }

            return isUseable;
        }

        /// <summary>
        /// 获取管理员实体
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public AdminEntity QuerySysUserEntity(int userID)
        {
            string sql = "SELECT * FROM M_System_User WHERE [User_ID]=@User_ID";

            using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
            {
                return conn.Query<AdminEntity>(sql, new { User_ID = userID }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 修改管理员密码
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool UpdatePassword(int userID, string password)
        {
            bool isSuccess = false;

            try
            {
                SqlParameter[] prms = {
                                        new SqlParameter("@User_ID", SqlDbType.Int),
                                        new SqlParameter("@User_Pwd", SqlDbType.NVarChar,100)
                                    };
                prms[0].Value = userID;
                prms[1].Value = password;

                string sql = "UPDATE M_System_User SET User_Pwd=@User_Pwd WHERE User_ID=@User_ID";
                isSuccess = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, sql, prms) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);                
            }

            return isSuccess;
        }

        /// <summary>
        /// 初始化系统数据
        /// </summary>
        /// <returns></returns>
        public bool InitDataBase()
        {
            bool isSuccess = false;

            try
            {
                isSuccess = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.StoredProcedure, "M_PROC_InitDataBase") >= 0;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }

            return isSuccess;
        }

        #region 构造查询条件
        /// <summary>
        /// 生成拼接sql参数列表
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public List<SqlParameter> ParseToSqlParameters(AdminSearchEntity entity)
        {
            List<SqlParameter> paraList = new List<SqlParameter>();
            //table
            paraList.Add(CPTable(entity));

            //fields
            paraList.Add(CPFields(entity));

            //filter_SqlWhere
            paraList.Add(CPWhere(entity));

            //order
            paraList.Add(CPOrder(entity));

            //pagesize
            paraList.Add(new SqlParameter("@pageSize", entity.PageSize));

            //pageindex
            paraList.Add(new SqlParameter("@pageIndex", entity.PageIndex));

            paraList.Add(new SqlParameter() { ParameterName = "@Records", Value = 0, Direction = ParameterDirection.Output });

            return paraList;
        }

        /// <summary>
        /// 查询条件
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private SqlParameter CPWhere(AdminSearchEntity entity)
        {
            StringBuilder sbwhere = new StringBuilder(" User_ID != 1 ");

            if (!string.IsNullOrEmpty(entity.LoginName))
            {
                sbwhere.Append(" AND [User_Name] like '" + entity.LoginName + "%'");
            }

            if (!string.IsNullOrEmpty(entity.TrueName))
            {
                sbwhere.Append(" AND TrueName like '" + entity.TrueName + "%'");
            }
            return new SqlParameter("@sqlWhere", sbwhere.ToString());
        }

        /// <summary>
        /// 设置排序
        /// </summary>
        /// <param name="enity"></param>
        /// <returns></returns>
        private SqlParameter CPOrder(AdminSearchEntity enity)
        {
            StringBuilder sborder = new StringBuilder();

            if (enity.OrderfieldType == OrderFieldType.Desc)
            {
                sborder.Append(" User_ID DESC ");
            }
            else
            {
                sborder.Append(" User_ID ASC ");
            }

            return new SqlParameter("@OrderField", sborder.ToString());
        }

        /// <summary>
        /// 设置显示字段
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private SqlParameter CPFields(AdminSearchEntity entity)
        {
            StringBuilder sbfileds = new StringBuilder();
            if (entity.UseDBPagination)
            {
                sbfileds.Append(@" User_ID, User_Name, TrueName, User_ShowName, Dept_Name, Role_Name, User_Tel, CONVERT(NVARCHAR(19),RegisterDate,121) AS RegisterDate ");
            }
            else
            {
                throw new NotImplementedException();
            }
            return new SqlParameter("@Fields", sbfileds.ToString());
        }

        /// <summary>
        /// 设置表关联 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private SqlParameter CPTable(AdminSearchEntity entity)
        {
            StringBuilder sbtable = new StringBuilder();
            //基本表
            sbtable.Append(" M_V_System_UserSimple ");
            return new SqlParameter("@TableName", sbtable.ToString());
        }
        #endregion
    }
}
