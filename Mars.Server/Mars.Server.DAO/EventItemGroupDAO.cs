using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using Mars.Server.Entity;
using System.Data.SqlClient;
using Mars.Server.Utils;
using System.Data;

namespace Mars.Server.DAO
{
    public class EventItemGroupDAO
    {
        public DataTable QueryGroupTable(EventItemGroupSearchEntity searchEntity, out int totalcnt)
        {
            DataTable table = null;
            totalcnt = 0;

            try
            {
                SqlParameter[] prms = ParseToSqlParameters(searchEntity).ToArray();
                table = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.StoredProcedure, "sp_pager06", prms).Tables[0];
                totalcnt = Convert.ToInt32(prms[prms.Length - 1].Value);
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }

            return table;
        }

        public DataTable QueryGroupRelViewList(EventItemGroupSearchEntity searchEntity, out int totalcnt)
        {
            DataTable table = null;
            totalcnt = 0;

            try
            {
                List<SqlParameter> prmslist = new List<SqlParameter>();
                prmslist.Add(new SqlParameter("@TableName", " M_V_EventItemGroupRel "));

                string fields = @" Title, EventGroupID,EventItemID,DisplayOrder,FirstTypName,SecondTypeName,GroupState,CarouselPictureID, IsSingleGroupState,PictureID";
                prmslist.Add(new SqlParameter("@Fields", fields));

                prmslist.Add(new SqlParameter("@OrderField", " DisplayOrder ASC"));

                string wheresql = " EventGroupID=" + searchEntity.GroupEventID;
                prmslist.Add(new SqlParameter("@sqlWhere", wheresql));

                prmslist.Add(new SqlParameter("@pageIndex", searchEntity.PageIndex));

                prmslist.Add(new SqlParameter("@pageSize", searchEntity.PageSize));

                prmslist.Add(new SqlParameter() { ParameterName = "@Records", Value = 0, Direction = ParameterDirection.Output });

                SqlParameter[] prms = prmslist.ToArray();

                table = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.StoredProcedure, "sp_pager06", prms).Tables[0];
                totalcnt = Convert.ToInt32(prms[prms.Length - 1].Value);

            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }

            return table;
        }

        public List<EventItemGroupRelViewEntity> QueryGroupRelViewList(int eventGroupID)
        {
            try
            {
                string selectSQL = "SELECT Title, EventGroupID,EventItemID,DisplayOrder,FirstTypName,SecondTypeName,GroupState FROM M_V_EventItemGroupRel WHERE EventGroupID=@EventGroupID";
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return conn.Query<EventItemGroupRelViewEntity>(selectSQL, new { EventGroupID = eventGroupID }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        public EventItemGroupRelViewEntity QueryGroupRelViewEntity(int eventGroupID, int eventItemID)
        {
            try
            {
                string selectSQL = "SELECT Title, EventGroupID,EventItemID,DisplayOrder,FirstTypName,SecondTypeName,GroupState FROM M_V_EventItemGroupRel WHERE EventGroupID=@EventGroupID AND EventItemID=@EventItemID";
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return conn.Query<EventItemGroupRelViewEntity>(selectSQL, new { EventGroupID = eventGroupID, EventItemID = eventItemID }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        public EventItemGroupEntity QueryGroupEntity(int eventGroupID)
        {
            try
            {
                string selectSQL = "SELECT GroupEventID,GroupEventName,PublishTime,CreatedTime FROM M_EventItemGroups WHERE GroupEventID=@GroupEventID ";
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return conn.Query<EventItemGroupEntity>(selectSQL, new { GroupEventID = eventGroupID }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取前20个分组
        /// </summary>
        /// <returns></returns>
        public List<EventItemGroupEntity> QueryTop20GroupList(string groupName)
        {
            try
            {
                string selectSQL = "SELECT TOP 20 GroupEventID,GroupEventName FROM M_EventItemGroups  ORDER BY CreatedTime DESC";
                if (!string.IsNullOrEmpty(groupName))
                {
                    using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                    {
                        return conn.Query<EventItemGroupEntity>(selectSQL).ToList();
                    }
                }
                else
                {
                    selectSQL = "SELECT TOP 20 GroupEventID,GroupEventName FROM M_EventItemGroups WHERE GroupEventName LIKE '" + groupName.Trim() + "%' ORDER BY CreatedTime DESC";
                    using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                    {
                        return conn.Query<EventItemGroupEntity>(selectSQL).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return new List<EventItemGroupEntity>();
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="groupEntity"></param>
        /// <returns></returns>
        public bool InsertGroup(EventItemGroupEntity entity)
        {
            try
            {
                string insertSQL = "INSERT INTO M_EventItemGroups(GroupEventName,CreatedTime,PublishTime) VALUES(@GroupEventName,@CreatedTime,@PublishTime)";
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return conn.Execute(insertSQL, new { GroupEventName = entity.GroupEventName, CreatedTime = entity.CreatedTime, PublishTime = entity.PublishTime }) > 0;
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateGroup(EventItemGroupEntity entity)
        {
            try
            {
                string updateSQL = "UPDATE M_EventItemGroups SET GroupEventName=@GroupEventName,CreatedTime=@CreatedTime,PublishTime=@PublishTime  WHERE GroupEventID=@GroupEventID";
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return conn.Execute(updateSQL, new { GroupEventID = entity.GroupEventID, GroupEventName = entity.GroupEventName, CreatedTime = entity.CreatedTime, PublishTime = entity.PublishTime }) > 0;
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 删除分组及明细
        /// </summary>
        /// <param name="groupEventID"></param>
        /// <returns></returns>
        public bool DeleteGroup(int groupEventID)
        {
            bool isSuccess = false;
            SqlTransaction trans = null;

            try
            {
                string delGroupSQL = "Delete From M_EventItemGroups Where GroupEventID=@GroupEventID";
                string delGroupRel = "Delete From M_EventItem_Group_Rel Where EventGroupID=@EventGroupID";

                SqlParameter[] prms = {
                                        new SqlParameter("@GroupEventID", groupEventID),
                                        new SqlParameter("@EventGroupID", groupEventID)
                                      };

                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    conn.Open();
                    trans = conn.BeginTransaction();
                    int num1 = SQlHelper.ExecuteNonQuery(trans, CommandType.Text, delGroupSQL, prms);
                    int num2 = SQlHelper.ExecuteNonQuery(trans, CommandType.Text, delGroupRel, prms);

                    isSuccess = true;
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);

                if (trans != null)
                {
                    trans.Rollback();
                }
                return false;
            }

            return isSuccess;
        }

        /// <summary>
        /// 判断专题组名称是否可用
        /// </summary>
        /// <param name="groupEventID"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public bool IsUseableGroupName(int groupEventID, string groupEventName)
        {
            bool isUseable = false;
            string selectSQL = string.Empty;
            try
            {
                SqlParameter[] prms = {
                                        new SqlParameter("@GroupEventID", groupEventID),
                                        new SqlParameter("@GroupEventName", groupEventName)
                                     };

                if (groupEventID > 0)
                {
                    selectSQL = "SELECT COUNT(GroupEventID) FROM M_EventItemGroups Where GroupEventID != @GroupEventID AND GroupEventName=@GroupEventName";
                    object result = SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, selectSQL, prms);
                    int num = 0;

                    if (int.TryParse(result.ToString(), out num) && num == 0)
                    {
                        isUseable = true;
                    }
                }
                else
                {
                    selectSQL = "Select COUNT(GroupEventID) FROM M_EventItemGroups Where GroupEventName=@GroupEventName";
                    object result = SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, selectSQL, prms);
                    int num = 0;
                    if (int.TryParse(result.ToString(), out num) && num == 0)
                    {
                        isUseable = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }

            return isUseable;
        }

        /// <summary>
        /// 添加分组关联项
        /// </summary>
        /// <param name="groupEntity"></param>        
        /// <param name="groupRelEntityList"></param>
        /// <returns></returns>
        public bool InsertGroupRel(EventItemGroupEntity groupEntity, List<EventItemGroupRelEntity> groupRelEntityList)
        {
            bool isSuccess = false;
            SqlTransaction trans = null;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                string insertGroupSQL = "INSERT INTO M_EventItemGroups(GroupEventName,PublishTime,CreatedTime,GroupState) VALUES (@GroupEventName,@PublishTime,@CreatedTime,@GroupState);SELECT @@IDENTITY;";
                string insertRelSQL = "INSERT INTO M_EventItem_Group_Rel(EventGroupID,EventItemID,DisplayOrder) VALUES({0},{1},{2});";

                SqlParameter[] prms = {
                                         new SqlParameter("@GroupEventName", SqlDbType.VarChar, 100),
                                         new SqlParameter("@PublishTime", SqlDbType.DateTime),
                                         new SqlParameter("@CreatedTime", SqlDbType.DateTime),
                                         new SqlParameter("@GroupState",SqlDbType.Int)
                                      };
                prms[0].Value = groupEntity.GroupEventName;
                prms[1].Value = groupEntity.PublishTime;
                prms[2].Value = groupEntity.CreatedTime;
                prms[3].Value = groupEntity.GroupState;

                //去除重专题文章
                //groupRelEntityList = groupRelEntityList.Distinct(new EventItemGroupRelEntityDistinct()).ToList();

                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    conn.Open();
                    trans = conn.BeginTransaction();

                    int groupEventID = Convert.ToInt32(SQlHelper.ExecuteScalar(trans, CommandType.Text, insertGroupSQL, prms));

                    if (groupRelEntityList != null && groupRelEntityList.Count > 0)
                    {
                        foreach (EventItemGroupRelEntity groupRelEntity in groupRelEntityList)
                        {
                            sbInsert.AppendFormat(insertRelSQL, groupEventID, groupRelEntity.EventItemID, groupRelEntity.DisplayOrder);
                        }

                        int num2 = SQlHelper.ExecuteNonQuery(trans, CommandType.Text, sbInsert.ToString());
                    }

                    isSuccess = true;
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);

                if (trans != null)
                {
                    trans.Rollback();
                }
                return false;
            }

            return isSuccess;
        }

        /// <summary>
        /// 修改分组关联项
        /// </summary>        
        /// <param name="groupRelEntity"></param>
        /// <returns></returns>
        public bool UpdateGroupRel(int eventGroupID, List<EventItemGroupRelEntity> groupRelEntityList)
        {
            bool isSuccess = false;
            SqlTransaction trans = null;

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.AppendFormat("DELETE FROM M_EventItem_Group_Rel WHERE EventGroupID={0};", eventGroupID);
                string insertRelSQL = "INSERT INTO M_EventItem_Group_Rel(EventGroupID,EventItemID,DisplayOrder) VALUES({0},{1},{2});";

                //去除重专题文章
                //groupRelEntityList = groupRelEntityList.Distinct(new EventItemGroupRelEntityDistinct()).ToList();

                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    conn.Open();
                    trans = conn.BeginTransaction();

                    if (groupRelEntityList != null && groupRelEntityList.Count > 0)
                    {
                        foreach (EventItemGroupRelEntity groupRelEntity in groupRelEntityList)
                        {
                            sbUpdate.AppendFormat(insertRelSQL, eventGroupID, groupRelEntity.EventItemID, groupRelEntity.DisplayOrder);
                        }

                        int num2 = SQlHelper.ExecuteNonQuery(trans, CommandType.Text, sbUpdate.ToString());

                        isSuccess = true;
                        trans.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                if (trans != null)
                {
                    trans.Rollback();
                }
                return false;
            }

            return isSuccess;
        }

        /// <summary>
        /// 更新专题中指定文章显示序号
        /// </summary>
        /// <param name="eventGroupID"></param>
        /// <param name="eventItemID"></param>
        /// <param name="displayOrder"></param>
        /// <returns></returns>
        public bool UpdateGroupRelOrder(int eventGroupID, int eventItemID, int displayOrder)
        {
            bool isSuccess = false;

            try
            {
                string updateSQL = "UPDATE M_EventItem_Group_Rel SET DisplayOrder=@DisplayOrder WHERE EventGroupID=@EventGroupID AND EventItemID=@EventItemID";
                SqlParameter[] prms = {
                                          new SqlParameter("@DisplayOrder",displayOrder),
                                          new SqlParameter("@EventGroupID",eventGroupID),
                                          new SqlParameter("@EventItemID",eventItemID),
                                      };

                isSuccess = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, updateSQL, prms) > 0;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }

            return isSuccess;
        }

        /// <summary>
        /// 删除分组关联项
        /// </summary>
        /// <param name="groupRelEntity"></param>
        /// <returns></returns>
        public bool DeleteGroupRel(EventItemGroupRelEntity groupRelEntity)
        {
            bool isSuccess = false;

            try
            {
                string delSQL = "DELETE FROM M_EventItem_Group_Rel WHERE EventGroupID=@EventGroupID AND EventItemID=@EventItemID";
                SqlParameter[] prms = {
                                          new SqlParameter("@EventGroupID",groupRelEntity.EventGroupID),
                                          new SqlParameter("@EventItemID",groupRelEntity.EventItemID)
                                      };
                isSuccess = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, delSQL, prms) > 0;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }

            return isSuccess;
        }

        /// <summary>
        ///  以文章 EventItemID 删除 
        /// </summary>
        /// <param name="eventItemID">文章ID</param>
        /// <returns></returns>
        public bool DeleteGroupRel(int eventItemID)
        {
            bool isSuccess = false;
            try
            {
                string delSQL = "DELETE FROM M_EventItem_Group_Rel WHERE EventItemID=@EventItemID";
                SqlParameter[] prms = {
                                          new SqlParameter("@EventItemID",eventItemID)
                                      };
                isSuccess = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, delSQL, prms) > 0;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
            return isSuccess;
        }




        /// <summary>
        ///  判断指定专题中的文章，该显示序号是否可用
        /// </summary>
        /// <param name="eventGroupID"></param>
        /// <param name="eventItemID"></param>
        /// <param name="displayOrder"></param>
        /// <returns></returns>
        public bool IsUseableOrderByGroupArticle(int eventGroupID, int eventItemID, int displayOrder)
        {
            bool isUseable = false;
            string selectSQL = "SELECT COUNT(EventGroupID) FROM M_EventItem_Group_Rel Where EventGroupID=@EventGroupID";

            try
            {
                SqlParameter[] prms = {
                                        new SqlParameter("@EventGroupID", eventGroupID),
                                        new SqlParameter("@EventItemID", eventItemID),
                                        new SqlParameter("@DisplayOrder", displayOrder)
                                     };

                if (eventItemID > 0)
                {
                    selectSQL += " AND EventItemID != @EventItemID AND DisplayOrder=@DisplayOrder ";
                    object result = SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, selectSQL, prms);
                    int num = 0;

                    if (int.TryParse(result.ToString(), out num) && num == 0)
                    {
                        isUseable = true;
                    }
                }
                else
                {
                    selectSQL += " AND DisplayOrder=@DisplayOrder";
                    object result = SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, selectSQL, prms);
                    int num = 0;
                    if (int.TryParse(result.ToString(), out num) && num == 0)
                    {
                        isUseable = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
            return isUseable;
        }

        /// <summary>
        /// 判断文章列表中是否至少有一篇有封面图片
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <returns></returns>
        public bool IsHasCoverPicByGroupArticle(List<int> eventItemIDList)
        {
            bool isHas = false;

            try
            {
                string selectSQL = "SELECT COUNT(PictureID) FROM M_EventItem WHERE PictureID > 0 AND EventItemID in (" + string.Join(",", eventItemIDList) + ")";

                object result = SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, selectSQL);

                if (Convert.ToInt32(result) > 0)
                {
                    isHas = true;
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }

            return isHas;
        }

        #region 构造查询条件
        /// <summary>
        /// 生成拼接sql参数列表
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public List<SqlParameter> ParseToSqlParameters(EventItemGroupSearchEntity entity)
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
        /// 设置排序
        /// </summary>
        /// <param name="enity"></param>
        /// <returns></returns>
        private SqlParameter CPOrder(EventItemGroupSearchEntity enity)
        {
            StringBuilder sborder = new StringBuilder();

            if (enity.OrderfieldType == OrderFieldType.Desc)
            {
                sborder.AppendFormat(" GroupEventID DESC");
            }
            else
            {
                sborder.AppendFormat(" GroupEventID ASC");
            }

            return new SqlParameter("@OrderField", sborder.ToString());
        }


        /// <summary>
        /// 设置显示字段
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private SqlParameter CPFields(EventItemGroupSearchEntity entity)
        {
            StringBuilder sbfileds = new StringBuilder();
            if (entity.UseDBPagination)
            {
                sbfileds.Append(" GroupEventID,GroupEventName,CONVERT(VARCHAR(10),CreatedTime,121) AS CreatedTime,CONVERT(VARCHAR(16),PublishTime,121) AS PublishTime,GroupState");
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
        private SqlParameter CPTable(EventItemGroupSearchEntity entity)
        {
            StringBuilder sbtable = new StringBuilder();
            //基本表
            sbtable.Append(" M_EventItemGroups ");
            return new SqlParameter("@TableName", sbtable.ToString());
        }

        /// <summary>
        /// 查询条件
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private SqlParameter CPWhere(EventItemGroupSearchEntity entity)
        {
            StringBuilder sbwhere = new StringBuilder(" 1=1 ");
            sbwhere.Append(" AND GroupState=" + entity.GroupState);

            if (!string.IsNullOrEmpty(entity.GroupEventName))
            {
                sbwhere.Append(" AND GroupEventName like '" + entity.GroupEventName.Trim() + "%'");
            }

            if (entity.PublishStartTime.HasValue)
            {
                sbwhere.Append(" AND PublishTime >= '" + entity.PublishStartTime.Value.Date + "'");
            }

            if (entity.PublishEndTime.HasValue)
            {
                sbwhere.Append(" AND PublishTime <= '" + entity.PublishEndTime.Value.AddDays(1).Date.AddSeconds(-1) + "' ");
            }
            
            return new SqlParameter("@sqlWhere", sbwhere.ToString());
        }
        #endregion
    }

    /// <summary>
    /// 分组关联实体去除重复项
    /// </summary>
    public class EventItemGroupRelEntityDistinct : IEqualityComparer<EventItemGroupRelEntity>
    {
        public bool Equals(EventItemGroupRelEntity x, EventItemGroupRelEntity y)
        {
            return x.EventGroupID == y.EventGroupID && x.EventItemID == y.EventItemID;
        }

        public int GetHashCode(EventItemGroupRelEntity obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}
