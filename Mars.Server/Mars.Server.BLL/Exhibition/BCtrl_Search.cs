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
    public class BCtrl_Search
    {
        SearchDAO dao = new SearchDAO();

        /// <summary>
        /// 根据展场id 查找属于这次展场的展商和活动的标题信息
        /// </summary>
        /// <param name="ExhibitionID"></param>
        /// <returns></returns>
        public DataTable GetSearchKeyWordTable(int exhibitionID)
        {
            return dao.GetSearchKeyWordTable(exhibitionID);
        }

        /// <summary>
        /// 根据展场id 查找属于这次展场的展商和活动的标题查询关键字的信息
        /// </summary>
        /// <param name="ExhibitionID"></param>
        /// <returns></returns>
        public List<SearchKeyWordEntity> GetSearchKeyWordEntityList(int exhibitionID)
        {
            List<SearchKeyWordEntity> list = null;
            DataTable table = this.GetSearchKeyWordTable(exhibitionID);

            if (table != null)
            {
                list = new List<SearchKeyWordEntity>();
                foreach (DataRow dr in table.Rows)
                {
                    SearchKeyWordEntity item = this.GetSearchKeyWordEntityByDataRow(dr);
                    if (item != null)
                    {
                        list.Add(item);
                    }
                }
            }

            return list;
        }
        /// <summary>
        /// 获得查询关键词使用缓存
        /// </summary>
        /// <param name="exhibitionID"></param>
        /// <returns></returns>
        public List<SearchKeyWordEntity> GetSearchKeyWordEntityListUseMyCache(int exhibitionID)
        {
            string key = string.Format(WebKeys.SearchKeyWordyCacheKey, exhibitionID.ToString());
            List<SearchKeyWordEntity> list = MyCache<List<SearchKeyWordEntity>>.Get(key);
            if (list == null)
            {
                list = this.GetSearchKeyWordEntityList(exhibitionID);
                MyCache<List<SearchKeyWordEntity>>.Insert(key, list, 600);
            }

            return list;
        }

        /// <summary>
        /// 将单个展商或者活动查询关键字实例化
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public SearchKeyWordEntity GetSearchKeyWordEntityByDataRow(DataRow dr)
        {
            SearchKeyWordEntity item = null;

            if (dr != null)
            {
                item = new SearchKeyWordEntity();

                if (dr["SearchName"] != null)
                {
                    item.SearchName = dr["SearchName"].ToString();
                }
                if (dr["SearchPinYin"] != null)
                {
                    item.SearchPinYin = dr["SearchPinYin"].ToString();
                }
                if (dr["SearchType"] != null && dr["SearchType"].ToString() != "")
                {
                    item.SearchType = int.Parse(dr["SearchType"].ToString());
                }
            }

            return item;
        }

    }
}
