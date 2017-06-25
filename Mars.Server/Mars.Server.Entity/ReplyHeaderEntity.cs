using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity
{
    public class ReplyHeaderEntity
    {
        public int CommentID { get; set; }
        public Int64 EventItemID { get; set; }
        public int UserID { get; set; }
        public bool IsAnonymous { get; set; }
        public string CommentContent { get; set; }
        public DateTime? CommentTime { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 第三方头像
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public string ThirdPictureUrl { get; set; }

        /// <summary>
        /// 用户上传头像
        /// </summary>
        public string UserPicture { get; set; }


        /// <summary>
        /// 用户头像图片域名
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public string UserPictureDomain { get; set; }
        

        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 文章地址
        /// </summary>
        public string Url { get; set; }
        

        /// <summary>
        /// 文章封面图
        /// </summary>
        public string EventItemPicture { get; set; }


        /// <summary>
        /// 文章封面图片域名
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public string EventItemPictureDomain { get; set; }

        /// <summary>
        /// 回复总数
        /// </summary>
        public int ReplyCount { get; set; }

        /// <summary>
        ///  评论总赞数
        /// </summary>
        public int ReviewLikeCount { get; set; }

        /// <summary>
        /// 用户是否点赞
        /// </summary>
        public bool IsLike { get; set; }

        private DateTime? _starttime = new DateTime(4000, 1, 1);
        /// <summary>
        /// 
        /// </summary>
        public DateTime? StartTime
        {
            set { _starttime = value; }
            get { return _starttime; }
        }
        private DateTime? _endtime = new DateTime(4000, 1, 1);
        /// <summary>
        /// 
        /// </summary>
        public DateTime? EndTime
        {
            set { _endtime = value; }
            get { return _endtime; }
        }
        
        private string _content;
        /// <summary>
        /// 
        /// </summary>
        public string Content
        {
            set { _content = value; }
            get { return _content; }
        }

        public Guid EventItemGUID { get; set; }
    }
}
