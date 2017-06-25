using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using Mars.Server.Utils;
using Mars.Server.Entity;
using System.Data.SqlClient;
using System.Data;
namespace Mars.Server.DAO
{

    /*
     * 模块：移动端手机页面弹框
     * 作用：移动端手机页面弹框数据
     * 作者：
     * 时间：2015-12-03
     * 备注：DialogMessagesDAO将引用OperationResult.cs 作为返回数据结果承载体
     */
    public class DialogMessagesDAO
    {
        public OperationResult<IList<DialogMessagesEntity>> DialogMessages_GetWhere(string strWhere = null)
        {
            IList<DialogMessagesEntity> items = new List<DialogMessagesEntity>();
            try
            {
                string sql = " SELECT * FROM M_DialogMessages  WHERE  1=1 " + strWhere;
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    items = con.Query<DialogMessagesEntity>(sql).ToList();
                    return new OperationResult<IList<DialogMessagesEntity>>(OperationResultType.Success, "数据完成查询", items);
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return new OperationResult<IList<DialogMessagesEntity>>(OperationResultType.Success, "异常结果：" + ex.Message);
            }
        }

        public OperationResult<bool> DialogMessages_Insert(DialogMessagesEntity entity) 
        {
           SqlTransaction trans = null;
           try
           {
               string sql = string.Empty;
               sql += " INSERT INTO M_DialogMessages(ImageLink,MoblieType,ButtonText,ArticleLink,Contents,StartType,StartTime,EndTime,StartCount) ";
               sql += " VALUES(@ImageLink,@MoblieType,@ButtonText,@ArticleLink,@Contents,@StartType,@StartTime,@EndTime,@StartCount)";

               SqlParameter[] prms = { 
                                      new SqlParameter("@ImageLink", SqlDbType.NVarChar,128),
                                      new SqlParameter("@MoblieType", SqlDbType.Int),
                                      new SqlParameter("@ButtonText",SqlDbType.NVarChar,64),
                                      new SqlParameter("@ArticleLink",SqlDbType.NVarChar,128),
                                      new SqlParameter("@Contents",SqlDbType.NVarChar,1024), 
                                      new SqlParameter("@StartType",SqlDbType.Int),
                                      new SqlParameter("@StartTime", SqlDbType.DateTime),
                                      new SqlParameter("@EndTime", SqlDbType.DateTime),
                                      new SqlParameter("@StartCount",SqlDbType.Int),
                                   };
               prms[0].Value = entity.ImageLink;
               prms[1].Value = entity.MoblieType;
               prms[2].Value = entity.ButtonText;
               prms[3].Value = entity.ArticleLink;
               prms[4].Value = entity.Contents;
               prms[5].Value = entity.StartType;
               prms[6].Value = entity.StartTime;
               prms[7].Value = entity.EndTime;
               prms[8].Value = entity.StartCount;
               using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
               {
                   conn.Open();
                   trans = conn.BeginTransaction();
                   SQlHelper.ExecuteScalar(trans, CommandType.Text, sql, prms);
                   trans.Commit();
               }
               return new OperationResult<bool>(OperationResultType.Success, "数据加入成功！", true);
           }
           catch(Exception ex)
           {
               try
               {
                   if (trans != null)
                   {
                       trans.Rollback();
                   }
               }
               catch { }
              return new OperationResult<bool>(OperationResultType.Error,"异常结果:"+ex.Message,false);
           }
        }

        public OperationResult<DataTable> DialogMessages_GetList(int pageIndex, int pageSize, string orderType, string strWhere, out int recordCount)
        {
            DataTable table = new DataTable();
            try
            {
                List<SqlParameter> prmslist = new List<SqlParameter>();
                prmslist.Add(new SqlParameter("@TableName", " M_DialogMessages "));

                prmslist.Add(new SqlParameter("@Fields", "*"));

                prmslist.Add(new SqlParameter("@OrderField", orderType));

                prmslist.Add(new SqlParameter("@sqlWhere", strWhere));

                prmslist.Add(new SqlParameter("@pageIndex", pageIndex));

                prmslist.Add(new SqlParameter("@pageSize", pageSize));

                prmslist.Add(new SqlParameter() { ParameterName = "@Records", Value = 0, Direction = ParameterDirection.Output });

                SqlParameter[] prms = prmslist.ToArray();

                table = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.StoredProcedure, "sp_pager06", prms).Tables[0];
                recordCount = Convert.ToInt32(prms[prms.Length - 1].Value);
                return new OperationResult<DataTable>(OperationResultType.Success, "分页数据抓取成功", table);
            }
            catch (Exception ex)
            {
                recordCount = 0;
                return new OperationResult<DataTable>(OperationResultType.Error, "异常结果:" + ex.Message, table);
            }
           
        }

        public OperationResult<bool> DialogMessages_Update(DialogMessagesEntity entity)
        {
            SqlTransaction trans = null;
            try
            {
                string sql = string.Empty;
                sql += " UPDATE M_DialogMessages SET ImageLink=@ImageLink,MoblieType=@MoblieType,ButtonText=@ButtonText ";
                sql += ",ArticleLink=@ArticleLink,Contents=@Contents,StartType=@StartType ";
                sql += ",StartTime=@StartTime,EndTime=@EndTime,StartCount=@StartCount ";
                sql += " WHERE MessageID=@MessageID ";
                SqlParameter[] prms = { 
                                      new SqlParameter("@ImageLink", SqlDbType.NVarChar,128),
                                      new SqlParameter("@MoblieType", SqlDbType.Int),
                                      new SqlParameter("@ButtonText",SqlDbType.NVarChar,64),
                                      new SqlParameter("@ArticleLink",SqlDbType.NVarChar,128),
                                      new SqlParameter("@Contents",SqlDbType.NVarChar,1024), 
                                      new SqlParameter("@StartType",SqlDbType.Int),
                                      new SqlParameter("@StartTime", SqlDbType.DateTime),
                                      new SqlParameter("@EndTime", SqlDbType.DateTime),
                                      new SqlParameter("@StartCount",SqlDbType.Int),
                                      new SqlParameter("@MessageID",SqlDbType.Int)
                                   };
                prms[0].Value = entity.ImageLink;
                prms[1].Value = entity.MoblieType;
                prms[2].Value = entity.ButtonText;
                prms[3].Value = entity.ArticleLink;
                prms[4].Value = entity.Contents;
                prms[5].Value = entity.StartType;
                prms[6].Value = entity.StartTime;
                prms[7].Value = entity.EndTime;
                prms[8].Value = entity.StartCount;
                prms[9].Value = entity.MessageID;
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    conn.Open();
                    trans = conn.BeginTransaction();
                    SQlHelper.ExecuteScalar(trans, CommandType.Text, sql, prms);
                    trans.Commit();
                }
                return new OperationResult<bool>(OperationResultType.Success, "数据更新成功！", true);
            }
            catch (Exception ex)
            {
                try
                {
                    if (trans != null)
                    {
                        trans.Rollback();
                    }
                }
                catch { }
                return new OperationResult<bool>(OperationResultType.Error, "异常结果:" + ex.Message, false);
            }
        }

        public OperationResult<bool> DialogMessages_Delete(int messageID) 
        {
            try
            {
                string sql = " DELETE FROM M_DialogMessages WHERE  MessageID={0} ";
                sql = string.Format(sql, messageID);
                bool result= int.Parse(SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, sql).ToString()) > 0;
                return new OperationResult<bool>(OperationResultType.Success, "删除完成！", result);
            }
            catch (Exception ex)
            {
                return new OperationResult<bool>(OperationResultType.Success, "异常结果:" + ex.Message, false);
            }
        }

    }
}
