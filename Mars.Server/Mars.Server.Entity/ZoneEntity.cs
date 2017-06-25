using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.Entity
{
    /// <summary>
    /// 省份实体
    /// </summary>
   public class ZoneEntity
    {
       public int ZoneID { get; set; }

       public string ZoneName { get; set; }

       public int ZoneLevel { get; set; }

       public int ZoneParentID { get; set; }

       public List<ZoneEntity> ZoneList { get; set; }

    }
}
