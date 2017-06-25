using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Mars.Server.Entity;
namespace Mars.Server.DAO
{
    public class PictureDAO
    {
        public int AddPicInfoToDB(int imgserverid, string path)
        {
            try
            {
                string sql = "insert into M_Pictures(PictureServerID,PicturePath,PictureState) output inserted.PictureID values(@PictureServerID,@PicturePath,1)";
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return con.Query<int>(sql, new { PictureServerID = imgserverid, PicturePath = path }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return 0;
            }
        }

        public DataTable QueryStartPic()
        {
            try
            {
                string sql = @" SELECT TOP 1 p.PictureID,p.PicturePath,p.Domain,s.IsDefault,StartTime,s.EndTime,s.Url FROM [dbo].[M_StartPictures] s INNER JOIN dbo.M_V_Picture p";
                sql += " ON s.PictureID=p.PictureID WHERE s.IsDefault=1 AND StartTime<=GETDATE() AND EndTime>=GETDATE() ";

                return SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.Text, sql).Tables[0];
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return new DataTable();
            }
        }


        public List<PictureEntity> QueryImages(List<int> picids)
        {
            try
            {
                StringBuilder sbsql = new StringBuilder();
                if (picids.Count > 1)
                {
                    sbsql.Append(" with a as (");
                    for (int i = 0; i < picids.Count; i++)
                    {
                        if (i == 0)
                            sbsql.AppendFormat(" select {0} as PID ", picids[i]);
                        else
                            sbsql.AppendFormat(" select {0} ", picids[i]);

                        if (i < picids.Count - 1)
                        {
                            sbsql.Append(" union all ");
                        }
                    }
                    sbsql.Append(" ) select p.* from M_V_Picture p inner join a on p.PictureID=a.PID ");
                }
                else
                {
                    sbsql.AppendFormat(" select * from M_V_Picture where PictureID={0}", picids[0]);
                }
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return con.Query<PictureEntity>(sbsql.ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
           
        }

        public PictureEntity QueryPictureEntity(int pictureID)
        {
            try
            {
                string sql = "Select * From M_Pictures Where PictureID=@PictureID";

                using(SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return conn.Query<PictureEntity>(sql, new { PictureID = pictureID }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        public int Insert(PictureEntity entity)
        {
            int returnValue = 0;
            string sql = "Insert Into M_Pictures(PictureServerID,PicturePath,PictureState) Values(@PictureServerID,@PicturePath,@PictureState);SELECT @@IDENTITY;";
            SqlParameter[] prms = {
                                      new SqlParameter("@PictureServerID", SqlDbType.Int),
                                      new SqlParameter("@PicturePath", SqlDbType.NVarChar,200),
                                      new SqlParameter("@PictureState", SqlDbType.Int)
                                  };
            prms[0].Value = entity.PictureServerID;
            prms[1].Value = entity.PicturePath;
            prms[2].Value = entity.PictureState;

            try
            {
                object obj = SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, sql, prms);
                if (obj != null)
                {
                    returnValue = obj.ToInt();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
            }

            return returnValue;
        }
    }
}
