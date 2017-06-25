using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity
{
    public class EventItemCommentEntity
    {
        public int CommentID { get; set; }
        public Int64 EventItemID { get; set; }
        public int UserID { get; set; }
        public bool IsAnonymous { get; set; }
        public string CommentContent { get; set; }
        public DateTime? CommentTime { get; set; }
        public int CheckTypeID { get; set; }
        public int ViewStateID { get; set; }
    }
}
