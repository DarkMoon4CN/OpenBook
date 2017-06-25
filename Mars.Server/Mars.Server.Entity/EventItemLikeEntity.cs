using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity
{
    public class EventItemLikeEntity
    {
        public int ELID { get; set; }
        public Int64 EventItemID { get; set; }
        public int LikeCnt { get; set; }
        public int UserID { get; set; }
        public DateTime? LikeTime { get; set; }
    }

    public class CommentLikeEntity 
    {
        public int ELID { get; set; }
        public Int64 CommentID { get; set; }
        public int LikeCnt { get; set; }
        public int UserID { get; set; }
        public DateTime? LikeTime { get; set; }
    }
}
