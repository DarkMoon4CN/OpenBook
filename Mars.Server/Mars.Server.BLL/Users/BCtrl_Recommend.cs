using Mars.Server.DAO.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.BLL.Users
{
    public class BCtrl_Recommend
    {
        RecommendDAO dao = new RecommendDAO();

        public int WriteNumber(int recommendId, string phoneNum)
        {
            return dao.WriteNumber(recommendId, phoneNum);
        }
    }
}
