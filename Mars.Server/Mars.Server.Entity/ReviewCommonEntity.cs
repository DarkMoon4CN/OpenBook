using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity
{
    public class ReviewCommonEntity
    {
        public int  ReviewCount { get; set; }
        public int  ArticleLikeCount { get; set; }
        public bool IsArticleLike { get; set; }
    }

    public class ReplyCommonEntity 
    {
        public Int64 ReplyCount { get; set; }
        public Int64 ReviewLikeCount { get; set; }
        public bool IsReviewLike { get; set; }
    }

    public class AddReplyCommonEntity : ReplyCommonEntity
    {
        public int  ReplyID { get; set; }
        public DateTime? ReplyTime { get; set; } 
    }

}
