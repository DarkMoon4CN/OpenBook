using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Mars.Server.DAO.Exhibition
{
    public class SearchDAO
    {
        /// <summary>
        /// 根据展场id 查找属于这次展场的展商和活动的标题信息
        /// </summary>
        /// <param name="ExhibitionID">展场id</param>
        /// <returns></returns>
        public DataTable GetSearchKeyWordTable(int exhibitionID)
        {
            DataTable table = null;

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT ee.ExhibitorName AS SearchName,
                        ee.ExhibitorPinYin AS SearchPinYin,
                        1 AS SearchType
                        FROM 
                        dbo.M_Exhibition_Exhibitors AS ee
                        INNER JOIN dbo.M_Exhibition_Main AS em ON em.ExhibitionID = ee.ExhibitionID
                        WHERE ee.StateTypeID=1 AND em.StateTypeID = 1 AND em.ExhibitionID = {0}
                        UNION 
                        SELECT ea.ActivityTitle AS SearchName,
                        '' AS SearchPinYin,
                        2 AS SearchType  
                        FROM dbo.M_Exhibition_Activity AS ea
                        LEFT JOIN dbo.M_Exhibition_Exhibitors AS ee ON ee.ExhibitorID = ea.ExhibitorID
                        INNER JOIN dbo.M_Exhibition_Main AS em ON em.ExhibitionID = ea.ExhibitionID
                        WHERE ea.StateTypeID = 1 AND ((ee.ExhibitorID IS NOT NULL AND ee.StateTypeID=1) OR ee.ExhibitorID IS NULL) AND em.StateTypeID =1 AND em.ExhibitionID = {0} AND ea.ParentID=0"
                        , exhibitionID.ToString());

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
    }
}
