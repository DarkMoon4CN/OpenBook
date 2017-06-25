using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity
{
    public class EventItemReplyEntity
    {
        public int ReplyID { get; set; }
        public int CommentID { get; set; }
        public int UserID { get; set; }
        public bool IsAnonymous { get; set; }
        public int ReplyParentID { get; set; }
        public string ReplyContent { get; set; }
        public DateTime? ReplyTime { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public int CheckTypeID { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public int ViewStateID { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string ThirdPictureUrl { get; set; }
        public string FromNickName { get; set; }
        public string UserPicture { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public string UserPictureDomain { get; set; }
        public int ToUserID { get; set; }
        public string ToNickName { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public bool? ToIsAnonymous { get; set; }
    }
}
