using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Mars.Server.Entity;
using System.Data;
using System.Data.SqlClient;
using Mars.Server.Utils;

namespace Mars.Server.DAO
{
    public class SysRoleDAO
    {
        public DataTable QueryRoleTableByPage(SysRoleSearchEntity entity, out int totalcnt)
        {
            totalcnt = 0;
            DataTable table = null;

            try
            {
                if (entity.UseDBPagination)
                {
                    SqlParameter[] prms = ParseToSqlParameters(entity).ToArray();
                    table = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.StoredProcedure, "sp_pager06", prms).Tables[0];
                    totalcnt = int.Parse(prms[prms.Length - 1].Value.ToString());
                }
                else
                {
                    List<SqlParameter> prmsList = new List<SqlParameter>();
                    string sql = "SELECT Role_ID,Role_Name,Convert(nvarchar(19), CreateTime,121) AS CreateTime FROM M_System_Role ";
                    string sqlWhere = " WHERE 1=1 ";
                    string sqlOrder = " ORDER BY Role_ID Asc";
                    if (!string.IsNullOrEmpty(entity.Role_Name))
                    {
                        prmsList.Add(new SqlParameter("@Role_Name", entity.Role_Name));
                        sqlWhere += " AND Role_Name like @Role_Name + '%' ";
                    }

                    if (entity.OrderfieldType == OrderFieldType.Desc)
                    {
                        sqlOrder = " ORDER BY Role_ID Desc";
                    }

                    sql = sql + sqlWhere + sqlOrder;

                    table = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.Text, sql, prmsList.ToArray()).Tables[0];
                    totalcnt = table != null ? table.Rows.Count : 0;
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
        /// 获取实体
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public SysRoleEntity QueryEntity(int roleID)
        {
            using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
            {
                string sql = "SELECT * FROM M_System_Role WHERE Role_ID=@Role_ID";

                return conn.Query<SysRoleEntity>(sql, new { Role_ID = roleID }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Insert(SysRoleEntity entity)
        {
            try
            {
                string sql = " INSERT INTO M_System_Role(Role_Name, CreateTime) VALUES (@Role_Name, @CreateTime)";
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    conn.Execute(sql, new { Role_Name = entity.Role_Name, CreateTime = entity.CreateTime });
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
        /// 添加角色并和菜单关联
        /// </summary>
        /// <param name="roleEntity"></param>
        /// <param name="roleFunRelList"></param>
        /// <returns></returns>
        public bool Insert(SysRoleEntity roleEntity, List<SysRoleFunctionRelEntity> roleFunRelList)
        {
            bool isSuccess = false;
            SqlTransaction trans = null;

            try
            {
                SqlParameter[] rolePrms = {
                                              new SqlParameter("@Role_Name",SqlDbType.NVarChar,20),
                                              new SqlParameter("@CreateTime",SqlDbType.DateTime)                                             
                                          };
                rolePrms[0].Value = roleEntity.Role_Name;
                rolePrms[1].Value = roleEntity.CreateTime;


                string roleSql = " INSERT INTO M_System_Role(Role_Name, CreateTime) VALUES (@Role_Name, @CreateTime);SELECT @@IDENTITY; ";
                string roleFunRelSql = "INSERT INTO M_System_Role_Fun_Rel(RFRel_FunctionID, RFRel_RoleID) VALUES(@RFRel_FunctionID, @RFRel_RoleID)";

                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    conn.Open();
                    trans = conn.BeginTransaction();

                    object result = SQlHelper.ExecuteScalar(trans, CommandType.Text, roleSql, rolePrms);
                    int pkid = Convert.ToInt32(result);

                    foreach (SysRoleFunctionRelEntity entity in roleFunRelList)
                    {
                        SqlParameter[] roleFunRelPrms = {
                                                   new SqlParameter("@RFRel_FunctionID",entity.RFRel_FunctionID),
                                                   new SqlParameter("@RFRel_RoleID",pkid)
                                               };
                        SQlHelper.ExecuteNonQuery(trans, CommandType.Text, roleFunRelSql, roleFunRelPrms);
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

        public bool Update(SysRoleEntity entity)
        {
            try
            {
                string sql = " UPDATE M_System_Role SET Role_Name=@Role_Name, CreateTime=@CreateTime WHERE Role_ID=@Role_ID";
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    conn.Execute(sql, new { Role_Name = entity.Role_Name, CreateTime = entity.CreateTime, Role_ID = entity.Role_ID });
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
        /// 更新角色及关联菜单
        /// </summary>
        /// <param name="roleEntity"></param>
        /// <param name="roleFunRelList"></param>
        /// <returns></returns>
        public bool Update(SysRoleEntity roleEntity, List<SysRoleFunctionRelEntity> roleFunRelList)
        {
            bool isSuccess = false;
            SqlTransaction trans = null;

            try
            {
                SqlParameter[] rolePrms = {
                                              new SqlParameter("@Role_Name",SqlDbType.NVarChar,20),
                                              new SqlParameter("@CreateTime",SqlDbType.DateTime),
                                              new SqlParameter("@Role_ID",SqlDbType.Int)
                                          };
                rolePrms[0].Value = roleEntity.Role_Name;
                rolePrms[1].Value = roleEntity.CreateTime;
                rolePrms[2].Value = roleEntity.Role_ID;

                string roleSql = " UPDATE M_System_Role SET Role_Name=@Role_Name, CreateTime=@CreateTime WHERE Role_ID=@Role_ID";

                StringBuilder sbDealRoleFunRel = new StringBuilder();
                sbDealRoleFunRel.Append("IF EXISTS(SELECT RFRel_RoleID FROM M_System_Role_Fun_Rel WHERE RFRel_RoleID=@Role_ID)");
                sbDealRoleFunRel.Append(" DELETE FROM M_System_Role_Fun_Rel WHERE RFRel_RoleID=@Role_ID");

                #region 当修改角色时，管理员个性权限都将消失，和角色权限同步
                string delUserfunSql = @"DELETE FROM a FROM M_System_User_Fun_Rel a 
                                         INNER JOIN  M_System_User_Role_Rel b ON a.UFRel_UserID=b.[User_ID]                                         
                                         WHERE b.[User_ID]!=1 AND b.User_RoleID=@Role_ID";

                string selUserIDsByRole = "SELECT [User_ID] as UserID FROM M_System_User_Role_Rel WHERE [User_ID]!=1 AND User_RoleID=@Role_ID";

                string insertUserfunSql = "Insert Into M_System_User_Fun_Rel(UFRel_FunctionID,UFRel_UserID) Values(@FunctionID,@UserID)";
                #endregion

                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    conn.Open();
                    trans = conn.BeginTransaction();

                    SQlHelper.ExecuteNonQuery(trans, CommandType.Text, roleSql, rolePrms);
                    SQlHelper.ExecuteNonQuery(trans, CommandType.Text, sbDealRoleFunRel.ToString(), rolePrms);

                    string roleFunRelSql = "INSERT INTO M_System_Role_Fun_Rel(RFRel_FunctionID, RFRel_RoleID) VALUES(@RFRel_FunctionID, @RFRel_RoleID)";

                    //查询关联该角色所有用户ID
                    DataTable table = SQlHelper.ExecuteDataset(trans, CommandType.Text, selUserIDsByRole, rolePrms).Tables[0];
                    List<int> userIdList = null;
                    if (table != null && table.Rows.Count > 0)
                    {
                        userIdList = table.AsEnumerable().Select(dr => int.Parse(dr.Field<string>("UserID"))).ToList();
                        int lines = SQlHelper.ExecuteNonQuery(trans, CommandType.Text, delUserfunSql, rolePrms);
                    }

                    foreach (SysRoleFunctionRelEntity entity in roleFunRelList)
                    {
                        SqlParameter[] roleFunRelPrms = {
                                                   new SqlParameter("@RFRel_FunctionID",entity.RFRel_FunctionID),
                                                   new SqlParameter("@RFRel_RoleID",entity.RFRel_RoleID)
                                               };
                        SQlHelper.ExecuteNonQuery(trans, CommandType.Text, roleFunRelSql, roleFunRelPrms);

                        #region 修改角色时，同步该角色关联所有用户的所有权限菜单

                        if (table != null && table.Rows.Count > 0)
                        {
                            foreach (int userID in userIdList)
                            {
                                SqlParameter[] userrolePrms = {
                                                                  new SqlParameter("@UserID",userID),
                                                                  new SqlParameter("@FunctionID",entity.RFRel_FunctionID)
                                                              };
                                int lines1 = SQlHelper.ExecuteNonQuery(trans, CommandType.Text, insertUserfunSql, userrolePrms);
                            }
                        }

                        #endregion
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
        /// 删除角色 
        /// 执行此操作前，先调用 IsCanDelRole（）方法判断是否可以删除当前角色
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public bool Delete(int roleID)
        {
            bool isSuccess = false;
            SqlTransaction trans = null;

            string delRoleSql = "DELETE FROM M_System_Role WHERE Role_ID=@Role_ID";
            string delRoleFunRelSql = "DELETE FROM M_System_Role_Fun_Rel WHERE RFRel_RoleID=@Role_ID";

            SqlParameter[] prms = {
                                   new SqlParameter("@Role_ID",roleID)
                               };

            try
            {
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    conn.Open();
                    trans = conn.BeginTransaction();

                    SQlHelper.ExecuteNonQuery(trans, CommandType.Text, delRoleSql, prms);
                    SQlHelper.ExecuteNonQuery(trans, CommandType.Text, delRoleFunRelSql, prms);

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
        /// 判断是否存在相同角色名
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="rolename"></param>
        /// <returns></returns>
        public bool IsUseableRoleName(int roleID, string rolename)
        {
            bool isSuccess = false;
            SqlParameter[] prms = { 
                                     new SqlParameter("@Role_ID", roleID),
                                     new SqlParameter("@Role_Name", rolename)
                                     };
            try
            {
                if (roleID > 0)
                {
                    string sql = "SELECT COUNT(Role_ID) FROM M_System_Role WHERE Role_ID != @Role_ID AND Role_Name=@Role_Name";
                    object obj = SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, sql, prms);

                    if (obj != null && Convert.ToInt32(obj) == 0)
                    {
                        isSuccess = true;
                    }
                }
                else
                {

                    string sql = "SELECT COUNT(Role_ID) FROM M_System_Role WHERE Role_Name=@Role_Name";
                    object obj = SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, sql, prms);

                    if (obj != null && Convert.ToInt32(obj) == 0)
                    {
                        isSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
            return isSuccess;
        }

        /// <summary>
        /// 判断是否可以删除角色
        /// 当 用户没有使用该角色时方可删除
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public bool IsCanDelRole(int roleID)
        {
            bool isCanDel = false;

            try
            {
                SqlParameter[] prms = {
                                         new SqlParameter("@User_RoleID",roleID)
                                      };

                string sql = "SELECT COUNT(*) FROM M_System_User_Role_Rel WHERE User_RoleID=@User_RoleID";

                object result = SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, sql, prms);
                if (result != null && Convert.ToInt32(result) == 0)
                {
                    isCanDel = true;
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }

            return isCanDel;
        }

        #region 构造查询条件
        /// <summary>
        /// 生成拼接sql参数列表
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public List<SqlParameter> ParseToSqlParameters(SysRoleSearchEntity entity)
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
        private SqlParameter CPWhere(SysRoleSearchEntity entity)
        {
            StringBuilder sbwhere = new StringBuilder(" 1=1 ");

            if (!string.IsNullOrEmpty(entity.Role_Name))
            {
                sbwhere.Append(" AND Role_Name like '" + entity.Role_Name + "%'");
            }

            if (entity.Role_ID > 0)
            {
                sbwhere.Append(" AND Role_ID = " + entity.Role_ID + "");
            }
            return new SqlParameter("@sqlWhere", sbwhere.ToString());
        }

        /// <summary>
        /// 设置排序
        /// </summary>
        /// <param name="enity"></param>
        /// <returns></returns>
        private SqlParameter CPOrder(SysRoleSearchEntity enity)
        {
            StringBuilder sborder = new StringBuilder();

            if (enity.OrderfieldType == OrderFieldType.Desc)
            {
                sborder.Append(" Role_ID DESC ");
            }
            else
            {
                sborder.Append(" Role_ID ASC ");
            }

            return new SqlParameter("@OrderField", sborder.ToString());
        }

        /// <summary>
        /// 设置显示字段
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private SqlParameter CPFields(SysRoleSearchEntity entity)
        {
            StringBuilder sbfileds = new StringBuilder();
            if (entity.UseDBPagination)
            {
                sbfileds.Append(@" Role_ID, Role_Name, CONVERT(NVARCHAR(19),CreateTime,121) AS CreateTime ");
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
        private SqlParameter CPTable(SysRoleSearchEntity entity)
        {
            StringBuilder sbtable = new StringBuilder();
            //基本表
            sbtable.Append(" M_System_Role ");
            return new SqlParameter("@TableName", sbtable.ToString());
        }
        #endregion
    }
}
