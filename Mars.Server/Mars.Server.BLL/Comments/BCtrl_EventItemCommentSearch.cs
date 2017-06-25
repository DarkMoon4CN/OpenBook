using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Mars.Server.Entity.Comments;
using Mars.Server.DAO.Comments;

namespace Mars.Server.BLL.Comments
{
    public class BCtrl_EventItemCommentSearch
    {
        EventItemCommentSearchDAO dao = new EventItemCommentSearchDAO();
        public DataTable QueryData(EventItemCommentSearchEntity entity, out int totalcnt)
        {
            return dao.QueryData(entity, out totalcnt);
        }
    }
}
