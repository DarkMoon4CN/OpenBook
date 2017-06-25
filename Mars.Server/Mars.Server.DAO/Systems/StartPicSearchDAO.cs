using Mars.Server.Entity.Systems;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Mars.Server.DAO.Systems
{
    public class StartPicSearchDAO
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="totalcnt"></param>
        /// <returns></returns>
        public DataTable QueryData(StartPicSearchEntity info, out int totalcnt)
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

        public bool Add(StartPicEntity item)
        {
            StringBuilder strSql = new StringBuilder();
            string sd = item.StartTime == null ? "" : "StartTime,";
            string sdd = item.StartTime == null ? "" : "'"+Convert.ToDateTime(item.StartTime).ToString("yyyy-MM-dd")+"',";

            string ed = item.EndTime == null ? "" : "EndTime,";
            string edd = item.EndTime == null ? "" : "'" + Convert.ToDateTime(item.EndTime).ToString("yyyy-MM-dd") + "',";
            strSql.AppendFormat(@" insert into M_StartPictures(
                  PictureID, {4} {5} IsDefault, IsOutdate,Url)
                  SELECT PictureID,{0} {1} {2}, {3},'{7}'
                FROM M_Pictures WHERE PicturePath LIKE '%{6}%'"
, sdd,edd,Convert.ToInt16(item.IsDefault), Convert.ToInt16(item.IsOutdate),sd,ed, item.PicURL.Substring(item.PicURL.LastIndexOf('/')),item.URL);


            return SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, strSql.ToString(), null) > 0;
        }

        public bool DeleteStartPic(int id)
        {
            bool returnValue = false;

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@" DELETE FROM [M_StartPictures] WHERE PictureID = {0} "
                , id.ToString());

            try
            {
                if (SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, strSql.ToString(), null) > 0)
                {
                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
            }
            return returnValue;
        }

        public bool ChangeDefault(int id, int isdefault)
        {
            bool returnValue = false;

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@" UPDATE M_StartPictures SET IsDefault = {1} WHERE PictureID = {0} "
                , id.ToString()
                , isdefault.ToString());

            try
            {
                if (SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, strSql.ToString(), null) > 0)
                {
                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
            }
            return returnValue;
        }

        #region 构造SQL分页参数
        /// <summary>
        /// 核心方法。查询条件转换成sql参数
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        protected virtual List<SqlParameter> ParseToSqlParameters(StartPicSearchEntity sp)
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
        /// 
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        protected virtual SqlParameter CreateParameter_Table(StartPicSearchEntity sp)
        {
            StringBuilder sbtable = new StringBuilder();
            sbtable.Append(@" [M_StartPictures] AS sp
                    INNER JOIN M_V_Picture AS p ON p.PictureID = sp.PictureID ");

            return new SqlParameter("@TableName", sbtable.ToString());
        }

        protected virtual SqlParameter CreateParamter_Orderby(StartPicSearchEntity sp)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(" sp.PictureID DESC ");

            return new SqlParameter("@OrderField", sb.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        protected virtual SqlParameter CreateParameter_Fileds(StartPicSearchEntity sp)
        {
            StringBuilder sbfileds = new StringBuilder();
            sbfileds.Append(@" sp.*,p.PicturePath,p.Domain,CONVERT(NVARCHAR(30),sp.StartTime,20) AS FormatStartTime,CONVERT(NVARCHAR(30),sp.EndTime,20) AS FormatEndTime ");
            return new SqlParameter("@Fields", sbfileds.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        protected virtual SqlParameter CreateParameter_Where(StartPicSearchEntity sp)
        {
            StringBuilder sbwhere = new StringBuilder(" 1=1 ");

            return new SqlParameter("@sqlWhere", sbwhere.ToString());
        }

        #endregion
    }
}
