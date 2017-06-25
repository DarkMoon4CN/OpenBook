using Mars.Server.DAO;
using Mars.Server.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mars.Server.Utils;
namespace Mars.Server.BLL
{
    public class BCtrl_CalendarType
    {
        CalendarTypeDAO ctypeobj = new CalendarTypeDAO();
        public List<CalendarTypeEntity> QueryCalendarTypes(int pageno, int pagesize, int parentid, int ctype,int userid)
        {
            return ctypeobj.QueryCalendarTypes(pageno, pagesize, parentid, ctype,userid);
        }
        public List<CalendarTypeEntity> QueryCalendarTypes(string key,int userid=0)
        {
            return ctypeobj.QueryCalendarTypes(key.ToSafeString(),userid);
        }

        public bool AddFavor(int ctypeid, int userid)
        {
            return ctypeobj.AddFavor(ctypeid, userid);
        }
        public bool RemoveFavor(int ctypeid, int userid)
        {
            return ctypeobj.RemoveFavor(ctypeid, userid);
        }

        public List<CalendarTypeEntity> QueryFavorCalendarTypes(int pageno, int pagesize, int userid)
        {
            return ctypeobj.QueryFavorCalendarTypes(pageno, pagesize, userid);
        }

          /// <summary>
        /// 查询一级分类
        /// </summary>      
        /// <param name="calendarTypeKind"></param>
        /// <returns></returns>
        public List<CalendarTypeEntity> QueryFirstCalendarType(int calendarTypeKind=2)
        {
            return ctypeobj.QueryFirstCalendarType(calendarTypeKind);
        }

          /// <summary>
        /// 查询一级分类下的二级分类
        /// </summary>
        /// <param name="parentCalendarTypeID"></param>
        /// <param name="calendarTypeKind"></param>
        /// <returns></returns>
        public List<CalendarTypeEntity> QuerySecondCalendarType(int parentCalendarTypeID, int calendarTypeKind=2)
        {
            return ctypeobj.QuerySecondCalendarType(parentCalendarTypeID, calendarTypeKind);
        }
    }
}
