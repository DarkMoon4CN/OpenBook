using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.Entity
{
    public class ReminderEntity
    {
          [Newtonsoft.Json.JsonIgnore]
        public int ReminderID { get; set; }
          [Newtonsoft.Json.JsonIgnore]
        public int UserID { get; set; }
        public Guid EventItemGUID { get; set; }
        public int ReminderPreTime { get; set; }
        public int ReminderState { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public int DeletedFlag { get; set; }
        public Guid ReminderGUID { get; set; }
    }
}
