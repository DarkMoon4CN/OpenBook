using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.Entity
{
    public class SysRoleEntity
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Role_ID { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Role_Name { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateTime { get; set; }
    }

    public class SysRoleSearchEntity : EntityBase
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Role_ID { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Role_Name { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateTime { get; set; }
    }


    public class SysUserRoleRelEntity
    {
        public int User_ID { get; set; }

        public int User_RoleID { get; set; }

        public string User_ShowName { get; set; }
    }

    public class SysRoleFunctionRelEntity
    {
        public int RFRel_FunctionID { get; set; }

        public int RFRel_RoleID { get; set; }
    }


    public class SysUserFunctionRelEntity
    {
        public int UFRel_FunctionID { get; set; }

        public int UFRel_UserID { get; set; }
    }
}
