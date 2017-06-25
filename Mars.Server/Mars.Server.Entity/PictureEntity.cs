using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.Entity
{
    public class PictureEntity
    {
        public int PictureID { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public string PicturePath { get; set; }
         [Newtonsoft.Json.JsonIgnore]
        public string Domain { get; set; }
         [Newtonsoft.Json.JsonIgnore]
        public string PictureServerID { get; set; }

        [Newtonsoft.Json.JsonIgnore]
         public int PictureState { get; set; }

        public string PicUrl
        {
            get
            {
                return string.IsNullOrEmpty(Domain) ? string.Empty : string.Concat(Domain, PicturePath);
            }
        }
    }
}
