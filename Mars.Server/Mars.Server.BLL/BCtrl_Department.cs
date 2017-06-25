using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mars.Server.Entity;
using Mars.Server.DAO;

namespace Mars.Server.BLL
{
    public class BCtrl_Department
    {
        DepartmentDAO dao = new DepartmentDAO();

        /// <summary>
        /// 查询部门列表
        /// </summary>
        /// <returns></returns>
        public List<DepartmentEntity> QueryDepartments()
        {
            return dao.QueryDepartments();
        }
    }
}
