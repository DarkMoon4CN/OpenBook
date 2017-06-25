using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity.Comments
{
    public class EventItemCommentSearchEntity : EntityBase
    {
        /// <summary>
        /// 是否是分享
        /// </summary>
        public int IsShare { get; set; }

        /// <summary>
        /// 评论文章id
        /// </summary>
        public int EventItemID { get; set; }

        /// <summary>
        /// 文章GUID
        /// </summary>
        public string EventItemGUID { get; set; }

        /// <summary>
        /// 登录用户ID
        /// </summary>
        public int UserID { get; set; }
    }
}
