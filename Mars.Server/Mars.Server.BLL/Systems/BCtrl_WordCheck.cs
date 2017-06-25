using Mars.Server.DAO.Systems;
using Mars.Server.Entity.Comments;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Mars.Server.BLL.Systems
{
    public class BCtrl_WordCheck
    {
        public DataTable GetWordsTable() {
            WordCheckDAO dao = new WordCheckDAO();

            return dao.GetWordsTable();
        }

        public List<SensitiveWordEntity> GetWordsList() {
            List<SensitiveWordEntity> list = null;

            DataTable table = this.GetWordsTable();
            if (table != null)
            {
                list = new List<SensitiveWordEntity>();
                foreach (DataRow dr in table.Rows)
                {
                    SensitiveWordEntity item = this.GetEntityByDataRow(dr);
                    if (item != null)
                    {
                        list.Add(item);
                    }
                }
            }

            return list;
        }

        public Dictionary<int, string> GetNewWordStringList()
        {
            Dictionary<int, string> dict = null;

            DataTable table = this.GetNewWordStringTable();
            if (table != null)
            {
                dict = new Dictionary<int, string>();
                foreach (DataRow dr in table.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["SensitiveWords"].ToString()))
                    {
                        dict.Add(int.Parse(dr["SWID"].ToString()),dr["SensitiveWords"].ToString());
                    }
                }
            }

            return dict;
        }

        public DataTable GetNewWordStringTable()
        {
            WordCheckDAO dao = new WordCheckDAO();

            return dao.GetNewWordStringTable();
        }

        public List<string> GetWordStringList()
        {
            List<string> list = null;

            DataTable table = this.GetWordsTable();
            if (table != null)
            {
                list = new List<string>();
                foreach (DataRow dr in table.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["SensitiveWords"].ToString()))
                    {
                        list.Add(dr["SensitiveWords"].ToString());
                    }
                }
            }

            return list;
        }

        private SensitiveWordEntity GetEntityByDataRow(DataRow dr)
        {
            SensitiveWordEntity item = null;

            if (dr != null)
            {
                item = new SensitiveWordEntity();
                if (dr["SWID"] != null && dr["SWID"].ToString() != "")
                {
                    item.SWID = int.Parse(dr["SWID"].ToString());
                }
                if (dr["SensitiveWords"] != null)
                {
                    item.SensitiveWords = dr["SensitiveWords"].ToString();
                }
                if (dr["StateTypeID"] != null && dr["StateTypeID"].ToString() != "")
                {
                    item.StateTypeID = int.Parse(dr["StateTypeID"].ToString());
                }
                if (dr["IsNeedRecheck"] != null && dr["IsNeedRecheck"].ToString() != "")
                {
                    if ((dr["IsNeedRecheck"].ToString() == "1") || (dr["IsNeedRecheck"].ToString().ToLower() == "true"))
                    {
                        item.IsNeedRecheck = true;
                    }
                    else
                    {
                        item.IsNeedRecheck = false;
                    }
                }
                if (dr["CreateUserID"] != null && dr["CreateUserID"].ToString() != "")
                {
                    item.CreateUserID = int.Parse(dr["CreateUserID"].ToString());
                }
                if (dr["CreateTime"] != null && dr["CreateTime"].ToString() != "")
                {
                    item.CreateTime = DateTime.Parse(dr["CreateTime"].ToString());
                }
            }

            return item;
        }
    }
}
