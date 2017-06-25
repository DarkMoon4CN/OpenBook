using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mars.Server.Utils;
using System.Data;
using System.Data.SqlClient;

namespace Mars.Server.DAO
{
    public class EventItemBrowseCntDao
    {
        /// <summary>
        /// 更新浏览次数
        /// </summary>
        /// <param name="eventItemGuid"></param>
        /// <returns></returns>
        public bool UpdateBrowserCnt(Guid eventItemGuid)
        {
            bool isSuccess = false;
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("IF EXISTS (SELECT * FROM  M_EventItemBrowseCnts WHERE EventItemGUID=@EventItemGUID)");
            sbSql.Append(" UPDATE M_EventItemBrowseCnts SET BrowseCnt=BrowseCnt+1 WHERE EventItemGUID=@EventItemGUID");
            sbSql.Append(" ELSE");
            sbSql.Append(" INSERT INTO M_EventItemBrowseCnts(EventItemGUID,BrowseCnt) VALUES (@EventItemGUID,@BrowseCnt)");

            SqlParameter[] prms = {
                                      new SqlParameter("@EventItemGUID",SqlDbType.UniqueIdentifier, 16),
                                      new SqlParameter("@BrowseCnt",SqlDbType.Int)
                                  };
            prms[0].Value = eventItemGuid;
            prms[1].Value = 1;
            
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
        /// 获取文章浏览次数
        /// </summary>
        /// <param name="eventItemGuid"></param>
        /// <returns></returns>
        public int GetBrowserCnt(Guid eventItemGuid)
        {
            int broCnts = 0;
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("IF EXISTS (SELECT * FROM  M_EventItemBrowseCnts WHERE EventItemGUID=@EventItemGUID)");
            sbSql.Append(" SELECT 0");
            sbSql.Append(" ELSE");
            sbSql.Append(" SELECT BrowseCnt FROM  M_EventItemBrowseCnts WHERE EventItemGUID=@EventItemGUID");

            SqlParameter[] prms = {
                                      new SqlParameter("@EventItemGUID",SqlDbType.UniqueIdentifier, 16)                                    
                                  };
            prms[0].Value = eventItemGuid;         

            try
            {
                object result = SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, sbSql.ToString(), prms);
                int.TryParse(result.ToString(), out broCnts);
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return 0;
            }
            return broCnts;
        }
    }
}
