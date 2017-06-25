using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mars.Server.DAO;

namespace Mars.Server.BLL
{
    public class BCtrl_EventItemBrowseCnt
    {
        EventItemBrowseCntDao dao = new EventItemBrowseCntDao();

        /// <summary>
        /// 更新浏览次数
        /// </summary>
        /// <param name="eventItemGuid"></param>
        /// <returns></returns>
        public bool UpdateBrowserCnt(Guid eventItemGuid)
        {
            return dao.UpdateBrowserCnt(eventItemGuid);
        }

        /// <summary>
        /// 获取文章浏览次数
        /// </summary>
        /// <param name="eventItemGuid"></param>
        /// <returns></returns>
        public int GetBrowserCnt(Guid eventItemGuid)
        {
            return dao.GetBrowserCnt(eventItemGuid);
        }
    }
}
