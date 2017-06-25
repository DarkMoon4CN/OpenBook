using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mars.Server.Entity;
using Dapper;
using Mars.Server.Utils;
using System.Data.SqlClient;

namespace Mars.Server.DAO
{
   public class PictureServerDAO
    {
       public PictureServerEntity QueryPicServer(int picServerID)
       {
           string sql = "SELECT * FROM M_PictureServer WHERE PictureServerID=@PictureServerID";

           try
           {
               using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
               {
                   return conn.Query<PictureServerEntity>(sql, new { PictureServerID = picServerID }).FirstOrDefault();
               }
           }
           catch (Exception ex)
           {
               LogUtil.WriteLog(ex);
               return null;
           }
       }    
      
    }
}
