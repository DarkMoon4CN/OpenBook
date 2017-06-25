using Mars.Server.DAO.Exhibition;
using Mars.Server.Entity.Exhibition;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Mars.Server.BLL.Exhibition
{
    public class BCtrl_Console_Activity
    {
        Console_ActivityDAO dao = new Console_ActivityDAO();

        /// <summary>
        /// 使用缓存查询信息
        /// </summary>
        /// <param name="info">查询主体</param>
        /// <param name="totalcnt">返回条目数</param>
        /// <returns></returns>
        public DataTable QueryData(ActivitySearchEntity info, out int totalcnt)
        {
            return dao.QueryData(info, out totalcnt);
        }

        /// <summary>
        /// 删除活动，假删除，删除时同时会删除所有相关子活动
        /// </summary>
        /// <param name="activityID"></param>
        /// <returns></returns>
        public bool DeleteActivity(int activityID)
        {
            return dao.DeleteActivity(activityID);
        }

        /// <summary>
        /// 更新活动信息
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Update_Activity(ActivityEntity item)
        {
            return dao.Update_Activity(item);
        }
        /// <summary>
        /// 添加活动信息
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int Add_Activity(ActivityEntity item)
        {
            return dao.Add_Activity(item);
        }

        public ActivityEntity GetEntity(int id)
        {
            ActivityEntity item = null;
            DataTable table = this.GetTable(id);
            if (table != null)
            {
                item = new ActivityEntity();
                DataRow row = table.Rows[0];
                if (row["ActivityID"] != null && row["ActivityID"].ToString() != "")
                {
                    item.ActivityID = int.Parse(row["ActivityID"].ToString());
                }
                if (row["ParentID"] != null && row["ParentID"].ToString() != "")
                {
                    item.ParentID = int.Parse(row["ParentID"].ToString());
                }
                if (row["ExhibitorID"] != null && row["ExhibitorID"].ToString() != "")
                {
                    item.ExhibitorID = int.Parse(row["ExhibitorID"].ToString());
                }
                if (row["ActivityTitle"] != null)
                {
                    item.ActivityTitle = row["ActivityTitle"].ToString();
                }
                if (row["ActivityStartTime"] != null && row["ActivityStartTime"].ToString() != "")
                {
                    item.ActivityStartTime = DateTime.Parse(row["ActivityStartTime"].ToString());
                }
                if (row["ActivityEndTime"] != null && row["ActivityEndTime"].ToString() != "")
                {
                    item.ActivityEndTime = DateTime.Parse(row["ActivityEndTime"].ToString());
                }
                if (row["ActivityLocation"] != null)
                {
                    item.ActivityLocation = row["ActivityLocation"].ToString();
                }
                if (row["ActivityHostUnit"] != null)
                {
                    item.ActivityHostUnit = row["ActivityHostUnit"].ToString();
                }
                if (row["ActivityAbstract"] != null)
                {
                    item.ActivityAbstract = row["ActivityAbstract"].ToString();
                }
                if (row["ActivityGuest"] != null)
                {
                    item.ActivityGuest = row["ActivityGuest"].ToString();
                }
                if (row["StateTypeID"] != null && row["StateTypeID"].ToString() != "")
                {
                    item.StateTypeID = int.Parse(row["StateTypeID"].ToString());
                }
                if (row["ActivityOrder"] != null && row["ActivityOrder"].ToString() != "")
                {
                    item.ActivityOrder = int.Parse(row["ActivityOrder"].ToString());
                }
                if (row["ActivityTypeID"] != null && row["ActivityTypeID"].ToString() != "")
                {
                    item.ActivityTypeID = int.Parse(row["ActivityTypeID"].ToString());
                }
                if (row["CreateUserID"] != null)
                {
                    item.CreateUserID = row["CreateUserID"].ToString();
                }
                if (row["CreateTime"] != null && row["CreateTime"].ToString() != "")
                {
                    item.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
                }

                if (row.Table.Columns.Contains("ExhibitionID"))
                {
                    if (row["ExhibitionID"] != null && row["ExhibitionID"].ToString() != "")
                    {
                        item.ExhibitionID = int.Parse(row["ExhibitionID"].ToString());
                    }
                }
            }
            return item;
        }

        public bool ImportActivitys(DataTable table, int eid, string sys_UserID)
        {
            return dao.ImportActivitys(table, eid, sys_UserID);
        }

        public DataTable GetTable(int id)
        {
            return dao.GetTable(id);
        }
    }
}
