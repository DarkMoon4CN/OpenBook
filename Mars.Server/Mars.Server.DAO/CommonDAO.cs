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
    public class CommonDAO
    {
        public bool AddFeedback(FeedbackEntity info)
        {
            SqlParameter[] prms ={ 
                new SqlParameter("@Content",info.Content),
                new SqlParameter("@UserID",info.UserID),
                new SqlParameter("@ContactMethod",info.ContactMethod)
            };
            string sql = "insert into M_User_Feedback(Content,UserID,CreateTime,ContactMethod) values(@Content,@UserID,getdate(),@ContactMethod)";
            try
            {
                SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, sql, prms);
                return true;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
        }

        public AppUpdateEntity TryGetNewVer(int apptype)
        {
            try
            {
                string sql = "select top 1 * from M_AppUpdate where appType=@appType order by createTime desc";
                using(SqlConnection con=new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return con.Query<AppUpdateEntity>(sql,new{appType=apptype}).FirstOrDefault();
                }
            }
            catch ( Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        public AppUpdateEntity TryGetNewVer(int apptype, int versiontype)
        {
            try
            {
                string sql = "select top 1 * from M_AppUpdate where appType=@appType and versionType=@versionType   order by createTime desc";
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return con.Query<AppUpdateEntity>(sql, new { appType = apptype, versionType = versiontype }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 根据版本号，判断是否进行审核
        /// </summary>
        /// <param name="versionCode"></param>
        /// <returns></returns>
        public bool IsVersionCheck(string versionCode)
        {
            bool returnValue = true;
            StringBuilder strSql = new StringBuilder();
            try
            {
                strSql.AppendFormat(@"SELECT [VerState] FROM [M_Version_CheckState] WHERE VersionCode = '{0}'"
                , versionCode);

                object obj = SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, strSql.ToString(), null);
                if (obj != null)
                {
                    if (int.Parse(obj.ToString()) == 0)
                    {
                        returnValue = false;
                    }
                }
                else
                {
                    returnValue = false;
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return true;
            }

            return returnValue;
        }

        public AppUpdateEntity TryGetVersion(int appId)
        {
            try
            {
                string sql = "select * from M_AppUpdate where appId=@appId";
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return con.Query<AppUpdateEntity>(sql, new { appId = appId }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }


        }
        public DataTable GetAppUpdateList(searchAppUpdateEntity entity, out int totalcnt)
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
        #region 构造查询条件
        /// <summary>
        /// 生成拼接sql参数列表
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public List<SqlParameter> ParseToSqlParameters(searchAppUpdateEntity entity)
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
        private SqlParameter CPWhere(searchAppUpdateEntity entity)
        {
            StringBuilder sbwhere = new StringBuilder("1=1");
            if (entity.selAppType != 0)
            {
                sbwhere.Append("and appType='" + entity.selAppType + "'");
            }
            if (!string.IsNullOrEmpty(entity.txtVersion))
            {
                sbwhere.Append("and version='" + entity.txtVersion + "'");
            }
            if (!string.IsNullOrEmpty(entity.txtStartTime))
            {
                sbwhere.Append("and createTime >='" + entity.txtStartTime + "'");
            }
            if (!string.IsNullOrEmpty(entity.txtEndTime))
            {
                sbwhere.Append("and createTime <='" + entity.txtEndTime + "'");
            }
            return new SqlParameter("@sqlWhere", sbwhere.ToString());
        }

        /// <summary>
        /// 设置排序
        /// </summary>
        /// <param name="enity"></param>
        /// <returns></returns>
        private SqlParameter CPOrder(searchAppUpdateEntity enity)
        {
            StringBuilder sborder = new StringBuilder();

            if (enity.OrderfieldType == OrderFieldType.Desc)
            {
                sborder.Append(" createTime  DESC ");
            }
            else
            {
                sborder.Append(" createTime  ASC ");
            }

            return new SqlParameter("@OrderField", sborder.ToString());
        }
        /// <summary>
        /// 设置显示字段
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private SqlParameter CPFields(searchAppUpdateEntity entity)
        {
            StringBuilder sbfileds = new StringBuilder();
            if (entity.UseDBPagination)
            {
                sbfileds.Append(@" appId,appType,version,downloadUrl,forcedUpdate,convert(varchar(19),createTime,121) as createTime,appSize,updateProfile");
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
        private SqlParameter CPTable(searchAppUpdateEntity entity)
        {
            StringBuilder sbtable = new StringBuilder();
            //基本表
            sbtable.Append(" M_AppUpdate ");
            return new SqlParameter("@TableName", sbtable.ToString());
        }
        #endregion
        public bool UpdateAppVersion(AppUpdateEntity entity)
        {
            string sql = "";
            if (entity.AppId == 0)
            {
                sql = "insert into M_AppUpdate (appType,version,downloadUrl,forcedUpdate,createTime,appSize,updateProfile) values (@appType,@version,@downloadUrl,@forcedUpdate,getDate(),@appSize,@updateProfile)";
            }
            else {
                sql = "update M_AppUpdate set appType=@appType,version=@version,downloadUrl=@downloadUrl,createTime=getdate(),appSize=@appSize,updateProfile=@updateProfile,forcedUpdate=@forcedUpdate where appid=@appid";
            } 
            SqlParameter[] pars ={new SqlParameter("@appType",SqlDbType.Int),
                                 new SqlParameter("@version",SqlDbType.NVarChar,50),
                                 new SqlParameter("@downloadUrl",SqlDbType.NVarChar,500),
                                 new SqlParameter("@appSize",SqlDbType.Int),
                                 new SqlParameter("@updateProfile",SqlDbType.NVarChar,500),
                                 new SqlParameter("@forcedUpdate",SqlDbType.Bit),
                                 new SqlParameter("@appid",SqlDbType.Int)
                                 };
            pars[0].Value = entity.AppType;
            pars[1].Value = entity.Version;
            pars[2].Value = entity.DownloadUrl;
            pars[3].Value = entity.AppSize;
            pars[4].Value = entity.UpdateProfile;
            pars[5].Value = entity.ForcedUpdate;
            pars[6].Value = entity.AppId;
            try
            {
                SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, sql, pars);
                return true;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 删除系统升级记录
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        public string deleteAppUpdate(int appid)
        {
            string flg = "0";
            string sql = "delete  from M_AppUpdate where appId=@appId";
            SqlParameter[] pars = { new SqlParameter("appid", appid) }; 
            try
            {
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    SQlHelper.ExecuteNonQuery(conn, CommandType.Text, sql, pars);
                    flg = "1";
                }
            }
            catch (Exception e)
            {
                flg = "0";
                LogUtil.WriteLog(e);
            }
            return flg;
        }

    }
}
