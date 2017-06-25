using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mars.Server.Entity;
using Mars.Server.Utils;
using Dapper;
using System.Data.SqlClient;

namespace Mars.Server.DAO
{
   public class ZoneDAO
    {
       public List<ZoneEntity> QueryZoneProvince()
       {
           string sql = "SELECT * FROM M_V_Zone ";

           try
           {
               using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
               {
                   return conn.Query<ZoneEntity>(sql).ToList();
               }
           }
           catch (Exception ex)
           {
               LogUtil.WriteLog(ex);
               return null;
           }
       }

       /// <summary>
       /// 查询按地区分组省
       /// </summary>
       /// <returns></returns>
       public List<ZoneEntity> QueryZoneToGroup()
       {
           string zoneSql = "Select ZoneID,ZoneName,ZoneLevel,ZoneParentID From M_Zone WHERE ZoneLevel=1";
           string provinceSql = "SELECT * FROM M_V_Zone ";
           try
           {
               using(SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
               {
                   List<ZoneEntity> zoneList = conn.Query<ZoneEntity>(zoneSql).ToList();
                   List<ZoneEntity> provinceList = conn.Query<ZoneEntity>(provinceSql).ToList();

                   foreach (ZoneEntity item in zoneList)
                   {
                       item.ZoneList = provinceList.Where(entity => entity.ZoneParentID == item.ZoneID).ToList();
                   }
                   return zoneList;
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
