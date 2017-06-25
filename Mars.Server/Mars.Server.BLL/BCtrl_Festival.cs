using Mars.Server.Entity;
using Mars.Server.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mars.Server.Utils;
using System.Data;

namespace Mars.Server.BLL
{
    public class BCtrl_Festival
    {
        FestivalDAO fesobj = new FestivalDAO();
        public List<FestivalEntity> QueryAllFestivalsTillNow(DateTime till_date)
        {
            var data=fesobj.QueryAllFestivalsTillNow(till_date);
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].FestivalType == 3 && string.IsNullOrEmpty(data[i].FestivalShortName)  && data[i].FestivalShortName!="") 
                {
                    data[i].FestivalName = data[i].FestivalShortName;
                }    
            }
            return data; 
        }

        public string QueryAllFestivalsHash(DateTime till_date)
        {
            List<FestivalEntity> data = fesobj.QueryAllFestivalsTillNow(till_date);
            if (data != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in data)
                {
                    sb.AppendFormat("{0}_{1}", item.FestivalID.ToString("N").ToUpper(), item.FestivalWeight);
                }
                return MD5.Fun_MD5(sb.ToString());
            }
            else
                return string.Empty;
        }
        public string InsertFestival(FestivalEntity entity)
        {
            return fesobj.InsertFestival(entity);
        }
        public DataTable GetFestivalList(searchFestivalEntity entity, out int totalcnt)
        {
            return fesobj.GetFestivalList(entity, out totalcnt);
        }
        public string deleteFestival(FestivalEntity entity)
        {
            return fesobj.deleteFestival(entity);
        }
        public DataTable GetFestival(FestivalEntity entity)
        {
            return fesobj.GetFestival(entity);
        }
        public string UpdateFestival(FestivalEntity entity)
        {
            return fesobj.UpdateFestival(entity);
        }

        /// <summary>
        /// 1班   2休   3行业日期
        /// </summary>
        /// <param name="fesivalType"></param>
        /// <returns></returns>
        public List<FestivalEntity> GetFestivalList(string fesivalName,int fesivalType = 3)
        {
            return fesobj.GetFestivalList(fesivalName,fesivalType);
        }
    }
}
