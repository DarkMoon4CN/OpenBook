using Mars.Server.DAO.NewYear;
using Mars.Server.Entity;
using Mars.Server.Entity.NewYear;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Mars.Server.BLL.NewYear
{
    public class BCtrl_Couplet
    {
        CoupletDAO dao = new CoupletDAO();

        /// <summary>
        /// 得到对联表
        /// </summary>
        /// <returns></returns>
        public DataTable GetCoupletTable()
        {
            return dao.GetCoupletTable();
        }

        /// <summary>
        /// 得到对联表
        /// </summary>
        /// <returns></returns>
        public DataTable GetFuImageTable()
        {
            return dao.GetFuImageTable();
        }

        /// <summary>
        /// 得到对联表
        /// </summary>
        /// <returns></returns>
        public DataTable GetFuImageTable(int iId)
        {
            return dao.GetFuImageTable(iId);
        }

        /// <summary>
        /// 得到对联表
        /// </summary>
        /// <returns></returns>
        public DataTable GetCoupletTable(int cid)
        {
            return dao.GetCoupletTable(cid);
        }

        public List<FuImageEntity> GetFuImageList() {
            List<FuImageEntity> list = null;
            DataTable table = this.GetFuImageTable();
            if (table != null)
            {
                list = new List<FuImageEntity>();
                foreach (DataRow dr in table.Rows)
                {
                    FuImageEntity item = DataRowToImageModel(dr);
                    list.Add(item);
                }
            }
            return list;
        }

        public FuImageEntity GetFuImageEntity(int iId)
        {
            FuImageEntity item = null;
            DataTable table = this.GetFuImageTable(iId);
            if (table != null)
            {
                item = DataRowToImageModel(table.Rows[0]);
            }
            return item;
        }

        private FuImageEntity DataRowToImageModel(DataRow dr)
        {
            FuImageEntity item = null;

            if (dr != null)
            {
                item = new FuImageEntity();
                if (dr["ImageID"] != null && dr["ImageID"].ToString() != "")
                {
                    item.ImageID = int.Parse(dr["ImageID"].ToString());
                }
                if (dr["ImageUrl"] != null)
                {
                    item.ImageUrl = dr["ImageUrl"].ToString();
                }
            }

            return item;
        }

        /// <summary>
        /// 获取对联对象
        /// </summary>
        /// <returns></returns>
        public List<CoupletEntity> GetCoupletList() {
            List<CoupletEntity> list = null;
            DataTable table = this.GetCoupletTable();
            if (table != null)
            {
                list = new List<CoupletEntity>();
                foreach (DataRow dr in table.Rows)
                {
                    CoupletEntity item = DataRowToModel(dr);
                    list.Add(item);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取对联内容对象包括id 上联 下联 横批
        /// </summary>
        /// <returns></returns>
        public List<CoupletGroupEntity> GetCoupletGroupEntityList()
        {
            List <CoupletGroupEntity> list = null;

            DataTable table = this.GetCoupletTable();
            if (table != null)
            {
                list = new List<CoupletGroupEntity>();
                DataTable newTable = table.DefaultView.ToTable(true, "CoupletID", "CoupletTypeID");

                foreach (DataRow dr in newTable.Rows) {
                    if (dr != null) {
                        DataRow[] drs = table.Select("CoupletID = " + dr["CoupletID"]);
                        if (drs != null)
                        {
                            CoupletGroupEntity item = new CoupletGroupEntity();
                            item.CoupletID = dr["CoupletID"].ToInt();
                            foreach (DataRow _dr in drs)
                            {
                                switch (_dr["CoupletContentTypeID"].ToString())
                                {
                                    case "0":
                                        item.UpCouplet = _dr["CoupletContent"].ToString().Replace("\r\n", "");
                                        break;
                                    case "1":
                                        item.DownCouplet = _dr["CoupletContent"].ToString().Replace("\r\n", "");
                                        break;
                                    case "2":
                                        item.HorizontalCouplet = _dr["CoupletContent"].ToString().Replace("\r\n", "");
                                        break;
                                }
                            }
                            item.CoupletTypeID = dr["CoupletTypeID"].ToInt();
                            list.Add(item);
                        }
                    }
                }
            } 

            return list;
        }

        public CoupletGroupEntity GetCoupletGroupEntity(int cid) {
            CoupletGroupEntity item = null;

            DataTable table = this.GetCoupletTable(cid);
            if (table != null)
            {
                item = new CoupletGroupEntity();
                DataTable newTable = table.DefaultView.ToTable(true, "CoupletID", "CoupletTypeID");

                foreach (DataRow dr in newTable.Rows)
                {
                    if (dr != null)
                    {
                        DataRow[] drs = table.Select("CoupletID = " + dr["CoupletID"]);
                        if (drs != null)
                        {
                            item.CoupletID = dr["CoupletID"].ToInt();
                            foreach (DataRow _dr in drs)
                            {
                                switch (_dr["CoupletContentTypeID"].ToString())
                                {
                                    case "0":
                                        item.UpCouplet = _dr["CoupletContent"].ToString().Replace("\r\n", "");
                                        break;
                                    case "1":
                                        item.DownCouplet = _dr["CoupletContent"].ToString().Replace("\r\n", "");
                                        break;
                                    case "2":
                                        item.HorizontalCouplet = _dr["CoupletContent"].ToString().Replace("\r\n", "");
                                        break;
                                }
                            }
                            item.CoupletTypeID = dr["CoupletTypeID"].ToInt();
                        }
                    }
                }
            }

            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="up">上联</param>
        /// <param name="down">下联</param>
        /// <param name="horizontal">横批</param>
        /// <returns></returns>
        public int Add(string up, string down, string horizontal)
        {
            return dao.Add(up, down, horizontal);
        }

        /// <summary>
        /// 得到春联  使用缓存
        /// </summary>
        /// <param name="isReload"></param>
        /// <returns></returns>
        public List<CoupletGroupEntity> GetCoupletGroupEntityListWithCache(bool isReload=false)
        {
            string key = WebKeys.CoupletCacheKey;
            List<CoupletGroupEntity> list = null;
            if (!isReload)
            {
                list = MyCache<List<CoupletGroupEntity>>.Get(key);
            }
            if (list == null)
            {
                list = this.GetCoupletGroupEntityList();
                MyCache<List<CoupletGroupEntity>>.Insert(key, list, 60);
            }

            return list;
        }

        /// <summary>
        /// 得到春联  使用缓存
        /// </summary>
        /// <param name="isReload"></param>
        /// <returns></returns>
        public List<FuImageEntity> GetFuImageEntityListWithCache(bool isReload = false)
        {
            string key = WebKeys.FuimageCacheKey;
            List<FuImageEntity> list = null;
            if (!isReload)
            {
                list = MyCache<List<FuImageEntity>>.Get(key);
            }
            if (list == null)
            {
                list = this.GetFuImageList();
                MyCache<List<FuImageEntity>>.Insert(key, list, 60);
            }

            return list;
        }

        private CoupletEntity DataRowToModel(DataRow row)
        {
            CoupletEntity model = null;
            if (row != null)
            {
                model = new CoupletEntity();
                if (row["CoupletID"] != null && row["CoupletID"].ToString() != "")
                {
                    model.CoupletID = int.Parse(row["CoupletID"].ToString());
                }
                if (row["CoupletContentTypeID"] != null && row["CoupletContentTypeID"].ToString() != "")
                {
                    model.CoupletContentTypeID = int.Parse(row["CoupletContentTypeID"].ToString());
                }
                if (row["CoupletContent"] != null)
                {
                    model.CoupletContent = row["CoupletContent"].ToString();
                }
                if (row["OrderBy"] != null && row["OrderBy"].ToString() != "")
                {
                    model.OrderBy = int.Parse(row["OrderBy"].ToString());
                }
                if (row["StateTypeID"] != null && row["StateTypeID"].ToString() != "")
                {
                    model.StateTypeID = int.Parse(row["StateTypeID"].ToString());
                }
                if (row["CoupletTypeID"] != null && row["CoupletTypeID"].ToString() != "")
                {
                    model.CoupletTypeID = int.Parse(row["CoupletTypeID"].ToString());
                }
            }
            return model;
        }

        public int AddShareLog(ShareLogEntity item)
        {
            return dao.AddShareLog(item);
        }

        /// <summary>
        /// 返回分享总数
        /// </summary>
        /// <returns></returns>
        public int GetShareCount()
        {
            return dao.GetShareCount();
        }
    }
}
