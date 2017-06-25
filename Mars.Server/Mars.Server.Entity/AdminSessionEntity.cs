using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.Entity
{
    public class AdminSessionEntity
    {
        private int _Sys_RoleID;

        public int Sys_RoleID
        {
            get { return _Sys_RoleID; }
            set { _Sys_RoleID = value; }
        }
        private string _Sys_RoleName;

        public string Sys_RoleName
        {
            get { return _Sys_RoleName; }
            set { _Sys_RoleName = value; }
        }
        private string _Sys_UserID;

        public string Sys_UserID
        {
            get { return _Sys_UserID; }
            set { _Sys_UserID = value; }
        }

        private string _Sys_DisplayName;

        public string Sys_DisplayName
        {
            get { return _Sys_DisplayName; }
            set { _Sys_DisplayName = value; }
        }

        private string _Sys_LoginName;

        public string Sys_LoginName
        {
            get { return _Sys_LoginName; }
            set { _Sys_LoginName = value; }
        }

        private List<FunctionEntity> _Functions = new List<FunctionEntity>();

        public List<FunctionEntity> Functions
        {
            get { return _Functions; }
            set { _Functions = value; }
        }
    }
}
