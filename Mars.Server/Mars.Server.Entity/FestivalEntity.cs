using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity
{
    public class FestivalEntity
    {
        public FestivalEntity()
        {
            FestivalList = new List<FestivalEntity>();
        }

        public Guid FestivalID { get; set; }
        public string FestivalName { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string FestivalShortName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        
        public int FestivalType { get; set; }

        public int FestivalWeight { get; set; }
      
        public List<FestivalEntity> FestivalList { get; set; }
    }
    public class searchFestivalEntity : EntityBase
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
