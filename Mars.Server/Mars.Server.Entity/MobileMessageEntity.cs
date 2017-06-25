using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity
{
    public class MobileMessageEntity
    {
        public int MessageID { get; set; }
        public int MessageType { get; set; }
        public string Contents { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public bool IsRead { get;set;}
        public int ToUserID { get; set; }
        public int FromUserID { get; set; }
        public string Tag { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public class RelateEntity 
    {
        public int MessageID { get; set; }
        public int MessageType { get; set; }
        public string Contents { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public string Url { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public string Title { get; set; }
        public bool IsRead { get; set; }
        public int ToUserID { get; set; }
        public int FromUserID { get; set; }
        public string Tag { get; set; }
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 回复者的昵称
        /// </summary>
        public string FromUserNickName { get; set; }
        /// <summary>
        /// 回复者的第三方头像
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public string FromUserThirdPictureUrl { get; set; }

        /// <summary>
        /// 回复者的本地头像域名
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public string FromUserPictureDomain { get; set; }

        /// <summary>
        /// 回复者本地头像
        /// </summary>
        public string FromUserPicture { get; set; }

        /// <summary>
        /// 回复者是否匿名
        /// </summary>
        public bool FromUserIsAnonymous { get; set; }

        /// <summary>
        /// 文章的ID
        /// </summary>
        public Int64 EventItemID { get; set; }

        /// <summary>
        /// 文章标题
        /// </summary>
        public string EventItemTitle { get; set; }

        /// <summary>
        /// 文章的域名
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public string EventItemPictureDomain { get; set; }

        /// <summary>
        /// 文章的图片
        /// </summary>
        public string EventItemPicture { get; set; }


        private DateTime? _starttime = new DateTime(4000, 1, 1);
        /// <summary>
        /// 文章起始时间
        /// </summary>
        public DateTime? EventItemStartTime
        {
            set { _starttime = value; }
            get { return _starttime; }
        }
        private DateTime? _endtime = new DateTime(4000, 1, 1);
        /// <summary>
        /// 文章结束时间
        /// </summary>
        public DateTime? EventItemEndTime
        {
            set { _endtime = value; }
            get { return _endtime; }
        }

        /// <summary>
        /// 文章内容
        /// </summary>
        public string EventItemContent { get; set; }

        /// <summary>
        /// 文章地址
        /// </summary>
        public string EventItemUrl { get; set; }

        /// <summary>
        /// 评论ID MessageType =2 时有效
        /// </summary>
        public int ReviewsId { get; set; }

        /// <summary>
        ///  文章的GUID
        /// </summary>
        public Guid EventItemGUID { get; set; }

    }
}
