using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity.Comments
{
    public class CommentSearchEntity : EntityBase
    {
        /// <summary>
        /// 通过文章标题来搜索
        /// </summary>
        public string Title { get; set; }
        public string CheckType { get; set; }
        public string ViewType { get; set; }
    }
}
