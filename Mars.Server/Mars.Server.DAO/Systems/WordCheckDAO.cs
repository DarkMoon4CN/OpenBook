using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Mars.Server.DAO.Systems
{
    public class WordCheckDAO
    {
        /// <summary>
        /// 查询所有需要匹配的敏感词
        /// </summary>
        /// <returns></returns>
        public DataTable GetWordsTable()
        {
            DataTable table = null;

            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * FROM dbo.M_System_SensitiveWords WHERE StateTypeID = 1 ");

            DataSet ds = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.Text, strSql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    table = ds.Tables[0];
                }
            }

            return table;
        }

        public DataTable GetNewWordStringTable()
        {
            DataTable table = null;

            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * FROM dbo.M_System_SensitiveWords WHERE StateTypeID = 1 AND IsNeedRecheck=1 ");

            DataSet ds = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.Text, strSql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    table = ds.Tables[0];
                }
            }

            return table;
        }
    }
}
