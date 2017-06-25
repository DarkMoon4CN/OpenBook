using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Mars.Server.Utils;
using Mars.Server.Entity.Exhibition;

namespace Mars.Server.DAO.Exhibition
{
    public class ExhibitionDAO
    {
        /// <summary>
        /// 通过调用 up_getExhibitionIsPublish 存储过程来获取某个展会是否开始活动
        /// 存储过程中有临时写死的展会id
        /// </summary>
        /// <returns></returns>
        public bool IsPublished()
        {
            bool returnValue = false;

            object obj = SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.StoredProcedure, "up_getExhibitionIsPublish",null);
            if (obj != null)
            {
                returnValue = bool.Parse(obj.ToString());
            }

            return returnValue;
        }

        public DataTable QueryData(ExhibitionSearchEntity info, out int totalcnt)
        {
            try
            {
                SqlParameter[] prms = ParseToSqlParameters(info).ToArray();
                DataTable dt = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.StoredProcedure, "sp_pager05", prms).Tables[0];
                totalcnt = int.Parse(prms[prms.Length - 1].Value.ToString());

                return dt;
            }
            catch (Exception ex)
            {
                totalcnt = -1;
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        #region 构造SQL分页参数
        /// <summary>
        /// 核心方法。查询条件转换成sql参数
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        protected virtual List<SqlParameter> ParseToSqlParameters(ExhibitionSearchEntity sp)
        {
            List<SqlParameter> _parameters = new List<SqlParameter>();
            _parameters.Add(CreateParameter_Table(sp));
            _parameters.Add(CreateParameter_Fileds(sp));
            _parameters.Add(CreateParamter_Orderby(sp));
            _parameters.Add(CreateParameter_Where(sp));

            _parameters.Add(new SqlParameter("@pageSize", sp.PageSize));
            _parameters.Add(new SqlParameter("@pageIndex", sp.PageIndex));
            _parameters.Add(new SqlParameter() { ParameterName = "@Records", Value = 0, Direction = System.Data.ParameterDirection.Output });
            return _parameters;
        }

        /// <summary>
        /// 删除展会信息（假删除，讲展会信息的公共状态修改）
        /// </summary>
        /// <param name="exhibitionID"></param>
        /// <returns></returns>
        public bool DeleteExhibition(int exhibitionID)
        {
            bool returnValue = false;
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"UPDATE dbo.M_Exhibition_Main SET StateTypeID = 0 WHERE ExhibitionID = {0}"
                    , exhibitionID.ToString());

            returnValue = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, strSql.ToString(), null) > 0;

            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        protected virtual SqlParameter CreateParameter_Table(ExhibitionSearchEntity sp)
        {
            StringBuilder sbtable = new StringBuilder();
            sbtable.Append(@"dbo.M_Exhibition_Main AS em");

            return new SqlParameter("@TableName", sbtable.ToString());
        }

        protected virtual SqlParameter CreateParamter_Orderby(ExhibitionSearchEntity sp)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(" em.ExhibitionOrder ASC,em.ExhibitionID ASC ");

            return new SqlParameter("@OrderField", sb.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        protected virtual SqlParameter CreateParameter_Fileds(ExhibitionSearchEntity sp)
        {
            StringBuilder sbfileds = new StringBuilder();
            sbfileds.Append(@"*");
            return new SqlParameter("@Fields", sbfileds.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        protected virtual SqlParameter CreateParameter_Where(ExhibitionSearchEntity sp)
        {
            StringBuilder sbwhere = new StringBuilder(" 1=1 AND em.StateTypeID = 1");

            if (!string.IsNullOrEmpty(sp.ExhibitionTitle))
            {
                sbwhere.AppendFormat(" AND em.ExhibitionTitle LIKE '%{0}%'", sp.ExhibitionTitle);
            }

            return new SqlParameter("@sqlWhere", sbwhere.ToString());
        }

        #endregion

        public DataTable GetExhibitionTable(bool isAll = false)
        {
            DataTable table = null;
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" SELECT * FROM dbo.M_Exhibition_Main ");
            if (!isAll) {
                strSql.Append(@" WHERE StateTypeID = 1 ");
            }
            DataSet ds = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.Text, strSql.ToString(), null);
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    table = ds.Tables[0];
                }
            }
            return table;
        }

        public DataSet GetExhibitionDataSet(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@" SELECT * FROM dbo.M_Exhibition_Main WHERE ExhibitionID = {0};
                                  SELECT * FROM dbo.M_Exhibition_Advertisement WHERE ExhibitionID = {0};"
                                ,id.ToString());
            return SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.Text, strSql.ToString(), null);
           
        }

        public bool Update(ExhibitionEntity item)
        {
            bool returnValue = false;
            SqlTransaction trans = null;
            using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
            {
                try
                {
                    con.Open();
                    trans = con.BeginTransaction();
                    //清除位置记录
                    StringBuilder strDelLocSql = new StringBuilder();
                    strDelLocSql.AppendFormat(@"DELETE FROM M_Exhibition_Advertisement WHERE [ExhibitionID] = {0}", item.ExhibitionID.ToString());
                    SQlHelper.ExecuteNonQuery(trans, CommandType.Text, strDelLocSql.ToString(), null);

                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("update M_Exhibition_Main set ");
                    strSql.Append("ExhibitionTitle=@ExhibitionTitle,");
                    strSql.Append("ExhibitionLogoUrl=@ExhibitionLogoUrl,");
                    strSql.Append("ExhibitionStartTime=@ExhibitionStartTime,");
                    strSql.Append("ExhibitionEndTime=@ExhibitionEndTime,");
                    strSql.Append("ExhibitionAddress=@ExhibitionAddress,");
                    strSql.Append("ExhibitionTraffic=@ExhibitionTraffic,");
                    strSql.Append("ExhibitionLocation=@ExhibitionLocation,");
                    strSql.Append("ExhibitionAbstract=@ExhibitionAbstract,");
                    strSql.Append("ExhibitionAbout=@ExhibitionAbout,");
                    strSql.Append("ExhibitionOrder=@ExhibitionOrder,");
                    strSql.Append("ExhibitionBookListDesc=@ExhibitionBookListDesc,");
                    strSql.Append("StateTypeID=@StateTypeID,");
                    strSql.Append("IsPublish=@IsPublish,");
                    strSql.Append("BookListDownloadUrl=@BookListDownloadUrl,");
                    strSql.Append("IsDownloadBookList=@IsDownloadBookList");
                    strSql.Append(" where ExhibitionID=@ExhibitionID");
                    SqlParameter[] parameters = {
                    new SqlParameter("@ExhibitionTitle", SqlDbType.NVarChar,50),
                    new SqlParameter("@ExhibitionLogoUrl", SqlDbType.VarChar,100),
                    new SqlParameter("@ExhibitionStartTime", SqlDbType.DateTime),
                    new SqlParameter("@ExhibitionEndTime", SqlDbType.DateTime),
                    new SqlParameter("@ExhibitionAddress", SqlDbType.NVarChar,100),
                    new SqlParameter("@ExhibitionTraffic", SqlDbType.NVarChar,100),
                    new SqlParameter("@ExhibitionLocation", SqlDbType.NVarChar,100),
                    new SqlParameter("@ExhibitionAbstract", SqlDbType.NVarChar,1000),
                    new SqlParameter("@ExhibitionAbout", SqlDbType.NVarChar,500),
                    new SqlParameter("@ExhibitionOrder", SqlDbType.Int,4),
                    new SqlParameter("@ExhibitionBookListDesc", SqlDbType.NVarChar,500),
                    new SqlParameter("@StateTypeID", SqlDbType.Int,4),
                    new SqlParameter("@IsPublish", SqlDbType.Bit,1),
                    new SqlParameter("@BookListDownloadUrl", SqlDbType.NVarChar,255),
                    new SqlParameter("@IsDownloadBookList", SqlDbType.Bit,1),
                    new SqlParameter("@ExhibitionID", SqlDbType.Int,4)};
                    parameters[0].Value = item.ExhibitionTitle;
                    parameters[1].Value = item.ExhibitionLogoUrl;
                    parameters[2].Value = item.ExhibitionStartTime;
                    parameters[3].Value = item.ExhibitionEndTime;
                    parameters[4].Value = item.ExhibitionAddress;
                    parameters[5].Value = item.ExhibitionTraffic;
                    parameters[6].Value = item.ExhibitionLocation;
                    parameters[7].Value = item.ExhibitionAbstract;
                    parameters[8].Value = item.ExhibitionAbout;
                    parameters[9].Value = item.ExhibitionOrder;
                    parameters[10].Value = item.ExhibitionBookListDesc;
                    parameters[11].Value = item.StateTypeID;
                    parameters[12].Value = item.IsPublish;
                    parameters[13].Value = item.BookListDownloadUrl;
                    parameters[14].Value = item.IsDownloadBookList;
                    parameters[15].Value = item.ExhibitionID;

                    SQlHelper.ExecuteNonQuery(trans, CommandType.Text, strSql.ToString(), parameters);

                    //写入位置
                    if (item.AdvertisementList != null)
                    {
                        foreach (AdvertisementEntity _loc in item.AdvertisementList)
                        {
                            StringBuilder strSqladv = new StringBuilder();
                            strSqladv.Append("insert into M_Exhibition_Advertisement(");
                            strSqladv.Append("ExhibitionID,AdvertisementUrl,AdvertisementTitle,AdvertisementOrder,StateTypeID,CreateUserID,CreateTime)");
                            strSqladv.Append(" values (");
                            strSqladv.Append("@ExhibitionID,@AdvertisementUrl,@AdvertisementTitle,@AdvertisementOrder,@StateTypeID,@CreateUserID,@CreateTime)");
                            strSqladv.Append(";select @@IDENTITY");
                            SqlParameter[] parametersadv = {
                                    new SqlParameter("@ExhibitionID", SqlDbType.Int,4),
                                    new SqlParameter("@AdvertisementUrl", SqlDbType.NVarChar,500),
                                    new SqlParameter("@AdvertisementTitle", SqlDbType.NVarChar,200),
                                    new SqlParameter("@AdvertisementOrder", SqlDbType.Int,4),
                                    new SqlParameter("@StateTypeID", SqlDbType.Int,4),
                                    new SqlParameter("@CreateUserID", SqlDbType.VarChar,20),
                                    new SqlParameter("@CreateTime", SqlDbType.DateTime)};
                            parametersadv[0].Value =item.ExhibitionID;
                            parametersadv[1].Value = _loc.AdvertisementUrl;
                            parametersadv[2].Value = _loc.AdvertisementTitle;
                            parametersadv[3].Value = _loc.AdvertisementOrder;
                            parametersadv[4].Value = _loc.StateTypeID;
                            parametersadv[5].Value = _loc.CreateUserID;
                            parametersadv[6].Value = _loc.CreateTime;

                            SQlHelper.ExecuteNonQuery(trans, CommandType.Text, strSqladv.ToString(), parametersadv);
                        }

                        trans.Commit();
                        returnValue = true;
                    }
                }
                catch (Exception ex)
                {
                    if (trans != null)
                    {
                        trans.Rollback();
                    }
                    LogUtil.WriteLog(ex);
                }
            }

            return returnValue;
        }

        public int Add(ExhibitionEntity item)
        {
            int returnValue = 0;

            SqlTransaction trans = null;
            using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
            {
                try
                {
                    con.Open();
                    trans = con.BeginTransaction();

                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("insert into M_Exhibition_Main(");
                    strSql.Append("ExhibitionTitle,ExhibitionLogoUrl,ExhibitionStartTime,ExhibitionEndTime,ExhibitionAddress,ExhibitionTraffic,ExhibitionLocation,ExhibitionAbstract,ExhibitionAbout,ExhibitionOrder,ExhibitionBookListDesc,StateTypeID,IsPublish,BookListDownloadUrl,IsDownloadBookList,CreateUserID,CreateTime)");
                    strSql.Append(" values (");
                    strSql.Append("@ExhibitionTitle,@ExhibitionLogoUrl,@ExhibitionStartTime,@ExhibitionEndTime,@ExhibitionAddress,@ExhibitionTraffic,@ExhibitionLocation,@ExhibitionAbstract,@ExhibitionAbout,@ExhibitionOrder,@ExhibitionBookListDesc,@StateTypeID,@IsPublish,@BookListDownloadUrl,@IsDownloadBookList,@CreateUserID,@CreateTime)");
                    strSql.Append(";select @@IDENTITY");
                    SqlParameter[] parameters = {
                    new SqlParameter("@ExhibitionTitle", SqlDbType.NVarChar,50),
                    new SqlParameter("@ExhibitionLogoUrl", SqlDbType.VarChar,100),
                    new SqlParameter("@ExhibitionStartTime", SqlDbType.DateTime),
                    new SqlParameter("@ExhibitionEndTime", SqlDbType.DateTime),
                    new SqlParameter("@ExhibitionAddress", SqlDbType.NVarChar,100),
                    new SqlParameter("@ExhibitionTraffic", SqlDbType.NVarChar,100),
                    new SqlParameter("@ExhibitionLocation", SqlDbType.NVarChar,100),
                    new SqlParameter("@ExhibitionAbstract", SqlDbType.NVarChar,1000),
                    new SqlParameter("@ExhibitionAbout", SqlDbType.NVarChar,500),
                    new SqlParameter("@ExhibitionOrder", SqlDbType.Int,4),
                    new SqlParameter("@ExhibitionBookListDesc", SqlDbType.NVarChar,500),
                    new SqlParameter("@StateTypeID", SqlDbType.Int,4),
                    new SqlParameter("@IsPublish", SqlDbType.Bit,1),
                    new SqlParameter("@BookListDownloadUrl", SqlDbType.NVarChar,255),
                    new SqlParameter("@IsDownloadBookList", SqlDbType.Bit,1),
                    new SqlParameter("@CreateUserID", SqlDbType.VarChar,20),
                    new SqlParameter("@CreateTime", SqlDbType.DateTime)};
                    parameters[0].Value = item.ExhibitionTitle;
                    parameters[1].Value = item.ExhibitionLogoUrl;
                    parameters[2].Value = item.ExhibitionStartTime;
                    parameters[3].Value = item.ExhibitionEndTime;
                    parameters[4].Value = item.ExhibitionAddress;
                    parameters[5].Value = item.ExhibitionTraffic;
                    parameters[6].Value = item.ExhibitionLocation;
                    parameters[7].Value = item.ExhibitionAbstract;
                    parameters[8].Value = item.ExhibitionAbout;
                    parameters[9].Value = item.ExhibitionOrder;
                    parameters[10].Value = item.ExhibitionBookListDesc;
                    parameters[11].Value = item.StateTypeID;
                    parameters[12].Value = item.IsPublish;
                    parameters[13].Value = item.BookListDownloadUrl;
                    parameters[14].Value = item.IsDownloadBookList;
                    parameters[15].Value = item.CreateUserID;
                    parameters[16].Value = item.CreateTime;

                    object objrec = SQlHelper.ExecuteScalar(trans, CommandType.Text, strSql.ToString(), parameters);
                    if (objrec != null)
                    {
                        //写入位置
                        if (item.AdvertisementList != null)
                        {
                            foreach (AdvertisementEntity _loc in item.AdvertisementList)
                            {
                                StringBuilder strSqladv = new StringBuilder();
                                strSqladv.Append("insert into M_Exhibition_Advertisement(");
                                strSqladv.Append("ExhibitionID,AdvertisementUrl,AdvertisementTitle,AdvertisementOrder,StateTypeID,CreateUserID,CreateTime)");
                                strSqladv.Append(" values (");
                                strSqladv.Append("@ExhibitionID,@AdvertisementUrl,@AdvertisementTitle,@AdvertisementOrder,@StateTypeID,@CreateUserID,@CreateTime)");
                                strSqladv.Append(";select @@IDENTITY");
                                SqlParameter[] parametersadv = {
                                    new SqlParameter("@ExhibitionID", SqlDbType.Int,4),
                                    new SqlParameter("@AdvertisementUrl", SqlDbType.NVarChar,500),
                                    new SqlParameter("@AdvertisementTitle", SqlDbType.NVarChar,200),
                                    new SqlParameter("@AdvertisementOrder", SqlDbType.Int,4),
                                    new SqlParameter("@StateTypeID", SqlDbType.Int,4),
                                    new SqlParameter("@CreateUserID", SqlDbType.VarChar,20),
                                    new SqlParameter("@CreateTime", SqlDbType.DateTime)};
                                parametersadv[0].Value = int.Parse(objrec.ToString());
                                parametersadv[1].Value = _loc.AdvertisementUrl;
                                parametersadv[2].Value = _loc.AdvertisementTitle;
                                parametersadv[3].Value = _loc.AdvertisementOrder;
                                parametersadv[4].Value = _loc.StateTypeID;
                                parametersadv[5].Value = _loc.CreateUserID;
                                parametersadv[6].Value = _loc.CreateTime;


                                SQlHelper.ExecuteNonQuery(trans, CommandType.Text, strSqladv.ToString(), parametersadv);
                            }
                        }

                        trans.Commit();
                        returnValue = int.Parse(objrec.ToString());
                    }
                }
                catch (Exception ex)
                {
                    if (trans != null)
                    {
                        trans.Rollback();
                    }
                    LogUtil.WriteLog(ex);
                }
            }

            return returnValue;

            
        }
    }
}
