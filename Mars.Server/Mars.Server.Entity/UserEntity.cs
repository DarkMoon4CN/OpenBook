using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.Entity
{
    public class UserEntity
    {
        public int UserID { get; set; }
        public string EMail { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public string Address { get; set; }
        public int AreaID { get; set; }
        public string NickName { get; set; }
    }
}
