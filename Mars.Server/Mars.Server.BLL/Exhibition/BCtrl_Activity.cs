using Mars.Server.DAO.Exhibition;
using Mars.Server.Entity;
using Mars.Server.Entity.Exhibition;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Mars.Server.BLL.Exhibition
{
    public class BCtrl_Activity
    {
        ActivityDAO dao = new ActivityDAO();

        /// <summary>
        /// 根据展场id 获取展场相关活动信息
        /// </summary>
        /// <param name="exhibitionID">展场id</param>
        /// <returns>活动表格</returns>
        public DataTable GetActivityDataTable(int exhibitionID)
        {
            return dao.GetActivityDataTable(exhibitionID);
        }

        /// <summary>
        /// 根据展场id 获取展场相关活动信息
        /// </summary>
        /// <param name="exhibitionID">展场id</param>
        /// <returns>活动集合</returns>
        public List<ActivityToCustomerEntity> GetActivityEntityList(int exhibitionID)
        {
            List<ActivityToCustomerEntity> list = GetActivityEntityList(this.GetActivityDataTable(exhibitionID));

            return list;
        }

        /// <summary>
        /// 以展场id为主键，查询所有展商
        /// </summary>
        /// <param name="exhibitionID">展场id</param>
        /// <returns>展商List集合</returns>
        public List<ActivityToCustomerEntity> GetActivityEntityListUseMyCache(int exhibitionID)
        {
            string key = string.Format(WebKeys.ActivityCacheKey, exhibitionID.ToString());
            List<ActivityToCustomerEntity> list = MyCache<List<ActivityToCustomerEntity>>.Get(key);
            if (list == null)
            {
                list = this.GetActivityEntityList(exhibitionID);
                MyCache<List<ActivityToCustomerEntity>>.Insert(key, list, 600);
            }

            return list;
        }

        /// <summary>
        /// 实例化单个返回给客户的展商信息
        /// </summary>
        /// <param name="dr">展场信息datarow</param>
        /// <param name="table">展位的表格</param>
        /// <returns>展商的信息</returns>
        private ActivityToCustomerEntity GetActivityToCustomerEntity(DataRow dr,DataTable table)
        {
            ActivityToCustomerEntity item = null;

            if (dr != null)
            {
                item = new ActivityToCustomerEntity();
                if (dr["ActivityID"] != null && dr["ActivityID"].ToString() != "")
                {
                    item.ActivityID = int.Parse(dr["ActivityID"].ToString());
                }
                if (dr["ExhibitorID"] != null && dr["ExhibitorID"].ToString() != "")
                {
                    item.ExhibitorID = int.Parse(dr["ExhibitorID"].ToString());
                }
                if (dr["ActivityTitle"] != null)
                {
                    item.ActivityTitle = dr["ActivityTitle"].ToString();
                }
                if (dr["ActivityStartTime"] != null && dr["ActivityStartTime"].ToString() != "")
                {
                    item.ActivityStartTime = DateTime.Parse(dr["ActivityStartTime"].ToString());
                    item.FormatActivityStartTime = item.ActivityStartTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (dr["ActivityEndTime"] != null && dr["ActivityEndTime"].ToString() != "")
                {
                    item.ActivityEndTime = DateTime.Parse(dr["ActivityEndTime"].ToString());
                    item.FormatActivityEndTiem = item.ActivityEndTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (dr["ActivityLocation"] != null)
                {
                    item.ActivityLocation = dr["ActivityLocation"].ToString();
                }
                if (dr["ActivityHostUnit"] != null)
                {
                    item.ActivityHostUnit = dr["ActivityHostUnit"].ToString();
                }
                if (dr["ActivityAbstract"] != null)
                {
                    item.ActivityAbstract = dr["ActivityAbstract"].ToString();
                }
                if (dr["ActivityGuest"] != null)
                {
                    item.ActivityGuest = dr["ActivityGuest"].ToString();
                }
                if (dr["ExhibitorName"] != null)
                {
                    item.ExhibitorName = dr["ExhibitorName"].ToString();
                }
                if (dr["ExhibitorPinYin"] != null)
                {
                    item.ExhibitorPinYin = dr["ExhibitorPinYin"].ToString();
                }
                if (dr["ActivityIsEnd"] != null && dr["ActivityIsEnd"].ToString() != "")
                {
                    item.ActivityIsEnd = int.Parse(dr["ActivityIsEnd"].ToString())==1;
                }
                if (item.ActivityStartTime!=null && item.ActivityEndTime != null)
                {
                    if (item.ActivityStartTime.DayOfYear == item.ActivityEndTime.DayOfYear)
                    {
                        item.FormatActivityTime = item.ActivityStartTime.ToString("yyyy年MM月dd日 HH:mm") + " - " + item.ActivityEndTime.ToString("HH:mm");
                    }
                    else
                    {
                        item.FormatActivityTime = item.ActivityStartTime.ToString("yyyy年MM月dd日 HH:mm") + " - " + item.ActivityEndTime.ToString("MM月dd日 HH:mm");
                    }
                }


                item.SubactivityList = new List<ActivityToCustomerEntity>();

                DataRow[] drs = table.Select("ParentID = " + item.ActivityID);
                if (drs != null)
                {
                    if (drs.Length > 0)
                    {
                        foreach (DataRow drsub in drs)
                        {
                            ActivityToCustomerEntity subItem = this.GetActivityToCustomerEntity(drsub, table);
                            if (subItem != null)
                            {
                                item.SubactivityList.Add(subItem);
                            }
                        }
                    }
                }
            }

            return item;
        }

        /// <summary>
        /// 使用缓存查询信息
        /// </summary>
        /// <param name="info">查询主体</param>
        /// <param name="totalcnt">返回条目数</param>
        /// <returns></returns>
        public DataTable QueryData(ActivitySearchEntity info, out int totalcnt)
        {
            if (info.UseDBPagination)
            {
                string key = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(info.ToJson(info), "MD5");
                DataTableRSCacheEntity rsobj = MyCache<DataTableRSCacheEntity>.Get(key);
                if (rsobj == null)
                {
                    DataTable dt = dao.QueryData(info, out totalcnt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        rsobj = new DataTableRSCacheEntity() { Source = dt, TotalCnt = totalcnt };
                        MyCache<DataTableRSCacheEntity>.Insert(key, rsobj, 60);
                    }
                    else
                    {
                        rsobj = new DataTableRSCacheEntity();
                    }
                }
                totalcnt = rsobj.TotalCnt;
                return rsobj.Source;
            }
            else
            {
                return dao.QueryData(info, out totalcnt);
            }
        }

        /// <summary>
        /// 根据表格返回值，查询所有展商
        /// </summary>
        /// <param name="table">展商表格</param>
        /// <returns>展商List集合</returns>
        public List<ActivityToCustomerEntity> GetActivityEntityList(DataTable table)
        {
            List<ActivityToCustomerEntity> list = null;

            if (table != null)
            {
                DataRow[] drs = table.Select("ParentID = 0");
                if (drs != null)
                {
                    if (drs.Length > 0)
                    {
                        list = new List<ActivityToCustomerEntity>();
                        foreach (DataRow dr in drs)
                        {
                            ActivityToCustomerEntity item = this.GetActivityToCustomerEntity(dr, table);
                            if (item != null)
                            {
                                list.Add(item);
                            }
                        }
                    }
                }
            }

            return list;
        }
    }
}
