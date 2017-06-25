using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity
{
    public class UserRemindEntity
    {
        [Newtonsoft.Json.JsonIgnore]
        public int RemindID { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public int UserID { get; set; }
        public int RemindTypeID { get; set; }
        public string Data { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public DateTime? UpdateTime { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public UserRemindTypeEntity TypeEntity { get; set; }
    }

    public class UserRemindTypeEntity 
    {
        public int RemindTypeID { get; set; }
        public string RemindTypeName { get; set; }
        public string Remarks { get; set; }
    }
}
