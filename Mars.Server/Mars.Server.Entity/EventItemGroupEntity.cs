using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity
{
    public class EventItemGroupEntity
    {
        public int GroupEventID { get; set; }
        public string GroupEventName { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public DateTime CreatedTime { get; set; }
         [Newtonsoft.Json.JsonIgnore]
        public DateTime PublishTime { get; set; }

        /// <summary>
         /// 专题组状态 0单篇成组 1多篇成组
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
         public int GroupState { get; set; }

         public List<EventItemEntity> EventItems { get; set; } 
    }

    public class EventItemGroupRelEntity
    {
        public int EventGroupID { get; set; }

        public int EventItemID { get; set; }

        public int DisplayOrder { get; set; }
    }

    public class EventItemGroupSearchEntity:EntityBase
    {
        public int GroupEventID { get; set; }
        public string GroupEventName { get; set; }

        public DateTime? PublishStartTime { get; set; }

        public DateTime? PublishEndTime { get; set; }

        public string EventItemIDs { get; set; }

        /// <summary>
        /// 0单篇成组  1手工创建专题分组
        /// </summary>
        public int GroupState { get; set; }
    }

    public class EventItemGroupRelViewEntity
    {
        public int EventGroupID { get; set; }

        public int EventItemID { get; set; }

        public int DisplayOrder { get; set; }

        public string GroupEventName { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime PublishTime { get; set; }

        public Guid EventItemGUID { get; set; }

        public string Title { get; set; }

        public string FirstTypName { get; set; }

        public string SecondTypeName { get; set; }

        public string Url { get; set; }

        public DateTime EventItemPublishTime { get; set; }

        /// <summary>
        /// 0单篇成组  1手工创建专题分组
        /// </summary>
        public int GroupState { get; set; }
         
    }
}
