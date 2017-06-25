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
    public class BCtrl_Exhibition
    {
        ExhibitionDAO dao = new ExhibitionDAO();
        public bool IsPublished()
        {
            return dao.IsPublished();
        }


        /// <summary>
        /// 使用缓存查询信息
        /// </summary>
        /// <param name="info">查询主体</param>
        /// <param name="totalcnt">返回条目数</param>
        /// <returns></returns>
        public DataTable QueryData(ExhibitionSearchEntity info, out int totalcnt)
        {
            //if (info.UseDBPagination)
            //{
            //    string key = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(info.ToJson(info), "MD5");
            //    DataTableRSCacheEntity rsobj = MyCache<DataTableRSCacheEntity>.Get(key);
            //    if (rsobj == null)
            //    {
            //        DataTable dt = dao.QueryData(info, out totalcnt);
            //        if (dt != null && dt.Rows.Count > 0)
            //        {
            //            rsobj = new DataTableRSCacheEntity() { Source = dt, TotalCnt = totalcnt };
            //            MyCache<DataTableRSCacheEntity>.Insert(key, rsobj, 60);
            //        }
            //        else
            //        {
            //            rsobj = new DataTableRSCacheEntity();
            //        }
            //    }
            //    totalcnt = rsobj.TotalCnt;
            //    return rsobj.Source;
            //}
            //else
            //{
            //    return dao.QueryData(info, out totalcnt);
            //}
            return dao.QueryData(info, out totalcnt);
        }

        public ExhibitionEntity GetEntity(int id)
        {
            ExhibitionEntity item = null;

            DataSet ds = this.GetExhibitionDataSet(id);
            if (ds != null && ds.Tables.Count == 2)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    item = new ExhibitionEntity();
                    DataRow row = ds.Tables[0].Rows[0];

                    if (row["ExhibitionID"] != null && row["ExhibitionID"].ToString() != "")
                    {
                        item.ExhibitionID = int.Parse(row["ExhibitionID"].ToString());
                    }
                    if (row["ExhibitionTitle"] != null)
                    {
                        item.ExhibitionTitle = row["ExhibitionTitle"].ToString();
                    }
                    if (row["ExhibitionLogoUrl"] != null)
                    {
                        item.ExhibitionLogoUrl = row["ExhibitionLogoUrl"].ToString();
                    }
                    if (row["ExhibitionStartTime"] != null && row["ExhibitionStartTime"].ToString() != "")
                    {
                        item.ExhibitionStartTime = DateTime.Parse(row["ExhibitionStartTime"].ToString());
                    }
                    if (row["ExhibitionEndTime"] != null && row["ExhibitionEndTime"].ToString() != "")
                    {
                        item.ExhibitionEndTime = DateTime.Parse(row["ExhibitionEndTime"].ToString());
                    }
                    if (row["ExhibitionAddress"] != null)
                    {
                        item.ExhibitionAddress = row["ExhibitionAddress"].ToString();
                    }
                    if (row["ExhibitionTraffic"] != null)
                    {
                        item.ExhibitionTraffic = row["ExhibitionTraffic"].ToString();
                    }
                    if (row["ExhibitionLocation"] != null)
                    {
                        item.ExhibitionLocation = row["ExhibitionLocation"].ToString();
                    }
                    if (row["ExhibitionAbstract"] != null)
                    {
                        item.ExhibitionAbstract = row["ExhibitionAbstract"].ToString();
                    }
                    if (row["ExhibitionAbout"] != null)
                    {
                        item.ExhibitionAbout = row["ExhibitionAbout"].ToString();
                    }
                    if (row["ExhibitionOrder"] != null && row["ExhibitionOrder"].ToString() != "")
                    {
                        item.ExhibitionOrder = int.Parse(row["ExhibitionOrder"].ToString());
                    }
                    if (row["ExhibitionBookListDesc"] != null)
                    {
                        item.ExhibitionBookListDesc = row["ExhibitionBookListDesc"].ToString();
                    }
                    if (row["StateTypeID"] != null && row["StateTypeID"].ToString() != "")
                    {
                        item.StateTypeID = int.Parse(row["StateTypeID"].ToString());
                    }
                    if (row["IsPublish"] != null && row["IsPublish"].ToString() != "")
                    {
                        if ((row["IsPublish"].ToString() == "1") || (row["IsPublish"].ToString().ToLower() == "true"))
                        {
                            item.IsPublish = true;
                        }
                        else
                        {
                            item.IsPublish = false;
                        }
                    }
                    if (row["BookListDownloadUrl"] != null)
                    {
                        item.BookListDownloadUrl = row["BookListDownloadUrl"].ToString();
                    }
                    if (row["IsDownloadBookList"] != null && row["IsDownloadBookList"].ToString() != "")
                    {
                        if ((row["IsDownloadBookList"].ToString() == "1") || (row["IsDownloadBookList"].ToString().ToLower() == "true"))
                        {
                            item.IsDownloadBookList = true;
                        }
                        else
                        {
                            item.IsDownloadBookList = false;
                        }
                    }
                    if (row["CreateUserID"] != null)
                    {
                        item.CreateUserID = row["CreateUserID"].ToString();
                    }
                    if (row["CreateTime"] != null && row["CreateTime"].ToString() != "")
                    {
                        item.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
                    }

                    item.AdvertisementList = new List<AdvertisementEntity>();
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dra in ds.Tables[1].Rows)
                        {
                            if (dra != null)
                            {
                                AdvertisementEntity _item = new AdvertisementEntity();
                                if (dra["AdvertisementID"] != null && dra["AdvertisementID"].ToString() != "")
                                {
                                    _item.AdvertisementID = int.Parse(dra["AdvertisementID"].ToString());
                                }
                                if (dra["ExhibitionID"] != null && dra["ExhibitionID"].ToString() != "")
                                {
                                    _item.ExhibitionID = int.Parse(dra["ExhibitionID"].ToString());
                                }
                                if (dra["AdvertisementUrl"] != null)
                                {
                                    _item.AdvertisementUrl = dra["AdvertisementUrl"].ToString();
                                }
                                if (dra["AdvertisementTitle"] != null)
                                {
                                    _item.AdvertisementTitle = dra["AdvertisementTitle"].ToString();
                                }
                                if (dra["AdvertisementOrder"] != null && dra["AdvertisementOrder"].ToString() != "")
                                {
                                    _item.AdvertisementOrder = int.Parse(dra["AdvertisementOrder"].ToString());
                                }
                                if (dra["StateTypeID"] != null && dra["StateTypeID"].ToString() != "")
                                {
                                    _item.StateTypeID = int.Parse(dra["StateTypeID"].ToString());
                                }
                                if (dra["CreateUserID"] != null)
                                {
                                    _item.CreateUserID = dra["CreateUserID"].ToString();
                                }
                                if (dra["CreateTime"] != null && dra["CreateTime"].ToString() != "")
                                {
                                    _item.CreateTime = DateTime.Parse(dra["CreateTime"].ToString());
                                }

                                item.AdvertisementList.Add(_item);
                            }
                        }
                    }
                }
            }

            return item;
        }

        public bool Save(ExhibitionEntity item)
        {
            bool returnValue = false;

            if (item.ExhibitionID > 0)
            {
                returnValue = dao.Update(item);
            }
            else
            {
                returnValue = dao.Add(item) > 0;
            }

            return returnValue;
        }

        public DataSet GetExhibitionDataSet(int id)
        {
            return dao.GetExhibitionDataSet(id);
        }

        /// <summary>
        /// 删除展会信息（假删除，讲展会信息的公共状态修改）
        /// </summary>
        /// <param name="exhibitionID"></param>
        /// <returns></returns>
        public bool DeleteExhibition(int exhibitionID)
        {
            return dao.DeleteExhibition(exhibitionID);
        }

        public DataTable GetExhibitionTable(bool isAll = false) {
            return dao.GetExhibitionTable(isAll);
        }


    }
}
