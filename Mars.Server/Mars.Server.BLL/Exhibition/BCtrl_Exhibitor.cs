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
    public class BCtrl_Exhibitor
    {
        ExhibitorDAO dao = new ExhibitorDAO();
        /// <summary>
        /// 修改展商是否存在书目信息
        /// </summary>
        /// <param name="exhibitorID"></param>
        /// <param name="isHadBookList"></param>
        /// <returns></returns>
        public bool ChangeExhibitorIsHadBookList(int exhibitorID, int isHadBookList)
        {
            return dao.ChangeExhibitorIsHadBookList(exhibitorID, isHadBookList);
        }

        /// <summary>
        /// 已展场id为主键，查询所有展商
        /// </summary>
        /// <param name="exhibitionID">展场id</param>
        /// <returns></returns>
        public DataSet GetExhibitorDataSet(int exhibitionID)
        {
            return dao.GetExhibitorDataSet(exhibitionID);
        }

        /// <summary>
        /// 以展场id为主键，查询所有展商
        /// </summary>
        /// <param name="exhibitionID">展场id</param>
        /// <returns>展商List集合</returns>
        public List<ExhibitorToCustomerEntity> GetExhibitorEntityList(int exhibitionID)
        {
            List<ExhibitorToCustomerEntity> list = null;

            DataSet ds = this.GetExhibitorDataSet(exhibitionID);
            if (ds != null && ds.Tables.Count == 2)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    list = new List<ExhibitorToCustomerEntity>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ExhibitorToCustomerEntity item = this.GetExhibitorToCustomerEntity(dr, ds.Tables[1]);
                        if (item != null)
                        {
                            list.Add(item);
                        }
                    }
                }
            }

            return list;
        }

        public bool ImportExhibitors(DataTable table,int exhibitionID,string userid)
        {
            return dao.ImportExhibitors(table, exhibitionID, userid);
        }

        /// <summary>
        /// 以展场id为主键，查询所有展商
        /// </summary>
        /// <param name="exhibitionID">展场id</param>
        /// <returns>展商List集合</returns>
        public List<ExhibitorToCustomerEntity> GetExhibitorEntityListUseMyCache(int exhibitionID)
        {
            string key = string.Format(WebKeys.ExhibitorCacheKey, exhibitionID.ToString());
            List<ExhibitorToCustomerEntity> list = MyCache<List<ExhibitorToCustomerEntity>>.Get(key);
            if (list == null)
            {
                list = this.GetExhibitorEntityList(exhibitionID);
                MyCache<List<ExhibitorToCustomerEntity>>.Insert(key, list, 600);
            }

            return list;
        }

        /// <summary>
        /// 将后台的展商实例化
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="table_location"></param>
        /// <returns></returns>
        public List<ExhibitorEntity> GetConsoleExhibitorEntityList(DataTable dt, DataTable table_location)
        {
            List<ExhibitorEntity> list = null;

            if (dt != null)
            {
                list = new List<ExhibitorEntity>();

                foreach (DataRow dr in dt.Rows)
                {
                    ExhibitorEntity item = this.GetConsoleExhibitorEntity(dr, table_location);
                    if (item != null)
                    {
                        list.Add(item);
                    }
                }
            }

            return list;
        }

        private ExhibitorEntity GetConsoleExhibitorEntity(DataRow dr, DataTable table_location)
        {
            ExhibitorEntity item = null;

            if (dr != null)
            {
                item = new ExhibitorEntity();
                if (dr["ExhibitorID"] != null && dr["ExhibitorID"].ToString() != "")
                {
                    item.ExhibitorID = int.Parse(dr["ExhibitorID"].ToString());
                }
                if (dr["ExhibitionID"] != null && dr["ExhibitionID"].ToString() != "")
                {
                    item.ExhibitionID = int.Parse(dr["ExhibitionID"].ToString());
                }
                if (dr["ExhibitorName"] != null)
                {
                    item.ExhibitorName = dr["ExhibitorName"].ToString();
                }
                if (dr["ExhibitorPinYin"] != null)
                {
                    item.ExhibitorPinYin = dr["ExhibitorPinYin"].ToString();
                }
                if (dr["OBCustomerID"] != null && dr["OBCustomerID"].ToString() != "")
                {
                    item.OBCustomerID = int.Parse(dr["OBCustomerID"].ToString());
                }
                if (dr["OBCustomerTypeID"] != null && dr["OBCustomerTypeID"].ToString() != "")
                {
                    item.OBCustomerTypeID = int.Parse(dr["OBCustomerTypeID"].ToString());
                }
                if (dr["IsHadBookList"] != null && dr["IsHadBookList"].ToString() != "")
                {
                    if ((dr["IsHadBookList"].ToString() == "1") || (dr["IsHadBookList"].ToString().ToLower() == "true"))
                    {
                        item.IsHadBookList = true;
                    }
                    else
                    {
                        item.IsHadBookList = false;
                    }
                }
                if (dr["StateTypeID"] != null && dr["StateTypeID"].ToString() != "")
                {
                    item.StateTypeID = int.Parse(dr["StateTypeID"].ToString());
                }
                if (dr["CreateUserID"] != null)
                {
                    item.CreateUserID = dr["CreateUserID"].ToString();
                }
                if (dr["CreateTime"] != null && dr["CreateTime"].ToString() != "")
                {
                    item.CreateTime = DateTime.Parse(dr["CreateTime"].ToString());
                }
                if (dr["rowId"] != null && dr["rowId"].ToString() != "")
                {
                    item.rowId = int.Parse(dr["rowId"].ToString());
                }

                if (table_location != null)
                {
                    DataRow[] drs = table_location.Select(string.Format(" ExhibitorID = {0} ", item.ExhibitorID.ToShort()));
                    if (drs != null)
                    {
                        item.ExhibitorLocationList = new List<ExhibitorLocationEntity>();
                        foreach (DataRow drl in drs) {
                            ExhibitorLocationEntity locationEntity = this.GetConsoleExhibitorLocationEntity(drl);
                            if (locationEntity != null)
                            {
                                item.ExhibitorLocationList.Add(locationEntity);
                            }
                        }
                    }
                }
            }

            return item;
        }

        private ExhibitorLocationEntity GetConsoleExhibitorLocationEntity(DataRow drl)
        {
            ExhibitorLocationEntity item = null;

            if (drl != null)
            {
                item = new ExhibitorLocationEntity();

                if (drl["ExhibitorLocationID"] != null && drl["ExhibitorLocationID"].ToString() != "")
                {
                    item.ExhibitorLocationID = int.Parse(drl["ExhibitorLocationID"].ToString());
                }
                if (drl["ExhibitorID"] != null && drl["ExhibitorID"].ToString() != "")
                {
                    item.ExhibitorID = int.Parse(drl["ExhibitorID"].ToString());
                }
                if (drl["ExhibitorLocation"] != null)
                {
                    item.ExhibitorLocation = drl["ExhibitorLocation"].ToString();
                }
                if (drl["StateTypeID"] != null && drl["StateTypeID"].ToString() != "")
                {
                    item.StateTypeID = int.Parse(drl["StateTypeID"].ToString());
                }
                if (drl["ExhibitiorLocationOrder"] != null && drl["ExhibitiorLocationOrder"].ToString() != "")
                {
                    item.ExhibitiorLocationOrder = int.Parse(drl["ExhibitiorLocationOrder"].ToString());
                }
                if (drl["CreateUserID"] != null)
                {
                    item.CreateUserID = drl["CreateUserID"].ToString();
                }
                if (drl["CreateTime"] != null && drl["CreateTime"].ToString() != "")
                {
                    item.CreateTime = DateTime.Parse(drl["CreateTime"].ToString());
                }
            }

            return item;
        }

        /// <summary>
        /// 实例化单个返回给客户的展商信息
        /// </summary>
        /// <param name="dr">展场信息datarow</param>
        /// <param name="table">展位的表格</param>
        /// <returns>展商的信息</returns>
        private ExhibitorToCustomerEntity GetExhibitorToCustomerEntity(DataRow dr, DataTable table)
        {
            ExhibitorToCustomerEntity item = null;

            if(dr!=null)
            {
                item = new ExhibitorToCustomerEntity();
                if (dr["ExhibitorID"] != null && dr["ExhibitorID"].ToString() != "")
                {
                    item.ExhibitorID = int.Parse(dr["ExhibitorID"].ToString());
                }
                if (dr["ExhibitionID"] != null && dr["ExhibitionID"].ToString() != "")
                {
                    item.ExhibitionID = int.Parse(dr["ExhibitionID"].ToString());
                }
                if (dr["ExhibitorName"] != null)
                {
                    item.ExhibitorName = dr["ExhibitorName"].ToString();
                }
                if (dr["ExhibitorPinYin"] != null)
                {
                    item.ExhibitorPinYin = dr["ExhibitorPinYin"].ToString();
                }
                if (dr["IsHadBookList"] != null && dr["IsHadBookList"].ToString() != "")
                {
                    if ((dr["IsHadBookList"].ToString() == "1") || (dr["IsHadBookList"].ToString().ToLower() == "true"))
                    {
                        item.IsHadBookList = true;
                    }
                    else
                    {
                        item.IsHadBookList = false;
                    }
                }

                DataRow[] drs = table.Select("ExhibitorID = "+item.ExhibitorID.ToString());
                if(drs!=null)
                {
                    if(drs.Length>0)
                    {
                        item.ExhibitorLocationList = new List<ExhibitorToCustomerLocationEntity>();
                        foreach (DataRow drl in drs)
                        {
                            ExhibitorToCustomerLocationEntity locationItem = this.GetExhibitorToCustomerLocationEntity(drl);
                            if (locationItem != null)
                            {
                                item.ExhibitorLocationList.Add(locationItem);
                            }
                        }
                    }

                }

            }

            return item;
        }

        /// <summary>
        /// 根据展位信息实例化展位对象
        /// </summary>
        /// <param name="drl">展位datarow</param>
        /// <returns>展位对象</returns>
        private ExhibitorToCustomerLocationEntity GetExhibitorToCustomerLocationEntity(DataRow drl)
        {
            ExhibitorToCustomerLocationEntity item = null;

            if (drl != null)
            {
                item = new ExhibitorToCustomerLocationEntity();
                if (drl["ExhibitorLocationID"] != null && drl["ExhibitorLocationID"].ToString() != "")
                {
                    item.ExhibitorLocationID = int.Parse(drl["ExhibitorLocationID"].ToString());
                }
                if (drl["ExhibitorLocation"] != null)
                {
                    item.ExhibitorLocation = drl["ExhibitorLocation"].ToString();
                }
                if (drl["ExhibitiorLocationOrder"] != null && drl["ExhibitiorLocationOrder"].ToString() != "")
                {
                    item.ExhibitiorLocationOrder = int.Parse(drl["ExhibitiorLocationOrder"].ToString());
                }
            }

            return item;
        }

        /// <summary>
        /// 删除展商 假删除
        /// </summary>
        /// <param name="exhibitorID"></param>
        /// <returns></returns>
        public bool DeleteExhibitor(int exhibitorID)
        {
            return dao.DeleteExhibitor(exhibitorID);
        }

        /// <summary>
        /// 使用缓存查询信息
        /// </summary>
        /// <param name="info">查询主体</param>
        /// <param name="totalcnt">返回条目数</param>
        /// <returns></returns>
        public DataTable QueryData(ExhibitorSearchEntity info,out int totalcnt)
        {
            if (info.UseDBPagination)
            {
                string key = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(info.ToJson(info), "MD5");
                DataTableRSCacheEntity rsobj = MyCache<DataTableRSCacheEntity>.Get(key);
                if (rsobj == null)
                {
                    DataTable dt =dao.QueryData(info, out totalcnt);
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
        public List<ExhibitorToCustomerEntity> GetExhibitorEntityList(DataTable table)
        {
            List<ExhibitorToCustomerEntity> list = null;

            if (table != null)
            {
                //去重展商
                DataTable tableExhibitor = table.DefaultView.ToTable(true, "ExhibitorID", "ExhibitionID", "ExhibitorName", "ExhibitorPinYin", "IsHadBookList");
                if (tableExhibitor != null)
                {
                    list = new List<ExhibitorToCustomerEntity>();
                    foreach (DataRow dr in tableExhibitor.Rows)
                    {
                        ExhibitorToCustomerEntity item = this.GetExhibitorToCustomerEntity(dr, table);
                        if (item != null)
                        {
                            list.Add(item);
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// 根据表格返回值，查询所有展商
        /// </summary>
        /// <param name="table">展商表格</param>
        /// <returns>展商List集合</returns>
        public List<ExhibitorToCustomerEntity> GetExhibitorEntityList(DataTable table,DataTable tableLocation)
        {
            List<ExhibitorToCustomerEntity> list = null;

            if (table != null)
            {
                //去重展商
                DataTable tableExhibitor = table.DefaultView.ToTable(true, "ExhibitorID", "ExhibitionID", "ExhibitorName", "ExhibitorPinYin", "IsHadBookList");
                if (tableExhibitor != null)
                {
                    list = new List<ExhibitorToCustomerEntity>();
                    foreach (DataRow dr in tableExhibitor.Rows)
                    {
                        ExhibitorToCustomerEntity item = this.GetExhibitorToCustomerEntity(dr, tableLocation);
                        if (item != null)
                        {
                            list.Add(item);
                        }
                    }
                }
            }

            return list;
        }

        public DataTable QueryDataConsole(ExhibitorSearchEntity info, out int totalcnt)
        {
            return dao.QueryDataConsole(info, out totalcnt);  
        }
        /// <summary>
        /// 获取展会所有展商地址
        /// </summary>
        /// <param name="exhibitionID"></param>
        /// <returns></returns>
        public DataTable GetExhibitorLocation(int exhibitionID)
        {
            string key = string.Format(WebKeys.ExhibitorLocationConsoleCacheKey, exhibitionID.ToString());
            DataTableRSCacheEntity rsobj = MyCache<DataTableRSCacheEntity>.Get(key);
            if (rsobj == null)
            {
                DataTable dt = dao.GetExhibitorLocation(exhibitionID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    rsobj = new DataTableRSCacheEntity() { Source = dt, TotalCnt = dt.Rows.Count };
                    MyCache<DataTableRSCacheEntity>.Insert(key, rsobj, 60);
                }
                else
                {
                    rsobj = new DataTableRSCacheEntity();
                }
            }
            return rsobj.Source;

            //return dao.GetExhibitorLocation(exhibitionID);
        }

        /// <summary>
        /// 添加新展商
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int Add_Exhibitor(ExhibitorEntity item)
        {
            return dao.Add_Exhibitor(item);
        }
        /// <summary>
        /// 更新展商
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Update_Exhibitor(ExhibitorEntity item)
        {
            return dao.Update_Exhibitor(item);
        }


        public ExhibitorEntity GetExhibitorEntity(int exhibitorID)
        {
            ExhibitorEntity item = null;
            DataSet ds = dao.GetExhibitorEntityDataSet(exhibitorID);
            if (ds != null && ds.Tables.Count == 2)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    item = this.GetConsoleExhibitorEntity(ds.Tables[0].Rows[0], ds.Tables[1]);
                }
            }

            return item;
        }
    }
}
