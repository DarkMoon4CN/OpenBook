using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Dapper; 
using System.Linq; 
using System.Threading.Tasks; 

namespace Mars.Server.DAO
{
    public class CategoriesDAO
    {
        /// <summary>
        /// 得到某一项的详细信息
        /// </summary>
        /// <param name="CategoriesID"></param>
        /// <returns></returns>
        public CategoriesEntity QueryCategories(int CategoriesID)
        {
            string sql = "SELECT * FROM M_CalendarType WHERE CalendarTypeID=@CalendarTypeID";

            try
            {
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return conn.Query<CategoriesEntity>(sql, new { CalendarTypeID = CategoriesID }).FirstOrDefault(); 
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }
        /// <summary>
        /// 新增分类
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Insert(CategoriesEntity entity)
        {
            bool isSuccess = false;
            SqlTransaction trans = null;

            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(" INSERT INTO M_CalendarType(ParentCalendarTypeID,CalendarTypeName,CalendarTypeKind,Descripition,Dismiss,PictureID) values(@ParentCalendarTypeID,@CalendarTypeName,@CalendarTypeKind,@Descripition,@Dismiss,@PictureID)");

            SqlParameter[] prms = { 
                                      new SqlParameter("@ParentCalendarTypeID", SqlDbType.Int),
                                      new SqlParameter("@CalendarTypeName", SqlDbType.NVarChar,100),
                                      new SqlParameter("@CalendarTypeKind",SqlDbType.SmallInt),
                                      new SqlParameter("@Descripition",SqlDbType.NVarChar,200),
                                      new SqlParameter("@Dismiss",SqlDbType.Bit), 
                                      new SqlParameter("@PictureID",SqlDbType.Int)
                                   };

            prms[0].Value = entity.ParentCalendarTypeID;
            prms[1].Value = entity.CalendarTypeName;
            prms[2].Value = entity.CalendarTypeKind;
            prms[3].Value = entity.Descripition;
            prms[4].Value = entity.Dismiss;
            prms[5].Value = entity.PictureID;

            try
            {
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    conn.Open();
                    trans = conn.BeginTransaction();

                    SQlHelper.ExecuteScalar(trans, CommandType.Text, sbSql.ToString(), prms);
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
        /// <summary>
        /// 修改分类
        /// </summary>
        /// <param name="Cateedit"></param>
        /// <returns></returns>
        public bool Categoriesedit(CategoriesEntity Cateedit)
        {
            bool flg = false;
            SqlTransaction trans = null;
            string sql = "update M_CalendarType set ParentCalendarTypeID=@ParentCalendarTypeID,CalendarTypeName=@CalendarTypeName,CalendarTypeKind=@CalendarTypeKind,Descripition=@Descripition,Dismiss=@Dismiss,PictureID=@PictureID WHERE CalendarTypeID=@CalendarTypeID";
            SqlParameter[] prms = { 
                                      
                                      new SqlParameter("@ParentCalendarTypeID", SqlDbType.Int),
                                      new SqlParameter("@CalendarTypeName", SqlDbType.NVarChar,100),
                                      new SqlParameter("@CalendarTypeKind",SqlDbType.SmallInt),
                                      new SqlParameter("@Descripition",SqlDbType.NVarChar,200),
                                      new SqlParameter("@Dismiss",SqlDbType.Bit), 
                                      new SqlParameter("@PictureID",SqlDbType.Int),
                                      new SqlParameter("@CalendarTypeID", SqlDbType.Int)
                                   };

            prms[0].Value = Cateedit.ParentCalendarTypeID;
            prms[1].Value = Cateedit.CalendarTypeName;
            prms[2].Value = Cateedit.CalendarTypeKind;
            prms[3].Value = Cateedit.Descripition;
            prms[4].Value = Cateedit.Dismiss;
            prms[5].Value = Cateedit.PictureID;
            prms[6].Value = Cateedit.CalendarTypeID;
            try
            {
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    conn.Open();
                    trans = conn.BeginTransaction();

                    SQlHelper.ExecuteScalar(trans, CommandType.Text, sql.ToString(), prms);
                    flg = true;
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);

            }
            return flg;
        }
        /// <summary>
        /// 查询出一级分类
        /// </summary>
        /// <returns></returns>
        public List<CategoriesEntity> CategoriesFirstLevel()
        {
            string sql = "Select CalendarTypeID, CalendarTypeName From M_CalendarType Where ParentCalendarTypeID = -1";

            try
            {
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return conn.Query<CategoriesEntity>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }
        /// <summary>
        /// 判断分类是否可以添加
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="fname"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsUserable(int pid, string fname, int id)
        {
            bool flg = false;
            if (pid == 0)
            { pid = -1; }
            string str = "select * from M_CalendarType where ParentCalendarTypeID=@ParentCalendarTypeID and CalendarTypeName=@CalendarTypeName and CalendarTypeID <> @CalendarTypeID";
            SqlParameter[] pars= {  new SqlParameter("@ParentCalendarTypeID",pid),
                                 new SqlParameter("@CalendarTypeName",fname),
                                 new SqlParameter("@CalendarTypeID",id)};
            try
            {
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    DataTable db=SQlHelper.ExecuteDataset(conn,CommandType.Text,str,pars).Tables[0];
                    if (db.Rows.Count>0) 
                        flg = false;
                    else flg = true;
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
            }
            return flg;
        }
        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string delectCategories(string id)
        {
            string flg = "0";
            string sql = "Delete M_CalendarType where CalendarTypeID=@CalendarTypeID"; 
            try
            {
                SqlParameter[] pars = { new SqlParameter("CalendarTypeID", int.Parse(id)) };
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    SQlHelper.ExecuteScalar(conn, CommandType.Text, sql,pars);
                    flg = "1";
                }
            }
            catch (Exception ex)
            {
                flg = "0";
                LogUtil.WriteLog(ex);
            }
            return flg;
        }
        /// <summary>
        /// 分类管理列表
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="totalcnt"></param>
        /// <returns></returns>
        public DataTable QueryCategoriesTable(CategoriesSearchEntity entity, out int totalcnt)
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
        public List<SqlParameter> ParseToSqlParameters(CategoriesSearchEntity entity)
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
        private SqlParameter CPWhere(CategoriesSearchEntity entity)
        {
            StringBuilder sbwhere = new StringBuilder(" ParentCalendarTypeID is not null ");
            if (!string.IsNullOrEmpty(entity.CalendarTypeName))
            {
                sbwhere.Append(" AND [CalendarTypeName] like '%" + entity.CalendarTypeName + "%'");
            }
            if (entity.ParentCalendarTypeID != 0)
            {
                sbwhere.Append(" AND [ParentCalendarTypeID] =" + entity.ParentCalendarTypeID);
            }
            return new SqlParameter("@sqlWhere", sbwhere.ToString());
        }

       /// <summary>
       /// 设置排序
       /// </summary>
       /// <param name="enity"></param>
       /// <returns></returns>
        private SqlParameter CPOrder(CategoriesSearchEntity enity)
        {
            StringBuilder sborder = new StringBuilder();

            if (enity.OrderfieldType == OrderFieldType.Desc)
            {
                sborder.Append(" CalendarTypeID DESC ");
            }
            else
            {
                sborder.Append(" CalendarTypeID ASC ");
            }

            return new SqlParameter("@OrderField", sborder.ToString());
        }
        /// <summary>
        /// 设置显示字段
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private SqlParameter CPFields(CategoriesSearchEntity entity)
        {
            StringBuilder sbfileds = new StringBuilder();
            if (entity.UseDBPagination)
            {
                sbfileds.Append(@" CalendarTypeID, CalendarTypeName,CalendarTypeKind,ParentCalendarTypeID,Dismiss,Descripition ");
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
        private SqlParameter CPTable(CategoriesSearchEntity entity)
        {
            StringBuilder sbtable = new StringBuilder();
            //基本表
            sbtable.Append(" M_CalendarType ");
            return new SqlParameter("@TableName", sbtable.ToString());
        }
        #endregion
    }
}
