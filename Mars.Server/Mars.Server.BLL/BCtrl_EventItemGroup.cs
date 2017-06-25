using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mars.Server.DAO;
using Mars.Server.Entity;
using System.Data;

namespace Mars.Server.BLL
{
    public class BCtrl_EventItemGroup
    {
        EventItemGroupDAO dao = new EventItemGroupDAO();

        public DataTable QueryGroupTable(EventItemGroupSearchEntity searchEntity, out int totalcnt)
        {
            return dao.QueryGroupTable(searchEntity, out totalcnt);
        }

        public DataTable QueryGroupRelViewList(EventItemGroupSearchEntity searchEntity, out int totalcnt)
        {
            return dao.QueryGroupRelViewList(searchEntity, out totalcnt);
        }

        public List<EventItemGroupRelViewEntity> QueryGroupRelViewList(int eventGroupID)
        {
            return dao.QueryGroupRelViewList(eventGroupID);
        }

         public EventItemGroupRelViewEntity QueryGroupRelViewEntity(int eventGroupID, int eventItemID)
        {
            return dao.QueryGroupRelViewEntity(eventGroupID, eventItemID);
        }

        public EventItemGroupEntity QueryGroupEntity(int eventGroupID)
        {
            return dao.QueryGroupEntity(eventGroupID);
        }

        /// <summary>
        /// 获取前20个分组
        /// </summary>
        /// <returns></returns>
        public List<EventItemGroupEntity> QueryTop20GroupList(string groupName)
        {
            return dao.QueryTop20GroupList(groupName);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="groupEntity"></param>
        /// <returns></returns>
        public bool InsertGroup(EventItemGroupEntity entity)
        {
            return dao.InsertGroup(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateGroup(EventItemGroupEntity entity)
        {
            return dao.UpdateGroup(entity);
        }

        /// <summary>
        /// 删除分组及明细
        /// </summary>
        /// <param name="groupEventID"></param>
        /// <returns></returns>
        public bool DeleteGroup(int groupEventID)
        {
            return dao.DeleteGroup(groupEventID);
        }

        /// <summary>
        /// 判断专题组名称是否可用
        /// </summary>
        /// <param name="groupEventID"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public bool IsUseableGroupName(int groupEventID, string groupEventName)
        {
            return dao.IsUseableGroupName(groupEventID, groupEventName);
        }

        /// <summary>
        /// 添加分组关联项
        /// </summary>
        /// <param name="groupEntity"></param>        
        /// <param name="groupRelEntityList"></param>
        /// <returns></returns>
        public bool InsertGroupRel(EventItemGroupEntity groupEntity, List<EventItemGroupRelEntity> groupRelEntityList)
        {
            return dao.InsertGroupRel(groupEntity, groupRelEntityList);
        }

        /// <summary>
        /// 修改分组关联项
        /// </summary>        
        /// <param name="groupRelEntity"></param>
        /// <returns></returns>
        public bool UpdateGroupRel(int eventGroupID, List<EventItemGroupRelEntity> groupRelEntityList)
        {
            return dao.UpdateGroupRel(eventGroupID, groupRelEntityList);
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
            return dao.UpdateGroupRelOrder(eventGroupID, eventItemID, displayOrder);
        }

        /// <summary>
        /// 删除分组关联项
        /// </summary>
        /// <param name="groupRelEntity"></param>
        /// <returns></returns>
        public bool DeleteGroupRel(EventItemGroupRelEntity groupRelEntity)
        {
            return dao.DeleteGroupRel(groupRelEntity);
        }

        /// <summary>
        ///  以文章 EventItemID 删除 
        /// </summary>
        /// <param name="eventItemID">文章ID</param>
        /// <returns></returns>
        public bool DeleteGroupRel(int eventItemID) 
        {
            return dao.DeleteGroupRel(eventItemID);
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
            return dao.IsUseableOrderByGroupArticle(eventGroupID, eventItemID, displayOrder);
        }

            /// <summary>
        /// 判断文章列表中是否至少有一篇有封面图片
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <returns></returns>
        public bool IsHasCoverPicByGroupArticle(List<int> eventItemIDList)
        {
            return dao.IsHasCoverPicByGroupArticle(eventItemIDList);
        }
    }
}
