using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Mars.Server.Entity
{
    public class SubscriptionEntity
    {
        public int SubID { get; set; }
        public string SubName { get; set; }
        public string PinYin { get; set; }
        public string Description { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public int PictureID { get; set; }
        public string SubShortName { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public int ViewStateID { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public string PicturePath { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public string Domain { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public string _subPicturePath;

        public string SubPicturePath
        {
            get
            {
                return string.IsNullOrEmpty(Domain) ? string.Empty : string.Concat(Domain, PicturePath);
            }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 订阅时间
        /// </summary>
        public DateTime? SubTime { get; set; }

        /// <summary>
        /// 是否已订阅 1已订阅  0未订阅
        /// </summary>
        public bool IsAlready { get; set; }
    }

    public class SubscriptionEventItemEntity
    {
        public int SubID { get; set; }
        public Guid EventItemGUID { get; set; }
    }

    public class UserSubscriptionEntity 
    {
        public int UserID { get; set; }
        public int SubID { get; set; }
    }
}

