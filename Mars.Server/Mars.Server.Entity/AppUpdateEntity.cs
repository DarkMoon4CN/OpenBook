using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity
{
    public class AppUpdateEntity
    {
        public int AppId { get; set; }
        public int AppType { get; set; }
        public string Version { get; set; }
        public string DownloadUrl { get; set; }
        public bool ForcedUpdate { get; set; }
        public DateTime CreateTime { get; set; }
        public int AppSize { get; set; }    
        public string UpdateProfile { get; set; }

        public bool NeedUpdate { get; set; }
        public int VersionType { get; set; }
    }
    public class searchAppUpdateEntity : EntityBase
    {
        public int selAppType { get; set; }
        public string txtVersion { get; set; }
         public string txtStartTime { get; set; }
         public string txtEndTime { get; set; }
    }
}
