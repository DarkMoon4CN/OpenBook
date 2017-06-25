using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity
{
    public class ReportEntity
    {
        public int ReportID { get; set; }
        public int FromUserID { get; set; }
        public int ReportTypeID { get; set; }
        public string ReportContent { get; set; }
        public int ReportInfoTypeID { get; set; }
        public int ReportInfoID { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
