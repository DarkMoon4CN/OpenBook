using Mars.Server.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mars.Server.DAO;

namespace Mars.Server.BLL
{
    public class BCtrl_Zone
    {
        ZoneDAO dao = new ZoneDAO();
        public List<ZoneEntity> QueryZoneProvince()
        {
            return dao.QueryZoneProvince();
        }


        /// <summary>
        /// 查询按地区分组省
        /// </summary>
        /// <returns></returns>
        public List<ZoneEntity> QueryZoneToGroup()
        {
            return dao.QueryZoneToGroup();
        }

        /// <summary>
        /// 转换为具体选中区域
        /// </summary>
        /// <param name="zoneID"></param>
        /// <returns></returns>
        public string ConvertZoneIDs(int zoneID)
        {
            List<int> zoneIDList = new List<int>();

            List<ZoneEntity> list = dao.QueryZoneProvince();           
            foreach (ZoneEntity entity in list)
            {
                if ((zoneID & entity.ZoneID) > 0)
                {
                    zoneIDList.Add(entity.ZoneID);
                }
            }

            if (zoneIDList.Count == 0)
            {
                zoneIDList.Add(0);
            }

            return string.Join(",", zoneIDList);
        }
    }
}
