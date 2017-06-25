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
    public class FunctionDAO
    {
        public DataTable QueryFunctionTable(FunctionSearchEntity entity, out int totalcnt)
        {
            totalcnt = 0;
            DataTable table = null;

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
                LogUtil.WriteLog(ex);
                return null;
            }

            return table;
        }

        /// <summary>
        /// 获取角色关联的菜单集
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public List<FunctionEntity> QueryFunctiolnList(int roleID)
        {
            string sql = "SELECT fun.* FROM dbo.M_System_Function fun";
            sql += " INNER JOIN M_System_Role_Fun_Rel rolefunrel ON fun.Function_ID=rolefunrel.RFRel_FunctionID";
            sql += " WHERE rolefunrel.RFRel_RoleID=@RFRel_RoleID";

            try
            {
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return conn.Query<FunctionEntity>(sql, new { RFRel_RoleID = roleID }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 查询所有菜单
        /// </summary>
        /// <returns></returns>
        public List<FunctionEntity> QueryFunctionList()
        {
            List<FunctionEntity> list = null;

            try
            {
                string sql = "SELECT * FROM M_System_Function";

                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return conn.Query<FunctionEntity>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }

            return list;
        }

        /// <summary>
        /// 查询菜单实体
        /// </summary>
        /// <param name="functionID"></param>
        /// <returns></returns>
        public FunctionEntity QueryFunction(int functionID)
        {
            try
            {
                string sql = "SELECT * FROM M_System_Function WHERE Function_ID=@Function_ID";

                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return conn.Query<FunctionEntity>(sql, new { Function_ID = functionID }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 新建功能菜单 
        /// 默认给管理员角色添加该权限菜单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Insert(FunctionEntity entity)
        {
            bool isSuccess = false;
            SqlTransaction trans = null;

            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(" INSERT INTO M_System_Function");
            sbSql.Append(" (Function_Name,Function_URL,Function_ParentID,Function_Order,Function_isValid,Function_Level,Function_URL_New,Function_IsNew,CreateDate)");
            sbSql.Append(" VALUES(");
            sbSql.Append(" @Function_Name,@Function_URL,@Function_ParentID,@Function_Order,@Function_isValid,@Function_Level,@Function_URL_New,@Function_IsNew,@CreateDate");
            sbSql.Append(" );");
            sbSql.Append(" SELECT @@IDENTITY;");

            SqlParameter[] prms = {
                                      new SqlParameter("@Function_Name", SqlDbType.NVarChar,20),
                                      new SqlParameter("@Function_URL", SqlDbType.NText),
                                      new SqlParameter("@Function_ParentID", SqlDbType.Int),
                                      new SqlParameter("@Function_Order",SqlDbType.NVarChar,20),
                                      new SqlParameter("@Function_isValid",SqlDbType.Int),
                                      new SqlParameter("@Function_Level",SqlDbType.Int),
                                      new SqlParameter("@Function_URL_New",SqlDbType.NVarChar,200),
                                      new SqlParameter("@Function_IsNew",SqlDbType.Int),
                                      new SqlParameter("@CreateDate",SqlDbType.DateTime)
                                   };

            prms[0].Value = entity.Function_Name;
            prms[1].Value = entity.Function_URL;
            prms[2].Value = entity.Function_ParentID;
            prms[3].Value = entity.Function_Order;
            prms[4].Value = entity.Function_isValid;
            prms[5].Value = entity.Function_Level;
            prms[6].Value = entity.Function_URL_New;
            prms[7].Value = entity.Function_IsNew;
            prms[8].Value = entity.CreateDate;

            try
            {
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    conn.Open();
                    trans = conn.BeginTransaction();

                    object result = SQlHelper.ExecuteScalar(trans, CommandType.Text, sbSql.ToString(), prms);
                    int functionID = Convert.ToInt32(result);

                    SqlParameter[] rolePrms = {
                                          new SqlParameter("@FunctionID",functionID),
                                          new SqlParameter("@RoleID",100) //100是管理员角色ID
                                      };

                    //管理员添加该菜单权限
                    string insertRoleFunRelSql = "INSERT INTO M_System_Role_Fun_Rel(RFRel_FunctionID, RFRel_RoleID) VALUES(@FunctionID, @RoleID)";
                    int effectline = SQlHelper.ExecuteNonQuery(trans, CommandType.Text, insertRoleFunRelSql, rolePrms);
                   
                    string searchUserRoleRelSql = "SELECT [User_ID] FROM M_System_User_Role_Rel WHERE User_RoleID=@RoleID";
                    DataTable table = SQlHelper.ExecuteDataset(trans, CommandType.Text, searchUserRoleRelSql, rolePrms).Tables[0];
                    if (table != null && table.Rows.Count > 0)
                    {
                        string insertUserFunRelSql = "INSERT INTO M_System_User_Fun_Rel(UFRel_FunctionID, UFRel_UserID) VALUES(@FunctionID,@UserID)";
                        List<int> userIdList = table.AsEnumerable().Select(row => int.Parse(row.Field<string>("User_ID"))).ToList();

                        foreach (int userid in userIdList)
                        {
                            SqlParameter[] userPrms = {
                                                        new SqlParameter("@FunctionID",functionID),
                                                        new SqlParameter("@UserID",userid)
                                                  };
                            int resultnum = SQlHelper.ExecuteNonQuery(trans, CommandType.Text, insertUserFunRelSql, userPrms);
                        }                     
                    }

                    isSuccess = true;
                    trans.Commit();
                }
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
            return isSuccess;
        }

        public bool Update(FunctionEntity entity)
        {
            bool isSuccess = false;

            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(" UPDATE M_System_Function");
            sbSql.Append(" SET");
            sbSql.Append(" Function_Name=@Function_Name,Function_URL=@Function_URL,Function_ParentID=@Function_ParentID,Function_Order=@Function_Order,");
            sbSql.Append(" Function_isValid=@Function_isValid,Function_Level=@Function_Level,Function_URL_New=@Function_URL_New,Function_IsNew=@Function_IsNew,CreateDate=@CreateDate");
            sbSql.Append(" WHERE  Function_ID=@Function_ID");

            SqlParameter[] prms = {
                                      new SqlParameter("@Function_Name", SqlDbType.NVarChar,20),
                                      new SqlParameter("@Function_URL", SqlDbType.NText),
                                      new SqlParameter("@Function_ParentID", SqlDbType.Int),
                                      new SqlParameter("@Function_Order",SqlDbType.NVarChar,20),
                                      new SqlParameter("@Function_isValid",SqlDbType.Int),
                                      new SqlParameter("@Function_Level",SqlDbType.Int),
                                      new SqlParameter("@Function_URL_New",SqlDbType.NVarChar,200),
                                      new SqlParameter("@Function_IsNew",SqlDbType.Int),
                                      new SqlParameter("@CreateDate",SqlDbType.DateTime),
                                      new SqlParameter("@Function_ID",SqlDbType.Int)
                                   };

            prms[0].Value = entity.Function_Name;
            prms[1].Value = entity.Function_URL;
            prms[2].Value = entity.Function_ParentID;
            prms[3].Value = entity.Function_Order;
            prms[4].Value = entity.Function_isValid;
            prms[5].Value = entity.Function_Level;
            prms[6].Value = entity.Function_URL_New;
            prms[7].Value = entity.Function_IsNew;
            prms[8].Value = entity.CreateDate;
            prms[9].Value = entity.Function_ID;

            try
            {
                isSuccess = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, sbSql.ToString(), prms) > 0;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
            return isSuccess;
        }

        /// <summary>
        /// 删除功能菜单
        /// 如果功能菜单已被用户使用，则不可删除
        /// </summary>
        /// <param name="functionID"></param>
        /// <returns></returns>
        public bool Delete(int functionID)
        {
            bool isSuccess = false;
            SqlTransaction trans = null;

            string selectFunsql = "SELECT Function_Level FROM M_System_Function WHERE Function_ID=@Function_ID";
            string delFunsql = "DELETE FROM M_System_Function WHERE Function_ID=@Function_ID";
            string delFunSecondLevelsql = "DELETE FROM M_System_Function WHERE Function_ParentID=@Function_ID";
            string delRoleFunRelsql = "DELETE FROM M_System_Role_Fun_Rel WHERE RFRel_FunctionID=@Function_ID";
            string delUserFunRelsql = "DELETE FROM M_System_User_Fun_Rel WHERE UFRel_FunctionID=@Function_ID";

            SqlParameter[] prms = {
                                      new SqlParameter("@Function_ID",functionID)
                                  };

            try
            {
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    conn.Open();
                    trans = conn.BeginTransaction();

                    int funlevel =  Convert.ToInt32(SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, selectFunsql, prms));

                    if (funlevel == 1)
                    {
                        SQlHelper.ExecuteNonQuery(trans, CommandType.Text, delFunsql, prms);
                        SQlHelper.ExecuteNonQuery(trans, CommandType.Text, delFunSecondLevelsql, prms);
                        SQlHelper.ExecuteNonQuery(trans, CommandType.Text, delRoleFunRelsql, prms);
                        SQlHelper.ExecuteNonQuery(trans, CommandType.Text, delUserFunRelsql, prms);
                    }
                    else
                    {
                        SQlHelper.ExecuteNonQuery(trans, CommandType.Text, delFunsql, prms);
                        SQlHelper.ExecuteNonQuery(trans, CommandType.Text, delRoleFunRelsql, prms);
                        SQlHelper.ExecuteNonQuery(trans, CommandType.Text, delUserFunRelsql, prms);
                    }

                 
                    trans.Commit();
                    isSuccess = true;
                }
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

            return isSuccess;
        }

        /// <summary>
        /// 判断菜单名是否可用 true可以 false不可以
        /// </summary>
        /// <param name="functionID"></param>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public bool IsUseableFunctionName(int functionID, string functionName)
        {
            bool isCanuse = false;
            SqlParameter[] prms = {
                                      new SqlParameter("@Function_ID",functionID),
                                      new SqlParameter("@Function_Name",functionName)
                                  };

            try
            {
                string sql = string.Empty;

                if (functionID > 0)
                {
                    sql = "SELECT COUNT(Function_ID) FROM M_System_Function WHERE Function_ID!=@Function_ID AND Function_Name=@Function_Name";
                    object result = SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, sql, prms);

                    if (Convert.ToInt32(result) == 0)
                    {
                        isCanuse = true;
                    }
                }
                else
                {
                    sql = "SELECT COUNT(Function_ID) FROM M_System_Function WHERE Function_Name=@Function_Name";
                    object result = SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, sql, prms);

                    if (Convert.ToInt32(result) == 0)
                    {
                        isCanuse = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }

            return isCanuse;
        }

        /// <summary>
        /// 判断该菜单项是可否删除 true可删除  false不可删除
        /// </summary>
        /// <param name="functionID"></param>
        /// <returns></returns>
        public bool IsCanDelFunction(int functionID)
        {
            bool isCandel = false;
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(" SELECT COUNT(fun.Function_ID) FROM M_System_Function fun");
            sbSql.Append(" INNER JOIN M_System_User_Fun_Rel userfun ON fun.Function_ID=userfun.UFRel_FunctionID");
            sbSql.Append(" INNER JOIN M_V_System_User us ON us.[User_ID]=userfun.UFRel_UserID");
            sbSql.Append(" WHERE fun.Function_ID=@Function_ID");

            SqlParameter[] prms = {
                                      new SqlParameter("@Function_ID", functionID)
                                   };

            try
            {
                object result = SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, sbSql.ToString(), prms);
                if (Convert.ToInt32(result) == 0)
                {
                    isCandel = true;
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }

            return isCandel;
        }

        /// <summary>
        /// 获取第一级菜单
        /// </summary>
        /// <param name="funLevel">菜单层级</param>
        /// <returns></returns>
        public List<FunctionEntity> QueryFirstLevelList(int funLevel)
        {
            try
            {
                string sql = "SELECT Function_ID, Function_Name FROM M_System_Function WHERE Function_ParentID=0 AND Function_Level=@Function_Level";

                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return conn.Query<FunctionEntity>(sql, new { Function_Level = funLevel }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        #region 功能列表
        /// <summary>
        /// 根据用户编号查询拥有权限的所有节点
        /// </summary>
        /// <remarks>
        /// author: zp
        /// createtime: 2010.07.14
        /// </remarks> 
        /// <param name="userid">用户编号</param>
        /// <returns></returns>
        public DataSet GetFunctionRelUser(String userid)
        {
            SqlParameter[] prms ={ 
               new SqlParameter("@userid",userid)
            };
            try
            {
                return SQlHelper.ExecuteDataset(new SqlConnection(SQlHelper.MyConnectStr), CommandType.Text, "select * from [M_V_FunctionRelUser] where User_ID=@userid and Function_isValid=1", prms);
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return new DataSet();
            }
        }
        /// <summary>
        /// 根据角色编号查询默认权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public DataSet GetFunctionIDByRoleID(int roleid)
        {
            SqlParameter[] prms ={ 
               new SqlParameter("@roleid",roleid)
            };
            try
            {
                return SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.Text, "select RFRel_FunctionID from M_System_Role_Fun_Rel where RFRel_RoleID=@roleid", prms);
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return new DataSet();
            }
        }
        /// <summary>
        /// 查询所有权限
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllFunction()
        {
            try
            {
                return SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.Text, @"SELECT [Function_ID]
                                                                                    ,[Function_Name]
                                                                                    ,[Function_URL]
                                                                                    ,[Function_ParentID]
                                                                                    ,[Function_Order]
                                                                                    ,[Function_isValid]
                                                                                    ,[Function_Level]
                                                                                    ,[Function_URL_New] AS newurl
	                                                                                ,[Function_URL_New]
                                                                                    ,[Function_isNew]
                                                                                FROM [dbo].[M_System_Function]", null);
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return new DataSet();
            }
        }
        /// <summary>
        /// 编辑用户的权限(带事务)
        /// </summary>
        /// <remarks>
        /// author: zp
        /// createtime: 2010.07.16
        /// </remarks> 
        /// <param name="userid">用户编号</param>
        /// <param name="list">权限集合</param>
        /// <returns></returns>
        public bool EditUserFunRel(String userid, List<FunctionEntity> list)
        {
            #region
            SqlTransaction trans = null;
            using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
            {
                SqlParameter[] prms ={ 
                   new SqlParameter("@userid",userid)
                };
                conn.Open();
                trans = conn.BeginTransaction();
                try
                {
                    if (SQlHelper.ExecuteNonQuery(trans, CommandType.Text, "delete from M_System_User_Fun_Rel where UFRel_UserID=@userid", prms) >= 0)
                    {
                        #region
                        foreach (FunctionEntity var in list)
                        {
                            SqlParameter[] prms_fun ={
                            new SqlParameter("@userid",userid),
                            new SqlParameter("@funid",var.Function_ID)
                        };
                            if (!(SQlHelper.ExecuteNonQuery(trans, CommandType.Text, "insert into M_System_User_Fun_Rel values(@funid,@userid)", prms_fun) >= 0))
                            {
                                trans.Rollback();
                                return false;
                            }
                        }
                        trans.Commit();
                        return true;
                        #endregion
                    }
                    else
                    {
                        trans.Rollback();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    trans.Rollback();
                    return false;
                }
            }
            #endregion
        }
        /// <summary>
        /// 根据用户编号查询信息维护员的所在维护员管理下的所有节点
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataSet GetInfoManagerWithFunByID(String userid)
        {
            SqlParameter[] prms ={ 
               new SqlParameter("@userid",userid)
            };
            try
            {
                return SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.Text, "select UFRel_FunctionID from M_System_User_Fun_Rel where UFRel_UserID=@userid and UFRel_FunctionID>=4000 and UFRel_FunctionID<5000", prms);
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return new DataSet();
            }
        }
        /// <summary>
        /// 为信息维护员删除某一节点的权限
        /// </summary>
        /// <remarks>
        /// create by perry
        /// create time 2010-07-22
        /// </remarks>
        /// <param name="userid"></param>
        /// <returns></returns>
        public Boolean DelInfoManagerWithFun(String userid, int functionid)
        {
            SqlParameter[] prms ={ 
               new SqlParameter("@userid",userid),
               new SqlParameter("@functionid",functionid)
            };
            try
            {
                int i = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, "delete from M_System_User_Fun_Rel where UFRel_UserID=@userid and UFRel_FunctionID=@functionid", prms);
                if (i == 1)
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
        /// 为信息维护员添加某一节点的权限
        /// </summary>
        /// <remarks>
        /// create by perry
        /// create time 2010-07-22
        /// </remarks>
        /// <param name="userid"></param>
        /// <returns></returns>
        public Boolean AddInfoManagerWithFun(String userid, int functionid)
        {
            SqlParameter[] prms ={ 
               new SqlParameter("@userid",userid),
               new SqlParameter("@functionid",functionid)
            };
            try
            {
                int i = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, "insert into M_System_User_Fun_Rel(UFRel_FunctionID,UFRel_UserID) values(@functionid,@userid)", prms);
                if (i == 1)
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
        #endregion

        #region 构造查询条件
        /// <summary>
        /// 生成拼接sql参数列表
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public List<SqlParameter> ParseToSqlParameters(FunctionSearchEntity entity)
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
        private SqlParameter CPWhere(FunctionSearchEntity entity)
        {
            StringBuilder sbwhere = new StringBuilder(" 1=1 ");

            if (!string.IsNullOrEmpty(entity.Function_Name))
            {
                sbwhere.Append(" AND Function_Name like '" + entity.Function_Name + "%'");
            }

            return new SqlParameter("@sqlWhere", sbwhere.ToString());
        }

        /// <summary>
        /// 设置排序
        /// </summary>
        /// <param name="enity"></param>
        /// <returns></returns>
        private SqlParameter CPOrder(FunctionSearchEntity enity)
        {
            StringBuilder sborder = new StringBuilder();

            if (enity.OrderfieldType == OrderFieldType.Desc)
            {
                sborder.Append(" Function_ID DESC ");
            }
            else
            {
                sborder.Append(" Function_ID ASC ");
            }

            return new SqlParameter("@OrderField", sborder.ToString());
        }

        /// <summary>
        /// 设置显示字段
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private SqlParameter CPFields(FunctionSearchEntity entity)
        {
            StringBuilder sbfileds = new StringBuilder();
            if (entity.UseDBPagination)
            {
                sbfileds.Append(@" Function_ID, Function_Name, Function_URL_New, Function_ParentID, Function_Order,Function_isValid,Function_Level,Function_IsNew,CONVERT(NVARCHAR(19),CreateDate,121) AS CreateDate ");
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
        private SqlParameter CPTable(FunctionSearchEntity entity)
        {
            StringBuilder sbtable = new StringBuilder();
            //基本表
            sbtable.Append(" M_System_Function ");
            return new SqlParameter("@TableName", sbtable.ToString());
        }
        #endregion
    }
}
