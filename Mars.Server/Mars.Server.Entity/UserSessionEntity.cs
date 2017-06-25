using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.Entity
{
    public class UserSessionEntity
    {
        public int UserID { get; set; }
        public string SessionID { get; set; }

        public int ZoneID { get; set; }

        public string NickName { get; set; }

        public string Telphone { get; set; }

        public string ThirdWxUserName { get; set; }
        public string ThirdWbUserName { get; set; }
        public string ThirdQqUserNameW { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string ThirdPictureUrl { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string PicturePath { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public string Domain { get; set; }

        public string UserPictureUrl { get {
            if (!string.IsNullOrEmpty(PicturePath))
            {
                return string.Format("{0}{1}", Domain, PicturePath);
            }
            else
                return ThirdPictureUrl;
        } }
    }
}
